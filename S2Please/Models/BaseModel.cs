using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class BaseModel
    {
        public long ID { get; set; }
        public long CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }

        public long UPDATED_BY { get; set; }

        public DateTime? UPDATED_DATE { get; set; }

        public bool? IS_DELETED { get; set; }

    }
}