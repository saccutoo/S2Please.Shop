using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
namespace S2Please.ViewModel
{
    public class InputModel
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
        public string LabelName { get; set; } = string.Empty;
        public bool IsRequied { get; set; } = false;
        public string Readonly { get; set; } = string.Empty;
        public bool IsSetDate { get; set; } = false;
        public string OnBlur { get; set; } = string.Empty;
        public bool IsFuntionBlur { get; set; } = false;
        public string TypeInputMask { get; set; } = string.Empty;

    }
}