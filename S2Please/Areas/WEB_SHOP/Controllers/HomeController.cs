using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Database;
using S2Please.Models;
using S2Please.Areas.WEB_SHOP.ViewModel;
using Newtonsoft.Json;
using SHOP.COMMON.Helpers;
using S2Please.ParramType;
using Repository;

namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class HomeController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/Home
        private ISystemRepository _systemRepository;
        private IProductRepository _productRepository;
        public HomeController(ISystemRepository systemRepository, IProductRepository productRepository)
        {
            this._systemRepository = systemRepository;
            this._productRepository = productRepository;
        }
        public ActionResult Index()
        {
            ProductViewModel vm = new ProductViewModel();

            //Lấy danh sách loại sản phẩm
            var resultProductTypes = _productRepository.GetProductType();
            vm.ProductTypes = JsonConvert.DeserializeObject<List<ProductTypeModel>>(JsonConvert.SerializeObject(resultProductTypes.Results));

            //Lấy danh sách group
            var resultProductGroups = _productRepository.GetProductGroup();
            vm.ProductGroups = JsonConvert.DeserializeObject<List<ProductGroupModel>>(JsonConvert.SerializeObject(resultProductGroups.Results));

            var param = new List<Param>();
            var basicParam = new List<ParamType>();
            basicParam.Add(new ParamType { PAGE_SIZE =12, PAGE_NUMBER = 1 });
            var basicParamType = MapperHelper.MapList<ParamType,Repository.Type.ParamType>(basicParam);
            //Lấy danh sách sản phẩm
            var resultProductByGroups = _productRepository.ProductFromWeb(basicParamType);
            vm.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            vm.Param = new ParamType { PAGE_SIZE = 12, PAGE_NUMBER = 1 };
            return View(vm);
        }
        public ActionResult LoadSlide(ParamType model)
        {
            ProductContentViewModel vm = new ProductContentViewModel();
            //Lấy danh sách sản phẩm
            var param = new List<Param>();
            var basicParam = new List<ParamType>();
            basicParam.Add(model);
            var basicParamType = MapperHelper.MapList<ParamType, Repository.Type.ParamType>(basicParam);
            var resultProductByGroups = _productRepository.GetProductByGroupId(basicParamType);
            vm.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            vm.Index = int.Parse(model.STRING_FILTER) + 2;
            var html= RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/Product/_ListProductLi.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Broken()
        {
            var resultProductByGroups = _systemRepository.Broken();
            var response = JsonConvert.DeserializeObject<List<MULTI_LANGUAGE>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            if (response!=null && response.Count()>0)
            {
                return RedirectToAction("Index", "Home", new { Area = "WEB_SHOP" });
            }
            return View();
        }
    }
}