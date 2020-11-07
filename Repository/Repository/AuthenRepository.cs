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
    public partial class AuthenRepository : CommonRepository, IAuthenRepository
    {
        //Get user
        public ResultModel GetUser(string userName,string passWord,string isEmployee)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@USER_NAME", Value = userName });
            param.Add(new Param { Key = "@PASS_WORD", Value = passWord });
            param.Add(new Param { Key = "@IS_EMPLOYEE", Value = isEmployee });
            return ListProcedure<UserModel>(new UserModel(), "User_Get_User", param);
        }

        //Get User By Email
        public ResultModel GetUserByEmail(string email, string code, string isEmployee)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@EMAIL", Value = email });
            param.Add(new Param { Key = "@CODE", Value = code });
            param.Add(new Param { Key = "@IS_EMPLOYEE", Value = isEmployee });
            param.Add(new Param { Key = "@DATE_TIME", Value = DateTime.Now.AddMinutes(15).ToString() });
            return ListProcedure<UserModel>(new UserModel(), "User_Get_UserByEmail", param);
        }

        //Get_History_Reset_Pass
        public ResultModel GetHistoryResetPass(string code)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@CODE", Value = code });
            return ListProcedure<HistoryResetPassword>(new HistoryResetPassword(), "Get_History_Reset_Pass", param);
        }

        //Get_History_Reset_Pass
        public ResultModel ChangePasswordEmployees(string pass,string email )
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@PASS_WORD", Value = pass });
            param.Add(new Param { Key = "@EMAIL", Value = email });
            return ListProcedure<UserModel>(new UserModel(), "User_Update_ChangePasswordEmployees", param);
        }
    }
}
