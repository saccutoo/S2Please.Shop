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

        public ActionResult Index()
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

        public ActionResult Detail(long id=0)
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

        public ActionResult ViewPriceDetail(long colorId=0, long sizeId=0, long productId=0)
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

        public ActionResult OrderSave(long id=0)
        {
            if (Session["CART_ORDER"]!=null)
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
            return View(vm);
        }

        public ActionResult ShowFormAddProductToCart(long productId=0)
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
            if (resultMapper != null && resultMapper.Count()>0)
            {
                if (vm.Carts != null && vm.Carts.Count()>0)
                {
                    var product = vm.Carts.Where(s => s.PRODUCT_ID == productId && s.COLOR_ID == colorId && sizeId == s.SIZE_ID).FirstOrDefault();
                    if (product!=null)
                    {
                        if ((product.AMOUNT+1)> resultMapper.FirstOrDefault().AMOUNT)
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
                            vm.Carts.Where(s => s.PRODUCT_ID == productId && s.COLOR_ID == colorId && sizeId == s.SIZE_ID).FirstOrDefault().AMOUNT+=1;
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

        public ActionResult UpdateCartAll(List<CartModel> model)
        {
            OrderSaveViewModel vm = new OrderSaveViewModel();
            ResultModel result = new ResultModel();
            var html = string.Empty;
            if (Session["CART_ORDER"] != null)
            {
                vm.Carts = Session["CART_ORDER"] as List<CartModel>;
            }
            if (model!=null && model.Count()>0)
            {
                foreach (var item in model)
                {
                    var product = vm.Carts.Where(s => s.PRODUCT_ID == item.PRODUCT_ID && s.COLOR_ID == item.COLOR_ID && item.SIZE_ID == s.SIZE_ID).FirstOrDefault();
                    if (item.IS_CHECK)
                    {
                        vm.Carts.Remove(product);
                        continue;
                    }                    
                    else if (product!=null)
                    {
                        //Lấy bản ghi mapper giữa size và color
                        var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(item.COLOR_ID, item.SIZE_ID, item.PRODUCT_ID, "0");
                        var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
                        if (item.AMOUNT> resultMapper.FirstOrDefault().AMOUNT)
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
                            vm.Carts.Where(s => s.PRODUCT_ID == item.PRODUCT_ID && s.COLOR_ID == item.COLOR_ID && item.SIZE_ID == s.SIZE_ID).FirstOrDefault().AMOUNT=item.AMOUNT;
                        }
                    }
                }
            }
            else if (vm.Carts==null || vm.Carts.Count==0 || model==null || model.Count==0)
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
            if (tableName==TableName.Product)
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
            if (tableData.TABLE_NAME==TableName.ProductOrder)
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
            if (tableData.TABLE_NAME==TableName.Order)
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
                var response = _productRepository.ProductFromAdmin(type,false);
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
                    var value = string.Empty;
                    Cell = Cell + 1;
                    foreach (var item in dt1.Columns)
                    {

                        for (var j = 0; j < dt1.Columns.Count; j++)
                        {
                            if (tableData.DATA[i][dt1.Columns[j].ToString()] != null && tableData.DATA[i][dt1.Columns[j].ToString()].Value != null)
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