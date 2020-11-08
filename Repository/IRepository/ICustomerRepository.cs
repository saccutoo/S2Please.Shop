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
    public partial interface ICustomerRepository
    {
        //Get Customer By Id
        ResultModel GetCustomerById(long userId);
    }
}
