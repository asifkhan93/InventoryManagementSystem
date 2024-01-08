using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Models
{
    public class InvProductIssueFormModel
    {
        public int Id { get; set; }
        public string ReqNo { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime PrevReqDate { get; set; }
        public string GinNo { get; set; }
        public DateTime GinDate { get; set; }
        public string TrNo { get; set; }
        public DateTime TrDate { get; set; }
        public string IssueTo { get; set; }
        public string IssueDept { get; set; }
        public string Department { get; set; }
        public string Details { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string Unit { get; set; }
        public string Ui { get; set; }
        public string ReqFrom { get; set; }
        public string ApprovedBy { get; set; }
        public string PrevReqNo { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public int ReqQty { get; set; }
        public decimal PurchasePrice { get; set; }
        public string SupplierName { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public int PReqQty { get; set; }
        public int ReqInQty { get; set; }
        public int ReqApprovedQty { get; set; }
        public int IssueQty { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public int ChkDone { get; set; }
        public double PerUnitPrice { get; set; }
        


    }
}