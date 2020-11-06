using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class FileModel : BaseModel
    {
        public string Base64 { get; set; }
        public int Index { get; set; }
        public string FILE_NAME { get; set; }
        public string SIZE { get; set; }
        public string TYPE { get; set; }
        public string COLOR { get; set; }

    }
}