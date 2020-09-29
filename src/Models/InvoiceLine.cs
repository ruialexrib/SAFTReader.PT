using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT_Reader.Models
{
    public class InvoiceLine
    {
        public string InvoiceNo { get; set; }
        [DataType(DataType.Date)]
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public string CustomerTaxID { get; set; }
        [Display(Name = "CompanyName", Description = "Description for CompanyName")]
        public string CompanyName { get; set; }
        public string LineNumber { get; set; }
        public string ProductDescription { get; set; }
        public float Quantity { get; set; }
        [DataType(DataType.Currency)]
        public float UnitPrice { get; set; }
        public string TaxCode { get; set; }
        public float TaxPercentage { get; set; }
        [DataType(DataType.Currency)]
        public float CreditAmount { get; set; }
        [DataType(DataType.Currency)]
        public float DebitAmount { get; set; }
        [DataType(DataType.Currency)]
        public float TaxPayable { get; set; }
    }
}
