using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Areas.WEB_SHOP.Models;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class CustomerViewModel : BaseModel
    {
        public CustomerModel Customer { get; set; } = new CustomerModel();
        public List<dynamic> Citys { get; set; } = new List<dynamic>();
        public List<dynamic> Districts { get; set; } = new List<dynamic>();
        public List<dynamic> Communitys { get; set; } = new List<dynamic>();
        public List<dynamic> Genders { get; set; } = new List<dynamic>();
        public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
        public List<StatusOrderModel> StatusOrders { get; set; } = new List<StatusOrderModel>();
        public List<StatusPayModel> StatusPays { get; set; } = new List<StatusPayModel>();

    }
}