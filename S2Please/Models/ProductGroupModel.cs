using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ProductGroupModel : BaseModel
    {
        public string GROUP_NAME { get; set; }
        public bool IS_SHOW { get; set; }
        public int ORDER { get; set; }
        public long TYPE { get; set; }

    }
}