using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class PermissionViewModel
    {
        public List<dynamic> Roles { get; set; } = new List<dynamic>();
        public List<dynamic> Users { get; set; } = new List<dynamic>();

        public List<MenuModel> Menus { get; set; } = new List<MenuModel>();
        public List<MenuPermissionModel> Permissions { get; set; } = new List<MenuPermissionModel>();

    }
}