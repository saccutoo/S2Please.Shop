using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SHOP.COMMON;
namespace S2Please.ParramType
{
    public class OrderDetailType
    {
        public long ID { get; set; }
        public long ORDER_ID { get; set; }
        public long PRODUCT_ID { get; set; }
        public long COLOR_ID { get; set; }
        public long SIZE_ID { get; set; }
        public long AMOUNT { get; set; }
        public string DECRIPTION { get; set; } = string.Empty;

    }
}