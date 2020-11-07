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
    public partial class SystemRepository : CommonRepository, ISystemRepository
    {
        //Get MultiLanguage
        public ResultModel GetMultiLanguage()
        {
            return ListProcedure<MultiLanguageModel>(new MultiLanguageModel(), "MultiLanguage_Get_MultiLanguage", new List<Param>(), true);
        }

        //Get ExecSqlString
        public ResultModel ExecSql(string sql)
        {
            return ExecSqlString(sql);
        }

        //Get Get_Condition_Filter
        public ResultModel GetConditionFilter()
        {
            return ListProcedure<ConditionFilterModel>(new ConditionFilterModel(), "ConditionFilter_Get_ConditionFilter", new List<Param>(),true);
        }

        //Get System_Get_ExcuteSqlString
        public ResultModel GetExcuteSqlString(string sql)
        {
            var paramType = new List<Param>();
            paramType.Add(new Param() { Key = "@SqlData", Value = sql });
            return ListProcedure<TableColumnModel>(new TableColumnModel(), "System_Get_ExcuteSqlString", paramType,true);
        }

        //Get import  adrress
        public ResultModel ImportCountry(List<ImportCountryType> datas,string isCity, string isDistrict, string isCommunity)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@IS_CITY", Value = isCity });
            param.Add(new Param { Key = "@IS_DISTRICT", Value = isDistrict });
            param.Add(new Param { Key = "@IS_COMMUNITY", Value = isCommunity });
            param.Add(new Param
            {
                IsUserDefinedTableType = true,
                paramUserDefinedTableType = new SqlParameter("@Country", SqlDbType.Structured)
                {
                    TypeName = "dbo.CountryType",
                    Value = DataTableHelper.ConvertToUserDefinedDataTable(datas)
                }
            });
            return ListProcedure<ImportCountryType>(new ImportCountryType(), "ImportCountry", param);
        }
    }
}
