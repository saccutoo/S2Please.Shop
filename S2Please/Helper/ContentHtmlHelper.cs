using System;
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
        public static string ContentMessenger(ChatModel model)
        {
            var html = string.Empty;

            if (model.EMPLOYEE_ID!=0 || model.IS_ADMIN)
            {
                html += " " + @"<div class='media media-chat' title=' {{EMPLOYEE_NAME}} '>
                                    <img class='avatar' src='/Image/administrator-male.png' alt='...' >
                                    <div class='media-body'>
                                        <p>{{CONTENT}}</p>
                                        <p class='meta'><time datetime = '2018' > {{SEND_DATE}} </ time ></ p >
                                    </ div >
                                </div>";
                html=html.Trim().Replace("{{EMPLOYEE_NAME}}", model.EMPLOYEE_NAME).Replace("{{CONTENT}}", model.CONTENT).Replace("{{SEND_DATE}}", model.DATE_SEND.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            else if (model.USER_CUSTOMER_ID!=0 || !string.IsNullOrEmpty(model.CUSTOMER_NAME)) {
                html += " " + @"<div class='media media-chat media-chat-reverse' title='{{CUSTOMER_NAME}}'>
                                        <div class='media-body'>
                                             {{CONTENT}}
                                            <p class='meta' style='color: #9b9b9b;'><time datetime = '2018' > {{SEND_DATE}} </ time ></ p >
                                        </ div >
                                    </div>";
                html=html.Trim().Replace("{{CUSTOMER_NAME}}", model.CUSTOMER_NAME).Replace("{{CONTENT}}", model.CONTENT).Replace("{{SEND_DATE}}", model.DATE_SEND.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            return html;
        }

        public static string ContentNotificationMessenger(NotificationViewModel vm)
        {
            var html = string.Empty;
            if (vm.Messengers!=null && vm.Messengers.Count()>0)
            {
                foreach (var item in vm.Messengers)
                {
                    html += " " + @"<div class='mess__item'>
                                    <div class='image img-cir img-40'>
                                        <img src = '/Image/depositphotos_96173202-stock-illustration-customer-flat-icon.jpg' alt='Michelle Moreno' />
                                    </div>
                                    <div class='content'>
                                        <h6>{{CUSTOMER_NAME}}</h6>
                                        <p>{{TOTAL_NEW_MESSENGER}}</p>
                                        <span class='time'>{{TIME_AGO}}</span>
                                    </div>
                                </div>";
                    html = html.Replace("{{CUSTOMER_NAME}}",item.CUSTOMER_NAME).Replace("{{TOTAL_NEW_MESSENGER}}",string.Format(FunctionHelpers.GetValueLanguage("Messenger.MessengerTotalNewBySession"), item.TOTAL)).Replace("{{TIME_AGO}}",FunctionHelpers.getTimeAgo(item.DATE_SEND.Value));
                }
            }
            if (vm.Messengers != null && vm.Messengers.Count() > 0)
            {
                html += " " + @"<div class='mess__footer'>
                                    <a href='#'>{{VIEW_ALL_MESSENGER}}</a>
                                </div>";
                html = html.Replace("{{VIEW_ALL_MESSENGER}}", FunctionHelpers.GetValueLanguage("Messenger.MessengerViewAll"));
            }
            return html.Trim();
        }
    }
}