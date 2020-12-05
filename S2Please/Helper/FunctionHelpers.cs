using System;
using System.Collections.Generic;
using System.Linq;
using SHOP.COMMON;
using System.Text;
using System.Security.Cryptography;
using S2Please.Controllers;
using S2Please.Models;
using Newtonsoft.Json;
using Repository;
using SHOP.COMMON.Helpers;
using System.Collections;
using System.Web;

namespace S2Please.Helper
{
    public static class FunctionHelpers
    {
        public static CommonRepository commonRepository = new CommonRepository();
        public static string GetValueLanguage(string key)
        {
            //Lấy danh sách đa ngôn ngữ
            var model = new MultiLanguageModel();
            var param = new List<Param>();
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
            var response = commonRepository.ListProcedure<MultiLanguageModel>(model, "MultiLanguage_Get_MultiLanguage", paramType, true);
            var result = JsonConvert.DeserializeObject<List<MultiLanguageModel>>(JsonConvert.SerializeObject(response.Results));
            var data = result.Where(s => s.KEY == key && s.LANGUAGE_ID == long.Parse(CurrentUser.LANGUAGE_ID.ToString())).FirstOrDefault();
            if (data != null)
            {
                key = data.VALUE;
            }
            return key;
        }
        //Mã hóa model to token
        public static string Encrypt(string toEncrypt, string key, bool useHashing = true)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length).ToString().Replace("/","V1997").Replace("+","-").Replace("&","And");
        }
        //dich ngược token
        public static string Decrypt(string toDecrypt, string key, bool useHashing = true)
        {
            var rt = "";
            if (toDecrypt==null || toDecrypt=="")
            {
                return rt;
            }
            toDecrypt= toDecrypt.Replace("V1997", "/").Replace("-", "+").Replace("And1997", "&");
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                rt = UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                //return "";
            }
            return rt;
        }
        public static string ConvertKey(string toDecrypt, string key)
        {
            var encryptedTicket = Decrypt(toDecrypt, key);
            return encryptedTicket;
        }

        public static string KeyEncrypt = "Key_";
        //public static string KeyEncryptProductType = "Key_Product_Type";
        public static string RandomProductCode()
        {
            Random rand = new Random();
            int value=rand.Next(10000, 99999);
            int value2 = rand.Next(10000, 99999);
            return value.ToString() + value2.ToString();
        }

        public static string GetValueLocalization(long dataId,string dataType,string propertyName)
        {
            //Lấy danh sách đa ngôn ngữ
            string key = string.Empty;
            var model = new LocalizationModel();
            var param = new List<Param>();
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
            var response = commonRepository.ListProcedure<LocalizationModel>(model, "Localization_Get_Localization", paramType, true);
            var result = JsonConvert.DeserializeObject<List<LocalizationModel>>(JsonConvert.SerializeObject(response.Results));
            var data = result.Where(s=>s.DATA_ID== dataId && s.DATA_TYPE== dataType && s.LANGUAGE_ID==CurrentUser.LANGUAGE_ID && s.PROPERTY_NAME== propertyName).FirstOrDefault();
            if (data != null)
            {
                key = data.PROPERTY_VALUE;
            }
            else
            {
                key = "{NoData}";
            }
            return key;
        }

        public static string GetValueLocalizationClass(long dataId, string dataType)
        {
            //Lấy danh sách đa ngôn ngữ
            string key = string.Empty;
            var model = new LocalizationModel();
            var param = new List<Param>();
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
            var response = commonRepository.ListProcedure<LocalizationModel>(model, "LocalizationClass_Get_LocalizationClass", paramType, true);
            var result = JsonConvert.DeserializeObject<List<LocalizationModel>>(JsonConvert.SerializeObject(response.Results));
            var data = result.Where(s => s.DATA_ID == dataId && s.DATA_TYPE == dataType).FirstOrDefault();
            if (data != null)
            {
                key = data.PROPERTY_VALUE;
            }
            else
            {
                key = "{NoData}";
            }
            return key;
        }

        public static bool CheckPermission(string menuName,string action)
        {
            return commonRepository.CheckPermission(menuName, action,CurrentUser.UserAdmin.USER_ID);
        }
        public static string GetTimeVersion()
        {
            //Lấy bản ghi time version
            long timeVersion = 0;
            var param = new List<Param>();
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
            var response = commonRepository.ListProcedure<TimeVersion>(new TimeVersion(), "TimeVersion_Get_TimeVersion", paramType);
            var result = JsonConvert.DeserializeObject<List<TimeVersion>>(JsonConvert.SerializeObject(response.Results));
            if (result!=null && result.Count()>0)
            {
                timeVersion = result.FirstOrDefault().ID;
                if (CurrentUser.TIME_VERSION != timeVersion)
                {
                    RemoveAllCache();
                    Security.CheckCookieTimeVersion(timeVersion);
                }
            }       
            return timeVersion.ToString();
        }

        public static void RemoveCacheByProcedure(string key)
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

            while (enumerator.MoveNext())
                keys.Add(enumerator.Key.ToString());

            var listKey = key.Split(';');
            for (int i = 0; i < keys.Count; i++)
            {
                foreach (var item in listKey)
                {
                    if (keys[i].Contains(item))
                    {
                        HttpRuntime.Cache.Remove(keys[i]);
                    }
                }              
            }
           
        }

        public static void RemoveAllCache()
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

            while (enumerator.MoveNext())
                keys.Add(enumerator.Key.ToString());

            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].IndexOf("LoggedInUsers") == -1)
                {
                    HttpRuntime.Cache.Remove(keys[i]);
                }

            }
        }

        public static void InsertTimeVersion()
        {
            var param = new List<Param>();
            var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
            var response = commonRepository.ListProcedure<TimeVersion>(new TimeVersion(), "TimeVersion_Update_TimeVersion", paramType);
        }

        public static string getTimeAgo(DateTime strDate)
        {
            string strTime = string.Empty;
            if (IsDate(Convert.ToString(strDate)))
            {
                TimeSpan t = DateTime.Now - Convert.ToDateTime(strDate);
                double deltaSeconds = t.TotalSeconds;

                double deltaMinutes = deltaSeconds / 60.0f;
                int minutes;

                if (deltaSeconds < 5)
                {
                    return FunctionHelpers.GetValueLanguage("TimeAgo.JustNow");
                }
                else if (deltaSeconds < 60)
                {
                    return Math.Floor(deltaSeconds) + " "+ FunctionHelpers.GetValueLanguage("Seconds ago").ToLower();
                }
                else if (deltaSeconds < 120)
                {
                    return FunctionHelpers.GetValueLanguage("TimeAgo.AMinuteAgo");
                }
                else if (deltaMinutes < 60)
                {
                    return Math.Floor(deltaMinutes) + " " + FunctionHelpers.GetValueLanguage("TimeAgo.MinutesAgo").ToLower();
                }
                else if (deltaMinutes < 120)
                {
                    return FunctionHelpers.GetValueLanguage("TimeAgo.AnHourAgo");
                }
                else if (deltaMinutes < (24 * 60))
                {
                    minutes = (int)Math.Floor(deltaMinutes / 60);
                    return minutes + " " + FunctionHelpers.GetValueLanguage("TimeAgo.HoursAgo").ToLower();
                }
                else if (deltaMinutes < (24 * 60 * 2))
                {
                    return FunctionHelpers.GetValueLanguage("TimeAgo.Yesterday");
                }
                else if (deltaMinutes < (24 * 60 * 7))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24));
                    return minutes + " " +FunctionHelpers.GetValueLanguage("TimeAgo.DaysAgo").ToLower();
                }
                else if (deltaMinutes < (24 * 60 * 14))
                {
                    return FunctionHelpers.GetValueLanguage("TimeAgo.LastWeek");
                }
                else if (deltaMinutes < (24 * 60 * 31))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 7));
                    return minutes + " " + FunctionHelpers.GetValueLanguage("TimeAgo.WeeksAgo").ToLower();
                }
                else if (deltaMinutes < (24 * 60 * 61))
                {
                    return FunctionHelpers.GetValueLanguage("TimeAgo.LastMonth");
                }
                else if (deltaMinutes < (24 * 60 * 365.25))
                {
                    minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 30));
                    return minutes + " " + FunctionHelpers.GetValueLanguage("TimeAgo.LastMonth").ToLower();
                }
                else if (deltaMinutes < (24 * 60 * 731))
                {
                    return FunctionHelpers.GetValueLanguage("TimeAgo.LastYear");
                }

                minutes = (int)Math.Floor(deltaMinutes / (60 * 24 * 365));
                return minutes + " "+ FunctionHelpers.GetValueLanguage("TimeAgo.LastYear");
            }
            else
            {
                return "";
            }
        }

        public static bool IsDate(string o)
        {
            DateTime tmp;
            return DateTime.TryParse(o, out tmp);
        }
    }
}