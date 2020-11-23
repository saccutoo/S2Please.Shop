using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class ProductTypeModel : BaseModel
    {
        public string NAME { get; set; }
        public long PRODUCT_GROUP_ID { get; set; }
        public bool IS_SHOW_VIEW { get; set; }
        public long MENU_ID { get; set; }
        public string MENU_NAME { get; set; }
    }
}
