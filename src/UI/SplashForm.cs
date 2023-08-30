using System;
using System.Drawing;
using System.Windows.Forms;

using Syncfusion.WinForms.Controls;

namespace SAFT_Reader.UI
{
    public partial class SplashForm : SfForm
    {
        public bool IsSplash { get; set; }

        private int _counter;

        /// <summary>
        /// Initializes a new instance of the SplashForm class.
        /// </summary>
        public SplashForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the view for the SplashForm.
        /// </summary>
        private void InitializeView()
        {
            if (!IsSplash)
            {
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }

            timer1.Enabled = IsSplash;
            lblVersion.Text = $"{Globals.VersionLabel}";
            lblReader.ForeColor = ColorTranslator.FromHtml("#00BFA6");
            lblCopy.Text = "Copyright 2020, Rui Ribeiro. Todos os direitos reservados.";
        }

        /// <summary>
        /// Event handler for the timer tick event.
        /// Increments a counter and closes the form when the counter reaches a specific value.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            _counter++;

            if (_counter == 50)
            {
                timer1.Stop();
                this.Close();
            }
        }

        /// <summary>
        /// Event handler for the form's Load event.
        /// Initializes the view of the form.
        /// </summary>
        private void SplashForm_Load(object sender, EventArgs e)
        {
            InitializeView();
        }
    }
}