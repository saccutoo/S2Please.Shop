using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Database;
using S2Please.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using SHOP.COMMON;
using S2Please.Controllers;
using S2Please.Helper;
using System.Web.Security;
using System;
using System.Web;
using S2Please.Areas.ADMIN.ViewModel;
using System.Configuration;
using Repository;
namespace S2Please.Areas.ADMIN.Controllers
{
    public class AuthenController : BaseController
    {
        // GET: ADMIN/Authen
        private IAuthenRepository _authenRepository;
        public AuthenController(IAuthenRepository authenRepository)
        {
            this._authenRepository = authenRepository;
        }

        [HttpGet]
        public ActionResult Login(LoginViewModel vm)
        {
            return View(vm);
        }
        
        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            LoginViewModel vm = new LoginViewModel();
            vm.Is_Login = true;
            vm.Success = true;
            vm.User = model;
            if (model.USER_NAME==null || model.USER_NAME=="")
            {
                vm.Is_Error_User_Name_Required = true;
                vm.Success = false;
            }
            if (model.PASS_WORD == null || model.PASS_WORD == "")
            {
                vm.IS_Error_Password_Required = true;
                vm.Success = false;
            }
            if (vm.Success)
            {
                var responseUser = _authenRepository.GetUser(model.USER_NAME, Security.EncryptKey(model.PASS_WORD),"1");
                var resultUser = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(responseUser.Results));
                if (resultUser != null && resultUser.Count() > 0)
                {
                    Security.UserSignInAdmin(resultUser.FirstOrDefault(), System.Web.HttpContext.Current);
                    return RedirectToAction("Index", "Dashboard", new { Area = "ADMIN" });
                }
                else
                {
                    vm.Success = false;
                    vm.Is_Login = false;
                    vm.Message = FunctionHelpers.GetValueLanguage("Authen.Message.WrongLogin");
                }
            }
            return View(vm);
        }

        [NoCache]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //Clear session
            var current = System.Web.HttpContext.Current;
            ////Clears out Session
            current.Response.Cookies.Clear();
            // clear authentication cookie
            current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            HttpCookie cookie = current.Request.Cookies[FormsAuthentication.FormsCookieName];
            Security.LogoutAdmin(System.Web.HttpContext.Current);
            var m = new LoginViewModel();
            return RedirectToAction("Login", "Authen", new { Area = "ADMIN", vm=m });
        }
        [HttpGet]
        public ActionResult SendRequestChangePass(LoginViewModel vm)
        {
            return View(vm);
        }
        [HttpPost]
        public ActionResult SendRequestChangePass(UserModel model)
        {
            LoginViewModel vm = new LoginViewModel();
            vm.Success = true;
            vm.Is_SendRequest = true;
            if (model.EMAIL==null || model.EMAIL=="")
            {
                vm.Is_Error_Email_Required = true;
                vm.Success = false;
            }
            else if (model.EMAIL != null && model.EMAIL != "")
            {
                if (!Regex.IsMatch(model.EMAIL, Constant.Email_Reg))
                {
                    vm.IS_Error_Format_Email = true;
                    vm.Success = false;
                }
            }
            if (vm.Success)
            {
                Guid guid = Guid.NewGuid();
                var code = guid.ToString();
                var responseUser = _authenRepository.GetUserByEmail(model.EMAIL, code,"1");
                var resultUser = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(responseUser.Results));
                if (resultUser!=null && resultUser.Count()>0)
                {
                    vm.Message = FunctionHelpers.GetValueLanguage("Authen.Message.SendComfirmResetPass");
                    vm.Is_Confirm = true;
                    vm.Code = code;
                    vm.Email = model.EMAIL;
                    string subject = FunctionHelpers.GetValueLanguage("Email.SubjectComfirmResetPass");
                    string body = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Authen/_EmailComfirmResetPass.cshtml", vm);
                    List<string> toMail = new List<string>();
                    toMail.Add(model.EMAIL);
                    string from = ConfigurationManager.AppSettings["MailUserName"] + ";" + "S2Please";
                    string replyTo = ConfigurationManager.AppSettings["MailUserName"];
                    int resultCode = SendMail(subject, body, toMail, new List<string>(), new List<string>(), from, replyTo, new List<AttachmentJs>());
                    if (resultCode != 200)
                    {
                        vm.Success = false;
                        vm.Is_SendRequest = false;
                        vm.Code = code;
                    }
                }
                else
                {
                    vm.Success = false;
                    vm.Is_SendRequest = false;
                    vm.Message = FunctionHelpers.GetValueLanguage("Error.Email.NoAlreadyExist");
                    vm.Code = code;
                }
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult ForgotPassword(LoginViewModel model)
        {
            if (String.IsNullOrEmpty(model.Code) || String.IsNullOrEmpty(model.Email))
            {
                return RedirectToAction("Index","Dashboard",new { area="ADMIN"});
            }
            model.Success = false;
            var response = _authenRepository.GetHistoryResetPass(model.Code);
            var result = JsonConvert.DeserializeObject<List<HistoryResetPassword>>(JsonConvert.SerializeObject(response.Results));
            if (result != null && result.Count() > 0)
            {
                if (DateTime.Now > result.FirstOrDefault().DATE_TIME)
                {
                    model.Is_Expired = true;
                }
            }
            else
            {
                model.Is_Expired = true;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ForgotPassword(UserModel model)
        {
            LoginViewModel vm = new LoginViewModel();
            vm.Success = true;
            vm.Is_ChangePass = true;
            vm.Code = model.CODE;
            vm.Email = model.EMAIL;
            if (model.PASS_WORD == null || model.PASS_WORD == "")
            {
                vm.IS_Error_Password_Required = true;
                vm.Success = false;
            }
            if (model.PASS_WORD_AGAIN == null || model.PASS_WORD_AGAIN == "")
            {
                vm.IS_Error_Password_Required_Again = true;
                vm.Success = false;
            }
            if (model.PASS_WORD != null && model.PASS_WORD != "" && model.PASS_WORD_AGAIN != null && model.PASS_WORD_AGAIN != "")
            {
                if (model.PASS_WORD != model.PASS_WORD_AGAIN)
                {
                    vm.IS_Error_Password_Not_Match_Again = true;
                    vm.Success = false;
                }
            }
            if (vm.Success)
            {
                var response = _authenRepository.GetHistoryResetPass(vm.Code);
                var result = JsonConvert.DeserializeObject<List<HistoryResetPassword>>(JsonConvert.SerializeObject(response.Results));
                if (result != null && result.Count() > 0)
                {
                    if (DateTime.Now > result.FirstOrDefault().DATE_TIME)
                    {
                        vm.Is_Expired = true;
                    }
                    else
                    {
                        var responseS = _authenRepository.ChangePasswordEmployees(Security.EncryptKey(model.PASS_WORD), model.EMAIL);
                        var resultS = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(responseS.Results));
                        if (resultS != null && resultS.Count() > 0)
                        {
                            vm.Success = true;
                            vm.Message = FunctionHelpers.GetValueLanguage("Authen.ChangePassSucces");
                        }
                    }
                }
                else if (result == null)
                {
                    vm.Is_Expired = true;
                }
                else
                {
                    vm.Success = false;
                    vm.Message = FunctionHelpers.GetValueLanguage("Authen.ChangePassEror");

                }
            }
            
            return View(vm);
        }
    }
}