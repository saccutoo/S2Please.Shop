using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Helper;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class ProductGroupSaveViewModel : BaseModel
    {
        public ProductGroupModel ProductGroup{ get; set; } = new ProductGroupModel();
        public LocalizadataViewModel Localiza { get; set; } = new LocalizadataViewModel();
        public List<dynamic> Types { get; set; } = new List<dynamic>();
        public bool Is_Save { get; set; } = true;
    }

}
