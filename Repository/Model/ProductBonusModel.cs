using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class ProductBonusModel : BaseModel
    {
        public string BONUS_NAME { get; set; }
        public long PRODUCT_ID { get; set; }
    }
}
