using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class SearchViewModel
    {
        public List<SearchModel> Datas { get; set; } = new List<SearchModel>();
        public List<string> Groups { get; set; } = new List<string>() { "Product","Order"};
    }

}