using Repository;
using Newtonsoft.Json;
using S2Please.Models;
using System.Collections.Generic;
using S2Please.Controllers;
using SHOP.COMMON;
namespace S2Please.Jobs
{
    public class MailJob
    {
        static IMailQueueRepository _mailQueueRepository;
        static BaseController _baseC = new BaseController();
        public MailJob(IMailQueueRepository mailQueueRepository)
        {
            _mailQueueRepository = mailQueueRepository;
        }
        public string RecurringJob { get; set; } = "Mail job";
        /// <summary>
        /// minute/ hour/ day of month/ month/ day of week/ year
        ///  chạy liên tục và nghỉ sau mỗi 15 minutes
        /// </summary>
        public string CronExpression { get; set; } = "*/15 * * * *";

        /// <summary>
        /// khung giờ chạy cố định là 5:00am tới 21:00pm
        /// </summary>
        private static int HourseMin { get; set; } = 6;

        private static int HourseMax { get; set; } = 22;

        public static void _Execute()
        {
            var response = _mailQueueRepository.GetTop5MailQueueFalse();
            var result = JsonConvert.DeserializeObject<List<MailQueueModel>>(JsonConvert.SerializeObject(response.Results));
            if (result!=null && result.Count>0)
            {
                foreach (var item in result)
                {
                    List<string> mailTo = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(item.MAIL_TO));
                    List<string> mailCc = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(item.MAIL_CC));
                    List<string> mailBcc = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(item.MAIL_BCC));

                    if (item.DATA_TYPE==DataType.Order)
                    {
                        int resultCode = _baseC.SendMail(item.MAIL_NAME,item.CONTENT,mailTo,mailCc,mailBcc,item.FROM,string.Empty,new List<AttachmentJs>(),item.DATA_ID,item.DATA_TYPE);
                    }

                }
            }
        }
    }
}