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
    }
}