using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class NotificationViewModel : BaseModel
    {      
        public int Total { get; set; }
        public List<ChatModel> Messengers { get; set; } = new List<ChatModel>();
        public List<ChatModel> MessengerRights { get; set; } = new List<ChatModel>();
        public string SESSION_ID { get; set; } = string.Empty;
        public List<NotificationModel> Notifications { get; set; } = new List<NotificationModel>();

    }
}