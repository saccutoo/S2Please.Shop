using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Areas.WEB_SHOP.Models;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class UpdateOrderViewModel
    {
        public List<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
        public OrderModel Order { get; set; } = new OrderModel();
        public List<dynamic> Citys { get; set; } = new List<dynamic>();
        public List<dynamic> Districts { get; set; } = new List<dynamic>();
        public List<dynamic> Communitys { get; set; } = new List<dynamic>();
    }
}