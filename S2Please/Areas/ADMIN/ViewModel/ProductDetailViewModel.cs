using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class ProductDetailViewModel : BaseModel
    {      
        public ProductModel Product { get; set; } = new ProductModel();
        public List<ProductColorModel> Colors { get; set; } = new List<ProductColorModel>();
        public List<ProductSizeModel> Sizes { get; set; } = new List<ProductSizeModel>();
        public List<ProductColorSizeMapperModel> ColorSizeMapper { get; set; } = new List<ProductColorSizeMapperModel>();
        public List<ProductImgModel> ProductImgs { get; set; } = new List<ProductImgModel>();
    }
}