namespace PagosIntermex
{
    partial class F_FECHA_PAGO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_FECHA_PAGO));
            this.label1 = new System.Windows.Forms.Label();
            this.dTPFechaPago = new System.Windows.Forms.DateTimePicker();
            this.bFechaPAgo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Seleccionar la fecha de pago";
            // 
            // dTPFechaPago
            // 
            this.dTPFechaPago.CustomFormat = "dd/MM/yyyy";
            this.dTPFechaPago.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPFechaPago.Location = new System.Drawing.Point(37, 25);
            this.dTPFechaPago.Name = "dTPFechaPago";
            this.dTPFechaPago.Size = new System.Drawing.Size(96, 20);
            this.dTPFechaPago.TabIndex = 1;
            // 
            // bFechaPAgo
            // 
            this.bFechaPAgo.Location = new System.Drawing.Point(37, 51);
            this.bFechaPAgo.Name = "bFechaPAgo";
            this.bFechaPAgo.Size = new System.Drawing.Size(96, 23);
            this.bFechaPAgo.TabIndex = 2;
            this.bFechaPAgo.Text = "Aceptar";
            this.bFechaPAgo.UseVisualStyleBackColor = true;
            this.bFechaPAgo.Click += new System.EventHandler(this.bFechaPAgo_Click);
            // 
            // F_FECHA_PAGO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(170, 81);
            this.Controls.Add(this.bFechaPAgo);
            this.Controls.Add(this.dTPFechaPago);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_FECHA_PAGO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fecha de pago";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_FECHA_PAGO_FormClosing);
            this.Load += new System.EventHandler(this.F_FECHA_PAGO_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dTPFechaPago;
        private System.Windows.Forms.Button bFechaPAgo;
    }
}