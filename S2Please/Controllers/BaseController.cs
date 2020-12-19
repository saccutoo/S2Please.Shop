using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using System.Data;
using S2Please.Helper;
using System.IO;
using SHOP.COMMON;
using System.Net.Mail;
using S2Please.ViewModel;
using SHOP.COMMON.Helpers;

namespace S2Please.Controllers
{
    public class BaseController : Controller
    {
        // GET: Home
   
        //private string _connection = ConfigurationManager.AppSettings["DBConnection"];

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

        public ActionResult Error(ResultModel model)
        {
            return View(model);
        }

        public int SendMail(string subject, string body, List<string> toMail, List<string> cc, List<string> bcc, string from, string replyTo, List<AttachmentJs> attachments,long dataId=0,string dataType="")
        {

            var lstAttach = new List<Attachment>();
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var path = Server.MapPath("~/Image/" + attachment.Realname);
                    if (System.IO.File.Exists(path))
                    {
                        lstAttach.Add(new Attachment(new MemoryStream(System.IO.File.ReadAllBytes(path)), attachment.Name));
                    }
                }
            }
            var resultCode = EmailHelper.SendMail(subject, body, from, toMail, cc, bcc, lstAttach, dataId, dataType);
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

        public List<FileModel> SaveFile(IEnumerable<HttpPostedFileBase> attachment)
        {
            // The Name of the Upload component is "files"
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
                LogHelper.LogError(ex.Message, "SAVE_FILE_ERROR");
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

        public ActionResult ShowFormDelete(long id = 0)
        {
            BaseModel model = new BaseModel();
            model.ID = id;
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/_DeleteModal.cshtml", model);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StartChat(ChatModel model)
        {
            var validations = ValidationHelper.Validation(model, "model");
            if (validations.Count > 0)
            {
                return Json(new { Result = validations, Invalid = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = validations, Invalid = false }, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public ActionResult SessionChat(string chats)
        {
            if (!string.IsNullOrEmpty(chats))
            {
                var listChat = JsonConvert.DeserializeObject<List<ChatModel>>(chats);
                var listChatCookie = new List<ChatModel>();
                if (Session[Constant.ChatOnline]!=null)
                {
                    listChatCookie = Session[Constant.ChatOnline] as List<ChatModel>;
                }
                if (listChat==null || listChat.Count()==0)
                {
                    listChat = new List<ChatModel>();
                }
                var listChatCookieNew = new List<ChatModel>(listChat.Count +
                                    listChatCookie.Count);
                listChatCookieNew.AddRange(listChat);
                listChatCookieNew.AddRange(listChatCookie);
                Session[Constant.ChatOnline] = listChatCookieNew;
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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