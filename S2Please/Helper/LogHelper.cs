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
            var param = new List<Param>();
            param.Add(new Param { Key = "@ERROR_NUM", Value = new Random().Next(100000000, 999999999).ToString() });
            param.Add(new Param { Key = "@ERROR_MSG", Value = msg });
            param.Add(new Param { Key = "@ERROR_PROC", Value = procedure });
            param.Add(new Param { Key = "@CREATED_BY", Value = CurrentUser.UserAdmin.USER_ID.ToString() });
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
            _commonRepository.ListProcedure<ErrorModel>(modelError, "utl_Insert_ErrorLog", paramType);
        }

        public static void LogMailQueue(long id,long dataId,string dataType,string mailTo,string mailCc,string mailBcc,string mailName,string content,string message,string status,string from)
        {
            var model = new Repository.Model.MailQueueModel();
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            param.Add(new Param { Key = "@DATA_ID", Value =dataId.ToString() });
            param.Add(new Param { Key = "@DATA_TYPE", Value = dataType });
            param.Add(new Param { Key = "@MAIL_TO", Value = mailTo });
            param.Add(new Param { Key = "@MAIL_CC", Value = mailCc });
            param.Add(new Param { Key = "@MAIL_BCC", Value = mailBcc });
            param.Add(new Param { Key = "@MAIL_NAME", Value = mailName });
            param.Add(new Param { Key = "@CONTENT", Value = content });
            param.Add(new Param { Key = "@MESSAGE", Value = message });
            param.Add(new Param { Key = "@STATUS", Value = status });
            param.Add(new Param { Key = "@FROM", Value = from });
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
            _commonRepository.ListProcedure<Repository.Model.MailQueueModel>(model, "MailQueue_Update_MailQueue", paramType);
        }
    }

}