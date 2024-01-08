using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Models
{
    public class InvSupplierRegisterModel
    {
        public string SupplierId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string AccountNo { get; set; }
        public string SwiftCode { get; set; }
        public string Address { get; set; }
        public string Pno { get; set; }
    }
}