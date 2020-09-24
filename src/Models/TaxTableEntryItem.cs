using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT_Reader.Models
{
    public class TaxTableEntryItem
    {
        public string InvoiceNo { get; set; }
        public string InvoiceType { get; set; }
        public string TaxCode { get; set; }
        public float TaxPercentage { get; set; }
        public float CreditAmount { get; set; }
        public float DebitAmount { get; set; }
    }
}
