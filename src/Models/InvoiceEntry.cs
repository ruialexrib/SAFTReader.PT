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
        public string InvoiceNo { get; set; }
        [DataType(DataType.Date)]
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public string Period { get; set; }
        public string SourceID { get; set; }
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string InvoiceStatus { get; set; }
        [DataType(DataType.Currency)]
        public float TaxPayable { get; set; }
        [DataType(DataType.Currency)]
        public float NetTotal { get; set; }
        [DataType(DataType.Currency)]
        public float GrossTotal { get; set; }
    }
}
