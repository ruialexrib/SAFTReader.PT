using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using SAFT_Reader.Extensions;
using SAFT_Reader.Models;

using Syncfusion.Data;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGridConverter;

namespace SAFT_Reader.UI
{
    public partial class MainForm : RibbonForm
    {
        public SfDataGrid SelectedGrid { get; set; }
        private readonly WaitingForm wf = new WaitingForm();

        public MainForm()
        {
            InitializeComponent();
            InitializeView();
        }

        private void InitializeView()
        {
            ribbonControlAdv1.Size = new Size { Width = this.ribbonControlAdv1.Size.Width, Height = 150 };
            this.lblInfoApp.Text = $"{Globals.VersionLabel}";
            SelectedGrid = gridLines;
        }

        private void LoadGrids()
        {
            SetWaitingMsg("A carregar ficheiro");
            var audit = Globals.AuditFile;
            SetWaitingMsg("A carregar as linhas das faturas...");
            var invoiceLines = Globals.LoadInvoiceLines();
            SetWaitingMsg("A carregar impostos das faturas...");
            var totals = Globals.LoadTaxEntryTotals(invoiceLines);
            SetWaitingMsg("A carregar faturas...");
            var invoices = Globals.LoadInvoiceEntries();
            SetWaitingMsg("A carregar clientes...");
            var customers = Globals.LoadCustomerEntries();
            SetWaitingMsg("A carregar produtos...");
            var products = Globals.LoadProductEntries();
            SetWaitingMsg("A carregar impostos...");
            var tax = Globals.LoadTaxEntries();
            SetWaitingMsg("A carregar plano de contas...");
            var accounts = Globals.LoadAccountEntries();
            SetWaitingMsg("A carregar outras transações...");
            var trans = Globals.LoadTransactionEntries();

            // set grid datasources
            SetWaitingMsg("A preparar listas...");
            gridLines.DataSource = invoiceLines;
            gridTotals.DataSource = totals;
            gridDocuments.DataSource = invoices;
            gridCustomers.DataSource = customers;
            gridProducts.DataSource = products;
            gridTax.DataSource = tax;
            gridAccounts.DataSource = accounts;
            gridTransactions.DataSource = trans;

            // set grid summaries
            SetWaitingMsg("A preparar resumos...");
            SetGridTotalsSummaries();
            SetGridLinesGroupColumnSummaries();
            SetGridLinesSummaries();
            SetGridDocumentsTableSummaries();
            SetGridCustomerTableSummaries();
            SetGridProductTableSummaries();
            SetGridTaxTableSummaries();
            SetGridAccountsTableSummaries();
            SetGridTransactionsTableSummaries();

            SetWaitingMsg("A acabar apresentação dos dados...");
            // format gridTotals
            gridTotals.Columns["TaxCode"].CellStyle.Font.Bold = true;
            gridTotals.Columns["BalanceAmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridTotals.Columns["BalanceAmount"].CellStyle.Font.Bold = true;
            gridTotals.Columns["BalanceTaxPayable"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.DarkColumnColor);
            gridTotals.Columns["BalanceTaxPayable"].CellStyle.Font.Bold = true;

            // format gridLines
            gridLines.Columns["TaxCode"].CellStyle.Font.Bold = true;
            gridLines.Columns["InvoiceNo"].CellStyle.Font.Bold = true;
            gridLines.Columns["CreditTaxPayable"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.DarkColumnColor);
            gridLines.Columns["CreditTaxPayable"].CellStyle.Font.Bold = true;
            gridLines.Columns["DebitTaxPayable"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.DarkColumnColor);
            gridLines.Columns["DebitTaxPayable"].CellStyle.Font.Bold = true;
            gridLines.Columns["CreditAmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridLines.Columns["CreditAmount"].CellStyle.Font.Bold = true;
            gridLines.Columns["DebitAmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridLines.Columns["DebitAmount"].CellStyle.Font.Bold = true;
            gridLines.Columns["ProductCode"].Width = 70;
            gridLines.Columns["LineNumber"].Width = 50;
            gridLines.Columns["InvoiceType"].Width = 50;
            gridLines.Columns["InvoiceStatus"].Width = 50;
            gridLines.Columns["TaxCode"].Width = 50;
            gridLines.Columns["TaxPercentage"].Width = 50;
            gridLines.Columns["Quantity"].Width = 50;

            // format gridDocuments
            gridDocuments.Columns["InvoiceNo"].CellStyle.Font.Bold = true;
            gridDocuments.Columns["NetTotal"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.DarkColumnColor);
            gridDocuments.Columns["NetTotal"].CellStyle.Font.Bold = true;
            gridDocuments.Columns["TaxPayable"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridDocuments.Columns["TaxPayable"].CellStyle.Font.Bold = true;
            gridDocuments.Columns["GrossTotal"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.DarkColumnColor);
            gridDocuments.Columns["GrossTotal"].CellStyle.Font.Bold = true;

            // format gridCustomers
            gridCustomers.Columns["CustomerID"].CellStyle.Font.Bold = true;
            gridCustomers.Columns["CompanyName"].CellStyle.Font.Bold = true;
            gridCustomers.Columns["TotalCreditAmmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridCustomers.Columns["TotalCreditAmmount"].CellStyle.Font.Bold = true;
            gridCustomers.Columns["TotalDebitAmmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridCustomers.Columns["TotalDebitAmmount"].CellStyle.Font.Bold = true;

            // format gridProducts
            gridProducts.Columns["ProductCode"].CellStyle.Font.Bold = true;
            gridProducts.Columns["TotalCreditAmmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridProducts.Columns["TotalCreditAmmount"].CellStyle.Font.Bold = true;
            gridProducts.Columns["TotalDebitAmmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridProducts.Columns["TotalDebitAmmount"].CellStyle.Font.Bold = true;

            // format gridTax
            gridTax.Columns["TaxCode"].CellStyle.Font.Bold = true;
            gridTax.Columns["TotalCreditAmmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridTax.Columns["TotalCreditAmmount"].CellStyle.Font.Bold = true;
            gridTax.Columns["TotalDebitAmmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridTax.Columns["TotalDebitAmmount"].CellStyle.Font.Bold = true;

            // format gridAccounts
            gridAccounts.Columns["AccountID"].CellStyle.Font.Bold = true;

            // format gridTransactions
            gridTransactions.Columns["TransactionID"].CellStyle.Font.Bold = true;
            gridTransactions.Columns["CreditAmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridTransactions.Columns["CreditAmount"].CellStyle.Font.Bold = true;
            gridTransactions.Columns["DebitAmount"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridTransactions.Columns["DebitAmount"].CellStyle.Font.Bold = true;

            SetWaitingMsg("Pronto!");
        }

        private void LoadAuditHeaderPropertyGrids()
        {
            foreach (var file in Globals.AttachedFiles)
            {
                var pg = new PropertyGrid();
                var tab = new TabPageAdv();

                pg.SelectedObject = file.AuditFile.ToHeaderPt();
                pg.Dock = DockStyle.Fill;
                pg.HelpVisible = false;
                pg.ToolbarVisible = false;
                tab.Text = $"{System.IO.Path.GetFileName(file.FilePath)}".ToUpper();
                tab.ImageIndex = 0;
                tab.Controls.Add(pg);

                tabControlAdv3.TabPages.Add(tab);
            }
        }

        private void SetGridTotalsSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow
            {
                ShowSummaryInRow = false,
                TitleColumnCount = 1,
                Position = VerticalPosition.Bottom,
                Title = "Totais"
            };

            GridSummaryColumn ca = new GridSummaryColumn
            {
                Name = "CreditAmount",
                Format = "{Sum:c}",
                MappingName = "CreditAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn da = new GridSummaryColumn
            {
                Name = "DebitAmount",
                Format = "{Sum:c}",
                MappingName = "DebitAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn ba = new GridSummaryColumn
            {
                Name = "BalanceAmount",
                Format = "{Sum:c}",
                MappingName = "BalanceAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn ctp = new GridSummaryColumn
            {
                Name = "CreditTaxPayable",
                Format = "{Sum:c}",
                MappingName = "CreditTaxPayable",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn dtp = new GridSummaryColumn
            {
                Name = "DebitTaxPayable",
                Format = "{Sum:c}",
                MappingName = "DebitTaxPayable",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn btp = new GridSummaryColumn
            {
                Name = "BalanceTaxPayable",
                Format = "{Sum:c}",
                MappingName = "BalanceTaxPayable",
                SummaryType = SummaryType.DoubleAggregate
            };

            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);
            sum.SummaryColumns.Add(ba);
            sum.SummaryColumns.Add(ctp);
            sum.SummaryColumns.Add(dtp);
            sum.SummaryColumns.Add(btp);

            this.gridTotals.TableSummaryRows.Add(sum);
        }

        private void SetGridLinesSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow
            {
                ShowSummaryInRow = false,
                TitleColumnCount = 1,
                Position = VerticalPosition.Bottom,
                Title = "Totais"
            };

            GridSummaryColumn ca = new GridSummaryColumn
            {
                Name = "CreditAmount",
                Format = "{Sum:c}",
                MappingName = "CreditAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn da = new GridSummaryColumn
            {
                Name = "DebitAmount",
                Format = "{Sum:c}",
                MappingName = "DebitAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn ctp = new GridSummaryColumn
            {
                Name = "CreditTaxPayable",
                Format = "{Sum:c}",
                MappingName = "CreditTaxPayable",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn dtp = new GridSummaryColumn
            {
                Name = "DebitTaxPayable",
                Format = "{Sum:c}",
                MappingName = "DebitTaxPayable",
                SummaryType = SummaryType.DoubleAggregate
            };

            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);
            sum.SummaryColumns.Add(ctp);
            sum.SummaryColumns.Add(dtp);

            this.gridLines.TableSummaryRows.Add(sum);
        }

        private void SetGridCustomerTableSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow
            {
                ShowSummaryInRow = false,
                TitleColumnCount = 1,
                Position = VerticalPosition.Bottom,
                Title = "Totais"
            };

            GridSummaryColumn ca = new GridSummaryColumn
            {
                Name = "TotalCreditAmmount",
                Format = "{Sum:c}",
                MappingName = "TotalCreditAmmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn da = new GridSummaryColumn
            {
                Name = "TotalDebitAmmount",
                Format = "{Sum:c}",
                MappingName = "TotalDebitAmmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);

            this.gridCustomers.TableSummaryRows.Add(sum);
        }

        private void SetGridProductTableSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow
            {
                ShowSummaryInRow = false,
                TitleColumnCount = 1,
                Position = VerticalPosition.Bottom,
                Title = "Totais"
            };

            GridSummaryColumn ca = new GridSummaryColumn
            {
                Name = "TotalCreditAmmount",
                Format = "{Sum:c}",
                MappingName = "TotalCreditAmmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn da = new GridSummaryColumn
            {
                Name = "TotalDebitAmmount",
                Format = "{Sum:c}",
                MappingName = "TotalDebitAmmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);

            this.gridProducts.TableSummaryRows.Add(sum);
        }

        private void SetGridTaxTableSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow
            {
                ShowSummaryInRow = false,
                TitleColumnCount = 1,
                Position = VerticalPosition.Bottom,
                Title = "Totais"
            };

            GridSummaryColumn ca = new GridSummaryColumn
            {
                Name = "TotalCreditAmmount",
                Format = "{Sum:c}",
                MappingName = "TotalCreditAmmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn da = new GridSummaryColumn
            {
                Name = "TotalDebitAmmount",
                Format = "{Sum:c}",
                MappingName = "TotalDebitAmmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);

            this.gridTax.TableSummaryRows.Add(sum);
        }

        private void SetGridAccountsTableSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow
            {
                ShowSummaryInRow = false,
                TitleColumnCount = 1,
                Position = VerticalPosition.Bottom,
                Title = "Totais"
            };

            GridSummaryColumn odb = new GridSummaryColumn
            {
                Name = "OpeningDebitBalance",
                Format = "{Sum:c}",
                MappingName = "OpeningDebitBalance",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn ocb = new GridSummaryColumn
            {
                Name = "OpeningCreditBalance",
                Format = "{Sum:c}",
                MappingName = "OpeningCreditBalance",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn cdb = new GridSummaryColumn
            {
                Name = "ClosingDebitBalance",
                Format = "{Sum:c}",
                MappingName = "ClosingDebitBalance",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn ccb = new GridSummaryColumn
            {
                Name = "ClosingCreditBalance",
                Format = "{Sum:c}",
                MappingName = "ClosingCreditBalance",
                SummaryType = SummaryType.DoubleAggregate
            };

            sum.SummaryColumns.Add(odb);
            sum.SummaryColumns.Add(ocb);
            sum.SummaryColumns.Add(cdb);
            sum.SummaryColumns.Add(ccb);

            this.gridAccounts.TableSummaryRows.Add(sum);
        }

        private void SetGridLinesGroupColumnSummaries()
        {
            // Creates the GridSummaryRow.
            GridSummaryRow captionSummaryRow = new GridSummaryRow
            {
                Name = "CaptionSummary",
                ShowSummaryInRow = false,
                TitleColumnCount = 3,
                Title = "{ColumnName} = {Key} ({ItemsCount} Registos)"
            };

            GridSummaryColumn summaryColumn1 = new GridSummaryColumn
            {
                Name = "CreditAmount",
                Format = "{Sum:c}",
                MappingName = "CreditAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn summaryColumn2 = new GridSummaryColumn
            {
                Name = "DebitAmount",
                Format = "{Sum:c}",
                MappingName = "DebitAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn summaryColumn3 = new GridSummaryColumn
            {
                Name = "CreditTaxPayable",
                Format = "{Sum:c}",
                MappingName = "CreditTaxPayable",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn summaryColumn4 = new GridSummaryColumn
            {
                Name = "DebitTaxPayable",
                Format = "{Sum:c}",
                MappingName = "DebitTaxPayable",
                SummaryType = SummaryType.DoubleAggregate
            };

            // Adds the summary column in the SummaryColumns collection.
            captionSummaryRow.SummaryColumns.Add(summaryColumn1);
            captionSummaryRow.SummaryColumns.Add(summaryColumn2);
            captionSummaryRow.SummaryColumns.Add(summaryColumn3);
            captionSummaryRow.SummaryColumns.Add(summaryColumn4);

            // Initializes the caption summary row.
            this.gridLines.CaptionSummaryRow = captionSummaryRow;
        }

        private void SetGridDocumentsTableSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow
            {
                ShowSummaryInRow = false,
                TitleColumnCount = 1,
                Position = VerticalPosition.Bottom,
                Title = "Totais"
            };

            GridSummaryColumn tp = new GridSummaryColumn
            {
                Name = "TaxPayable",
                Format = "{Sum:c}",
                MappingName = "TaxPayable",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn nt = new GridSummaryColumn
            {
                Name = "NetTotal",
                Format = "{Sum:c}",
                MappingName = "NetTotal",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn gt = new GridSummaryColumn
            {
                Name = "GrossTotal",
                Format = "{Sum:c}",
                MappingName = "GrossTotal",
                SummaryType = SummaryType.DoubleAggregate
            };

            sum.SummaryColumns.Add(tp);
            sum.SummaryColumns.Add(nt);
            sum.SummaryColumns.Add(gt);

            this.gridDocuments.TableSummaryRows.Add(sum);
        }

        private void SetGridTransactionsTableSummaries()
        {
            GridTableSummaryRow sum = new GridTableSummaryRow
            {
                ShowSummaryInRow = false,
                TitleColumnCount = 1,
                Position = VerticalPosition.Bottom,
                Title = "Totais"
            };

            GridSummaryColumn cre = new GridSummaryColumn
            {
                Name = "CreditAmmount",
                Format = "{Sum:c}",
                MappingName = "CreditAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn deb = new GridSummaryColumn
            {
                Name = "DebitAmmount",
                Format = "{Sum:c}",
                MappingName = "DebitAmount",
                SummaryType = SummaryType.DoubleAggregate
            };

            sum.SummaryColumns.Add(cre);
            sum.SummaryColumns.Add(deb);

            this.gridTransactions.TableSummaryRows.Add(sum);
        }

        #region Events

        private void SetWaitingMsg(string msg)
        {
            if (wf != null)
            {
                wf.SetMsg(msg);
            }
        }

        private void TaxTableEntryTotalForm_Load(object sender, EventArgs e)
        {
            wf.Show();

            LoadGrids();
            LoadAuditHeaderPropertyGrids();
            mainSplitter.Visible = true;
            SelectedGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            gridDocuments.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            gridCustomers.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            gridDocuments.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            gridTax.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            gridAccounts.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            gridTransactions.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;

            wf.Hide();
            wf.Close();
            wf.Dispose();
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
            if (SelectedGrid.AutoSizeColumnsMode == AutoSizeColumnsMode.Fill)
            {
                SelectedGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            }
            else
            {
                SelectedGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            }
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

                case 6:
                    gridTransactions.Focus();
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

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xlsx File|*.xlsx|Ficheiro Excel|*.xlsx",
                Title = "Guardar ficheiro Excel"
            };

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
            var document = new PdfDocument();
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            var page = document.Pages.Add();

            options.AutoColumnWidth = true;
            options.AutoRowHeight = true;
            options.RepeatHeaders = true;
            options.ExportGroups = true;
            options.ExportStackedHeaders = true;
            options.FitAllColumnsInOnePage = true;

            foreach (var c in SelectedGrid.Columns)
            {
                if (c.Width == 0)
                {
                    options.ExcludeColumns.Add(c.MappingName);
                }
            }

            var PDFGrid = SelectedGrid.ExportToPdfGrid(SelectedGrid.View, options);

            var format = new PdfGridLayoutFormat()
            {
                Layout = PdfLayoutType.Paginate,
                Break = PdfLayoutBreakType.FitPage
            };

            PDFGrid.Draw(page, new PointF(), format);

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Pdf Image|*.pdf|Ficheiro pdf|*.pdf",
                Title = "Guardar ficheiro pdf"
            };

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
            Application.Exit();
            Cursor.Current = Cursors.Default;
        }

        private void cmdToolValidate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var validator = $"{AppDomain.CurrentDomain.BaseDirectory}/validador_v1_04.jar";
            Process.Start(validator);
            Cursor.Current = Cursors.Default;
        }

        private void cmdToolTaxByDocument_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<TaxByDocumentFormDialog>();
            f.DataGrid = this.gridLines;
            f.Show(this);

            Cursor.Current = Cursors.Default;
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var f = CompositionRoot.Resolve<MainForm>();
            var o = this.Owner;
            this.Dispose();
            f.Show(o);

            Cursor.Current = Cursors.Default;
        }

        private void cmdToolAbout_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var f = CompositionRoot.Resolve<WaitingForm>();
            f.ShowDialog(this);
            Cursor.Current = Cursors.Default;
        }

        private void lblWebsite_Click(object sender, EventArgs e)
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

        private void cmdToolTaxByDocumentType_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<TaxByDocumentTypeFormDialog>();
            f.DataGrid = this.gridLines;
            f.Show(this);

            Cursor.Current = Cursors.Default;
        }

        private void cmdToolTaxByCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<TaxByCustomerFormDialog>();
            f.DataGrid = this.gridLines;
            f.Show(this);

            Cursor.Current = Cursors.Default;
        }

        private void cmdTooltaxByProduct_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<TaxByProductFormDialog>();
            f.DataGrid = this.gridLines;
            f.Show(this);

            Cursor.Current = Cursors.Default;
        }

        private void cmdToolOpen_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }

        private void cmdToolAddSaft_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<AttachedFilesFormDialog>();
            f.DataGrid = this.gridLines;
            var r = f.ShowDialog(this);

            if (r == DialogResult.OK)
            {
                var fr = CompositionRoot.Resolve<MainForm>();
                var o = this.Owner;
                this.Dispose();
                fr.Show(o);
            }

            Cursor.Current = Cursors.Default;
        }

        private void gridAccounts_QueryRowStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryRowStyleEventArgs e)
        {
            if (e.RowType == RowType.DefaultRow)
            {
                if ((e.RowData as AccountEntry).GroupingCategory.Equals("GR".ToAccountGroupCatDesc()))
                {
                    e.Style.Font.Bold = true;
                    e.Style.BackColor = ColorTranslator.FromHtml("#ebebe0");
                }
                else if ((e.RowData as AccountEntry).GroupingCategory.Equals("GA".ToAccountGroupCatDesc()))
                {
                    e.Style.Font.Bold = true;
                }
            }
        }

        #endregion Events
    }
}