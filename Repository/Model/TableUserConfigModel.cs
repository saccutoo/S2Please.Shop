using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class TableUserConfigModel:BaseModel
    {
        public long USER_ID { get; set; }
        public long TABLE_ID { get; set; }
        public string TABLE_CONTENT { get; set; }
        public string PARAM { get; set; }
        public string TABLE_NAME { get; set; }
    }
}
