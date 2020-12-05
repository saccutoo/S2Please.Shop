using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHOP.COMMON.Entity
{
    public class User
    {
        public long ID { get; set; }
        public long USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string PASS_WORD { get; set; }
        public string EMAIL { get; set; }
        public bool IS_EMPLOYEE { get; set; }
        public string IS_LOCK { get; set; }
        public long ROLE_ID { get; set; }
        public string FULL_NAME { get; set; }
        public string PHONE { get; set; }

    }
}
