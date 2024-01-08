using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Controllers
{
    public class InvRequisitionApprovalModel
    {

        public string ProductCode { get; set; }
        public string Balance { get; set; }
        public int Id { get; set; }
        public string ReqNo { get; set; }
        public string TrNo { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ReqFrom { get; set; }
        public string ApprovedBy { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string Details { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string Ui { get; set; }
        public int OurStock { get; set; }
        public int ReqInQty { get; set; }
        public int ReqQty { get; set; }
        public int ReqApprovedQty { get; set; }
        public int InQty { get; set; }
        public decimal TotalQty { get; set; }
        public string Note { get; set; }
        public string YesNo { get; set; }
    }
}
