using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    class PermissionActionModel:BaseModel
    {
        public long MENU_ID { get; set; }
        public string PERMISSION_NAME { get; set; }
    }
}
