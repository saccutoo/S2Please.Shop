using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class MenuModel : BaseModel
    {
        public string MENU_NAME { get; set; }
        public long PARENT_ID { get; set; }
        public string LINK { get; set; }
        public string LINK_VIEW { get; set; }
        public int ORDER_MENU { get; set; }
        public long PRODUCT_GROUP_ID { get; set; }
    }
}
