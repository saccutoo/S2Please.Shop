using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class Param
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsUserDefinedTableType { get; set; } = false;
        public SqlParameter paramUserDefinedTableType { get; set; }
        public bool IsOutPut { get; set; } = false;
        public string Type { get; set; } = string.Empty;
    }
}
