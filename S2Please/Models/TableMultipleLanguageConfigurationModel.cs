using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class TableMultipleLanguageConfigurationModel : BaseModel
    {
        public long TABLE_ID { get; set; }
        public string PROPERTY_NAME { get; set; }
        public string TYPE { get; set; }
        public long ORDER { get; set; }
    }
}