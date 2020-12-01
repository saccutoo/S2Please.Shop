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
    public partial interface IPermissonRepository
    {
        //Get permission role by role id
        ResultModel GetPermissionRoleByRoleId(long roleId);
    }
}
