using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class HistoryResetPassword:BaseModel
    {
        public string CODE { get; set; }
        public DateTime? DATE_TIME { get; set; }

    }
}