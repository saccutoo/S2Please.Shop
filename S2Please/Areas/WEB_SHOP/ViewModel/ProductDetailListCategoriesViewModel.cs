using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class ProductDetailListCategoriesViewModel
    {
        public List<ProductTypeModel> ProductTypes { get; set; } = new List<ProductTypeModel>();
        public string TitleName { get; set; }
        public long ProductTypeID { get; set; }
        public long GroupID { get; set; }

    }
}