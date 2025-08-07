namespace PagosIntermex
{
    partial class F_DATOS_PARTICULARES
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_DATOS_PARTICULARES));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dGVEmpresas = new System.Windows.Forms.DataGridView();
            this.EMP_ID_MSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIENE = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NOMBRE_CORTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.T = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVEmpresas)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(396, 46);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 34);
            this.label1.TabIndex = 1;
            this.label1.Text = "Seleccione las empresas que desea crear campos particulares necesarios para poder" +
    " generar el layout de pago del banco";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(309, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dGVEmpresas);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 46);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(396, 404);
            this.panel2.TabIndex = 1;
            // 
            // dGVEmpresas
            // 
            this.dGVEmpresas.AllowUserToAddRows = false;
            this.dGVEmpresas.AllowUserToDeleteRows = false;
            this.dGVEmpresas.AllowUserToResizeRows = false;
            this.dGVEmpresas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dGVEmpresas.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dGVEmpresas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dGVEmpresas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVEmpresas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EMP_ID_MSP,
            this.TIENE,
            this.NOMBRE_CORTO,
            this.T});
            this.dGVEmpresas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVEmpresas.Location = new System.Drawing.Point(3, 3);
            this.dGVEmpresas.Name = "dGVEmpresas";
            this.dGVEmpresas.RowHeadersVisible = false;
            this.dGVEmpresas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVEmpresas.Size = new System.Drawing.Size(390, 398);
            this.dGVEmpresas.TabIndex = 1;
            // 
            // EMP_ID_MSP
            // 
            this.EMP_ID_MSP.HeaderText = "EMP_ID_MSP";
            this.EMP_ID_MSP.Name = "EMP_ID_MSP";
            this.EMP_ID_MSP.Visible = false;
            // 
            // TIENE
            // 
            this.TIENE.FillWeight = 34.7057F;
            this.TIENE.HeaderText = "Selec";
            this.TIENE.MinimumWidth = 30;
            this.TIENE.Name = "TIENE";
            this.TIENE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TIENE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // NOMBRE_CORTO
            // 
            this.NOMBRE_CORTO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NOMBRE_CORTO.FillWeight = 193.1951F;
            this.NOMBRE_CORTO.HeaderText = "Nombre";
            this.NOMBRE_CORTO.Name = "NOMBRE_CORTO";
            this.NOMBRE_CORTO.ReadOnly = true;
            // 
            // T
            // 
            this.T.HeaderText = "T";
            this.T.Name = "T";
            this.T.Visible = false;
            // 
            // F_DATOS_PARTICULARES
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(412, 489);
            this.MinimumSize = new System.Drawing.Size(412, 489);
            this.Name = "F_DATOS_PARTICULARES";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurar datos particulares";
            this.Shown += new System.EventHandler(this.F_DATOS_PARTICULARES_Shown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVEmpresas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dGVEmpresas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMP_ID_MSP;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TIENE;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOMBRE_CORTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn T;
    }
}