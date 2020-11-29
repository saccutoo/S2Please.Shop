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
    }
}
