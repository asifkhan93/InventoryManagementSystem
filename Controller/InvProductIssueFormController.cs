using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;

namespace PointOfSalesApp.Controllers
{
    public class InvProductIssueFormController : Controller
    {
        readonly InvProductIssueFormModel _aModel = new InvProductIssueFormModel();
        readonly InvProductIssueFormGateway _gt = new InvProductIssueFormGateway();
        //
        // GET: /InvProductIssueForm/
        public  ActionResult Index(string ReqNo)
                {

                    ViewBag.reqList = _gt.GetCodeDescriptionForDropDown("SELECT DISTINCT TrNo as Code, 0 as Description FROM InvRequisitionApprovalMain ");
            //ViewBag.productList = _gt.GetCodeDescriptionForDropDown("SELECT DISTINCT ProductCode as Code, 0 as Description FROM InvRequisutionDetails where ReqNo = '" + ReqNo + "'");

            return View();
        }
        public JsonResult Save(List<InvProductIssueFormModel> _aModel)
        {
            //
            string msg = "";
            if (_gt.FncSeekRecordNew("InvProductIssueFormMain", " GinNo='" + _aModel.ElementAt(0).GinNo + "' AND GinDate='" + _aModel.ElementAt(0).GinDate.ToString("yyyy-MM-dd") + "' "))
            {
                //msg = _gt.Update(_aModel);
            }

            else
            {
                msg = _gt.Save(_aModel);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteProduct(string param)
        {
            string msg = "";
            _gt.DeleteInsert("DELETE FROM InvProductIssueFormMain WHERE GinNo='" + param + "'");
            _gt.DeleteInsert("DELETE FROM InvReqLedger WHERE TrNo='" + param + "' And UiFrom='ProductIssueForm'");
            _gt.InsertUserLog("Delete Item :" + _gt.ReturnFieldValue("InvProductIssueFormMain", "GinNo= '" + param + "'", "GinNo"));
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInvList(string searchString)
        {
            var data = _gt.GetInvList(searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetBatchList(int param, string searchString, string category)
        {
            var data = _gt.GetBatchList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetProductList(int param, string searchString, string category)
        {
            var data = _gt.GetProductList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetRequisitionList(string param)
        {
            var data = _gt.GetRequisitionList(param);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetReqList(int param, string searchString, string category)
        {
            var data = _gt.GetReqList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetApprovedQty(int param, string searchString, string category)
        {
            var data = _gt.GetApprovedQty(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult EditIssueReqList(int param, string searchString, string category)
        {
            var data = _gt.EditIssueReqList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetEditProductList(int param, string searchString, string category)
        {
            var data = _gt.GetEditProductList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
	}
}