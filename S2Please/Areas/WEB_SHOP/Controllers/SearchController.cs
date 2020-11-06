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
using Hrm.Common.Helpers;
using S2Please.Helper;

namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class SearchController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/Home
        private ado _db=new ado();
        private string _connection = ConfigurationManager.AppSettings["DBConnection"];
        public ActionResult SearchProduct(string searchString)
        {
            //Lấy danh sách sản phẩm
            var param = new List<Param>();
            var basicParamype = new List<ParamType>();
            ParamType model = new ParamType();
            model.STRING_FILTER = searchString;
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
            //Lấy danh sách sản phẩm
            var response= ListProcedure<ProductModel>(new ProductModel(), "Product_Get_SearchProduct", param);
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
            var response = ListProcedure<CityModel>(new CityModel(), "City_Get_City", new List<Param>(),true,false);
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