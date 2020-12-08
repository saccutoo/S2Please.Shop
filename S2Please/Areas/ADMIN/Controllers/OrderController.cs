using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using SHOP.COMMON;
using S2Please.Controllers;
using S2Please.Helper;
using System;
using S2Please.Areas.ADMIN.ViewModel;
using S2Please.ParramType;
using System.Data;
using SHOP.COMMON.Helpers;
using ClosedXML.Excel;
using System.IO;
using Repository;
using S2Please.Areas.ADMIN.Models;
using System.Text.RegularExpressions;
using S2Please.ViewModel;
using System.Configuration;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class OrderController : BaseController
    {
        // GET: ADMIN/Order

        private ITableRepository _tableRepository;
        private IOrderRepository _orderRepository;
        private ISystemRepository _systemRepository;
        private IProductRepository _productRepository;
        public OrderController(ITableRepository tableRepository, IOrderRepository orderRepository, ISystemRepository systemRepository, IProductRepository productRepository)
        {
            this._tableRepository = tableRepository;
            this._orderRepository = orderRepository;
            this._systemRepository = systemRepository;
            this._productRepository = productRepository;
        }

        public ActionResult Index(long id)
        {

            OrderViewModel order = new OrderViewModel();
            var responseTableUser = _tableRepository.GetTableUserConfig(TableName.Order, CurrentUser.UserAdmin.USER_ID);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                order.Table = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                var responseOrder = _tableRepository.GetTableByTableName(TableName.Order);
                var resultOrder = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                if (resultOrder != null && resultOrder.Count() > 0)
                {
                    order.Table = JsonConvert.DeserializeObject<TableViewModel>(resultOrder.FirstOrDefault().TABLE_CONTENT);
                }
            }

            ParamType paramType = new ParamType();
            paramType.PAGE_SIZE = order.Table.PAGE_SIZE == 0 ? 1 : order.Table.PAGE_SIZE;
            paramType.PAGE_NUMBER = order.Table.PAGE_INDEX == 0 ? 1 : order.Table.PAGE_INDEX;
            paramType.STRING_FILTER = id != 0 ? "AND O.ID=" + id : "";
            order.Table.TABLE_NAME = TableName.Order;
            order.Table.TITLE_TABLE_NAME = FunctionHelpers.GetValueLanguage("Table.Title.Order");
            order.Table = RenderTable(order.Table, paramType);
            if (!order.Table.IS_PERMISSION)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            //Table common
            return View(order);
        }

        public ActionResult Detail(long id = 0)
        {
            OrderDetailViewModel vm = new OrderDetailViewModel();
            //Lấy sản phẩm ID sản phẩm
            vm.ID = id;
            var response = _orderRepository.GetOrderDetailByOrderId(id);
            var result = JsonConvert.DeserializeObject<List<OrderDetailModel>>(JsonConvert.SerializeObject(response.Results));
            if (result != null && result.Count() > 0)
            {
                vm.OrderDetails = result;
            }

            var responseOrder = _orderRepository.GetOrderById(id);
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder != null && resultOrder.Count() > 0)
            {
                vm.Order = resultOrder.FirstOrDefault();
            }

            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_Detail.cshtml", vm);
            return Content(JsonConvert.SerializeObject(new
            {
                html
            }));
        }

        public ActionResult ViewPriceDetail(long colorId = 0, long sizeId = 0, long productId = 0)
        {
            ProductPriceViewModel vm = new ProductPriceViewModel();
            var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(colorId, sizeId, productId, "0");
            var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
            if (resultMapper != null && resultMapper.Count() > 0)
            {
                vm.ProductMapper = resultMapper.FirstOrDefault();
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_PriceDetail.cshtml", vm);
            return Content(JsonConvert.SerializeObject(new
            {
                html,
                vm.ProductMapper.AMOUNT
            }));
        }

        //public ActionResult ShowFormAddOrderCustomer()
        //{
        //    BaseModel model = new BaseModel();
        //    var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_FormAddOrderCustomer.cshtml", model);
        //    return Json(html, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult OrderSave(long id = 0)
        {
            if (Session["CART_ORDER"] != null)
            {
                Session["CART_ORDER"] = null;
            }
            bool checkPermission = FunctionHelpers.CheckPermission(TableName.Product, Permission.Update);
            if (!checkPermission)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }

            OrderSaveViewModel vm = new OrderSaveViewModel();
            vm.ID = id;
            var responseCity = _systemRepository.GetCity();
            vm.Citys = responseCity.Results;

            var responseTableUser = _tableRepository.GetTableUserConfig(TableName.ProductOrder, CurrentUser.UserAdmin.USER_ID);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                vm.Table = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                var responseOrder = _tableRepository.GetTableByTableName(TableName.ProductOrder);
                var resultOrder = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                if (resultOrder != null && resultOrder.Count() > 0)
                {
                    vm.Table = JsonConvert.DeserializeObject<TableViewModel>(resultOrder.FirstOrDefault().TABLE_CONTENT);
                }
            }

            ParamType paramType = new ParamType();
            paramType.PAGE_SIZE = vm.Table.PAGE_SIZE == 0 ? 1 : vm.Table.PAGE_SIZE;
            paramType.PAGE_NUMBER = vm.Table.PAGE_INDEX == 0 ? 1 : vm.Table.PAGE_INDEX;
            vm.Table.TABLE_NAME = TableName.ProductOrder;
            vm.Table.TITLE_TABLE_NAME = "";
            vm.Table = RenderTable(vm.Table, paramType);

            var responseStatusOrder = _systemRepository.GetStatusOrder();
            vm.StatusOrders = responseStatusOrder.Results;

            var responseStatusPay = _systemRepository.GetStatusPay();
            vm.StatusPays = responseStatusPay.Results;

            var responseMethodPay = _systemRepository.GetMethodPayAll();
            vm.MethodPays = responseMethodPay.Results;

            var responseShipFee = _systemRepository.GetAllShipFee();
            vm.ShipFees = responseShipFee.Results;

            var resultStatusMethodPay = JsonConvert.DeserializeObject<List<MethodPayModel>>(JsonConvert.SerializeObject(responseMethodPay.Results));

            return View(vm);
        }

        public ActionResult ShowFormAddProductToCart(long productId = 0)
        {
            OrderSaveViewModel vm = new OrderSaveViewModel();
            //Lấy sản phẩm ID sản phẩm
            var responseProduct = _productRepository.GetProductById(productId);
            var resultProduct = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(responseProduct.Results));
            if (resultProduct != null && resultProduct.Count() > 0)
            {
                vm.Product = resultProduct.FirstOrDefault();
            }

            //Lấy danh sách ảnh ID sản phẩm
            var responseProductImgs = _productRepository.GetProductImgByProductId(productId);
            vm.ProductImgs = JsonConvert.DeserializeObject<List<ProductImgModel>>(JsonConvert.SerializeObject(responseProductImgs.Results));

            //Lấy danh sách màu theo ID sản phẩm
            var responseColors = _productRepository.GetProductColorByProductId(productId);
            vm.ProductColors = JsonConvert.DeserializeObject<List<ProductColorModel>>(JsonConvert.SerializeObject(responseColors.Results));

            //Lấy danh sách size theo ID sản phẩm
            var responseSizes = _productRepository.GetProductSizeByProductId(productId);
            vm.ProductSizes = JsonConvert.DeserializeObject<List<ProductSizeModel>>(JsonConvert.SerializeObject(responseSizes.Results));

            //Lấy bản ghi mapper giữa size và color
            long colorID = vm.ProductColors != null && vm.ProductColors.Count() > 0 && vm.ProductColors.Where(s => s.IS_MAIN == true).FirstOrDefault() != null ? vm.ProductColors.Where(s => s.IS_MAIN == true).FirstOrDefault().ID : 0;
            long sizeID = vm.ProductSizes != null && vm.ProductSizes.Count() > 0 && vm.ProductSizes.Where(s => s.IS_MAIN == true).FirstOrDefault() != null ? vm.ProductSizes.Where(s => s.IS_MAIN == true).FirstOrDefault().ID : 0;
            var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(colorID, sizeID, vm.Product.ID, "1");
            var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
            if (resultMapper != null && resultMapper.Count() > 0)
            {
                vm.ProductMapper = resultMapper.FirstOrDefault();
            }

            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_FormAddProductToCart.cshtml", vm);
            return Content(JsonConvert.SerializeObject(new
            {
                html
            }));
        }

        public ActionResult AddProductToCart(long colorId = 0, long sizeId = 0, long productId = 0)
        {
            OrderSaveViewModel vm = new OrderSaveViewModel();
            ResultModel result = new ResultModel();
            if (Session["CART_ORDER"] != null)
            {
                vm.Carts = Session["CART_ORDER"] as List<CartModel>;
            }

            if (colorId == 0 || sizeId == 0 || productId == 0)
            {
                result.Success = false;
                result.Message = "Thông tin trên form chưa đầy đủ vui lòng chọn lại.";
                return Content(JsonConvert.SerializeObject(new
                {
                    result
                }));
            }

            //Lấy sản phẩm ID sản phẩm
            var responseProduct = _productRepository.GetProductById(productId);
            var resultProduct = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(responseProduct.Results));

            //Lấy bản ghi mapper giữa size và color
            var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(colorId, sizeId, productId, "0");
            var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
            if (resultMapper != null && resultMapper.Count() > 0)
            {
                if (vm.Carts != null && vm.Carts.Count() > 0)
                {
                    var product = vm.Carts.Where(s => s.PRODUCT_ID == productId && s.COLOR_ID == colorId && sizeId == s.SIZE_ID).FirstOrDefault();
                    if (product != null)
                    {
                        if ((product.AMOUNT + 1) > resultMapper.FirstOrDefault().AMOUNT)
                        {
                            result.Success = false;
                            var message = FunctionHelpers.GetValueLanguage("Message.Error.UpdateCartAmountMinus");
                            result.Message = string.Format(message, product.NAME, resultMapper.FirstOrDefault().SIZE_NAME, resultMapper.FirstOrDefault().COLOR, resultMapper.FirstOrDefault().AMOUNT);
                            result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                            return Content(JsonConvert.SerializeObject(new
                            {
                                result
                            }));
                        }
                        else
                        {
                            vm.Carts.Where(s => s.PRODUCT_ID == productId && s.COLOR_ID == colorId && sizeId == s.SIZE_ID).FirstOrDefault().AMOUNT += 1;
                        }

                    }
                    else
                    {
                        CartModel cart = new CartModel();
                        cart.ProductColorSizeMapper = resultMapper.FirstOrDefault();
                        cart.Cart(resultProduct.FirstOrDefault());
                        vm.Carts.Add(cart);
                    }
                }
                else
                {
                    CartModel cart = new CartModel();
                    cart.ProductColorSizeMapper = resultMapper.FirstOrDefault();
                    cart.Cart(resultProduct.FirstOrDefault());
                    vm.Carts.Add(cart);
                }
            }
            Session["CART_ORDER"] = vm.Carts;
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_ContentCart.cshtml", vm);
            result.Html = html;
            result.Success = true;
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult Order(OrderModel model)
        {
            ResultModel result = new ResultModel();
            var validations = new List<ValidationModel>();
            if (string.IsNullOrEmpty(model.FULL_NAME))
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.FULL_NAME";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (string.IsNullOrEmpty(model.PHONE))
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.PHONE";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (!string.IsNullOrEmpty(model.PHONE))
            {
                var phoneReg = @"^\d{9,15}$";
                if (!Regex.IsMatch(model.PHONE, phoneReg))
                {
                    ValidationModel validaton = new ValidationModel();
                    validaton.ColumnName = "model.PHONE";
                    validaton.IsError = false;
                    validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.PhoneFormat");
                    validations.Add(validaton);
                }

            }
            if (model.STATUS == 0)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.STATUS";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (model.STATUS_PAY == 0)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.STATUS_PAY";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (model.METHOD_PAY == 0)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.METHOD_PAY";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (model.FEE_SHIP == 0)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.FEE_SHIP";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (Session["CART_ORDER"] == null)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "listCard";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Message.UpdateAllCart.Error");
                validations.Add(validaton);
            }
            if (validations != null && validations.Count() > 0)
            {
                return Json(new { Result = validations, Invalid = true }, JsonRequestBehavior.AllowGet);
            }

            List<CartModel> carts = Session["CART_ORDER"] as List<CartModel>;
            List<OrderDetailType> orderDetails = new List<OrderDetailType>();
            foreach (var item in carts)
            {
                orderDetails.Add(new OrderDetailType()
                {
                    ID = 0,
                    ORDER_ID = 0,
                    PRODUCT_ID = item.PRODUCT_ID,
                    SIZE_ID = item.SIZE_ID,
                    COLOR_ID = item.COLOR_ID,
                    AMOUNT = item.AMOUNT
                });
            }
            model.IS_ORDER = false;
            var modelInser = MapperHelper.Map<OrderModel, Repository.Model.OrderModel>(model);
            var orderDetailInserts = MapperHelper.MapList<OrderDetailType, Repository.Type.OrderDetailType>(orderDetails);

            var data = new OrderModel();
            var responseOrder = _orderRepository.UpdateOrder(modelInser, orderDetailInserts,true);
            if (responseOrder.Success == false && CheckPermision(responseOrder.StatusCode) == false)
            {
                result.SetUrl("/Base/Page404");
            }
            else if (responseOrder != null && responseOrder.Success == true)
            {
                result.Success = true;
                result.Message = FunctionHelpers.GetValueLanguage("Order.AddOrder");
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
                result.Results = responseOrder.Results;
                data = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(result.Results)).FirstOrDefault();
                if (!string.IsNullOrEmpty(model.EMAIL))
                {
                    OrderInformationViewModel vm = new OrderInformationViewModel();
                    vm.Carts = Session["CART_ORDER"] as List<CartModel>;
                    var response = _orderRepository.GetOrderById(data.ID);
                    var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(response.Results));

                    var responseShipFee = _systemRepository.GetAllShipFee();
                    var resultShipFee = JsonConvert.DeserializeObject<List<ShipFeeModel>>(JsonConvert.SerializeObject(responseShipFee.Results));
                    if (resultShipFee != null && resultShipFee.Count() > 0)
                    {
                        vm.ShipFees = resultShipFee;
                    }

                    if (resultOrder != null && resultOrder.Count() > 0)
                    {
                        vm.Order = resultOrder.FirstOrDefault();
                    }

                    string subject = FunctionHelpers.GetValueLanguage("Email.SubjectOrder");
                    string body = RenderViewToString(this.ControllerContext, "~/Views/TemplateEmail/_EmailOrder.cshtml", vm);
                    List<string> toMail = new List<string>();
                    toMail.Add(vm.Order.EMAIL);
                    string from = ConfigurationManager.AppSettings["MailUserName"] + ";" + "S2Please shop";
                    string replyTo = ConfigurationManager.AppSettings["MailUserName"];
                    int resultCode = SendMail(subject, body, toMail, new List<string>(), new List<string>(), from, replyTo, new List<AttachmentJs>(), vm.Order.ID, DataType.Order);
                }
                Session["CART_ORDER"] = null;

            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult CheckStatus(long id=0)
        {
            ResultModel result = new ResultModel();
            result.Success = true;
            var responseOrder= _orderRepository.GetOrderById(id);
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder.FirstOrDefault().STATUS!=StatusOrder.Pending)
            {
                result.Success = false;
                var message = FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusOrder");
                var statusName = FunctionHelpers.GetValueLocalization(resultOrder.FirstOrDefault().STATUS, DataType.CL_STATUS_ORDER, "NAME");
                result.CacheName = string.Empty;
                result.Message = string.Format(message, statusName.ToLower());
            }
            else if (resultOrder.FirstOrDefault().STATUS_PAY == StatusPay.IsPay)
            {
                result.Success = false;
                result.CacheName = string.Empty;
                result.Message = FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusPay");
            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult UpdateOrder(long id = 0)
        {
            bool checkPermission = FunctionHelpers.CheckPermission(TableName.Product, Permission.Update);
            if (!checkPermission)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            OrderSaveViewModel vm = new OrderSaveViewModel();
            vm.ID = id;

            var responseOrder = _orderRepository.GetOrderById(id,true);
            if (responseOrder.Success == false && CheckPermision(responseOrder.StatusCode) == false)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder != null && resultOrder.Count() > 0)
            {
                vm.Order = resultOrder.FirstOrDefault();
            }

            var responseOrderDetail = _orderRepository.GetOrderDetailByOrderId(id);
            var resultOrderDetail = JsonConvert.DeserializeObject<List<OrderDetailModel>>(JsonConvert.SerializeObject(responseOrderDetail.Results));
            if (resultOrderDetail != null && resultOrderDetail.Count() > 0)
            {
                vm.OrderDetails = resultOrderDetail;
            }

            var responseCity = _systemRepository.GetCity();
            vm.Citys = responseCity.Results;

            if (vm.Order.CITY!=0)
            {
                var responseDistrict= _systemRepository.GetDistrictByCodeCity(vm.Order.CITY);
                if (responseDistrict != null && responseDistrict.Results.Count() > 0)
                {
                    vm.Districts = responseDistrict.Results;
                }
            }

            if (vm.Order.DISTRICT != 0)
            {
                var responseCommunity = _systemRepository.GetCommunityByCodeDistrict(vm.Order.DISTRICT);
                if (responseCommunity != null && responseCommunity.Results.Count() > 0)
                {
                    vm.Communitys = responseCommunity.Results;
                }
            }

            var responseStatusOrder = _systemRepository.GetStatusOrder();
            vm.StatusOrders = responseStatusOrder.Results;

            var responseStatusPay = _systemRepository.GetStatusPay();
            vm.StatusPays = responseStatusPay.Results;

            var responseMethodPay = _systemRepository.GetMethodPayAll();
            vm.MethodPays = responseMethodPay.Results;

            var responseShipFee = _systemRepository.GetAllShipFee();
            vm.ShipFees = responseShipFee.Results;

            var resultStatusMethodPay = JsonConvert.DeserializeObject<List<MethodPayModel>>(JsonConvert.SerializeObject(responseMethodPay.Results));

            return View(vm);
        }

        public ActionResult SaveUpdateOrder(OrderModel model,List<OrderDetailModel> orderDetails)
        {
            ResultModel result = new ResultModel();
            var validations = new List<ValidationModel>();
            if (string.IsNullOrEmpty(model.FULL_NAME))
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.FULL_NAME";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (string.IsNullOrEmpty(model.PHONE))
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.PHONE";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (!string.IsNullOrEmpty(model.PHONE))
            {
                var phoneReg = @"^\d{9,15}$";
                if (!Regex.IsMatch(model.PHONE, phoneReg))
                {
                    ValidationModel validaton = new ValidationModel();
                    validaton.ColumnName = "model.PHONE";
                    validaton.IsError = false;
                    validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.PhoneFormat");
                    validations.Add(validaton);
                }

            }
            if (model.STATUS == 0)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.STATUS";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (model.STATUS_PAY == 0)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.STATUS_PAY";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (model.METHOD_PAY == 0)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.METHOD_PAY";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }
            if (model.FEE_SHIP == 0)
            {
                ValidationModel validaton = new ValidationModel();
                validaton.ColumnName = "model.FEE_SHIP";
                validaton.IsError = false;
                validaton.ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required");
                validations.Add(validaton);
            }

            if (orderDetails !=null && orderDetails.Count()>0)
            {
                int index = 0;
                foreach (var item in orderDetails)
                {
                    if (item.AMOUNT <= 0)
                    {
                        validations.Add(new ValidationModel()
                        {
                            IsError = true,
                            ColumnName = "orderDetails[" + index + "].AMOUNT",
                            ErrorMessage = FunctionHelpers.GetValueLanguage("Message.Error.Amount")
                        });
                    }
                    else
                    {
                        var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(item.COLOR_ID, item.SIZE_ID, item.PRODUCT_ID, "0");
                        var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
                        if (resultMapper != null && resultMapper.Count() > 0)
                        {
                            var orderDetail = resultMapper.FirstOrDefault();
                            if (orderDetail.AMOUNT < item.AMOUNT)
                            {
                                validations.Add(new ValidationModel()
                                {
                                    IsError = true,
                                    ColumnName = "orderDetails[" + index + "].AMOUNT",
                                    ErrorMessage = String.Format(FunctionHelpers.GetValueLanguage("Message.Error.Amount1"), orderDetail.AMOUNT)
                                });
                            }
                        }
                    }
                    index++;
                }
            }

            if (validations != null && validations.Count() > 0)
            {
                return Json(new { Result = validations, Invalid = true }, JsonRequestBehavior.AllowGet);
            }

            orderDetails = orderDetails.Where(s => s.IS_CHECK == false).ToList();
            if (orderDetails==null)
            {
                orderDetails = new List<OrderDetailModel>();
            }
            var modelUpdate = MapperHelper.Map<OrderModel, Repository.Model.OrderModel>(model);
            var orderDetailUpdates = MapperHelper.MapList<OrderDetailModel, Repository.Type.OrderDetailType>(orderDetails);
            var data = new OrderModel();
            var responseOrder = _orderRepository.UpdateOrder(modelUpdate, orderDetailUpdates, true);
            if (responseOrder.Success == false && CheckPermision(responseOrder.StatusCode) == false)
            {
                result.SetUrl("/Base/Page404");
            }
            else if (responseOrder != null && responseOrder.Success == true)
            {
                result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.Order.UpdateSuccess"), FunctionHelpers.GetValueLanguage("Message.Success"), string.Empty);
                result.Results = responseOrder.Results;
                data = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(result.Results)).FirstOrDefault();              
            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult UpdateCartAll(List<CartModel> listCard)
        {
            OrderSaveViewModel vm = new OrderSaveViewModel();
            ResultModel result = new ResultModel();
            var html = string.Empty;
            if (Session["CART_ORDER"] != null)
            {
                vm.Carts = Session["CART_ORDER"] as List<CartModel>;
            }
            if (listCard != null && listCard.Count() > 0)
            {
                foreach (var item in listCard)
                {
                    var product = vm.Carts.Where(s => s.PRODUCT_ID == item.PRODUCT_ID && s.COLOR_ID == item.COLOR_ID && item.SIZE_ID == s.SIZE_ID).FirstOrDefault();
                    if (item.IS_CHECK)
                    {
                        vm.Carts.Remove(product);
                        continue;
                    }
                    else if (product != null)
                    {
                        //Lấy bản ghi mapper giữa size và color
                        var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(item.COLOR_ID, item.SIZE_ID, item.PRODUCT_ID, "0");
                        var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
                        if (item.AMOUNT > resultMapper.FirstOrDefault().AMOUNT)
                        {
                            var message = FunctionHelpers.GetValueLanguage("Message.Error.UpdateCartAmountMinus");
                            result.Success = false;
                            result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                            result.Message = string.Format(message, product.NAME, resultMapper.FirstOrDefault().SIZE_NAME, resultMapper.FirstOrDefault().COLOR, resultMapper.FirstOrDefault().AMOUNT);
                            html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_ContentCart.cshtml", vm);
                            result.Html = html;
                            return Content(JsonConvert.SerializeObject(new
                            {
                                result
                            }));
                        }
                        else
                        {
                            vm.Carts.Where(s => s.PRODUCT_ID == item.PRODUCT_ID && s.COLOR_ID == item.COLOR_ID && item.SIZE_ID == s.SIZE_ID).FirstOrDefault().AMOUNT = item.AMOUNT;
                        }
                    }
                }
            }
            else if (vm.Carts == null || vm.Carts.Count == 0 || listCard == null || listCard.Count == 0)
            {
                html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_ContentCart.cshtml", vm);
                result.Html = html;
                result.Success = false;
                result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                result.Message = FunctionHelpers.GetValueLanguage("Message.UpdateAllCart.Error");
                return Content(JsonConvert.SerializeObject(new
                {
                    result
                }));
            }
            Session["CART_ORDER"] = vm.Carts;
            html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_ContentCart.cshtml", vm);
            result.Success = true;
            result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
            result.Message = FunctionHelpers.GetValueLanguage("Message.UpdateProduct.Success");
            result.Html = html;
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult UpdateStatusOrder(List<BaseModel> listDatas,long status=0)
        {
            ResultModel result = new ResultModel();
            var type = MapperHelper.MapList<BaseModel, Repository.Type.DataIdType>(listDatas);
            if (listDatas==null || listDatas.Count()==0)
            {
                result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusOrder1"), FunctionHelpers.GetValueLanguage("Message.Error"),string.Empty);
            }
            else
            {
                var response = _orderRepository.UpdateStatusOrder(type, status, true);
                if (response.Success == false && CheckPermision(response.StatusCode) == false)
                {
                    result.SetUrl("/Base/Page404");
                }
                else
                {
                    var resultProduct = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(response.Results));
                    if (resultProduct!=null && resultProduct.Count()>1)
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusOrder2"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else if (resultProduct != null && resultProduct.Count() ==1 && resultProduct.FirstOrDefault().ID==1)
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusOrder3"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else if (resultProduct != null && resultProduct.Count() == 1 && resultProduct.FirstOrDefault().ID == 2)
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusOrder4"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else if (resultProduct != null && resultProduct.Count() == 1 && resultProduct.FirstOrDefault().ID == 3)
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusOrder5"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else if (resultProduct != null && resultProduct.Count() == 1 && resultProduct.FirstOrDefault().ID == 4)
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusOrder6"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else if (resultProduct != null && resultProduct.Count() == 1 && resultProduct.FirstOrDefault().ID == 5)
                    {
                        var textStatus = FunctionHelpers.GetValueLocalization(status, DataType.CL_STATUS_ORDER, PropertyName.Name);
                        var responseOrder = _orderRepository.GetOrderById(type.FirstOrDefault().ID);
                        var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));              
                        var textStatusCurrent = FunctionHelpers.GetValueLocalization(resultOrder.FirstOrDefault().STATUS, DataType.CL_STATUS_ORDER, PropertyName.Name);
                        result.SetDataMessage(false, string.Format(FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusOrder7"), textStatusCurrent, textStatus), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else
                    {
                        if (status==StatusOrder.NoApproval || status == StatusOrder.Sending)
                        {
                            foreach (var item in type)
                            {
                                OrderInformationViewModel vm = new OrderInformationViewModel();
                                long id = item.ID;
                                var responseOrder = _orderRepository.GetOrderById(id);
                                var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                                if (resultOrder != null && resultOrder.Count() > 0)
                                {
                                    vm.Order = resultOrder.FirstOrDefault();
                                }

                                var responseShipFee = _systemRepository.GetShipFee();
                                var resultShipFee = JsonConvert.DeserializeObject<List<ShipFeeModel>>(JsonConvert.SerializeObject(responseShipFee.Results));
                                if (resultShipFee != null && resultShipFee.Count() > 0)
                                {
                                    vm.ShipFees = resultShipFee;
                                }

                                var responseOrderDetail = _orderRepository.GetOrderDetailByOrderId(vm.Order.ID);
                                var resultOrderDetail = JsonConvert.DeserializeObject<List<OrderDetailModel>>(JsonConvert.SerializeObject(responseOrderDetail.Results));

                                if (resultOrderDetail != null && resultOrderDetail.Count() > 0)
                                {
                                    foreach (var orderDetail in resultOrderDetail)
                                    {
                                        ProductModel product = new ProductModel();
                                        var responseProduct = _productRepository.GetProductById(orderDetail.PRODUCT_ID);
                                        var resultProductSendEmail = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(responseProduct.Results));
                                        if (resultProductSendEmail != null && resultProductSendEmail.Count() > 0)
                                        {
                                            product = resultProductSendEmail.FirstOrDefault();
                                        }

                                        CartModel cart = new CartModel();
                                        var responseProductMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(orderDetail.COLOR_ID, orderDetail.SIZE_ID, orderDetail.PRODUCT_ID, "0");
                                        var resultProductMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseProductMapper.Results));
                                        if (resultProductMapper != null && resultProductMapper.Count() > 0)
                                        {
                                            cart.ProductColorSizeMapper = resultProductMapper.FirstOrDefault();
                                            cart.ProductColorSizeMapper.PRICE = orderDetail.PRICE;
                                            cart.ProductColorSizeMapper.DISCOUNT_RATE = orderDetail.RATE;
                                        }
                                        cart.AMOUNT = orderDetail.AMOUNT;
                                        cart.NAME = product.NAME;
                                        vm.Carts.Add(cart);
                                    }
                                }

                                if (vm.Carts != null && vm.Carts.Count() > 0)
                                {
                                    string subject = string.Format(FunctionHelpers.GetValueLanguage("Subject.UpdateStatus"), vm.Order.ORDER_CODE);
                                    string body = RenderViewToString(this.ControllerContext, "~/Views/TemplateEmail/_EmailOrder.cshtml", vm);
                                    List<string> toMail = new List<string>();
                                    toMail.Add(vm.Order.EMAIL);
                                    string from = ConfigurationManager.AppSettings["MailUserName"] + ";" + "S2Please shop";
                                    string replyTo = ConfigurationManager.AppSettings["MailUserName"];
                                    int resultCode = SendMail(subject, body, toMail, new List<string>(), new List<string>(), from, replyTo, new List<AttachmentJs>(), item.ID, DataType.Order);
                                }
                            }

                        }
                        result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.Order.UpdateSuccess"), FunctionHelpers.GetValueLanguage("Message.Success"));
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult Delete(long id=0)
        {
            ResultModel result = new ResultModel();
            var response = _orderRepository.Delete(id, true);
            if (response.Success == false && CheckPermision(response.StatusCode) == false)
            {
                result.SetUrl("/Base/Page404");
            }
            else
            {
                result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.Remove.Sucess"), FunctionHelpers.GetValueLanguage("Message.Success"));
            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult UpdateStatusPay(List<BaseModel> listDatas, long statusPay = 0)
        {
            ResultModel result = new ResultModel();
            var type = MapperHelper.MapList<BaseModel, Repository.Type.DataIdType>(listDatas);
            if (listDatas == null || listDatas.Count() == 0)
            {
                result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusPay1"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
            }
            else
            {
                var response = _orderRepository.UpdateStatusPay(type, statusPay, true);
                if (response.Success == false && CheckPermision(response.StatusCode) == false)
                {
                    result.SetUrl("/Base/Page404");
                }
                else
                {
                    var resultProduct = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(response.Results));
                    if (resultProduct != null && resultProduct.Count() > 1)
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusPay2"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else if (resultProduct != null && resultProduct.Count() == 1 && resultProduct.FirstOrDefault().ID == 1)
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusPay3"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else if (resultProduct != null && resultProduct.Count() == 1 && resultProduct.FirstOrDefault().ID == 2)
                    {
                        var textStatus = FunctionHelpers.GetValueLocalization(statusPay, DataType.CL_STATUS_PAY, PropertyName.Name);
                        var responseOrder = _orderRepository.GetOrderById(type.FirstOrDefault().ID);
                        var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                        var textStatusCurrent = FunctionHelpers.GetValueLocalization(resultOrder.FirstOrDefault().STATUS_PAY, DataType.CL_STATUS_PAY, PropertyName.Name);
                        result.SetDataMessage(false, string.Format(FunctionHelpers.GetValueLanguage("Message.Error.CheckStatusPay4"), textStatusCurrent, textStatus), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }
                    else
                    {
                        result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.Order.UpdateSuccess"), FunctionHelpers.GetValueLanguage("Message.Success"));
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        #region RenderTable
        public ActionResult ReloadTable(TableViewModel tableData, ParamType param)
        {
            var paramType = new List<Param>();
            var tableName = tableData.TABLE_NAME;
            if (param.STRING_FILTER == null)
            {
                param.STRING_FILTER = string.Empty;
            }

            var responseTableUser = _tableRepository.GetTableUserConfig(tableData.TABLE_NAME, CurrentUser.UserAdmin.USER_ID);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                tableData = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                var responseOrder = _tableRepository.GetTableByTableName(tableData.TABLE_NAME);
                var resultOrder = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                if (resultOrder != null && resultOrder.Count() > 0)
                {
                    tableData = JsonConvert.DeserializeObject<TableViewModel>(resultOrder.FirstOrDefault().TABLE_CONTENT);
                }
            }

            tableData.TABLE_NAME = tableName;
            if (tableName == TableName.Product)
            {
                tableData.TITLE_TABLE_NAME = FunctionHelpers.GetValueLanguage("Table.Title.Order");
            }
            tableData = RenderTable(tableData, param);

            if (!tableData.IS_PERMISSION)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/_TableContent.cshtml", tableData);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        private TableViewModel RenderTable(TableViewModel tableData, ParamType paramType)
        {
            //model param
            paramType.LANGUAGE_ID = CurrentUser.LANGUAGE_ID;
            paramType.USER_ID = CurrentUser.UserAdmin.USER_ID;
            paramType.ROLE_ID = CurrentUser.UserAdmin.ROLE_ID;

            //Gọi hàm lấy thông tin 

            tableData = GetData(tableData, paramType);
            tableData.PAGE_SIZE = paramType.PAGE_SIZE;
            tableData.PAGE_INDEX = paramType.PAGE_NUMBER;
            tableData.PAGE_SIZE = paramType.PAGE_SIZE;
            tableData.PAGE_INDEX = paramType.PAGE_NUMBER;
            tableData.TABLE_COLUMN_FIELD = tableData.TABLE_COLUMN_FIELD.Where(s => s.IS_SHOW == true).OrderBy(s => s.POSITION).ToList();
            tableData.VALUE_DYNAMIC = paramType.VALUE;
            tableData.STRING_FILTER = paramType.STRING_FILTER;
            if (tableData.TABLE_NAME == TableName.ProductOrder)
            {
                tableData.TABLE_URL = TableUrl.ProductOrder;
                tableData.MENU_NAME = MenuName.ProductOrder;
                tableData.TABLE_EXPORT_URL = TableExportUrl.ProductOrder;
                tableData.TABLE_SESION_EXPORT_URL = TableSesionExportUrl.ProductOrder;
            }
            if (tableData.TABLE_NAME == TableName.Order)
            {
                tableData.TABLE_URL = TableUrl.Order;
                tableData.MENU_NAME = MenuName.Order;
                tableData.TABLE_EXPORT_URL = TableExportUrl.Order;
                tableData.TABLE_SESION_EXPORT_URL = TableSesionExportUrl.Order;
            }

            if (tableData.TABLE_COLUMN == null || tableData.TABLE_COLUMN.Count() == 0)
            {
                var responseColumn = _tableRepository.GetTableColumnByTableName(tableData.TABLE_NAME);
                if (responseColumn != null && responseColumn.Results.Count() > 0)
                {
                    tableData.TABLE_COLUMN = JsonConvert.DeserializeObject<List<TableColumnModel>>(JsonConvert.SerializeObject(responseColumn.Results));
                }
            }

            if (tableData.SELECT_OPTION == null || tableData.SELECT_OPTION.Count() == 0)
            {
                var responseSelectOption = _tableRepository.GetSelectOption();
                if (responseSelectOption != null && responseSelectOption.Results.Count() > 0)
                {
                    tableData.SELECT_OPTION = JsonConvert.DeserializeObject<List<SelectOptionModel>>(JsonConvert.SerializeObject(responseSelectOption.Results));
                }
            }

            if (tableData.SELECT_OPTION != null && tableData.SELECT_OPTION.Count() > 0)
            {
                foreach (var item in tableData.SELECT_OPTION)
                {
                    if (Convert.ToInt32(item.VALUE) == paramType.PAGE_SIZE)
                    {
                        item.IS_SELECTED = true;
                        break;
                    }
                }
            }
            return tableData;
        }

        public TableViewModel GetData(TableViewModel tableData, ParamType paramType)
        {
            var type = MapperHelper.Map<ParamType, Repository.Type.ParamType>(paramType);
            if (tableData.TABLE_NAME == TableName.Order)
            {
                var response = _orderRepository.GetOrderFromAdmin(type);
                if (response != null)
                {
                    if (response.Success == false && CheckPermision(response.StatusCode) == false)
                    {

                        tableData.IS_PERMISSION = false;
                    }
                    else
                    {
                        tableData.DATA = response.Results;
                        tableData.TOTAL = Convert.ToInt32(response.OutValue.Parameters["@TotalRecord"].Value.ToString());
                    }
                }
            }
            else if (tableData.TABLE_NAME == TableName.ProductOrder)
            {
                var response = _productRepository.ProductFromAdmin(type, false);
                if (response != null)
                {
                    if (response.Success == false && CheckPermision(response.StatusCode) == false)
                    {

                        tableData.IS_PERMISSION = false;
                    }
                    else
                    {
                        tableData.DATA = response.Results;
                        tableData.TOTAL = Convert.ToInt32(response.OutValue.Parameters["@TotalRecord"].Value.ToString());
                    }
                }
            }
            return tableData;
        }

        public ActionResult SesionExport(TableViewModel tableData, ParamType paramType)
        {
            if (String.IsNullOrEmpty(paramType.STRING_FILTER))
            {
                paramType.STRING_FILTER = string.Empty;
            }
            paramType.LANGUAGE_ID = CurrentUser.LANGUAGE_ID;
            paramType.USER_ID = CurrentUser.UserAdmin.USER_ID;
            paramType.ROLE_ID = CurrentUser.UserAdmin.ROLE_ID;
            paramType.STRING_FILTER = paramType.STRING_FILTER;
            paramType.PAGE_NUMBER = 1;
            paramType.PAGE_SIZE = long.MaxValue;
            paramType.VALUE = paramType.VALUE;
            tableData = GetData(tableData, paramType);
            Session["Export"] = tableData.DATA;
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Export(string TABLE_NAME)
        {
            TableViewModel tableData = new TableViewModel();
            tableData.TABLE_NAME = TABLE_NAME;
            var responseOrder = _tableRepository.GetTableByTableName(TABLE_NAME);
            var resultOrder = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseOrder.Results));
            if (resultOrder != null && resultOrder.Count() > 0)
            {
                tableData = JsonConvert.DeserializeObject<TableViewModel>(resultOrder.FirstOrDefault().TABLE_CONTENT);
            }

            //tableData = GetData(tableData, paramType);
            tableData.TABLE_COLUMN_FIELD = tableData.TABLE_COLUMN_FIELD.Where(s => s.IS_SHOW == true).OrderBy(s => s.POSITION).ToList();
            tableData.DATA = Session["Export"] as List<dynamic>;

            var responseColumn = _tableRepository.GetTableColumnByTableName(TABLE_NAME);
            if (responseColumn != null && responseColumn.Results.Count() > 0)
            {
                tableData.TABLE_COLUMN = JsonConvert.DeserializeObject<List<TableColumnModel>>(JsonConvert.SerializeObject(responseColumn.Results));

            }

            DataTable dt = new DataTable("Grid");
            DataTable dt1 = new DataTable("Grid");
            if (tableData.TABLE_COLUMN_FIELD != null && tableData.TABLE_COLUMN_FIELD.Count() > 0)
            {
                foreach (var item in tableData.TABLE_COLUMN_FIELD)
                {
                    if (item.PRESENTATION != "CHECK_BOX" && item.PRESENTATION != "ACTION")
                    {
                        var column = tableData.TABLE_COLUMN.Where(s => s.COLUMN_NAME == item.COLUMN_NAME).ToList().FirstOrDefault();
                        if (column != null)
                        {
                            dt.Columns.Add(FunctionHelpers.GetValueLocalization(column.ID, "TABLE_COLUMN", "COLUMN_NAME"));
                            dt1.Columns.Add(column.COLUMN_NAME);
                        }
                    }
                }
            }

            var wb = new XLWorkbook();
            wb.AddWorksheet(TABLE_NAME);
            var ws = wb.Worksheet(TABLE_NAME);
            var rowIndex = tableData.DATA.Count();
            var columnIndex = dt1.Columns.Count;
            int Cell = 1;
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                ws.Cell(Cell, i + 1).Value = dt.Columns[i].ToString();

            }
            if (tableData.DATA != null && tableData.DATA.Count() > 0)
            {
                for (int i = 0; i < tableData.DATA.Count(); i++)
                {
                    Cell = Cell + 1;
                    foreach (var item in dt1.Columns)
                    {

                        for (var j = 0; j < dt1.Columns.Count; j++)
                        {
                            var value = string.Empty;
                            var column = tableData.TABLE_COLUMN.Where(s => s.COLUMN_NAME == dt1.Columns[j].ToString()).ToList().FirstOrDefault();
                            if (!string.IsNullOrEmpty(column.DATA_TYPE) && !string.IsNullOrEmpty(column.PROPERTY_NAME))
                            {
                                value = FunctionHelpers.GetValueLocalization(long.Parse(tableData.DATA[i][column.COLUMN_DATA_ID].Value), column.DATA_TYPE, column.PROPERTY_NAME);
                            }
                            else if (tableData.DATA[i][dt1.Columns[j].ToString()] != null && tableData.DATA[i][dt1.Columns[j].ToString()].Value != null)
                            {
                                value = tableData.DATA[i][dt1.Columns[j].ToString()].Value;
                            }
                            ws.Cell(Cell, j + 1).Value = value;
                        }
                    }
                }
            }
            ws.Rows(1, 1).Style.Font.Bold = true;
            for (var j = 1; j <= columnIndex; j++)
            {
                ws.Column(j).AdjustToContents();
            }
            Session["Export"] = null;
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", TABLE_NAME + ".xlsx");
            }
        }
        #endregion
    }

}