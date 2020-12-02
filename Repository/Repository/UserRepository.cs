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
    public partial class UserRepository : CommonRepository, IUserRepository
    {
        //Get all user
        public ResultModel GetAllUser()
        {
            var param = new List<Param>();
            return ListProcedure<EmployesModel>(new EmployesModel(), "User_Get_AllUser", param);
        }   
    }
}
