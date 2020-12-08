using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class NotificationModel:BaseModel
    {
        public long DATA_ID { get; set; }
        public string DATA_TYPE { get; set; }
        public string CONTENT { get; set; }
        public string URL { get; set; }
        public string ICON { get; set; }

        public bool IS_VIEW { get; set; }
    }
}
