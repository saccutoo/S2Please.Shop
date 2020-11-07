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
using SHOP.COMMON.Helpers;
using S2Please.ParramType;

namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class HomeController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/Home
        private ado _db=new ado();
        private string _connection = ConfigurationManager.AppSettings["DBConnection"];
        public ActionResult Index()
        {
            ProductViewModel vm = new ProductViewModel();

            //Lấy danh sách loại sản phẩm
            var modelProductType = new ProductTypeModel();
            var resultProductTypes = ListProcedure<ProductTypeModel>(modelProductType, "ProductType_Get_ProductType", new List<Param>());
            vm.ProductTypes = JsonConvert.DeserializeObject<List<ProductTypeModel>>(JsonConvert.SerializeObject(resultProductTypes.Results));

            //Lấy danh sách group
            var modelProductGroup = new ProductGroupModel();
            var resultProductGroups = ListProcedure<ProductGroupModel>(modelProductGroup, "ProductGroup_Get_ProductGroup", new List<Param>());
            vm.ProductGroups = JsonConvert.DeserializeObject<List<ProductGroupModel>>(JsonConvert.SerializeObject(resultProductGroups.Results));

            var param = new List<Param>();
            var basicParamype = new List<ParamType>();
            basicParamype.Add(new ParamType { PAGE_SIZE =12, PAGE_NUMBER = 1 });
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamype)
                }
            });

            //Lấy danh sách sản phẩm
            var resultProductByGroups = ListProcedure<ProductModel>(new ProductModel(), "Product_Get_Product", param);
            vm.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            vm.Param = new ParamType { PAGE_SIZE = 12, PAGE_NUMBER = 1 };
            return View(vm);
        }
        public ActionResult LoadSlide(ParamType model)
        {
            ProductContentViewModel vm = new ProductContentViewModel();
            //Lấy danh sách sản phẩm
            var param = new List<Param>();
            var basicParamype = new List<ParamType>();
            basicParamype.Add(model);
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamype)
                }
            });

            var resultProductByGroups = ListProcedure<ProductModel>(new ProductModel(), "Product_Get_ProductByGroupId", param);
            vm.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            vm.Index = int.Parse(model.STRING_FILTER) + 2;
            var html= RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/Product/_ListProductLi.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Broken()
        {
            var resultProductByGroups = ListProcedure<MULTI_LANGUAGE>(new MULTI_LANGUAGE(), "Broken", new List<Param>());
            var response = JsonConvert.DeserializeObject<List<MULTI_LANGUAGE>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            if (response!=null && response.Count()>0)
            {
                return RedirectToAction("Index", "Home", new { Area = "WEB_SHOP" });
            }
            return View();
        }
    }
}