using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Syncfusion.Data;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid;

namespace SAFT_Reader.UI
{
    public partial class TaxByDocumentFormDialog : SfForm
    {
        public SfDataGrid DataGrid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxByDocumentFormDialog"/> class.
        /// </summary>
        public TaxByDocumentFormDialog()
        {
            InitializeComponent();
            InitializeView();
        }

        /// <summary>
        /// Initializes the view for the TaxByDocumentFormDialog, populating the multi-column combo box with records.
        /// </summary>
        private void InitializeView()
        {
            var records = Globals.AuditFile.SourceDocuments
                                                .SalesInvoices
                                                .Invoice
                                                .Select(z => new
                                                {
                                                    Documento = z.InvoiceNo,
                                                    Tipo = z.InvoiceType,
                                                    Data = z.InvoiceDate,
                                                    Imposto = z.DocumentTotals.TaxPayable
                                                }).ToList();

            this.multiColumnComboBox1.DataSource = records;
            this.multiColumnComboBox1.DisplayMember = "Documento";
            this.multiColumnComboBox1.ValueMember = "Documento";
        }

        /// <summary>
        /// Handles the click event when the OK button is pressed in the TaxByDocumentFormDialog.
        /// Applies filters and grouping based on user selections to display relevant data in the grid.
        /// </summary>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var filter = multiColumnComboBox1.SelectedValue;
            DataGrid.ClearFilters();
            DataGrid.ClearGrouping();
            DataGrid.ClearSorting();

            DataGrid.Columns["InvoiceNo"].FilterPredicates.Add(new FilterPredicate()
            {
                FilterType = FilterType.Equals,
                FilterValue = filter
            });
            if (chkOnlyNormal.Checked)
            {
                DataGrid.Columns["InvoiceStatus"].FilterPredicates.Add(new FilterPredicate()
                {
                    FilterType = FilterType.Equals,
                    FilterValue = "N"
                });
            }
            DataGrid.GroupColumnDescriptions.Add(new GroupColumnDescription()
            {
                ColumnName = "TaxCode"
            });
            //DataGrid.ExpandAllGroup();
            DataGrid.AutoFitGroupDropAreaItem = true;

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Handles the click event when the Cancel button is pressed in the TaxByDocumentFormDialog.
        /// Closes the dialog.
        /// </summary>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
    }
}