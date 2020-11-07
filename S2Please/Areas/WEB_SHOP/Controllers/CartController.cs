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
using S2Please.ParramType;
using S2Please.Helper;
using S2Please.Areas.WEB_SHOP.Models;
using SHOP.COMMON;
using S2Please.ViewModel;

namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class CartController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/Home
        private ado _db = new ado();
        private string _connection = ConfigurationManager.AppSettings["DBConnection"];

        public ActionResult AddCart(CartModel cart)
        {
            var result = new ResultModel();
            var listCart = new List<CartModel>();
            ProductColorSizeMapperModel data = new ProductColorSizeMapperModel();
            var message = string.Empty;
            var param = new List<Param>();
            param.Add(new Param { Key = "@COLOR_ID", Value = cart.COLOR_ID.ToString() });
            param.Add(new Param { Key = "@SIZE_ID", Value = cart.SIZE_ID.ToString() });
            param.Add(new Param { Key = "@PRODUCT_ID", Value = cart.PRODUCT_ID.ToString() });
            param.Add(new Param { Key = "@IS_MAIN", Value = "0" });

            var responseMapper = ListProcedure<ProductColorSizeMapperModel>(new ProductColorSizeMapperModel(), "Product_Get_GetProductColorSizeMapperByColorIdAndSizeId", param);
            var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
            if (resultMapper != null && resultMapper.Count() > 0)
            {
                data = resultMapper.FirstOrDefault();
                cart.ProductColorSizeMapper = resultMapper.FirstOrDefault();
                if (data == null)
                {
                    result.Success = false;
                    result.Message = FunctionHelpers.GetValueLanguage("Message.Error.ProductNull");
                    result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }


            if (data.AMOUNT < cart.AMOUNT)
            {
                result.Success = false;
                result.Message = FunctionHelpers.GetValueLanguage("Message.Error.AddCartAmount");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (data.AMOUNT <= 0)
            {
                result.Success = false;
                result.Message = FunctionHelpers.GetValueLanguage("Message.Error.AddCartAmountMinus");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                return Json(result, JsonRequestBehavior.AllowGet);
            }


            //Lấy sản phẩm bởi ID sản phẩm
            param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = cart.PRODUCT_ID.ToString() });
            var responseProduct = ListProcedure<ProductModel>(new ProductModel(), "Product_Get_GetProductById", param);
            var resultProduct = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(responseProduct.Results));
            var product = new ProductModel();
            if (resultProduct != null && resultProduct.Count() > 0)
            {
                product = resultProduct.FirstOrDefault();
            }


            //Lấy danh sách bonus ID sản phẩm
            var responseNonus = ListProcedure<ProductBonusModel>(new ProductBonusModel(), "Product_Get_GetProductBonusByProductId", param);
            var resultProductBonus = JsonConvert.DeserializeObject<List<ProductBonusModel>>(JsonConvert.SerializeObject(responseNonus.Results));
            if (resultProductBonus != null && resultProductBonus.Count() > 0)
            {
                cart.ProductBonus = resultProductBonus;
            }

            cart.NAME = product.NAME;
            if (Session["Products"] != null)
            {
                listCart = Session["Products"] as List<CartModel>;
                var cartSession = listCart.Where(s => s.PRODUCT_ID == cart.PRODUCT_ID && s.COLOR_ID == cart.COLOR_ID && s.SIZE_ID == cart.SIZE_ID).ToList();
                if (cartSession != null && cartSession.Count() > 0)
                {
                    long totalSession = listCart.Where(s => s.PRODUCT_ID == cart.PRODUCT_ID && s.COLOR_ID == cart.COLOR_ID && s.SIZE_ID == cart.SIZE_ID).FirstOrDefault().AMOUNT = listCart.Where(s => s.PRODUCT_ID == cart.PRODUCT_ID && s.COLOR_ID == cart.COLOR_ID && s.SIZE_ID == cart.SIZE_ID).FirstOrDefault().AMOUNT;

                    long totalMore = totalSession + cart.AMOUNT;

                    if (totalMore > data.AMOUNT)
                    {
                        result.Success = false;
                        string error = FunctionHelpers.GetValueLanguage("Message.Error.AddCartAmountMore");
                        result.Message = String.Format(error, totalSession);
                        result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    listCart.Where(s => s.PRODUCT_ID == cart.PRODUCT_ID && s.COLOR_ID == cart.COLOR_ID && s.SIZE_ID == cart.SIZE_ID).FirstOrDefault().AMOUNT = listCart.Where(s => s.PRODUCT_ID == cart.PRODUCT_ID && s.COLOR_ID == cart.COLOR_ID && s.SIZE_ID == cart.SIZE_ID).FirstOrDefault().AMOUNT + cart.AMOUNT;
                }
                else
                {
                    listCart.Add(cart);
                }
            }
            else
            {
                listCart.Add(cart);
                Session["Products"] = listCart;
            }

            result.Success = true;
            result.Message = FunctionHelpers.GetValueLanguage("Message.AddToCartSuccess");
            result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
            result.Total = listCart.Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalCart()
        {
            var listCart = new List<CartModel>();
            if (Session["Products"] != null)
            {
                listCart = Session["Products"] as List<CartModel>;
            }
            ViewBag.TotalCart = listCart.Count();
            return View();
        }

        public ActionResult Cart()
        {
            CartViewModel vm = new CartViewModel();
            if (Session["Products"] != null)
            {
                vm.Carts = Session["Products"] as List<CartModel>;
            }
            return View(vm);
        }

        public ActionResult Update(List<CartModel> model)
        {
            ResultModel result = new ResultModel();
            bool check = true;
            var message = string.Empty;

            if (model != null && model.Count() > 0)
            {
                foreach (var item in model)
                {
                    var param = new List<Param>();
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
                            check = false;
                            message += String.Format(FunctionHelpers.GetValueLanguage("Message.Error.UpdateCartAmountMinus"), item.NAME, data.SIZE_NAME,data.COLOR,data.AMOUNT) + "<br/>";
                        }
                    }
                }
            }
            if (check)
            {
                var listCart = new List<CartModel>();
                var listNewCart = new List<CartModel>();

                listCart = Session["Products"] as List<CartModel>;
                if (model != null && model.Count() > 0)
                {
                    foreach (var item in model)
                    {
                        if (item.CHECK_ID!=0)
                        {
                            //listCart.Remove(item);
                        }
                        else
                        {
                           
                            listNewCart.Add(listCart.Where(s => s.PRODUCT_ID == item.PRODUCT_ID && s.COLOR_ID == item.COLOR_ID && s.SIZE_ID == item.SIZE_ID).FirstOrDefault());
                            listNewCart.Where(s => s.PRODUCT_ID == item.PRODUCT_ID && s.COLOR_ID == item.COLOR_ID && s.SIZE_ID == item.SIZE_ID).FirstOrDefault().AMOUNT = item.AMOUNT;
                        }
                       
                    }
                }
                Session["Products"] = listNewCart;
                result.Success = true;
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
            }
            else
            {
                result.Success = false;
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                result.Message = message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CheckOut()
        {
            CheckOutViewModel vm = new CheckOutViewModel();
            var param = new List<Param>();

            //danh sách phương thức thanh toán
            var responseStatusPay = ListProcedure<MethodPayModel>(new MethodPayModel(), "MethodPay_Get_MethodPay", new List<Param>(),true);
            var resultStatusPay= JsonConvert.DeserializeObject<List<MethodPayModel>>(JsonConvert.SerializeObject(responseStatusPay.Results));
            if (resultStatusPay != null && resultStatusPay.Count() > 0)
            {
                vm.MethodPays = resultStatusPay;
            }

            var responseShipFee = ListProcedure<ShipFeeModel>(new ShipFeeModel(), "ShipFee_Get_GetShipFee", new List<Param>(), true);
            var resultShipFee = JsonConvert.DeserializeObject<List<ShipFeeModel>>(JsonConvert.SerializeObject(responseShipFee.Results));
            if (resultShipFee != null && resultShipFee.Count() > 0)
            {
                vm.ShipFees = resultShipFee;
            }

            if (CurrentUser.User.USER_ID!=0)
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

            if (vm.Customer.CITY!=0)
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
            if (Session["UrlConTroller"] != null && Session["UrlView"] != null)
            {
                Session["UrlConTroller"] = null;
                Session["UrlView"] = null;
            }
            if (Session["Products"]!=null)
            {
                vm.Carts= Session["Products"] as List<CartModel>;
            }
            vm.UserId = CurrentUser.User.USER_ID;
            return View(vm);
        }

        public ActionResult ReloadDistrict(long codeCity)
        {
            SelectModel vm = new SelectModel();
            var param = new List<Param>();
            param.Add(new Param { Key = "@CODE_CITY", Value = codeCity.ToString() });
            var response= ListProcedure<DistrictModel>(new DistrictModel(), "District_Get_DistrictByCodeCity", param);
            if (response != null && response.Results.Count() > 0)
            {
                vm.Datas = response.Results;

            }
            vm.Id = "DISTRICT";
            vm.Name = "model.DISTRICT";
            vm.Class = "input-xlarge";
            vm.LabelName = "Cart.District";
            vm.IsRequied = true;
            vm.FieldId = "CODE";
            vm.FieldName = "NAME_VN";

            var html = RenderViewToString(this.ControllerContext, "~/Views/Template/_Select.cshtml", vm);

            vm.Datas = new List<dynamic>();
            vm.Id = "COMMUNITY";
            vm.Name = "model.COMMUNITY";
            vm.Class = "input-xlarge";
            vm.LabelName = "Cart.Community";
            vm.IsRequied = true;
            vm.FieldId = "CODE";
            vm.FieldName = "NAME_VN";

            var html1 = RenderViewToString(this.ControllerContext, "~/Views/Template/_Select.cshtml", vm);
            List<string> listHtml = new List<string>();
            listHtml.Add(html);
            listHtml.Add(html1);

            return Json(listHtml, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ReloadCommunity(long codeDistrict)
        {
            SelectModel vm = new SelectModel();
            var param = new List<Param>();
            param.Add(new Param { Key = "@CODE_DISTRICT", Value = codeDistrict.ToString() });
            var response = ListProcedure<DistrictModel>(new DistrictModel(), "Community_Get_CommunityByCodeDistrict", param);
            if (response != null && response.Results.Count() > 0)
            {
                vm.Datas = response.Results;

            }
            vm.Id = "COMMUNITY";
            vm.Name = "model.COMMUNITY";
            vm.Class = "input-xlarge";
            vm.LabelName = "Cart.Community";
            vm.IsRequied = true;
            vm.FieldId = "CODE";
            vm.FieldName = "NAME_VN";

            var html = RenderViewToString(this.ControllerContext, "~/Views/Template/_Select.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Order(OrderModel model)
        {
            if (model != null)
            {
                var validations = ValidationHelper.Validation(model, "model");
                if (validations.Count > 0)
                {
                    return Json(new { Result = validations, Invalid = true }, JsonRequestBehavior.AllowGet);
                }
            }
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@ORDER_CODE", Value = new Random().Next(1000000,9999999).ToString() });
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

            List<OrderDetailType> orderDetails = new List<OrderDetailType>();
            if (Session["Products"] !=null)
            {
                var carts = Session["Products"] as List<CartModel>;
                if (carts!=null && carts.Count()>0)
                {
                    foreach (var item in carts)
                    {
                        orderDetails.Add(new OrderDetailType() {
                            ID=0,
                            ORDER_ID=0,
                            PRODUCT_ID=item.PRODUCT_ID,
                            SIZE_ID=item.SIZE_ID,
                            COLOR_ID = item.COLOR_ID,
                            AMOUNT=item.AMOUNT
                        });
                    }
                }

            }
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@OrderDetailType", SqlDbType.Structured)
                {
                    TypeName = "dbo.OrderDetailType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(orderDetails)
                }
            });


            var data = new OrderModel();
            var responseOrder = ListProcedure<OrderModel>(new OrderModel(), "Order_Update_Order", param);
            var result = new ResultModel();
            if (responseOrder != null && responseOrder.Success == true)
            {
                result.Success = true;
                result.Message = FunctionHelpers.GetValueLanguage("Cart.OrderSucces");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
                result.Results = responseOrder.Results;
                data = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(result.Results)).FirstOrDefault();
                data.ToDecrypt = FunctionHelpers.Encrypt(data.ID.ToString(), FunctionHelpers.KeyEncrypt);
            }
            else
            {
                result.Success = false;
                result.Message = FunctionHelpers.GetValueLanguage("Cart.OrderError");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
            }

            return Json(new { result= result,data=data}, JsonRequestBehavior.AllowGet);

        }

        public ActionResult OrderInformation(string toDecrypt)
        {
            OrderInformationViewModel vm  = new OrderInformationViewModel();
            var id = FunctionHelpers.ConvertKey(toDecrypt, FunctionHelpers.KeyEncrypt);


            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id });
            var responseOrder = ListProcedure<OrderModel>(new OrderModel(), "Order_Get_GetOrderByID", param);
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));

            var responseShipFee = ListProcedure<ShipFeeModel>(new ShipFeeModel(), "ShipFee_Get_GetShipFee", new List<Param>(), true);
            var resultShipFee = JsonConvert.DeserializeObject<List<ShipFeeModel>>(JsonConvert.SerializeObject(responseShipFee.Results));
            if (resultShipFee != null && resultShipFee.Count() > 0)
            {
                vm.ShipFees = resultShipFee;
            }

            if (resultOrder!=null && resultOrder.Count()>0)
            {
                vm.Order = resultOrder.FirstOrDefault();
            }

            if (Session["Products"] != null)
            {
                vm.Carts = Session["Products"] as List<CartModel>;
                string subject = FunctionHelpers.GetValueLanguage("Email.SubjectOrder");
                string body = RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/Cart/_EmailOrder.cshtml", vm);
                List<string> toMail = new List<string>();
                toMail.Add(vm.Order.EMAIL);
                string from = ConfigurationManager.AppSettings["MailUserName"] + ";" + "S2Please";
                string replyTo = ConfigurationManager.AppSettings["MailUserName"];
                int resultCode = SendMail(subject, body, toMail, new List<string>(), new List<string>(), from, replyTo, new List<AttachmentJs>());
                if (resultCode != 200)
                {

                }
            }         
            Session["Products"] = null;
            return View(vm);
        }


        
    }
}