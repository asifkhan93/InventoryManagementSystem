using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;

namespace PointOfSalesApp.Controllers
{
    public class InvStockAdjustController : Controller
    {
        readonly InvStockAdjustModel _aModel = new InvStockAdjustModel();
        readonly InvStockAdjustGateway _gt = new InvStockAdjustGateway();


        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Save(List<InvStockAdjustModel> _aModel)
        {
            //
            string msg = "";
            //if (_gt.FncSeekRecordNew("InvProductIssueFormMain", " ReqNo='" + _aModel.ElementAt(0).ReqNo + "' AND GinDate='" + _aModel.ElementAt(0).ReqDate.ToString("yyyy-MM-dd") + "' "))
            //{
            //    //msg = _gt.Update(_aModel);
            //}

            //else
            //{
            msg = _gt.Save(_aModel);
            // }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetProductList(string param, string searchString, string lastIssue)
        {
            var data = _gt.GetProductList(param, searchString, lastIssue);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetProduct(string param, string searchString, string category)
        {
            var data = _gt.GetProduct(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
	}
}