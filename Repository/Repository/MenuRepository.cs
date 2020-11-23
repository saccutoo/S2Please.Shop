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
    public partial class MenuRepository : CommonRepository, IMenuRepository
    {
        //Get all menu
        public ResultModel GetMenu()
        {
            return ListProcedure<MenuModel>(new MenuModel(), "Menu_Get_Menu", new List<Param>(), true);
        }
    }
}
