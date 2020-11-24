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
    public partial interface IProductTypeRepository
    {
        //Get product type
        ResultModel GetProductType(ParamType paramType, bool isCheckPermission = true);

        //Save product type
        ResultModel SaveProductType(ProductTypeModel model, List<LocalizationType> type, bool isCheckPermission = true);

        //Get Product Type By ID
        ResultModel GetProductTypeByID(long id);

        //delete product type by id
        ResultModel DeleteById(long id, long userId, bool isCheckPermission = true);
    }
}
