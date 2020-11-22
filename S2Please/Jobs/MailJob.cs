using Repository;
using Newtonsoft.Json;
using S2Please.Models;
using System.Collections.Generic;
using S2Please.Controllers;
using SHOP.COMMON;
using SHOP.COMMON.Helpers;

namespace S2Please.Jobs
{
    public class MailJob
    {
        //static IMailQueueRepository _mailQueueRepository;
        static BaseController _baseC = new BaseController();
        public static CommonRepository commonRepository = new CommonRepository();

        //public MailJob(IMailQueueRepository mailQueueRepository)
        //{
        //    _mailQueueRepository = mailQueueRepository;
        //}
        public static string RecurringJob { get; set; } = "Mail job";
        /// <summary>
        /// minute/ hour/ day of month/ month/ day of week/ year
        ///  chạy liên tục và nghỉ sau mỗi 15 minutes
        /// </summary>
        public static string CronExpression { get; set; } = "*/2 * * * *";     
        /// <summary>
        /// khung giờ chạy cố định là 5:00am tới 21:00pm
        /// </summary>
        private static int HourseMin { get; set; } = 6;

        private static int HourseMax { get; set; } = 22;

        public void _Execute()
        {
            var param = new List<Param>();
            var model = new Repository.Model.MailQueueModel();
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
            var response = commonRepository.ListProcedure<Repository.Model.MailQueueModel>(model, "MailQueue_Get_GetTop5MailQueueFalse", paramType);
            var result = JsonConvert.DeserializeObject<List<MailQueueModel>>(JsonConvert.SerializeObject(response.Results));
            if (result!=null && result.Count>0)
            {
                foreach (var item in result)
                {
                    List<string> mailTo = new List<string>();
                    List<string> mailCc = new List<string>();
                    List<string> mailBcc = new List<string>();
                    if (!string.IsNullOrEmpty(item.MAIL_TO))
                    {
                        var listMailTo = JsonConvert.DeserializeObject<List<ListMail>>(item.MAIL_TO);
                        if (listMailTo!=null && listMailTo.Count>0)
                        {
                            foreach (var mail in listMailTo)
                            {
                                mailTo.Add(mail.Mail);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(item.MAIL_CC))
                    {
                        var listMailCc = JsonConvert.DeserializeObject<List<ListMail>>(item.MAIL_CC);
                        if (listMailCc != null && listMailCc.Count > 0)
                        {
                            foreach (var mail in listMailCc)
                            {
                                mailCc.Add(mail.Mail);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(item.MAIL_BCC))
                    {
                        var listMailBcc = JsonConvert.DeserializeObject<List<ListMail>>(item.MAIL_BCC);
                        if (listMailBcc != null && listMailBcc.Count > 0)
                        {
                            foreach (var mail in listMailBcc)
                            {
                                mailBcc.Add(mail.Mail);
                            }
                        }
                    }

                    if (item.DATA_TYPE==DataType.Order)
                    {
                        int resultCode = _baseC.SendMail(item.MAIL_NAME,item.CONTENT,mailTo,mailCc,mailBcc,item.FROM,string.Empty,new List<AttachmentJs>(),item.DATA_ID,item.DATA_TYPE);
                    }

                }
            }
        }
    }
}