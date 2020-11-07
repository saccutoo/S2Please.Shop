using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Type
{
    public class ProductImgType
    {
        public long ID { get; set; }
        public string PRODUCT_IMAGE { get; set; }
        public long PRODUCT_ID { get; set; }
        public string TITLE { get; set; }
        public string DECRIPTION { get; set; }
        public bool IS_MAIN { get; set; }
        public bool IS_SHOW_COLOR { get; set; }
        public bool IS_SHOW_SLIDE { get; set; }
        public long PRODUCT_DETAIL_ID { get; set; }

    }
}
