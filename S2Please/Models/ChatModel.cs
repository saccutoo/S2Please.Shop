using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ChatModel:BaseModel
    {
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public long USER_ID { get; set; }
        public string CONTENT { get; set; }
        public bool IS_EMPLOYESS { get; set; }
        public bool IS_AUTO_CONTENT { get; set; }
        public DateTime DATE_SEND { get; set; }

        public void Chat(string name,string email, long userId,string content, DateTime dateSend, bool is_employess=true,bool is_auto_content=false)
        {
            NAME = name;
            EMAIL = email;
            USER_ID = userId;
            CONTENT = content;
            DATE_SEND = dateSend;
            IS_EMPLOYESS = is_employess;
            IS_AUTO_CONTENT = is_auto_content;
        }
    }
}