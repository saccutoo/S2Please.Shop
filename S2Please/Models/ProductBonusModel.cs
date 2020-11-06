using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ProductBonusModel : BaseModel
    {
        public string BONUS_NAME { get; set; }
        public long PRODUCT_ID { get; set; }
    }
}