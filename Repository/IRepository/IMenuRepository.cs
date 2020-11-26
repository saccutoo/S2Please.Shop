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
    public partial interface IMenuRepository
    {
        //Get all menu
        ResultModel GetMenu();

        //Get menu render to table
        ResultModel GetMenuFromAdmin(ParamType paramType, bool isCheckPermission = true);

        //Save product group
        ResultModel SaveMenu(MenuModel model, List<LocalizationType> type, bool isCheckPermission = true);

        //Get menu by ID
        ResultModel GetMenuById(long id);

        //Delete menu by Id
        ResultModel Delete(long id);
    }
}
