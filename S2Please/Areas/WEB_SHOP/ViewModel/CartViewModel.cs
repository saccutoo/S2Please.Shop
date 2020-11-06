using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Areas.WEB_SHOP.Models;

namespace S2Please.Areas.WEB_SHOP.ViewModel
{
    public class CartViewModel : BaseModel
    {
        public List<CartModel> Carts { get; set; } = new List<CartModel>();
    }
}