using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHOP.COMMON
{
    public static class CurrentMultiLanguage
    {
        public static List<MultiLanguage> multiLanguages { get; set; } = new List<MultiLanguage>();
    }
    public class MultiLanguage
    {
        public long ID { get; set; }
        public long LANGUAGE_ID { get; set; }
        public string VALUE { get; set; }
        public string KEY { get; set; }
    }
}
