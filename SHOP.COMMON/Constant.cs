using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHOP.COMMON
{
    public static class Constant
    {
        public static string APP_CURRENT_LANG = "App.Current.Lang";
        public static  string Email_Reg = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){1,4})+)$";
        public static  string ShopKeyAuthen = "Shop_Key_Authen";
        public static string ShopKeyAuthenAdmin = "Shop_Key_Authen_Admin";
        public static  string ShopOnline = "Shop_Online";
        public static string  AdminShopOnline = "Shop_Online_Admin";
        public static  string ClientDateFormat = "ClientDateFormat";
        public static string ViewProduct = "View_Product";
        public static string ShopKeyViewProduct = "Shop_Key_View_Product";

    }
    public static class Language
    {
        public static string VIETNAM = "1";
        public static string ENGLAND = "2";
    }
    public static class WriterLog
    {
        public static string FilePathJsonMultiLanguage = @"C:\Log\JsonMultiLanguage.txt";
    }
    public static class Url
    {
        public static string UrlLoadSlide = "/WEB_SHOP/Home/LoadSlide";
    }
    public static class TypeGroup
    {
        public static long ProductNew = 1;
        public static long Category = 2;
    }

    public static class TableName
    {
        public static string Product = "Product";
        public static string Order = "Order";
        public static string ProductOrder = "Product-Order";

    }

    public static class MenuName
    {
        public static string Product = "Product";
        public static string Order = "Order";
        public static string ProductOrder = "Product-Order";

    }

    public static class TableUrl
    {
        public static string Product = "/ADMIN/Product/ReloadTable";
        public static string Order = "/ADMIN/Order/ReloadTable";
        public static string ProductOrder = "/ADMIN/Order/ReloadTable";

    }
    public static class TableExportUrl
    {
        public static string Product = "/ADMIN/Product/Export";
        public static string Order = "/ADMIN/Order/Export";
        public static string ProductOrder = "/ADMIN/Order/Export";
    }
    public static class TableSesionExportUrl
    {
        public static string Product = "/ADMIN/Product/SesionExport";
        public static string Order = "/ADMIN/Order/SesionExport";
        public static string ProductOrder = "/ADMIN/Order/SesionExport";
    }
    public static class Permission
    {
        public static string Update = "Update";
        public static string Get = "Get";
        public static string Delete = "Delete";
        public static string Export = "Export";
        public static string Import = "Import";
    }

    public static class StatusOrder
    {
        public static long Pending = 1;//Chờ duyệt
        public static long NoApproval = 2;//Không duyệt
        public static long Sending = 3;//Đang giao hàng
        public static long Received = 4;//Đã nhận hàng
        public static long Cancelled = 5;//Đã hủy
    }

    public static class StatusPay
    {
        public static long IsPay =1;//Đã thanh toán
        public static long NoPay = 2;//chưa thanh toán
    }

    public static class DataType
    {
        public static string CL_STATUS_ORDER = "CL_STATUS_ORDER";//
        public static string CL_STATUS_PAY = "CL_STATUS_PAY";//
    }
}
