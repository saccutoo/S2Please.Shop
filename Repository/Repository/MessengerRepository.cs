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
            return ListProcedure<ChatModel>(new ChatModel(), "Messenger_update_Messenger", param);
        }
    }
}