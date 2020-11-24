using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class LocalizationModel : BaseModel
    {
        public long DATA_ID { get; set; }
        public string DATA_TYPE { get; set; }
        public long LANGUAGE_ID { get; set; }
        public string PROPERTY_NAME { get; set; }
        public string PROPERTY_VALUE { get; set; }
        public bool IS_DEFUALT { get; set; }

    }
}
