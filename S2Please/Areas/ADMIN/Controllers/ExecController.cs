using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Database;
using S2Please.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using SHOP.COMMON;
using S2Please.Controllers;
using S2Please.Helper;
using System.Web.Security;
using System;
using System.Web;
using S2Please.Areas.WEB_SHOP.ViewModel;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class ExecController : BaseController
    {
        // GET: ADMIN/Exec
        public ActionResult Index(ResultModel model)
        {
            return View(model);
        }
        public ActionResult EXEC(string sql)
        {
            var result = ExecSqlString(sql);
            result.CacheName = sql;
            return View("Index", result);
        }
    }
}