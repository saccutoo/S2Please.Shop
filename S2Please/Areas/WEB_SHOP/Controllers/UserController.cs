using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S2Please.Database;
using S2Please.Models;
using S2Please.Areas.WEB_SHOP.ViewModel;
using Newtonsoft.Json;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using SHOP.COMMON.Helpers;
using S2Please.Helper;
using S2Please.ParramType;
using SHOP.COMMON;

namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class UserController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/User
        private ado _db = new ado();
        private string _connection = ConfigurationManager.AppSettings["DBConnection"];

        //User
        public ActionResult Customer()
        {
            if (Request.Cookies[Constant.ShopOnline] == null)
            {
                return RedirectToAction("Login", "Authen", new { Area = "WEB_SHOP" });
            }
            CustomerViewModel vm = new CustomerViewModel();
            var param = new List<Param>();
            var responseGender = ListProcedure<GenderModel>(new GenderModel(), "Gender_Get_Gender", param,true);
            vm.Genders= responseGender.Results;
            if (CurrentUser.User.USER_ID != 0)
            {
                param.Add(new Param { Key = "@USER_ID", Value = CurrentUser.User.USER_ID.ToString() });
                var responseCustomer = ListProcedure<CustomerModel>(new CustomerModel(), "Customer_Get_GetCustomerById", param);
                var resultCustomer = JsonConvert.DeserializeObject<List<CustomerModel>>(JsonConvert.SerializeObject(responseCustomer.Results));
                if (resultCustomer != null && resultCustomer.Count() > 0)
                {
                    vm.Customer = resultCustomer.FirstOrDefault();
                }
            }

            var responseCity = ListProcedure<CityModel>(new CityModel(), "City_Get_City", new List<Param>(), true, false);
            vm.Citys = responseCity.Results;

            if (vm.Customer.CITY != 0)
            {

                param = new List<Param>();
                param.Add(new Param { Key = "@CODE_CITY", Value = vm.Customer.CITY.ToString() });
                var responseCommunity = ListProcedure<CommunityModel>(new CommunityModel(), "District_Get_DistrictByCodeCity", param, true, false);
                vm.Districts = responseCommunity.Results;

            }
            if (vm.Customer.DISTRICT != 0)
            {
                param = new List<Param>();
                param.Add(new Param { Key = "@CODE_DISTRICT", Value = vm.Customer.DISTRICT.ToString() });
                var response = ListProcedure<DistrictModel>(new DistrictModel(), "Community_Get_CommunityByCodeDistrict", param);
                if (response != null && response.Results.Count() > 0)
                {
                    vm.Communitys = response.Results;

                }
            }

            param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = CurrentUser.User.USER_ID.ToString() });
            var responseOrder = ListProcedure<OrderModel>(new OrderModel(), "Order_Get_GetOrderByCustomerId", param);
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder!=null && resultOrder.Count()>0)
            {
                vm.Orders = resultOrder;
            }

            param = new List<Param>();
            var responseStatusOrder = ListProcedure<StatusOrderModel>(new StatusOrderModel(), "StatusOrder_Get_StatusOrder", param,true);
            var resultStatusOrder = JsonConvert.DeserializeObject<List<StatusOrderModel>>(JsonConvert.SerializeObject(responseStatusOrder.Results));
            if (resultStatusOrder != null && resultStatusOrder.Count() > 0)
            {
                vm.StatusOrders = resultStatusOrder;
            }

            param = new List<Param>();
            var responseStatusPay = ListProcedure<StatusPayModel>(new StatusPayModel(), "StatusPay_Get_StatusPay", param, true);
            var resultStatusPay = JsonConvert.DeserializeObject<List<StatusPayModel>>(JsonConvert.SerializeObject(responseStatusPay.Results));
            if (resultStatusPay != null && resultStatusPay.Count() > 0)
            {
                vm.StatusPays = resultStatusPay;
            }
            return View(vm);
        }

        public ActionResult UpdateCustomer(CustomerModel model)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@FULL_NAME", Value = model.FULL_NAME });
            param.Add(new Param { Key = "@PHONE", Value = model.PHONE });
            param.Add(new Param { Key = "@EMAIL", Value = model.EMAIL });
            param.Add(new Param { Key = "@GENDER", Value = model.GENDER.ToString() });
            param.Add(new Param { Key = "@DATE_OF_BIRTH", Value = model.DATE_OF_BIRTH!=null? model.DATE_OF_BIRTH.Value.ToString("yyyy/MM/dd"):"" });
            param.Add(new Param { Key = "@FAX", Value = model.FAX });
            param.Add(new Param { Key = "@ADRESS_SPECIFIC", Value = model.ADRESS_SPECIFIC });
            param.Add(new Param { Key = "@CITY", Value = model.CITY.ToString() });
            param.Add(new Param { Key = "@DISTRICT", Value = model.DISTRICT.ToString() });
            param.Add(new Param { Key = "@COMMUNITY", Value = model.COMMUNITY.ToString() });
            param.Add(new Param { Key = "@IMAGE", Value = model.COMMUNITY.ToString() });

            var data = new OrderModel();
            var response = ListProcedure<CustomerModel>(new CustomerModel(), "Customer_Update_Customer", param);
            var result = new ResultModel();
            if (response != null && response.Success == true)
            {
                result.Success = true;
                result.Message = FunctionHelpers.GetValueLanguage("Message.UpdateSuccess");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
                result.Results = response.Results;
                data = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(result.Results)).FirstOrDefault();
                data.ToDecrypt = FunctionHelpers.Encrypt(data.ID.ToString(), FunctionHelpers.KeyEncrypt);
            }
            else
            {
                result.Success = false;
                result.Message = FunctionHelpers.GetValueLanguage("Message.UpdateFail");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
            }

            return Json(new { result = result, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewOrderDetail(long id)
        {
            OrderDetailViewModel vm = new OrderDetailViewModel();
            var param = new List<Param>();
            param.Add(new Param { Key = "@ORDER_ID", Value = id.ToString() });
            var response = ListProcedure<OrderDetailModel>(new OrderDetailModel(), "OrderDetail_Get_GetOrderDetailByOrderId", param);
            var result = JsonConvert.DeserializeObject<List<OrderDetailModel>>(JsonConvert.SerializeObject(response.Results));
            if (result!=null && result.Count()>0)
            {
                vm.OrderDetails = result;
            }
            param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            var responseOrder = ListProcedure<OrderModel>(new OrderModel(), "Order_Get_GetOrderByID", param);
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder != null && resultOrder.Count() > 0)
            {
                vm.Order = resultOrder.FirstOrDefault() ;
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/User/_OrderDetail.cshtml", vm);
            return Json(new { html }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveOrder(long id)
        {
            ResultModel resultModel = new ResultModel();
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            var response = ListProcedure<OrderModel>(new OrderModel(), "Order_Delete_DeleteById", param);
            var result = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(response.Results));
            if (result != null && result.Count() > 0)
            {
                resultModel.Success = false;
                resultModel.Message = FunctionHelpers.GetValueLanguage("Message.Remove.Error");
                resultModel.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
            }
            else
            {
                resultModel.Success = true;
                resultModel.Message = FunctionHelpers.GetValueLanguage("Delete sucess");
                resultModel.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");

            }
            return Json(new { result= resultModel }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult FormUpdateOrder(string toDecrypt)
        {
            UpdateOrderViewModel vm = new UpdateOrderViewModel();
            var id = FunctionHelpers.ConvertKey(toDecrypt, FunctionHelpers.KeyEncrypt);
            var param = new List<Param>();
            param.Add(new Param { Key = "@ORDER_ID", Value = id.ToString() });
            var response = ListProcedure<OrderDetailModel>(new OrderDetailModel(), "OrderDetail_Get_GetOrderDetailByOrderId", param);
            var result = JsonConvert.DeserializeObject<List<OrderDetailModel>>(JsonConvert.SerializeObject(response.Results));
            if (result != null && result.Count() > 0)
            {
                vm.OrderDetails = result;
            }
            param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            var responseOrder = ListProcedure<OrderModel>(new OrderModel(), "Order_Get_GetOrderByID", param);
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder != null && resultOrder.Count() > 0)
            {
                vm.Order = resultOrder.FirstOrDefault();
            }


            param = new List<Param>();
            var responseCity = ListProcedure<CommunityModel>(new CommunityModel(), "City_Get_City", param, true, false);
            vm.Citys = responseCity.Results;

            param = new List<Param>();
            param.Add(new Param { Key = "@CODE_CITY", Value = vm.Order.CITY.ToString() });
            var responseDistrict = ListProcedure<DistrictModel>(new DistrictModel(), "District_Get_DistrictByCodeCity", param);
            if (responseDistrict != null && responseDistrict.Results.Count() > 0)
            {
                vm.Districts = responseDistrict.Results;

            }

             param = new List<Param>();
            param.Add(new Param { Key = "@CODE_DISTRICT", Value = vm.Order.DISTRICT.ToString() });
            var responseCommunity = ListProcedure<DistrictModel>(new DistrictModel(), "Community_Get_CommunityByCodeDistrict", param);
            if (responseCommunity != null && responseCommunity.Results.Count() > 0)
            {
                vm.Communitys = responseCommunity.Results;

            }

            return View(vm);

        }

        public ActionResult UpdateOrder(OrderModel model,List<OrderDetailModel> orderDetail)
        {
            var param = new List<Param>();
            var validations = new List<ValidationModel>();
            if (model != null)
            {
                validations = ValidationHelper.Validation(model, "model");              
            }
            if (orderDetail.Count()>0)
            {
                int index = 0;
                foreach (var item in orderDetail)
                {
                    if (item.AMOUNT<=0)
                    {
                        validations.Add(new ValidationModel()
                        {
                            IsError = true,
                            ColumnName = "orderDetail[" + index + "].AMOUNT",
                            ErrorMessage = FunctionHelpers.GetValueLanguage("Message.Error.Amount")
                        });
                    }
                    else
                    {
                        param = new List<Param>();
                        param.Add(new Param { Key = "@COLOR_ID", Value = item.COLOR_ID.ToString() });
                        param.Add(new Param { Key = "@SIZE_ID", Value = item.SIZE_ID.ToString() });
                        param.Add(new Param { Key = "@PRODUCT_ID", Value = item.PRODUCT_ID.ToString() });
                        param.Add(new Param { Key = "@IS_MAIN", Value = "0" });

                        var responseMapper = ListProcedure<ProductColorSizeMapperModel>(new ProductColorSizeMapperModel(), "Product_Get_GetProductColorSizeMapperByColorIdAndSizeId", param);
                        var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
                        if (resultMapper != null && resultMapper.Count() > 0)
                        {
                            var data = resultMapper.FirstOrDefault();
                            if (data.AMOUNT < item.AMOUNT)
                            {                              
                                validations.Add(new ValidationModel()
                                {
                                    IsError = true,
                                    ColumnName = "orderDetail[" + index + "].AMOUNT",
                                    ErrorMessage = String.Format(FunctionHelpers.GetValueLanguage("Message.Error.Amount1"), data.AMOUNT)
                                });
                            }
                        }
                    }
                    index++;
                }
            }
            if (validations.Count > 0)
            {
                return Json(new { Result = validations, Invalid = true }, JsonRequestBehavior.AllowGet);
            }

            param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@ORDER_CODE", Value = new Random().Next(1000000, 9999999).ToString() });
            param.Add(new Param { Key = "@CUSTOMER_ID", Value = model.CUSTOMER_ID.ToString() });
            param.Add(new Param { Key = "@STATUS", Value = model.STATUS.ToString() });
            param.Add(new Param { Key = "@STATUS_PAY", Value = model.STATUS_PAY.ToString() });
            param.Add(new Param { Key = "@METHOD_PAY", Value = model.METHOD_PAY.ToString() });
            param.Add(new Param { Key = "@FEE_SHIP", Value = model.FEE_SHIP.ToString() });
            param.Add(new Param { Key = "@BONUS_ID", Value = model.BONUS_ID });
            param.Add(new Param { Key = "@DECRIPTION", Value = model.DECRIPTION });

            param.Add(new Param { Key = "@FULL_NAME", Value = model.FULL_NAME });
            param.Add(new Param { Key = "@PHONE", Value = model.PHONE });
            param.Add(new Param { Key = "@EMAIL", Value = model.EMAIL });
            param.Add(new Param { Key = "@FAX", Value = model.FAX });
            param.Add(new Param { Key = "@ADRESS_SPECIFIC", Value = model.ADRESS_SPECIFIC });
            param.Add(new Param { Key = "@CITY", Value = model.CITY.ToString() });
            param.Add(new Param { Key = "@DISTRICT", Value = model.DISTRICT.ToString() });
            param.Add(new Param { Key = "@COMMUNITY", Value = model.COMMUNITY.ToString() });

            var type= MapperHelper.MapList<OrderDetailModel, OrderDetailType >(orderDetail);


            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@OrderDetailType", SqlDbType.Structured)
                {
                    TypeName = "dbo.OrderDetailType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(type)
                }
            });


            var responseOrder = ListProcedure<OrderModel>(new OrderModel(), "Order_Update_Order", param);
            var result = new ResultModel();
            if (responseOrder != null && responseOrder.Success == true)
            {
                result.Success = true;
                result.Message = FunctionHelpers.GetValueLanguage("Message.Order.UpdateSuccess");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
                result.Results = responseOrder.Results;
               
            }
            else
            {
                result.Success = false;
                result.Message = FunctionHelpers.GetValueLanguage("Message.UpdateFail");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
    }
}