using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SHOP.COMMON.Entity;
using SHOP.COMMON.Helpers;
using Newtonsoft.Json;

namespace SHOP.COMMON
{
    public static class CurrentUser
    {
        //Get language id
        public static long LANGUAGE_ID
        {
            get
            {
                try
                {
                    HttpCookie getCookie = HttpContext.Current.Request.Cookies[Constant.APP_CURRENT_LANG];
                    if (getCookie != null)
                    {
                        return long.Parse(getCookie.Value);
                    }
                    else
                    {
                        HttpCookie cookie = new HttpCookie(Constant.APP_CURRENT_LANG);
                        cookie.Value = Language.VIETNAM;
                        cookie.Expires = DateTime.Now.AddYears(1);
                        HttpContext.Current.Response.Cookies.Add(cookie);
                        return long.Parse(Language.VIETNAM);
                    }

                }
                catch (Exception)
                {
                    HttpCookie cookie = new HttpCookie(Constant.APP_CURRENT_LANG);
                    cookie.Value = Language.VIETNAM;
                    cookie.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    return long.Parse(Language.VIETNAM);
                }
            }
        }

        //get user web
        public static User User
        {
            get
            {
                try
                {
                    HttpCookie getCookie = HttpContext.Current.Request.Cookies[Constant.ShopOnline];
                    if (getCookie != null)
                    {
                        var value = getCookie.Value;
                        string strConvertKey = Utils.Decrypt(value, Constant.ShopKeyAuthen);
                        var user = JsonConvert.DeserializeObject<User>(strConvertKey);
                        return JsonConvert.DeserializeObject<User>(strConvertKey);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return new User();
            }
        }

        //get user admin
        public static User UserAdmin
        {
            get
            {
                try
                {
                    HttpCookie getCookie = HttpContext.Current.Request.Cookies[Constant.AdminShopOnline];
                    if (getCookie != null)
                    {
                        var value = getCookie.Value;
                        string strConvertKey = Utils.Decrypt(value, Constant.ShopKeyAuthenAdmin);
                        var user = JsonConvert.DeserializeObject<User>(strConvertKey);
                        return JsonConvert.DeserializeObject<User>(strConvertKey);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return new User();
            }
        }

        //Get time verstion
        public static long TIME_VERSION
        {
            get
            {
                try
                {
                    HttpCookie getCookie = HttpContext.Current.Request.Cookies[Constant.APP_CURRENT_VESTION];
                    if (getCookie != null)
                    {
                        return long.Parse(getCookie.Value);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return 0;
            }
        }


        //Get chát
        public static List<ChatModel> Chats
        {
            get
            {
                try
                {
                    HttpCookie getCookie = HttpContext.Current.Request.Cookies[Constant.ChatOnline];
                    if (getCookie != null)
                    {
                        var value = getCookie.Value;
                        string strConvertKey = Utils.Decrypt(value, Constant.KeyChatOnline);
                        return JsonConvert.DeserializeObject<List<ChatModel>>(strConvertKey);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return new List<ChatModel>();
            }
        }
    }
}
