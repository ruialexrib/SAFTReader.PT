using Programatica.Saft.Models;
using Syncfusion.Data;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAFT_Reader.UI
{
    public partial class TaxByCustomerFormDialog : SfForm
    {
        public SfDataGrid DataGrid { get; set; }

        public TaxByCustomerFormDialog()
        {
            InitializeComponent();
            InitializeView();


        }

        private void InitializeView()
        {
            var records = Globals.AuditFile.SourceDocuments
                                                .SalesInvoices
                                                .Invoice
                                                .Select(z => new
                                                {
                                                    CustomerTaxID = Globals.AuditFile.MasterFiles
                                                                                        .Customer
                                                                                        .Where(x => x.CustomerID.Equals(z.CustomerID))
                                                                                        .FirstOrDefault()
                                                                                        .CustomerTaxID,
                                                    CompanyName = Globals.AuditFile.MasterFiles
                                                                                        .Customer
                                                                                        .Where(x => x.CustomerID.Equals(z.CustomerID))
                                                                                        .FirstOrDefault()
                                                                                        .CompanyName,
                                                })
                                                .GroupBy(p=>p.CustomerTaxID)
                                                .Select(p=>p.First())
                                                .ToList();

            this.multiColumnComboBox1.DataSource = records;
            this.multiColumnComboBox1.DisplayMember = "CustomerTaxID";
            this.multiColumnComboBox1.ValueMember = "CustomerTaxID";
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var filter = multiColumnComboBox1.SelectedValue;
            DataGrid.ClearFilters();
            DataGrid.ClearGrouping();
            DataGrid.ClearSorting();

            DataGrid.Columns["CustomerTaxID"].FilterPredicates.Add(new FilterPredicate()
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
