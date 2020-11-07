using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Type
{
    public class ImportCountryType
    {
        public long CODE { get; set; }
        public string NAME_VN { get; set; }
        public string NAME_EN { get; set; }
        public long CODE_CITY { get; set; }
        public long CODE_DISTRICT { get; set; }
    }
}
