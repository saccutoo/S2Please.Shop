using Hrm.Common.Helpers;
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
    public partial class OrderRepository : CommonRepository, IOrderRepository
    {
        //Get order detail by order id
        public ResultModel GetOrderDetailByOrderId(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ORDER_ID", Value = id.ToString() });
            return ListProcedure<OrderDetailModel>(new OrderDetailModel(), "OrderDetail_Get_GetOrderDetailByOrderId", param);
        }

        //Get order by order id
        public ResultModel GetOrderById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Get_GetOrderByID", param);
        }

        //Get list order from admin
        public ResultModel GetOrderFromAdmin(ParamType paramType)
        {
            var param = new List<Param>();
            var basicParamype = new List<ParamType>();
            basicParamype.Add(paramType);
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamype)
                }
            });
            param.Add(new Param { Key = "@TotalRecord", Value = "0", IsOutPut = true, Type = "Int" });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Get_OrderFromAdmin", param, false, true);
        }
    }
}
