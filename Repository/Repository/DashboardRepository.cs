using Repository.Model;
using Repository.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHOP.COMMON.Helpers;

namespace Repository
{
    public partial class DashboardRepository : CommonRepository, IDashboardRepository
    {
        //Get Dashboard
        public ResultModel GetDashboard()
        {
            var param = new List<Param>();
            return ListProcedure<OrderModel>(new OrderModel(), "Dashboard_Get_GetOrder", param, false, true);
        }
    }
}
