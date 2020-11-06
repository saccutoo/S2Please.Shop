using S2Please.Database;
using Newtonsoft.Json;
using SHOP.COMMON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2Please.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private ado _ado = new ado();
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "WEB_SHOP" });
        }
    }
}