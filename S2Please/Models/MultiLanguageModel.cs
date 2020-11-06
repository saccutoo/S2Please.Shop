using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class MultiLanguageModel
    {
        public long ID { get; set; }
        public long LANGUAGE_ID { get; set; }
        public string KEY { get; set; }
        public string VALUE { get; set; }

    }
}