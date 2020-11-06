using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class TableColumnModel : BaseModel
    {
        public long TABLE_ID { get; set; }
        public string COLUMN_NAME { get; set; }
        public long COLUMN_TYPE { get; set; }
        public string SQL_QUERY { get; set; }
        public string CSS { get; set; }
        public int ORDER { get; set; }
        public bool IS_SHOW { get; set; }
        public bool IS_FILTER { get; set; }
        public bool? IS_SORT { get; set; }
        public string ORIGINAL_COLUMN_ALIAS { get; set; }
        public string ORIGINAL_COLUMN_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public string PROPERTY_NAME { get; set; }
        public bool IS_CHECK { get; set; }
        public string VALUE { get; set; }

    }
}