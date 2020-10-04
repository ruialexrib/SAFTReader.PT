using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT_Reader.Models
{
    public class InvoiceEntry
    {
        [Display(Name = "Doc.Nº")]
        public string InvoiceNo { get; set; }
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public string InvoiceDate { get; set; }
        [Display(Name = "Tipo")]
        public string InvoiceType { get; set; }
        [Display(Name = "Periodo")]
        public string Period { get; set; }
        [Display(Name = "Utilizador")]
        public string SourceID { get; set; }
        [Display(Name = "NIF")]
        public string CustomerTaxID { get; set; }
        [Display(Name = "Cliente")]
        public string CompanyName { get; set; }
        [Display(Name = "Estado")]
        public string InvoiceStatus { get; set; }


        [DataType(DataType.Currency)]
        [Display(Name = "Base s/Imp.")]
        public float NetTotal { get; set; }
        [Display(Name = "Imposto")]
        [DataType(DataType.Currency)]
        public float TaxPayable { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Total c/Imp.")]
        public float GrossTotal { get; set; }
    }
}
