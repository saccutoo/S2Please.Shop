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
    public partial interface ISystemRepository
    {
        //Get MultiLanguage
        ResultModel GetMultiLanguage();

        //Get ExecSqlString
        ResultModel ExecSql(string sql);

        //Get Get_Condition_Filter
        ResultModel GetConditionFilter();

        //Get System_Get_ExcuteSqlString
        ResultModel GetExcuteSqlString(string sql);

        //Get import  adrress
        ResultModel ImportCountry(List<ImportCountryType> datas, string isCity, string isDistrict, string isCommunity);

        //Get Get Method Pay
        ResultModel GetMethodPay();

        //Get Get Method Pay
        ResultModel GetShipFee();

        //Get City
        ResultModel GetCity();

        //Get District By Code City
        ResultModel GetDistrictByCodeCity(long cityCode);

        //GetCommunity By Code District
        ResultModel GetCommunityByCodeDistrict(long districtCode);
    }
}
