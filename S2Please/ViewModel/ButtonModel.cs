using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
namespace S2Please.ViewModel
{
    public class ButtonModel
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
        public string Title { get; set; } = string.Empty;

    }
}