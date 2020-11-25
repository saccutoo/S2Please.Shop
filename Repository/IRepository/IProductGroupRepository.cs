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
    public partial interface IProductGroupRepository
    {
        //Get all product group
        ResultModel GetAllProductGroup();

        //Get product type
        ResultModel GetProductGroupGromAdmin(ParamType paramType, bool isCheckPermission = true);

        //Save product group
        ResultModel SaveProductGroup(ProductGroupModel model, List<LocalizationType> type, bool isCheckPermission = true);

        //Save product group by id
        ResultModel GetProductGroupById(long Id);

        //delete product group by id
        ResultModel DeleteById(long id, bool isCheckPermission = true);
    }
}
