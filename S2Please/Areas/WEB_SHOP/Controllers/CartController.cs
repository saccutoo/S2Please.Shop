using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Models;
using S2Please.Areas.WEB_SHOP.ViewModel;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using SHOP.COMMON.Helpers;
using S2Please.ParramType;
using S2Please.Helper;
using SHOP.COMMON;
using S2Please.ViewModel;
using Repository;
namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class CartController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/Home
        private ISystemRepository _systemRepository;
        private IProductRepository _productRepository;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        public CartController(ISystemRepository systemRepository, IProductRepository productRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            this._systemRepository = systemRepository;
            this._productRepository = productRepository;
            this._customerRepository = customerRepository;
            this._orderRepository = orderRepository;
        }

        public ActionResult AddCart(CartModel cart)
        {
            var result = new ResultModel();
            var listCart = new List<CartModel>();
            ProductColorSizeMapperModel data = new ProductColorSizeMapperModel();
            var message = string.Empty;
            var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(cart.COLOR_ID, cart.SIZE_ID, cart.PRODUCT_ID,"0");
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
            var responseProduct = _productRepository.GetProductById(cart.PRODUCT_ID);
            var resultProduct = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(responseProduct.Results));
            var product = new ProductModel();
            if (resultProduct != null && resultProduct.Count() > 0)
            {
                product = resultProduct.FirstOrDefault();
            }


            //Lấy danh sách bonus ID sản phẩm
            var responseNonus = _productRepository.GetProductBonusByProductId(cart.PRODUCT_ID);
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
                    var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(item.COLOR_ID, item.SIZE_ID, item.PRODUCT_ID,"0");
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
            var responseStatusPay = _systemRepository.GetMethodPay();
            var resultStatusPay= JsonConvert.DeserializeObject<List<MethodPayModel>>(JsonConvert.SerializeObject(responseStatusPay.Results));
            if (resultStatusPay != null && resultStatusPay.Count() > 0)
            {
                vm.MethodPays = resultStatusPay;
            }

            var responseShipFee = _systemRepository.GetShipFee();
            var resultShipFee = JsonConvert.DeserializeObject<List<ShipFeeModel>>(JsonConvert.SerializeObject(responseShipFee.Results));
            if (resultShipFee != null && resultShipFee.Count() > 0)
            {
                vm.ShipFees = resultShipFee;
            }

            if (CurrentUser.User.USER_ID!=0)
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

            if (vm.Customer.CITY!=0)
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
            var response = _systemRepository.GetDistrictByCodeCity(codeCity);
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
            var response = _systemRepository.GetCommunityByCodeDistrict(codeDistrict);
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
            model.IS_ORDER = true;
            model.STATUS = 1;
            var modelInser = MapperHelper.Map<OrderModel,Repository.Model.OrderModel>(model);
            var orderDetailInserts = MapperHelper.MapList<OrderDetailType, Repository.Type.OrderDetailType>(orderDetails);
            var data = new OrderModel();
            var responseOrder = _orderRepository.UpdateOrder(modelInser, orderDetailInserts);
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
            var responseOrder = _orderRepository.GetOrderById(long.Parse(id));
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));

            var responseShipFee = _systemRepository.GetShipFee();
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
                string subject = FunctionHelpers.GetValueLanguage("Email.SubjectNewOrder");
                string body = RenderViewToString(this.ControllerContext, "~/Views/TemplateEmail/_EmailOrder.cshtml", vm);
                List<string> toMail = new List<string>();
                toMail.Add(vm.Order.EMAIL);
                string from = ConfigurationManager.AppSettings["MailUserName"] + ";" + "S2Please shop";
                string replyTo = ConfigurationManager.AppSettings["MailUserName"];
                int resultCode = SendMail(subject, body, toMail, new List<string>(), new List<string>(), from, replyTo, new List<AttachmentJs>(), vm.Order.ID, DataType.Order);
            }         
            Session["Products"] = null;
            return View(vm);
        }
        
    }
}