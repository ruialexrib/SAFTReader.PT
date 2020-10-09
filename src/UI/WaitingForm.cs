using System;
using System.Drawing;
using System.Windows.Forms;

using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.Core.Utils;

namespace SAFT_Reader.UI
{
    public partial class WaitingForm : SfForm
    {
        private readonly BusyIndicator busyIndicator = new BusyIndicator();

        public WaitingForm()
        {
            InitializeComponent();
        }

        private void InitializeView()
        {
            lblVersion.Text = $"{Globals.VersionLabel}";
            lblReader.ForeColor = ColorTranslator.FromHtml("#00BFA6");
            lblCopy.Text = "Copyright 2020, Rui Ribeiro. Todos os direitos reservados.";
            busyIndicator.Show(picWaiting);
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {
            InitializeView();
            Application.DoEvents();
        }

        public void SetMsg(string msg)
        {
            lblMsg.Text = msg;
            Application.DoEvents();
        }
    }
}