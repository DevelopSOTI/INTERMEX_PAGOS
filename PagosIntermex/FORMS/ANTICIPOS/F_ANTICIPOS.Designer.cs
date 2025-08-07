namespace PagosIntermex
{
    partial class F_ANTICIPOS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_ANTICIPOS));
            this.dgvAnticipo = new System.Windows.Forms.DataGridView();
            this.btn_aceptar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Fecha = new System.Windows.Forms.DateTimePicker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtUser = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CHECK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.c_proveedor_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.c_proveedor_clave = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.c_proveedor_nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Requisicion_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Folio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOTAL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Empresa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prioridad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus_general = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnticipo)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAnticipo
            // 
            this.dgvAnticipo.AllowUserToAddRows = false;
            this.dgvAnticipo.AllowUserToDeleteRows = false;
            this.dgvAnticipo.AllowUserToResizeRows = false;
            this.dgvAnticipo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAnticipo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CHECK,
            this.c_proveedor_id,
            this.c_proveedor_clave,
            this.c_proveedor_nombre,
            this.Requisicion_id,
            this.Folio,
            this.TOTAL,
            this.Empresa,
            this.Prioridad,
            this.Estatus_general});
            this.dgvAnticipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAnticipo.Location = new System.Drawing.Point(3, 3);
            this.dgvAnticipo.MultiSelect = false;
            this.dgvAnticipo.Name = "dgvAnticipo";
            this.dgvAnticipo.RowHeadersVisible = false;
            this.dgvAnticipo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAnticipo.Size = new System.Drawing.Size(866, 369);
            this.dgvAnticipo.TabIndex = 3;
            // 
            // btn_aceptar
            // 
            this.btn_aceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(97)))), ((int)(((byte)(238)))));
            this.btn_aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_aceptar.Location = new System.Drawing.Point(703, 21);
            this.btn_aceptar.Name = "btn_aceptar";
            this.btn_aceptar.Size = new System.Drawing.Size(75, 23);
            this.btn_aceptar.TabIndex = 2;
            this.btn_aceptar.Text = "Aceptar";
            this.btn_aceptar.UseVisualStyleBackColor = false;
            this.btn_aceptar.Click += new System.EventHandler(this.btn_aceptar_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Fecha);
            this.panel1.Controls.Add(this.btn_aceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(872, 61);
            this.panel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Fecha";
            // 
            // Fecha
            // 
            this.Fecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Fecha.Location = new System.Drawing.Point(108, 24);
            this.Fecha.MaxDate = new System.DateTime(2050, 12, 31, 0, 0, 0, 0);
            this.Fecha.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.Fecha.Name = "Fecha";
            this.Fecha.Size = new System.Drawing.Size(79, 20);
            this.Fecha.TabIndex = 3;
            this.Fecha.Value = new System.DateTime(2022, 11, 22, 16, 2, 32, 0);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtUser,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 436);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(872, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(50, 22);
            this.toolStripLabel1.Text = "Usuario:";
            // 
            // txtUser
            // 
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvAnticipo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 61);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(872, 375);
            this.panel2.TabIndex = 8;
            // 
            // CHECK
            // 
            this.CHECK.HeaderText = "Seleccionar";
            this.CHECK.Name = "CHECK";
            // 
            // c_proveedor_id
            // 
            this.c_proveedor_id.HeaderText = "";
            this.c_proveedor_id.Name = "c_proveedor_id";
            this.c_proveedor_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.c_proveedor_id.Visible = false;
            // 
            // c_proveedor_clave
            // 
            this.c_proveedor_clave.HeaderText = "Clave Proveedor";
            this.c_proveedor_clave.Name = "c_proveedor_clave";
            this.c_proveedor_clave.ReadOnly = true;
            this.c_proveedor_clave.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.c_proveedor_clave.Width = 130;
            // 
            // c_proveedor_nombre
            // 
            this.c_proveedor_nombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.c_proveedor_nombre.HeaderText = "Nombre Proveedor";
            this.c_proveedor_nombre.Name = "c_proveedor_nombre";
            this.c_proveedor_nombre.ReadOnly = true;
            this.c_proveedor_nombre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Requisicion_id
            // 
            this.Requisicion_id.HeaderText = "REQ_ID";
            this.Requisicion_id.Name = "Requisicion_id";
            this.Requisicion_id.Visible = false;
            // 
            // Folio
            // 
            this.Folio.HeaderText = "Folio";
            this.Folio.Name = "Folio";
            this.Folio.ReadOnly = true;
            this.Folio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TOTAL
            // 
            this.TOTAL.HeaderText = "Importe";
            this.TOTAL.Name = "TOTAL";
            this.TOTAL.ReadOnly = true;
            this.TOTAL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Empresa
            // 
            this.Empresa.HeaderText = "Empresa";
            this.Empresa.Name = "Empresa";
            this.Empresa.ReadOnly = true;
            this.Empresa.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Prioridad
            // 
            this.Prioridad.HeaderText = "Prioridad";
            this.Prioridad.Name = "Prioridad";
            this.Prioridad.ReadOnly = true;
            this.Prioridad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Estatus_general
            // 
            this.Estatus_general.HeaderText = "Tipo";
            this.Estatus_general.Name = "Estatus_general";
            this.Estatus_general.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // F_ANTICIPOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(872, 461);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F_ANTICIPOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Anticipo de pago";
            this.Load += new System.EventHandler(this.F_ANTICIPOS_Load);
            this.Shown += new System.EventHandler(this.F_ANTICIPOS_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnticipo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvAnticipo;
        private System.Windows.Forms.Button btn_aceptar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel txtUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker Fecha;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CHECK;
        private System.Windows.Forms.DataGridViewTextBoxColumn c_proveedor_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn c_proveedor_clave;
        private System.Windows.Forms.DataGridViewTextBoxColumn c_proveedor_nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Requisicion_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Folio;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTAL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Empresa;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prioridad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus_general;
    }
}