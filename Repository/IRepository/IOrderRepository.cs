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
        ResultModel GetOrderById(long id, bool isCheckPermission = false);

        //Get list order from admin
        ResultModel GetOrderFromAdmin(ParamType paramType);

        //Get Update_Order
        ResultModel UpdateOrder(OrderModel model, List<OrderDetailType> orderDetails, bool isCheckPermission = false);

        //Get Order By Customer Id
        ResultModel GetOrderByCustomerId(long userId);

        //delete order
        ResultModel DeleteById(long id);

        //Update status order by list id
        ResultModel UpdateStatusOrder(List<DataIdType> listDatas, long status, bool isCheckPermission = false);
    }
}
