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
namespace S2Please.SignalR
{
    public class MessengerHub : Hub
    {
        //private ISystemRepository _systemRepository;
        //private IMessengerRepository _messengerRepository;

        //public MessengerHub(ISystemRepository systemRepository, IMessengerRepository messengerRepository)
        //{
        //    this._systemRepository = systemRepository;
        //    this._messengerRepository = messengerRepository;
        //}
        public MessengerRepository _messengerRepository = new MessengerRepository();
        public void StartChat(string customerName,string email, string phone, string userCustomerId,string html )
        {
            List<ChatModel> chats = new List<ChatModel>();
            try
            {
                html = string.Empty;
                var content = string.Empty;
                ChatModel chat = new ChatModel();
                content += FunctionHelpers.GetValueLanguage("Messenger.IsAutoContent");
                chat.Chat(customerName, email, long.Parse(phone), long.Parse(userCustomerId), content, 1, true, true);
                chat.EMPLOYEE_NAME = "Admin";
                chat.CONTENT = ContentHtmlHelper.ContentMessenger(chat);
                chats.Add(chat);

                ChatModel chat1 = new ChatModel();
                content = string.Empty;
                content = "<p>" + FunctionHelpers.GetValueLanguage("Chat.Name") + " : " + customerName + "</p>";
                content += "<p>Email : " + email + "</p>";
                chat1.Chat(customerName, email, long.Parse(phone), long.Parse(userCustomerId), content, 0, false, true);
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
            }
            catch (Exception ex)
            {

                Clients.All.addNewMessageToPage(customerName, email, phone, userCustomerId, ex);
            }
           

            Clients.All.addNewMessageToPage(customerName,email,phone,userCustomerId, html, JsonConvert.SerializeObject(chats));
        }
    }
}