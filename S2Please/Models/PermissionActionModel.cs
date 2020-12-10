using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class PermissionActionModel :BaseModel
    {
        public long MENU_ID { get; set; }
        public string PERMISSION_NAME { get; set; }

    }
}