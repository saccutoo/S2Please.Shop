﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class DistrictModel : BaseModel
    {
        public long CODE { get; set; }
        public long CODE_CITY { get; set; }
        public string NAME_VN { get; set; }
        public long NAME_EN { get; set; }
    }
}

