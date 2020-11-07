using Repository.Model;
using Repository.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public partial interface IOrderRepository
    {
        //Get order detail by order id
        ResultModel GetOrderDetailByOrderId(long id);

        //Get order by order id
        ResultModel GetOrderById(long id);

        //Get list order from admin
        ResultModel GetOrderFromAdmin(ParamType paramType);
    }
}
