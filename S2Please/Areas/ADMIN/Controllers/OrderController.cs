using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Database;
using S2Please.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using SHOP.COMMON;
using S2Please.Controllers;
using S2Please.Helper;
using System.Web.Security;
using System;
using System.Web;
using S2Please.Areas.ADMIN.ViewModel;
using System.Configuration;
using S2Please.ParramType;
using System.Data.SqlClient;
using System.Data;
using Hrm.Common.Helpers;
using ClosedXML.Excel;
using System.IO;
using SHOP.COMMON.Helpers;
using Repository;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class OrderController : BaseController
    {
        // GET: ADMIN/Order
        public ActionResult Index()
        {

            OrderViewModel order = new OrderViewModel();
            var param = new List<Param>();

            param.Add(new Param() { Key = "@TABLE_NAME", Value = TableName.Order });
            param.Add(new Param() { Key = "@USER_ID", Value = CurrentUser.UserAdmin.USER_ID.ToString() });

            var responseTableUser = ListProcedure<TableUserConfigModel>(new TableUserConfigModel(), "Table_Get_TableUserConfig", param, false, false);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                order.Table = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                param = new List<Param>();
                param.Add(new Param() { Key = "@TABLE_NAME", Value = TableName.Order });
                var responseOrder = ListProcedure<TableModel>(new TableModel(), "Table_Get_Table", param, false, false);
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

        public ActionResult Detail(long id)
        {
            OrderDetailViewModel vm = new OrderDetailViewModel();

            //Lấy sản phẩm ID sản phẩm
            var param = new List<Param>();
            vm.ID = id;

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

            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Order/_Detail.cshtml", vm);
            return Content(JsonConvert.SerializeObject(new
            {
                html
            }));
        }
        #region RenderTable
        public ActionResult ReloadTable(TableViewModel tableData, ParamType param)
        {
            var paramType = new List<Param>();
            if (param.STRING_FILTER == null)
            {
                param.STRING_FILTER = string.Empty;
            }

            paramType.Add(new Param() { Key = "@TABLE_NAME", Value = TableName.Order });
            paramType.Add(new Param() { Key = "@USER_ID", Value = CurrentUser.UserAdmin.USER_ID.ToString() });

            var responseTableUser = ListProcedure<TableUserConfigModel>(new TableUserConfigModel(), "Table_Get_TableUserConfig", paramType, false, false);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                tableData = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                paramType = new List<Param>();
                paramType.Add(new Param() { Key = "@TABLE_NAME", Value = TableName.Order });
                var responseOrder = ListProcedure<TableModel>(new TableModel(), "Table_Get_Table", paramType, false, false);
                var resultOrder = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                if (resultOrder != null && resultOrder.Count() > 0)
                {
                    tableData = JsonConvert.DeserializeObject<TableViewModel>(resultOrder.FirstOrDefault().TABLE_CONTENT);
                }
            }


            tableData.TABLE_NAME = TableName.Order;
            tableData.TITLE_TABLE_NAME = FunctionHelpers.GetValueLanguage("Table.Title.Order");
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
            tableData.TABLE_URL = TableUrl.Order;
            tableData.MENU_NAME = MenuName.Order;
            tableData.PAGE_SIZE = paramType.PAGE_SIZE;
            tableData.PAGE_INDEX = paramType.PAGE_NUMBER;
            tableData.PAGE_SIZE = paramType.PAGE_SIZE;
            tableData.PAGE_INDEX = paramType.PAGE_NUMBER;
            tableData.TABLE_COLUMN_FIELD = tableData.TABLE_COLUMN_FIELD.Where(s => s.IS_SHOW == true).OrderBy(s => s.POSITION).ToList();
            tableData.VALUE_DYNAMIC = paramType.VALUE;
            tableData.STRING_FILTER = paramType.STRING_FILTER;
            tableData.TABLE_EXPORT_URL = TableExportUrl.Order;
            tableData.TABLE_SESION_EXPORT_URL = TableSesionExportUrl.Order;

            if (tableData.TABLE_COLUMN == null || tableData.TABLE_COLUMN.Count() == 0)
            {
                var param = new List<Param>();
                param.Add(new Param() { Key = "@TABLE_NAME", Value = tableData.TABLE_NAME });
                var responseColumn = ListProcedure<TableColumnModel>(new TableColumnModel(), "TableColumn_Get_TableColumnByTableName", param, false, false);
                if (responseColumn != null && responseColumn.Results.Count() > 0)
                {
                    tableData.TABLE_COLUMN = JsonConvert.DeserializeObject<List<TableColumnModel>>(JsonConvert.SerializeObject(responseColumn.Results));
                }
            }


            if (tableData.SELECT_OPTION == null || tableData.SELECT_OPTION.Count() == 0)
            {
                var param = new List<Param>();
                var responseSelectOption = ListProcedure<SelectOptionModel>(new SelectOptionModel(), "SelectOption_Get_SelectOption ", param, false, false);
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

            var param = new List<Param>();
            var basicParamype = new List<ParamType>();
            basicParamype.Add(paramType);
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamype)
                }
            });
            param.Add(new Param { Key = "@TotalRecord", Value = "0", IsOutPut = true, Type = "Int" });

            var response = ListProcedure<OrderModel>(new OrderModel(), "Order_Get_OrderFromAdmin", param, false, true);
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

            return tableData;
        }
        public ActionResult SesionExport(string TABLE_NAME, ParamType paramType)
        {
            if (String.IsNullOrEmpty(paramType.STRING_FILTER))
            {
                paramType.STRING_FILTER = string.Empty;
            }
            TableViewModel tableData = new TableViewModel();
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
            var param = new List<Param>();

            param.Add(new Param() { Key = "@TABLE_NAME", Value = tableData.TABLE_NAME });
            param.Add(new Param() { Key = "@USER_ID", Value = CurrentUser.UserAdmin.USER_ID.ToString() });

            var responseTableUser = ListProcedure<TableUserConfigModel>(new TableUserConfigModel(), "Table_Get_TableUserConfig", param, false, false);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                tableData = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                param = new List<Param>();
                param.Add(new Param() { Key = "@TABLE_NAME", Value = TABLE_NAME });
                var responseOrder = ListProcedure<TableModel>(new TableModel(), "Table_Get_Table", param, false, false);
                var resultOrder= JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                if (resultOrder != null && resultOrder.Count() > 0)
                {
                    tableData = JsonConvert.DeserializeObject<TableViewModel>(resultOrder.FirstOrDefault().TABLE_CONTENT);
                }
            }

            //tableData = GetData(tableData, paramType);
            tableData.TABLE_COLUMN_FIELD = tableData.TABLE_COLUMN_FIELD.Where(s => s.IS_SHOW == true).OrderBy(s => s.POSITION).ToList();
            tableData.DATA = Session["Export"] as List<dynamic>;

            param = new List<Param>();
            param.Add(new Param() { Key = "@TABLE_NAME", Value = TABLE_NAME });
            var responseColumn = ListProcedure<TableColumnModel>(new TableColumnModel(), "TableColumn_Get_TableColumnByTableName", param, false, false);
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