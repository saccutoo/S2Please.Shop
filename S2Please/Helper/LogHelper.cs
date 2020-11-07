using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SHOP.COMMON;
using System.Text;
using System.Security.Cryptography;
using S2Please.Controllers;
using S2Please.Models;
using Newtonsoft.Json;
using System.Web.Mvc;
using SHOP.COMMON.Entity;
using SHOP.COMMON.Helpers;
using Repository;

namespace S2Please.Helper
{
    public static class LogHelper
    {
        public static CommonRepository _commonRepository = new CommonRepository();
        public static void LogError(string msg,string procedure)
        {
            var modelError = new ErrorModel();
            var param1 = new List<Param>();
            param1.Add(new Param { Key = "@ERROR_NUM", Value = new Random().Next(100000000, 999999999).ToString() });
            param1.Add(new Param { Key = "@ERROR_MSG", Value = msg });
            param1.Add(new Param { Key = "@ERROR_PROC", Value = procedure });
            param1.Add(new Param { Key = "@CREATED_BY", Value = CurrentUser.UserAdmin.USER_ID.ToString() });
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param1);
            _commonRepository.ListProcedure<ErrorModel>(modelError, "utl_Insert_ErrorLog", paramType);
        }
    }

}