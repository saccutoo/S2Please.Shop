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
    public partial interface ITableRepository
    {
        //Get table user config
        ResultModel GetTableUserConfig(string tableName, long userId);

        //Get Table by table name
        ResultModel GetTableByTableName(string tableName);

        //Get Table column by table name
        ResultModel GetTableColumnByTableName(string tableName);

        //Get SelectOption 
        ResultModel GetSelectOption();
    }
}
