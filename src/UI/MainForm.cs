using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
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
        private readonly WaitingForm _wf;

        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            // handle injection
            _wf = CompositionRoot.Resolve<WaitingForm>();

            InitializeComponent();
            InitializeView();
        }

        /// <summary>
        /// Initializes the view of the MainForm.
        /// </summary>
        /// <remarks>
        /// This method adjusts the size of the ribbon control and sets the label text with the application version.
        /// Additionally, it sets the SelectedGrid to the gridLines.
        /// </remarks>
        private void InitializeView()
        {
            ribbonControlAdv1.Size = new Size { Width = this.ribbonControlAdv1.Size.Width, Height = 150 };
            this.lblInfoApp.Text = $"{Globals.VersionLabel}";
            SelectedGrid = gridLines;
        }

        /// <summary>
        /// Asynchronously loads and prepares data for display in grids.
        /// </summary>
        /// <remarks>
        /// This method orchestrates the process of loading and preparing data for display in grids. It sets
        /// a waiting message to inform the user that data is being loaded, asynchronously retrieves data 
        /// using the <see cref="LoadData"/> method, and then performs grid-related operations such as setting
        /// data sources, preparing summaries, and formatting columns and lines. Once the data is ready, it
        /// signals the user that the operation is complete.
        /// </remarks>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task LoadGrids()
        {
            // Set a waiting message to indicate that data loading is in progress.
            SetWaitingMsg("A carregar ficheiro");

            // Asynchronously retrieve data using the LoadData method.
            var result = await LoadData();

            // Set the data sources for the grids based on the retrieved data.
            SetGridDataSources(result);

            // Prepare summaries for the grid.
            PrepareGridSummaries();

            // Format columns related to grid totals.
            FormatGridTotalsColumns();

            // Format grid lines for improved display.
            FormatGridLines();

            // Signal that the operation is complete.
            SetWaitingMsg("Pronto!");
        }

        /// <summary>
        /// Asynchronously loads data from various sources and returns the result as an anonymous object.
        /// </summary>
        /// <remarks>
        /// This method coordinates the asynchronous loading of data from multiple sources. It uses
        /// asynchronous tasks to load data efficiently and then combines the results into an anonymous
        /// object. The loading process includes audit information, invoice lines, taxes, invoices,
        /// customers, products, tax entries, account entries, and transaction entries.
        /// </remarks>
        /// <returns>
        /// An anonymous object containing various loaded data, such as audit information, invoice lines,
        /// totals, invoices, customers, products, tax entries, account entries, and transaction entries.
        /// </returns>
        private async Task<object> LoadData()
        {
            var auditTask = Task.Run(() => Globals.AuditFile);
            SetWaitingMsg("A carregar as linhas das faturas...");
            var invoiceLinesTask = Task.Run(() => Globals.LoadInvoiceLines());

            // wait for all
            await Task.WhenAll(auditTask, invoiceLinesTask);

            SetWaitingMsg("A carregar impostos das faturas...");
            var totalsTask = Task.Run(() => Globals.LoadTaxEntryTotals(invoiceLinesTask.Result));
            SetWaitingMsg("A carregar faturas...");
            var invoicesTask = Task.Run(() => Globals.LoadInvoiceEntries());
            SetWaitingMsg("A carregar clientes...");
            var customersTask = Task.Run(() => Globals.LoadCustomerEntries());
            SetWaitingMsg("A carregar produtos...");
            var productsTask = Task.Run(() => Globals.LoadProductEntries());
            SetWaitingMsg("A carregar impostos...");
            var taxTask = Task.Run(() => Globals.LoadTaxEntries());
            SetWaitingMsg("A carregar plano de contas...");
            var accountsTask = Task.Run(() => Globals.LoadAccountEntries());
            SetWaitingMsg("A carregar outras transações...");
            var transTask = Task.Run(() => Globals.LoadTransactionEntries());

            // wait for remaining tasks
            await Task.WhenAll(totalsTask, invoicesTask, customersTask, productsTask, taxTask, accountsTask, transTask);

            // load variables
            var result = new
            {
                Audit = auditTask.Result,
                InvoiceLines = invoiceLinesTask.Result,
                Totals = totalsTask.Result,
                Invoices = invoicesTask.Result,
                Customers = customersTask.Result,
                Products = productsTask.Result,
                Tax = taxTask.Result,
                Accounts = accountsTask.Result,
                Trans = transTask.Result
            };

            return result;
        }

        /// <summary>
        /// Sets the data sources for the specified grids with the data from the given dynamic object.
        /// </summary>
        /// <param name="result">The dynamic object containing data for the grids.</param>
        /// <remarks>
        /// This method sets the data sources for multiple grids using data from the provided dynamic object.
        /// It is used to populate different grids with data efficiently.
        /// </remarks>
        private void SetGridDataSources(dynamic result)
        {
            SetWaitingMsg("A preparar listas...");
            gridLines.DataSource = result.InvoiceLines;
            gridTotals.DataSource = result.Totals;
            gridDocuments.DataSource = result.Invoices;
            gridCustomers.DataSource = result.Customers;
            gridProducts.DataSource = result.Products;
            gridTax.DataSource = result.Tax;
            gridAccounts.DataSource = result.Accounts;
            gridTransactions.DataSource = result.Trans;
        }

        /// <summary>
        /// Prepares grid summaries for various data tables.
        /// </summary>
        /// <remarks>
        /// This method prepares summary information for different data tables displayed in grids.
        /// It sets up summary calculations and formatting for a variety of grid tables to provide a concise view of data.
        /// </remarks>
        private void PrepareGridSummaries()
        {
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
        }

        /// <summary>
        /// Formats the columns of the 'gridTotals' data grid.
        /// </summary>
        /// <remarks>
        /// This method is responsible for formatting the columns of the 'gridTotals' data grid.
        /// It sets font styles, background colors, and creates a stacked header for organizing column headers.
        /// The stacked header groups columns related to tax types, debit, and credit amounts.
        /// </remarks>
        private void FormatGridTotalsColumns()
        {
            SetWaitingMsg("A acabar apresentação dos dados...");

            gridTotals.Columns["TaxCode"].CellStyle.Font.Bold = true;
            gridTotals.Columns["TotalCredit"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridTotals.Columns["TotalCredit"].CellStyle.Font.Bold = true;
            gridTotals.Columns["TotalDebit"].CellStyle.BackColor = ColorTranslator.FromHtml(Globals.LightColumnColor);
            gridTotals.Columns["TotalDebit"].CellStyle.Font.Bold = true;

            var stackedHeaderRow1 = new StackedHeaderRow();
            stackedHeaderRow1.StackedColumns.Add(new StackedColumn() { ChildColumns = "TaxCode,TaxDescription,TaxPercentage", HeaderText = "Tipo de Imposto" });
            stackedHeaderRow1.StackedColumns.Add(new StackedColumn() { ChildColumns = "DebitAmount,DebitTaxPayable,TotalDebit", HeaderText = "Débito" });
            stackedHeaderRow1.StackedColumns.Add(new StackedColumn() { ChildColumns = "CreditAmount,CreditTaxPayable,TotalCredit", HeaderText = "Crédito" });
            gridTotals.StackedHeaderRows.Add(stackedHeaderRow1);
        }

        /// <summary>
        /// Formats the columns of various data grids.
        /// </summary>
        /// <remarks>
        /// This method is responsible for formatting the columns of multiple data grids, including 'gridLines', 'gridDocuments', 
        /// 'gridCustomers', 'gridProducts', 'gridTax', 'gridAccounts', and 'gridTransactions'.
        /// It sets font styles, background colors, and column widths to enhance the presentation of data in these grids.
        /// </remarks>
        private void FormatGridLines()
        {
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
            gridLines.Columns["TaxExemptionCode"].Width = 50;
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
        }

        /// <summary>
        /// Loads audit header PropertyGrids for attached files.
        /// </summary>
        /// <remarks>
        /// For each attached file in the Globals.AttachedFiles collection, this method creates a PropertyGrid and a TabPageAdv.
        /// It sets the PropertyGrid's selected object to the header of the AuditFile, customizes its properties,
        /// and adds it to the TabPageAdv. The TabPageAdv is then added to the tabControlAdv3 control.
        /// </remarks>
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

        /// <summary>
        /// Sets up grid totals summaries for various columns in the grid.
        /// </summary>
        /// <remarks>
        /// This method configures the grid to display totals summaries for specific columns.
        /// It creates a GridTableSummaryRow and adds GridSummaryColumn objects for each desired column.
        /// The summary type for each column is set to double aggregate, and the format is specified as currency.
        /// Finally, the summary columns are added to the summary row, and the summary row is added to the grid.
        /// </remarks>
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

            GridSummaryColumn tc = new GridSummaryColumn
            {
                Name = "TotalCredit",
                Format = "{Sum:c}",
                MappingName = "TotalCredit",
                SummaryType = SummaryType.DoubleAggregate
            };

            GridSummaryColumn td = new GridSummaryColumn
            {
                Name = "TotalDebit",
                Format = "{Sum:c}",
                MappingName = "TotalDebit",
                SummaryType = SummaryType.DoubleAggregate
            };


            sum.SummaryColumns.Add(ca);
            sum.SummaryColumns.Add(da);
            sum.SummaryColumns.Add(td);
            sum.SummaryColumns.Add(tc);
            sum.SummaryColumns.Add(ctp);
            sum.SummaryColumns.Add(dtp);


            this.gridTotals.TableSummaryRows.Add(sum);
        }

        /// <summary>
        /// Sets up grid line summaries for specific columns in the grid.
        /// </summary>
        /// <remarks>
        /// This method configures the grid to display line summaries for selected columns.
        /// It creates a GridTableSummaryRow and adds GridSummaryColumn objects for each desired column.
        /// The summary type for each column is set to double aggregate, and the format is specified as currency.
        /// Finally, the summary columns are added to the summary row, and the summary row is added to the grid.
        /// </remarks>
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

        /// <summary>
        /// Sets up table summaries for customer-related data in the grid.
        /// </summary>
        /// <remarks>
        /// This method configures the grid to display table-level summaries for customer data.
        /// It creates a GridTableSummaryRow and adds GridSummaryColumn objects for the total credit and debit amounts.
        /// The summary type for each column is set to double aggregate, and the format is specified as currency.
        /// Finally, the summary columns are added to the summary row, and the summary row is added to the grid.
        /// </remarks>
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

        /// <summary>
        /// Sets up table summaries for product-related data in the grid.
        /// </summary>
        /// <remarks>
        /// This method configures the grid to display table-level summaries for product data.
        /// It creates a GridTableSummaryRow and adds GridSummaryColumn objects for the total credit and debit amounts.
        /// The summary type for each column is set to double aggregate, and the format is specified as currency.
        /// Finally, the summary columns are added to the summary row, and the summary row is added to the grid.
        /// </remarks>
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

        /// <summary>
        /// Sets up table summaries for tax-related data in the grid.
        /// </summary>
        /// <remarks>
        /// This method configures the grid to display table-level summaries for tax data.
        /// It creates a GridTableSummaryRow and adds GridSummaryColumn objects for the total credit and debit amounts.
        /// The summary type for each column is set to double aggregate, and the format is specified as currency.
        /// Finally, the summary columns are added to the summary row, and the summary row is added to the grid.
        /// </remarks>
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

        /// <summary>
        /// Sets up table summaries for account-related data in the grid.
        /// </summary>
        /// <remarks>
        /// This method configures the grid to display table-level summaries for account data.
        /// It creates a GridTableSummaryRow and adds GridSummaryColumn objects for opening and closing debit and credit balances.
        /// The summary type for each column is set to double aggregate, and the format is specified as currency.
        /// Finally, the summary columns are added to the summary row, and the summary row is added to the grid.
        /// </remarks>
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

        /// <summary>
        /// Configures summary rows and columns for grouped columns in the grid.
        /// </summary>
        /// <remarks>
        /// This method sets up a summary row and summary columns to display aggregated information
        /// for grouped columns in the grid. It creates a caption summary row with placeholders for
        /// column name, key, and item count. It also creates summary columns for specific data fields,
        /// such as CreditAmount, DebitAmount, CreditTaxPayable, and DebitTaxPayable. These summary columns
        /// calculate the sum of their respective data values. Finally, the caption summary row is assigned
        /// to the grid's CaptionSummaryRow property.
        /// </remarks>
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

        /// <summary>
        /// Sets up table summaries for document-related data in the grid.
        /// </summary>
        /// <remarks>
        /// This method configures the grid to display table-level summaries for document data.
        /// It creates a GridTableSummaryRow and adds GridSummaryColumn objects for Tax Payable, Net Total,
        /// and Gross Total. The summary type for each column is set to double aggregate, and the format is
        /// specified as currency. Finally, the summary columns are added to the summary row, and the summary
        /// row is added to the grid.
        /// </remarks>
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

        /// <summary>
        /// Sets up table summaries for transaction-related data in the grid.
        /// </summary>
        /// <remarks>
        /// This method configures the grid to display table-level summaries for transaction data.
        /// It creates a GridTableSummaryRow and adds GridSummaryColumn objects for Credit Amount and Debit Amount.
        /// The summary type for each column is set to double aggregate, and the format is specified as currency.
        /// Finally, the summary columns are added to the summary row, and the summary row is added to the grid.
        /// </remarks>
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

        /// <summary>
        /// Sets a waiting message in a waiting form if available.
        /// </summary>
        /// <param name="msg">The message to be displayed in the waiting form.</param>
        /// <remarks>
        /// This method updates the waiting message displayed in a waiting form, if the waiting form instance (_wf) is not null.
        /// The provided message will be passed to the waiting form's SetMsg method to update the displayed message.
        /// </remarks>
        private void SetWaitingMsg(string msg)
        {
            if (_wf != null)
            {
                _wf.SetMsg(msg);
            }
        }

        #region Events

        /// <summary>
        /// Event handler for the TaxTableEntryTotalForm's Load event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the TaxTableEntryTotalForm is loaded.
        /// It sets the cursor to a wait cursor, shows a waiting form (_wf), and asynchronously loads grids
        /// using the LoadGrids method. After loading grids and configuring the AuditHeaderPropertyGrids,
        /// it sets the mainSplitter control to be visible, closes and disposes of the waiting form, and
        /// restores the default cursor.
        /// </remarks>
        private async void TaxTableEntryTotalForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            _wf.Show();

            await LoadGrids();
            LoadAuditHeaderPropertyGrids();
            mainSplitter.Visible = true;

            _wf.Close(); _wf.Dispose();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for entering a grid control.
        /// </summary>
        /// <param name="sender">The sender of the event, which is a grid control.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the user enters a grid control. It sets the SelectedGrid
        /// property to the grid control that triggered the event. This property can be used to keep track
        /// of the currently selected grid.
        /// </remarks>
        private void GridEnter(object sender, EventArgs e)
        {
            SelectedGrid = (SfDataGrid)sender;
        }

        /// <summary>
        /// Event handler for the "Group" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Group" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler toggles the visibility of the group drop area in the currently selected grid.
        /// When the "Group" tool button is clicked, it sets the cursor to a wait cursor, toggles the
        /// ShowGroupDropArea property of the currently selected grid, and restores the default cursor.
        /// This allows users to show or hide the group drop area in the grid as needed.
        /// </remarks>
        private void cmdToolGroup_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SelectedGrid.ShowGroupDropArea = !SelectedGrid.ShowGroupDropArea;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Auto Expand" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Auto Expand" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler toggles the auto-expand behavior of the currently selected grid's columns.
        /// When the "Auto Expand" tool button is clicked, it sets the cursor to a wait cursor, checks the
        /// current AutoSizeColumnsMode of the selected grid, and toggles it between AutoSizeColumnsMode.Fill
        /// and AutoSizeColumnsMode.AllCells. This allows users to automatically resize columns to fit the
        /// content or fill the available space based on the current mode.
        /// </remarks>
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

        /// <summary>
        /// Event handler for the "Filter" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Filter" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler enables filtering in the currently selected grid, performs a search based on
        /// the provided filter text (txtToolFilter), and sets the cursor back to the default cursor after
        /// the search is completed. Users can use this tool button to filter and search for specific data
        /// in the grid.
        /// </remarks>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SelectedGrid.SearchController.AllowFiltering = true;
            SelectedGrid.SearchController.Search(txtToolFilter.Text);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Remove Filters" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Remove Filters" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler clears any active filters, search criteria, and disables filtering in the
        /// currently selected grid. It also resets the filter text input field (txtToolFilter) and sets
        /// the cursor back to the default cursor. Users can use this tool button to clear all filters and
        /// search criteria applied to the grid.
        /// </remarks>
        private void cmdToolRemoveFilters_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SelectedGrid.ClearFilters();
            SelectedGrid.SearchController.ClearSearch();
            SelectedGrid.SearchController.AllowFiltering = false;
            txtToolFilter.Text = string.Empty;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the tabControlAdv1's SelectedIndexChanged event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the tabControlAdv1 control.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the selected tab in the tabControlAdv1 control is changed.
        /// It sets the cursor to a wait cursor, determines the index of the selected tab, and focuses on
        /// the corresponding grid control based on the selected tab index. After the focus is set, the
        /// cursor is restored to the default cursor.
        /// </remarks>
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

        /// <summary>
        /// Event handler for the click event of a label containing a GitHub URL.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the label control.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when a label containing a GitHub URL is clicked. It sets the cursor
        /// to a wait cursor, extracts the URL from the label's text, checks if it contains a protocol (http://
        /// or https://), and if not, adds "https://" as the default protocol. It then creates a ProcessStartInfo
        /// object with the URL and opens the URL in the default web browser using the Process.Start method. After
        /// the operation is complete, the cursor is restored to the default cursor.
        /// </remarks>
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

        /// <summary>
        /// Event handler for the KeyDown event of a text box used for filtering.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the text box control.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when a key is pressed while the text box used for filtering
        /// (txtToolFilter) has focus. It sets the cursor to a wait cursor and checks if the pressed key is
        /// the Enter key. If the Enter key is pressed, it enables filtering in the currently selected grid,
        /// performs a search based on the filter text (txtToolFilter), and then restores the cursor to the
        /// default cursor. This allows users to apply a filter by pressing Enter in the filter text box.
        /// </remarks>
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

        /// <summary>
        /// Event handler for the "Export to Excel" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Export to Excel" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler exports the data from the currently selected grid to an Excel file in the
        /// .xlsx format. It sets up Excel exporting options, performs the export, and saves the Excel
        /// workbook to a user-specified location using a SaveFileDialog. After the export is completed,
        /// the Excel file is opened in the default associated application, and the cursor is restored to
        /// the default cursor.
        /// </remarks>
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

        /// <summary>
        /// Event handler for the "Export to PDF" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Export to PDF" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler exports the data from the currently selected grid to a PDF file. It configures
        /// PDF exporting options, sets up a PDF document with landscape orientation, and exports the grid data
        /// to a PDF grid. The PDF grid is then drawn on the PDF document, and the resulting PDF is saved to a
        /// user-specified location using a SaveFileDialog. After the export is completed, the PDF file is opened
        /// in the default associated application, and the cursor is restored to the default cursor.
        /// </remarks>
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

        /// <summary>
        /// Event handler for the "Exit" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Exit" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Exit" tool button is clicked. It sets the cursor to a wait
        /// cursor, gracefully exits the application using the Application.Exit() method, and restores the cursor
        /// to the default cursor. Users can use this tool button to close the application.
        /// </remarks>
        private void cmdToolExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.Exit();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Validate" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Validate" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Validate" tool button is clicked. It sets the cursor to a
        /// wait cursor, specifies the path to a validator JAR file, and starts a new process to execute the
        /// validator using the Process.Start() method. After launching the validator, the cursor is restored to
        /// the default cursor. Users can use this tool button to initiate a validation process with an external
        /// JAR file.
        /// </remarks>
        private void cmdToolValidate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var validator = $"{AppDomain.CurrentDomain.BaseDirectory}/validador_v1_04.jar";
            Process.Start(validator);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Tax by Document" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Tax by Document" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Tax by Document" tool button is clicked. It sets the cursor
        /// to a wait cursor, changes the selected tab in the tabControlAdv1 control to the first tab (index 0),
        /// and then opens a new TaxByDocumentFormDialog. The TaxByDocumentFormDialog is initialized with the
        /// gridLines control and displayed as a dialog. After the operation is complete, the cursor is restored
        /// to the default cursor. Users can use this tool button to view tax information by document.
        /// </remarks>
        private void cmdToolTaxByDocument_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<TaxByDocumentFormDialog>();
            f.DataGrid = this.gridLines;
            f.Show(this);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Reset" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Reset" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Reset" tool button is clicked. It sets the cursor to a wait
        /// cursor, resolves and creates a new instance of the MainForm (assuming CompositionRoot.Resolve is used
        /// for dependency injection), and shows the MainForm as a replacement for the current form. After the
        /// operation is complete, the current form is disposed, and the cursor is restored to the default cursor.
        /// Users can use this tool button to reset the application to its main form.
        /// </remarks>
        private void cmdReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var f = CompositionRoot.Resolve<MainForm>();
            var o = this.Owner;
            this.Dispose();
            f.Show(o);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "About" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "About" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "About" tool button is clicked. It sets the cursor to a wait
        /// cursor, resolves and creates a new instance of the SplashForm (assuming CompositionRoot.Resolve is used
        /// for dependency injection), and displays the SplashForm as a modal dialog with the current form as its
        /// owner. After the SplashForm is displayed, the cursor is restored to the default cursor.
        /// Users can use this tool button to view information about the application.
        /// </remarks>
        private void cmdToolAbout_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var f = CompositionRoot.Resolve<SplashForm>();
            f.ShowDialog(this);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the click event of a label containing a website URL.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the label control.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when a label containing a website URL is clicked. It sets the cursor
        /// to a wait cursor, extracts the URL from the label's text, checks if it contains a protocol (http://
        /// or https://), and if not, adds "https://" as the default protocol. It then creates a ProcessStartInfo
        /// object with the URL and opens the URL in the default web browser using the Process.Start method. After
        /// the operation is complete, the cursor is restored to the default cursor. Users can use this label to
        /// open a website link.
        /// </remarks>
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

        /// <summary>
        /// Event handler for the "Tax by Document Type" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Tax by Document Type" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Tax by Document Type" tool button is clicked. It sets the cursor
        /// to a wait cursor, changes the selected tab in the tabControlAdv1 control to the first tab (index 0), and
        /// then opens a new TaxByDocumentTypeFormDialog. The TaxByDocumentTypeFormDialog is initialized with the
        /// gridLines control and displayed as a dialog. After the operation is complete, the cursor is restored to
        /// the default cursor. Users can use this tool button to view tax information by document type.
        /// </remarks>
        private void cmdToolTaxByDocumentType_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<TaxByDocumentTypeFormDialog>();
            f.DataGrid = this.gridLines;
            f.Show(this);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Tax by Customer" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Tax by Customer" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Tax by Customer" tool button is clicked. It sets the cursor
        /// to a wait cursor, changes the selected tab in the tabControlAdv1 control to the first tab (index 0),
        /// and then opens a new TaxByCustomerFormDialog. The TaxByCustomerFormDialog is initialized with the gridLines
        /// control and displayed as a dialog. After the operation is complete, the cursor is restored to the default
        /// cursor. Users can use this tool button to view tax information by customer.
        /// </remarks>
        private void cmdToolTaxByCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<TaxByCustomerFormDialog>();
            f.DataGrid = this.gridLines;
            f.Show(this);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Tax by Product" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Tax by Product" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Tax by Product" tool button is clicked. It sets the cursor
        /// to a wait cursor, changes the selected tab in the tabControlAdv1 control to the first tab (index 0),
        /// and then opens a new TaxByProductFormDialog. The TaxByProductFormDialog is initialized with the gridLines
        /// control and displayed as a dialog. After the operation is complete, the cursor is restored to the default
        /// cursor. Users can use this tool button to view tax information by product.
        /// </remarks>
        private void cmdTooltaxByProduct_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.tabControlAdv1.SelectedIndex = 0;

            var f = CompositionRoot.Resolve<TaxByProductFormDialog>();
            f.DataGrid = this.gridLines;
            f.Show(this);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Open" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Open" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Open" tool button is clicked. It sets the cursor to a wait cursor,
        /// and then closes the current form. After the operation is complete, the cursor is restored to the default cursor.
        /// Users can use this tool button to close the current form.
        /// </remarks>
        private void cmdToolOpen_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Event handler for the "Add SAFT File" tool button click event.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the "Add SAFT File" tool button.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the "Add SAFT File" tool button is clicked. It sets the cursor to a wait cursor,
        /// changes the selected tab in the tabControlAdv1 control to the first tab (index 0), and then opens a new AttachedFilesFormDialog.
        /// The AttachedFilesFormDialog is initialized with the gridLines control and displayed as a dialog. After the user interacts
        /// with the dialog and closes it, the event handler checks if the result is DialogResult.OK. If it is, a new instance of the
        /// MainForm is created, and the current form is disposed. The new MainForm is then shown with the current form as its owner.
        /// After the operation is complete, the cursor is restored to the default cursor. Users can use this tool button to add SAFT files
        /// and update the application's main form.
        /// </remarks>
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

        /// <summary>
        /// Event handler for the QueryRowStyle event of the gridAccounts control.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the gridAccounts control.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This event handler is executed when the gridAccounts control queries the style of a row. It is used to customize
        /// the appearance of rows in the grid based on certain conditions. Specifically, it checks the row type, and if it is
        /// a default row, it checks the grouping category of the corresponding data item (assumed to be of type AccountEntry).
        /// If the grouping category is "GR" (interpreted from the extension method ToAccountGroupCatDesc()), the row style
        /// is modified to make the font bold and set a light gray background color. If the grouping category is "GA," only the
        /// font is set to bold. This styling helps distinguish and highlight certain rows in the grid.
        /// </remarks>
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