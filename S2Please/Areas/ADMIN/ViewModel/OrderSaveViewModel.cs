using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Helper;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class OrderSaveViewModel : BaseModel
    {
        public CustomerModel Customer { get; set; } = new CustomerModel();
        public List<dynamic> Citys { get; set; } = new List<dynamic>();
        public TableViewModel Table { get; set; } = new TableViewModel();
        public ProductModel Product { get; set; } = new ProductModel();
        public List<ProductImgModel> ProductImgs { get; set; } = new List<ProductImgModel>();
        public List<ProductSizeModel> ProductSizes { get; set; } = new List<ProductSizeModel>();
        public List<ProductColorModel> ProductColors { get; set; } = new List<ProductColorModel>();
        public ProductColorSizeMapperModel ProductMapper { get; set; } = new ProductColorSizeMapperModel();

    }

}
