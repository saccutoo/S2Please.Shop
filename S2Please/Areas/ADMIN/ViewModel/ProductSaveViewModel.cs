using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Helper;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class ProductSaveViewModel : BaseModel
    {
        public ProductModel Product { get; set; } = new ProductModel();
        public List<dynamic> Types { get; set; } = new List<dynamic>();
        public List<dynamic> Groups { get; set; } = new List<dynamic>();
        public List<ProductColorModel> ProductColors { get; set; } = new List<ProductColorModel>();
        public List<ProductSizeModel> ProductSizes { get; set; } = new List<ProductSizeModel>();
        public List<ProductColorSizeMapperModel> ProductMapp { get; set; } = new List<ProductColorSizeMapperModel>();
        public List<ProductImgModel> ProductSlides { get; set; } = new List<ProductImgModel>();
        public List<FileModel> FileColors { get; set; } = new List<FileModel>();
        public List<FileModel> FileSlides { get; set; } = new List<FileModel>();

    }

}
