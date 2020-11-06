using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S2Please.Database;
using S2Please.Models;
using S2Please.Areas.WEB_SHOP.ViewModel;
using Newtonsoft.Json;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using S2Please.Helper;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.Caching;
using SHOP.COMMON;
using System.Collections;
using S2Please.Areas.WEB_SHOP.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using S2Please.ViewModel;
using System.Web.Routing;
using System.Web.Security;
using S2Please.ParramType;
using System.Net;
namespace S2Please.Controllers
{
    public class BaseController : Controller
    {
        // GET: Home
        private string _connection = ConfigurationManager.AppSettings["DBConnection"];
        public ResultModel ListProcedure<T>(T model, string stroedProcedure, List<Param> param, bool IsCache = false, bool IsCheckPermison = false)
        {
            //check quyền
            if (IsCheckPermison)
            {
                var stroedSplit = stroedProcedure.Split('_');
                bool check = CheckPermission(stroedSplit[0], stroedSplit[1], CurrentUser.UserAdmin.USER_ID);
                if (!check)
                {
                    return new ResultModel() { Success=false,StatusCode=404};
                }
            }
            var result = new List<dynamic>();
            SqlCommand cacheTotal = new SqlCommand();
            string data = string.Empty;
            try
            {
                //sử dụng cache
                if (IsCache)
                {
                    var cacheValueName = string.Empty;
                    var cacheParam = string.Empty;
                    var message = string.Empty;
                    if (param != null && param.Count() > 0)
                    {
                        var listParam = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(param));
                        if (listParam != null)
                        {
                            message = JsonConvert.SerializeObject(listParam);
                            cacheParam = message;
                        }
                        else
                        {
                            message = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(param)));
                        }
                    }
                    cacheValueName = stroedProcedure + "-" + cacheParam + "-" + CurrentUser.User.USER_ID.ToString();
                    var cacheValueNameOutput = stroedProcedure + "-" + cacheParam + "-" + CurrentUser.User.USER_ID.ToString() + "-output";
                    result = HttpRuntime.Cache.Get(cacheValueName) as dynamic;
                    cacheTotal = HttpRuntime.Cache.Get(cacheValueNameOutput) as SqlCommand;
                    if (result == null)
                    {
                        result = new List<dynamic>();
                        var response = GetData<T>(model, stroedProcedure, param);
                        result = response.Results;
                        HttpRuntime.Cache.Insert(cacheValueName, result);
                        HttpRuntime.Cache.Insert(cacheValueNameOutput, response.OutValue);
                        //using (var connection = new SqlConnection(_connection))
                        //{
                        //    if (connection.State == ConnectionState.Closed)
                        //    {
                        //        connection.Open();
                        //    }
                        //    SqlDependency.Start(_connection);

                        //    var command = new SqlCommand("Dependency_" + stroedProcedure, connection);
                        //    command.CommandType = CommandType.StoredProcedure;
                        //    var dependency = new SqlCacheDependency(command);
                        //    HttpRuntime.Cache.Insert(cacheValueName, result, dependency);
                        //    HttpRuntime.Cache.Insert(cacheValueNameOutput, response.OutValue);
                        //    try
                        //    {
                        //        command.ExecuteNonQuery();
                        //        return new ResultModel
                        //        {
                        //            StatusCode = 0,
                        //            Success = true,
                        //            Results = result,
                        //        };
                        //    }
                        //    catch (Exception)
                        //    {
                        //        HttpRuntime.Cache.Remove(cacheValueName);
                        //        HttpRuntime.Cache.Remove(cacheValueNameOutput);

                        //        return new ResultModel
                        //        {
                        //            StatusCode = 0,
                        //            Success = false,
                        //            Results = result,
                        //            error= response.error
                        //        };
                        //    }
                        //}
                    }
                }
                else
                {
                    var response = GetData<T>(model, stroedProcedure, param);
                    if (!response.Success)
                    {
                        var modelError = new ErrorModel();
                       
                        var param1 = new List<Param>();
                        param1.Add(new Param { Key = "@ERROR_NUM", Value = new Random().Next(10000, 99999).ToString() });
                        param1.Add(new Param { Key = "@ERROR_MSG", Value = response.error });
                        param1.Add(new Param { Key = "@ERROR_PROC", Value = stroedProcedure });
                        param1.Add(new Param { Key = "@CREATED_BY", Value = CurrentUser.UserAdmin.USER_ID.ToString() });
                        var resultProductGroups = ListProcedure<ErrorModel>(modelError, "utl_Insert_ErrorLog", param1);
                        return new ResultModel
                        {
                            StatusCode = 0,
                            Success =false,
                            Results = result,
                            OutValue = response.OutValue,
                            Message = response.Message
                        };
                    }
                    result = response.Results;
                    return new ResultModel
                    {
                        StatusCode = 0,
                        Success = response.Success,
                        Results = result,
                        OutValue = response.OutValue,
                        Message=response.Message
                    };
                }
                return new ResultModel
                {
                    StatusCode = 0,
                    Success = true,
                    Results = result,
                    error = stroedProcedure,
                    OutValue= cacheTotal
                };
            }
            catch (Exception ex)
            {
                var modelError = new ErrorModel();
                var param1 = new List<Param>();
                param.Add(new Param { Key = "@ERROR_NUM", Value = new Random().Next(10000, 99999).ToString().ToString() });
                param.Add(new Param { Key = "@ERROR_MSG", Value = ex.Message });
                param.Add(new Param { Key = "@ERROR_PROC", Value = stroedProcedure });
                param.Add(new Param { Key = "@CREATED_BY", Value = "1" });
                var resultProductGroups = ListProcedure<ErrorModel>(modelError, "utl_Insert_ErrorLog", param);
                return new ResultModel
                {
                    StatusCode = 200,
                    Success = false,
                    Results = new List<dynamic>(),
                    error = stroedProcedure + "- " + ex.Message
                };               
            }
        }

        public ResultModel GetData<T>(T model, string stroedProcedure, List<Param> param)
        {
            var result = new List<dynamic>();
            var outPutDynamic = string.Empty;
            SqlCommand outValue = new SqlCommand();
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = _connection;
                        con.Open();
                    }
                    using (var cmd = new SqlCommand(stroedProcedure, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (param != null && param.Count() > 0)
                        {
                            foreach (var item in param)
                            {

                                if (item.IsUserDefinedTableType)
                                {
                                    cmd.Parameters.Add(item.paramUserDefinedTableType);
                                }
                                else if (item.IsOutPut)
                                {
                                    if (item.Type == "Int")
                                    {
                                        cmd.Parameters.Add(item.Key, SqlDbType.Int).Direction = ParameterDirection.Output;
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add(item.Key, SqlDbType.NVarChar, 500);
                                        cmd.Parameters[item.Key].Direction = ParameterDirection.Output;

                                    }
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                                }
                            }
                        }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var dynamic = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(model));
                                if (dr.FieldCount > 0)
                                {
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        byte[] img;
                                        var column = dr.GetName(i);
                                        
                                        var propVal = dr[column].ToString();
                                        PropertyInfo propertyInfo = model.GetType().GetProperty(column);
                                        if (propertyInfo != null)
                                        {
                                            Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                                            if (t.Name == "DateTime" && propVal == "")
                                            {
                                                propVal = null;
                                            }
                                            else if (t.Name == "String" && propVal == "")
                                            {
                                                propVal = "";
                                            }
                                            else if (t.Name != "Boolean" && propVal == "")
                                            {
                                                propVal = "0";
                                            }
                                            else if (t.Name == "Boolean" && propVal != "")
                                            {
                                                if (propVal == "0")
                                                {
                                                    propVal = "False";
                                                }
                                                else if (propVal == "1")
                                                {
                                                    propVal = "True";
                                                }
                                            }
                                            if (dr.GetFieldType(i).Name == "Byte[]")
                                            {
                                                if (!string.IsNullOrEmpty(dr[column].ToString()))
                                                {
                                                    img = (byte[])(dr[column]);
                                                    if (img.Count() > 0)
                                                    {
                                                        dynamic[column] = Convert.ToBase64String(img);
                                                    }
                                                }
                                               
                                            }
                                            else
                                            {
                                                dynamic[column] = propVal;
                                            }
                                        }
                                    }
                                    result.Add(dynamic);
                                }

                            }
                        }
                        outValue = cmd;
                    }

                }
            }
            catch (Exception ex)
            {
                var parramError = string.Empty;
                if (param != null && param.Count() > 0)
                {
                    var listParam = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(param));
                    if (listParam != null)
                    {
                        parramError = JsonConvert.SerializeObject(listParam);
                    }
                    else
                    {
                        parramError = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(param)));
                    }
                }

                return new ResultModel()
                {
                    Results = result,
                    OutValue = outValue,
                    Success = false,
                    error = "StroedName:"+stroedProcedure + " - " + " MessageError "+ ex.Message + " - Parram:"+ parramError
                };
            }

            return new ResultModel() {
                Results = result,
                OutValue = outValue,
                Success=true
            };
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rawUrl = System.Web.HttpContext.Current.Request.RawUrl;
            if (System.Web.HttpContext.Current.Request.FilePath.ToString().ToLower().IndexOf("admin") > -1)
            {
                if (filterContext.ActionDescriptor.ActionName.ToLower() != "login")
                {
                    if (CurrentUser.UserAdmin == null || CurrentUser.UserAdmin.USER_ID == 0)
                    {
                        if (filterContext.ActionDescriptor.ActionName.ToLower().Contains("sendrequestchangepass") || filterContext.ActionDescriptor.ActionName.ToLower().Contains("forgotpassword"))
                        {
                            goto Finish;
                        }
                        filterContext.Result = new RedirectToRouteResult(new
             System.Web.Routing.RouteValueDictionary(new { controller = "Authen", action = "Login", RedirectUrl = rawUrl }));
                    }
                                 
                    else
                    {
                        goto Finish;
                    }
                }
                else
                {
                    if (CurrentUser.UserAdmin == null || CurrentUser.UserAdmin.USER_ID == 0)
                    {
                        goto Finish;
                    }                 
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
            System.Web.Routing.RouteValueDictionary(new { controller = "Dashboard", action = "Index", RedirectUrl = rawUrl }));
                    }                   
                }
            }
           
            Finish:
            base.OnActionExecuting(filterContext);
        }

        public bool CheckPermision(int status)
        {
            if (status == 404)
            {
                return false;
            }
            return true;
        }

        public ActionResult ChangeLanguage(int languageId)
        {
            bool check = Security.ChangeCookieLanguage(languageId);
            return Json(check, JsonRequestBehavior.AllowGet);
        }


        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);

                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public ResultModel ExecSqlString(string sqlString)
        {
            var result = new List<dynamic>();
            try
            {
                using (var con = new SqlConnection(_connection))
                {
                    using (var cmd = new SqlCommand(sqlString, con))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            var dynamic = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(new Dynamic()));
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                var column = dr.GetName(i);
                                dynamic[column] = column;

                            }
                            result.Add(dynamic);

                            while (dr.Read())
                            {
                                dynamic = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(new Dynamic()));
                                if (dr.FieldCount > 0)
                                {
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        var column = dr.GetName(i);
                                        var propVal = dr[column].ToString();
                                        dynamic[column] = propVal;
                                    }

                                    result.Add(dynamic);
                                }

                            }
                        }
                    }
                    return new ResultModel()
                    {
                        Success = true,
                        Results = result
                    };
                }
            }
            catch (Exception ex)
            {

                return new ResultModel()
                {
                    Success = false,
                    error = ex.Message.ToString(),
                };
            }
        }

        public ActionResult Error(ResultModel model)
        {
            return View(model);
        }

        public int SendMail(string subject, string body, List<string> toMail, List<string> cc, List<string> bcc, string from, string replyTo, List<AttachmentJs> attachments)
        {

            var lstAttach = new List<Attachment>();
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var dir = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"].Replace("\\", "/");
                    var path = Path.Combine(dir, attachment.Realname);
                    if (System.IO.File.Exists(path))
                    {
                        lstAttach.Add(new Attachment(new MemoryStream(System.IO.File.ReadAllBytes(path)), attachment.Name));
                    }
                }
            }
            var resultCode = EmailHelper.SendMail(subject, body, from, toMail, cc, bcc, lstAttach);
            return resultCode;
        }

        public ActionResult Remove(long id,string linkRemove)
        {
            RemoveViewModel vm = new RemoveViewModel();
            vm.Id = id;
            vm.LinkRemove = linkRemove;
            var html = RenderViewToString(this.ControllerContext, "~/Views/Base/_Remove.cshtml", vm);
            return Json(new { html }, JsonRequestBehavior.AllowGet);
        }

        public bool CheckPermission(string menuName,string actionName,long userID)
        {           
            if (CurrentUser.UserAdmin.USER_NAME.ToLower()=="admin")
            {
                return true;
            }
            else
            {
                try
                {
                    var result = ExecSqlString("SELECT " +
                                         " MA.MENU_NAME," +
                                         " CP.[PERMISSION_NAME]" +
                                     " FROM dbo.EMPLOYEE_ROLE_PERMISSON ERP" +
                                     " INNER JOIN dbo.MENU_ADMIN_PERMISSION MAP ON MAP.ID = ERP.MENU_PERMISSION_ID AND MAP.IS_DELETED = 0" +
                                     " INNER JOIN dbo.CL_PERMISSION CP ON CP.ID = MAP.PERMISSION_ID AND CP.IS_DELETED = 0" +
                                     " INNER JOIN dbo.MENU_ADMIN MA ON MA.ID = MAP.MENU_ADMIN_ID AND MA.IS_DELETED = 0" +
                                     " INNER JOIN dbo.EMPLOYES E ON E.ID=" + userID + " AND E.IS_DELETED=0" +
                                     " WHERE ERP.DATA_TYPE = 0 AND ERP.DATA_ID = " + userID + " AND ERP.IS_DELETED = 0" +
                                     " UNION" +
                                     " SELECT" +
                                         " MA.MENU_NAME," +
                                         " CP.[PERMISSION_NAME]" +
                                     " FROM dbo.EMPLOYEE_ROLE_PERMISSON ERP" +
                                     " INNER JOIN dbo.MENU_ADMIN_PERMISSION MAP ON MAP.ID = ERP.MENU_PERMISSION_ID AND MAP.IS_DELETED = 0" +
                                     " INNER JOIN dbo.CL_PERMISSION CP ON CP.ID = MAP.PERMISSION_ID AND CP.IS_DELETED = 0" +
                                     " INNER JOIN dbo.MENU_ADMIN MA ON MA.ID = MAP.MENU_ADMIN_ID AND MA.IS_DELETED = 0" +
                                     " INNER JOIN dbo.CL_ROLE CR ON CR.ID= (SELECT TOP 1 ROLE_ID FROM dbo.EMPLOYEE_ROLE_MAPPER WHERE EMPLOYEE_ID="+userID+" )  AND CR.IS_DELETED=0" +
                                     " WHERE ERP.DATA_TYPE = 1 AND ERP.DATA_ID = (SELECT TOP 1 ROLE_ID FROM dbo.EMPLOYEE_ROLE_MAPPER WHERE EMPLOYEE_ID=" + userID + " ) AND ERP.IS_DELETED = 0"
                                    );
                    if (result != null && result.Results.Count() > 0)
                    {
                        var listData = JsonConvert.DeserializeObject<List<CheckPermission>>(JsonConvert.SerializeObject(result.Results));
                        var data = listData.Where(s => s.MENU_NAME == menuName && s.PERMISSION_NAME == actionName).ToList();
                        if (data !=null && data.Count()>0)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception)
                {

                    return false;
                }
              
            }
            return false;
        }

        public ActionResult Page404()
        {
            Page404Model vm = new Page404Model();
            vm.USER_NAME = CurrentUser.UserAdmin.USER_NAME;
            return View(vm);
        }

        public  List<AttachmentJs> GetFileUpload(IEnumerable<HttpPostedFileBase> attachment)
        {
            var result = new List<AttachmentJs>();
            // The Name of the Upload component is "files"
            if (attachment != null)
            {
                foreach (var file in attachment)
                {
                    //check if file exist and rename
                    var dir = System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["UploadFolder"]);
                    string newFileName = file.FileName;
                    var path = Path.Combine(dir.FullName, newFileName);
                    var index = 0;
                    {
                        string fName = Path.GetFileNameWithoutExtension(file.FileName);
                        string fExt = Path.GetExtension(file.FileName);
                        newFileName = String.Concat(fName, string.Format("_{0}", index), fExt);
                        path = Path.Combine(dir.FullName, newFileName);
                        index++;
                    }
                    file.SaveAs(path);
                    result.Add(new AttachmentJs()
                    {
                        Id = 0,
                        Realname = newFileName,
                        Name = file.FileName,
                        Extension = Path.GetExtension(file.FileName),
                        Size = file.ContentLength,
                        CreatedDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    });
                }
            }
            return result;
        }

        public  bool RemoveFileUpload(string fileName)
        {
            bool isSuccess = false;
            if (!string.IsNullOrEmpty(fileName))
            {
                var path = Server.MapPath("~/Image/" + fileName);
                if (System.IO.File.Exists(path))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(path);
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public ActionResult getValueLanguage(string key)
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
            return Json(new { key }, JsonRequestBehavior.AllowGet);
        }

        public List<FileModel> SaveFile(IEnumerable<HttpPostedFileBase> attachment)
        {
            // The Name of the Upload component is "files"
            LogHelper.LogError("CHECK 1 " + attachment.Count().ToString(), "") ;
            List<FileModel> listFile = new List<FileModel>();          
            try
            {
                if (attachment != null)
                {
                    var index = 0;

                    foreach (var file in attachment)
                    {
                        //check if file exist and rename
                        var check = false;                   
                        var files = Directory.GetFiles(Server.MapPath("~/Image"));
                        do
                        {
                            string fName = Path.GetFileNameWithoutExtension(file.FileName);
                            string fExt = Path.GetExtension(file.FileName);
                            var newFileName = String.Concat(fName, string.Format("_{0}", index), fExt);
                            var fileExists = files.Where(s => s.Contains(newFileName)).ToList();
                            if (fileExists == null || fileExists.Count == 0)
                            {
                                var pathNew = Server.MapPath("~/Image/" + newFileName);
                                file.SaveAs(pathNew);
                                listFile.Add(new FileModel()
                                {
                                    ID = 0,
                                    FILE_NAME = newFileName,
                                    SIZE = file.ContentLength.ToString(),
                                    TYPE = file.ContentType
                                });
                                check = true;
                            }
                            else
                            {
                                index++;
                            }
                        } while (!check);
                        

                    }
                    return listFile;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex.Message, "");
                return new List<FileModel>();
            }
            return new List<FileModel>(); 
        }


        //public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> attachment)
        //{
        //    int index = 0;
        //    ProductModel product = new ProductModel();
        //    List<ProductImgType> productImgs = new List<ProductImgType>();
        //    if (Session["Product"] != null)
        //    {
        //        product = Session["Product"] as ProductModel;
        //        Session["Product"] = null;
        //        foreach (var file in attachment)
        //        {
        //            try
        //            {
        //                //check if file exist and rename
        //                //var dir = System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["UploadFolder"]);
        //                var dir = System.IO.Directory.CreateDirectory("~/Image");

        //                //string newFileName = file.FileName;
        //                //var path = Path.Combine(dir.FullName, newFileName);
        //                //var index = 0;
        //                //{
        //                //    string fName = Path.GetFileNameWithoutExtension(file.FileName);
        //                //    string fExt = Path.GetExtension(file.FileName);
        //                //    newFileName = String.Concat(fName, string.Format("_{0}", index), fExt);
        //                //    path = Path.Combine(dir.FullName, newFileName);
        //                //    index++;
        //                //}
        //                var path = Server.MapPath("~/Image/" + file.FileName);
        //                file.SaveAs(path);
        //            }
        //            catch (Exception)
        //            {

        //                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);

        //            }

        //        }
        //    }

        //    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult removeFile(long id, string fileName)
        {
            bool check = RemoveFileUpload(fileName);
            return Json(new { Success = check }, JsonRequestBehavior.AllowGet);
        }
    }
    public class Dynamic{
        public long ID { get; set; }
    }
    public class CheckPermission
    {
        public string MENU_NAME { get; set; }
        public string PERMISSION_NAME { get; set; }

    }
}