namespace SAFT_Reader.UI
{
    partial class WaitingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.lblCopy = new System.Windows.Forms.Label();
            this.lblSaft = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblReader = new System.Windows.Forms.Label();
            this.picWaiting = new System.Windows.Forms.PictureBox();
            this.lblMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picWaiting)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(343, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 39);
            this.label2.TabIndex = 44;
            this.label2.Text = ".PT";
            // 
            // lblCopy
            // 
            this.lblCopy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopy.ForeColor = System.Drawing.Color.Black;
            this.lblCopy.Location = new System.Drawing.Point(11, 235);
            this.lblCopy.Name = "lblCopy";
            this.lblCopy.Size = new System.Drawing.Size(533, 21);
            this.lblCopy.TabIndex = 43;
            this.lblCopy.Text = "#";
            this.lblCopy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSaft
            // 
            this.lblSaft.AutoSize = true;
            this.lblSaft.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaft.Location = new System.Drawing.Point(132, 5);
            this.lblSaft.Name = "lblSaft";
            this.lblSaft.Size = new System.Drawing.Size(98, 39);
            this.lblSaft.TabIndex = 38;
            this.lblSaft.Text = "SAFT";
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.Black;
            this.lblVersion.Location = new System.Drawing.Point(11, 64);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(533, 30);
            this.lblVersion.TabIndex = 41;
            this.lblVersion.Text = "#";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(142, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 16);
            this.label1.TabIndex = 40;
            this.label1.Text = "Visualizador de Ficheiro SAFT-PT 1.04_01";
            // 
            // lblReader
            // 
            this.lblReader.AutoSize = true;
            this.lblReader.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReader.Location = new System.Drawing.Point(221, 5);
            this.lblReader.Name = "lblReader";
            this.lblReader.Size = new System.Drawing.Size(131, 39);
            this.lblReader.TabIndex = 39;
            this.lblReader.Text = "Reader";
            // 
            // picWaiting
            // 
            this.picWaiting.Location = new System.Drawing.Point(203, 97);
            this.picWaiting.Name = "picWaiting";
            this.picWaiting.Size = new System.Drawing.Size(147, 85);
            this.picWaiting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picWaiting.TabIndex = 45;
            this.picWaiting.TabStop = false;
            // 
            // lblMsg
            // 
            this.lblMsg.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Black;
            this.lblMsg.Location = new System.Drawing.Point(11, 190);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(533, 25);
            this.lblMsg.TabIndex = 46;
            this.lblMsg.Text = "#";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WaitingForm
            // 
            this.ClientSize = new System.Drawing.Size(549, 267);
            this.ControlBox = false;
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.picWaiting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCopy);
            this.Controls.Add(this.lblSaft);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblReader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WaitingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "Aguarde...";
            this.Load += new System.EventHandler(this.WaitingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picWaiting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCopy;
        private System.Windows.Forms.Label lblSaft;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblReader;
        private System.Windows.Forms.PictureBox picWaiting;
        private System.Windows.Forms.Label lblMsg;
    }
}