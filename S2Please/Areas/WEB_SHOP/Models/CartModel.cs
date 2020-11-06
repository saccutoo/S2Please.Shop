using S2Please.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Areas.WEB_SHOP.Models
{
    public class CartModel
    {
        public long ID { get; set; } = new Random().Next(10000, 90000);
        public long CHECK_ID { get; set; }
        public long PRODUCT_ID { get; set; }
        public string NAME { get; set; }
        public long COLOR_ID { get; set; }
        public long SIZE_ID { get; set; }
        public long AMOUNT { get; set; }
        public List<ProductBonusModel> ProductBonus { get; set; } = new List<ProductBonusModel>();
        public ProductColorSizeMapperModel ProductColorSizeMapper { get; set; } = new ProductColorSizeMapperModel();

    }
}