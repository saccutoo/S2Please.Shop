using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Models;
using S2Please.Areas.WEB_SHOP.ViewModel;
using Newtonsoft.Json;
using SHOP.COMMON.Helpers;
using S2Please.Helper;
using SHOP.COMMON;
using Repository;

namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class UserController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/User
        private ISystemRepository _systemRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private ICustomerRepository _customerRepository;

        public UserController(ISystemRepository systemRepository, IProductRepository productRepository, IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            this._systemRepository = systemRepository;
            this._productRepository = productRepository;
            this._orderRepository = orderRepository;
            this._customerRepository = customerRepository;
        }

        //User
        public ActionResult Customer()
        {
            if (Request.Cookies[Constant.ShopOnline] == null)
            {
                return RedirectToAction("Login", "Authen", new { Area = "WEB_SHOP" });
            }
            CustomerViewModel vm = new CustomerViewModel();
            var responseGender = _systemRepository.GetGender();
            vm.Genders= responseGender.Results;
            if (CurrentUser.User.USER_ID != 0)
            {
                var responseCustomer = _customerRepository.GetCustomerById(CurrentUser.User.USER_ID);
                var resultCustomer = JsonConvert.DeserializeObject<List<CustomerModel>>(JsonConvert.SerializeObject(responseCustomer.Results));
                if (resultCustomer != null && resultCustomer.Count() > 0)
                {
                    vm.Customer = resultCustomer.FirstOrDefault();
                }
            }

            var responseCity = _systemRepository.GetCity();
            vm.Citys = responseCity.Results;

            if (vm.Customer.CITY != 0)
            {
                var responseCommunity = _systemRepository.GetDistrictByCodeCity(vm.Customer.CITY);
                vm.Districts = responseCommunity.Results;
            }
            if (vm.Customer.DISTRICT != 0)
            {
                var response = _systemRepository.GetCommunityByCodeDistrict(vm.Customer.DISTRICT);
                if (response != null && response.Results.Count() > 0)
                {
                    vm.Communitys = response.Results;
                }
            }

            var responseOrder = _orderRepository.GetOrderByCustomerId(CurrentUser.User.USER_ID);
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder!=null && resultOrder.Count()>0)
            {
                vm.Orders = resultOrder;
            }

            var responseStatusOrder = _systemRepository.GetStatusOrder();
            var resultStatusOrder = JsonConvert.DeserializeObject<List<StatusOrderModel>>(JsonConvert.SerializeObject(responseStatusOrder.Results));
            if (resultStatusOrder != null && resultStatusOrder.Count() > 0)
            {
                vm.StatusOrders = resultStatusOrder;
            }

            var responseStatusPay = _systemRepository.GetStatusPay();
            var resultStatusPay = JsonConvert.DeserializeObject<List<StatusPayModel>>(JsonConvert.SerializeObject(responseStatusPay.Results));
            if (resultStatusPay != null && resultStatusPay.Count() > 0)
            {
                vm.StatusPays = resultStatusPay;
            }
            return View(vm);
        }

        public ActionResult UpdateCustomer(CustomerModel model)
        {
            var customerInser = MapperHelper.Map<CustomerModel,Repository.Model.CustomerModel>(model);
            var data = new OrderModel();
            var response = _customerRepository.UpdateCustomer(customerInser);
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
            var response = _orderRepository.GetOrderDetailByOrderId(id);
            var result = JsonConvert.DeserializeObject<List<OrderDetailModel>>(JsonConvert.SerializeObject(response.Results));
            if (result!=null && result.Count()>0)
            {
                vm.OrderDetails = result;
            }

            var responseOrder = _orderRepository.GetOrderById(id);
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
            var response = _orderRepository.DeleteById(id);
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
            var response = _orderRepository.GetOrderDetailByOrderId(long.Parse(id));
            var result = JsonConvert.DeserializeObject<List<OrderDetailModel>>(JsonConvert.SerializeObject(response.Results));
            if (result != null && result.Count() > 0)
            {
                vm.OrderDetails = result;
            }

            var responseOrder = _orderRepository.GetOrderById(long.Parse(id));
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder != null && resultOrder.Count() > 0)
            {
                vm.Order = resultOrder.FirstOrDefault();
            }

            var responseCity = _systemRepository.GetCity();
            vm.Citys = responseCity.Results;

            var responseDistrict = _systemRepository.GetDistrictByCodeCity(vm.Order.CITY);
            if (responseDistrict != null && responseDistrict.Results.Count() > 0)
            {
                vm.Districts = responseDistrict.Results;

            }

            var responseCommunity = _systemRepository.GetCommunityByCodeDistrict(vm.Order.DISTRICT);
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
                        var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(item.COLOR_ID, item.SIZE_ID, item.PRODUCT_ID,"0");
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

            var odelInser = MapperHelper.Map<OrderModel,Repository.Model.OrderModel>(model);
            var type = MapperHelper.MapList<OrderDetailModel, Repository.Type.OrderDetailType>(orderDetail);

            var responseOrder = _orderRepository.UpdateOrder(odelInser, type);
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