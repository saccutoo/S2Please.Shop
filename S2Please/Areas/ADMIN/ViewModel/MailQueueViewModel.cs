using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class MailQueueViewModel : BaseModel
    {      
        public TableViewModel Table { get; set; } = new TableViewModel();
        public MailQueueModel MailQueue { get; set; } = new MailQueueModel();
        public bool Is_Save { get; set; } = true;
    }
}