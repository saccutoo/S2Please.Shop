using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Areas.WEB_SHOP.Models;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class OrderDetailViewModel 
    {
        public List<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
        public OrderModel Order { get; set; } = new OrderModel();
    }
}