using System.ComponentModel.DataAnnotations;

namespace SAFT_Reader.Models
{
    public class AccountEntry
    {
        [Display(Name = "Conta", Description = "Devem constar do ficheiro todas as contas, incluindo as respetivas contas integradoras, até às contas do Razão.")]
        public string AccountID { get; set; }
        [Display(Name = "Descrição")]
        public string AccountDescription { get; set; }
        [Display(Name = "Saldo Inic. Déb.")]
        public float OpeningDebitBalance { get; set; }
        [Display(Name = "Saldo Inic. Créd.")]
        public float OpeningCreditBalance { get; set; }
        [Display(Name = "Saldo Final Déb.")]
        public float ClosingDebitBalance { get; set; }
        [Display(Name = "Saldo Final Créd.")]
        public float ClosingCreditBalance { get; set; }
        [Display(Name = "Categoria")]
        public string GroupingCategory { get; set; }
        [Display(Name = "Hierarquia")]
        public string GroupingCode { get; set; }
        [Display(Name = "Taxonomia")]
        public string TaxonomyCode { get; set; }
    }
}
