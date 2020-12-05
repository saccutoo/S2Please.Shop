using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;
using SHOP.COMMON;
using SHOP;
using S2Please.Models;
using SHOP.COMMON.Helpers;
using Repository;
namespace S2Please.Helper
{
    
    public static class Security 
    {
        public static CommonRepository _commonRepository = new CommonRepository();
        //Set cookie language
        public static void CheckCookieLanguage()
        {           
            try
            {
                HttpCookie getCookie = HttpContext.Current.Request.Cookies[Constant.APP_CURRENT_LANG];
                if (getCookie != null)
                {
                    //CurrentUser.LANGUAGE_ID = long.Parse(getCookie.Value);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie(Constant.APP_CURRENT_LANG);
                    cookie.Value = Language.VIETNAM;
                    cookie.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }

            }
            catch (Exception)
            {
                HttpCookie cookie = new HttpCookie(Constant.APP_CURRENT_LANG);
                cookie.Value = Language.VIETNAM;
                cookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }          
         }
        public static bool ChangeCookieLanguage(int languageId)
        {
            try
            {

                HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies[Constant.APP_CURRENT_LANG];
                HttpContext.Current.Response.Cookies.Remove(Constant.APP_CURRENT_LANG);
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                HttpContext.Current.Response.SetCookie(currentUserCookie);

                HttpCookie cookie = new HttpCookie(Constant.APP_CURRENT_LANG);
                cookie.Value = languageId.ToString() ;
                cookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch (Exception)
            {
                
                return false;

            }
        }

        //Set time vestion
        public static void CheckCookieTimeVersion(long version=0)
        {
            try
            {
                HttpCookie getCookie = HttpContext.Current.Request.Cookies[Constant.APP_CURRENT_VESTION];
                if (getCookie != null)
                {
                    //CurrentUser.LANGUAGE_ID = long.Parse(getCookie.Value);
                    long timeVersion= long.Parse(getCookie.Value);
                    if (version!=0)
                    {
                        var param = new List<Param>();
                        var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
                        var response = _commonRepository.ListProcedure<TimeVersion>(new TimeVersion(), "TimeVersion_Get_TimeVersion", paramType);
                        var result = JsonConvert.DeserializeObject<List<TimeVersion>>(JsonConvert.SerializeObject(response.Results));
                        if (result != null && result.Count() > 0)
                        {
                            timeVersion = result.FirstOrDefault().ID;
                        }

                        HttpCookie cookie = new HttpCookie(Constant.APP_CURRENT_VESTION);
                        cookie.Value = timeVersion.ToString();
                        cookie.Expires = DateTime.Now.AddMonths(1);
                        HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                }
                else
                {
                    long timeVersion = 0;
                    var param = new List<Param>();
                    var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
                    var response = _commonRepository.ListProcedure<TimeVersion>(new TimeVersion(), "TimeVersion_Get_TimeVersion", paramType);
                    var result = JsonConvert.DeserializeObject<List<TimeVersion>>(JsonConvert.SerializeObject(response.Results));
                    if (result != null && result.Count() > 0)
                    {
                        timeVersion = result.FirstOrDefault().ID;
                    }

                    HttpCookie cookie = new HttpCookie(Constant.APP_CURRENT_VESTION);
                    cookie.Value = timeVersion.ToString();
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }

            }
            catch (Exception)
            {
               
            }
        }

        //Mã hóa MD5
        public static string EncryptKey(string EncryptKey)
        {
            using (MD5 md5hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5hash.ComputeHash(Encoding.UTF8.GetBytes(EncryptKey));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        public static void UserSignIn(UserModel accountInfo, HttpContext curentHttpContext)
        {
            var token = SetAuthCookie(curentHttpContext, JsonConvert.SerializeObject(accountInfo), Constant.ShopOnline, Constant.ShopKeyAuthen, DateTime.Now.AddDays(15));
        }
        public static void UserSignInAdmin(UserModel accountInfo, HttpContext curentHttpContext)
        {
            var token = SetAuthCookie(curentHttpContext, JsonConvert.SerializeObject(accountInfo), Constant.AdminShopOnline, Constant.ShopKeyAuthenAdmin, DateTime.Now.AddDays(7));
        }
        public static void ProductView(List<ProductModel> products, HttpContext curentHttpContext)
        {
            var token = SetAuthCookie(curentHttpContext, JsonConvert.SerializeObject(products), Constant.ViewProduct, Constant.ShopKeyViewProduct, DateTime.Now.AddDays(7));
        }

        public static string SetAuthCookie(HttpContext httpContext, string authenticationTicket, string cookieName, string key, DateTime Expiration)
        {
            var encryptedTicket = Utils.Encrypt(authenticationTicket, key);
            var cookie = new HttpCookie(cookieName, encryptedTicket)
            {
                HttpOnly = true,
                Expires = Expiration
            };
            httpContext.Response.Cookies.Add(cookie);
            return encryptedTicket;
        }

        public static void Logout(HttpContext httpContext)
        {
            var cookie = new HttpCookie(Constant.ShopOnline);
            DateTime nowDateTime = DateTime.Now;
            cookie.Expires = nowDateTime.AddDays(-1);
            httpContext.Response.Cookies.Add(cookie);
            httpContext.Request.Cookies.Remove(Constant.ShopOnline);
            FormsAuthentication.SignOut();
        }
        public static void LogoutAdmin(HttpContext httpContext)
        {
            var cookie = new HttpCookie(Constant.AdminShopOnline);
            DateTime nowDateTime = DateTime.Now;
            cookie.Expires = nowDateTime.AddDays(-1);
            httpContext.Response.Cookies.Add(cookie);
            httpContext.Request.Cookies.Remove(Constant.AdminShopOnline);
            FormsAuthentication.SignOut();
        }

        public static void SetChatCokkie(List<ChatModel> chats, HttpContext curentHttpContext)
        {
            SetAuthCookie(curentHttpContext, JsonConvert.SerializeObject(chats), Constant.ChatOnline, Constant.KeyChatOnline, DateTime.Now.AddHours(2));
        }
    }
}