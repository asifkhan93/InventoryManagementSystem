using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;

namespace PointOfSalesApp.Controllers
{
    public class InvPurchaseOrderController : Controller
    {

        readonly InvPurchaseOrderModel _aModel = new InvPurchaseOrderModel();
        readonly InvPurchaseOrderGateway _gt = new InvPurchaseOrderGateway();
        //
        // GET: /InvPurchaseOrder/
        public ActionResult Index()
        {
            ViewBag.reqList = _gt.GetCodeDescriptionForDropDown("SELECT DISTINCT ReqNo as Code, 0 as Description FROM InvPurchaseRequisitionMain ");

            return View();
        }
        public JsonResult Save(List<InvPurchaseOrderModel> _aModel)
        {
            
            string msg = "";
            if (_gt.FncSeekRecordNew("InvPurchaseOrderMain", " PoNo='" + _aModel.ElementAt(0).PoNo + "' AND PoDate='" + _aModel.ElementAt(0).PoDate.ToString("yyyy-MM-dd") + "' "))
            {
                msg = _gt.Update(_aModel);
            }

            else
            {
            msg = _gt.Save(_aModel);
             }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetRequisitionList(int param, string searchString, string category)
        {
            var data = _gt.GetRequisitionList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetSupplierList(int param, string searchString)
        {
            var data = _gt.GetSupplierList(param, searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetProductList(string param, string searchString, string lastIssue)
        {
            var data = _gt.GetProductList(param, searchString, lastIssue);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetEditProductList(string param, string searchString )
        {
            var data = _gt.GetEditProductList(param, searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetInvList(string searchString)
        {
            var data = _gt.GetInvList(searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult DeleteProduct(string param)
        {
            string msg = "";
            _gt.DeleteInsert("DELETE FROM InvPurchaseOrderMain WHERE PoNo='" + param + "'");
            _gt.DeleteInsert("DELETE FROM InvPurchaseOrderDetails WHERE PoNo='" + param + "' ");
            _gt.InsertUserLog("Delete Item :" + _gt.ReturnFieldValue("InvPurchaseOrderMain", "PoNo= '" + param + "'", "PoNo"));
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReport(string PoNo, string PoDate)
        {


            _gt.PrintReport("InvPurchaseReqOrderReport.rpt", "InvPurchaseOrderDetails", "WHERE PoNo='" + PoNo + "' AND PoDate='" + PoDate + "' AND Valid =1  ", "InvPurchaseOrderDetails", "Purchase Order", "", "V");

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSingleInvDetails(int param, string searchstring, string category)
        {
            var data = _gt.GetSingleInvDetails(param, searchstring, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
	}
}