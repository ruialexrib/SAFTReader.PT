using Programatica.Saft.Models;
using SAFT_Reader.Models;
using Syncfusion.Data;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGridConverter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SAFT_Reader.UI
{
    public partial class SAFTTotalsForm : RibbonForm
    {
        public SfDataGrid SelectedGrid { get; set; }

        public SAFTTotalsForm()
        {
            InitializeComponent();
            InitializeView();
        }

        private void InitializeView()
        {
            this.Text = $"{this.Text} [{Globals.Filepath}]";
            this.lblInfoApp.Text = $"{Globals.VersionLabel} - Copyright 2020, Rui Ribeiro. Todos os direitos reservados.";
            SelectedGrid = gridLines;
        }

        private void TaxTableEntryTotalForm_Load(object sender, EventArgs e)
        {
            var audit = Globals.AuditFile;
            var invoiceLines = LoadInvoiceLines();
            var totals = LoadTaxTableEntryTotals(invoiceLines);
            var invoices = LoadInvoiLoadDocuments();
            var customers = LoadCustomers();
            var products = LoadProducts();
            var tax = LoadTax();
            //var accounts = LoadAccounts();

            propertyGrid1.SelectedObject = audit.Header;
            propertyGrid2.SelectedObject = audit.SourceDocuments.SalesInvoices;
            gridLines.DataSource = invoiceLines;
            gridTotals.DataSource = totals;
            gridDocuments.DataSource = invoices;
            gridCustomers.DataSource = customers;
            gridProducts.DataSource = products;
            gridTax.DataSource = tax;
            //gridAccounts.DataSource = accounts;

            SetGridTotalsSummaries();
            SetGridLinesGroupSummaries();
            SetGridLinesSummaries();
            SetGridDocumentsSummaries();
        }

        private List<Customer> LoadCustomers()
        {
            var audit = Globals.AuditFile;
            return audit.MasterFiles.Customer;
        }

        private List<Product> LoadProducts()
        {
            var audit = Globals.AuditFile;
            return audit.MasterFiles.Product;
        }

        private List<TaxTableEntry> LoadTax()
        {
            var audit = Globals.AuditFile;
            return audit.MasterFiles.TaxTable.TaxTableEntry;
        }

        //private List<Account> LoadAccounts()
        //{
        //    var audit = Globals.AuditFile;
        //    if (audit.MasterFiles.GeneralLedgerAccounts.Account != null) { 
        //    return audit.MasterFiles.GeneralLedgerAccounts.Account;
        //    }
        //    else { return new List<Account>(); }
        //}

        private List<InvoiceEntry> LoadInvoiLoadDocuments()
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
                        CustomerID = i.CustomerID,
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

        private List<TaxTableEntryTotal> LoadTaxTableEntryTotals(List<InvoiceLine> invoiceLines)
        {
            var audit = Globals.AuditFile;

            var totals = invoiceLines
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
                    CreditTaxPayable = cl.Sum(c => c.CreditAmount) * (cl.First().TaxPercentage / 100),
                    DebitTaxPayable = cl.Sum(d => d.DebitAmount) * (cl.First().TaxPercentage / 100)
                }).ToList();

            return totals;
        }

        private List<InvoiceLine> LoadInvoiceLines()
        {
            var invoiceLines = new List<InvoiceLine>();
            var audit = Globals.AuditFile;

            var invoices = audit.SourceDocuments
                            .SalesInvoices
                            .Invoice
                            .Where(x => x.DocumentStatus.InvoiceStatus.Equals("N"));

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
                            invoiceLine.TaxPayable = ca * (tp / 100);
                        }
                        if (line.DebitAmount != null)
                        {
                            var da = float.Parse(line.DebitAmount.Replace(".", ","));
                            invoiceLine.DebitAmount = da;
                            invoiceLine.TaxPayable = da * (tp / 100);
                        }
                        invoiceLines.Add(invoiceLine);
                    }
                }
            }

            return invoiceLines;
        }

        private void SetGridTotalsSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow();
            sum.ShowSummaryInRow = false;
            sum.TitleColumnCount = 1;
            sum.Position = VerticalPosition.Bottom;
            sum.Title = "Totais";

            GridSummaryColumn ca = new GridSummaryColumn();
            ca.Name = "CreditAmount";
            ca.Format = "{Sum:c}";
            ca.MappingName = "CreditAmount";
            ca.SummaryType = SummaryType.DoubleAggregate;

            GridSummaryColumn da = new GridSummaryColumn();
            da.Name = "DebitAmount";
            da.Format = "{Sum:c}";
            da.MappingName = "DebitAmount";
            da.SummaryType = SummaryType.DoubleAggregate;

            GridSummaryColumn ctp = new GridSummaryColumn();
            ctp.Name = "CreditTaxPayable";
            ctp.Format = "{Sum:c}";
            ctp.MappingName = "CreditTaxPayable";
            ctp.SummaryType = SummaryType.DoubleAggregate;

            GridSummaryColumn dtp = new GridSummaryColumn();
            dtp.Name = "DebitTaxPayable";
            dtp.Format = "{Sum:c}";
            dtp.MappingName = "DebitTaxPayable";
            dtp.SummaryType = SummaryType.DoubleAggregate;

            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);
            sum.SummaryColumns.Add(ctp);
            sum.SummaryColumns.Add(dtp);

            this.gridTotals.TableSummaryRows.Add(sum);
        }

        private void SetGridLinesSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow();
            sum.ShowSummaryInRow = false;
            sum.TitleColumnCount = 1;
            sum.Position = VerticalPosition.Bottom;
            sum.Title = "Totais";

            GridSummaryColumn ca = new GridSummaryColumn();
            ca.Name = "CreditAmount";
            ca.Format = "{Sum:c}";
            ca.MappingName = "CreditAmount";
            ca.SummaryType = SummaryType.DoubleAggregate;

            GridSummaryColumn da = new GridSummaryColumn();
            da.Name = "DebitAmount";
            da.Format = "{Sum:c}";
            da.MappingName = "DebitAmount";
            da.SummaryType = SummaryType.DoubleAggregate;

            GridSummaryColumn tp = new GridSummaryColumn();
            tp.Name = "TaxPayable";
            tp.Format = "{Sum:c}";
            tp.MappingName = "TaxPayable";
            tp.SummaryType = SummaryType.DoubleAggregate;

            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);
            sum.SummaryColumns.Add(tp);

            this.gridLines.TableSummaryRows.Add(sum);
        }

        private void SetGridLinesGroupSummaries()
        {
            GridSummaryRow sum = new GridSummaryRow();
            sum.Name = "GroupSummary";
            sum.ShowSummaryInRow = false;
            sum.Title = "Sub-Totais";

            GridSummaryColumn ca = new GridSummaryColumn();
            ca.Name = "CreditAmount";
            ca.SummaryType = SummaryType.DoubleAggregate;
            ca.Format = "{Sum:c}";
            ca.MappingName = "CreditAmount";

            GridSummaryColumn da = new GridSummaryColumn();
            da.Name = "DebitAmount";
            da.SummaryType = SummaryType.DoubleAggregate;
            da.Format = "{Sum:c}";
            da.MappingName = "DebitAmount";

            GridSummaryColumn tp = new GridSummaryColumn();
            tp.Name = "TaxPayable";
            tp.SummaryType = SummaryType.DoubleAggregate;
            tp.Format = "{Sum:c}";
            tp.MappingName = "TaxPayable";

            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);
            sum.SummaryColumns.Add(tp);

            this.gridLines.GroupSummaryRows.Add(sum);
        }

        private void SetGridDocumentsSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow();
            sum.ShowSummaryInRow = false;
            sum.TitleColumnCount = 1;
            sum.Position = VerticalPosition.Bottom;
            sum.Title = "Totais";

            GridSummaryColumn tp = new GridSummaryColumn();
            tp.Name = "TaxPayable";
            tp.Format = "{Sum:c}";
            tp.MappingName = "TaxPayable";
            tp.SummaryType = SummaryType.DoubleAggregate;

            GridSummaryColumn nt = new GridSummaryColumn();
            nt.Name = "NetTotal";
            nt.Format = "{Sum:c}";
            nt.MappingName = "NetTotal";
            nt.SummaryType = SummaryType.DoubleAggregate;

            GridSummaryColumn gt = new GridSummaryColumn();
            gt.Name = "GrossTotal";
            gt.Format = "{Sum:c}";
            gt.MappingName = "GrossTotal";
            gt.SummaryType = SummaryType.DoubleAggregate;


            sum.SummaryColumns.Add(tp);
            sum.SummaryColumns.Add(nt);
            sum.SummaryColumns.Add(gt);

            this.gridDocuments.TableSummaryRows.Add(sum);
        }



        private void GridEnter(object sender, EventArgs e)
        {
            SelectedGrid = (SfDataGrid)sender;
        }

        private void cmdToolGroup_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SelectedGrid.ShowGroupDropArea = !SelectedGrid.ShowGroupDropArea;
            Cursor.Current = Cursors.Default;
        }

        private void cmdToolAutoExpand_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SelectedGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            Cursor.Current = Cursors.Default;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SelectedGrid.SearchController.AllowFiltering = true;
            SelectedGrid.SearchController.Search(txtToolFilter.Text);
            Cursor.Current = Cursors.Default;
        }

        private void cmdToolRemoveFilters_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SelectedGrid.ClearFilters();
            SelectedGrid.SearchController.ClearSearch();
            SelectedGrid.SearchController.AllowFiltering = false;
            txtToolFilter.Text = string.Empty;
            Cursor.Current = Cursors.Default;
        }


        private void tabControlAdv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var index = tabControlAdv1.SelectedIndex;
            switch (index)
            {
                case 0:
                    gridLines.Focus();
                    break;
                case 1:
                    gridDocuments.Focus();
                    break;
                case 2:
                    gridCustomers.Focus();
                    break;
                case 3:
                    gridProducts.Focus();
                    break;
                case 4:
                    gridTax.Focus();
                    break;
                case 5:
                    gridAccounts.Focus();
                    break;
                default:
                    break;
            }
            Cursor.Current = Cursors.Default;
        }

        private void lblInfoGithub_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ToolStripStatusLabel tssl = (ToolStripStatusLabel)sender;
            string url;
            url = tssl.Text;
            if (!url.Contains("://"))
            {
                url = "https://" + url;
            }
            var si = new ProcessStartInfo(url);
            Process.Start(si);
            Cursor.Current = Cursors.Default;
        }

        private void txtToolFilter_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.KeyCode == Keys.Enter)
            {
                SelectedGrid.SearchController.AllowFiltering = true;
                SelectedGrid.SearchController.Search(txtToolFilter.Text);
            }
            Cursor.Current = Cursors.Default;
        }

        private void cmdToolExportXLS_Click(object sender, EventArgs e)
        {
            var options = new ExcelExportingOptions();
            var excelEngine = SelectedGrid.ExportToExcel(SelectedGrid.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xlsx File|*.xlsx|Ficheiro Excel|*.xlsx";
            sfd.Title = "Guardar ficheiro Excel";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workBook.SaveAs(sfd.FileName);
                Process.Start(sfd.FileName);
            }
            Cursor.Current = Cursors.Default;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PdfExportingOptions options = new PdfExportingOptions();
            options.AutoColumnWidth = true;
            var document = SelectedGrid.ExportToPdf(options);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pdf Image|*.pdf|Ficheiro pdf|*.pdf";
            sfd.Title = "Guardar ficheiro pdf";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                document.Save(sfd.FileName);
                Process.Start(sfd.FileName);
            }
            Cursor.Current = Cursors.Default;
        }

        private void cmdToolExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
    }
}