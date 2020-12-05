using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHOP.COMMON.Entity
{
    public class ChatModel
    {
        public string CUSTOMER_NAME { get; set; }
        public string EMAIL { get; set; }
        public long PHONE { get; set; }
        public long ROLE_ID { get; set; }
        public bool IS_ADMIN { get; set; }
        public long EMPLOYEE_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public long USER_CUSTOMER_ID { get; set; }
        public string CONTENT { get; set; }
        public bool IS_EMPLOYESS { get; set; }
        public bool IS_AUTO_CONTENT { get; set; }
        public DateTime? DATE_SEND { get; set; }
        public bool IS_VIEW { get; set; }
        public bool IS_REP { get; set; }
        public string SESSION_ID { get; set; }

    }
}
