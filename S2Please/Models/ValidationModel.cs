using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S2Please.Models
{
    public class ValidationModel
    {
        public string ColumnName { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}