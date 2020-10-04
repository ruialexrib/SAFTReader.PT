using System.ComponentModel.DataAnnotations;

namespace SAFT_Reader.Models
{
    public class TaxTableEntryTotal
    {
        [Display(Name = "Cod. Imp.")]
        public string TaxCode { get; set; }
        [Display(Name = "Descrição")]
        public string TaxDescription { get; set; }
        [Display(Name = "% Imposto")]
        public float TaxPercentage { get; set; }
        [Display(Name = "Base Créd.")]
        [DataType(DataType.Currency)]
        public float CreditAmount { get; set; }
        [Display(Name = "Base Déb.")]
        [DataType(DataType.Currency)]
        public float DebitAmount { get; set; }

        [Display(Name = "Base Total")]
        [DataType(DataType.Currency)]
        public float BalanceAmount { get; set; }

        [Display(Name = "Imp. Créd.")]
        [DataType(DataType.Currency)]
        public float CreditTaxPayable { get; set; }
        [Display(Name = "Imp. Déb.")]
        [DataType(DataType.Currency)]
        public float DebitTaxPayable { get; set; }

        [Display(Name = "Imp. Total")]
        [DataType(DataType.Currency)]
        public float BalanceTaxPayable { get; set; }
    }
}
