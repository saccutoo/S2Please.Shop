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
    public partial class CustomerRepository : CommonRepository, ICustomerRepository
    {
        //Get Customer By Id
        public ResultModel GetCustomerById(long userId)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@USER_ID", Value = userId.ToString() }) ;
            return ListProcedure<CustomerModel>(new CustomerModel(), "Customer_Get_GetCustomerById", param);
        }
    }
}
