using System;
using System.Collections.Generic;
using System.Linq;
using Programatica.Saft.Models;
using SAFT_Reader.Models;

namespace SAFT_Reader.Services
{
    public class AuditService : IAuditService
    {
        private AuditFile _audit;
        private AuditReport _auditReport;

        public AuditService()
        {
            _auditReport = new AuditReport
            {
                AuditDate = DateTime.Now,
                AuditErrorList = new List<AuditError>()
            };
        }

        public AuditReport Audit(AuditFile audit)
        {
            _audit = audit;

            AuditInvoices();

            return _auditReport;
        }

        private void AuditInvoices()
        {
            foreach (var i in _audit.SourceDocuments.SalesInvoices.Invoice)
            {
                AssertInvoiceCustomerInCustomerList(i);
                AssertIfTaxExemptionReasonOnTaxPercentage(i);
            }
        }

        private void AssertInvoiceCustomerInCustomerList(Invoice i)
        {
            var c = _audit.MasterFiles.Customer.Where(x => x.CustomerID.Equals(i.CustomerID)).FirstOrDefault();
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
    }
}
