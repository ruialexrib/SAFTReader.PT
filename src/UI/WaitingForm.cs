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

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitingForm"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is responsible for initializing a new instance of the WaitingForm.
        /// It sets up the user interface components and prepares the form for display.
        /// </remarks>
        public WaitingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the view components and sets up the form for display.
        /// </summary>
        /// <remarks>
        /// This method initializes various view components, including labels and the busy indicator, and sets their properties
        /// to prepare the form for display. It also displays version information and copyright details.
        /// </remarks>
        private void InitializeView()
        {
            lblVersion.Text = $"{Globals.VersionLabel}";
            lblReader.ForeColor = ColorTranslator.FromHtml("#00BFA6");
            lblCopy.Text = "Copyright 2020, Rui Ribeiro. Todos os direitos reservados.";
            busyIndicator.Show(picWaiting);
        }

        /// <summary>
        /// Handles the form's Load event and initializes the view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// This event handler is triggered when the waiting form is loaded. It calls the InitializeView method to set up
        /// the view components and display relevant information. The Application.DoEvents method is called to process
        /// pending messages and ensure a responsive UI during loading.
        /// </remarks>
        private void WaitingForm_Load(object sender, EventArgs e)
        {
            InitializeView();
            Application.DoEvents();
        }

        /// <summary>
        /// Sets the message text on the waiting form.
        /// </summary>
        /// <param name="msg">The message text to be displayed on the form.</param>
        /// <remarks>
        /// This method updates the text of a label control on the waiting form to display a specific message.
        /// It is often used to inform the user about ongoing processes or tasks. The Application.DoEvents method
        /// is called to process pending messages and ensure the message is displayed promptly.
        /// </remarks>
        /// <param name="msg">The message text to be displayed on the form.</param>
        public void SetMsg(string msg)
        {
            lblMsg.Text = msg;
            Application.DoEvents();
        }
    }
}