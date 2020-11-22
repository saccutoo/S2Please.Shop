using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class MailQueueModel : BaseModel
    {
        public long DATA_ID { get; set; }
        public string DATA_TYPE { get; set; }
        public string MAIL_TO { get; set; }
        public string MAIL_CC { get; set; }
        public string MAIL_BCC { get; set; }
        public string MAIL_NAME { get; set; }
        public string CONTENT { get; set; }
        public string MESSAGE { get; set; }
        public string STATUS { get; set; }
        public string FROM { get; set; }

    }
}