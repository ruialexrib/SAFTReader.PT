using System.ComponentModel.DataAnnotations;

namespace SAFT_Reader.Models
{
    public class HeaderPt
    {
        public string Ficheiro_Auditoria { get; set; }
        public string Registo_Comercial_Empresa { get; set; }
        public string NIF_Empresa { get; set; }
        public string Nome_Empresa { get; set; }
        public string Designacao_Comercial { get; set; }
        public string Endereco_Detalhado { get; set; }
        public string Ano_Fiscal { get; set; }
        public string Data_Inicio { get; set; }
        public string Data_Fim { get; set; }
        public string Codigo_Moeda { get; set; }
        public string Data_Criacao { get; set; }
        public string Identificacao_Estabelecimento { get; set; }
        public string NIF_Produtora_Software { get; set; }
        public string Numero_Certificado { get; set; }
        public string Nome_Aplicacao { get; set; }
        public string Versao_Aplicacao { get; set; }
    }
}
