using Repository.Model;
using Repository.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHOP.COMMON.Helpers;
namespace Repository
{
    public partial class MenuAdminRepository : CommonRepository, IMenuAdminRepository
    {
        //Get menu admin render to table
        public ResultModel GetMenuAdminFromAdmin(ParamType paramType, bool isCheckPermission = true)
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
            return ListProcedure<MenuModel>(new MenuModel(), "MenuAdmin_Get_MenuAdminFromAdmin", param, false, isCheckPermission);
        }

        //Get menu left admin
        public ResultModel GetMenuLeftAdmin(long userId,long roleId)
        {
            var param = new List<Param>();        
            param.Add(new Param { Key = "@USER_ID", Value = userId.ToString() });
            param.Add(new Param { Key = "@ROLE_ID", Value = roleId.ToString() });
            return ListProcedure<MenuModel>(new MenuModel(), "MenuAdmin_Get_MenuLeftAdmin", param, true, false);
        }

        //Get all menu admin
        public ResultModel GetAllMenuAdmin(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<MenuModel>(new MenuModel(), "MenuAdmin_Get_GetAllMenuAdmin", param, true, false);
        }

        //Get menu admin by id
        public ResultModel GetMenuAdminById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<MenuModel>(new MenuModel(), "MenuAdmin_Get_MenuAdminById", param, true, false);
        }

        //Save menu admin
        public ResultModel SaveMenuAdmin(MenuModel model, List<LocalizationType> type,List<PermissionType> permission, bool isCheckPermission = true)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@MENU_NAME", Value = string.IsNullOrEmpty(model.MENU_NAME) ? " " : model.MENU_NAME });
            param.Add(new Param { Key = "@PARENT_ID", Value = model.PARENT_ID.ToString() });
            param.Add(new Param { Key = "@ICON", Value = string.IsNullOrEmpty(model.ICON) ? " " : model.ICON });
            param.Add(new Param { Key = "@LINK", Value = string.IsNullOrEmpty(model.LINK) ? " " : model.LINK });
            param.Add(new Param { Key = "@LINK_VIEW", Value = string.IsNullOrEmpty(model.LINK_VIEW) ? " " : model.LINK_VIEW });
            param.Add(new Param { Key = "@IS_SHOW_VIEW_MENU", Value =model.IS_SHOW_VIEW_MENU.ToString() });
            param.Add(new Param { Key = "@ORDER_MENU", Value = model.ORDER_MENU.ToString() });
            param.Add(new Param { Key = "@ACTIVE", Value = string.IsNullOrEmpty(model.ACTIVE) ? " " : model.ACTIVE });

            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@LocalizationType", SqlDbType.Structured)
                {
                    TypeName = "dbo.LocalizationType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(type)
                }
            });

            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@PermissionType", SqlDbType.Structured)
                {
                    TypeName = "dbo.PermissionType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(permission)
                }
            });

            return ListProcedure<MenuModel>(new MenuModel(), "MenuAdmin_Update_SaveMenuAdmin", param, false, isCheckPermission);
        }


        //Get menu admin by id
        public ResultModel DeleteMenuAdminById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<MenuModel>(new MenuModel(), "MenuAdmin_Delete_MenuAdminById", param, false, true);
        }
    }
}
