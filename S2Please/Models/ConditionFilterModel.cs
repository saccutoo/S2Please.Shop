using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ConditionFilterModel : BaseModel
    {
        public string NAME { get; set; }
        public string CONDITION { get; set; }
        public string GROUP_FILTER_TYPE { get; set; }
    }
}