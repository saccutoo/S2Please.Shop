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

        //Get product type
        public ResultModel GetProductGroupGromAdmin(ParamType paramType, bool isCheckPermission = true)
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
            return ListProcedure<ProductGroupModel>(new ProductGroupModel(), "ProductGroup_Get_ProductGroupFromAdmin", param, false, isCheckPermission);
        }

        //Save product group
        public ResultModel SaveProductGroup(ProductGroupModel model, List<LocalizationType> type, bool isCheckPermission = true)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@GROUP_NAME", Value = string.IsNullOrEmpty(model.GROUP_NAME) ? " " : model.GROUP_NAME });
            param.Add(new Param { Key = "@IS_SHOW", Value = model.IS_SHOW.ToString() });
            param.Add(new Param { Key = "@ORDER", Value = model.ORDER.ToString() });
            param.Add(new Param { Key = "@TYPE", Value = model.TYPE.ToString() });
            param.Add(new Param { Key = "@CREATED_BY", Value = model.CREATED_BY.ToString() });
            param.Add(new Param { Key = "@UPDATED_BY", Value = model.UPDATED_BY.ToString() });

            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@LocalizationType", SqlDbType.Structured)
                {
                    TypeName = "dbo.LocalizationType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(type)
                }
            });

            return ListProcedure<ProductGroupModel>(new ProductGroupModel(), "ProductGroup_Update_SaveProductGroup", param, false, isCheckPermission);
        }


        //Save product group by id
        public ResultModel GetProductGroupById(long Id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = Id.ToString() });
            return ListProcedure<ProductGroupModel>(new ProductGroupModel(), "ProductGroup_Get_ProductGroupById", param);
        }

        //delete product group by id
        public ResultModel DeleteById(long id, bool isCheckPermission = true)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductGroupModel>(new ProductGroupModel(), "ProductGroup_Delete_DeleteById", param, false, isCheckPermission);
        }
    }
}
