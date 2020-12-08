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
            param.Add(new Param() { Key = "@CONTENT_TEXT", Value = string.IsNullOrEmpty(model.CONTENT_TEXT) ? " " : model.CONTENT_TEXT });
            param.Add(new Param() { Key = "@IS_AUTO_CONTENT", Value =model.IS_AUTO_CONTENT.ToString()  });
            param.Add(new Param() { Key = "@IS_VIEW", Value = model.IS_VIEW.ToString() });
            param.Add(new Param() { Key = "@IS_REP", Value = model.IS_REP.ToString() });
            param.Add(new Param() { Key = "@SESSION_ID", Value = string.IsNullOrEmpty(model.SESSION_ID) ? " " : model.SESSION_ID });
            return ListProcedure<ChatModel>(new ChatModel(), "Messenger_update_Messenger", param);
        }

        //Get 3 messenger new 
        public ResultModel GetTop3MessengerNew(ParamType paramType,bool isCheckPermission=false)
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


        //Get messenger by session id 
        public ResultModel GetMessengerBySessionId(string sessionId,bool isUpdate=true,bool isCheckPermission=false)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@SESSION_ID", Value = sessionId });
            param.Add(new Param { Key = "@IS_UPDATE", Value = isUpdate.ToString() });

            return ListProcedure<ChatModel>(new ChatModel(), "Messenger_Get_GetMessengerBySessionId", param, false, isCheckPermission);
        }

        //Get messenger IS MAIN 
        public ResultModel GetMessengerIsMain(bool isCheckPermission = false)
        {
            var param = new List<Param>();
            return ListProcedure<ChatModel>(new ChatModel(), "Messenger_Get_GetMessengerIsMain", param, false, isCheckPermission);
        }

        //Get top 10  notification new 
        public ResultModel GetTop10NotificationNew(ParamType paramType, bool isCheckPermission = false)
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
            return ListProcedure<NotificationModel>(new NotificationModel(), "Notification_Get_GetTop10NotificationNew", param, false, isCheckPermission);
        }

        //Save notification
        public ResultModel SaveNotification(NotificationModel model, bool isCheckPermission = false)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@DATA_ID", Value = model.DATA_ID.ToString() });
            param.Add(new Param { Key = "@DATA_TYPE", Value = string.IsNullOrEmpty(model.DATA_TYPE) ? " " : model.DATA_TYPE });
            param.Add(new Param { Key = "@CONTENT", Value = string.IsNullOrEmpty(model.CONTENT) ? " " : model.CONTENT });
            param.Add(new Param { Key = "@URL", Value = string.IsNullOrEmpty(model.URL) ? " " : model.URL });
            param.Add(new Param { Key = "@ICON", Value = model.ICON.ToString() });
            param.Add(new Param { Key = "@IS_VIEW", Value = model.IS_VIEW.ToString() });
            param.Add(new Param { Key = "@CREATED_BY", Value = model.CREATED_BY.ToString() });
            param.Add(new Param { Key = "@UPDATED_BY", Value = model.UPDATED_BY.ToString() });
            return ListProcedure<NotificationModel>(new NotificationModel(), "Notification_Update_SaveNotification", param, false, isCheckPermission);
        }
    }
}