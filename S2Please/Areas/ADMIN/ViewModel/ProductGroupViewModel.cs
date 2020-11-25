using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class ProductGroupViewModel : BaseModel
    {      
        public TableViewModel Table { get; set; } = new TableViewModel();

    }
}