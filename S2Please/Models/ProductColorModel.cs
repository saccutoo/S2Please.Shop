using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ProductColorModel : BaseModel
    {
        public string COLOR { get; set; }
        public string IMG { get; set; }
        public bool IS_MAIN { get; set; }
        public long AMOUNT { get; set; }

    }
}