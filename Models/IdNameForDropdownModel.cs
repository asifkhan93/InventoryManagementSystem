using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PointOfSalesApp.Models
{
    public class IdNameForDropdownModel
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public string NameDesc { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}