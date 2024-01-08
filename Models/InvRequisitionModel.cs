using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Models
{
    public class InvRequisitionModel
    {
        public int Id { get; set; }
        public string ReqNo { get; set; }
        public DateTime ReqDate { get; set; }
        public string ReqFrom { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string Details { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string OurStock { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; } 
        public int ReqQty { get; set; }
        public int InQty { get; set; }
        public decimal TotalQty { get; set; }
        public string Note { get; set; }
        public string Supplier { get; set; }
        public string Ui { get; set; }
    }
}