using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
namespace S2Please.Areas.ADMIN.ViewModel
{
    public class LocalizadataViewModel
    {
        public List<TableMultipleLanguageConfigurationModel> MultipleLanguageConfigurations { get; set; } = new List<TableMultipleLanguageConfigurationModel>();
        public List<LanguageModel> Languages { get; set; } = new List<LanguageModel>();
        public List<LocalizationModel> Localizations { get; set; } = new List<LocalizationModel>();
        public long DATA_ID { get; set; }
        public string DATA_TYPE { get; set; }
        public bool Is_Save { get; set; }=true;

    }
}