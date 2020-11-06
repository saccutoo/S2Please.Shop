using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
namespace S2Please.ViewModel
{
    public class SelectModel
    {
        public string Style { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Attribute { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string OnChange { get; set; } = string.Empty;
        public string OnClick { get; set; } = string.Empty;
        public string OnKeyUp { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;
        public List<dynamic> Datas { get; set; } = new List<dynamic>();
        public string FieldId { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public bool IsSearchAjax { get; set; } = false;
        public string Url { get; set; } = string.Empty;
        public string LabelName { get; set; } = string.Empty;
        public bool IsRequied { get; set; } = false;
        public bool IsMultiLanguage { get; set; } = false;
        public string DataType { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public bool IsShowTitle { get; set; } = false;
        public string DropdownParent { get; set; }

    }
}