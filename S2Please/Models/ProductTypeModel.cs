using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ProductTypeModel : BaseModel
    {
        public string NAME { get; set; }
        public long PRODUCT_GROUP_ID { get; set; }

    }
}