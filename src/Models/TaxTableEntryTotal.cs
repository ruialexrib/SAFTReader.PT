using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT_Reader.Models
{
    public class TaxTableEntryTotal
    {
        public string TaxCode { get; set; }
        public string TaxDescription { get; set; }
        public float TaxPercentage { get; set; }
        public float CreditAmount { get; set; }
        public float DebitAmount { get; set; }
        public float CreditTaxPayable { get; set; }
        public float DebitTaxPayable { get; set; }
    }
}
