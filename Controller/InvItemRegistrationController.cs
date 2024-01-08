using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Models;
using PointOfSalesApp.Gateway;


namespace PointOfSalesApp.Controllers
{
    public class InvItemRegistrationController : Controller
    {
        readonly InvItemRegistrationModel _aModel = new InvItemRegistrationModel();
        readonly InvItemRegistrationGateway _gt = new InvItemRegistrationGateway();
        //
        // GET: /InvItemRegistration/
        public ActionResult Index()
        {
            ViewBag.UnitList = _gt.GetIdNameForDropDownBox("SELECT  Id, Name FROM tbl_ITEM_UNIT  ORDER BY ID");
            ViewBag.CodeList = _gt.GetCodeDescriptionForDropDown("SELECT  0 as Code, RIGHT(CONCAT('0',MAX(ProductCode)+1), 5) as Description FROM InvItemRegistration ");
            ViewBag.MainCatList = _gt.GetIdNameForDropDownBox("SELECT 0 as Id , ProductCategory as Name FROM InvItemCategory order by Name ");
            ViewBag.StoreList = _gt.GetIdNameForDropDownBox("SELECT distinct 0 as Id , Pno as Name FROM ProjectInformations  ORDER BY Pno");

            return View();
        }
        public JsonResult Save(InvItemRegistrationModel _aModel)
        {
            string msg = "";
            if (_gt.FncSeekRecordNew("InvItemRegistration", "Id=" + _aModel.Id + " "))
            {
                _gt.InsertUserLog("Update Product");
                msg = _gt.Update(_aModel);
            }
            else
            {
                if (_gt.FncSeekRecordNew("InvItemRegistration", "ProductName='" + _aModel.ProductName + "' "))
                {
                    msg = "Already Exist This Product Name Please Check !!!";
                }
                else
                {
                    msg = _gt.Save(_aModel);
                }
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductList(int param, string searchString, string category)
        {
            var data = _gt.GetProductList(param, searchString, category);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult DeleteProduct(int param)
        {
            string msg = "";
            _gt.DeleteInsert("DELETE FROM InvItemRegistration WHERE Id=" + param + "");
            _gt.InsertUserLog("Delete Item :" + _gt.ReturnFieldValue("InvItemRegistration", "Id=" + param + "", "ProductName"));
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductCategory(string searchString)
        {
            var data = _gt.GetProductCategory(searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetAcCode(string searchString)
        {
            var data = _gt.GetAcCode(searchString);
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult SaveCategory(InvItemRegistrationModel _aModel)
        {
            string msg = "";

            if (_gt.FncSeekRecordNew("InvItemCategory", "ProductCategory='" + _aModel.ProductCategory + "' "))
            {
                msg = "Already Exist This ProductCategory Please Check !!!";
            }
            else
            {
                msg = _gt.SaveCategory(_aModel);
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
