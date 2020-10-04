using System.ComponentModel.DataAnnotations;

namespace SAFT_Reader.Models
{
    public class CustomerEntry
    {
        [Display(Name = "ID")]
        public string CustomerID { get; set; }
        [Display(Name = "Conta")]
        public string AccountID { get; set; }
        [Display(Name = "NIF")]
        public string CustomerTaxID { get; set; }
        [Display(Name = "Nome Cliente")]
        public string CompanyName { get; set; }
        [Display(Name = "Morada")]
        public string AddressDetail { get; set; }
        [Display(Name = "Cidade")]
        public string City { get; set; }
        [Display(Name = "Cod.Postal")]
        public string PostalCode { get; set; }
        [Display(Name = "País")]
        public string Country { get; set; }
        [Display(Name = "Autofat.")]
        public string SelfBillingIndicator { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Total Créd.")]
        public float TotalCreditAmmount { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Total Déb.")]
        public float TotalDebitAmmount { get; set; }

    }
}
