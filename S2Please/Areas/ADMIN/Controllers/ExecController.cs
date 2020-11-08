using System.Web.Mvc;
using S2Please.Models;
using S2Please.Controllers;
using Repository;
namespace S2Please.Areas.ADMIN.Controllers
{
    public class ExecController : BaseController
    {
        // GET: ADMIN/Exec
        private ISystemRepository _systemRepository;
        public ExecController(ISystemRepository systemRepository)
        {
            this._systemRepository = systemRepository;
        }
        public ActionResult Index(ResultModel model)
        {
            return View(model);
        }
        public ActionResult EXEC(string sql)
        {
            var result = _systemRepository.ExecSql(sql);
            result.CacheName = sql;
            ResultModel model = new ResultModel();
            model.Results = result.Results;
            model.StatusCode = result.StatusCode;
            model.Success = result.Success;
            model.CacheName = result.CacheName;
            model.error = result.error;
            model.OutValue = result.OutValue;
            model.Message = result.Message;
            model.Total = result.Total;
            model.Html = result.Html;
            return View("Index", model);
        }
    }
}