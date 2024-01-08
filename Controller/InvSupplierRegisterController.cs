using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;


namespace PointOfSalesApp.Controllers
{
    public class InvSupplierRegisterController : Controller
    {
        readonly InvSupplierRegisterModel _aModel = new InvSupplierRegisterModel();
        readonly InvSupplierRegisterGateway _gt = new InvSupplierRegisterGateway();
        //
        // GET: /InvSupplierRegister/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Save(InvSupplierRegisterModel _aModel)
        {
            
            string msg = "";
            if (_gt.FncSeekRecordNew("InvSupplierRegisterMain", " SupplierId='" + _aModel.SupplierId + "' "))
            {
                //msg = _gt.Update(_aModel);
            }

            else
            {
                msg = _gt.Save(_aModel);
           }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSupplierList(int param, string searchString)
        {
            var data = _gt.GetSupplierList(param, searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        //public JsonResult DeleteProduct(int param)
        //{
        //    string msg = "";
        //    _gt.DeleteInsert("DELETE FROM InvRequisutionDetails WHERE Id=" + param + "");
        //    _gt.InsertUserLog("Delete Item :" + _gt.ReturnFieldValue("InvRequisutionDetails", "Id=" + param + "", "ReqNo"));
        //    return Json(msg, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetRequisitionList(string param)
        //{
        //    var data = _gt.GetRequisitionList(param);
        //    var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}
	}
}