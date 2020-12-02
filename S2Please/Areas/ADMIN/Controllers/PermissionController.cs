using System.Collections.Generic;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using S2Please.Controllers;
using Repository;
using S2Please.Jobs;
using S2Please.Areas.ADMIN.ViewModel;
using SHOP.COMMON;
using System.Linq;
using S2Please.Helper;
using SHOP.COMMON.Helpers;
using Repository.Type;
namespace S2Please.Areas.ADMIN.Controllers
{
    public class PermissionController : BaseController
    {
        // GET: ADMIN/Exec
        private IMenuAdminRepository _menuAdminRepository;
        private IPermissonRepository _permissonRepository;
        private ISystemRepository _systemRepository;
        private IUserRepository _userRepository;
        public PermissionController(IMenuAdminRepository menuAdminRepository, IPermissonRepository permissonRepository, ISystemRepository systemRepository, IUserRepository userRepository)
        {
            this._menuAdminRepository = menuAdminRepository;
            this._permissonRepository = permissonRepository;
            this._systemRepository = systemRepository;
            this._userRepository = userRepository;
        }
       
        public ActionResult PermissionRole()
        {
            bool checkPermission = FunctionHelpers.CheckPermission(TableName.PermissionRole, Permission.Get);
            if (!checkPermission)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            PermissionViewModel vm = new PermissionViewModel();
            var responseRole= _systemRepository.GetAllRole();
            vm.Roles = responseRole.Results;
            return View(vm);
        }

        public ActionResult ShowFormSavePermissonRole(long id = 0)
        {
            bool checkPermission = FunctionHelpers.CheckPermission(TableName.PermissionRole, Permission.Get);
            if (!checkPermission)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            PermissionViewModel vm = new PermissionViewModel();

            var responseMenu = _menuAdminRepository.GetAllMenuAdmin(0);
            var resultMenu = JsonConvert.DeserializeObject<List<MenuModel>>(JsonConvert.SerializeObject(responseMenu.Results));
            if (resultMenu != null && resultMenu.Count > 0)
            {
                vm.Menus = resultMenu;
            }

            var responsePermission = _permissonRepository.GetPermissionRoleByRoleId(id);
            var resultPermission = JsonConvert.DeserializeObject<List<MenuPermissionModel>>(JsonConvert.SerializeObject(responsePermission.Results));
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

        public ActionResult SavePermissionRole(List<MenuPermissionModel> datas,long roleId)
        {
            ResultModel result = new ResultModel();
            var types = MapperHelper.MapList<MenuPermissionModel, MenuPermissionType>(datas);
            var response = _permissonRepository.SavePermissionRole(types, roleId);
            if (response != null)
            {
                if (response.Success == false && CheckPermision(response.StatusCode) == false)
                {
                    result.SetUrl("/Base/Page404");
                    return Content(JsonConvert.SerializeObject(new
                    {
                        result
                    }));
                }
                else
                {
                    var resultProductType = JsonConvert.DeserializeObject<List<ProductGroupModel>>(JsonConvert.SerializeObject(response.Results));
                    if (resultProductType != null && resultProductType.Count() > 0)
                    {
                        result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.UpdateSuccess"), FunctionHelpers.GetValueLanguage("Message.Success"), string.Empty);
                    }
                    else
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.UpdateFail"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }

                }
            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }

        public ActionResult PermissionUser()
        {
            bool checkPermission = FunctionHelpers.CheckPermission(TableName.PermissionUser, Permission.Get);
            if (!checkPermission)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            PermissionViewModel vm = new PermissionViewModel();
            var responseUser = _userRepository.GetAllUser();
            vm.Users = responseUser.Results;
            return View(vm);
        }

        public ActionResult ShowFormSavePermissonUser(long id = 0)
        {
            bool checkPermission = FunctionHelpers.CheckPermission(TableName.PermissionUser, Permission.Get);
            if (!checkPermission)
            {
                return RedirectToRoute(new { action = "/Page404", controller = "Base", area = "" });
            }
            PermissionViewModel vm = new PermissionViewModel();

            var responseMenu = _menuAdminRepository.GetAllMenuAdmin(0);
            var resultMenu = JsonConvert.DeserializeObject<List<MenuModel>>(JsonConvert.SerializeObject(responseMenu.Results));
            if (resultMenu != null && resultMenu.Count > 0)
            {
                vm.Menus = resultMenu;
            }

            var responsePermission = _permissonRepository.GetPermissionUserByUserId(id);
            var resultPermission = JsonConvert.DeserializeObject<List<MenuPermissionModel>>(JsonConvert.SerializeObject(responsePermission.Results));
            if (resultPermission != null && resultPermission.Count > 0)
            {
                vm.Permissions = resultPermission;
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Permission/_FormSavePermisionUser.cshtml", vm);
            return Content(JsonConvert.SerializeObject(new
            {
                html
            }));
        }

        public ActionResult SavePermissionUser(List<MenuPermissionModel> datas, long userId)
        {
            ResultModel result = new ResultModel();
            var types = MapperHelper.MapList<MenuPermissionModel, MenuPermissionType>(datas);
            var response = _permissonRepository.SavePermissionUser(types, userId);
            if (response != null)
            {
                if (response.Success == false && CheckPermision(response.StatusCode) == false)
                {
                    result.SetUrl("/Base/Page404");
                    return Content(JsonConvert.SerializeObject(new
                    {
                        result
                    }));
                }
                else
                {
                    var resultProductType = JsonConvert.DeserializeObject<List<ProductGroupModel>>(JsonConvert.SerializeObject(response.Results));
                    if (resultProductType != null && resultProductType.Count() > 0)
                    {
                        result.SetDataMessage(true, FunctionHelpers.GetValueLanguage("Message.UpdateSuccess"), FunctionHelpers.GetValueLanguage("Message.Success"), string.Empty);
                    }
                    else
                    {
                        result.SetDataMessage(false, FunctionHelpers.GetValueLanguage("Message.UpdateFail"), FunctionHelpers.GetValueLanguage("Message.Error"), string.Empty);
                    }

                }
            }
            return Content(JsonConvert.SerializeObject(new
            {
                result
            }));
        }
    }
}