using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class SelectOptionModel : BaseModel
    {
        public string VALUE { get; set; }
        public long ORDER { get; set; }
        public bool IS_SELECTED { get; set; }

    }
}
