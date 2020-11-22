using System.Collections.Generic;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using S2Please.Controllers;
using Repository;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: ADMIN/Exec
        private IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            this._dashboardRepository = dashboardRepository;
        }
        public ActionResult Index()
        {
            var param = new List<Param>();
            var responseOrder = _dashboardRepository.GetDashboard();
            if (responseOrder!=null)
            {
                if (responseOrder.Success==false && CheckPermision(responseOrder.StatusCode)==false)
                {
                    return RedirectToRoute(new { action = "/Page404", controller = "Base",area=""  });

                }
                else
                {
                    var resultOrder = JsonConvert.DeserializeObject<List<OrderModel>>(JsonConvert.SerializeObject(responseOrder.Results));
                }
            }     
            return View();
        }
        public ActionResult WhitePage()
        {
            return View();
        }
    }
}