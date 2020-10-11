namespace SAFT_Reader.UI
{
    partial class OpenFileDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenFileDialogForm));
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdFind = new Syncfusion.WinForms.Controls.SfButton();
            this.cmdOK = new Syncfusion.WinForms.Controls.SfButton();
            this.cmdClose = new Syncfusion.WinForms.Controls.SfButton();
            this.cmdOpenDemo = new System.Windows.Forms.LinkLabel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Enabled = false;
            this.txtFilePath.Location = new System.Drawing.Point(210, 114);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(318, 21);
            this.txtFilePath.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(210, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Localização do ficheiro SAFT:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(210, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(355, 71);
            this.label3.TabIndex = 10;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // cmdFind
            // 
            this.cmdFind.AccessibleName = "Button";
            this.cmdFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFind.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdFind.Location = new System.Drawing.Point(534, 114);
            this.cmdFind.Name = "cmdFind";
            this.cmdFind.Size = new System.Drawing.Size(24, 21);
            this.cmdFind.TabIndex = 11;
            this.cmdFind.Text = "...";
            this.cmdFind.Click += new System.EventHandler(this.cmdFind_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.AccessibleName = "Button";
            this.cmdOK.Enabled = false;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Location = new System.Drawing.Point(462, 199);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(96, 28);
            this.cmdOK.TabIndex = 12;
            this.cmdOK.Text = "Abrir";
            this.cmdOK.Click += new System.EventHandler(this.cmdOk_Click_1);
            // 
            // cmdClose
            // 
            this.cmdClose.AccessibleName = "Button";
            this.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdClose.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdClose.Location = new System.Drawing.Point(360, 199);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(96, 28);
            this.cmdClose.TabIndex = 13;
            this.cmdClose.Text = "Fechar";
            this.cmdClose.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOpenDemo
            // 
            this.cmdOpenDemo.AutoSize = true;
            this.cmdOpenDemo.Location = new System.Drawing.Point(210, 152);
            this.cmdOpenDemo.Name = "cmdOpenDemo";
            this.cmdOpenDemo.Size = new System.Drawing.Size(325, 13);
            this.cmdOpenDemo.TabIndex = 15;
            this.cmdOpenDemo.TabStop = true;
            this.cmdOpenDemo.Text = "Abrir Ficheiro Demonstração [SAFT_IDEMO599999999_v1.04.xml]";
            this.cmdOpenDemo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cmdOpenDemo_LinkClicked);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(2, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(202, 184);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 17;
            this.pictureBox2.TabStop = false;
            // 
            // OpenFileDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 238);
            this.Controls.Add(this.cmdOpenDemo);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdFind);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(16, 16);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenFileDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "SAFT READER";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label3;
        private Syncfusion.WinForms.Controls.SfButton cmdFind;
        private Syncfusion.WinForms.Controls.SfButton cmdOK;
        private Syncfusion.WinForms.Controls.SfButton cmdClose;
        private System.Windows.Forms.LinkLabel cmdOpenDemo;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}