using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;
using System.Threading.Tasks;

namespace PointOfSalesApp.Controllers
{
    public class InvRequisitionApprovalController : Controller
    {
        readonly InvRequisitionApprovalModel _aModel = new InvRequisitionApprovalModel();
        readonly InvRequisitionApprovalGateway _gt = new InvRequisitionApprovalGateway();
        //
        // GET: /InvRequisitionApproval/
        public ActionResult Index()
        {

            ViewBag.reqList = _gt.GetCodeDescriptionForDropDown("SELECT DISTINCT ReqNo as Code, 0 as Description FROM InvRequisitionMain ");
            return View();
        }

        public async Task<JsonResult> Save(List<InvRequisitionApprovalModel> _aModel)
        {
            
            string msg = "";
            if (_gt.FncSeekRecordNew("InvRequisitionApprovalMain", " TrNo='" + _aModel.ElementAt(0).TrNo + "' AND TrDate='" + _aModel.ElementAt(0).ApprovalDate.ToString("yyyy-MM-dd") + "' "))
            {
                msg = _gt.Update(_aModel);
            }

            else
            {
                msg = await _gt.Save(_aModel);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteProduct(string param)
        {
            string msg = "";
            _gt.DeleteInsert("DELETE FROM InvRequisitionApprovalMain WHERE TrNo='" + param + "'");
            _gt.DeleteInsert("DELETE FROM InvReqLedger WHERE TrNo='" + param + "' And UiFrom='Req.Approval'");
            _gt.InsertUserLog("Delete Item :" + _gt.ReturnFieldValue("InvRequisitionApprovalMain", "TrNo= '" + param + "'", "TrNo"));
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInvList(string searchString)
        {
            var data = _gt.GetInvList(searchString);
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
        public JsonResult GetRequisitionList(int param, string searchString, string category)
       {
           var data = _gt.GetRequisitionList(param, searchString, category);
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
        public JsonResult GetReqList(int param, string searchString, string category)
        {
            var data = _gt.GetReqList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
	}
}