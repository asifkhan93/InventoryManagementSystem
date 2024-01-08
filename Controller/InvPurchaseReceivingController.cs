using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;
using PointOfSalesApp.Gateway.DB_Helper;

namespace PointOfSalesApp.Controllers
{
    public class InvPurchaseReceivingController : Controller
    {

        readonly InvPurchaseReceivingModel _aModel = new InvPurchaseReceivingModel();
        readonly InvPurchaseReceivingGateway _gt = new InvPurchaseReceivingGateway();
        DbConnection _aDbConnection = new DbConnection();
        //
        // GET: /InvPurchaseReceiving/
        public ActionResult Index()
        {

            //ViewBag.MainSupplierList = _gt.GetCodeDescriptionForDropDown("SELECT SupplierId as Code , Name as Description FROM InvSupplierRegisterMain  ");
            return View();
        }
        public JsonResult Save(List<InvPurchaseReceivingModel> _aModel)
        {
            
            string msg = "";
            if (_gt.FncSeekRecordNew("InvPurchaseReceivingMain", " GrnNo='" + _aModel.ElementAt(0).GrnNo + "' AND GrnDate='" + _aModel.ElementAt(0).GrnDate.ToString("yyyy-MM-dd") + "' "))
            {
                msg = _gt.Update(_aModel);
            }

            else
            {
                msg = _gt.Save(_aModel);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBatchList(int param, string searchString)
        {
            var data = _gt.GetBatchList(param, searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult DeleteProduct(string param)
        {
            string msg = "";
            _gt.DeleteInsert("DELETE FROM InvPurchaseReceivingMain WHERE GnrNo='" + param + "'");
            _gt.DeleteInsert("DELETE FROM InvStockLedger WHERE TrNo='" + param + "' ");
            _gt.InsertUserLog("Delete Item :" + _gt.ReturnFieldValue("InvPurchaseReceivingMain", "GnrNo= '" + param + "'", "GnrNo"));
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList(string param, string searchString)
        {
            var data = _gt.GetProductList(param, searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetPoList(string param, string searchString)
        {
            var data = _gt.GetPoList(param, searchString);
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
        public JsonResult GetInvList(string searchString)
        {
            var data = _gt.GetInvList(searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GetReport(string GrnNo, string GrnDate)
        {


            _gt.PrintReport("InvoiceReportPurchaseReceive.rpt", "InvPurchaseReceivingDetails", "WHERE GrnNo='" + GrnNo + "' AND GrnDate='" + GrnDate + "' ", "InvPurchaseReceivingDetails", "Purchase Report", "", "V");

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGrnList(int param, string searchString, string category)
        {
            var data = _gt.GetGrnList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GetEditProductList(string param, string searchString)
        {
            var data = _gt.GetEditProductList(param, searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}