﻿using Repository.Model;
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
        public ResultModel GetOrderById(long id,bool isCheckPermission=false)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Get_GetOrderByID", param,false, isCheckPermission);
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

        //Get Update_Order
        public ResultModel UpdateOrder(OrderModel model, List<OrderDetailType> orderDetails,bool isCheckPermission=false)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@ORDER_CODE", Value = new Random().Next(1000000, 9999999).ToString() });
            param.Add(new Param { Key = "@CUSTOMER_ID", Value = model.CUSTOMER_ID.ToString() });
            param.Add(new Param { Key = "@STATUS", Value = model.STATUS.ToString() });
            param.Add(new Param { Key = "@STATUS_PAY", Value = model.STATUS_PAY.ToString() });
            param.Add(new Param { Key = "@METHOD_PAY", Value = model.METHOD_PAY.ToString() });
            param.Add(new Param { Key = "@FEE_SHIP", Value = model.FEE_SHIP.ToString() });
            param.Add(new Param { Key = "@BONUS_ID", Value = model.BONUS_ID });
            param.Add(new Param { Key = "@DECRIPTION", Value = model.DECRIPTION });
            param.Add(new Param { Key = "@IS_ORDER", Value = model.IS_ORDER.ToString() });

            param.Add(new Param { Key = "@FULL_NAME", Value = model.FULL_NAME ==null ? " " :model.FULL_NAME });
            param.Add(new Param { Key = "@PHONE", Value = model.PHONE ==null? " " : model.PHONE });
            param.Add(new Param { Key = "@EMAIL", Value = model.EMAIL ==null ? " " : model.EMAIL });
            param.Add(new Param { Key = "@FAX", Value = model.FAX ==null? " " : model.FAX });
            param.Add(new Param { Key = "@ADRESS_SPECIFIC", Value = model.ADRESS_SPECIFIC==null ? " " : model.ADRESS_SPECIFIC });
            param.Add(new Param { Key = "@CITY", Value = model.CITY.ToString() });
            param.Add(new Param { Key = "@DISTRICT", Value = model.DISTRICT.ToString() });
            param.Add(new Param { Key = "@COMMUNITY", Value = model.COMMUNITY.ToString() });

            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@OrderDetailType", SqlDbType.Structured)
                {
                    TypeName = "dbo.OrderDetailType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(orderDetails)
                }
            });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Update_Order", param,false, isCheckPermission);
        }

        //Get Order By Customer Id
        public ResultModel GetOrderByCustomerId(long userId)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = userId.ToString() });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Get_GetOrderByCustomerId", param);
        }

        //delete order
        public ResultModel DeleteById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Delete_DeleteById", param);
        }

        //Update status order by list id
        public ResultModel UpdateStatusOrder(List<DataIdType> listDatas,long status, bool isCheckPermission = false)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@STATUS", Value = status.ToString() });
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@DataIdType", SqlDbType.Structured)
                {
                    TypeName = "dbo.DataIdType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(listDatas)
                }
            });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Update_UpdateStatusOrder", param, false, isCheckPermission);
        }

        //delete order
        public ResultModel Delete(long id, bool isCheckPermission = false)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Delete_DeleteOrder", param, false, isCheckPermission);
        }

        //Update status pay by list id
        public ResultModel UpdateStatusPay(List<DataIdType> listDatas, long statusPay, bool isCheckPermission = false)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@STATUS_PAY", Value = statusPay.ToString() });
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@DataIdType", SqlDbType.Structured)
                {
                    TypeName = "dbo.DataIdType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(listDatas)
                }
            });
            return ListProcedure<OrderModel>(new OrderModel(), "Order_Update_UpdateStatusPay", param, false, isCheckPermission);
        }

    }
}
