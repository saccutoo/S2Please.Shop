using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class ResultModel
    {
        public List<dynamic> Results { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string CacheName { get; set; }
        public string error { get; set; } = string.Empty;
        public SqlCommand OutValue { get; set; } = new SqlCommand();
        public string Message { get; set; }
        public int Total { get; set; }
        public string Html { get; set; }
    }
}
