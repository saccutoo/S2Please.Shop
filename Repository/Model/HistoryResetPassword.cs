using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class HistoryResetPassword : BaseModel
    {
        public string CODE { get; set; }
        public DateTime? DATE_TIME { get; set; }

    }
}
