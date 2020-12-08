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
        public UserRepository _userRepository = new UserRepository();

        public void Hello()
        {
            Clients.All.hello("123");
        }

        #region messager-chatbox
        public void StartChat(string customerName, string email, string phone, string userCustomerId, string html, string sessionId)
        {
            List<ChatModel> chats = new List<ChatModel>();
            try
            {
                html = string.Empty;
                var content = string.Empty;
                ChatModel chat = new ChatModel();
                content += FunctionHelpers.GetValueLanguage("Messenger.IsAutoContent");
                chat.Chat("Admin", "", 0, long.Parse(userCustomerId), content, content, 1, sessionId, true, true);
                chat.EMPLOYEE_NAME = "Admin";
                chat.IS_VIEW = true;
                chats.Add(chat);
                chat.CONTENT = ContentHtmlHelper.ContentMessenger(chats);

                ChatModel chat1 = new ChatModel();
                content = string.Empty;
                content = FunctionHelpers.GetValueLanguage("Chat.Name") + " : " + customerName + "<br/>";
                content += FunctionHelpers.GetValueLanguage("Cart.TelePhone") + " : " + phone + "<br/>";
                content += "Email : " + email;
                chat1.Chat(customerName, email, long.Parse(phone), long.Parse(userCustomerId), content, content, 0, sessionId, false, true);
                chats = new List<ChatModel>();
                chats.Add(chat1);
                chat1.CONTENT = ContentHtmlHelper.ContentMessenger(chats);

                //Security.SetChatCokkie(chats);

                chats = new List<ChatModel>();
                chats.Add(chat);
                chats.Add(chat1);

                var typeChat = MapperHelper.Map<ChatModel, Repository.Model.ChatModel>(chat);
                var responseChat = _messengerRepository.SaveMessenger(typeChat);
                var resultChat = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseChat.Results));

                var typeChat1 = MapperHelper.Map<ChatModel, Repository.Model.ChatModel>(chat1);
                var responseChat1 = _messengerRepository.SaveMessenger(typeChat1);
                var resultChat1 = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseChat1.Results));

                html = resultChat.FirstOrDefault().CONTENT + " " + resultChat1.FirstOrDefault().CONTENT;
                SendMessageToAdmin(true);
                ReloadListMessage(string.Empty, string.Empty);
                Clients.All.addNewMessageToPage(customerName, email, phone, userCustomerId, html, JsonConvert.SerializeObject(chats), sessionId);
            }
            catch (Exception ex)
            {

                Clients.All.addNewMessageToPage(customerName, email, phone, userCustomerId, ex);
            }
        }

        public void SendMessageToAdmin(bool sound = false)
        {
            NotificationViewModel vm = new NotificationViewModel();
            ParamType paramType = new ParamType();
            paramType.LANGUAGE_ID = CurrentUser.LANGUAGE_ID;
            paramType.USER_ID = CurrentUser.UserAdmin.USER_ID;
            paramType.ROLE_ID = CurrentUser.UserAdmin.ROLE_ID;
            var type = MapperHelper.Map<ParamType, Repository.Type.ParamType>(paramType);

            var responseMessengers = _messengerRepository.GetTop3MessengerNew(type, false);
            if (responseMessengers != null)
            {
                vm.Total = Convert.ToInt32(responseMessengers.OutValue.Parameters["@TotalRecord"].Value.ToString());
                vm.Messengers = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessengers.Results));
            }
            var html = ContentHtmlHelper.ContentNotificationMessenger(vm);

            Clients.All.sendMessageToAdmin(vm.Total.ToString(), string.Format(FunctionHelpers.GetValueLanguage("Messenger.MessengerTotal"), vm.Total.ToString()), html, sound);
        }

        public void SendMessageFromAdmin(long userId, string content, string sessionId)
        {
            List<ChatModel> chats = new List<ChatModel>();
            try
            {
                UserModel user = new UserModel();
                var responseUser = _userRepository.GetUserByUserId(userId);
                var resultUser = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(responseUser.Results));
                if (resultUser != null && resultUser.Count() > 0)
                {
                    user = resultUser.FirstOrDefault();
                }

                ChatModel chat = new ChatModel();
                chat.Chat(user.USER_NAME, "", 0, 0, content, content, userId, sessionId, true, false);
                chat.EMPLOYEE_NAME = user.FULL_NAME;
                chat.IS_VIEW = true;
                chats.Add(chat);
                chat.CONTENT = ContentHtmlHelper.ContentMessengerFromAdmin(chats);

                var typeChat = MapperHelper.Map<ChatModel, Repository.Model.ChatModel>(chat);
                var responseChat = _messengerRepository.SaveMessenger(typeChat);
                if (responseChat != null && responseChat.Success)
                {
                    var resultChat = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseChat.Results));

                    var responseMessenger = _messengerRepository.GetMessengerBySessionId(sessionId, true, false);
                    var resultMessenger = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessenger.Results));
                    if (resultMessenger != null && resultMessenger.Count > 0)
                    {
                        content = ContentHtmlHelper.ContentMessengerFromAdmin(resultMessenger);
                        ReloadContentMessageAdmin(sessionId, userId, content);
                        content = ContentHtmlHelper.ContentMessenger(resultMessenger);
                        ReloadContentMessageWeb(sessionId, 0, content, JsonConvert.SerializeObject(chats));
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendMessageFromWeb(string customerName, string email, string phone, long userCustomerId, string content, string sessionId)
        {
            List<ChatModel> chats = new List<ChatModel>();
            try
            {
                ChatModel chat = new ChatModel();
                chat.Chat(customerName, email, long.Parse(phone), userCustomerId, content, content, 0, sessionId, false, false);
                chat.IS_VIEW = false;
                chats.Add(chat);
                chat.CONTENT = ContentHtmlHelper.ContentMessenger(chats);

                var typeChat = MapperHelper.Map<ChatModel, Repository.Model.ChatModel>(chat);
                var responseChat = _messengerRepository.SaveMessenger(typeChat);
                if (responseChat != null && responseChat.Success)
                {
                    var resultChat = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseChat.Results));

                    var responseMessenger = _messengerRepository.GetMessengerBySessionId(sessionId, false, false);
                    var resultMessenger = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessenger.Results));
                    if (resultMessenger != null && resultMessenger.Count > 0)
                    {
                        content = ContentHtmlHelper.ContentMessenger(resultMessenger);
                        ReloadContentMessageWeb(sessionId, 0, content, JsonConvert.SerializeObject(chats));
                        content = ContentHtmlHelper.ContentMessengerFromAdmin(resultMessenger);
                        ReloadContentMessageAdmin(sessionId, 0, content);
                        ReloadListMessage(string.Empty, sessionId);
                        SendMessageToAdmin(false);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        //Reload danh mục messenger admin

        public void ReloadListMessage(string content, string sessionId)
        {
            NotificationViewModel vm = new NotificationViewModel();
            var responseMessengers = _messengerRepository.GetMessengerIsMain();
            var resultMessengers = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessengers.Results));
            if (resultMessengers != null && resultMessengers.Count > 0)
            {
                vm.Messengers = resultMessengers.OrderByDescending(s => s.DATE_SEND).ToList();
            }
            content = ContentHtmlHelper.ContentListMessage(vm.Messengers, sessionId);
            Clients.All.reloadListMessage(content, sessionId);
        }

        //public void Reload content admin

        public void ReloadContentMessageAdmin(string sessionId, long userId, string content)
        {
            Clients.All.reloadContentMessageAdmin(sessionId, userId, content);
        }

        //public void Reload content web

        public void ReloadContentMessageWeb(string sessionId, long userId, string content, string chats)
        {
            Clients.All.reloadContentMessageWeb(sessionId, userId, content, chats);
        }
        #endregion

        #region Notifications
        #endregion
    }
}