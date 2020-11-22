using Repository.Model;
using Repository.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHOP.COMMON.Helpers;

namespace Repository
{
    public partial class MailQueueRepository : CommonRepository, IMailQueueRepository
    {
        //Get top 5 row mail queue false
        public ResultModel GetTop5MailQueueFalse()
        {
            var param = new List<Param>();
            return ListProcedure<MailQueueModel>(new MailQueueModel(), "MailQueue_Get_GetTop5MailQueueFalse", param);
        }

      
    }
}
