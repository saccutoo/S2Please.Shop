﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;
using S2Please.Helper;
using S2Please.Areas.ADMIN.Models;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class OrderSaveViewModel : BaseModel
    {
        public CustomerModel Customer { get; set; } = new CustomerModel();
        public List<dynamic> Citys { get; set; } = new List<dynamic>();
        public List<dynamic> Districts { get; set; } = new List<dynamic>();
        public List<dynamic> Communitys { get; set; } = new List<dynamic>();
        public TableViewModel Table { get; set; } = new TableViewModel();
        public ProductModel Product { get; set; } = new ProductModel();
        public List<ProductImgModel> ProductImgs { get; set; } = new List<ProductImgModel>();
        public List<ProductSizeModel> ProductSizes { get; set; } = new List<ProductSizeModel>();
        public List<ProductColorModel> ProductColors { get; set; } = new List<ProductColorModel>();
        public ProductColorSizeMapperModel ProductMapper { get; set; } = new ProductColorSizeMapperModel();
        public List<CartModel> Carts { get; set; } = new List<CartModel>();
        public List<dynamic> StatusOrders { get; set; }=new List<dynamic>();
        public List<dynamic> StatusPays{ get; set; } = new List<dynamic>();
        public List<dynamic> MethodPays { get; set; } = new List<dynamic>();
        public List<dynamic> ShipFees { get; set; } = new List<dynamic>();
        public OrderModel Order { get; set; } = new OrderModel();
        public List<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
    }

}
