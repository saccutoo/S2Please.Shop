using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Helper;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class ProductTypeSaveViewModel : BaseModel
    {
        public ProductTypeModel ProductType { get; set; } = new ProductTypeModel();
        public List<dynamic> Menus { get; set; } = new List<dynamic>();
        public List<dynamic> Groups { get; set; } = new List<dynamic>();
        public LocalizadataViewModel Localiza { get; set; } = new LocalizadataViewModel();
    }

}
