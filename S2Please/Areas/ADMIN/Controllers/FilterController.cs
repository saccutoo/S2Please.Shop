using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using SHOP.COMMON;
using S2Please.Controllers;
using System;
using S2Please.Areas.ADMIN.ViewModel;
using System.Data;
using S2Please.ViewModel;
using Repository;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class FilterController : BaseController
    {
        // GET: ADMIN/Exec
        private ISystemRepository _systemRepository;
        private ITableRepository _tableRepository;
        public FilterController(ISystemRepository systemRepository, ITableRepository tableRepository)
        {
            this._systemRepository = systemRepository;
            this._tableRepository = tableRepository;
        }
        // GET: ADMIN/Filter
        public ActionResult ShowModalColumn(string type, string tableName)
        {
            FilterViewModel vm = new FilterViewModel();
            vm.IsMoblie = Request.Browser.IsMobileDevice;
            vm.Type = type;
            vm.TableName = tableName;
            var table = new TableViewModel();
            var responseTableUser = _tableRepository.GetTableUserConfig(tableName, CurrentUser.UserAdmin.USER_ID);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableUserConfigModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                table = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
                vm.ColumnFilters= JsonConvert.DeserializeObject<List<ListColumnFilterModel>>(resultTableUser.FirstOrDefault().PARAM);
            }

            var responseColumn = _tableRepository.GetTableColumnByTableName(tableName);
            var resultColumn = JsonConvert.DeserializeObject<List<TableColumnModel>>(JsonConvert.SerializeObject(responseColumn.Results));
            if (resultColumn != null && resultColumn.Count() > 0)
            {
                vm.Columns = resultColumn;
            }

            if (table.TABLE_COLUMN_FIELD!=null && table.TABLE_COLUMN_FIELD.Count()>0)
            {
                foreach (var item in table.TABLE_COLUMN_FIELD)
                {
                    var data = vm.Columns.Where(s => s.COLUMN_NAME == item.COLUMN_NAME).FirstOrDefault();
                    if (data!=null)
                    {
                        vm.Columns.Where(s => s.COLUMN_NAME == item.COLUMN_NAME).FirstOrDefault().IS_CHECK = item.IS_CHECK;
                        vm.Columns.Where(s => s.COLUMN_NAME == item.COLUMN_NAME).FirstOrDefault().ORDER = item.POSITION;
                        if (type=="0")
                        {
                            vm.Columns.Where(s => s.COLUMN_NAME == item.COLUMN_NAME).FirstOrDefault().IS_SHOW = item.IS_SHOW;

                        }
                    }
                }
            }

            var responseCondition = _systemRepository.GetConditionFilter();
            var resultCondition = JsonConvert.DeserializeObject<List<ConditionFilterModel>>(JsonConvert.SerializeObject(responseCondition.Results));
            if (resultCondition != null && resultCondition.Count() > 0)
            {
                vm.Conditions = resultCondition;
            }

            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/_showModalColumn.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterSelector(TableColumnModel column)
        {
            if (column.COLUMN_TYPE == 4)
            {
                var model = new SelectModel()
                {
                    Id = "input-value-" + column.ID,
                    Name = "select-name" + column.ID,
                    IsMultiLanguage = true,
                    FieldId = "ID",
                    PropertyName = column.PROPERTY_NAME,
                    DataType = column.DATA_TYPE,
                    Value=column.VALUE,
                    DropdownParent = "#modal-right",
                };

                if (!String.IsNullOrEmpty(column.SQL_QUERY))
                {
                    var responseQuery = _systemRepository.GetExcuteSqlString(column.SQL_QUERY);
                    model.Datas = responseQuery.Results;
                }
                return PartialView("~/Areas/ADMIN/Views/Template/_Select.cshtml", model);
            }
            else if (column.COLUMN_TYPE == 3)
            {
                var model = new InputModel()
                {
                    Id = "input-value-" + column.ID,
                    Name = "condition-name-input-date" + column.ID,
                    Class = "datepicker-input",
                    IsSetDate = true,
                    Value = column.VALUE

                };
                return PartialView("~/Areas/ADMIN/Views/Template/_DatePicker.cshtml", model);
            }
            else if (column.COLUMN_TYPE == 1 || column.COLUMN_TYPE == 2)
            {
                var model = new InputModel()
                {
                    Id = "input-value-" + column.ID,
                    Name = "condition-name-input-multi" + column.ID,
                    Class = "",
                    Value = column.VALUE

                };
                return PartialView("~/Areas/ADMIN/Views/Template/_InputMulti.cshtml", model);
            }

            return null;
        }

        public ActionResult SaveTable(List<ListColumnFilterModel> data, string type, string tableName)
        {
            var paramType = new List<Param>();
            ProductViewModel product = new ProductViewModel();
            var responseTableUser = _tableRepository.GetTableUserConfig(tableName, CurrentUser.UserAdmin.USER_ID);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableUserConfigModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                product.Table = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);               
            }
            else
            {
                var responseTable = _tableRepository.GetTableByTableName(tableName);
                var resultTable = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTable.Results));
                if (resultTable != null && resultTable.Count() > 0)
                {
                    product.Table = JsonConvert.DeserializeObject<TableViewModel>(resultTable.FirstOrDefault().TABLE_CONTENT);                  
                }
            }
            if (type == "0")
            {
                foreach (var item in product.Table.TABLE_COLUMN_FIELD)
                {
                    if (item.PRESENTATION== "CHECK_BOX" )
                    {
                        item.IS_SHOW = true;
                    }
                    else 
                    {
                        item.IS_SHOW = false;
                    }
                }
            }
            if (type == "1")
            {
                foreach (var item in product.Table.TABLE_COLUMN_FIELD)
                {
                    item.IS_CHECK = false;
                }
            }
            List<TableColumnModel> columns = new List<TableColumnModel>();
            var responseColumn = _tableRepository.GetTableColumnByTableName(tableName);
            var resultColumn = JsonConvert.DeserializeObject<List<TableColumnModel>>(JsonConvert.SerializeObject(responseColumn.Results));
            if (resultColumn != null && resultColumn.Count() > 0)
            {
                columns = resultColumn;             
            }

            paramType = new List<Param>();
            List<ConditionFilterModel> conditions = new List<ConditionFilterModel>();
            var responseCondition = _systemRepository.GetConditionFilter();
            var resultCondition = JsonConvert.DeserializeObject<List<ConditionFilterModel>>(JsonConvert.SerializeObject(responseCondition.Results));
            if (resultCondition != null && resultCondition.Count() > 0)
            {
                conditions = resultCondition;
            }

            foreach (var item in data)
            {
                var column = columns.Where(s => s.ID == item.COLUMN_ID).FirstOrDefault();
                if (column!=null)
                {
                    if (type == "1")
                    {
                        product.Table.TABLE_COLUMN_FIELD.Where(s => s.COLUMN_NAME == column.COLUMN_NAME).FirstOrDefault().IS_CHECK = true;
                    }
                    else
                    {
                        product.Table.TABLE_COLUMN_FIELD.Where(s => s.COLUMN_NAME == column.COLUMN_NAME).FirstOrDefault().IS_SHOW = item.IS_SHOW.Value;
                        product.Table.TABLE_COLUMN_FIELD.Where(s => s.COLUMN_NAME == column.COLUMN_NAME).FirstOrDefault().POSITION = item.POSITION.Value;

                    }
                }
               
            }

            #region lưu table theo người đăng nhập
            string parram = string.Empty;
            string content = string.Empty;

            if (type=="1")
            {
                if (data != null && data.Count() > 0)
                {
                    var listParam = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(data));
                    if (listParam != null)
                    {
                        parram = JsonConvert.SerializeObject(listParam);
                    }
                }         
            }
            else
            {
                if (resultTableUser!=null && resultTableUser.Count()>0)
                {
                    parram = resultTableUser.FirstOrDefault().PARAM;
                }
                else
                {
                    if (data != null && data.Count() > 0)
                    {
                        var listParam = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(data));
                        if (listParam != null)
                        {
                            parram = JsonConvert.SerializeObject(listParam);
                        }
                    }
                }
            }

            if (product.Table != null)
            {
                var listParam = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(product.Table));
                if (listParam != null)
                {
                    content = JsonConvert.SerializeObject(listParam);
                }
            }

            var responsUpdate = _tableRepository.UpdateTableUserConfig(tableName, CurrentUser.UserAdmin.USER_ID, content, parram);
            var resultUpdate = JsonConvert.DeserializeObject<List<ConditionFilterModel>>(JsonConvert.SerializeObject(responseCondition.Results));
            #endregion

            string filterString = string.Empty;
            if (type=="1")
            {
                #region Tạo chuỗi filter
                List<string> filterStrings = new List<string>();
                if (data != null && data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        var column = columns.Where(s => s.ID == item.COLUMN_ID).FirstOrDefault();
                        var condition = conditions.Where(s => s.ID == item.CONDITION_ID).FirstOrDefault();
                        if (column != null && condition != null)
                        {
                            string conditiontring = condition.CONDITION;
                            string newConditiontring = string.Empty;
                            if (column.COLUMN_TYPE == 1 || column.COLUMN_TYPE == 2)
                            {
                                string conditionTags = string.Empty;
                                if (String.IsNullOrEmpty(item.VALUE))
                                {
                                    item.VALUE = "";
                                }
                                var listValue = item.VALUE.Split(',');
                                if (listValue != null)
                                {
                                    for (int i = 0; i < listValue.Length; i++)
                                    {
                                        if (!String.IsNullOrEmpty(listValue[i]))
                                        {
                                            var columnNameFilter = string.Empty;
                                            if (!String.IsNullOrEmpty(column.ORIGINAL_COLUMN_ALIAS))
                                            {
                                                columnNameFilter = column.ORIGINAL_COLUMN_ALIAS + "." + column.ORIGINAL_COLUMN_NAME;
                                            }
                                            else
                                            {
                                                columnNameFilter = column.ORIGINAL_COLUMN_NAME;
                                            }
                                            conditionTags = String.Format(conditiontring, columnNameFilter, listValue[i]);
                                            if (!String.IsNullOrEmpty(newConditiontring))
                                            {
                                                newConditiontring += " OR " + conditionTags;
                                            }
                                            else
                                            {
                                                newConditiontring = conditionTags;
                                            }
                                        }
                                    }
                                    if (!String.IsNullOrEmpty(newConditiontring))
                                    {
                                        filterStrings.Add("(" + newConditiontring + ")");
                                    }
                                }
                            }
                            else if (column.COLUMN_TYPE == 4 || column.COLUMN_TYPE == 3)
                            {
                                newConditiontring = string.Empty;
                                var value = item.VALUE;
                                var columnNameFilter = string.Empty;
                                if (!String.IsNullOrEmpty(column.ORIGINAL_COLUMN_ALIAS))
                                {
                                    columnNameFilter = column.ORIGINAL_COLUMN_ALIAS + "." + column.ORIGINAL_COLUMN_NAME;
                                }
                                else
                                {
                                    columnNameFilter = column.ORIGINAL_COLUMN_NAME;
                                }
                                if (!String.IsNullOrEmpty(value))
                                {
                                    newConditiontring = String.Format(conditiontring, columnNameFilter, value);
                                    if (!String.IsNullOrEmpty(newConditiontring))
                                    {
                                        filterStrings.Add(newConditiontring);
                                    }
                                }
                            }
                        }
                    }
                    if (filterStrings != null && filterStrings.Count() > 0)
                    {
                        filterString = " AND " + String.Join(" AND ", filterStrings);
                    }

                }
                #endregion
            }

            return Json(new { result = filterString }, JsonRequestBehavior.AllowGet);

        }

    }
}