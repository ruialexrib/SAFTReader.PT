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

        /// <summary>
        /// Gets the version label for the application.
        /// </summary>
        /// <returns>The version label in the format "Versão: {major}.{minor}.{build}.{revision} (NetworkDeployed/Debug)"</returns>
        public static string VersionLabel
        {
            get
            {
                var ver = ApplicationDeployment.IsNetworkDeployed
                    ? ApplicationDeployment.CurrentDeployment.CurrentVersion
                    : Assembly.GetExecutingAssembly().GetName().Version;

                var deploymentType = ApplicationDeployment.IsNetworkDeployed ? "(NetworkDeployed)" : "(Debug)";

                return $"Versão: {ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision} {deploymentType} {Assembly.GetEntryAssembly().GetName().Name}";
            }
        }

        /// <summary>
        /// Loads a list of customer entries based on audit data and associated invoice lines.
        /// </summary>
        /// <remarks>
        /// This method retrieves customer entries from the audit file's master files and calculates
        /// the total credit and debit amounts for each customer based on associated invoice lines.
        /// </remarks>
        /// <returns>A list of CustomerEntry objects representing customer information.</returns>
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

        /// <summary>
        /// Loads a list of tax entries based on audit data and associated invoice lines.
        /// </summary>
        /// <remarks>
        /// This method retrieves tax entries from the audit file's tax table and calculates
        /// the total credit and debit amounts for each tax entry based on associated invoice lines.
        /// </remarks>
        /// <returns>A list of TaxEntry objects representing tax information.</returns>
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
                                                        .Where(x => x.TaxCode == c.TaxCode && 
                                                                    x.TaxPercentage == (c.TaxPercentage ?? "0.00").ToFloat())
                                                        .Sum(s => s.CreditAmount),
                                TotalDebitAmmount = lines
                                                        .Where(x => x.TaxCode == c.TaxCode && 
                                                                    x.TaxPercentage == (c.TaxPercentage ?? "0.00").ToFloat())
                                                        .Sum(s => s.DebitAmount),
                            }).ToList();
        }

        /// <summary>
        /// Loads a list of account entries based on audit data.
        /// </summary>
        /// <remarks>
        /// This method retrieves account entries from the audit file's general ledger accounts
        /// and projects them into AccountEntry objects with relevant information.
        /// </remarks>
        /// <returns>A list of AccountEntry objects representing account information.</returns>
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

        /// <summary>
        /// Loads a list of product entries based on audit data and associated invoice lines.
        /// </summary>
        /// <remarks>
        /// This method retrieves product entries from the audit file's product data and calculates
        /// the total credit and debit amounts for each product based on associated invoice lines.
        /// </remarks>
        /// <returns>A list of ProductEntry objects representing product information.</returns>
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

        /// <summary>
        /// Loads transaction entries from the audit file and returns a list of TransactionEntry objects.
        /// </summary>
        /// <remarks>
        /// This method retrieves transaction data from the audit file and transforms it into TransactionEntry objects.
        /// It iterates through journals, transactions, and lines in the audit file to create TransactionEntry objects for each line.
        /// </remarks>
        /// <returns>A list of TransactionEntry objects representing the transaction entries.</returns>
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

        /// <summary>
        /// Loads a list of transaction entries based on audit data.
        /// </summary>
        /// <remarks>
        /// This method retrieves transaction entries from the audit file's general ledger entries,
        /// including debit and credit lines, and projects them into TransactionEntry objects.
        /// </remarks>
        /// <returns>A list of TransactionEntry objects representing transaction information.</returns>
        public static List<InvoiceLine> LoadInvoiceLines()
        {
            var invoiceLines = new List<InvoiceLine>();
            var audit = Globals.AuditFile;

            var invoices = audit
                            ?.SourceDocuments
                            ?.SalesInvoices
                            ?.Invoice;

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
                            TaxPercentage = tp,
                            TaxExemptionCode = line.TaxExemptionCode
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

        /// <summary>
        /// Loads a list of invoice entries based on audit data.
        /// </summary>
        /// <remarks>
        /// This method retrieves invoice entries from the audit file's source documents and
        /// projects them into InvoiceEntry objects with relevant information.
        /// </remarks>
        /// <returns>A list of InvoiceEntry objects representing invoice information.</returns>
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

        /// <summary>
        /// Loads tax entry totals from a list of invoice lines and returns a list of TaxTableEntryTotal objects.
        /// </summary>
        /// <remarks>
        /// This method calculates tax entry totals based on a list of invoice lines.
        /// It groups the lines by tax code and percentage, and then calculates the total credit and debit amounts, as well as tax payable amounts.
        /// </remarks>
        /// <param name="invoiceLines">The list of invoice lines to calculate tax entry totals from.</param>
        /// <returns>A list of TaxTableEntryTotal objects representing the tax entry totals.</returns>
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
                    TotalCredit = cl.Sum(c => c.CreditAmount) + cl.Sum(c => c.CreditTaxPayable),
                    CreditTaxPayable = cl.Sum(c => c.CreditTaxPayable),
                    DebitTaxPayable = cl.Sum(d => d.DebitTaxPayable),
                    TotalDebit = cl.Sum(d => d.DebitAmount) + cl.Sum(d => d.DebitTaxPayable),
                }).ToList();

            return totals;
        }
    }

    /// <summary>
    /// Represents an attached file associated with an audit file.
    /// </summary>
    public class AttachedFile
    {
        public Guid ID { get; set; }
        public string FilePath { get; set; }
        public AuditFile AuditFile { get; set; }
        public bool IsPrincipal { get; set; }
    }
}