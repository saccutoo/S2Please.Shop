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
    public partial class PermissionRepository : CommonRepository, IPermissonRepository
    {
        //Get permission role by role id
        public ResultModel GetPermissionRoleByRoleId(long roleId)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ROLE_ID", Value = roleId.ToString() });
            return ListProcedure<MenuPermissionModel>(new MenuPermissionModel(), "PermissionRole_Get_PermissionRoleByRoleId", param);
        }

        //save permission role
        public ResultModel SavePermissionRole(List<MenuPermissionType> types,long roleId)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ROLE_ID", Value = roleId.ToString() });
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@MenuPermissionType", SqlDbType.Structured)
                {
                    TypeName = "dbo.MenuPermissionType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(types)
                }
            });
            return ListProcedure<MenuPermissionModel>(new MenuPermissionModel(), "PermissionRole_Update_SavePermissionRole", param,false,true);
        }

        //Get permission user by role id
        public ResultModel GetPermissionUserByUserId(long userId)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@USER_ID", Value = userId.ToString() });
            return ListProcedure<MenuPermissionModel>(new MenuPermissionModel(), "PermissionUser_Get_PermissionUserByUserId", param);
        }

        //save permission user
        public ResultModel SavePermissionUser(List<MenuPermissionType> types, long userId)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@USER_ID", Value = userId.ToString() });
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@MenuPermissionType", SqlDbType.Structured)
                {
                    TypeName = "dbo.MenuPermissionType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(types)
                }
            });
            return ListProcedure<MenuPermissionModel>(new MenuPermissionModel(), "PermissionUser_Update_SavePermissionUser", param, false, true);
        }
    }
}
