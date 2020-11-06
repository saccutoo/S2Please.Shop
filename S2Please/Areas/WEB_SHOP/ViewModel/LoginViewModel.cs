using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class LoginViewModel :BaseModel
    {
        public bool Success { get; set; } = true;
        public bool Is_Create { get; set; }
        public bool Is_Login { get; set; } = true;
        public bool Is_Error_User_Name_Required { get; set; } = false;
        public bool Is_Error_User_Name_Exist { get; set; } = false;
        public bool Is_Error_Email_Required { get; set; } = false;
        public bool IS_Error_Format_Email { get; set; } = false;
        public bool Is_Error_User_Email_Exist { get; set; } = false;
        public bool IS_Error_Password_Required { get; set; } = false;
        public bool IS_Error_Password_Length{ get; set; } = false;
        public bool IS_Error_Password_Required_Again { get; set; } = false;
        public bool IS_Error_Password_Not_Match_Again { get; set; } = false;
        public string Message { get; set; }
        public bool Is_Confirm { get; set; } = false;
        public string Code { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Is_Expired { get; set; } = false;
        public bool Is_ChangePass { get; set; } = false;

        public UserModel User { get; set; } = new UserModel();

    }
}