using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class OrderModel : BaseModel
    {
        public string ORDER_CODE { get; set; } = string.Empty;
        public string FULL_NAME { get; set; } = string.Empty;
        public string EMAIL { get; set; } = string.Empty;
        public string PHONE { get; set; } = string.Empty;
        public string FAX { get; set; } = string.Empty;
        public long CITY { get; set; }
        public long DISTRICT { get; set; }
        public long COMMUNITY { get; set; }
        public string ADRESS_SPECIFIC { get; set; } = string.Empty;
        public long METHOD_PAY { get; set; }
        public long FEE_SHIP { get; set; }
        public long CUSTOMER_ID { get; set; }
        public long STATUS { get; set; }
        public long STATUS_PAY { get; set; }
        public string BONUS_ID { get; set; } = string.Empty;
        public string DECRIPTION { get; set; } = string.Empty;
        public string ToDecrypt { get; set; } = string.Empty;
        public string STATUS_NAME { get; set; } = string.Empty;
        public string STATUS_PAY_NAME { get; set; } = string.Empty;
        public string METHOD_PAY_NAME { get; set; } = string.Empty;
        public string FEE_SHIP_NAME { get; set; } = string.Empty;
        public string CITY_NAME { get; set; } = string.Empty;
        public string DISTRICT_NAME { get; set; } = string.Empty;
        public string COMMUNITY_NAME { get; set; } = string.Empty;
        public float PRICE_FEE_SHIP { get; set; }
        public float TOTAL_PRICE { get; set; }
        public bool? IS_ORDER { get; set; }

    }
}