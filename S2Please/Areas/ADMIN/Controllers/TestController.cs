using System.Collections.Generic;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using S2Please.Controllers;
using Repository;
using S2Please.Jobs;
using System.Web;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class TestController : BaseController
    {
        // GET: ADMIN/Exec
        private IMailQueueRepository _mailQueueRepository;
        public TestController(IMailQueueRepository mailQueueRepository)
        {
            this._mailQueueRepository = mailQueueRepository;
        }
        public ActionResult TestJobMailQueue()
        {
            MailJob job = new MailJob();
            job._Execute();
            return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
        }
        public ActionResult Test()
        {
            HttpContext.Response.Redirect("/Base/Page404");
            return View();
        }
    }
}