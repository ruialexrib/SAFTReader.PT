using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT_Reader.Models
{
    public class InvoiceLine
    {
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public string CustomerTaxID { get; set; }
        public string CompanyName { get; set; }
        public string LineNumber { get; set; }
        public string ProductDescription { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
        public string TaxCode { get; set; }
        public float TaxPercentage { get; set; }
        public float CreditAmount { get; set; }
        public float DebitAmount { get; set; }
        public float TaxPayable { get; set; }
    }
}
