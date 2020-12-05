using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using S2Please.Models;
using SHOP.COMMON;
using Repository;
using S2Please.Helper;
using SHOP.COMMON.Helpers;
using Newtonsoft.Json;
using S2Please.Areas.ADMIN.ViewModel;
using S2Please.ParramType;
namespace S2Please.SignalR
{
    public class MessengerHub : Hub
    {

        public MessengerRepository _messengerRepository = new MessengerRepository();

        public void Hello()
        {
            Clients.All.hello("123");
        }
        public void StartChat(string customerName,string email, string phone, string userCustomerId,string html,string sessionId )
        {
            List<ChatModel> chats = new List<ChatModel>();
            try
            {
                html = string.Empty;
                var content = string.Empty;
                ChatModel chat = new ChatModel();
                content += FunctionHelpers.GetValueLanguage("Messenger.IsAutoContent");
                chat.Chat("Admin", "", 0, long.Parse(userCustomerId), content, 1, sessionId, true, true);
                chat.EMPLOYEE_NAME = "Admin";
                chat.CONTENT = ContentHtmlHelper.ContentMessenger(chat);
                chat.IS_VIEW = true;
                chats.Add(chat);

                ChatModel chat1 = new ChatModel();
                content = string.Empty;
                content = "<p>" + FunctionHelpers.GetValueLanguage("Chat.Name") + " : " + customerName + "</p>";
                content += "<p>"+FunctionHelpers.GetValueLanguage("Cart.TelePhone") +" : " + phone + "</p>";
                content += "<p>Email : " + email + "</p>";
                chat1.Chat(customerName, email, long.Parse(phone), long.Parse(userCustomerId), content, 0,sessionId, false, true);
                chat1.CONTENT = ContentHtmlHelper.ContentMessenger(chat1);
                chats.Add(chat1);

                //Security.SetChatCokkie(chats);


                var typeChat = MapperHelper.Map<ChatModel, Repository.Model.ChatModel>(chat);
                var responseChat = _messengerRepository.SaveMessenger(typeChat);
                var resultChat = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseChat.Results));

                var typeChat1 = MapperHelper.Map<ChatModel, Repository.Model.ChatModel>(chat1);
                var responseChat1 = _messengerRepository.SaveMessenger(typeChat1);
                var resultChat1 = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseChat1.Results));

                html = resultChat.FirstOrDefault().CONTENT + " " + resultChat1.FirstOrDefault().CONTENT;
                SendMessageToAdmin();
            }
            catch (Exception ex)
            {

                Clients.All.addNewMessageToPage(customerName, email, phone, userCustomerId, ex);
            }
           

            Clients.All.addNewMessageToPage(customerName,email,phone,userCustomerId, html, JsonConvert.SerializeObject(chats), sessionId);
        }

        public void SendMessageToAdmin()
        {
            NotificationViewModel vm = new NotificationViewModel();
            ParamType paramType = new ParamType();
            paramType.LANGUAGE_ID = CurrentUser.LANGUAGE_ID;
            paramType.USER_ID = CurrentUser.UserAdmin.USER_ID;
            paramType.ROLE_ID = CurrentUser.UserAdmin.ROLE_ID;
            var type = MapperHelper.Map<ParamType, Repository.Type.ParamType>(paramType);

            var responseMessengers = _messengerRepository.GetTop3MessengerNew(type,false);
            if (responseMessengers != null)
            {
                vm.Total = Convert.ToInt32(responseMessengers.OutValue.Parameters["@TotalRecord"].Value.ToString());
                vm.Messengers = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessengers.Results));
            }
            var html = ContentHtmlHelper.ContentNotificationMessenger(vm);

            Clients.All.sendMessageToAdmin(vm.Total.ToString(), string.Format(FunctionHelpers.GetValueLanguage("Messenger.MessengerTotal"), vm.Total.ToString()), html,true);
        }
    }
}