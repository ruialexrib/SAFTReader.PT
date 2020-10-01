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
        [Display(Name = "Doc.Nº")]
        public string InvoiceNo { get; set; }
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public string InvoiceDate { get; set; }
        [Display(Name = "Tipo")]
        public string InvoiceType { get; set; }
        [Display(Name = "NIF")]
        public string CustomerTaxID { get; set; }
        [Display(Name = "Cliente")]
        public string CompanyName { get; set; }
        [Display(Name = "Linha")]
        public string LineNumber { get; set; }
        [Display(Name = "Produto")]
        public string ProductDescription { get; set; }
        [Display(Name = "Quant.")]
        public float Quantity { get; set; }
        [Display(Name = "Pr. Unit.")]
        [DataType(DataType.Currency)]
        public float UnitPrice { get; set; }
        [Display(Name = "Cod. Imp.")]
        public string TaxCode { get; set; }
        [Display(Name = "% Imposto")]
        public float TaxPercentage { get; set; }
        [Display(Name = "Crédito")]
        [DataType(DataType.Currency)]
        public float CreditAmount { get; set; }
        [Display(Name = "Débito")]
        [DataType(DataType.Currency)]
        public float DebitAmount { get; set; }
        [Display(Name = "Imposto")]
        [DataType(DataType.Currency)]
        public float TaxPayable { get; set; }
    }
}
