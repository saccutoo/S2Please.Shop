using S2Please.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class PermissionActionViewModel
    {
        public List<PermissionActionModel> PermissionActions { get; set; } = new List<PermissionActionModel>();
        public long MENU_ID { get; set; }
    }
}