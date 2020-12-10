using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class MenuViewModel : BaseModel
    {      
        public TableViewModel Table { get; set; } = new TableViewModel();
        public MenuModel Menu { get; set; } = new MenuModel();
        public List<dynamic> Menus { get; set; } = new List<dynamic>();
        public List<dynamic> Groups { get; set; } = new List<dynamic>();
        public LocalizadataViewModel Localiza { get; set; } = new LocalizadataViewModel();
        public List<dynamic> PermissionActions { get; set; } = new List<dynamic>();
        public bool Is_Save { get; set; } = true;
    }
}