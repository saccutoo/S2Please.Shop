using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class FilterViewModel : BaseModel
    {

        public List<TableColumnModel> Columns = new List<TableColumnModel>();
        public string Type { get; set; }
        public string TableName { get; set; }
        public bool IsMoblie { get; set; }

        public List<ConditionFilterModel> Conditions { get; set; } = new List<ConditionFilterModel>();
        public List<ListColumnFilterModel> ColumnFilters { get; set; } = new List<ListColumnFilterModel>();
    }
}