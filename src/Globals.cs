using Programatica.Saft.Models;
using SAFT_Reader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAFT_Reader
{
    public static class Globals
    {
        public static AuditFile AuditFile { get; set; }
        //public static string Filepath { get; set; }
        //public static List<string> AttachedFilePaths { get; set; }
        //public static List<AuditFile> AttachedAuditFiles { get; set; }

        public static List<AttachedFile> AttachedFiles { get; set; }

        public static string VersionLabel
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return string.Format("Versão: {0}.{1}.{2}.{3} (NetworkDeployed)",
                        ver.Major, ver.Minor, ver.Build, ver.Revision,
                        Assembly.GetEntryAssembly().GetName().Name);
                }
                else
                {
                    var ver = Assembly.GetExecutingAssembly().GetName().Version;
                    return string.Format("Versão: {0}.{1}.{2}.{3} (Debug)",
                        ver.Major, ver.Minor, ver.Build, ver.Revision,
                        Assembly.GetEntryAssembly().GetName().Name);
                }
            }
        }


        public static List<CustomerEntry> LoadCustomerLines()
        {
            var audit = Globals.AuditFile;
            var lines = LoadInvoiceLines();

            return audit.MasterFiles
                            .Customer
                            .Select(c => new CustomerEntry
                            {
                                CustomerID = c.CustomerID,
                                AccountID = c.AccountID,
                                CustomerTaxID = c.CustomerTaxID,
                                CompanyName = c.CompanyName,
                                AddressDetail = c.BillingAddress.AddressDetail,
                                City = c.BillingAddress.City,
                                PostalCode = c.BillingAddress.PostalCode,
                                Country = c.BillingAddress.Country,
                                SelfBillingIndicator = c.SelfBillingIndicator,
                                TotalCreditAmmount = lines
                                                        .Where(x=>x.CustomerTaxID == c.CustomerTaxID)
                                                        .Sum(s=>s.CreditAmount),
                                TotalDebitAmmount = lines
                                                        .Where(x => x.CustomerTaxID == c.CustomerTaxID)
                                                        .Sum(s => s.DebitAmount),

                            }).ToList();

        }

        public static List<ProductEntry> LoadProductLines()
        {
            var audit = Globals.AuditFile;
            var lines = LoadInvoiceLines();

            return audit.MasterFiles
                            .Product
                            .Select(c => new ProductEntry
                            {
                                ProductType = c.ProductType,
                                ProductCode = c.ProductCode,
                                ProductDescription = c.ProductDescription,
                                ProductNumberCode = c.ProductNumberCode,                             
                                TotalCreditAmmount = lines
                                                        .Where(x => x.ProductCode == c.ProductCode)
                                                        .Sum(s => s.CreditAmount),
                                TotalDebitAmmount = lines
                                                        .Where(x => x.ProductCode == c.ProductCode)
                                                        .Sum(s => s.DebitAmount),

                            }).ToList();

        }

        public static List<InvoiceLine> LoadInvoiceLines()
        {
            var invoiceLines = new List<InvoiceLine>();
            var audit = Globals.AuditFile;

            var invoices = audit.SourceDocuments
                            .SalesInvoices
                            .Invoice;
                            //.Where(x => x.DocumentStatus.InvoiceStatus.Equals("N"));

            foreach (var invoice in invoices)
            {
                foreach (var line in invoice.Line)
                {
                    {
                        var tp = float.Parse(line.Tax.TaxPercentage.Replace(".", ","));
                        var invoiceLine = new InvoiceLine
                        {
                            InvoiceNo = invoice.InvoiceNo,
                            InvoiceDate = invoice.InvoiceDate,
                            InvoiceType = invoice.InvoiceType,
                            InvoiceStatus = invoice.DocumentStatus.InvoiceStatus,
                            CustomerTaxID = audit.MasterFiles
                                                .Customer
                                                .Where(x => x.CustomerID.Equals(invoice.CustomerID))
                                                .FirstOrDefault()
                                                .CustomerTaxID,
                            CompanyName = audit.MasterFiles
                                                .Customer
                                                .Where(x => x.CustomerID.Equals(invoice.CustomerID))
                                                .FirstOrDefault()
                                                .CompanyName,
                            LineNumber = line.LineNumber,
                            ProductCode = line.ProductCode,
                            ProductDescription = line.ProductDescription,
                            Quantity = float.Parse(line.Quantity.Replace(".", ",")),
                            UnitPrice = float.Parse(line.UnitPrice.Replace(".", ",")),
                            TaxCode = line.Tax.TaxCode,
                            TaxPercentage = tp
                        };
                        if (line.CreditAmount != null)
                        {
                            var ca = float.Parse(line.CreditAmount.Replace(".", ","));
                            invoiceLine.CreditAmount = ca;
                            invoiceLine.CreditTaxPayable = ca * (tp / 100);
                        }
                        if (line.DebitAmount != null)
                        {
                            var da = float.Parse(line.DebitAmount.Replace(".", ","));
                            invoiceLine.DebitAmount = da;
                            invoiceLine.DebitTaxPayable = da * (tp / 100);
                        }
                        invoiceLines.Add(invoiceLine);
                    }
                }
            }

            return invoiceLines;
        }

        public static List<InvoiceEntry> LoadInvoiLoadDocuments()
        {
            var audit = Globals.AuditFile;
            return audit
                    .SourceDocuments
                    .SalesInvoices
                    .Invoice
                    .Select(i => new InvoiceEntry
                    {
                        InvoiceNo = i.InvoiceNo,
                        Period = i.Period,
                        InvoiceDate = i.InvoiceDate,
                        InvoiceType = i.InvoiceType,
                        SourceID = i.SourceID,
                        CustomerTaxID = audit.MasterFiles
                                                .Customer
                                                .Where(x => x.CustomerID.Equals(i.CustomerID))
                                                .FirstOrDefault()
                                                .CustomerTaxID,
                        CompanyName = audit.MasterFiles
                                                .Customer
                                                .Where(x => x.CustomerID.Equals(i.CustomerID))
                                                .FirstOrDefault()
                                                .CompanyName,
                        InvoiceStatus = i.DocumentStatus.InvoiceStatus,
                        TaxPayable = float.Parse(i.DocumentTotals.TaxPayable.Replace(".", ",")),
                        NetTotal = float.Parse(i.DocumentTotals.NetTotal.Replace(".", ",")),
                        GrossTotal = float.Parse(i.DocumentTotals.GrossTotal.Replace(".", ","))
                    }).ToList();
        }

        public static List<TaxTableEntryTotal> LoadTaxTableEntryTotals(List<InvoiceLine> invoiceLines)
        {
            var audit = Globals.AuditFile;

            var totals = invoiceLines
                .Where(s=>s.InvoiceStatus.Equals("N"))
                .GroupBy(g => new { g.TaxCode, g.TaxPercentage })
                .Select(cl => new TaxTableEntryTotal
                {
                    TaxCode = cl.First().TaxCode,
                    TaxDescription = audit
                                        .MasterFiles
                                        .TaxTable
                                        .TaxTableEntry
                                        .Where(x => x.TaxCode.Equals(cl.First().TaxCode))
                                        .FirstOrDefault()
                                        .Description,
                    TaxPercentage = cl.First().TaxPercentage,
                    CreditAmount = cl.Sum(c => c.CreditAmount),
                    DebitAmount = cl.Sum(d => d.DebitAmount),
                    BalanceAmount = cl.Sum(c => c.CreditAmount) - cl.Sum(d => d.DebitAmount),
                    CreditTaxPayable = cl.Sum(c => c.CreditAmount) * (cl.First().TaxPercentage / 100),
                    DebitTaxPayable = cl.Sum(d => d.DebitAmount) * (cl.First().TaxPercentage / 100),
                    BalanceTaxPayable = cl.Sum(c => c.CreditAmount) * (cl.First().TaxPercentage / 100) - cl.Sum(d => d.DebitAmount) * (cl.First().TaxPercentage / 100)
                }).ToList();

            return totals;
        }

    }

    public class AttachedFile
    {
        public Guid ID { get; set; }
        public string FilePath { get; set; }
        public AuditFile AuditFile { get; set; }
        public bool IsPrincipal { get; set; }
    }
}
