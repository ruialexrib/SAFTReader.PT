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
    public partial class TaxByDocumentFormDialog : SfForm
    {
        public SfDataGrid DataGrid { get; set; }

        public TaxByDocumentFormDialog()
        {
            InitializeComponent();
            InitializeView();


        }

        private void InitializeView()
        {
            var records = Globals.AuditFile.SourceDocuments
                                                .SalesInvoices
                                                .Invoice
                                                .Where(x=>x.DocumentStatus.InvoiceStatus.Equals("N"))
                                                .Select(z => new
                                                {
                                                    z.InvoiceNo,
                                                    z.InvoiceType,
                                                    z.InvoiceDate,
                                                    z.CustomerID,
                                                    z.DocumentTotals.TaxPayable
                                                }).ToList();

            this.multiColumnComboBox1.DataSource = records;
            this.multiColumnComboBox1.DisplayMember = "InvoiceNo";
            this.multiColumnComboBox1.ValueMember = "InvoiceNo";
        }

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
