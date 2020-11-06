using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class ProductDetailViewModel
    {
        public ProductModel Product { get; set; } = new ProductModel();
        public List<ProductImgModel> ProductImgs { get; set; } = new List<ProductImgModel>();
        public List<ProductBonusModel> ProductBonus { get; set; } = new List<ProductBonusModel>();
        public List<ProductColorModel> ProductColors { get; set; } = new List<ProductColorModel>();
        public List<ProductSizeModel> ProductSizes { get; set; } = new List<ProductSizeModel>();
        public ProductColorSizeMapperModel ProductMapper { get; set; } = new ProductColorSizeMapperModel();
        public List<ProductColorSizeMapperModel> ProductMappers { get; set; } = new List<ProductColorSizeMapperModel>();
        public List<ProductModel> RelatedProducts { get; set; } = new List<ProductModel>();
        public List<ProductModel> ViewProducts { get; set; } = new List<ProductModel>();

    }
}