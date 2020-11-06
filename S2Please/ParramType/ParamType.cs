using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SHOP.COMMON;
namespace S2Please.ParramType
{
    public class ParamType
    {
        public string STRING_FILTER { get; set; } = string.Empty;
        public string ORDER_BY { get; set; }
        public long USER_ID { get; set; } = CurrentUser.User.USER_ID;
        public long ROLE_ID { get; set; }
        public long PAGE_NUMBER { get; set; }
        public long PAGE_SIZE{ get; set; }
        public long LANGUAGE_ID { get; set; } = CurrentUser.LANGUAGE_ID;
        public string VALUE { get; set; }

    }
}