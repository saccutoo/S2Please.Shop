using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Areas.WEB_SHOP.Models;


namespace S2Please.Models
{
    public class TableModel:BaseModel
    {
        public string TABLE_NAME { get; set; }
        public string TABLE_CONTENT { get; set; }

    }
    public class TableViewModel
    {
        //setting from dbo
        public List<Action> BULK_ACTION { get; set; }

        public bool IS_SHOW_FILTER { get; set; }
        public bool IS_SHOW_SORT { get; set; }
        public long PAGE_SIZE { get; set; }

        public List<TableColumnField> TABLE_COLUMN_FIELD { get; set; }

        //End setting from dbo
        //setting code
        public string TABLE_NAME { get; set; }
        public long PAGE_INDEX { get; set; }
        public  List<SelectOptionModel> SELECT_OPTION { get;set; }
        public string TABLE_URL { get; set; }
        public string TITLE_TABLE_NAME { get; set; }
        public string MENU_NAME{ get; set; } //Check quyền với menu name
        public long TOTAL { get; set; }
        public List<TableColumnModel> TABLE_COLUMN { get; set; }

        public List<dynamic> DATA { get; set; }

        public bool IS_PERMISSION { get; set; } = true;

        public bool IS_SHOW_FOOTER { get; set; } = true;

        public string VALUE_DYNAMIC { get; set; } = string.Empty;

        public string STRING_FILTER { get; set; }

        public string TABLE_EXPORT_URL { get; set; }
        public string TABLE_SESION_EXPORT_URL { get; set; }


        //end setting code
    }

    public class TableColumnField
    {
        public string COLUMN_NAME { get; set; }
        public string ACTION_LINK { get; set; }
        public string ON_CLICK { get; set; }
        public string FUNTION_NAME { get; set; }
        public string CSS { get; set; }
        public string CLASS { get; set; }
        public string WIDTH { get; set; }
        public string PRESENTATION { get; set; }
        public string DATA_TYPE { get; set; }
        public string VALUE_FIELD { get; set; }

        public List<PresentationData> PRESENTATION_DATA { get; set; }
        public int POSITION { get; set; }
        public bool IS_SHOW { get; set; }
        public List<Action> ACTION { get; set; }
        public bool IS_CHECK { get; set; }

    }

    public class Action
    {
        public string BUTTON_NAME { get; set; }
        public string CLASS { get; set; }
        public string ACTION_NAME { get; set; }
        public List<Action> DROPDOWN { get; set; }
        public string FUNTION_NAME { get; set; }
    }

    public class PresentationData
    {
        public string VALUE_FIELD { get; set; }
        public string DATA_TYPE { get; set; }
        public string PROPERTY_NAME { get; set; }
        public bool IS_MUITL_LANGUAGE { get; set; }
    }
}