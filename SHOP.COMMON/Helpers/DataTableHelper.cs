using System;
using System.Collections.Generic;
using System.Data;

namespace SHOP.COMMON.Helpers
{
    public static class DataTableHelper
    {
        public static DataTable ConvertToUserDefinedDataTable<T>(this IEnumerable<T> values) where T : class
        {
            var table = new DataTable();

            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            if (values != null)
            {
                foreach (var value in values)
                {
                    if (value != null)
                    {
                        var newRow = table.NewRow();
                        foreach (var prop in properties)
                        {
                            if (table.Columns.Contains(prop.Name))
                                newRow[prop.Name] = prop.GetValue(value, null) ?? DBNull.Value;
                        }
                        table.Rows.Add(newRow);
                    }
                }
            }
            return table;
        }
    }
}