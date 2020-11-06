using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ColumnValidationModel : BaseModel
    {
        public long COLUM_ID { get; set; }
        public bool IS_REQUIRED { get; set; }
        public long MIN_LENGTH { get; set; }
        public long MAX_LENGTH { get; set; }
        public bool IS_EMAIL { get; set; }
        public bool IS_PHONE { get; set; }
        public bool IS_NUMBER { get; set; }
        public string COLUMN_NAME { get; set; }

    }
}