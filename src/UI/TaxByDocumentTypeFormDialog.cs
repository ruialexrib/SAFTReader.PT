using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Syncfusion.Data;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid;

namespace SAFT_Reader.UI
{
    public partial class TaxByDocumentTypeFormDialog : SfForm
    {
        public SfDataGrid DataGrid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxByDocumentTypeFormDialog"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is responsible for initializing a new instance of the TaxByDocumentTypeFormDialog class.
        /// It initializes the visual components and custom view for the dialog.
        /// </remarks>
        public TaxByDocumentTypeFormDialog()
        {
            InitializeComponent();
            InitializeView();
        }

        /// <summary>
        /// Initializes the custom view of the TaxByDocumentTypeFormDialog.
        /// </summary>
        /// <remarks>
        /// This method populates a multi-column combo box with unique invoice types from the loaded audit file.
        /// It sets the data source, display member, and value member for the combo box.
        /// </remarks>
        private void InitializeView()
        {
            var records = Globals.AuditFile.SourceDocuments
                                                .SalesInvoices
                                                .Invoice
                                                .GroupBy(p => p.InvoiceType)
                                                .Select(p => p.First())
                                                .Select(z => new
                                                {
                                                    Tipo = z.InvoiceType
                                                }).ToList();

            this.multiColumnComboBox1.DataSource = records;
            this.multiColumnComboBox1.DisplayMember = "Tipo";
            this.multiColumnComboBox1.ValueMember = "Tipo";
        }

        /// <summary>
        /// Handles the click event of the OK button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This method is called when the OK button is clicked. It filters and groups data in the data grid
        /// based on the selected invoice type and optionally filters for normal invoices only.
        /// </remarks>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var filter = multiColumnComboBox1.SelectedValue;
            DataGrid.ClearFilters();
            DataGrid.ClearGrouping();
            DataGrid.ClearSorting();

            DataGrid.Columns["InvoiceType"].FilterPredicates.Add(new FilterPredicate()
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
        /// Handles the click event of the Cancel button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This method is called when the Cancel button is clicked. It closes the form.
        /// </remarks>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
    }
}