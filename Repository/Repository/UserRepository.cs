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

        //Get all user
        public ResultModel GetUserByUserId(long userId)
        {
            var param = new List<Param>();
            param.Add(new Param() { Key = "@USER_ID", Value = userId.ToString() });
            return ListProcedure<UserModel>(new UserModel(), "User_Get_UserByUserId", param,true,false);
        }
    }
}
