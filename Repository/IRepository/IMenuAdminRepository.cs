using Repository.Model;
using Repository.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public partial interface IMenuAdminRepository
    {
        //Get menu admin render to table
        ResultModel GetMenuAdminFromAdmin(ParamType paramType, bool isCheckPermission = true);

        //Get menu left admin
        ResultModel GetMenuLeftAdmin(long userId, long roleId);
    }
}
