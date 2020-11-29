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

        //Save mail queue
        public ResultModel  SaveMailQueue(MailQueueModel model)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@DATA_ID", Value = model.DATA_ID.ToString() });
            param.Add(new Param { Key = "@DATA_TYPE", Value = string.IsNullOrEmpty(model.DATA_TYPE) ? " " : model.DATA_TYPE });
            param.Add(new Param { Key = "@MAIL_TO", Value = string.IsNullOrEmpty(model.MAIL_TO) ? " " : model.MAIL_TO });
            param.Add(new Param { Key = "@MAIL_CC", Value = string.IsNullOrEmpty(model.MAIL_CC) ? " " : model.MAIL_CC });
            param.Add(new Param { Key = "@MAIL_BCC", Value = string.IsNullOrEmpty(model.MAIL_BCC) ? " " : model.MAIL_BCC });
            param.Add(new Param { Key = "@MAIL_NAME", Value = string.IsNullOrEmpty(model.MAIL_NAME) ? " " : model.MAIL_NAME });
            param.Add(new Param { Key = "@CONTENT", Value = string.IsNullOrEmpty(model.CONTENT) ? " " : model.CONTENT });
            param.Add(new Param { Key = "@MESSAGE", Value = string.IsNullOrEmpty(model.MESSAGE) ? " " : model.MESSAGE });
            param.Add(new Param { Key = "@STATUS", Value = string.IsNullOrEmpty(model.STATUS) ? " " : model.STATUS });
            param.Add(new Param { Key = "@FROM", Value = string.IsNullOrEmpty(model.FROM) ? " " : model.FROM });
            return ListProcedure<MailQueueModel>(new MailQueueModel(), "MailQueue_Update_MailQueue", param,false,true);
        }

        //Delete mail queue by id
        public ResultModel DeleteMailQueueById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<MailQueueModel>(new MailQueueModel(), "MailQueue_Delete_MailQueueById", param);
        }
    }
}
