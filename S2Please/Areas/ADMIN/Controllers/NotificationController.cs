using System.Collections.Generic;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using S2Please.Controllers;
using Repository;
using S2Please.Jobs;
using System.Web;
using S2Please.Areas.ADMIN.ViewModel;
using S2Please.ParramType;
using SHOP.COMMON;
using SHOP.COMMON.Helpers;
using System;
using System.Linq;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class NotificationController : BaseController
    {
        // GET: ADMIN/Exec
        private IMailQueueRepository _mailQueueRepository;
        private ISystemRepository _systemRepository;
        private IMessengerRepository _messengerRepository;
        public NotificationController(IMailQueueRepository mailQueueRepository, ISystemRepository systemRepository, IMessengerRepository messengerRepository)
        {
            this._mailQueueRepository = mailQueueRepository;
            this._systemRepository = systemRepository;
            this._messengerRepository = messengerRepository;
        }
        
        public ActionResult NotificationMessage()
        {
            NotificationViewModel vm = new NotificationViewModel();
            ParamType paramType = new ParamType();
            paramType.LANGUAGE_ID = CurrentUser.LANGUAGE_ID;
            paramType.USER_ID = CurrentUser.UserAdmin.USER_ID;
            paramType.ROLE_ID = CurrentUser.UserAdmin.ROLE_ID;
            var type = MapperHelper.Map<ParamType, Repository.Type.ParamType>(paramType);

            var responseMessengers = _messengerRepository.GetTop3MessengerNew(type);
            if (responseMessengers != null && CheckPermision(responseMessengers.StatusCode))
            {
                vm.Total = Convert.ToInt32(responseMessengers.OutValue.Parameters["@TotalRecord"].Value.ToString());
                vm.Messengers = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessengers.Results));
            }
            return View(vm);
        }

        public ActionResult Message(string sessionId="")
        {
            NotificationViewModel vm = new NotificationViewModel();
            vm.SESSION_ID = sessionId;
            if (!string.IsNullOrEmpty(sessionId))
            {
                var responseMessenger = _messengerRepository.GetMessengerBySessionId(sessionId, true,true);
                if (responseMessenger.Success == false && CheckPermision(responseMessenger.StatusCode) == false)
                {
                    return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
                }
                else
                {
                    var resultMessenger = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessenger.Results));
                    if (resultMessenger != null && resultMessenger.Count > 0)
                    {
                        vm.MessengerRights = resultMessenger;
                    }
                }
            }

            var responseMessengers = _messengerRepository.GetMessengerIsMain();
            if (responseMessengers.Success == false && CheckPermision(responseMessengers.StatusCode) == false)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }

            else
            {
                var resultMessengers = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessengers.Results));
                if (resultMessengers != null && resultMessengers.Count > 0)
                {
                    vm.Messengers = resultMessengers.OrderByDescending(s=>s.DATE_SEND).ToList();
                }
            }

          
            return View(vm);
        }

        public ActionResult ContentMessenger(string sessionId = "")
        {
            NotificationViewModel vm = new NotificationViewModel();
            if (!string.IsNullOrEmpty(sessionId))
            {
                var responseMessenger = _messengerRepository.GetMessengerBySessionId(sessionId, true,false);
                var resultMessenger = JsonConvert.DeserializeObject<List<ChatModel>>(JsonConvert.SerializeObject(responseMessenger.Results));
                if (resultMessenger != null && resultMessenger.Count > 0)
                {
                    vm.MessengerRights = resultMessenger;
                }
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Notification/_ContentMessageBySession.cshtml", vm);
            return Json(new { html }, JsonRequestBehavior.AllowGet);

        }
    }
}