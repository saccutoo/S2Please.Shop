using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.ParramType;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class ProductListViewModel
    {
        //list sản phẩm
        public List<ProductModel> Products { get; set; } = new List<ProductModel>(); 
        public ParamType Parram { get; set; } = new ParamType();
        public List<string> ProductTypes { get; set; } = new List<string>();
        public List<string> ProductGroups { get; set; } = new List<string>();
        public int Total { get; set; }
        public string Url { get; set; }
        public List<ProductTypeModel> DataProductTypes { get; set; } = new List<ProductTypeModel>();
        public int FromMoney { get; set; }
        public int ToMoney { get; set; }

    }
}