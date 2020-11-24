using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class TableMultipleLanguageConfigurationModel : BaseModel
    {
        public long TABLE_ID { get; set; }
        public string PROPERTY_NAME { get; set; }
        public string TYPE { get; set; }
        public long ORDER { get; set; }
    }
}
