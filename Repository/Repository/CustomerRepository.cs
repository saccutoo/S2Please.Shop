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

        //update customer
        public ResultModel UpdateCustomer(CustomerModel model)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@FULL_NAME", Value = model.FULL_NAME });
            param.Add(new Param { Key = "@PHONE", Value = model.PHONE });
            param.Add(new Param { Key = "@EMAIL", Value = model.EMAIL });
            param.Add(new Param { Key = "@GENDER", Value = model.GENDER.ToString() });
            param.Add(new Param { Key = "@DATE_OF_BIRTH", Value = model.DATE_OF_BIRTH != null ? model.DATE_OF_BIRTH.Value.ToString("yyyy/MM/dd") : "" });
            param.Add(new Param { Key = "@FAX", Value = model.FAX });
            param.Add(new Param { Key = "@ADRESS_SPECIFIC", Value = model.ADRESS_SPECIFIC });
            param.Add(new Param { Key = "@CITY", Value = model.CITY.ToString() });
            param.Add(new Param { Key = "@DISTRICT", Value = model.DISTRICT.ToString() });
            param.Add(new Param { Key = "@COMMUNITY", Value = model.COMMUNITY.ToString() });
            param.Add(new Param { Key = "@IMAGE", Value = model.COMMUNITY.ToString() });
            return ListProcedure<CustomerModel>(new CustomerModel(), "Customer_Update_Customer", param);
        }
    }
}
