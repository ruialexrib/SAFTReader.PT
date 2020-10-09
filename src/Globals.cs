using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Threading;

using Programatica.Saft.Models;

using SAFT_Reader.Extensions;
using SAFT_Reader.Models;

namespace SAFT_Reader
{
    public static class Globals
    {
        public static AuditFile AuditFile { get; set; }

        public static List<AttachedFile> AttachedFiles { get; set; }

        public const string LightColumnColor = "#ebebe0";
        public const string DarkColumnColor = "#ccccb3";

        public static char NumberDecimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

        public static string VersionLabel
        {
            get
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
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

        public static List<CustomerEntry> LoadCustomerEntries()
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
                                                        .Where(x => x.CustomerTaxID == c.CustomerTaxID)
                                                        .Sum(s => s.CreditAmount),
                                TotalDebitAmmount = lines
                                                        .Where(x => x.CustomerTaxID == c.CustomerTaxID)
                                                        .Sum(s => s.DebitAmount),
                            }).ToList();
        }

        public static List<TaxEntry> LoadTaxEntries()
        {
            var audit = Globals.AuditFile;
            var lines = LoadInvoiceLines();

            return audit.MasterFiles
                            .TaxTable
                            .TaxTableEntry
                            .Select(c => new TaxEntry
                            {
                                TaxType = c.TaxType,
                                TaxCountryRegion = c.TaxCountryRegion,
                                TaxCode = c.TaxCode,
                                Description = c.Description,
                                TaxPercentage = (c.TaxPercentage ?? "0.00").ToFloat(),
                                TotalCreditAmmount = lines
                                                        .Where(x => x.TaxCode == c.TaxCode && x.TaxPercentage == (c.TaxPercentage ?? "0.00").ToFloat())
                                                        .Sum(s => s.CreditAmount),
                                TotalDebitAmmount = lines
                                                        .Where(x => x.TaxCode == c.TaxCode && x.TaxPercentage == (c.TaxPercentage ?? "0.00").ToFloat())
                                                        .Sum(s => s.DebitAmount),
                            }).ToList();
        }

        public static List<AccountEntry> LoadAccountEntries()
        {
            var audit = Globals.AuditFile;

            return audit.MasterFiles
                                ?.GeneralLedgerAccounts
                                ?.Account
                                ?.Select(c => new AccountEntry
                                {
                                    AccountID = c.AccountID,
                                    AccountDescription = c.AccountDescription,
                                    OpeningDebitBalance = c.OpeningDebitBalance.ToFloat(),
                                    OpeningCreditBalance = c.OpeningCreditBalance.ToFloat(),
                                    ClosingDebitBalance = c.ClosingDebitBalance.ToFloat(),
                                    ClosingCreditBalance = c.ClosingCreditBalance.ToFloat(),
                                    GroupingCategory = c.GroupingCategory.ToAccountGroupCatDesc(),
                                    GroupingCode = c.GroupingCode,
                                    TaxonomyCode = c.TaxonomyCode
                                }).ToList() ?? new List<AccountEntry>();
        }

        public static List<ProductEntry> LoadProductEntries()
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

        public static List<TransactionEntry> LoadTransactionEntries()
        {
            var transactionlines = new List<TransactionEntry>();
            var audit = Globals.AuditFile;

            var journals = audit
                                ?.GeneralLedgerEntries
                                ?.Journal;

            foreach (var j in journals ?? new List<Journal>())
            {
                foreach (var t in j.Transaction ?? new List<Transaction>())
                {
                    foreach (var l in t?.Lines?.DebitLine ?? new List<DebitLine>())
                    {
                        var line = new TransactionEntry
                        {
                            JournalID = j.JournalID,
                            JournalDescription = j.Description,
                            TransactionID = t.TransactionID,
                            Period = t.Period,
                            TransactionDate = DateTime.Parse(t.TransactionDate),
                            SourceID = t.SourceID,
                            TransactionDescription = t.Description,
                            DocArchivalNumber = t.DocArchivalNumber,
                            TransactionType = t.TransactionType,
                            CustomerID = t.CustomerID,
                            AccountID = l.AccountID,
                            LineDescription = l.Description,
                            DebitAmount = l.DebitAmount.ToFloat()
                        };
                        transactionlines.Add(line);
                    }
                    foreach (var l in t?.Lines?.CreditLine ?? new List<CreditLine>())
                    {
                        var line = new TransactionEntry
                        {
                            JournalID = j.JournalID,
                            JournalDescription = j.Description,
                            TransactionID = t.TransactionID,
                            Period = t.Period,
                            TransactionDate = DateTime.Parse(t.TransactionDate),
                            SourceID = t.SourceID,
                            TransactionDescription = t.Description,
                            DocArchivalNumber = t.DocArchivalNumber,
                            TransactionType = t.TransactionType,
                            CustomerID = t.CustomerID,
                            AccountID = l.AccountID,
                            LineDescription = l.Description,
                            CreditAmount = l.CreditAmount.ToFloat()
                        };
                        transactionlines.Add(line);
                    }
                }
            }
            return transactionlines;
        }

        public static List<InvoiceLine> LoadInvoiceLines()
        {
            var invoiceLines = new List<InvoiceLine>();
            var audit = Globals.AuditFile;

            var invoices = audit
                            ?.SourceDocuments
                            ?.SalesInvoices
                            ?.Invoice;
            //.Where(x => x.DocumentStatus.InvoiceStatus.Equals("N"));

            foreach (var invoice in invoices ?? new List<Invoice>())
            {
                foreach (var line in invoice.Line)
                {
                    {
                        var tp = line.Tax.TaxPercentage.ToFloat();
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
                            Quantity = line.Quantity.ToFloat(),
                            UnitPrice = line.UnitPrice.ToFloat(),
                            TaxCode = line.Tax.TaxCode,
                            TaxPercentage = tp
                        };
                        if (line.CreditAmount != null)
                        {
                            var ca = line.CreditAmount;
                            invoiceLine.CreditAmount = ca.ToFloat();
                            invoiceLine.CreditTaxPayable = (ca.ToFloat(false) * (tp / 100)).Round();
                        }
                        if (line.DebitAmount != null)
                        {
                            var da = line.DebitAmount;
                            invoiceLine.DebitAmount = da.ToFloat();
                            invoiceLine.DebitTaxPayable = (da.ToFloat(true) * (tp / 100)).Round();
                        }
                        invoiceLines.Add(invoiceLine);
                    }
                }
            }

            return invoiceLines;
        }

        public static List<InvoiceEntry> LoadInvoiceEntries()
        {
            var audit = Globals.AuditFile;
            return audit
                    ?.SourceDocuments
                    ?.SalesInvoices
                    ?.Invoice
                    ?.Select(i => new InvoiceEntry
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
                        TaxPayable = i.DocumentTotals.TaxPayable.ToFloat(),
                        NetTotal = i.DocumentTotals.NetTotal.ToFloat(),
                        GrossTotal = i.DocumentTotals.GrossTotal.ToFloat()
                    }).ToList() ?? new List<InvoiceEntry>();
        }

        public static List<TaxTableEntryTotal> LoadTaxEntryTotals(List<InvoiceLine> invoiceLines)
        {
            var audit = Globals.AuditFile;

            var totals = invoiceLines
                .Where(s => s.InvoiceStatus.Equals("N"))
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
                    CreditTaxPayable = cl.Sum(c => c.CreditTaxPayable),
                    DebitTaxPayable = cl.Sum(d => d.DebitTaxPayable),
                    BalanceTaxPayable = cl.Sum(c => c.CreditTaxPayable) - cl.Sum(d => d.DebitTaxPayable)
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