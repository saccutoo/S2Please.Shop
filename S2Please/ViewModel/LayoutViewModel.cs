using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SHOP.COMMON;
using Newtonsoft.Json;
namespace S2Please.ViewModel
{
    public static class LayoutViewModel
    {
        public static long UserId { get; set; } = CurrentUser.User.USER_ID;
        public static long ProductNew { get; set; } = TypeGroup.ProductNew;
        public static long Category { get; set; } = TypeGroup.Category;
        public static string ShopOnline { get; set; } = Constant.ShopOnline;

    }
}