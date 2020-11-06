using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ShipFeeModel:BaseModel
    {
        public string NAME { get; set; }
        public float PRICE { get; set; }
        public long CODE_CITY { get; set; }
    }
}