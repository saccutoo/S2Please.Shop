using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class ProductPriceViewModel
    {
        public ProductColorSizeMapperModel ProductMapper { get; set; } = new ProductColorSizeMapperModel();
        public string Html { get; set; }
    }
}