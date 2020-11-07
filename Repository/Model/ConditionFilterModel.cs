using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class ConditionFilterModel : BaseModel
    {
        public string NAME { get; set; }
        public string CONDITION { get; set; }
        public string GROUP_FILTER_TYPE { get; set; }
    }
}
