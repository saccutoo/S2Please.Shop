using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class LanguageViewModel
    {
        public List<LanguageModel> languages { get; set; } = new List<LanguageModel>();
        public string languageId { get; set; }
    }
}