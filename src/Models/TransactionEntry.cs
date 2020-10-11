using System;
using System.ComponentModel.DataAnnotations;

namespace SAFT_Reader.Models
{
    public class TransactionEntry
    {
        [Display(Name = "Diário")]
        public string JournalID { get; set; }

        [Display(Name = "Descrição Diário")]
        public string JournalDescription { get; set; }

        [Display(Name = "ID")]
        public string TransactionID { get; set; }

        [Display(Name = "Period")]
        public string Period { get; set; }

        [Display(Name = "Data")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Utilizador")]
        public string SourceID { get; set; }

        [Display(Name = "Descrição Movimento")]
        public string TransactionDescription { get; set; }

        [Display(Name = "Tipo")]
        public string TransactionType { get; set; }

        [Display(Name = "Arquivo")]
        public string DocArchivalNumber { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerID { get; set; }

        [Display(Name = "Conta")]
        public string AccountID { get; set; }

        [Display(Name = "Descrição Linha")]
        public string LineDescription { get; set; }

        [Display(Name = "Débito")]
        [DataType(DataType.Currency)]
        public float DebitAmount { get; set; }

        [Display(Name = "Crédito")]
        [DataType(DataType.Currency)]
        public float CreditAmount { get; set; }
    }
}