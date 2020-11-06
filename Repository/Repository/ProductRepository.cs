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
    public partial class ProductRepository : CommonRepository, IProductRepository
    {

        public ResultModel GetProductById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return  ListProcedure<ProductModel>(new ProductModel(), "Product_Get_GetProductById", param);
        }

        public ResultModel ProductFromAdmin(ParamType paramType)
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
            return ListProcedure<ProductModel>(new ProductModel(), "Product_Get_ProductFromAdmin", param, false, true);
        }
    }
}
