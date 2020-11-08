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
    public partial interface IAuthenRepository
    {
        //Get user
        ResultModel GetUser(string userName, string passWord, string isEmployee);

        //Get User By Email
        ResultModel GetUserByEmail(string email, string code, string isEmployee);

        //Get_History_Reset_Pass
        ResultModel GetHistoryResetPass(string code);

        //Get_History_Reset_Pass
        ResultModel ChangePasswordEmployees(string pass, string email);

        //check exits usernam and email
        ResultModel CheckUserNameEmail(string userName, string email);

        //update pass word user
        ResultModel UpdateUser(string userName, string password, string email);

        //update change pass word customer
        ResultModel ChangePasswordCustomer(string pass, string email);
    }
}
