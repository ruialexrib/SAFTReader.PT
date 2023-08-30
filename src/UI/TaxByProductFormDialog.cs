using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Syncfusion.Data;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid;

namespace SAFT_Reader.UI
{
    public partial class TaxByProductFormDialog : SfForm
    {
        public SfDataGrid DataGrid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxByProductFormDialog"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the TaxByProductFormDialog by calling the InitializeComponent() method and
        /// setting up the initial view using the InitializeView() method.
        /// </remarks>
        public TaxByProductFormDialog()
        {
            InitializeComponent();
            InitializeView();
        }

        /// <summary>
        /// Initializes the view components for the TaxByProductFormDialog.
        /// </summary>
        /// <remarks>
        /// This method populates the multi-column combo box with product records obtained from invoice lines. It groups
        /// the records by product description and selects the first item from each group to display in the combo box.
        /// It sets the data source, display member, and value member for the combo box.
        /// </remarks>
        private void InitializeView()
        {
            var records = Globals.LoadInvoiceLines()
                                    .GroupBy(x => x.ProductDescription)
                                    .Select(x => x.First())
                                    .Select(z => new
                                    {
                                        Código = z.ProductCode,
                                        Descrição = z.ProductDescription
                                    }).ToList();

            this.multiColumnComboBox1.DataSource = records;
            this.multiColumnComboBox1.DisplayMember = "Descrição";
            this.multiColumnComboBox1.ValueMember = "Código";
        }

        /// <summary>
        /// Handles the Click event of the OK button to apply filters and grouping to the data grid.
        /// </summary>
        /// <remarks>
        /// This method is called when the OK button is clicked. It applies filters and grouping to the data grid based on
        /// the selected product code from the multi-column combo box and the "Only Normal" checkbox. It clears existing
        /// filters, grouping, and sorting, sets new filter predicates, adds a group column description, and adjusts the
        /// auto-fit group drop area item.
        /// </remarks>
        /// <param name="sender">The sender of the event (the OK button).</param>
        /// <param name="e">The event arguments.</param>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var filter = multiColumnComboBox1.SelectedValue;
            DataGrid.ClearFilters();
            DataGrid.ClearGrouping();
            DataGrid.ClearSorting();

            DataGrid.Columns["ProductCode"].FilterPredicates.Add(new FilterPredicate()
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
        /// Handles the Click event of the Cancel button to close the current dialog.
        /// </summary>
        /// <remarks>
        /// This method is called when the Cancel button is clicked. It sets the cursor to the wait cursor, closes
        /// the current dialog, and then restores the default cursor.
        /// </remarks>
        /// <param name="sender">The sender of the event (the Cancel button).</param>
        /// <param name="e">The event arguments.</param>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
    }
}