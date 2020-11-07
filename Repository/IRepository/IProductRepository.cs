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

        //Get product by product id
        ResultModel GetProductImgByProductId(long id);

        //Get product color by product id
        ResultModel GetProductColorByProductId(long id);
        //Get product size by product id
        ResultModel GetProductSizeByProductId(long id);

        //Get product color size mapper by product id
        ResultModel GetProductColorSizeMapperByProductId(long id);

        //Delete product by id
        ResultModel DeleteProductByID(long id, long userId);

        //Get list product type
        ResultModel GetProductType();

        //Get list product group
        ResultModel GetProductGroup();

        //Get product by product id from admin
        ResultModel GetProductByIdFromAdmin(long id);

        //Get product mapper by product id
        ResultModel GetProductMapperProductId(long id);

        //SaveProduct
        ResultModel SaveProduct(ProductModel model, ParamType paramType, List<ColorType> colorTypes, List<SizeType> sizeTypes, List<ProductImgType> productImgs, List<ColorSizeMapperType> colorSizeMapType);
    }
}
