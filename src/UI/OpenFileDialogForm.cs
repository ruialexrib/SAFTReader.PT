using System;
using System.Windows.Forms;

using Programatica.Saft.Models;

using SAFT_Reader.Adapter;

using Syncfusion.WinForms.Controls;

namespace SAFT_Reader.UI
{
    public partial class OpenFileDialogForm : SfForm
    {
        /// <summary>
        /// Gets or sets the file path displayed in the txtFilePath TextBox.
        /// </summary>
        public string FilePath
        {
            get { return txtFilePath.Text; }
            set { txtFilePath.Text = value; }
        }

        private readonly IFileStreamAdapter _fileStreamAdapter;
        private readonly IXmlSerializerAdapter _xmlSerializerAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialogForm"/> class.
        /// </summary>
        /// <param name="fileStreamAdapter">The file stream adapter to use for file operations.</param>
        /// <param name="xmlSerializerAdapter">The XML serializer adapter to use for XML conversion.</param>
        public OpenFileDialogForm(
            IFileStreamAdapter fileStreamAdapter,
            IXmlSerializerAdapter xmlSerializerAdapter)
        {
            _fileStreamAdapter = fileStreamAdapter;
            _xmlSerializerAdapter = xmlSerializerAdapter;

            InitializeComponent();
            InitializeView();
        }

        /// <summary>
        /// Initializes the view and performs any necessary setup for the OpenFileDialogForm.
        /// </summary>
        private void InitializeView()
        {
        }

        /// <summary>
        /// Opens and reads an AuditFile from the specified file path.
        /// </summary>
        /// <param name="path">The file path of the AuditFile to open.</param>
        /// <returns>An AuditFile object representing the contents of the file.</returns>
        private AuditFile OpenFile(string path)
        {
            var model = _fileStreamAdapter.Read(path);
            return _xmlSerializerAdapter.ConvertXml<AuditFile>(model);
        }

        /// <summary>
        /// Handles the Click event of the Find button to open a file dialog and select a file.
        /// Updates the FilePath property with the selected file path.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments.</param>
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

        /// <summary>
        /// Handles the Click event of the OK button to open and process a selected Saft-PT file.
        /// Clears existing attached files, opens the selected file, and displays the main form.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments.</param>
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

                var f = CompositionRoot.Resolve<MainForm>();
                f.ShowDialog(this);
                Application.DoEvents();
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

        /// <summary>
        /// Handles the Click event of the Cancel button to exit the application.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments.</param>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.Exit();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Handles the LinkClicked event of the "Open Demo" link label to load a demo SAFT file path.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments.</param>
        private void cmdOpenDemo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            txtFilePath.Text = $"{AppDomain.CurrentDomain.BaseDirectory}/SAFT_IDEMO599999999_v1.04.xml";
            cmdOK.Enabled = true;
            Cursor.Current = Cursors.Default;
        }
    }
}