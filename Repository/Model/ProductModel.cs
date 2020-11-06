using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    class ProductModel:BaseModel
    {
        public long PRODUCT_TYPE_ID { get; set; }
        public string NAME { get; set; }
        public bool? IS_SHOW { get; set; }
        public string STATUS { get; set; }
        public string DECRIPTION { get; set; }
        public string COLOR { get; set; }
        public string SIZE { get; set; }

        public float PRICE { get; set; }
        public long AMOUNT { get; set; }
        public string GROUP_NAME { get; set; }
        public long PRODUCT_GROUP_ID { get; set; }
        public string IMAGE { get; set; }
        public long DISCOUNT_RATE { get; set; }
        public string PRODUCT_TYPE_NAME { get; set; }
        public string PRODUCT_MATERIAL { get; set; }
        public string PRODUCT_ORIGIN { get; set; }
        public string PRODUCT_CODE { get; set; }
        public int PRODUCT_AMOUNT { get; set; }
        public bool IS_NEW { get; set; }
        public string PRODUCT_KEY { get; set; }
    }
}
