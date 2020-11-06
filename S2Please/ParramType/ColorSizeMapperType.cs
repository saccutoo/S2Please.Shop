using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SHOP.COMMON;
namespace S2Please.ParramType
{
    public class ColorSizeMapperType
    {
        public long ID { get; set; }
        public string COLOR { get; set; }
        public string SIZE { get; set; }
        public long AMOUNT { get; set; }
        public float PRICE { get; set; }
        public long RATE { get; set; }
        public string IMG { get; set; }
        public bool IS_MAIN { get; set; }
        public long INDEX { get; set; }

    }
}