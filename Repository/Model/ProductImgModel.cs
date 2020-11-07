using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class ProductImgModel : BaseModel
    {
        public string PRODUCT_IMAGE { get; set; }
        public long PRODUCT_ID { get; set; }
        public bool IS_MAIN { get; set; }
        public bool IS_SHOW_SLIDE { get; set; }
        public long PRODUCT_DETAIL_ID { get; set; }

    }
}
