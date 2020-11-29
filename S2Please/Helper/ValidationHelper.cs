using S2Please.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Controllers;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Repository;
using SHOP.COMMON.Helpers;

namespace S2Please.Helper
{
    public static class ValidationHelper
    {
        public static CommonRepository _commonRepository = new CommonRepository();
        public static List<ValidationModel> ListValidation<T>(List<T> models, string modelName)
        {
            var result = new List<ValidationModel>();
            List<ColumnValidationModel> validationsR = new List<ColumnValidationModel>();
            if (models != null && models.Count > 0)
            {
                var param = new List<Param>();
                param.Add(new Param { Key = "@TABLE_NAME", Value = models[0].GetType().Name.Replace("Model", string.Empty) });
                var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);
                var validationResponse = _commonRepository.ListProcedure<ColumnValidationModel>(new ColumnValidationModel(), "Validation_Get_GetValidation", paramType,false,true);
                var validationResult = JsonConvert.DeserializeObject<List<ColumnValidationModel>>(JsonConvert.SerializeObject(validationResponse.Results));

                if (validationResult != null && validationResult.Count() > 0)
                {
                    validationsR = validationResult;
                }

                var index = -1;
                if (models.Any())
                {
                    foreach (var model in models)
                    {
                        index++;
                        result.AddRange(Validation(model, modelName, index, validationsR));
                    }
                }
            }
            return result;
        }


        public static List<ValidationModel> Validation<T>(T model, string modelName, int index = -1, List<ColumnValidationModel> validationsR = null, string dbName = "")
        {
            var result = new List<ValidationModel>();
            var nameOfModel = modelName;
            if (validationsR == null)
            {
                var param = new List<Param>();              
                param.Add(new Param { Key = "@TABLE_NAME", Value = model.GetType().Name.Replace("Model", string.Empty) });
                var paramType = MapperHelper.MapList<Param, Repository.Model.Param>(param);

                var validationResponse = _commonRepository.ListProcedure<ColumnValidationModel>(new ColumnValidationModel(), "Validation_Get_GetValidation", paramType,false,true);
                var validationResult = JsonConvert.DeserializeObject<List<ColumnValidationModel>>(JsonConvert.SerializeObject(validationResponse.Results));

                if (validationResult!=null && validationResult.Count()>0)
                {
                    validationsR = validationResult;
                }
               
            }

            #region column validation

            var properties = model.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var columnName = prop.Name;
                var columnValue = string.Empty;
                if (prop.GetValue(model) != null)
                    columnValue = prop.GetValue(model).ToString();
                var validations = validationsR.Where(x => x.COLUMN_NAME == columnName).ToList();
                if (validations.Any())
                {
                    foreach (var validation in validations)
                    {
                        if (validation != null)
                        {
                            if (validation.IS_REQUIRED)
                            {
                                if (string.IsNullOrEmpty(columnValue) || ((prop.PropertyType.Name.ToLower() == "decimal"
                                                                            || prop.PropertyType.Name.ToLower() == "float"
                                                                            || prop.PropertyType.Name.ToLower() == "double"
                                                                            || prop.PropertyType.Name.ToLower() == "int16"
                                                                            || prop.PropertyType.Name.ToLower() == "int32"
                                                                            || prop.PropertyType.Name.ToLower() == "int64"
                                                                            || prop.PropertyType.Name.ToLower() == "single"
                                                                           ) && columnValue == "0"))
                                {
                                    result.Add(new ValidationModel
                                    {
                                        ColumnName = nameOfModel + (index != -1 ? "[" + index.ToString() + "]." : nameOfModel == "" ? "" : ".") + columnName,
                                        IsError = false,
                                        ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Required")
                                    });
                                    goto Finish;
                                }
                                else
                                {
                                    goto MinLength;
                                }
                            }
                            MinLength:
                            if (validation.MIN_LENGTH != 0)
                            {
                                if (prop.PropertyType.Name.ToLower() == "decimal"
                                                                            || prop.PropertyType.Name.ToLower() == "float"
                                                                            || prop.PropertyType.Name.ToLower() == "double"
                                                                            || prop.PropertyType.Name.ToLower() == "single"
                                                                           )
                                {
                                    columnValue = Convert.ToInt64(Math.Floor(Convert.ToDouble(columnValue))).ToString();
                                }
                                if (columnName != null && columnValue.Length < validation.MIN_LENGTH)
                                {
                                    result.Add(new ValidationModel
                                    {
                                        ColumnName = nameOfModel + (index != -1 ? "[" + index.ToString() + "]." : nameOfModel == "" ? "" : ".") + columnName,
                                        IsError = false,
                                        ErrorMessage = string.Format(FunctionHelpers.GetValueLanguage("Error.MinLength"), validation.MIN_LENGTH)
                                    });
                                    goto Finish;
                                }
                                else
                                {
                                    goto MaxLength;
                                }
                            }
                            MaxLength:
                            if (validation.MAX_LENGTH != 0)
                            {
                                if (prop.PropertyType.Name.ToLower() == "decimal"
                                                                            || prop.PropertyType.Name.ToLower() == "float"
                                                                            || prop.PropertyType.Name.ToLower() == "double"
                                                                            || prop.PropertyType.Name.ToLower() == "single"
                                                                           )
                                {
                                    columnValue = Convert.ToInt64(Math.Floor(Convert.ToDouble(columnValue))).ToString();
                                }
                                if (columnName != null && columnValue.Length > validation.MAX_LENGTH)
                                {
                                    result.Add(new ValidationModel
                                    {
                                        ColumnName = nameOfModel + (index != -1 ? "[" + index.ToString() + "]." : nameOfModel == "" ? "" : ".") + columnName,
                                        IsError = false,
                                        ErrorMessage = string.Format(FunctionHelpers.GetValueLanguage("Error.MaxLength"), "{0}", validation.MAX_LENGTH)
                                    });
                                    goto Finish;
                                }
                                else
                                {
                                    goto Email;
                                }
                            }
                            Email:
                            if (validation.IS_EMAIL)
                            {
                                var emailReg = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){1,4})+)$";
                                if (!Regex.IsMatch(columnValue, emailReg))
                                {
                                    result.Add(new ValidationModel
                                    {
                                        ColumnName = nameOfModel + (index != -1 ? "[" + index.ToString() + "]." : nameOfModel == "" ? "" : ".") + columnName,
                                        IsError = false,
                                        ErrorMessage = FunctionHelpers.GetValueLanguage("Error.EmailFormat")
                                    });
                                    goto Finish;
                                }
                                else
                                {
                                    goto Phone;
                                }

                            }
                            Phone:
                            if (validation.IS_PHONE)
                            {
                                var phoneReg = @"^\d{9,15}$";
                                if (!Regex.IsMatch(columnValue, phoneReg))
                                {
                                    result.Add(new ValidationModel
                                    {
                                        ColumnName = nameOfModel + (index != -1 ? "[" + index.ToString() + "]." : nameOfModel == "" ? "" : ".") + columnName,
                                        IsError = false,
                                        ErrorMessage = FunctionHelpers.GetValueLanguage("Error.PhoneFormat")
                                    });
                                    goto Finish;
                                }
                                else
                                {
                                    goto Number;
                                }
                            }
                            Number:
                            if (validation.IS_NUMBER)
                            {
                                var numberReg = @"^[0-9]*$";
                                if (!Regex.IsMatch(columnValue, numberReg))
                                {
                                    result.Add(new ValidationModel
                                    {
                                        ColumnName = nameOfModel + (index != -1 ? "[" + index.ToString() + "]." : nameOfModel == "" ? "" : ".") + columnName,
                                        IsError = false,
                                        ErrorMessage = FunctionHelpers.GetValueLanguage("Error.Number")
                                    });
                                    goto Finish;
                                }
                                else
                                {
                                    goto Finish;
                                }
                            }
                            Finish:
                            continue;
                        }
                    }
                }
                #endregion
            }
            return result;
        }
    }
}