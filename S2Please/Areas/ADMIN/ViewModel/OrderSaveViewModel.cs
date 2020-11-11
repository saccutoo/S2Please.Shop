using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Helper;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class OrderSaveViewModel : BaseModel
    {
        public CustomerModel Customer { get; set; } = new CustomerModel();
        public List<dynamic> Citys { get; set; } = new List<dynamic>();
    }

}
