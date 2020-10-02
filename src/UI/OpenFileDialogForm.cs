using Programatica.Saft.Models;
using SAFT_Reader.Adapter;
using Syncfusion.WinForms.Controls;
using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace SAFT_Reader.UI
{
    public partial class OpenFileDialogForm : SfForm
    {
        public string FilePath
        {
            get { return txtFilePath.Text; }
            set { txtFilePath.Text = value; }
        }

        private readonly IFileStreamAdapter _fileStreamAdapter;
        private readonly IXmlSerializerAdapter _xmlSerializerAdapter;

        public OpenFileDialogForm(
            IFileStreamAdapter fileStreamAdapter,
            IXmlSerializerAdapter xmlSerializerAdapter)
        {
            _fileStreamAdapter = fileStreamAdapter;
            _xmlSerializerAdapter = xmlSerializerAdapter;

            InitializeComponent();
            InitializeView();
        }

        private void InitializeView()
        {

        }

        private AuditFile OpenFile(string path)
        {
            var model = _fileStreamAdapter.Read(path);
            return _xmlSerializerAdapter.ConvertXml<AuditFile>(model);
        }

        private void cmdFind_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog1.FileName;
                cmdOK.Enabled = true;
            }
            else
            {
                txtFilePath.Text = string.Empty;
                cmdOK.Enabled = false;
            }

            Cursor.Current = Cursors.Default;
        }

        private void cmdOk_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Globals.AttachedFiles.Clear();
            try
            {
                var a = OpenFile(FilePath);
                Globals.AttachedFiles.Add(new AttachedFile
                {
                    ID = Guid.NewGuid(),
                    AuditFile = a,
                    FilePath = FilePath,
                    IsPrincipal = true
                });

                Globals.AuditFile = a;

                var f = CompositionRoot.Resolve<SAFTTotalsForm>();
                f.ShowDialog(this);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocorreu um erro ao abrir o ficheiro Saft-PT. \n\r" +
                    "Garanta que se trata de um ficheiro válido, no formato 1.04_01 " +
                    "(Portaria n.º 302/2016, de 2 de dezembro)",
                    "Erro ao abrir ficheiro Saft-PT",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.Exit();
            Cursor.Current = Cursors.Default;
        }

        private void cmdOpenDemo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            txtFilePath.Text = $"{AppDomain.CurrentDomain.BaseDirectory}/SAFT_IDEMO599999999_v1.04.xml";
            cmdOK.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

    }
}
