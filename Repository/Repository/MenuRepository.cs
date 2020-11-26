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
    public partial class MenuRepository : CommonRepository, IMenuRepository
    {
        //Get all menu
        public ResultModel GetMenu()
        {
            return ListProcedure<MenuModel>(new MenuModel(), "Menu_Get_Menu", new List<Param>(), true);
        }

        //Get menu render to table
        public ResultModel GetMenuFromAdmin(ParamType paramType, bool isCheckPermission = true)
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
            return ListProcedure<MenuModel>(new MenuModel(), "Menu_Get_MenuFromAdmin", param, false, isCheckPermission);
        }

        //Save product group
        public ResultModel SaveMenu(MenuModel model, List<LocalizationType> type, bool isCheckPermission = true)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@MENU_NAME", Value = string.IsNullOrEmpty(model.MENU_NAME) ? " " : model.MENU_NAME });
            param.Add(new Param { Key = "@PRODUCT_GROUP_ID", Value = model.PRODUCT_GROUP_ID.ToString() });
            param.Add(new Param { Key = "@PARENT_ID", Value = model.PARENT_ID.ToString() });
            param.Add(new Param { Key = "@LINK", Value = string.IsNullOrEmpty(model.LINK) ? " " : model.LINK });
            param.Add(new Param { Key = "@LINK_VIEW", Value = string.IsNullOrEmpty(model.LINK_VIEW) ? " " : model.LINK_VIEW });
            param.Add(new Param { Key = "@ORDER_MENU", Value = model.ORDER_MENU.ToString() });
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@LocalizationType", SqlDbType.Structured)
                {
                    TypeName = "dbo.LocalizationType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(type)
                }
            });

            return ListProcedure<MenuModel>(new MenuModel(), "Menu_Update_SaveMenu", param, false, isCheckPermission);
        }

        //Get menu by ID
        public ResultModel GetMenuById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<MenuModel>(new MenuModel(), "Menu_Get_MenuById", param, false,false);
        }

        //Delete menu by Id
        public ResultModel Delete(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<MenuModel>(new MenuModel(), "Menu_Delete_DeleteMenuById", param, false, true);
        }
    }
}
