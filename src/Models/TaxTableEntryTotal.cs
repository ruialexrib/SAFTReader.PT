using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Currency)]
        public float CreditAmount { get; set; }
        [DataType(DataType.Currency)]
        public float DebitAmount { get; set; }
        [DataType(DataType.Currency)]
        public float CreditTaxPayable { get; set; }
        [DataType(DataType.Currency)]
        public float DebitTaxPayable { get; set; }
    }
}
