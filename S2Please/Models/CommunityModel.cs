using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class CommunityModel : BaseModel
    {
        public long CODE { get; set; }
        public long CODE_DISTRICT { get; set; }
        public string NAME_VN { get; set; }
        public long NAME_EN { get; set; }
    }
}