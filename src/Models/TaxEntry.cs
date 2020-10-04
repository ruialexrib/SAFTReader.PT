using System.ComponentModel.DataAnnotations;

namespace SAFT_Reader.Models
{
    public class TaxEntry
    {
        [Display(Name = "Tipo")]
        public string TaxType { get; set; }
        [Display(Name = "Regiao")]
        public string TaxCountryRegion { get; set; }
        [Display(Name = "Código")]
        public string TaxCode { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Display(Name = "% Imposto")]
        public float TaxPercentage { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Total Créd.")]
        public float TotalCreditAmmount { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Total Déb.")]
        public float TotalDebitAmmount { get; set; }
    }
}
