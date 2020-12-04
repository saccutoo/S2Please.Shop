using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ChatModel:BaseModel
    {
        public string CUSTOMER_NAME { get; set; }
        public string EMAIL { get; set; }
        public long PHONE { get; set; }
        public long ROLE_ID { get; set; }
        public bool IS_ADMIN { get; set; }
        public long EMPLOYEE_ID { get; set; }
        public string EMPLOYEE_NAME{ get; set; }
        public long USER_CUSTOMER_ID { get; set; }
        public string CONTENT { get; set; }
        public bool IS_EMPLOYESS { get; set; }
        public bool IS_AUTO_CONTENT { get; set; }
        public DateTime? DATE_SEND { get; set; }

        public void Chat(string name,string email, long phone, long userId,string content,long employeeId, bool is_admin = false, bool is_auto_content=false)
        {
            CUSTOMER_NAME = name;
            EMAIL = email;
            USER_CUSTOMER_ID = userId;
            CONTENT = content;
            EMPLOYEE_ID = employeeId;
            PHONE = phone;
            IS_AUTO_CONTENT = is_auto_content;
            IS_ADMIN = is_admin;
            DATE_SEND = DateTime.Now;
        }
    }
}