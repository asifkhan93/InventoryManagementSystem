using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Models
{
    public class InvStockAdjustModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string TrNo { get; set; }
        public string Note { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public int CurrentQty { get; set; }
        public int CorrectedQty { get; set; }
        public string BatchNo { get; set; }
        public DateTime TrDate { get; set; }
    }
}
