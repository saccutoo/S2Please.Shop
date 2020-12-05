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
    public partial class MessengerRepository : CommonRepository, IMessengerRepository
    {

        //save message
        public ResultModel SaveMessenger(ChatModel model)
        {
            var param = new List<Param>();
            param.Add(new Param() { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param() { Key = "@EMPLOYEE_ID", Value = model.EMPLOYEE_ID.ToString() });
            param.Add(new Param() { Key = "@USER_CUSTOMER_ID", Value = model.ROLE_ID.ToString() });
            param.Add(new Param() { Key = "@CUSTOMER_NAME", Value = string.IsNullOrEmpty(model.CUSTOMER_NAME) ? " " : model.CUSTOMER_NAME });
            param.Add(new Param() { Key = "@EMAIL", Value = string.IsNullOrEmpty(model.EMAIL) ? " " : model.EMAIL });
            param.Add(new Param() { Key = "@PHONE", Value = model.PHONE.ToString() });
            param.Add(new Param() { Key = "@CONTENT", Value = string.IsNullOrEmpty(model.CONTENT) ? " " : model.CONTENT });
            param.Add(new Param() { Key = "@IS_AUTO_CONTENT", Value =model.IS_AUTO_CONTENT.ToString()  });
            param.Add(new Param() { Key = "@IS_VIEW", Value = model.IS_VIEW.ToString() });
            param.Add(new Param() { Key = "@IS_REP", Value = model.IS_REP.ToString() });
            param.Add(new Param() { Key = "@SESSION_ID", Value = string.IsNullOrEmpty(model.SESSION_ID) ? " " : model.SESSION_ID });
            return ListProcedure<ChatModel>(new ChatModel(), "Messenger_update_Messenger", param);
        }

        //Get 3 messenger new 
        public ResultModel GetTop3MessengerNew(ParamType paramType,bool isCheckPermission=true)
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
            return ListProcedure<ChatModel>(new ChatModel(), "Messenger_Get_GetTop3MessengerNew", param,false, isCheckPermission);
        }
    }
}