using System;
using System.Collections.Generic;
using System.Linq;

using Programatica.Saft.Models;

using SAFT_Reader.Adapter;
using SAFT_Reader.Models;

namespace SAFT_Reader.Services
{
    public class AuditService : IAuditService
    {
        private readonly AuditReport _auditReport;

        private readonly IFileStreamAdapter _fileStreamAdapter;
        private readonly IXmlSerializerAdapter _xmlSerializerAdapter;

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

        public AuditReport Audit()
        {
            AuditInvoices();
            return _auditReport;
        }

        private void AuditInvoices()
        {
            foreach (var i in Globals.AuditFile.SourceDocuments.SalesInvoices.Invoice)
            {
                AssertInvoiceCustomerInCustomerList(i);
                AssertIfTaxExemptionReasonOnTaxPercentage(i);
            }
        }

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

        public AuditFile OpenFile(string path)
        {
            var model = _fileStreamAdapter.Read(path);
            return _xmlSerializerAdapter.ConvertXml<AuditFile>(model);
        }
    }
}