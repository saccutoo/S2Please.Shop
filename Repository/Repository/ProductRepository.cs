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
    public partial class ProductRepository : CommonRepository, IProductRepository
    {
        //Get sản phẩm theo mã
        public ResultModel GetProductById(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return  ListProcedure<ProductModel>(new ProductModel(), "Product_Get_GetProductById", param);
        }

        //Get danh sách sản phẩm từ view admin
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

        //Get product by product id
        public ResultModel GetProductImgByProductId(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductImgModel>(new ProductImgModel(), "Product_Get_GetProductImgByProductId", param);
        }

        //Get product color by product id
        public ResultModel GetProductColorByProductId(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductColorModel>(new ProductColorModel(), "Product_Get_GetProductColorByProductId", param);
        }

        //Get product size by product id
        public ResultModel GetProductSizeByProductId(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductSizeModel>(new ProductSizeModel(), "Product_Get_GetProductSizeByProductId", param);
        }

        //Get product color size mapper by product id
        public ResultModel GetProductColorSizeMapperByProductId(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductColorSizeMapperModel>(new ProductColorSizeMapperModel(), "Product_Get_GetProductByProductID", param);
        }

        //Delete product by id
        public ResultModel DeleteProductByID(long id,long userId)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            param.Add(new Param { Key = "@UPDATED_BY", Value = userId.ToString() });
            return ListProcedure<ProductModel>(new ProductModel(), "Product_Delete_DeleteProductByID", param, false, true);
        }

        //Get list product type
        public ResultModel GetProductType()
        {
            return ListProcedure<ProductTypeModel>(new ProductTypeModel(), "ProductType_Get_ProductType", new List<Param>(),true);
        }

        //Get list product group
        public ResultModel GetProductGroup()
        {
            return ListProcedure<ProductGroupModel>(new ProductGroupModel(), "ProductGroup_Get_ProductGroup", new List<Param>(),true);
        }

        //Get product by product id from admin
        public ResultModel GetProductByIdFromAdmin(long id)
        {
            var param = new List<Param>();
            param.Add(new Param() { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductModel>(new ProductModel(), "Product_Get_GetProductByIdFromAdmin", param, false, true);
        }

        //Get product mapper by product id
        public ResultModel GetProductMapperProductId(long id)
        {
            var param = new List<Param>();
            param.Add(new Param() { Key = "@PRODUCT_ID", Value = id.ToString() });
            return ListProcedure<ProductColorSizeMapperModel>(new ProductColorSizeMapperModel(), "Product_Get_GetProductMapperProductId", param);
        }

        //SaveProduct
        public ResultModel SaveProduct(ProductModel model, ParamType paramType,List<ColorType> colorTypes,List<SizeType> sizeTypes,List<ProductImgType> productImgs,List<ColorSizeMapperType> colorSizeMapType)
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

            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@ColorType", SqlDbType.Structured)
                {
                    TypeName = "dbo.ColorType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(colorTypes)
                }
            });

            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@SizeType", SqlDbType.Structured)
                {
                    TypeName = "dbo.SizeType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(sizeTypes)
                }
            });

            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@ProductImgType", SqlDbType.Structured)
                {
                    TypeName = "dbo.ProductImgType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(productImgs)
                }
            });

            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@ColorSizeMap", SqlDbType.Structured)
                {
                    TypeName = "dbo.ColorSizeMapperType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(colorSizeMapType)
                }
            });

            param.Add(new Param { Key = "@ID", Value = model.ID.ToString() });
            param.Add(new Param { Key = "@PRODUCT_CODE", Value = model.PRODUCT_CODE });
            param.Add(new Param { Key = "@PRODUCT_TYPE_ID", Value = model.PRODUCT_TYPE_ID.ToString() });
            param.Add(new Param { Key = "@PRODUCT_GROUP_ID", Value = model.PRODUCT_GROUP_ID.ToString() });
            param.Add(new Param { Key = "@NAME", Value = model.NAME });
            param.Add(new Param { Key = "@DECRIPTION", Value = model.DECRIPTION == null ? " " : model.DECRIPTION });
            param.Add(new Param { Key = "@PRODUCT_MATERIAL", Value = model.PRODUCT_MATERIAL == null ? " " : model.PRODUCT_MATERIAL });
            param.Add(new Param { Key = "@PRODUCT_ORIGIN", Value = model.PRODUCT_ORIGIN == null ? " " : model.PRODUCT_ORIGIN });
            param.Add(new Param { Key = "@IS_SHOW", Value = model.IS_SHOW.ToString() });
            param.Add(new Param { Key = "@IS_NEW", Value = model.IS_NEW.ToString() });
            return ListProcedure<ProductModel>(new ProductModel(), "Product_Update_SaveProduct", param, false, true);
        }

        //Get Product Color Size Mapper By ColorId AndSize Id
        public ResultModel GetProductColorSizeMapperByColorIdAndSizeId(long colorId,long sizeId,long productId,string isMain)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@COLOR_ID", Value = colorId.ToString() });
            param.Add(new Param { Key = "@SIZE_ID", Value = sizeId.ToString() });
            param.Add(new Param { Key = "@PRODUCT_ID", Value = productId.ToString() });
            param.Add(new Param { Key = "@IS_MAIN", Value = isMain });
            return ListProcedure<ProductColorSizeMapperModel>(new ProductColorSizeMapperModel(), "Product_Get_GetProductColorSizeMapperByColorIdAndSizeId", param);
        }

        //Get Product Bonus By Product Id
        public ResultModel GetProductBonusByProductId(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductBonusModel>(new ProductBonusModel(), "Product_Get_GetProductBonusByProductId", param);
        }

        //Get danh sách sản phẩm từ view web
        public ResultModel ProductFromWeb(List<ParamType> basicParamType)
        {
            var param = new List<Param>();
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamType)
                }
            });
            return ListProcedure<ProductModel>(new ProductModel(), "Product_Get_Product", param);
        }

        //Get danh sách sản phẩm từ view web by group Id
        public ResultModel GetProductByGroupId(List<ParamType> basicParamType)
        {
            var param = new List<Param>();
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamType)
                }
            });

            return ListProcedure<ProductModel>(new ProductModel(), "Product_Get_ProductByGroupId", param);
        }

        //Get product bu proceder auto
        public ResultModel GetProductbyStoredProcedureName(List<ParamType> basicParamType,string stroedPRocedure)
        {
            var param = new List<Param>();
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamType)
                }
            });

            return ListProcedure<ProductModel>(new ProductModel(), stroedPRocedure, param);
        }

        //Get Product By Type Id
        public ResultModel GetProductByTypeId(List<ParamType> basicParamType)
        {
            var param = new List<Param>();
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamType)
                }
            });

            return ListProcedure<ProductModel>(new ProductModel(), "Product_Get_ProductByTypeId", param);
        }

        //Get Product random
        public ResultModel GetProductRanDom(List<ParamType> basicParamType)
        {
            var param = new List<Param>();
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamType)
                }
            });

            return ListProcedure<ProductModel>(new ProductModel(), "Product_Get_ProductRanDom", param);
        }

        //Get Product Type By ID
        public ResultModel GetProductTypeByID(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductTypeModel>(new ProductTypeModel(), "ProductType_Get_ProductTypeByID", param);
        }


        //Get Products
        public ResultModel GetProducts(List<ParamType> basicParamType)
        {
            var param = new List<Param>();
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamType)
                }
            });
            param.Add(new Param { Key = "@TotalRecord", Value = "0", IsOutPut = true, Type = "Int" });
            return ListProcedure<ProductModel>(new ProductModel(), "Product_Get_Products", param);
        }

        //Get Product Type By GroupID
        public ResultModel GetProductTypeByGroupID(long id)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@ID", Value = id.ToString() });
            return ListProcedure<ProductTypeModel>(new ProductTypeModel(), "ProductType_Get_ProductTypeByGroupID", param);
        }

        //Get Search Product
        public ResultModel SearchProduct(List<ParamType> basicParamType)
        {
            var param = new List<Param>();
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@BasicParamType", SqlDbType.Structured)
                {
                    TypeName = "dbo.BasicParamType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(basicParamType)
                }
            });
            //Lấy danh sách sản phẩm
            return ListProcedure<ProductModel>(new ProductModel(), "Product_Get_SearchProduct", param);
        }
    }
}
