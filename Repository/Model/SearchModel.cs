using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class SearchModel : BaseModel
    {
        public long CODE { get; set; }
        public string NAME { get; set; }
        public string DECRIPTION { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string CITY_NAME { get; set; }
        public string DISTRICT_NAME { get; set; }
        public string COMMUNITY_NAME { get; set; }
        public string ADRESS_SPECIFIC { get; set; }
        public string GROUP_NAME { get; set; }

    }
}
