using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ListColumnFilterModel : BaseModel
    {
        public long COLUMN_ID { get; set; }
        public long CONDITION_ID { get; set; }
        public string VALUE { get; set; }
        public int? POSITION { get; set; }
        public bool? IS_SHOW { get; set; }
    }
}