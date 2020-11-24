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
    public partial class ProductTypeRepository : CommonRepository, IProductTypeRepository
    {
        //Get product type
        public ResultModel GetProductType(ParamType paramType, bool isCheckPermission = true)
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
            return ListProcedure<ProductTypeModel>(new ProductTypeModel(), "ProductType_Get_ProductTypeFromAdmin", param, false, isCheckPermission);
        }

        //Save product type
        public ResultModel SaveProductType(ProductTypeModel model,List<LocalizationType> type, bool isCheckPermission = true)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@PRODUCT_GROUP_ID", Value = model.PRODUCT_GROUP_ID.ToString() });
            param.Add(new Param { Key = "@NAME", Value =string.IsNullOrEmpty(model.NAME) ? " " : model.NAME });
            param.Add(new Param { Key = "@IS_SHOW_VIEW", Value = model.IS_SHOW_VIEW.ToString() });
            param.Add(new Param { Key = "@MENU_ID", Value = model.MENU_ID.ToString() });
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

            return ListProcedure<ProductTypeModel>(new ProductTypeModel(), "ProductType_Update_SaveProductType", param, false, isCheckPermission);
        }

        //Get Product Type By ID
        public ResultModel GetProductTypeByID(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductTypeModel>(new ProductTypeModel(), "ProductType_Get_ProductTypeByID1", param);
        }

        //delete product type by id
        public ResultModel DeleteById(long id,long userId, bool isCheckPermission = true)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            param.Add(new Param { Key = "@UPDATED_BY", Value = userId.ToString() });
            return ListProcedure<ProductTypeModel>(new ProductTypeModel(), "ProductType_Delete_DeleteById", param,false, isCheckPermission);
        }
    }
}
