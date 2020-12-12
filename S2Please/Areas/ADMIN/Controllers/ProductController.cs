using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using SHOP.COMMON;
using S2Please.Controllers;
using S2Please.Helper;
using System;
using System.Web;
using S2Please.Areas.ADMIN.ViewModel;
using S2Please.ParramType;
using System.Data;
using SHOP.COMMON.Helpers;
using ClosedXML.Excel;
using System.IO;
using Repository;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class ProductController : BaseController
    {
        // GET: ADMIN/Product

        //khởi tạo các Repository
        private IProductRepository _productRepository;
        private ITableRepository _tableRepository;
        public ProductController(IProductRepository productRepository, ITableRepository tableRepository)
        {
            this._productRepository = productRepository;
            this._tableRepository = tableRepository;

        }

        [HttpGet]
        public ActionResult Index(long id=0)
        {
            ProductViewModel product = new ProductViewModel();
            var responseTableUser = _tableRepository.GetTableUserConfig(TableName.Product, CurrentUser.UserAdmin.USER_ID);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));

            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                product.Table = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                var responseProduct = _tableRepository.GetTableByTableName(TableName.Product);
                var resultProduct = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseProduct.Results));
                if (resultProduct != null && resultProduct.Count() > 0)
                {
                    product.Table = JsonConvert.DeserializeObject<TableViewModel>(resultProduct.FirstOrDefault().TABLE_CONTENT);
                }
            }

            ParamType paramType = new ParamType();
            paramType.PAGE_SIZE = product.Table.PAGE_SIZE == 0 ? 1 : product.Table.PAGE_SIZE;
            paramType.PAGE_NUMBER = product.Table.PAGE_INDEX == 0 ? 1 : product.Table.PAGE_INDEX;
            product.Table.TABLE_NAME = TableName.Product;
            product.Table.TITLE_TABLE_NAME = FunctionHelpers.GetValueLanguage("Table.Title.Product");
            paramType.STRING_FILTER = id != 0 ? "AND P.ID=" + id : "";

            product.Table = RenderTable(product.Table, paramType);
            if (!product.Table.IS_PERMISSION)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            //Table common
            return View(product);
        }

        public ActionResult Detail(long id=0)
        {
            ProductDetailViewModel vm = new ProductDetailViewModel();

            var responseProduct = _productRepository.GetProductById(id);
            var resultProduct = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(responseProduct.Results));
            if (resultProduct != null && resultProduct.Count() > 0)
            {
                vm.Product = resultProduct.FirstOrDefault();
            }

            //Lấy danh sách ảnh ID sản phẩm
            var responseProductImgs = _productRepository.GetProductImgByProductId(id);
            vm.ProductImgs = JsonConvert.DeserializeObject<List<ProductImgModel>>(JsonConvert.SerializeObject(responseProductImgs.Results));

            //Lấy danh sách màu theo ID sản phẩm
            var responseColors = _productRepository.GetProductColorByProductId(id);
            vm.Colors = JsonConvert.DeserializeObject<List<ProductColorModel>>(JsonConvert.SerializeObject(responseColors.Results));

            //Lấy danh sách size theo ID sản phẩm
            var responseSizes = _productRepository.GetProductSizeByProductId(id);
            vm.Sizes = JsonConvert.DeserializeObject<List<ProductSizeModel>>(JsonConvert.SerializeObject(responseSizes.Results));

            //Lấy danh sách size theo ID sản phẩm
            var responseMapper = _productRepository.GetProductColorSizeMapperByProductId(id);
            vm.ColorSizeMapper = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseMapper.Results));

            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_Detail.cshtml", vm);
            //return Json(html, JsonRequestBehavior.AllowGet);
            return Content(JsonConvert.SerializeObject(new
            {
                html
            }));
        }

        //public ActionResult ShowFormDelete(long id=0)
        //{
        //    BaseModel model = new BaseModel();
        //    model.ID = id;
        //    var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/_DeleteModal.cshtml", model);
        //    return Json(html, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Delete(long id=0)
        {
            var result = new ResultModel();
            var response = _productRepository.DeleteProductByID(id, CurrentUser.UserAdmin.USER_ID);
            if (response != null)
            {
                if (response.Success == false && CheckPermision(response.StatusCode) == false)
                {
                    result.Success = false;
                    return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
                }
                else
                {
                    result.Success = true;
                    result.Message = FunctionHelpers.GetValueLanguage("Message.Delete.Success");
                    result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
                }
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductSave(long id = 0)
        {
            bool checkPermission = FunctionHelpers.CheckPermission(TableName.Product,Permission.Update);
            if (!checkPermission)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            if (id==0)
            {
                if (Session["SLIDE-IMG"] != null)
                {
                    var fileRemoves = Session["SLIDE-IMG"] as List<FileModel>;
                    foreach (var item in fileRemoves)
                    {
                        var check = RemoveFileUpload(item.FILE_NAME);
                    }
                }
                if (Session["COLOR-IMG"] != null)
                {
                    var fileRemoves = Session["COLOR-IMG"] as List<FileModel>;
                    foreach (var item in fileRemoves)
                    {
                        var check = RemoveFileUpload(item.FILE_NAME);
                    }
                }
                Session["SLIDE-IMG"] = null;
                Session["SLIDE-IMG-REMOVE"] = null;
                Session["COLOR-IMG"] = null;
                Session["COLOR-IMG-REMOVE"] = null;
            }
            ProductSaveViewModel vm = new ProductSaveViewModel();
            vm.Product.ID = id;

            var responseType = _productRepository.GetProductType();
            vm.Types = responseType.Results;

            var responseGroup = _productRepository.GetProductGroup();
            vm.Groups = responseGroup.Results.Where(s => s.TYPE == 2).ToList();

            if (id!=0)
            {
              
                var responseProduct = _productRepository.GetProductByIdFromAdmin(id);
                if (responseProduct != null)
                {
                    if (responseProduct.Success == false && CheckPermision(responseProduct.StatusCode) == false)
                    {
                        return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
                    }
                    else
                    {
                        vm.Product = JsonConvert.DeserializeObject<ProductModel>(JsonConvert.SerializeObject(responseProduct.Results.FirstOrDefault()));
                    }
                }

                var responseProductColor = _productRepository.GetProductColorByProductId(id);
                if (responseProductColor!=null && responseProductColor.Results!=null && responseProductColor.Results.Count()>0)
                {
                    vm.ProductColors = JsonConvert.DeserializeObject<List<ProductColorModel>>(JsonConvert.SerializeObject(responseProductColor.Results));
                    foreach (var item in vm.ProductColors)
                    {
                        vm.FileColors.Add(new FileModel() { 
                            ID=item.ID,
                            COLOR=item.COLOR,
                            FILE_NAME=item.IMG
                        });
                    }
                    Session["COLOR-IMG"] = vm.FileColors;
                }

                var responseProductSize = _productRepository.GetProductSizeByProductId(id);
                if (responseProductSize != null && responseProductSize.Results != null && responseProductSize.Results.Count() > 0)
                {
                    vm.ProductSizes = JsonConvert.DeserializeObject<List<ProductSizeModel>>(JsonConvert.SerializeObject(responseProductSize.Results));
                }

                var responseProductMap = _productRepository.GetProductMapperProductId(id);
                if (responseProductMap != null && responseProductMap.Results != null && responseProductMap.Results.Count() > 0)
                {
                    vm.ProductMapp = JsonConvert.DeserializeObject<List<ProductColorSizeMapperModel>>(JsonConvert.SerializeObject(responseProductMap.Results));
                }

                var responseProductSlides = _productRepository.GetProductImgByProductId(id);
                if (responseProductSlides != null && responseProductSlides.Results != null && responseProductSlides.Results.Count() > 0)
                {
                    vm.ProductSlides = JsonConvert.DeserializeObject<List<ProductImgModel>>(JsonConvert.SerializeObject(responseProductSlides.Results));
                    foreach (var item in vm.ProductSlides)
                    {
                        vm.FileSlides.Add(new FileModel()
                        {
                            ID = item.ID,
                            FILE_NAME = item.PRODUCT_IMAGE
                        });
                    }
                    Session["SLIDE-IMG"] = vm.FileSlides;

                }

            }
            return View(vm);
        }

        public ActionResult ReloadTableColorAndSize(string COLOR, string SIZE, List<ProductColorSizeMapperModel> ColorSizeMap)
        {
            RowTableColorAndSizeViewModel vm = new RowTableColorAndSizeViewModel();
            if (!String.IsNullOrEmpty(COLOR))
            {

                if (COLOR.IndexOf(",") > -1)
                {
                    var colors = COLOR.Split(',');
                    for (int i = 0; i < colors.Length; i++)
                    {
                        var obj = new
                        {
                            CODE = colors[i]
                        };
                        vm.Colors.Add(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(obj)));
                    }
                }
                else
                {
                    var obj = new
                    {
                        CODE = COLOR
                    };
                    vm.Colors.Add(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(obj)));
                }
            }

            if (!String.IsNullOrEmpty(SIZE))
            {

                if (SIZE.IndexOf(",") > -1)
                {
                    var sizes = SIZE.Split(',');
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        var obj = new
                        {
                            CODE = sizes[i]
                        };
                        vm.Sizes.Add(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(obj)));
                    }
                }
                else
                {
                    var obj = new
                    {
                        CODE = SIZE
                    };
                    vm.Sizes.Add(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(obj)));
                }
            }
            List<ProductColorSizeMapperModel> ColorSizeMaps = new List<ProductColorSizeMapperModel>();
            foreach (var color in vm.Colors)
            {
                foreach (var size in vm.Sizes)
                {
                    ColorSizeMaps.Add(new ProductColorSizeMapperModel()
                    {
                        COLOR = color["CODE"],
                        SIZE = size["CODE"]
                    });

                }
            }
            if (ColorSizeMaps != null && ColorSizeMaps.Count()>0)
            {
                foreach (var item in ColorSizeMaps)
                {
                    if (ColorSizeMap!=null && ColorSizeMap.Count()>0)
                    {
                        var data = ColorSizeMap.Where(s => s.COLOR == item.COLOR && s.SIZE == item.SIZE).FirstOrDefault();
                        if (data != null)
                        {
                            item.PRICE = data.PRICE;
                            item.RATE = data.RATE;
                            item.AMOUNT = data.AMOUNT;
                        }
                    }
                   
                }
            }
            vm.ColorSizeMapper = ColorSizeMaps;
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_RowTableColorAndSize.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckValidation(ProductModel model, List<ProductColorSizeMapperModel> ColorSizeMap)
        {
            if (model != null)
            {
                var validations = ValidationHelper.Validation(model, "model");
                if (ColorSizeMap == null)
                {
                    validations.Add(new ValidationModel()
                    {
                        IsError = true,
                        ErrorMessage = FunctionHelpers.GetValueLanguage("Product.Error.CreateOneProduct"),
                        ColumnName = "Card.ColorSizeMap"
                    });
                }
                if (validations.Count > 0)
                {
                    return Json(new { Result = validations, Invalid = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Invalid = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveProduct(ProductModel model, List<ProductColorSizeMapperModel> ColorSizeMap, List<ProductColorModel> ListImgByColor)
        {
 
            model.PRODUCT_CODE = new Random().Next(10000, 99999).ToString() + new Random().Next(10000, 99999).ToString();
            List<ColorType> colorTypes = new List<ColorType>();
            if (!String.IsNullOrEmpty(model.COLOR))
            {
                if (model.COLOR.IndexOf(",") > -1)
                {
                    var colors = model.COLOR.Split(',');
                    for (int i = 0; i < colors.Length; i++)
                    {
                        var value = colors[i].Replace(" ", "-");
                        var data = ListImgByColor.Where(s => s.COLOR == value).FirstOrDefault();

                        colorTypes.Add(new ColorType()
                        {
                            COLOR = colors[i],
                            PRODUCT_ID = model.ID,
                            ID= data.ID,
                            IMG= (data!=null ? data.IMG.ToString() : "")

                        });
                    }
                }
                else
                {
                    var data = ListImgByColor.Where(s => s.COLOR == model.COLOR.Replace(" ","-")).FirstOrDefault();
                    colorTypes.Add(new ColorType()
                    {
                        COLOR = model.COLOR,
                        PRODUCT_ID = model.ID,
                        ID = data.ID,
                        IMG = data != null ? data.IMG.ToString() : ""
                    });
                }
            }

            List<SizeType> sizeTypes = new List<SizeType>();
            if (!String.IsNullOrEmpty(model.SIZE))
            {
                if (model.SIZE.IndexOf(",") > -1)
                {
                    var sizes = model.SIZE.Split(',');
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        sizeTypes.Add(new SizeType()
                        {
                            SIZE_NAME = sizes[i],
                            PRODUCT_ID = model.ID
                        });
                    }
                }
                else
                {
                    sizeTypes.Add(new SizeType()
                    {
                        SIZE_NAME = model.SIZE,
                        PRODUCT_ID = model.ID
                    });
                }
            }

            List<ProductImgType> productImgs = new List<ProductImgType>();
            if (Session["SLIDE-IMG"]!=null)
            {
                var files= Session["SLIDE-IMG"] as List<FileModel>;
                if (files!=null && files.Count()>0)
                {
                    int index = 0;

                    foreach (var item in files)
                    {
                        productImgs.Add(new ProductImgType()
                        {
                            ID = item.ID,
                            PRODUCT_IMAGE = item.FILE_NAME,
                            PRODUCT_ID = model.ID,
                            TITLE = string.Empty,
                            DECRIPTION = string.Empty,
                            IS_MAIN = index == 0 ? true : false,
                            IS_SHOW_COLOR = true,
                            IS_SHOW_SLIDE = true,
                            PRODUCT_DETAIL_ID = 0
                        });
                        index++;
                    }
                }
            }
                     
            List<ColorSizeMapperType> colorSizeMapType = new List<ColorSizeMapperType>();                      
            colorSizeMapType = MapperHelper.MapList<ProductColorSizeMapperModel, ColorSizeMapperType>(ColorSizeMap);
            bool checkIsMAin = false;
            foreach (var item in colorSizeMapType)
            {
                if (item.IS_MAIN)
                {
                    checkIsMAin = true;
                    break;
                }
            }
            if (!checkIsMAin)
            {
                colorSizeMapType.FirstOrDefault().IS_MAIN = true;
            }


            var param = new List<Param>();

            ParamType paramType = new ParamType();
            paramType.LANGUAGE_ID = CurrentUser.LANGUAGE_ID;
            paramType.USER_ID = CurrentUser.UserAdmin.USER_ID;
            paramType.ROLE_ID = CurrentUser.UserAdmin.ROLE_ID;
            var paramTypeInsert = MapperHelper.Map<ParamType, Repository.Type.ParamType>(paramType);
            var colorTypeInsert = MapperHelper.MapList<ColorType, Repository.Type.ColorType>(colorTypes);
            var sizeTypeInsert = MapperHelper.MapList<SizeType, Repository.Type.SizeType>(sizeTypes);
            var productImgTypeInsert = MapperHelper.MapList<ProductImgType, Repository.Type.ProductImgType>(productImgs);
            var ColorSizeMapperTypeInsert = MapperHelper.MapList<ColorSizeMapperType, Repository.Type.ColorSizeMapperType>(colorSizeMapType);
            var modelInsert=MapperHelper.Map<ProductModel, Repository.Model.ProductModel>(model);

            var result = new ResultModel();
            var response = _productRepository.SaveProduct(modelInsert, paramTypeInsert, colorTypeInsert, sizeTypeInsert, productImgTypeInsert, ColorSizeMapperTypeInsert);
            if (response != null)
            {
                if (response.Success == false && CheckPermision(response.StatusCode) == false)
                {
                    result.SetUrl("/Base/Page404");
                    return Content(JsonConvert.SerializeObject(new
                    {
                        result
                    }));
                }
                else
                {
                    if (response.Results == null || response.Results.Count() == 0)
                    {
                        result.Success = false;
                        if (model.ID != 0)
                        {
                            result.Message = FunctionHelpers.GetValueLanguage("Message.UpdateProduct.Error");
                        }
                        else
                        {
                            result.Message = FunctionHelpers.GetValueLanguage("Message.AddProduct.Error");
                        }
                        result.CacheName = FunctionHelpers.GetValueLanguage("Message.Error");
                    }
                    else
                    {
                        if (Session["SLIDE-IMG-REMOVE"] != null)
                        {
                            var fileRemoves = Session["SLIDE-IMG-REMOVE"] as List<FileModel>;
                            foreach (var item in fileRemoves)
                            {
                                var check = RemoveFileUpload(item.FILE_NAME);
                            }
                        }
                        if (Session["COLOR-IMG-REMOVE"] != null)
                        {
                            var fileRemoves = Session["COLOR-IMG-REMOVE"] as List<FileModel>;
                            foreach (var item in fileRemoves)
                            {
                                var check = RemoveFileUpload(item.FILE_NAME);
                            }
                        }
                        Session["SLIDE-IMG"] = null;
                        Session["SLIDE-IMG-REMOVE"] = null;
                        Session["COLOR-IMG"] = null;
                        Session["COLOR-IMG-REMOVE"] = null;

                        Session["Product"] = JsonConvert.DeserializeObject<ProductModel>(JsonConvert.SerializeObject(response.Results.FirstOrDefault()));
                        result.Success = true;
                        if (model.ID!=0)
                        {
                            result.Message = FunctionHelpers.GetValueLanguage("Message.UpdateProduct.Success");
                        }
                        else
                        {
                            result.Message = FunctionHelpers.GetValueLanguage("Message.AddProduct.Success");
                        }
                        result.CacheName = FunctionHelpers.GetValueLanguage("Message.Success");
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetSessionFile(List<HttpPostedFileBase> attachments, string json)
        {
            if (attachments != null && attachments.Count() > 0)
            {
                Session["Files"] = attachments;
                Session["Json"] = json;
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListSlideImg(List<FileModel> model)
        {
            SlideImgViewModel vm = new SlideImgViewModel();
            vm.Files = model;
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_ListSlideImg.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadSlideImg(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null && files.Count() > 0)
            {
                var listFile = SaveFile(files);
                if (listFile != null && listFile.Count() > 0)
                {
                    Session["SLIDE-IMG"] = listFile;
                    SlideImgViewModel vm = new SlideImgViewModel();
                    vm.Files = listFile;
                    var response = new ResultModel();
                    response.Success = true;
                    response.Html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_ListSlideImg.cshtml", vm);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            var result = new ResultModel();
            result.Success = false;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveFileSlide(string fileName, long id=0, bool removeAll=false)
        {
            var response = new ResultModel();
            var listFile = Session["SLIDE-IMG"] as List<FileModel>;
            SlideImgViewModel vm = new SlideImgViewModel();

            if (!removeAll)
            {
                if (id != 0)
                {
                    List<FileModel> fileRemoves = new List<FileModel>();
                    if (Session["SLIDE-IMG-REMOVE"] != null)
                    {
                        fileRemoves = Session["SLIDE-IMG-REMOVE"] as List<FileModel>;
                    }
                    var file = listFile.Where(s => s.ID == id).FirstOrDefault();
                    if (file != null)
                    {
                        listFile.Remove(file);
                        Session["SLIDE-IMG"] = listFile;
                        fileRemoves.Add(file);
                        Session["SLIDE-IMG-REMOVE"] = fileRemoves;
                        vm.Files = listFile;
                        response.Success = true;
                        response.Html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_ListSlideImg.cshtml", vm);
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var file = listFile.Where(s => s.FILE_NAME == fileName).FirstOrDefault();
                    if (file != null)
                    {
                        listFile.Remove(file);
                        Session["SLIDE-IMG"] = listFile;
                        vm.Files = listFile;
                        var check = RemoveFileUpload(fileName);
                        if (check)
                        {
                            response.Success = true;
                            response.Html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_ListSlideImg.cshtml", vm);
                            return Json(response, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            else
            {
                List<FileModel> fileRemoves = new List<FileModel>();
                if (Session["SLIDE-IMG-REMOVE"] != null)
                {
                    fileRemoves = Session["SLIDE-IMG-REMOVE"] as List<FileModel>;
                }
                foreach (var item in listFile)
                {
                    if (item.ID==0)
                    {
                        var file = listFile.Where(s => s.FILE_NAME == item.FILE_NAME).FirstOrDefault();
                        if (file != null)
                        {
                            var check = RemoveFileUpload(item.FILE_NAME);
                           
                        }
                    }
                    else
                    {
                        var file = listFile.Where(s => s.ID == item.ID).FirstOrDefault();
                        fileRemoves.Add(file);
                        Session["SLIDE-IMG-REMOVE"] = fileRemoves;
                    }
                }
                Session["SLIDE-IMG"] = new List<FileModel>();
                vm.Files = new List<FileModel>();
                response.Success = true;
                response.Html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_ListSlideImg.cshtml", vm);
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            var result = new ResultModel();
            result.Success = false;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadImgByColor(IEnumerable<HttpPostedFileBase> files,string dynamic)
        {
            SlideImgViewModel vm = new SlideImgViewModel();

            var listFile = new List<FileModel>();
            if (Session["COLOR-IMG"] != null)
            {
                listFile = Session["COLOR-IMG"] as List<FileModel>;
            }

            List<FileModel> fileRemoves = new List<FileModel>();
            if (Session["COLOR-IMG-REMOVE"] != null)
            {
                fileRemoves = Session["COLOR-IMG-REMOVE"] as List<FileModel>;
            }
            if (files != null && files.Count() > 0)
            {
                var dataDynamic = JsonConvert.DeserializeObject<List<dynamic>>(dynamic);
                foreach (var item in dataDynamic)
                {
                    var save = SaveFile(files);
                    if (save!=null && save.Count()>0)
                    {
                        if (listFile != null && listFile.Count() > 0)
                        {
                            var file = listFile.Where(s => s.COLOR == item["COLOR"].ToString()).FirstOrDefault();
                            if (file != null)
                            {
                                if (file.ID != 0)
                                {
                                    listFile.Where(s => s.ID == long.Parse(item["ID"].ToString())).FirstOrDefault().FILE_NAME = save[0].FILE_NAME;

                                }
                                else
                                {
                                    listFile.Where(s => s.COLOR == item["COLOR"].ToString()).FirstOrDefault().FILE_NAME = save[0].FILE_NAME;
                                }
                            }
                            else
                            {
                                listFile.Add(new FileModel()
                                {
                                    ID = long.Parse(item["ID"].ToString()),
                                    FILE_NAME = item["FILE_NAME"].ToString(),
                                    COLOR = item["COLOR"].ToString(),
                                });
                            }
                        }
                        else
                        {
                            listFile.Add(new FileModel()
                            {
                                ID = long.Parse(item["ID"].ToString()),
                                FILE_NAME = item["FILE_NAME"].ToString(),
                                COLOR = item["COLOR"].ToString(),
                            });
                        }
                    }
                   
                    
                }
                Session["COLOR-IMG"] = listFile;
                vm.Files = listFile;
                var response = new ResultModel();
                response.Success = true;
                response.Html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_ListImgByColor.cshtml", vm);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                var dataDynamic = JsonConvert.DeserializeObject<List<dynamic>>(dynamic);
                if (listFile != null && listFile.Count() > 0 && dataDynamic.Count()> listFile.Count())
                {
                    foreach (var item in dataDynamic)
                    {
                        var file = listFile.Where(s => s.COLOR == item["COLOR"].ToString()).ToList().FirstOrDefault();
                        if (file==null)
                        {
                            listFile.Add(new FileModel()
                            {
                                ID= long.Parse(item["ID"].ToString()),
                                FILE_NAME = item["FILE_NAME"].ToString(),
                                COLOR = item["COLOR"].ToString(),

                            });
                        }
                    }
                }
                if (listFile!=null && listFile.Count()>0)
                {
                    foreach (var item in listFile)
                    {
                        var file = dataDynamic.Where(s => s["COLOR"].ToString() == item.COLOR).ToList().FirstOrDefault();
                        if (file!=null)
                        {
                           
                            if (file["IS_REMOVE"]=="1")
                            {
                                if (file["IS_REMOVE_IMG"] == "1")
                                {
                                    if (!string.IsNullOrEmpty(item.FILE_NAME) && item.ID != 0)
                                    {
                                        fileRemoves.Add(new FileModel()
                                        {
                                            ID = item.ID,
                                            FILE_NAME = item.FILE_NAME,
                                            COLOR = item.COLOR
                                        });
                                        Session["COLOR-IMG-REMOVE"] = fileRemoves;
                                    }
                                    else if (!string.IsNullOrEmpty(item.FILE_NAME) && item.ID == 0)
                                    {
                                        var check = RemoveFileUpload(item.FILE_NAME);
                                    }
                                    vm.Files.Add(new FileModel()
                                    {
                                        ID = long.Parse(file["ID"].ToString()),
                                        FILE_NAME = "",
                                        COLOR = file["COLOR"].ToString(),
                                    });
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(item.FILE_NAME) && item.ID != 0)
                                    {
                                        fileRemoves.Add(new FileModel()
                                        {
                                            ID = item.ID,
                                            FILE_NAME = item.FILE_NAME,
                                            COLOR = item.COLOR
                                        });
                                        Session["COLOR-IMG-REMOVE"] = fileRemoves;
                                    }
                                    else if (!string.IsNullOrEmpty(item.FILE_NAME) && item.ID == 0)
                                    {
                                        var check = RemoveFileUpload(item.FILE_NAME);
                                    }
                                }
                               
                            }
                            else
                            {
                                vm.Files.Add(new FileModel()
                                {
                                    ID = long.Parse(file["ID"].ToString()),
                                    FILE_NAME = file["FILE_NAME"].ToString(),
                                    COLOR = file["COLOR"].ToString(),
                                });
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.FILE_NAME) && item.ID != 0)
                            {
                                fileRemoves.Add(new FileModel()
                                {
                                    ID = item.ID,
                                    FILE_NAME = item.FILE_NAME,
                                    COLOR = item.COLOR
                                });
                                Session["COLOR-IMG-REMOVE"] = fileRemoves;
                            }
                            else if (!string.IsNullOrEmpty(item.FILE_NAME) && item.ID == 0)
                            {
                                var check = RemoveFileUpload(item.FILE_NAME);
                            }
                            //vm.Files.Add(new FileModel()
                            //{
                            //    ID = item.ID,
                            //    FILE_NAME = item.FILE_NAME,
                            //    COLOR = item.COLOR,
                            //});
                        }
                    }
                }
                else
                {
                    if (dataDynamic != null && dataDynamic.Count() > 0)
                    {
                        foreach (var item in dataDynamic)
                        {
                            vm.Files.Add(new FileModel()
                            {
                                ID = long.Parse(item["ID"].ToString()),
                                FILE_NAME = item["FILE_NAME"].ToString(),
                                COLOR = item["COLOR"].ToString(),
                            });
                        }
                    }
                }
                
                Session["COLOR-IMG"] = vm.Files;
                var response = new ResultModel();
                response.Success = true;
                response.Html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Product/_ListImgByColor.cshtml", vm);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
           
        }

        #region RenderTable
        public ActionResult ReloadTable(TableViewModel tableData, ParamType param)
        {
            if (param.STRING_FILTER == null)
            {
                param.STRING_FILTER = string.Empty;
            }

            var responseTableUser = _tableRepository.GetTableUserConfig(TableName.Product, CurrentUser.UserAdmin.USER_ID);
            var resultTableUser = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseTableUser.Results));
            if (resultTableUser != null && resultTableUser.Count() > 0)
            {
                tableData = JsonConvert.DeserializeObject<TableViewModel>(resultTableUser.FirstOrDefault().TABLE_CONTENT);
            }
            else
            {
                var responseProduct = _tableRepository.GetTableByTableName(TableName.Product);
                var resultProduct = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseProduct.Results));
                if (resultProduct != null && resultProduct.Count() > 0)
                {
                    tableData = JsonConvert.DeserializeObject<TableViewModel>(resultProduct.FirstOrDefault().TABLE_CONTENT);
                }
            }
            tableData.TABLE_NAME = TableName.Product;
            tableData.TITLE_TABLE_NAME = FunctionHelpers.GetValueLanguage("Table.Title.Product");
            tableData = RenderTable(tableData, param);

            if (!tableData.IS_PERMISSION)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/_TableContent.cshtml", tableData);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        private TableViewModel RenderTable(TableViewModel tableData, ParamType paramType)
        {
            //model param
            paramType.LANGUAGE_ID = CurrentUser.LANGUAGE_ID;
            paramType.USER_ID = CurrentUser.UserAdmin.USER_ID;
            paramType.ROLE_ID = CurrentUser.UserAdmin.ROLE_ID;

            //Gọi hàm lấy thông tin 

            tableData = GetData(tableData, paramType);
            tableData.TABLE_URL = TableUrl.Product;
            tableData.PAGE_SIZE = paramType.PAGE_SIZE;
            tableData.PAGE_INDEX = paramType.PAGE_NUMBER;
            tableData.MENU_NAME = MenuName.Product;
            tableData.PAGE_SIZE = paramType.PAGE_SIZE;
            tableData.PAGE_INDEX = paramType.PAGE_NUMBER;
            tableData.TABLE_COLUMN_FIELD = tableData.TABLE_COLUMN_FIELD.Where(s => s.IS_SHOW == true).OrderBy(s => s.POSITION).ToList();
            tableData.VALUE_DYNAMIC = paramType.VALUE;
            tableData.STRING_FILTER = paramType.STRING_FILTER;
            tableData.TABLE_EXPORT_URL = TableExportUrl.Product;
            tableData.TABLE_SESION_EXPORT_URL = TableSesionExportUrl.Product;

            if (tableData.TABLE_COLUMN == null || tableData.TABLE_COLUMN.Count() == 0)
            {
                var responseColumn = _tableRepository.GetTableColumnByTableName(tableData.TABLE_NAME);
                if (responseColumn != null && responseColumn.Results.Count() > 0)
                {
                    tableData.TABLE_COLUMN = JsonConvert.DeserializeObject<List<TableColumnModel>>(JsonConvert.SerializeObject(responseColumn.Results));
                }
            }

            if (tableData.SELECT_OPTION == null || tableData.SELECT_OPTION.Count() == 0)
            {
                var responseSelectOption = _tableRepository.GetSelectOption();
                if (responseSelectOption != null && responseSelectOption.Results.Count() > 0)
                {
                    tableData.SELECT_OPTION = JsonConvert.DeserializeObject<List<SelectOptionModel>>(JsonConvert.SerializeObject(responseSelectOption.Results));

                }
            }

            if (tableData.SELECT_OPTION != null && tableData.SELECT_OPTION.Count() > 0)
            {
                foreach (var item in tableData.SELECT_OPTION)
                {
                    if (Convert.ToInt32(item.VALUE) == paramType.PAGE_SIZE)
                    {
                        item.IS_SELECTED = true;
                        break;
                    }
                }
            }
            return tableData;
        }

        public TableViewModel GetData(TableViewModel tableData, ParamType paramType)
        {

            var type = MapperHelper.Map<ParamType, Repository.Type.ParamType> (paramType);
            var response = _productRepository.ProductFromAdmin(type);

            if (response != null)
            {
                if (response.Success == false && CheckPermision(response.StatusCode) == false)
                {

                    tableData.IS_PERMISSION = false;
                }
                else
                {
                    tableData.DATA = response.Results;
                    tableData.TOTAL = Convert.ToInt32(response.OutValue.Parameters["@TotalRecord"].Value.ToString());
                }
            }

            return tableData;
        }

        public ActionResult SesionExport(TableViewModel tableData, ParamType paramType)
        {
            if (String.IsNullOrEmpty(paramType.STRING_FILTER))
            {
                paramType.STRING_FILTER = string.Empty;
            }
            paramType.LANGUAGE_ID = CurrentUser.LANGUAGE_ID;
            paramType.USER_ID = CurrentUser.UserAdmin.USER_ID;
            paramType.ROLE_ID = CurrentUser.UserAdmin.ROLE_ID;
            paramType.STRING_FILTER = paramType.STRING_FILTER;
            paramType.PAGE_NUMBER = 1;
            paramType.PAGE_SIZE = long.MaxValue;
            paramType.VALUE = paramType.VALUE;
            tableData = GetData(tableData, paramType);
            Session["Export"] = tableData.DATA;
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Export(string TABLE_NAME)
        {
            TableViewModel tableData = new TableViewModel();
            tableData.TABLE_NAME = TABLE_NAME;
            var responseProduct = _tableRepository.GetTableByTableName(TABLE_NAME);
            var resultProduct = JsonConvert.DeserializeObject<List<TableModel>>(JsonConvert.SerializeObject(responseProduct.Results));
            if (resultProduct != null && resultProduct.Count() > 0)
            {
                tableData = JsonConvert.DeserializeObject<TableViewModel>(resultProduct.FirstOrDefault().TABLE_CONTENT);
            }

            tableData.TABLE_COLUMN_FIELD = tableData.TABLE_COLUMN_FIELD.Where(s => s.IS_SHOW == true).OrderBy(s => s.POSITION).ToList();
            tableData.DATA = Session["Export"] as List<dynamic>;

            var responseColumn = _tableRepository.GetTableColumnByTableName(TABLE_NAME);
            if (responseColumn != null && responseColumn.Results.Count() > 0)
            {
                tableData.TABLE_COLUMN = JsonConvert.DeserializeObject<List<TableColumnModel>>(JsonConvert.SerializeObject(responseColumn.Results));

            }

            DataTable dt = new DataTable("Grid");
            DataTable dt1 = new DataTable("Grid");
            if (tableData.TABLE_COLUMN_FIELD != null && tableData.TABLE_COLUMN_FIELD.Count() > 0)
            {
                foreach (var item in tableData.TABLE_COLUMN_FIELD)
                {
                    if (item.PRESENTATION != "CHECK_BOX" && item.PRESENTATION != "ACTION")
                    {
                        var column = tableData.TABLE_COLUMN.Where(s => s.COLUMN_NAME == item.COLUMN_NAME).ToList().FirstOrDefault();
                        if (column != null)
                        {
                            dt.Columns.Add(FunctionHelpers.GetValueLocalization(column.ID, "TABLE_COLUMN", "COLUMN_NAME"));
                            dt1.Columns.Add(column.COLUMN_NAME);
                        }
                    }
                }
            }

            var wb = new XLWorkbook();
            wb.AddWorksheet(TABLE_NAME);
            var ws = wb.Worksheet(TABLE_NAME);
            var rowIndex = tableData.DATA.Count();
            var columnIndex = dt1.Columns.Count;
            int Cell = 1;
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                ws.Cell(Cell, i + 1).Value = dt.Columns[i].ToString();

            }
            if (tableData.DATA != null && tableData.DATA.Count() > 0)
            {
                for (int i = 0; i < tableData.DATA.Count(); i++)
                {
                    
                    Cell = Cell + 1;
                    foreach (var item in dt1.Columns)
                    {
                        for (var j = 0; j < dt1.Columns.Count; j++)
                        {
                            var value = string.Empty;
                            var column = tableData.TABLE_COLUMN.Where(s => s.COLUMN_NAME == dt1.Columns[j].ToString()).ToList().FirstOrDefault();
                            if (!string.IsNullOrEmpty(column.DATA_TYPE) && !string.IsNullOrEmpty(column.PROPERTY_NAME))
                            {
                                value = FunctionHelpers.GetValueLocalization(long.Parse(tableData.DATA[i][column.COLUMN_DATA_ID].Value), column.DATA_TYPE, column.PROPERTY_NAME);
                            }
                            else if (tableData.DATA[i][dt1.Columns[j].ToString()] != null && tableData.DATA[i][dt1.Columns[j].ToString()].Value != null)
                            {
                                value = tableData.DATA[i][dt1.Columns[j].ToString()].Value;
                            }
                            ws.Cell(Cell, j + 1).Value = value;
                        }
                    }
                }
            }
            ws.Rows(1, 1).Style.Font.Bold = true;
            for (var j = 1; j <= columnIndex; j++)
            {
                ws.Column(j).AdjustToContents();
            }
            Session["Export"] = null;
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", TABLE_NAME + ".xlsx");
            }
        }
        #endregion
    }
    public class File
    {
        public string color { get; set; } = string.Empty;
        public string size { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
    }
}