using Repository.Model;
using Repository.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public partial interface IMailQueueRepository
    {
        //Get top 5 row mail queue false
        ResultModel GetTop5MailQueueFalse();
    }
}
