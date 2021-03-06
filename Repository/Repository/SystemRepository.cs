﻿using Repository.Model;
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

        //Get Get Method Pay
        public ResultModel GetMethodPay()
        {
            //danh sách phương thức thanh toán
           return ListProcedure<MethodPayModel>(new MethodPayModel(), "MethodPay_Get_MethodPay", new List<Param>(), true);
        }

        //Get Get Method Pay all
        public ResultModel GetMethodPayAll()
        {
            //danh sách phương thức thanh toán
            return ListProcedure<MethodPayModel>(new MethodPayModel(), "MethodPay_Get_MethodPayGetAll", new List<Param>(), true);
        }
 
        //Get ship fee
        public ResultModel GetShipFee()
        {
            //danh sách phương thức thanh toán
            return ListProcedure<ShipFeeModel>(new ShipFeeModel(), "ShipFee_Get_GetShipFee", new List<Param>(), true);
        }

        //Get all ship fee
        public ResultModel GetAllShipFee()
        {
            //danh sách phương thức thanh toán
            return ListProcedure<ShipFeeModel>(new ShipFeeModel(), "ShipFee_Get_GetAllShipFee", new List<Param>(), true);
        }
        //Get City
        public ResultModel GetCity()
        {
            return ListProcedure<CityModel>(new CityModel(), "City_Get_City", new List<Param>(), true);
        }

        //Get District By Code City
        public ResultModel GetDistrictByCodeCity(long cityCode)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@CODE_CITY", Value = cityCode.ToString() });
            return ListProcedure<DistrictModel>(new DistrictModel(), "District_Get_DistrictByCodeCity", param, true);
        }

        //GetCommunity By Code District
        public ResultModel GetCommunityByCodeDistrict(long districtCode)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@CODE_DISTRICT", Value = districtCode.ToString() });
            return ListProcedure<CommunityModel>(new CommunityModel(), "Community_Get_CommunityByCodeDistrict", param, true);
        }

        //Broken
        public ResultModel Broken()
        {
            return ListProcedure<MULTI_LANGUAGE>(new MULTI_LANGUAGE(), "Broken", new List<Param>(),true);
        }

        //Get Language
        public ResultModel GetLanguage()
        {
            return ListProcedure<LanguageModel>(new LanguageModel(), "Language_Get_Language", new List<Param>(),true);
        }

        //Get Gender
        public ResultModel GetGender()
        {
            return ListProcedure<GenderModel>(new GenderModel(), "Gender_Get_Gender", new List<Param>(), true);
        }

        //Get Status Order
        public ResultModel GetStatusOrder()
        {
            return ListProcedure<StatusOrderModel>(new StatusOrderModel(), "StatusOrder_Get_StatusOrder", new List<Param>(), true);
        }

        //Get Status pay
        public ResultModel GetStatusPay()
        {
            return ListProcedure<StatusPayModel>(new StatusPayModel(), "StatusPay_Get_StatusPay", new List<Param>(), true);
        }

        //Get table multiole language configuration
        public ResultModel GetTableMultipleLanguageConfigurationByTableName(string tableName)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@TABLE_NAME", Value = tableName });
            return ListProcedure<TableMultipleLanguageConfigurationModel>(new TableMultipleLanguageConfigurationModel(), "TableMultipleLanguageConfiguration_Get_TableMultipleLanguageConfigurationByTableName", param, true);
        }

        //Get Localization By DataId And Data Type
        public ResultModel GetLocalizationByDataIdAndDataType(long dataId,string dataType)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@DATA_ID", Value = dataId.ToString() });
            param.Add(new Param { Key = "@DATA_TYPE", Value = dataType });
            return ListProcedure<LocalizationModel>(new LocalizationModel(), "Localization_Get_LocalizationByDataIdAndDataType", param);
        }

        //Get Product Group Type
        public ResultModel GetProductGroupType()
        {
            return ListProcedure<BaseModel>(new BaseModel(), "ProductGroupType_Get_ProductGroupType", new List<Param>(), true);
        }

        //Get All role
        public ResultModel GetAllRole( )
        {
            return ListProcedure<BaseModel>(new BaseModel(), "Role_Get_GetAllRole", new List<Param>(), true);
        }

        //Get All role
        public ResultModel GetAllPermissionAction(long menuId)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@MENU_ID", Value = menuId.ToString() });
            return ListProcedure<PermissionActionModel>(new PermissionActionModel(), "Permission_Get_GetPermissionAction", param);
        }

        //Search common from admin
        public ResultModel SearchCommonFromAdmin(string filterString)
        {
            var param = new List<Param>();
            param.Add(new Param { Key = "@FILTER_STRING", Value = filterString.ToString() });
            return ListProcedure<SearchModel>(new SearchModel(), "Search_Get_SearchCommonFromAdmin", param);
        }

        //get silde
        public ResultModel GetSlide()
        {
            var param = new List<Param>();
            return ListProcedure<SlideModel>(new SlideModel(), "Silde_Get_Silde", param);
        }
    }
}
