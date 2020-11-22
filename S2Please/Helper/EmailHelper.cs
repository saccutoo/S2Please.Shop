using S2Please.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using SHOP.COMMON;
namespace S2Please.Helper
{
    public static class EmailHelper
    {
        public static EmailConfigModel GetConfiguration()
        {

            return new EmailConfigModel
            {
                SmtpServer = ConfigurationManager.AppSettings["SmtpServer"],
                MailUserName = ConfigurationManager.AppSettings["MailUserName"],
                MailPassword = ConfigurationManager.AppSettings["MailPassword"],
                EnableSsl = ConfigurationManager.AppSettings["EnableSsl"],
                MailPort = ConfigurationManager.AppSettings["MailPort"],
            };
        }
        public static int SendMail(string subject, string body, string from, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, IEnumerable<Attachment> attachments, long dataId = 0, string dataType = "")
        {
            var emailConfiguration = GetConfiguration();

            if (emailConfiguration == null)
                return 404;

            try
            {
                emailConfiguration.MailFrom = from;
                using (var smtpClient = new SmtpClient
                {
                    Host = emailConfiguration.SmtpServer,
                    EnableSsl = bool.Parse(emailConfiguration.EnableSsl),
                })
                {

                    if (!string.IsNullOrEmpty(emailConfiguration.MailUserName) && !string.IsNullOrEmpty(emailConfiguration.MailPassword))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(emailConfiguration.MailUserName, emailConfiguration.MailPassword);
                    }
                    else
                    {
                        smtpClient.UseDefaultCredentials = true;
                    }

                    if (!string.IsNullOrEmpty(emailConfiguration.MailPort))
                    {
                        smtpClient.Port = int.Parse(emailConfiguration.MailPort);
                    }

                    MailMessage message = PrepareMailMessage(subject, body, to, cc, bcc, attachments, emailConfiguration);
                    smtpClient.Send(message);
                    LogHelper.LogMailQueue(0, dataId, dataType, JsonConvert.SerializeObject(to), JsonConvert.SerializeObject(cc), JsonConvert.SerializeObject(bcc), subject, body, "Thành công", StatusMailQueue.Success,from);
                }

                return 200;
            }
            catch (Exception ex)
            {
                LogHelper.LogMailQueue(0, dataId, dataType, JsonConvert.SerializeObject(to), JsonConvert.SerializeObject(cc), JsonConvert.SerializeObject(bcc), subject, body, "Thất bại - " + ex.Message, StatusMailQueue.False, from);
                if (ex.ToString().Contains("5.7.0"))
                {
                    return 500;//Requested action not taken: mailbox unavailable
                }
                else
                if (ex.ToString().Contains("5.5.1"))
                {
                    return 501;//<xxx@domain> User doesn’t exist: xxx@domain
                }
                else
                    return 502;// 502 Bad Gateway
            }
        }
        private static MailMessage PrepareMailMessage(string subject, string body, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, IEnumerable<Attachment> attachments, EmailConfigModel emailConfiguration)
        {
            var message = new MailMessage();
            MailAddress fromAddress;
            if (emailConfiguration.MailFrom.IndexOf(";") > -1)
            {
                var mailFrom = emailConfiguration.MailFrom.Split(';');
                fromAddress = new MailAddress(mailFrom[0], mailFrom[1]);
            }
            else
            {
                fromAddress = new MailAddress(emailConfiguration.MailFrom);

            }

            message.From = fromAddress;
            if (to != null)
            {
                foreach (var item in to.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    message.To.Add(item);
                }
            }

            if (cc != null)
            {
                foreach (var item in cc.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    message.CC.Add(item);
                }
            }
            if (bcc != null)
            {
                foreach (var item in bcc.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    message.Bcc.Add(item);
                }
            }

            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            return message;
        }
    }

}