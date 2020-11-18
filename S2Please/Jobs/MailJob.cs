using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Jobs
{
    public class MailJob
    {
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

    }
}