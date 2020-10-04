namespace SAFT_Reader.UI
{
    partial class TaxByDocumentTypeFormDialog
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
            Syncfusion.Windows.Forms.MetroColorTable metroColorTable1 = new Syncfusion.Windows.Forms.MetroColorTable();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaxByDocumentTypeFormDialog));
            this.cmdOK = new Syncfusion.WinForms.Controls.SfButton();
            this.multiColumnComboBox1 = new Syncfusion.Windows.Forms.Tools.MultiColumnComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdCancel = new Syncfusion.WinForms.Controls.SfButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.chkOnlyNormal = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.multiColumnComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.AccessibleName = "Button";
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Location = new System.Drawing.Point(378, 188);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(96, 28);
            this.cmdOK.TabIndex = 21;
            this.cmdOK.Text = "Executar";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // multiColumnComboBox1
            // 
            this.multiColumnComboBox1.BeforeTouchSize = new System.Drawing.Size(258, 21);
            this.multiColumnComboBox1.FlatStyle = Syncfusion.Windows.Forms.Tools.ComboFlatStyle.System;
            this.multiColumnComboBox1.Location = new System.Drawing.Point(216, 122);
            this.multiColumnComboBox1.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.multiColumnComboBox1.Name = "multiColumnComboBox1";
            this.multiColumnComboBox1.ScrollMetroColorTable = metroColorTable1;
            this.multiColumnComboBox1.Size = new System.Drawing.Size(258, 21);
            this.multiColumnComboBox1.TabIndex = 22;
            this.multiColumnComboBox1.Text = "multiColumnComboBox1";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(213, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 75);
            this.label1.TabIndex = 24;
            this.label1.Text = "Este relatório permite calcular o Imposto e respetiva Base de Imposto por Tipolog" +
    "ia de Taxa de um determinado Tipo de Documento.\r\n\r\nPara continuar, seleccione o " +
    "Tipo de Documento.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(213, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Seleccione o Tipo de Documento:";
            // 
            // cmdCancel
            // 
            this.cmdCancel.AccessibleName = "Button";
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Location = new System.Drawing.Point(276, 188);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(96, 28);
            this.cmdCancel.TabIndex = 26;
            this.cmdCancel.Text = "Fechar";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(5, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(202, 184);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // chkOnlyNormal
            // 
            this.chkOnlyNormal.AutoSize = true;
            this.chkOnlyNormal.Checked = true;
            this.chkOnlyNormal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOnlyNormal.Location = new System.Drawing.Point(213, 149);
            this.chkOnlyNormal.Name = "chkOnlyNormal";
            this.chkOnlyNormal.Size = new System.Drawing.Size(261, 17);
            this.chkOnlyNormal.TabIndex = 29;
            this.chkOnlyNormal.Text = "Apenas calcular para docs. no estado \'N-Normal\'.";
            this.chkOnlyNormal.UseVisualStyleBackColor = true;
            // 
            // TaxByDocumentTypeFormDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 232);
            this.Controls.Add(this.chkOnlyNormal);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.multiColumnComboBox1);
            this.Controls.Add(this.cmdOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaxByDocumentTypeFormDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "SAFT READER";
            ((System.ComponentModel.ISupportInitialize)(this.multiColumnComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Syncfusion.WinForms.Controls.SfButton cmdOK;
        private Syncfusion.Windows.Forms.Tools.MultiColumnComboBox multiColumnComboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Syncfusion.WinForms.Controls.SfButton cmdCancel;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox chkOnlyNormal;
    }
}