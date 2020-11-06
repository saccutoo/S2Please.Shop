using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class OrderDetailModel : BaseModel
    {
        public long ORDER_ID { get; set; }
        public long PRODUCT_ID { get; set; }
        public long COLOR_ID { get; set; }
        public long SIZE_ID { get; set; }
        public long AMOUNT { get; set; }
        public float PRICE { get; set; }
        public float RATE { get; set; }
        public string DECRIPTION { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string SIZE_NAME { get; set; }
        public string COLOR { get; set; }
    
    }
}