using Repository.Model;
using Repository.Type;
using SHOP.COMMON.Helpers;
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

        //Get mail queue render table
        ResultModel GetMaiQueueRenderTable(ParamType paramType, bool isCheckPermission = true);

        //Get mail queue by id
        ResultModel GetMailQueueById(long id);

        //Save mail queue
        ResultModel SaveMailQueue(MailQueueModel model);

        //Delete mail queue by id
        ResultModel DeleteMailQueueById(long id);
    }
}
