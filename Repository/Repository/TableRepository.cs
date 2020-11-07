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
    public partial class TableRepository : CommonRepository, ITableRepository
    {
        //Get table user config
        public ResultModel GetTableUserConfig(string tableName,long userId)
        {
            var param = new List<Param>();
            param.Add(new Param() { Key = "@TABLE_NAME", Value = tableName });
            param.Add(new Param() { Key = "@USER_ID", Value = userId.ToString() });
            return ListProcedure<TableUserConfigModel>(new TableUserConfigModel(), "Table_Get_TableUserConfig", param);
        }

        //Get Table by table name
        public ResultModel GetTableByTableName(string tableName)
        {
            var param = new List<Param>();
            param = new List<Param>();
            param.Add(new Param() { Key = "@TABLE_NAME", Value = tableName });
            return ListProcedure<TableModel>(new TableModel(), "Table_Get_Table", param);
        }

        //Get Table column by table name
        public ResultModel GetTableColumnByTableName(string tableName)
        {
            var param = new List<Param>();
            param.Add(new Param() { Key = "@TABLE_NAME", Value = tableName });
            return ListProcedure<TableColumnModel>(new TableColumnModel(), "TableColumn_Get_TableColumnByTableName", param);
        }

        //Get SelectOption 
        public ResultModel GetSelectOption()
        {
            return ListProcedure<SelectOptionModel>(new SelectOptionModel(), "SelectOption_Get_SelectOption ", new List<Param>());
        }

        //Get update table user config 
        public ResultModel UpdateTableUserConfig(string tableName,long userId,string content,string parram)
        {
            var paramType = new List<Param>();
            paramType.Add(new Param() { Key = "@TABLE_NAME", Value = tableName });
            paramType.Add(new Param() { Key = "@USER_ID", Value = userId.ToString() });
            paramType.Add(new Param() { Key = "@TABLE_CONTENT", Value = content });
            paramType.Add(new Param() { Key = "@PARAM", Value = parram });
            return ListProcedure<ConditionFilterModel>(new ConditionFilterModel(), "Table_Update_TableUserConfig", paramType, false, false);
        }
    }
}
