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
            return ListProcedure<MenuModel>(new MenuModel(), "PermissionRole_Get_PermissionRoleByRoleId", param, true, true);
        }
    }
}
