using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SHOP.COMMON;
using System.Text;
using System.Security.Cryptography;
using S2Please.Controllers;
using S2Please.Models;
using Newtonsoft.Json;
using System.Web.Mvc;
using SHOP.COMMON.Entity;
using SHOP.COMMON.Helpers;

namespace S2Please.Helper
{
    public static class FunctionHelpers
    {
        public static string GetValueLanguage(string key)
        {
            //Lấy danh sách đa ngôn ngữ
            BaseController baseC = new BaseController();
            var model = new MultiLanguageModel();
            var response = baseC.ListProcedure<MultiLanguageModel>(model, "MultiLanguage_Get_MultiLanguage", new List<Param>(), true);
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
            BaseController baseC = new BaseController();
            var model = new LocalizationModel();
            var response = baseC.ListProcedure<LocalizationModel>(model, "Localization_Get_Localization", new List<Param>(), true);
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
            BaseController baseC = new BaseController();
            var model = new LocalizationModel();
            var response = baseC.ListProcedure<LocalizationModel>(model, "LocalizationClass_Get_LocalizationClass", new List<Param>(), true);
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
            BaseController baseC = new BaseController();
            return baseC.CheckPermission(menuName, action,CurrentUser.UserAdmin.USER_ID);
        }

        public static UserModel GetCurrentUserAdmin()
        {
            var user = JsonConvert.DeserializeObject<UserModel>(JsonConvert.SerializeObject(CurrentUser.UserAdmin));
            return user;
        }
    }
}