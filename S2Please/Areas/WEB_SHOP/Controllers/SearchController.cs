using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using SHOP.COMMON.Helpers;
using S2Please.ParramType;
using S2Please.Helper;
using Repository;
namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class SearchController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/Search
        private ISystemRepository _systemRepository;
        private IProductRepository _productRepository;
        public SearchController(ISystemRepository systemRepository, IProductRepository productRepository)
        {
            this._systemRepository = systemRepository;
            this._productRepository = productRepository;
        }
        public ActionResult SearchProduct(string searchString)
        {
            //Lấy danh sách sản phẩm
            var param = new List<Param>();
            var basicParam = new List<ParamType>();
            ParamType model = new ParamType();
            model.STRING_FILTER = searchString;
            basicParam.Add(model);
            var basicParamType = MapperHelper.MapList<ParamType, Repository.Type.ParamType>(basicParam);
            //Lấy danh sách sản phẩm
            var response = _productRepository.SearchProduct(basicParamType);
            var result = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(response.Results));
            if (result!=null && result.Count()>0)
            {
                foreach (var item in result)
                {
                    item.PRODUCT_KEY = FunctionHelpers.Encrypt(item.ID.ToString(), FunctionHelpers.KeyEncrypt);
                }
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnSelectProduct(string productId)
        {
            var token = FunctionHelpers.Encrypt(productId, FunctionHelpers.KeyEncrypt);
            return RedirectToAction("Detail","Product",new { Areas="WEB_SHOP", toDecrypt = token });
        }

        public ActionResult SearchCity(string value)
        {
            var response = _systemRepository.GetCity();
            var result = JsonConvert.DeserializeObject<List<CityModel>>(JsonConvert.SerializeObject(response.Results));
            var datas = new List<CityModel>();
            if (value!=null && !String.IsNullOrEmpty(value))
            {
                datas = result.Where(s => s.NAME.ToLower().Contains(value.ToLower())).ToList();
            }
            else
            {
                datas = result;
            }
            return Json(new { datas }, JsonRequestBehavior.AllowGet);
        }
    }
}