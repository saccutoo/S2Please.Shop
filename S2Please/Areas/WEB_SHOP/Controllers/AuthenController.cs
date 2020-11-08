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
using S2Please.Areas.WEB_SHOP.ViewModel;
using System.Configuration;
using Repository;
namespace S2Please.Areas.WEB_SHOP.Controllers
{
    public class AuthenController : BaseController
    {
        private IAuthenRepository _authenRepository;
        public AuthenController(IAuthenRepository authenRepository)
        {
            this._authenRepository = authenRepository;
        }
        // GET: WEB_SHOP/Authen
        private ado _ado = new ado();
        [HttpGet]
        public ActionResult Login(LoginViewModel vm)
        {
            if (Request.Cookies[Constant.ShopOnline] != null)
            {
                return RedirectToAction("Index", "Home", new { Area = "WEB_SHOP" });
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult Login(UserModel user)
        {
            LoginViewModel vm = new LoginViewModel();
            vm.Is_Create = false;
            if (user.USER_NAME == null || user.USER_NAME == "")
            {
                vm.Is_Error_User_Name_Required = true;
                vm.Success = false;
            }
            if (user.PASS_WORD == null || user.PASS_WORD == "")
            {
                vm.IS_Error_Password_Required = true;
                vm.Success = false;
            }
            if (vm.Success==true)
            {
                var responseUser = _authenRepository.GetUser(user.USER_NAME, Security.EncryptKey(user.PASS_WORD),"0");
                var resultUser = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(responseUser.Results));
                if (resultUser != null && resultUser.Count() > 0)
                {
                    Security.UserSignIn(resultUser.FirstOrDefault(), System.Web.HttpContext.Current);
                    if (Session["UrlConTroller"]!=null && Session["UrlView"] != null)
                    {
                        return RedirectToAction(Session["UrlView"].ToString(), Session["UrlConTroller"].ToString(), new { Area = "WEB_SHOP" });
                    }
                    return RedirectToAction("Index", "Home",new { Area = "WEB_SHOP" });
                }
                else
                {
                    vm.Success = false;
                    vm.Message = FunctionHelpers.GetValueLanguage("Authen.Message.WrongLogin");
                }
            }
            vm.Message = FunctionHelpers.GetValueLanguage("Authen.Message.WrongLogin");
            return View(vm);
        }
        public ActionResult Register(UserModel user)
        {
            S2Please.Areas.WEB_SHOP.ViewModel.LoginViewModel vm = new S2Please.Areas.WEB_SHOP.ViewModel.LoginViewModel();
            vm.Is_Create = true;
            vm.Is_Login = false;
            vm.User = user;
            if (user.USER_NAME==null || user.USER_NAME=="")
            {
                vm.Is_Error_User_Name_Required = true;
                vm.Success = false;
            }
            if (user.PASS_WORD == null || user.PASS_WORD == "")
            {
                vm.IS_Error_Password_Required = true;
                vm.Success = false;
            }
            if (user.PASS_WORD != null && user.PASS_WORD != "")
            {
                if (user.PASS_WORD.Length<6)
                {
                    vm.IS_Error_Password_Length = true;
                    vm.Success = false;
                }
            }
            if (user.PASS_WORD_AGAIN == null || user.PASS_WORD_AGAIN == "")
            {
                vm.IS_Error_Password_Required_Again = true;
                vm.Success = false;
            }
            if (user.PASS_WORD != null && user.PASS_WORD != "" && user.PASS_WORD_AGAIN != null && user.PASS_WORD_AGAIN != "")
            {
                if (user.PASS_WORD!= user.PASS_WORD_AGAIN)
                {
                    vm.IS_Error_Password_Not_Match_Again = true;
                    vm.Success = false;
                }
            }
            if (user.EMAIL == null || user.EMAIL == "")
            {
                vm.Is_Error_Email_Required = true;
                vm.Success = false;
            }
            if (user.EMAIL != null && user.EMAIL != "")
            {
                if (!Regex.IsMatch(user.EMAIL, Constant.Email_Reg))
                {
                    vm.IS_Error_Format_Email = true;
                    vm.Success = false;
                }
            }

            var responseUser = _authenRepository.CheckUserNameEmail(user.USER_NAME,user.EMAIL);
            var resultUser = JsonConvert.DeserializeObject<List<CheckUserNameEmail>>(JsonConvert.SerializeObject(responseUser.Results));
            if (resultUser != null && resultUser.Count() > 0)
            {
                if (resultUser.FirstOrDefault().Is_Exist_UserName == true)
                {
                    vm.Is_Error_User_Name_Exist = true;
                    vm.Success = false;
                }
                if (resultUser.FirstOrDefault().Is_Exist_Email == true)
                {
                    vm.Is_Error_User_Email_Exist = true;
                    vm.Success = false;
                }
            }

            if (vm.Success)
            {
                var response = _authenRepository.UpdateUser(user.USER_NAME,Security.EncryptKey(user.PASS_WORD), user.EMAIL);
                var result = JsonConvert.DeserializeObject<List<ProductModel>>(JsonConvert.SerializeObject(response.Results));
                if (result != null && result.Count() > 0)
                {
                    vm.Message = FunctionHelpers.GetValueLanguage("Authen.Message.CreateUser");
                }

            }
            return View("Login", vm);

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
            Security.Logout(System.Web.HttpContext.Current);
            var m = new LoginViewModel();
            return RedirectToAction("Login", "Authen", new { Area = "WEB_SHOP",m});
        }
        public ActionResult CreateAccount()
        {
            Session["UrlConTroller"] = "Cart";
            Session["UrlView"] = "CheckOut";
            return RedirectToAction("Login", "Authen", new { Area = "WEB_SHOP" });
        }

        public ActionResult LoginInCheckOut(string userName,string passWord)
        {
            LoginViewModel vm = new LoginViewModel();
            UserModel user = new UserModel();
            vm.Success = true;
            user.USER_NAME = userName;
            user.PASS_WORD = passWord;
            if (user.USER_NAME == null || user.USER_NAME == "")
            {
                vm.Is_Error_User_Name_Required = true;
                vm.Success = false;
            }
            if (user.PASS_WORD == null || user.PASS_WORD == "")
            {
                vm.IS_Error_Password_Required = true;
                vm.Success = false;
            }
            if (vm.Success == true)
            {
                var responseUser = _authenRepository.GetUser(user.USER_NAME, Security.EncryptKey(user.PASS_WORD),"0");
                var resultUser = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(responseUser.Results));
                if (resultUser != null && resultUser.Count() > 0)
                {
                    Security.UserSignIn(resultUser.FirstOrDefault(), System.Web.HttpContext.Current);
                    vm.Success = true;
                }
                else
                {
                    vm.Success = false;
                    vm.IS_Error_Password_Required = true;
                    vm.Message = FunctionHelpers.GetValueLanguage("Authen.Message.WrongLogin");
                }
            }
            return Json(vm, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ForgotPassword(LoginViewModel vm)
        {
            if (vm.Code!=null && vm.Is_Confirm && vm.Email!=null)
            {
                vm.Success = false;
                var response = _authenRepository.GetHistoryResetPass(vm.Code);
                var result = JsonConvert.DeserializeObject<List<HistoryResetPassword>>(JsonConvert.SerializeObject(response.Results));
                if (result != null && result.Count() > 0)
                {
                    if (DateTime.Now> result.FirstOrDefault().DATE_TIME)
                    {
                        vm.Is_Expired = true;
                    }
                }              
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserModel user)
        {
            LoginViewModel vm = new LoginViewModel();
            vm.Success = true;
            
            if (user.CODE!=null && user.IS_CONFIRM && user.EMAIL!=null)
            {
                vm.Code = user.CODE;
                vm.Is_Confirm = user.IS_CONFIRM;
                vm.Email = user.EMAIL;
                if (user.PASS_WORD == null || user.PASS_WORD == "")
                {
                    vm.IS_Error_Password_Required = true;
                    vm.Success = false;
                }
                if (user.PASS_WORD != null && user.PASS_WORD != "")
                {
                    if (user.PASS_WORD.Length < 6)
                    {
                        vm.IS_Error_Password_Length = true;
                        vm.Success = false;
                    }
                }
                if (user.PASS_WORD_AGAIN == null || user.PASS_WORD_AGAIN == "")
                {
                    vm.IS_Error_Password_Required_Again = true;
                    vm.Success = false;
                }
                if (user.PASS_WORD != null && user.PASS_WORD != "" && user.PASS_WORD_AGAIN != null && user.PASS_WORD_AGAIN != "")
                {
                    if (user.PASS_WORD != user.PASS_WORD_AGAIN)
                    {
                        vm.IS_Error_Password_Not_Match_Again = true;
                        vm.Success = false;
                    }
                }
                if (!vm.Success)
                {
                    if (vm.Code != null && vm.Is_Confirm && vm.Email != null)
                    {
                        var response = _authenRepository.GetHistoryResetPass(vm.Code);
                        var result = JsonConvert.DeserializeObject<List<HistoryResetPassword>>(JsonConvert.SerializeObject(response.Results));
                        if (result != null && result.Count() > 0)
                        {
                            if (DateTime.Now > result.FirstOrDefault().DATE_TIME)
                            {
                                vm.Is_Expired = true;
                                vm.Success = false;
                            }
                        }
                    }
                }
                else
                {

                    var response = _authenRepository.GetHistoryResetPass(vm.Code);
                    var result = JsonConvert.DeserializeObject<List<HistoryResetPassword>>(JsonConvert.SerializeObject(response.Results));
                    if (result != null && result.Count() > 0)
                    {
                        if (DateTime.Now > result.FirstOrDefault().DATE_TIME)
                        {
                            vm.Is_Expired = true;
                            vm.Success = false;

                        }
                        else
                        {
                            var responseS = _authenRepository.ChangePasswordCustomer(Security.EncryptKey(user.PASS_WORD), user.EMAIL);
                            var resultS = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(responseS.Results));
                            if (resultS != null && resultS.Count() > 0)
                            {
                                vm.Success = true;
                                vm.Is_ChangePass = true;
                            }
                        }
                    }

                    
                }
            }
            else
            {
                if (user.EMAIL == null || user.EMAIL == "")
                {
                    vm.Is_Error_Email_Required = true;
                    vm.Is_Confirm = false;
                    vm.Success = false;
                }
                else if (user.EMAIL != null && user.EMAIL != "")
                {
                    if (!Regex.IsMatch(user.EMAIL, Constant.Email_Reg))
                    {
                        vm.IS_Error_Format_Email = true;
                        vm.Is_Confirm = false;
                        vm.Success = false;
                    }
                }
                if (vm.Success)
                {
                    Guid guid = Guid.NewGuid();
                    var code = guid.ToString();
                    var responseUser = _authenRepository.GetUserByEmail(user.EMAIL,code,"0");
                    var resultUser = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(responseUser.Results));
                    if (resultUser != null && resultUser.Count() > 0)
                    {
                        vm.Is_Confirm = true;
                        vm.Code = code;
                        vm.Email = user.EMAIL;
                        string subject = FunctionHelpers.GetValueLanguage("Email.SubjectComfirmResetPass");
                        string body = RenderViewToString(this.ControllerContext, "~/Areas/WEB_SHOP/Views/Authen/_EmailComfirmResetPass.cshtml", vm);
                        List<string> toMail = new List<string>();
                        toMail.Add(user.EMAIL);
                        string from = ConfigurationManager.AppSettings["MailUserName"] + ";" + "S2Please";
                        string replyTo = ConfigurationManager.AppSettings["MailUserName"];
                        int resultCode = SendMail(subject, body, toMail, new List<string>(), new List<string>(), from, replyTo, new List<AttachmentJs>());
                        if (resultCode != 200)
                        {

                        }
                        vm.Message = FunctionHelpers.GetValueLanguage("Authen.Message.SendComfirmResetPass");
                        vm.User.EMAIL = user.EMAIL;
                    }
                }
            }
           
            
            return View(vm);
        }

    }
    public class CheckUserNameEmail
    {
        public bool Is_Exist_UserName { get; set; }
        public bool Is_Exist_Email { get; set; }
    }
}