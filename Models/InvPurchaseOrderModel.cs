using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Models
{
    public class InvPurchaseOrderModel
    {
        public int Id { get; set; }
        public string ReqNo { get; set; }
        public string PoNo { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime PoDate { get; set; }
        public string PoFor { get; set; }
        public string IssueDept { get; set; }
        public string Department { get; set; }
        public string ReqFrom { get; set; }
        public string Details { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string Unit { get; set; }
        public int InQty { get; set; }
        public int OrderdQty { get; set; }
        public int ReqQty { get; set; }
        public int CurrentQty { get; set; }
        public int IssuedQty { get; set; }
        public int EstReqQty { get; set; }
        public int EstQty { get; set; }
        public int LastIssueFor { get; set; }
        public int NextIssueFor { get; set; }
        public double TotalNet { get; set; }
        public double Total { get; set; }
        public double Price { get; set; }
        public string SupplierName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierMobileNo { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public int PReqQty { get; set; }
        public int ReqInQty { get; set; }
        public int ReqApprovedQty { get; set; }
        public string Note { get; set; }
        public int ChkDone { get; set; }
    }
}