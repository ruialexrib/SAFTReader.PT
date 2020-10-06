using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime SourceID { get; set; }
        [Display(Name = "Descrição Movimento")]
        public string Description { get; set; }
    }
}
