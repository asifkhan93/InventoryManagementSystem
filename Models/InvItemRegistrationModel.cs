using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Models
{
    public class InvItemRegistrationModel
    {
        public int Id { get; set; }
        public string ProductCategory { get; set; }
        public string ProductCode { get; set; }
        public string RegistrationGroup { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal ApproverPrice { get; set; }
        public string RackName { get; set; }
        public string Row { get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }
        public string InventoryType { get; set; }
        public string ReminderStock { get; set; }
        public string ReOrderOnly { get; set; }
        public string AcCode { get; set; }
        public string CostCenter { get; set; }
        public string Note { get; set; }
    }
}