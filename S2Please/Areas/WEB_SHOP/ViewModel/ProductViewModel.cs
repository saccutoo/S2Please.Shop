using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.ParramType;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class ProductViewModel
    {
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
        public List<ProductTypeModel> ProductTypes { get; set; } = new List<ProductTypeModel>();
        public List<ProductGroupModel> ProductGroups { get; set; } = new List<ProductGroupModel>();
        public ParamType Param { get; set; } = new ParamType();

    }
}