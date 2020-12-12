using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S2Please.Models;
using Newtonsoft.Json;
using SHOP.COMMON;
using S2Please.Controllers;
using System;
using S2Please.Areas.ADMIN.ViewModel;
using System.Data;
using S2Please.ViewModel;
using Repository;

namespace S2Please.Areas.ADMIN.Controllers
{
    public class SystemController : BaseController
    {
        // GET: ADMIN/Exec
        private ISystemRepository _systemRepository;
        private ITableRepository _tableRepository;
        public SystemController(ISystemRepository systemRepository, ITableRepository tableRepository)
        {
            this._systemRepository = systemRepository;
            this._tableRepository = tableRepository;
        }

        public ActionResult ReloadDistrict(long codeCity)
        {
            SelectModel vm = new SelectModel();
            var response = _systemRepository.GetDistrictByCodeCity(codeCity);
            if (response != null && response.Results.Count() > 0)
            {
                vm.Datas = response.Results;
            }
            vm.Id = "DISTRICT";
            vm.Name = "model.DISTRICT";
            vm.Class = "";
            vm.LabelName = "Cart.District";
            vm.IsRequied = true;
            vm.FieldId = "CODE";
            vm.FieldName = "NAME_VN";
            vm.IsShowTitle = true;
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/Form/_Select.cshtml", vm);

            vm.Datas = new List<dynamic>();
            vm.Id = "COMMUNITY";
            vm.Name = "model.COMMUNITY";
            vm.Class = "";
            vm.LabelName = "Cart.Community";
            vm.IsRequied = true;
            vm.FieldId = "CODE";
            vm.FieldName = "NAME_VN";
            vm.IsShowTitle = true;

            var html1 = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/Form/_Select.cshtml", vm);
            List<string> listHtml = new List<string>();
            listHtml.Add(html);
            listHtml.Add(html1);
            return Json(listHtml, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ReloadCommunity(long codeDistrict)
        {
            SelectModel vm = new SelectModel();
            var response = _systemRepository.GetCommunityByCodeDistrict(codeDistrict);
            if (response != null && response.Results.Count() > 0)
            {
                vm.Datas = response.Results;

            }
            vm.Id = "COMMUNITY";
            vm.Name = "model.COMMUNITY";
            vm.Class = "";
            vm.LabelName = "Cart.Community";
            vm.IsRequied = true;
            vm.FieldId = "CODE";
            vm.FieldName = "NAME_VN";
            vm.IsRequied = true;
            vm.IsShowTitle = true;

            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/Form/_Select.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SearchCommonFromAdmin(string filterString)
        {
            SearchViewModel vm = new SearchViewModel();
            var response = _systemRepository.SearchCommonFromAdmin(filterString);
            var result = JsonConvert.DeserializeObject<List<SearchModel>>(JsonConvert.SerializeObject(response.Results));
            if (result != null && result.Count()>0)
            {
                vm.Datas = result;
            }
            var html = RenderViewToString(this.ControllerContext, "~/Areas/ADMIN/Views/Template/_Search.cshtml", vm);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

    }
}