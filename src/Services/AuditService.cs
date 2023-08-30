using System;
using System.Collections.Generic;
using System.Linq;

using Programatica.Saft.Models;

using SAFT_Reader.Adapter;
using SAFT_Reader.Models;

namespace SAFT_Reader.Services
{
    /// <summary>
    /// Represents an audit service for auditing operations.
    /// </summary>

    public class AuditService : IAuditService
    {
        private readonly AuditReport _auditReport;

        private readonly IFileStreamAdapter _fileStreamAdapter;
        private readonly IXmlSerializerAdapter _xmlSerializerAdapter;

        /// <summary>
        /// Initializes a new instance of the AuditService class.
        /// </summary>
        /// <param name="fileStreamAdapter">An implementation of IFileStreamAdapter for file operations.</param>
        /// <param name="xmlSerializerAdapter">An implementation of IXmlSerializerAdapter for XML serialization.</param>
        public AuditService(IFileStreamAdapter fileStreamAdapter, IXmlSerializerAdapter xmlSerializerAdapter)
        {
            _fileStreamAdapter = fileStreamAdapter;
            _xmlSerializerAdapter = xmlSerializerAdapter;

            _auditReport = new AuditReport
            {
                AuditDate = DateTime.Now,
                AuditErrorList = new List<AuditError>()
            };
        }

        /// <summary>
        /// Performs an audit of invoices and returns an AuditReport.
        /// </summary>
        /// <remarks>
        /// This method executes the audit process for invoices and populates the AuditReport
        /// with audit results and error information.
        /// </remarks>
        /// <returns>An AuditReport containing the results of the audit.</returns>
        public AuditReport Audit()
        {
            AuditInvoices();
            return _auditReport;
        }

        /// <summary>
        /// Performs an audit of invoices in the audit file.
        /// </summary>
        /// <remarks>
        /// This method iterates through the invoices in the audit file's source documents,
        /// performing various audit checks and assertions.
        /// </remarks>
        private void AuditInvoices()
        {
            foreach (var i in Globals.AuditFile.SourceDocuments.SalesInvoices.Invoice)
            {
                AssertInvoiceCustomerInCustomerList(i);
                AssertIfTaxExemptionReasonOnTaxPercentage(i);
            }
        }

        /// <summary>
        /// Asserts whether the customer of an invoice exists in the customer list.
        /// </summary>
        /// <param name="i">The invoice to be audited.</param>
        /// <remarks>
        /// This method checks if the customer specified in the invoice exists in the customer list
        /// within the audit file's 'MasterFiles.Customer' table. If not, an error is recorded in the AuditReport.
        /// </remarks>
        /// <param name="i">The invoice to be audited.</param>
        private void AssertInvoiceCustomerInCustomerList(Invoice i)
        {
            var c = Globals.AuditFile.MasterFiles.Customer.Where(x => x.CustomerID.Equals(i.CustomerID)).FirstOrDefault();
            if (c == null)
            {
                _auditReport.AuditErrorList.Add(new AuditError
                {
                    AuditElement = $"Invoice [{i.InvoiceNo}]",
                    AuditErrorType = AuditErrorType.Error,
                    ErrorDescription = $"O elemento CustomerID:{i.CustomerID} não existe na tabela de 'MasterFiles.Customer' "
                });
            }
        }

        /// <summary>
        /// Asserts if the TaxExemptionReason is provided when TaxPercentage is not specified.
        /// </summary>
        /// <param name="i">The invoice to be audited.</param>
        /// <remarks>
        /// This method checks if a TaxExemptionReason is provided when the TaxPercentage is not specified
        /// for each line in the invoice. If TaxPercentage is missing or zero, TaxExemptionReason should not be empty.
        /// If the condition is not met, an error is recorded in the AuditReport.
        /// </remarks>
        /// <param name="i">The invoice to be audited.</param>
        private void AssertIfTaxExemptionReasonOnTaxPercentage(Invoice i)
        {
            foreach (var l in i.Line)
            {
                var tp = l.Tax.TaxPercentage;
                var ter = l.TaxExemptionReason;

                if (tp == null)
                {
                    if ((string.IsNullOrEmpty(tp) || float.Parse(tp) == 0) && (string.IsNullOrEmpty(ter)))
                    {
                        _auditReport.AuditErrorList.Add(new AuditError
                        {
                            AuditElement = $"Invoice [{i.InvoiceNo}]",
                            AuditErrorType = AuditErrorType.Error,
                            ErrorDescription = $"O elemento TaxExemptionReason não pode ser vazio."
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Merges multiple audit files into a single consolidated audit file.
        /// </summary>
        /// <remarks>
        /// This method combines data from multiple audit files into a single audit file.
        /// It merges customer data, tax tables, product data, invoices, and accounts
        /// from the principal audit file with those from attached sub-audit files.
        /// </remarks>
        /// <returns>The consolidated AuditFile containing merged data.</returns>
        public AuditFile MergeAudits()
        {
            AuditFile audit = OpenFile(Globals.AttachedFiles
                                                .Where(x => x.IsPrincipal == true)
                                                .FirstOrDefault().FilePath);

            foreach (var file in Globals.AttachedFiles.Where(x => x.IsPrincipal == false))
            {
                var subaudit = OpenFile(file.FilePath);

                // merge customers
                audit.MasterFiles.Customer = audit
                                                .MasterFiles
                                                .Customer
                                                .Concat(
                                                    subaudit.MasterFiles
                                                    .Customer
                                                    .Where(p => audit.MasterFiles.Customer.All(p2 => p2.CustomerID != p.CustomerID))
                                                    )
                                                .ToList();

                // merge taxtables
                audit.MasterFiles.TaxTable.TaxTableEntry = audit
                                                            .MasterFiles
                                                            .TaxTable
                                                            .TaxTableEntry
                                                            .Concat(
                                                                subaudit.MasterFiles.TaxTable
                                                                .TaxTableEntry
                                                                .Where(p => audit.MasterFiles.TaxTable.TaxTableEntry.All(p2 => p2.TaxCode != p.TaxCode &&
                                                                                                                               p2.TaxPercentage != p.TaxPercentage))
                                                                )
                                                            .ToList();

                // merge products
                audit.MasterFiles.Product = audit
                                                .MasterFiles
                                                .Product
                                                .Concat(
                                                    subaudit.MasterFiles
                                                    .Product
                                                    .Where(p => audit.MasterFiles.Product.All(p2 => p2.ProductCode != p.ProductCode))
                                                    )
                                                .ToList();

                // merge invoices
                audit.SourceDocuments.SalesInvoices.Invoice = audit
                                                                .SourceDocuments
                                                                .SalesInvoices
                                                                .Invoice
                                                                .Concat(
                                                                    subaudit.SourceDocuments.SalesInvoices
                                                                    .Invoice
                                                                    .Where(p => audit.SourceDocuments.SalesInvoices.Invoice.All(p2 => p2.InvoiceNo != p.InvoiceNo))
                                                                    )
                                                                .ToList();

                // merge accounts
                if (audit.MasterFiles.GeneralLedgerAccounts != null)
                {
                    audit.MasterFiles.GeneralLedgerAccounts.Account = audit
                                                                    .MasterFiles
                                                                    ?.GeneralLedgerAccounts
                                                                    ?.Account
                                                                    .Concat(
                                                                        subaudit.MasterFiles?.GeneralLedgerAccounts
                                                                        .Account
                                                                        .Where(p => audit.MasterFiles.GeneralLedgerAccounts.Account.All(p2 => p2.AccountID != p.AccountID))
                                                                        )
                                                                    .ToList() ?? new List<Account>();
                }
            }

            return audit;
        }

        /// <summary>
        /// Opens and deserializes an audit file from the specified path.
        /// </summary>
        /// <param name="path">The file path of the audit file to open and deserialize.</param>
        /// <returns>The deserialized AuditFile object.</returns>
        public AuditFile OpenFile(string path)
        {
            var model = _fileStreamAdapter.Read(path);
            return _xmlSerializerAdapter.ConvertXml<AuditFile>(model);
        }
    }
}