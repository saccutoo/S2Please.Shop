using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class SelectOptionModel : BaseModel
    {
        public string VALUE { get; set; }
        public long ORDER { get; set; }
        public bool IS_SELECTED { get; set; }

    }
}