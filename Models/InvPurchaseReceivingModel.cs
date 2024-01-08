using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Models
{
    public class InvPurchaseReceivingModel
    {
        public int Id { get; set; }
        public string GrnNo { get; set; }
        public DateTime GrnDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string SupplierName { get; set; }
        public DateTime PoDate { get; set; }
        public string PoNo { get; set; }
        public string Department { get; set; }
        public string Details { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string ProductCategory { get; set; }
        public int OurStock { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public double Total { get; set; }
        public double Vat { get; set; }
        public string VatType { get; set; }
        public double VatAmount { get; set; }
        public double Discount { get; set; }
        public string DiscountType { get; set; }
        public double DiscountAmount { get; set; }
        public double NetPrice { get; set; }
        public double TotalQty { get; set; }
        public double TotalPrice { get; set; }
        public double TotalVat { get; set; }
        public double TotalDiscount { get; set; }
        public String BatchNo { get; set; }
        public double TotalNetPrice { get; set; }
        public string Note { get; set; }
        public DateTime ExpDate { get; set; }
    }
}