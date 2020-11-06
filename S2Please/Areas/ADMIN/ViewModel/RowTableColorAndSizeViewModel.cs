using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Helper;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class RowTableColorAndSizeViewModel : BaseModel
    {
        public List<dynamic> Colors { get; set; } = new List<dynamic>();
        public List<dynamic> Sizes { get; set; } = new List<dynamic>();
        public List<ProductColorSizeMapperModel> ColorSizeMapper { get; set; } = new List<ProductColorSizeMapperModel>();
     

    }
}