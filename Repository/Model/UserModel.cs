using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class UserModel : BaseModel
    {
        public long USER_ID { get; set; }
        public long ROLE_ID { get; set; }
        public string USER_NAME { get; set; }
        public string PASS_WORD { get; set; }
        public string PASS_WORD_AGAIN { get; set; }
        public string EMAIL { get; set; }
        public string IS_LOCK { get; set; }
        public bool IS_EMPLOYEE { get; set; }
        public bool IS_CONFIRM { get; set; }
        public string CODE { get; set; }
        public string FULL_NAME { get; set; }

    }
}
