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

        //Get mail queue render table
        public ResultModel GetMaiQueueRenderTable(ParamType paramType, bool isCheckPermission = true)
        {
            var param = new List<Param>();
            var basicParamype = new List<ParamType>();
            basicParamype.Add(paramType);
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamype)
                }
            });
            param.Add(new Param { Key = "@TotalRecord", Value = "0", IsOutPut = true, Type = "Int" });
            return ListProcedure<MailQueueModel>(new MailQueueModel(), "MailQueue_Get_MailQueueFromAdmin", param, false, isCheckPermission);
        }

        //Get mail queue by id
        public ResultModel GetMailQueueById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString()});
            return ListProcedure<MailQueueModel>(new MailQueueModel(), "MailQueue_Get_MailQueueById", param);
        }
    }
}
