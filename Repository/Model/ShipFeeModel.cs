using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class ShipFeeModel : BaseModel
    {
        public string NAME { get; set; }
        public float PRICE { get; set; }
        public long CODE_CITY { get; set; }
    }
}
