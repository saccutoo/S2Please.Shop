using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public partial class MULTI_LANGUAGE
    {
        public long ID { get; set; }
        public long LANGUAGE_ID { get; set; }
        public string KEY { get; set; }
        public string VALUE { get; set; }
    }
}
