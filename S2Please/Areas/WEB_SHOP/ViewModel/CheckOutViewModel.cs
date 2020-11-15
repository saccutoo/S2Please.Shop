using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class CheckOutViewModel : BaseModel
    {
        public List<CartModel> Carts { get; set; } = new List<CartModel>();
        public List<ShipFeeModel> ShipFees { get; set; } = new List<ShipFeeModel>();
        public List<MethodPayModel> MethodPays { get; set; } = new List<MethodPayModel>();
        public CustomerModel Customer { get; set; } = new CustomerModel();
        public List<dynamic> Citys { get; set; } = new List<dynamic>();
        public List<dynamic> Districts { get; set; } = new List<dynamic>();
        public List<dynamic> Communitys { get; set; } = new List<dynamic>();

        public long UserId { get; set; }

    }
}