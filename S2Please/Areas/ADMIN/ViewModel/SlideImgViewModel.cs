using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S2Please.Models;
using S2Please.Database;

namespace S2Please.Areas.ADMIN.ViewModel
{
    public class SlideImgViewModel
    {
        public List<FileModel> Files { get; set; } = new List<FileModel>();
    }

}