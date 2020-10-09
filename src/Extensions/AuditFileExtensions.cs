using Programatica.Saft.Models;

using SAFT_Reader.Models;

namespace SAFT_Reader.Extensions
{
    public static class AuditFileExtensions
    {
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
                //Total_Registos = a?.SourceDocuments?.SalesInvoices != null
                //                                        ? a.SourceDocuments.SalesInvoices.NumberOfEntries.ToAuditInt()
                //                                        : 0,
                //Total_Creditos = a?.SourceDocuments?.SalesInvoices != null
                //                                        ? a.SourceDocuments.SalesInvoices.TotalCredit.ToAuditFloat()
                //                                        : 0,
                //Total_Debitos = a?.SourceDocuments?.SalesInvoices != null
                //                                        ? a.SourceDocuments.SalesInvoices.TotalDebit.ToAuditFloat()
                //                                        : 0,
            };
        }
    }
}