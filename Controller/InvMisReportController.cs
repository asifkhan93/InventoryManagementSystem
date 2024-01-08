using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointOfSalesApp.Gateway;
using PointOfSalesApp.Gateway.DB_Helper;
using PointOfSalesApp.Manager;


namespace PointOfSalesApp.Controllers
{
    public class InvMisReportController : Controller
    {
        readonly DbConnection _aDb = new DbConnection();
        readonly SupplierGateway _aSupplierGateway = new SupplierGateway();
        readonly IdNameForDropdownGateway _aIdNameForDropdownGateway = new IdNameForDropdownGateway();
        //
        // GET: /InvMisReport/

        public ActionResult PurchaseReceiveReport()
        {
            string userName = (string)Session["UserName"];
            ViewBag.Permission = _aDb.GetPermissionForUser(userName, "MisReport");
            ViewBag.SupplierList = _aSupplierGateway.GetSupplierList(0);
            return View();
        }
        [HttpPost]
        public string Index(ReportPrint aPrint)
        {
            string lcCondition = " '" + aPrint.DateFrom + "' AND '" + aPrint.DateTo + "'";
            switch (aPrint.ReportFileName)
            {
                case "Product Receiving Report":
                    _aDb.PrintReport("invpurchasereceiving.rpt", "InvPurchaseReceivingDetails", "WHERE GrnDate BETWEEN"+lcCondition, "InvPurchaseReceivingDetails", "Product Receiving Report", "Reporting date from " + aPrint.DateFrom + " to " + aPrint.DateTo, "V");
                    break;
                case "Product Name Wise Summarized":
                    _aDb.PrintReport("InvReceivingReportProductNameWise.rpt", "InvPurchaseReceivingDetails", "WHERE GrnDate BETWEEN" + lcCondition, "InvPurchaseReceivingDetails", "Product Receiving Report(Product Name Wise Summarized)", "Reporting date from " + aPrint.DateFrom + " to " + aPrint.DateTo, "V");
                    break;
                case "Company Name Wise Summarized":
                    _aDb.PrintReport("InvoiceReportCompanyNameWise.rpt", "InvPurchaseReceivingDetails", "WHERE GrnDate BETWEEN" + lcCondition, "InvPurchaseReceivingDetails", "Product Receiving Report (Company Name Wise Summarized)", "Reporting date from " + aPrint.DateFrom + " to " + aPrint.DateTo, "V");
                    break;
                case "Product Receive Report (Category Wise)":
                    _aDb.PrintReport("InvoiceReportCategoryWise.rpt", "InvPurchaseReceivingDetails", "WHERE GrnDate BETWEEN" + lcCondition, "InvPurchaseReceivingDetails", "Product Receive Report (Category Wise)", "Reporting date from " + aPrint.DateFrom + " to " + aPrint.DateTo, "V");
                    break;
            }
            return "";
        }

        public class ReportPrint
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string ItemId { get; set; }
            public string SuppId { get; set; }
            public string ReportFileName { get; set; }
            public string ReportType { get; set; }
        }

    }
}