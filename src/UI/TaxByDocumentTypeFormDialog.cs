using Syncfusion.Data;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SAFT_Reader.UI
{
    public partial class TaxByDocumentTypeFormDialog : SfForm
    {
        public SfDataGrid DataGrid { get; set; }

        public TaxByDocumentTypeFormDialog()
        {
            InitializeComponent();
            InitializeView();


        }

        private void InitializeView()
        {
            var records = Globals.AuditFile.SourceDocuments
                                                .SalesInvoices
                                                .Invoice
                                                .GroupBy(p=>p.InvoiceType)
                                                .Select(p=>p.First())
                                                .Select(z => new
                                                {
                                                    z.InvoiceType
                                                }).ToList();

            this.multiColumnComboBox1.DataSource = records;
            this.multiColumnComboBox1.DisplayMember = "InvoiceType";
            this.multiColumnComboBox1.ValueMember = "InvoiceType";
        }

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
            DataGrid.GroupColumnDescriptions.Add(new GroupColumnDescription()
            {
                ColumnName = "TaxCode"
            });
            //DataGrid.ExpandAllGroup();
            DataGrid.AutoFitGroupDropAreaItem = true;

            Cursor.Current = Cursors.Default;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
    }
}
