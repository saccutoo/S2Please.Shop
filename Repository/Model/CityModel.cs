using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class CityModel : BaseModel
    {
        public long CODE { get; set; }
        public string NAME_VN { get; set; }
        public long NAME_EN { get; set; }
        public string NAME { get; set; }

    }
}
