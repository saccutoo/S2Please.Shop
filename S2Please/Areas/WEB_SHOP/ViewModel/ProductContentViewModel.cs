using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.ParramType;
using SHOP.COMMON;
namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class ProductContentViewModel
    {
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
        public long Product_Type_Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string StoredProcedureName { get; set; }
        public ParamType Param { get; set; } = new ParamType();
        public int Index { get; set; }
        public string Url { get; set; } = SHOP.COMMON.Url.UrlLoadSlide;
        public int NumberInPage { get; set; } = 4;
        public int NumberProductGet { get; set; } = 12;
        public bool ProductRandome { get; set; } = false;
        public bool IsLazyLoad { get; set; } = true;
        public string FilterString { get; set; } = string.Empty;
    }
}