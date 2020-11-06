using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Type
{
    public class ParamType
    {
        public string STRING_FILTER { get; set; } 
        public string ORDER_BY { get; set; }
        public long USER_ID { get; set; } 
        public long ROLE_ID { get; set; }
        public long PAGE_NUMBER { get; set; }
        public long PAGE_SIZE { get; set; }
        public long LANGUAGE_ID { get; set; } 
        public string VALUE { get; set; }
    }
}
