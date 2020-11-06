using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SHOP.COMMON;
namespace S2Please.ParramType
{
    public class ColorType
    {
        public long ID { get; set; }
        public string COLOR { get; set; }
        public long PRODUCT_ID { get; set; }
        public string IMG { get; set; }

    }
}