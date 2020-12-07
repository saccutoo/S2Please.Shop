﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using SHOP.COMMON;
using S2Please.Helper;
using S2Please.Areas.ADMIN.ViewModel;
namespace S2Please.Helper
{
    public static class ContentHtmlHelper
    {
        public static string ContentMessenger(List<ChatModel> models)
        {
            var html = string.Empty;
            foreach (var model in models)
            {
                if (model.EMPLOYEE_ID != 0 )
                {
                    html += @"<div class='media media-chat' title=' {{EMPLOYEE_NAME}} '>
                                    <img class='avatar' src='/Image/administrator-male.png' alt='...' >
                                    <div class='media-body'>
                                        <p>{{CONTENT_TEXT}}</p>
                                        <p class='meta'><time datetime = '2018' > {{SEND_DATE}} </time></p>
                                    </div >
                                </div>";
                    html = html.Replace("{{EMPLOYEE_NAME}}", model.EMPLOYEE_NAME).Replace("{{CONTENT_TEXT}}", model.CONTENT_TEXT).Replace("{{SEND_DATE}}", FunctionHelpers.getTimeAgo(model.DATE_SEND.Value));
                }
                else if (model.USER_CUSTOMER_ID != 0 || model.EMPLOYEE_ID == 0)
                {
                    html += @"<div class='media media-chat media-chat-reverse' title='{{CUSTOMER_NAME}}'>
                                        <div class='media-body'>
                                              <p>{{CONTENT_TEXT}}</p>
                                            <p class='meta' style='color: #9b9b9b;'><time datetime = '2018' > {{SEND_DATE}} </time></p>
                                        </div>
                                    </div>";
                    html = html.Replace("{{CUSTOMER_NAME}}", model.CUSTOMER_NAME).Replace("{{CONTENT_TEXT}}", model.CONTENT_TEXT).Replace("{{SEND_DATE}}", FunctionHelpers.getTimeAgo(model.DATE_SEND.Value));
                }
            }        
            return html.Trim();
        }

        public static string ContentNotificationMessenger(NotificationViewModel vm)
        {
            var html = string.Empty;
            if (vm.Messengers!=null && vm.Messengers.Count()>0)
            {
                foreach (var item in vm.Messengers)
                {
                    html += " " + @"<a href='/admin/message?sessionId={{SESSION_ID}}'>
                                        <div class='mess__item click-messenger' value-session='{{SESSION_ID}}'>
                                            <div class='image img-cir img-40'>
                                                <img src = '/Image/depositphotos_96173202-stock-illustration-customer-flat-icon.jpg' alt='Michelle Moreno' />
                                            </div>
                                            <div class='content'>
                                                <h6>{{CUSTOMER_NAME}}</h6>
                                                {{CONTENT_TEXT}}
                                                <p>{{TOTAL_NEW_MESSENGER}}</p>
                                                <span class='time'>{{TIME_AGO}}</span>
                                            </div>
                                     </div>
                                    </a>";
                    html = html.Replace("{{CUSTOMER_NAME}}",item.CUSTOMER_NAME).Replace("{{TOTAL_NEW_MESSENGER}}",string.Format(FunctionHelpers.GetValueLanguage("Messenger.MessengerTotalNewBySession"), item.TOTAL)).Replace("{{TIME_AGO}}",FunctionHelpers.getTimeAgo(item.DATE_SEND.Value)).Replace("{{SESSION_ID}}", item.SESSION_ID).Replace("{{CONTENT_TEXT}}", item.CONTENT_TEXT);
                }
            }
            if (vm.Messengers != null && vm.Messengers.Count() > 0)
            {
                html += " " + @"<div class='mess__footer'>
                                    <a href='/admin/message'>{{VIEW_ALL_MESSENGER}}</a>
                                </div>";
                html = html.Replace("{{VIEW_ALL_MESSENGER}}", FunctionHelpers.GetValueLanguage("Messenger.MessengerViewAll"));
            }
            return html.Trim();
        }

        public static string ContentMessengerFromAdmin(List<ChatModel> models)
        {
            var html = string.Empty;
            html+= @"<input id='session-id' value='"+ models.FirstOrDefault().SESSION_ID +"' hidden />";
            foreach (var model in models)
            {
                if (model.EMPLOYEE_ID != 0)
                {
                    html += @"<div class='outgoing_msg' title='{{CUSTOMER_NAME}}'>
                                        <div class='sent_msg'>
                                            <p>{{CONTENT_TEXT}}</p>
                                            <span class='time_date'>{{DATE_SEND}}</span>
                                        </div>
                                    </div>";
                    html = html.Replace("{{CUSTOMER_NAME}}", model.CUSTOMER_NAME).Replace("{{CONTENT_TEXT}}", model.CONTENT_TEXT).Replace("{{DATE_SEND}}", FunctionHelpers.getTimeAgo(model.DATE_SEND.Value));
                }
                else if (model.USER_CUSTOMER_ID != 0 || model.EMPLOYEE_ID == 0)
                {
                    html += @"<div class='incoming_msg'>
                                        <div class='incoming_msg_img'> <img src='/Image/user-profile.png' alt='sunil' title='{{CUSTOMER_NAME}}'> </div>
                                        <div class='received_msg'>
                                            <div class='received_withd_msg'>
                                                <span>{{CONTENT_TEXT}}</span>
                                                <span class='time_date'> {{DATE_SEND}} </span>
                                            </div>
                                        </div>
                                    </div>";
                    html = html.Replace("{{CUSTOMER_NAME}}", model.CUSTOMER_NAME).Replace("{{CONTENT_TEXT}}", model.CONTENT_TEXT).Replace("{{DATE_SEND}}", FunctionHelpers.getTimeAgo(model.DATE_SEND.Value));
                }
            }
            
            return html.Trim();
        }
    }
}