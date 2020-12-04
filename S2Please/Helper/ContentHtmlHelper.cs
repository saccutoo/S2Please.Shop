using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using SHOP.COMMON;
using S2Please.Helper;
namespace S2Please.Helper
{
    public static class ContentHtmlHelper
    {
        public static string ContentMessenger(ChatModel model)
        {
            var html = string.Empty;
            //if (model.IS_AUTO_CONTENT)
            //{
            //    html += " " + @"<div class='media media-chat media-chat-reverse'>
            //                        <div class='media-body'>
            //                            {{CONTENT}}
            //                            <p class='meta' style='color: #9b9b9b;'><time datetime = '2018' > {{SEND_DATE}} </ time ></ p >
            //                        </ div >
            //                    </div>";
            //}
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
        
    }
}