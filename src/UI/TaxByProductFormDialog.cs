using Syncfusion.Data;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SAFT_Reader.UI
{
    public partial class TaxByProductFormDialog : SfForm
    {
        public SfDataGrid DataGrid { get; set; }

        public TaxByProductFormDialog()
        {
            InitializeComponent();
            InitializeView();


        }

        private void InitializeView()
        {
            var records = Globals.LoadInvoiceLines()
                                    .GroupBy(x=>x.ProductDescription)
                                    .Select(x=> x.First())
                                    .Select(z => new
                                    {
                                        Código = z.ProductCode,
                                        Descrição = z.ProductDescription
                                    }).ToList();

            this.multiColumnComboBox1.DataSource = records;
            this.multiColumnComboBox1.DisplayMember = "Descrição";
            this.multiColumnComboBox1.ValueMember = "Código";
        }

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

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
    }
}
