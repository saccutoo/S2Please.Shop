using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S2Please.Database;
using S2Please.Models;
using S2Please.Areas.WEB_SHOP.ViewModel;
using Newtonsoft.Json;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using SHOP.COMMON;
using S2Please.Helper;

namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class MenuController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/Home
        private ado _db=new ado();
        public ActionResult Index()
        {
            MenuViewModel vm = new MenuViewModel();

            //Lấy danh sách sản phẩm menu
            var menus = JsonConvert.SerializeObject((from M in _db.MENU
                                                     where M.IS_DELETED == false
                                                     select new
                                                     {
                                                         ID = M.ID,
                                                         MENU_NAME = M.MENU_NAME,
                                                         PARENT_ID = M.PARENT_ID,
                                                         LINK = M.LINK,
                                                         LINK_VIEW = M.LINK_VIEW,
                                                         ORDER_MENU = M.ORDER_MENU,
                                                         PRODUCT_GROUP_ID=M.PRODUCT_GROUP_ID
                                                     }).ToList().OrderBy(s => s.ORDER_MENU));
            vm.Menus = JsonConvert.DeserializeObject<List<MenuModel>>(menus);

            return View(vm);
        }
        public ActionResult DropdownLanuage()
        {
            LanguageViewModel vm = new LanguageViewModel();
            //Lấy danh sách ngôn ngữ
            vm.languageId = CurrentUser.LANGUAGE_ID.ToString();
            var response = ListProcedure<LanguageModel>(new LanguageModel(), "Language_Get_Language", new List<Param>());
            vm.languages = JsonConvert.DeserializeObject<List<LanguageModel>>(JsonConvert.SerializeObject(response.Results));
            return View(vm);
        }
    }
}