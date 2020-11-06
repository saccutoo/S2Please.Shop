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
using S2Please.ParramType;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Data;
using Hrm.Common.Helpers;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class ImportController : BaseController
    {
        // GET: ADMIN/Exec
        public ActionResult Index(ResultModel model)
        {
            return View(model);
        }
        public ActionResult ImportCountry(List<HttpPostedFileBase> attachment)
        {
            string fileName = string.Empty;
            var List = new List<ImportCountryType>();
            var param = new List<Param>();
            try
            {
                if (attachment != null && attachment.Count()>0)
                {
                    HttpPostedFileBase file = attachment[0];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator =1; rowIterator <= noOfRow; rowIterator++)
                            {
                                var country = new ImportCountryType();
                                country.CODE= workSheet.Cells[rowIterator, 1].Value == null ? 0 : long.Parse(workSheet.Cells[rowIterator, 1].Value.ToString());

                                country.NAME_VN = workSheet.Cells[rowIterator, 2].Value == null ? null : workSheet.Cells[rowIterator, 2].Value.ToString();

                                country.NAME_EN = workSheet.Cells[rowIterator, 3].Value == null ? null : workSheet.Cells[rowIterator, 3].Value.ToString();
                              
                                if (fileName.IndexOf("DISTRICT") !=-1)
                                {
                                    country.CODE_CITY = workSheet.Cells[rowIterator, 5].Value == null ? 0 : long.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                }
                                if (fileName.IndexOf("COMMUNITY") != -1)
                                {
                                    country.CODE_DISTRICT = workSheet.Cells[rowIterator, 5].Value == null ? 0 : long.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                }
                                List.Add(country);
                            }
                        }
                    }
                }
                if (List!=null && List.Count()>0)
                {
                    param.Add(new Param { Key = "@IS_CITY", Value = fileName.IndexOf("CITY")!=-1?"1":"0" });
                    param.Add(new Param { Key = "@IS_DISTRICT", Value = fileName.IndexOf("DISTRICT") != -1 ? "1" : "0" });
                    param.Add(new Param { Key = "@IS_COMMUNITY", Value = fileName.IndexOf("COMMUNITY") != -1 ? "1" : "0" });
                    param.Add(new Param
                    {
                        IsUserDefinedTableType = true,
                        paramUserDefinedTableType = new SqlParameter("@Country", SqlDbType.Structured)
                        {
                            TypeName = "dbo.CountryType",
                            Value = DataTableHelper.ConvertToUserDefinedDataTable(List)
                        }
                    });
                    var response = ListProcedure<ImportCountryType>(new ImportCountryType(), "ImportCountry", param);
                    var result = JsonConvert.DeserializeObject<List<ImportCountryType>>(JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception)
            {

                throw;
            }
           
          return View();
        }
    }
}