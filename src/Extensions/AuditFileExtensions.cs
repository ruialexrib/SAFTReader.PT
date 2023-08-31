using Programatica.Saft.Models;

using SAFT_Reader.Models;

namespace SAFT_Reader.Extensions
{
    /// <summary>
    /// Extension methods for converting AuditFile objects to HeaderPt objects.
    /// </summary>
    public static class AuditFileExtensions
    {
        /// <summary>
        /// Converts an AuditFile object to a HeaderPt object with Portuguese field names.
        /// </summary>
        /// <param name="a">The AuditFile object to convert.</param>
        /// <returns>A HeaderPt object containing audit file header information with Portuguese field names.</returns>
        public static HeaderPt ToHeaderPt(this AuditFile a)
        {
            return new HeaderPt
            {
                Ficheiro_Auditoria = a.Header.AuditFileVersion,
                Registo_Comercial_Empresa = a.Header.CompanyID,
                NIF_Empresa = a.Header.TaxRegistrationNumber,
                Nome_Empresa = a.Header.CompanyName,
                Designacao_Comercial = a.Header.BusinessName,
                Endereco_Detalhado = a.Header.CompanyAddress.AddressDetail,
                Ano_Fiscal = a.Header.FiscalYear,
                Data_Inicio = a.Header.StartDate,
                Data_Fim = a.Header.EndDate,
                Codigo_Moeda = a.Header.CurrencyCode,
                Data_Criacao = a.Header.DateCreated,
                Identificacao_Estabelecimento = a.Header.TaxEntity,
                NIF_Produtora_Software = a.Header.ProductCompanyTaxID,
                Numero_Certificado = a.Header.SoftwareCertificateNumber,
                Nome_Aplicacao = a.Header.ProductID,
                Versao_Aplicacao = a.Header.ProductVersion,
            };
        }
    }
}