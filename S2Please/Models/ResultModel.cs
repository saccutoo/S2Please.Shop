using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ResultModel
    {
        public List<dynamic> Results { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string CacheName { get; set; }
        public string error { get; set; } = string.Empty;
        public SqlCommand OutValue { get; set; } = new SqlCommand();
        public string Message { get; set; }
        public int Total { get; set; }
        public string Html { get; set; }
        public string Url { get; set; }
        public bool IsPermission { get; set; }=true;
        public void SetDataMessage(bool success,string message,string cacheName,string html)
        {
            Success = success;
            Message = message;
            CacheName = cacheName;
            Html = html;
        }
        public void SetUrl(string url)
        {
            IsPermission = false;
            Url = url;
        }
    }
}