using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ProductDetailModel :BaseModel
    {
        public long AMOUNT { get; set; }
        public float PRICE { get; set; }
        public string COLOR { get; set; }
        public long PRODUCT_SIZE_ID { get; set; }
        public long PRODUCT_ID { get; set; }
        public bool IS_MAIN_SHOW { get; set; }

    }
}