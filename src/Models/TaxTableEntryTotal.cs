using System.ComponentModel.DataAnnotations;

namespace SAFT_Reader.Models
{
    /// <summary>
    /// Represents a tax table entry total.
    /// </summary>
    public class TaxTableEntryTotal
    {
        [Display(Name = "Cod. Imp.")]
        public string TaxCode { get; set; }

        [Display(Name = "Descrição")]
        public string TaxDescription { get; set; }

        [Display(Name = "% Imposto")]
        public float TaxPercentage { get; set; }

        [Display(Name = "Base")]
        [DataType(DataType.Currency)]
        public float DebitAmount { get; set; }

        [Display(Name = "Imp.")]
        [DataType(DataType.Currency)]
        public float DebitTaxPayable { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public float TotalDebit { get; set; }


        [Display(Name = "Base")]
        [DataType(DataType.Currency)]
        public float CreditAmount { get; set; }

        [Display(Name = "Imp.")]
        [DataType(DataType.Currency)]
        public float CreditTaxPayable { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public float TotalCredit { get; set; }

    }
}