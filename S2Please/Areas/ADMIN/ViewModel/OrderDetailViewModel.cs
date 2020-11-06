using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class OrderDetailViewModel : BaseModel
    {      
        public List<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
        public OrderModel Order { get; set; } = new OrderModel();
    }
}