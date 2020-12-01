using System.Collections.Generic;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using S2Please.Controllers;
using Repository;
using S2Please.Jobs;
using S2Please.Areas.ADMIN.ViewModel;
using SHOP.COMMON;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class PermissionController : BaseController
    {
        // GET: ADMIN/Exec
        private IMenuAdminRepository _menuAdminRepository;
        private IPermissonRepository _permissonRepository;
        private ISystemRepository _systemRepository;

        public PermissionController(IMenuAdminRepository menuAdminRepository, IPermissonRepository permissonRepository, ISystemRepository systemRepository)
        {
            this._menuAdminRepository = menuAdminRepository;
            this._permissonRepository = permissonRepository;
            this._systemRepository = systemRepository;
        }
       
        public ActionResult PermissionRole()
        {
            PermissionViewModel vm = new PermissionViewModel();
            var responseRole= _systemRepository.GetAllRole();
            vm.Roles = responseRole.Results;
            return View(vm);
        }

        public ActionResult ShowFormSavePermissonRole(long id = 0)
        {
            PermissionViewModel vm = new PermissionViewModel();

            var responseMenu = _menuAdminRepository.GetAllMenuAdmin(0);
            var resultMenu = JsonConvert.DeserializeObject<List<MenuModel>>(JsonConvert.SerializeObject(responseMenu.Results));
            if (resultMenu != null && resultMenu.Count > 0)
            {
                vm.Menus = resultMenu;
            }

            var responsePermission = _permissonRepository.GetPermissionRoleByRoleId(id);
            var resultPermission = JsonConvert.DeserializeObject<List<MenuPermissionModel>>(JsonConvert.SerializeObject(responseMenu.Results));
            if (resultPermission != null && resultPermission.Count > 0)
            {
                vm.Permissions = resultPermission;
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Permission/_FormSavePermisionRole.cshtml", vm);
            return Content(JsonConvert.SerializeObject(new
            {
                html
            }));
        }
    }
}