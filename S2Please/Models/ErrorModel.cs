using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ErrorModel :BaseModel
    {
        public int ERROR_NUM { get; set; }
        public string ERROR_MSG { get; set; }
        public string ERROR_PROC { get; set; }
    }
}