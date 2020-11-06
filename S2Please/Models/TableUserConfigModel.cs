using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Areas.WEB_SHOP.Models;


namespace S2Please.Models
{
    public class TableUserConfigModel : BaseModel
    {
        public long USER_ID { get; set; }
        public long TABLE_ID { get; set; }
        public string TABLE_CONTENT { get; set; }
        public string PARAM { get; set; }
        public string TABLE_NAME { get; set; }

    }
}