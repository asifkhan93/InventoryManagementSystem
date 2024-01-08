using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;

namespace PointOfSalesApp.Controllers
{
    public class InvPurchaseRequisitionController : Controller
    {

        readonly InvPurchaseRequisitionModel _aModel = new InvPurchaseRequisitionModel();
        readonly InvPurchaseRequisitionGateway _gt = new InvPurchaseRequisitionGateway();
        //
        // GET: /InvPurchaseRequisition/
        public ActionResult Index()
        {
            ViewBag.MainCatList = _gt.GetIdNameForDropDownBox("SELECT distinct 0 as Id , Name as Name FROM InvSupplierRegisterMain  ORDER BY Name");
            return View();
        }

        public JsonResult DeleteProduct(string param)
        {
            string msg = "";
            _gt.DeleteInsert("DELETE FROM InvPurchaseRequisitionMain WHERE ReqNo='" + param + "'");
            _gt.DeleteInsert("DELETE FROM InvPurchaseRequisitionDetails WHERE ReqNo='" + param + "'");
            //_gt.DeleteInsert("DELETE FROM InvReqLedger WHERE TrNo='" + param + "' And UiFrom='Requsition'");
            _gt.InsertUserLog("Delete Item :" + _gt.ReturnFieldValue("InvPurchaseRequisitionMain", "ReqNo= '" + param + "'", "ReqNo"));
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInvList(string searchString)
        {
            var data = _gt.GetInvList(searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult Save(List<InvPurchaseRequisitionModel> _aModel)
        {
            //
            string msg = "";
            if (_gt.FncSeekRecordNew("InvPurchaseRequisitionMain", " ReqNo='" + _aModel.ElementAt(0).ReqNo + "' AND ReqDate='" + _aModel.ElementAt(0).ReqDate.ToString("yyyy-MM-dd") + "' "))
            {
                 msg = _gt.Update(_aModel);
            }

            else
            {
                msg = _gt.Save(_aModel);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetRequisitionList(string param)
        {
            var data = _gt.GetRequisitionList(param);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetEditProductList(int param, string searchString, string productcode)
        {
            var data = _gt.GetEditProductList(param, searchString, productcode);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
	}
}