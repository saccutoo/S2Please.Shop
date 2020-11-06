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
    public partial interface IProductRepository
    {
        //Get sản phẩm theo mã
        ResultModel GetProductById(long id);

        //Get danh sách sản phẩm từ view admin
        ResultModel ProductFromAdmin(ParamType paramType);
    }
}
