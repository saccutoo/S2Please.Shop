using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class MenuPermissionModel:BaseModel
    {
        public string MENU_NAME { get; set; }
        public long MENU_PERMISSION_ID { get; set; }
        public long PERMISSION_ID { get; set; }
        public long ORDER_MENU { get; set; }
        public long EMPLOYEE_ROLE_PERMISSON_ID { get; set; }
        public bool IS_CHECK { get; set; }
    }
}
