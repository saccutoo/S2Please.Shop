﻿using System.Collections.Generic;
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
    public class MenuAdminController : BaseController
    {
        // GET: ADMIN/Menu

        private ITableRepository _tableRepository;
        private ISystemRepository _systemRepository;
        private IMenuAdminRepository _menuAdminRepositor;
        public MenuAdminController(ITableRepository tableRepository, IOrderRepository orderRepository, ISystemRepository systemRepository, IMenuAdminRepository menuAdminRepositor)
        {
            this._tableRepository = tableRepository;
            this._systemRepository = systemRepository;
            this._menuAdminRepositor = menuAdminRepositor;
        }

        public ActionResult Index()
        {
            MenuAdminViewModel menu = new MenuAdminViewModel();
            var responseTableUser = _tableRepository.GetTableUserConfig(TableName.MenuAdmin, CurrentUser.UserAdmin.USER_ID);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                menu.Table = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                var responseOrder = _tableRepository.GetTableByTableName(TableName.MenuAdmin);
                var resultOrder = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                if (resultOrder != null && resultOrder.Count() > 0)
                {
                    menu.Table = JsonConvert.DeserializeObject<TableViewModel>(resultOrder.FirstOrDefault().TABLE_CONTENT);
                }
            }

            ParamType paramType = new ParamType();
            paramType.PAGE_SIZE = menu.Table.PAGE_SIZE == 0 ? 1 : menu.Table.PAGE_SIZE;
            paramType.PAGE_NUMBER = menu.Table.PAGE_INDEX == 0 ? 1 : menu.Table.PAGE_INDEX;
            menu.Table.TABLE_NAME = TableName.MenuAdmin;
            menu.Table.TITLE_TABLE_NAME = FunctionHelpers.GetValueLanguage("Table.Title.MenuAdmin");
            menu.Table = RenderTable(menu.Table, paramType);
            if (!menu.Table.IS_PERMISSION)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            //Table common
            return View(menu);
        }

        public ActionResult ShowFormSaveMenuAdmin(long id = 0, bool isUpdate = true)
        {
            MenuAdminViewModel vm = new MenuAdminViewModel();
            vm.Localiza.DATA_TYPE = DataType.MENU_ADMIN;
            vm.Is_Save = isUpdate;
            vm.Localiza.Is_Save = isUpdate;
            vm.PermissionAction.MENU_ID = id;

            var responseMenu = _menuAdminRepositor.GetAllMenuAdmin(id);
            vm.MenuDropdowns = responseMenu.Results;

            var responsePermissionAction = _systemRepository.GetAllPermissionAction(id);
            var resultPermissionAction = JsonConvert.DeserializeObject<List<PermissionActionModel>>(JsonConvert.SerializeObject(responsePermissionAction.Results));
            if (resultPermissionAction != null && resultPermissionAction.Count() > 0)
            {
                vm.PermissionAction.PermissionActions = resultPermissionAction;
            }

            var responseMultipleLanguage = _systemRepository.GetTableMultipleLanguageConfigurationByTableName(TableName.MenuAdmin);
            var resultMultipleLanguage = JsonConvert.DeserializeObject<List<TableMultipleLanguageConfigurationModel>>(JsonConvert.SerializeObject(responseMultipleLanguage.Results));
            if (resultMultipleLanguage != null && resultMultipleLanguage.Count() > 0)
            {
                vm.Localiza.MultipleLanguageConfigurations = resultMultipleLanguage;
            }

            var responseLanguage = _systemRepository.GetLanguage();
            var resultLanguage = JsonConvert.DeserializeObject<List<LanguageModel>>(JsonConvert.SerializeObject(responseLanguage.Results));
            if (resultLanguage != null && resultLanguage.Count() > 0)
            {
                vm.Localiza.Languages = resultLanguage;
            }

            if (id != 0)
            {
                vm.Localiza.DATA_ID = id;
                var responseMenuById = _menuAdminRepositor.GetMenuAdminById(id);
                var resultMenuById = JsonConvert.DeserializeObject<List<MenuModel>>(JsonConvert.SerializeObject(responseMenuById.Results));
                if (resultMenuById != null && resultMenuById.Count() > 0)
                {
                    vm.Menu = resultMenuById.FirstOrDefault();
                }
                var responseLocalizadata = _systemRepository.GetLocalizationByDataIdAndDataType(id, vm.Localiza.DATA_TYPE);
                var resultLocalizadata = JsonConvert.DeserializeObject<List<LocalizationModel>>(JsonConvert.SerializeObject(responseLocalizadata.Results));
                if (resultLocalizadata != null && resultLocalizadata.Count() > 0)
                {
                    vm.Localiza.Localizations = resultLocalizadata;
                }
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/MenuAdmin/_FormSaveMenuAdmin.cshtml", vm);
            return Content(JsonConvert.SerializeObject(new
            {
                html
            }));
        }

        public ActionResult SaveMenuAdmin(MenuAdminModel model, List<LocalizationModel> localiza, List<MenuPermissionModel> menuPermission)
        {
            var validations = ValidationHelper.Validation(model, "model");
            var validationLocaliza = ValidationHelper.ListValidation(localiza, "localiza");
            var allValidations = new List<ValidationModel>(validations.Count +
                                    validationLocaliza.Count);
            allValidations.AddRange(validations);
            allValidations.AddRange(validationLocaliza);

            if (allValidations.Count > 0)
            {
                return Json(new { Result = allValidations, Invalid = true }, JsonRequestBehavior.AllowGet);
            }
            model.CREATED_BY = CurrentUser.UserAdmin.ID;
            model.UPDATED_BY = CurrentUser.UserAdmin.ID;
            var modelType = MapperHelper.Map<MenuAdminModel, Repository.Model.MenuModel>(model);
            var localizaType = MapperHelper.MapList<LocalizationModel, Repository.Type.LocalizationType>(localiza);
            var permissionType = MapperHelper.MapList<MenuPermissionModel, Repository.Type.PermissionType>(menuPermission.Where(s=>s.ID!=0).ToList());

            var result = new ResultModel();
            var response = _menuAdminRepositor.SaveMenuAdmin(modelType, localizaType, permissionType);
            if (response != null)
            {
                if (response.Success == false && CheckPermision(response.StatusCode) == false)
                {
                    result.SetUrl("/Base/Page404");
                    return Content(JsonConvert.SerializeObject(new
                    {
                        result
                    }));
                }
                else
                {
                    var resultProductType = JsonConvert.DeserializeObject<List<ProductTypeModel>>(JsonConvert.SerializeObject(response.Results));
                    if (resultProductType != null && resultProductType.Count() > 0)
                    {
                        FunctionHelpers.InsertTimeVersion();
                        if (model.ID == 0)
                        {
                            result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.AddNewSuccess"), FunctionHelpers.GetValueLanguage("Message.Success"), string.Empty);
                        }
                        else
                        {
                            result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.UpdateSuccess"), FunctionHelpers.GetValueLanguage("Message.Success"), string.Empty);
                        }
                    }
                    else
                    {
                        if (model.ID == 0)
                        {
                            result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.AddNew.Error"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);

                        }
                        else
                        {
                            result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.UpdateFail"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                        }
                    }

                }
            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult Delete(long id = 0)
        {
            ResultModel result = new ResultModel();
            var response = _menuAdminRepositor.DeleteMenuAdminById(id);
            if (response.Success == false && CheckPermision(response.StatusCode) == false)
            {
                result.SetUrl("/Base/Page404");
            }
            else
            {
                FunctionHelpers.RemoveCacheByProcedure("Localization_Get_Localization;Menu");
                result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.Remove.Sucess"), FunctionHelpers.GetValueLanguage("Message.Success"));
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
            if (tableName == TableName.MenuAdmin)
            {
                tableData.TITLE_TABLE_NAME = FunctionHelpers.GetValueLanguage("Table.Title.MenuAdmin");
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

            if (tableData.TABLE_NAME == TableName.MenuAdmin)
            {
                tableData.TABLE_URL = TableUrl.MenuAdmin;
                tableData.MENU_NAME = MenuName.MenuAdmin;
                tableData.TABLE_EXPORT_URL = TableExportUrl.MenuAdmin;
                tableData.TABLE_SESION_EXPORT_URL = TableSesionExportUrl.MenuAdmin;
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
            if (tableData.TABLE_NAME == TableName.MenuAdmin)
            {
                var response = _menuAdminRepositor.GetMenuAdminFromAdmin(type);
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


        #region get menu left
        public ActionResult GetMenuLeftAdmin()
        {
            MenuAdminViewModel menu = new MenuAdminViewModel();
            var responseMenus = _menuAdminRepositor.GetMenuLeftAdmin(CurrentUser.UserAdmin.USER_ID, CurrentUser.UserAdmin.ROLE_ID);
            var resultMenus= JsonConvert.DeserializeObject<List<MenuModel>>(JsonConvert.SerializeObject(responseMenus.Results));
            if (resultMenus != null && resultMenus.Count() > 0)
            {
                menu.Menus = resultMenus;
            }
            return View(menu);
        }

        public ActionResult GetMenuLeftMobileAdmin()
        {
            MenuAdminViewModel menu = new MenuAdminViewModel();
            var responseMenus = _menuAdminRepositor.GetMenuLeftAdmin(CurrentUser.UserAdmin.USER_ID, CurrentUser.UserAdmin.ROLE_ID);
            var resultMenus = JsonConvert.DeserializeObject<List<MenuModel>>(JsonConvert.SerializeObject(responseMenus.Results));
            if (resultMenus != null && resultMenus.Count() > 0)
            {
                menu.Menus = resultMenus;
            }
            return View(menu);
        }
        #endregion
    }

}