using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S2Please.Models;
using S2Please.Areas.WEB_SHOP.ViewModel;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using SHOP.COMMON.Helpers;
using S2Please.Helper;
using S2Please.ParramType;
using SHOP.COMMON;
using Repository;

namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class ProductController : S2Please.Controllers.BaseController
    {
        // GET: WEB_SHOP/Home
        private ISystemRepository _systemRepository;
        private IProductRepository _productRepository;
        public ProductController(ISystemRepository systemRepository, IProductRepository productRepository)
        {
            this._systemRepository = systemRepository;
            this._productRepository = productRepository;
        }

        //PRODUCT_DETAIL
        public ActionResult Detail(string toDecrypt)
        {
            ProductDetailViewModel vm = new ProductDetailViewModel();
            var id = FunctionHelpers.ConvertKey(toDecrypt, FunctionHelpers.KeyEncrypt);

            //Lấy sản phẩm ID sản phẩm
            var responseProduct = _productRepository.GetProductById(long.Parse(id));
            var resultProduct = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(responseProduct.Results));
            if (resultProduct!=null && resultProduct.Count()>0)
            {
                vm.Product = resultProduct.FirstOrDefault();

            }

            //Lưu vào danh sách đã xem
            List<ProductModel> products=new List<ProductModel>();
            HttpCookie getCookie = System.Web.HttpContext.Current.Request.Cookies[Constant.ViewProduct];
            if (getCookie != null)
            {
                 var value = getCookie.Value;
                string strConvertKey = Utils.Decrypt(value, Constant.ShopKeyViewProduct);
                products = JsonConvert.DeserializeObject<List<ProductModel>>(strConvertKey);
                if (products != null && products.Count() > 0)
                {
                    vm.ViewProducts = products;
                    if (products.Where(s=>s.ID==vm.Product.ID).ToList().Count()==0)
                    {
                        products.Add(vm.Product);
                        Security.ProductView(products, System.Web.HttpContext.Current);
                    }
                }           
            }
            else
            {
                products.Add(vm.Product);
                Security.ProductView(products, System.Web.HttpContext.Current);
            }
            //Lấy danh sách ảnh ID sản phẩm
            var responseProductImgs = _productRepository.GetProductImgByProductId(long.Parse(id));
            vm.ProductImgs = JsonConvert.DeserializeObject<List<ProductImgModel>>(JsonConvert.SerializeObject(responseProductImgs.Results));

            //Lấy danh sách bonus ID sản phẩm
            var responseNonus = _productRepository.GetProductBonusByProductId(long.Parse(id));
            vm.ProductBonus = JsonConvert.DeserializeObject<List<ProductBonusModel>>(JsonConvert.SerializeObject(responseNonus.Results));

            //Lấy danh sách màu theo ID sản phẩm
            var responseColors = _productRepository.GetProductColorByProductId(long.Parse(id));
            vm.ProductColors = JsonConvert.DeserializeObject<List<ProductColorModel>>(JsonConvert.SerializeObject(responseColors.Results));

            //Lấy danh sách size theo ID sản phẩm
            var responseSizes = _productRepository.GetProductSizeByProductId(long.Parse(id));
            vm.ProductSizes = JsonConvert.DeserializeObject<List<ProductSizeModel>>(JsonConvert.SerializeObject(responseSizes.Results));

            //Lấy bản ghi mapper giữa size và color
            long colorID = vm.ProductColors != null && vm.ProductColors.Count() > 0 && vm.ProductColors.Where(s => s.IS_MAIN == true).FirstOrDefault()!=null? vm.ProductColors.Where(s => s.IS_MAIN == true).FirstOrDefault().ID : 0;
            long sizeID = vm.ProductSizes != null && vm.ProductSizes.Count() > 0 && vm.ProductSizes.Where(s => s.IS_MAIN == true).FirstOrDefault() !=null? vm.ProductSizes.Where(s => s.IS_MAIN == true).FirstOrDefault().ID : 0;
            var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(colorID, sizeID, vm.Product.ID, "1");
            var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
            if (resultMapper!=null && resultMapper.Count()>0)
            {
                vm.ProductMapper = resultMapper.FirstOrDefault();
            }

            //Lấy danh sách bản ghi mapper giữa size và color theo productid
            var responseMappers = _productRepository.GetProductMapperProductId(vm.Product.ID);
            vm.ProductMappers = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMappers.Results));
            return View(vm);
        }
        
        public ActionResult GetPrice(long colorID=0,long sizeID=0,long productId=0)
        {
            ProductPriceViewModel vm = new ProductPriceViewModel();
            var responseMapper = _productRepository.GetProductColorSizeMapperByColorIdAndSizeId(colorID, sizeID,productId, "0");
            var resultMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));
            if (resultMapper != null && resultMapper.Count() > 0)
            {
                vm.ProductMapper = resultMapper.FirstOrDefault();
            }
            vm.Html = RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/Product/_PriceDetail.cshtml", vm);
            return Json(vm, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _ProductContent(ProductContentViewModel viewModel)
        {
            //Lấy danh sách sản phẩm
            var param = new List<Param>();
            var basicParam= new List<ParamType>();
            ParamType model = new ParamType();
            model.VALUE = viewModel.Value.ToString();
            model.PAGE_SIZE = viewModel.NumberProductGet;
            model.PAGE_NUMBER = 1;
            basicParam.Add(model);
            var basicParamType = MapperHelper.MapList<ParamType, Repository.Type.ParamType>(basicParam);
            //Lấy danh sách sản phẩm
            var resultProductByGroups = _productRepository.GetProductbyStoredProcedureName(basicParamType, viewModel.StoredProcedureName);
            viewModel.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            return View(viewModel);
        }

        public ActionResult LoadSlideRelated(ParamType model)
        {
            ProductContentViewModel vm = new ProductContentViewModel();
            //Lấy danh sách sản phẩm
            var param = new List<Param>();
            var basicParam = new List<ParamType>();
            basicParam.Add(model);
            var basicParamType = MapperHelper.MapList<ParamType, Repository.Type.ParamType>(basicParam);
            var resultProductByGroups = _productRepository.GetProductByTypeId(basicParamType);
            vm.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            vm.Index = int.Parse(model.STRING_FILTER) + 2;
            vm.NumberInPage = 3;
            vm.NumberProductGet = int.Parse(model.PAGE_SIZE.ToString());

            var html = RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/Product/_ListProductLi.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _ProductDetail_Categories(ProductDetailListCategoriesViewModel viewModel)
        {
            //Lấy danh sách loại sản phẩm
            var modelProductType = new ProductTypeModel();
            var resultProductTypes = _productRepository.GetProductType();
            viewModel.ProductTypes = JsonConvert.DeserializeObject<List<ProductTypeModel>>(JsonConvert.SerializeObject(resultProductTypes.Results));
            return View(viewModel);
        }

        public ActionResult LoadSlideRandom(ParamType model)
        {
            ProductContentViewModel vm = new ProductContentViewModel();
            vm.Index = int.Parse(model.STRING_FILTER) + 2;
            vm.NumberInPage = 1;
            vm.NumberProductGet = int.Parse(model.PAGE_SIZE.ToString());

            if (Session["ID"]!=null)
            {
                model.STRING_FILTER = model.VALUE;
                model.VALUE = Session["ID"].ToString();
            }
            //Lấy danh sách sản phẩm
            var param = new List<Param>();
            var basicParam = new List<ParamType>();
            basicParam.Add(model);           
            var basicParamType = MapperHelper.MapList<ParamType, Repository.Type.ParamType>(basicParam);
            var resultProductByGroups = _productRepository.GetProductRanDom(basicParamType);
            vm.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(resultProductByGroups.Results));
            var html = RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/Product/_ListProductLi.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);

        }

        //PRODUCT_LIST
        public ActionResult Products(string toDecryptProductType, string toDecryptGroup,string textSearch)
        {
            var param = new List<Param>();
            ProductListViewModel vm = new ProductListViewModel();
            if (!String.IsNullOrEmpty(toDecryptProductType))
            {
                var typeID = FunctionHelpers.ConvertKey(toDecryptProductType, FunctionHelpers.KeyEncrypt);
                if (typeID != "0")
                {
                    vm.ProductTypes.Add(FunctionHelpers.ConvertKey(toDecryptProductType, FunctionHelpers.KeyEncrypt));
                    var resultProductType = _productRepository.GetProductTypeByID(long.Parse(FunctionHelpers.ConvertKey(toDecryptProductType, FunctionHelpers.KeyEncrypt)));
                    var query = JsonConvert.DeserializeObject<List<ProductTypeModel>>(JsonConvert.SerializeObject(resultProductType.Results));
                    if (query != null && query.Count() > 0)
                    {
                        vm.ProductGroups.Add(query.FirstOrDefault().PRODUCT_GROUP_ID.ToString());
                    }
                }
                
            }
            if (!String.IsNullOrEmpty(toDecryptGroup))
            {
                var groupId = FunctionHelpers.ConvertKey(toDecryptGroup, FunctionHelpers.KeyEncrypt);
                if (groupId!="0")
                {
                    vm.ProductGroups.Add(FunctionHelpers.ConvertKey(toDecryptGroup, FunctionHelpers.KeyEncrypt));
                }
            }

            ParamType paramModel = new ParamType();
            #region Tạo câu điều kiện tìm khi truyền data vào
            if (vm.ProductTypes !=null && vm.ProductTypes.Count()>0)
            {
                // TÌM THEO DANH MỤC
                paramModel.STRING_FILTER += "AND P.PRODUCT_TYPE_ID IN(" + string.Join(",", vm.ProductTypes) + ")";
            }
            if (vm.ProductGroups != null && vm.ProductGroups.Count() > 0)
            {
                // TÌM THEO DANH MỤC
                paramModel.STRING_FILTER += "AND P.PRODUCT_GROUP_ID IN(" + string.Join(",", vm.ProductGroups) + ")";
            }

            if (!String.IsNullOrEmpty(textSearch))
            {
                paramModel.STRING_FILTER += "AND P.NAME LIKE N" + "'%" + textSearch + "%'";
            }
            paramModel.PAGE_NUMBER = 1;
            paramModel.PAGE_SIZE = 9;

            var basicParam = new List<ParamType>();
            basicParam.Add(paramModel);
            var basicParamType = MapperHelper.MapList<ParamType, Repository.Type.ParamType>(basicParam);
            var result = _productRepository.GetProducts(basicParamType);
            vm.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(result.Results));
            if (result.OutValue!=null && result.OutValue.Parameters.Count>0)
            {
                vm.Total = Convert.ToInt32(result.OutValue.Parameters["@TotalRecord"].Value.ToString());
            }
            vm.Parram = paramModel;
            vm.Url = "/WEB_SHOP/Product/GetProduct";
            #endregion

            //Danh sách product type theo group
            param = new List<Param>();
            var value = vm.ProductGroups.FirstOrDefault();
            if (value==null)
            {
                value = "0";
            }
            var resultProductTypes = _productRepository.GetProductTypeByGroupID(long.Parse(value));
            vm.DataProductTypes = JsonConvert.DeserializeObject<List<ProductTypeModel>>(JsonConvert.SerializeObject(resultProductTypes.Results));
            return View(vm);
        }

        public ActionResult GetProduct(ParamType paramModel, List<string> productTypes,int? fromMoney,int? toMoney)
        {
            ProductListViewModel vm = new ProductListViewModel();

            if (productTypes!=null && productTypes.Count()>0)
            {
                // TÌM THEO DANH MỤC
                vm.ProductTypes = productTypes;
                paramModel.STRING_FILTER += "AND P.PRODUCT_TYPE_ID IN(" + string.Join(",", productTypes) + ")";
            }
            if (fromMoney!=null && fromMoney!=0)
            {
                paramModel.STRING_FILTER += " AND M.PRICE >= " + fromMoney;
            }
            if (toMoney != null && toMoney != 0)
            {
                paramModel.STRING_FILTER += " AND M.PRICE <= " + toMoney;
            }
            var basicParam = new List<ParamType>();
            basicParam.Add(paramModel);
            var basicParamType = MapperHelper.MapList<ParamType, Repository.Type.ParamType>(basicParam);
            var result = _productRepository.GetProducts(basicParamType);
            vm.Products = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(result.Results));
            vm.Total = Convert.ToInt32(result.OutValue.Parameters["@TotalRecord"].Value.ToString());
            vm.Parram = paramModel;
            vm.Url = "/WEB_SHOP/Product/GetProduct";

            var html = RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/Product/_ListProduct.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);
        }
    }
}