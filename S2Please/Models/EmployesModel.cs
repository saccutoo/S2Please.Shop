﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class EmployesModel : BaseModel
    {
        public string FULL_NAME { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public long GENDER { get; set; }
        public DateTime DATE_OF_BIRTH { get; set; }
        public string FAX { get; set; }
        public string ADRESS_SPECIFIC { get; set; }
        public long CITY { get; set; }
        public long DISTRICT { get; set; }
        public long COMMUNITY { get; set; }
        public string IMAGE { get; set; }

    }
}