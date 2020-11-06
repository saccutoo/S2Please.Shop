using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SHOP.COMMON;
namespace S2Please.ParramType
{
    public class SizeType
    {
        public long ID { get; set; }
        public string SIZE_NAME { get; set; }
        public long PRODUCT_ID { get; set; }
    }
}