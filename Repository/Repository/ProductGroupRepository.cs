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
    public partial class ProductGroupRepository : CommonRepository, IProductGroupRepository
    {
        //Get all product group
        public ResultModel GetAllProductGroup()
        {
            return ListProcedure<ProductGroupModel>(new ProductGroupModel(), "ProductGroup_Get_GetAllProductGroup", new List<Param>(), true);
        }
    }
}
