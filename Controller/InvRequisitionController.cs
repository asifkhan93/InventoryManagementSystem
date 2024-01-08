using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;

namespace PointOfSalesApp.Controllers
{
    public class InvRequisitionController : Controller
    {

        readonly InvRequisitionModel _aModel = new InvRequisitionModel();
        readonly InvRequisitionGateway _gt = new InvRequisitionGateway();
        //
        // GET: /InvRequisition/
        public ActionResult Index()
        {

            ViewBag.MainCatList = _gt.GetIdNameForDropDownBox("SELECT distinct 0 as Id , DeptName as Name FROM InvDepartmentInfo  ORDER BY DeptName");
            return View();
        }
        public JsonResult Save(List<InvRequisitionModel> _aModel)
        {
            
            string msg = "";
            if (_gt.FncSeekRecordNew("InvRequisitionMain", " ReqNo='" + _aModel.ElementAt(0).ReqNo + "' AND ReqDate='" + _aModel.ElementAt(0).ReqDate.ToString("yyyy-MM-dd") + "' "))
            {
                msg = _gt.Update(_aModel);
            }

            else
            {
                msg = _gt.Save(_aModel);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

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
            _gt.DeleteInsert("DELETE FROM InvRequisitionMain WHERE ReqNo='" + param + "'");
            _gt.DeleteInsert("DELETE FROM InvRequisutionDetails WHERE ReqNo='" + param + "'");
            _gt.DeleteInsert("DELETE FROM InvReqLedger WHERE TrNo='" + param + "' And UiFrom='Requsition'");
            _gt.InsertUserLog("Delete Item :" + _gt.ReturnFieldValue("InvRequisitionMain", "ReqNo= '" + param + "'", "ReqNo"));
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequisitionList(string param, string searchString, string category)
        {
            var data = _gt.GetRequisitionList(param,searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        
        public JsonResult GetProductList(int param, string searchString )
        {
            var data = _gt.GetProductList(param, searchString  );
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