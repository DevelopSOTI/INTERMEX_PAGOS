namespace PagosIntermex
{
    partial class F_DETALLE_PETICIONES
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_DETALLE_PETICIONES));
            this.panel3 = new System.Windows.Forms.Panel();
            this.pAgregarPagos = new System.Windows.Forms.Panel();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.pGuardarCambios = new System.Windows.Forms.Panel();
            this.bCambios = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtUser = new System.Windows.Forms.ToolStripLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dGVDetPagos = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.marcarComoCanceladoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarRenglonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DOCTO_PR_DET_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REQUISICION_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROVEEDOR_CLAVE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FOLIO_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FECHA_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROVEEDOR_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P_EMPRESA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_PROVEEDOR_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FECHA_VEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IMPORTE_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IMPORTE_AUT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ESTATUS_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_COMENTARIOS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_IMAGEN = new System.Windows.Forms.DataGridViewImageColumn();
            this.C_PAGO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3.SuspendLayout();
            this.pAgregarPagos.SuspendLayout();
            this.pGuardarCambios.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVDetPagos)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel3.Controls.Add(this.pAgregarPagos);
            this.panel3.Controls.Add(this.pGuardarCambios);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.ForeColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(2);
            this.panel3.Size = new System.Drawing.Size(800, 35);
            this.panel3.TabIndex = 11;
            // 
            // pAgregarPagos
            // 
            this.pAgregarPagos.Controls.Add(this.btnAgregar);
            this.pAgregarPagos.Dock = System.Windows.Forms.DockStyle.Right;
            this.pAgregarPagos.Location = new System.Drawing.Point(584, 2);
            this.pAgregarPagos.Name = "pAgregarPagos";
            this.pAgregarPagos.Size = new System.Drawing.Size(106, 31);
            this.pAgregarPagos.TabIndex = 3;
            this.pAgregarPagos.Visible = false;
            // 
            // btnAgregar
            // 
            this.btnAgregar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Location = new System.Drawing.Point(0, 0);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(106, 31);
            this.btnAgregar.TabIndex = 7;
            this.btnAgregar.Text = "Agregar Peticiones";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Visible = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // pGuardarCambios
            // 
            this.pGuardarCambios.Controls.Add(this.bCambios);
            this.pGuardarCambios.Dock = System.Windows.Forms.DockStyle.Right;
            this.pGuardarCambios.Location = new System.Drawing.Point(690, 2);
            this.pGuardarCambios.Name = "pGuardarCambios";
            this.pGuardarCambios.Size = new System.Drawing.Size(108, 31);
            this.pGuardarCambios.TabIndex = 2;
            this.pGuardarCambios.Visible = false;
            // 
            // bCambios
            // 
            this.bCambios.Dock = System.Windows.Forms.DockStyle.Right;
            this.bCambios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCambios.ForeColor = System.Drawing.Color.White;
            this.bCambios.Location = new System.Drawing.Point(8, 0);
            this.bCambios.Name = "bCambios";
            this.bCambios.Size = new System.Drawing.Size(100, 31);
            this.bCambios.TabIndex = 5;
            this.bCambios.Text = "Guardar cambios";
            this.bCambios.UseVisualStyleBackColor = true;
            this.bCambios.Visible = false;
            this.bCambios.Click += new System.EventHandler(this.bCambios_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Detalle de pagos";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtUser});
            this.toolStrip1.Location = new System.Drawing.Point(0, 425);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 12;
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
            // panel1
            // 
            this.panel1.Controls.Add(this.dGVDetPagos);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(800, 390);
            this.panel1.TabIndex = 13;
            // 
            // dGVDetPagos
            // 
            this.dGVDetPagos.AllowUserToAddRows = false;
            this.dGVDetPagos.AllowUserToDeleteRows = false;
            this.dGVDetPagos.AllowUserToResizeRows = false;
            this.dGVDetPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dGVDetPagos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DOCTO_PR_DET_ID,
            this.REQUISICION_ID,
            this.PROVEEDOR_CLAVE,
            this.FOLIO_P,
            this.FECHA_P,
            this.PROVEEDOR_P,
            this.P_EMPRESA,
            this.C_PROVEEDOR_ID,
            this.FECHA_VEN,
            this.IMPORTE_P,
            this.IMPORTE_AUT,
            this.ESTATUS_P,
            this.C_COMENTARIOS,
            this.C_IMAGEN,
            this.C_PAGO});
            this.dGVDetPagos.ContextMenuStrip = this.contextMenuStrip1;
            this.dGVDetPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVDetPagos.Location = new System.Drawing.Point(3, 3);
            this.dGVDetPagos.MultiSelect = false;
            this.dGVDetPagos.Name = "dGVDetPagos";
            this.dGVDetPagos.ReadOnly = true;
            this.dGVDetPagos.RowHeadersVisible = false;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.dGVDetPagos.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dGVDetPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVDetPagos.Size = new System.Drawing.Size(794, 384);
            this.dGVDetPagos.TabIndex = 2;
            this.dGVDetPagos.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dGVDetPagos_CellMouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.marcarComoCanceladoToolStripMenuItem,
            this.eliminarRenglonToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(203, 48);
            // 
            // marcarComoCanceladoToolStripMenuItem
            // 
            this.marcarComoCanceladoToolStripMenuItem.Name = "marcarComoCanceladoToolStripMenuItem";
            this.marcarComoCanceladoToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.marcarComoCanceladoToolStripMenuItem.Text = "Marcar como cancelado";
            this.marcarComoCanceladoToolStripMenuItem.Visible = false;
            // 
            // eliminarRenglonToolStripMenuItem
            // 
            this.eliminarRenglonToolStripMenuItem.Name = "eliminarRenglonToolStripMenuItem";
            this.eliminarRenglonToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.eliminarRenglonToolStripMenuItem.Text = "Eliminar renglón";
            this.eliminarRenglonToolStripMenuItem.Visible = false;
            this.eliminarRenglonToolStripMenuItem.Click += new System.EventHandler(this.eliminarRenglonToolStripMenuItem_Click);
            // 
            // DOCTO_PR_DET_ID
            // 
            this.DOCTO_PR_DET_ID.HeaderText = "DOCTO_PR_DET_ID";
            this.DOCTO_PR_DET_ID.Name = "DOCTO_PR_DET_ID";
            this.DOCTO_PR_DET_ID.ReadOnly = true;
            this.DOCTO_PR_DET_ID.Visible = false;
            // 
            // REQUISICION_ID
            // 
            this.REQUISICION_ID.HeaderText = "REQUISICION_ID";
            this.REQUISICION_ID.Name = "REQUISICION_ID";
            this.REQUISICION_ID.ReadOnly = true;
            this.REQUISICION_ID.Visible = false;
            // 
            // PROVEEDOR_CLAVE
            // 
            this.PROVEEDOR_CLAVE.HeaderText = "PROVEEDOR_CLAVE";
            this.PROVEEDOR_CLAVE.Name = "PROVEEDOR_CLAVE";
            this.PROVEEDOR_CLAVE.ReadOnly = true;
            this.PROVEEDOR_CLAVE.Visible = false;
            // 
            // FOLIO_P
            // 
            this.FOLIO_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FOLIO_P.HeaderText = "Folio";
            this.FOLIO_P.Name = "FOLIO_P";
            this.FOLIO_P.ReadOnly = true;
            this.FOLIO_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FOLIO_P.Width = 35;
            // 
            // FECHA_P
            // 
            this.FECHA_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FECHA_P.HeaderText = "Fecha";
            this.FECHA_P.Name = "FECHA_P";
            this.FECHA_P.ReadOnly = true;
            this.FECHA_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FECHA_P.Width = 43;
            // 
            // PROVEEDOR_P
            // 
            this.PROVEEDOR_P.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PROVEEDOR_P.HeaderText = "Proveedor";
            this.PROVEEDOR_P.Name = "PROVEEDOR_P";
            this.PROVEEDOR_P.ReadOnly = true;
            this.PROVEEDOR_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // P_EMPRESA
            // 
            this.P_EMPRESA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.P_EMPRESA.HeaderText = "Empresa";
            this.P_EMPRESA.Name = "P_EMPRESA";
            this.P_EMPRESA.ReadOnly = true;
            this.P_EMPRESA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // C_PROVEEDOR_ID
            // 
            this.C_PROVEEDOR_ID.HeaderText = "Proveedor ID";
            this.C_PROVEEDOR_ID.Name = "C_PROVEEDOR_ID";
            this.C_PROVEEDOR_ID.ReadOnly = true;
            this.C_PROVEEDOR_ID.Visible = false;
            // 
            // FECHA_VEN
            // 
            this.FECHA_VEN.HeaderText = "Fecha vencimiento";
            this.FECHA_VEN.Name = "FECHA_VEN";
            this.FECHA_VEN.ReadOnly = true;
            this.FECHA_VEN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FECHA_VEN.Width = 110;
            // 
            // IMPORTE_P
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.IMPORTE_P.DefaultCellStyle = dataGridViewCellStyle1;
            this.IMPORTE_P.HeaderText = "Importe";
            this.IMPORTE_P.Name = "IMPORTE_P";
            this.IMPORTE_P.ReadOnly = true;
            this.IMPORTE_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IMPORTE_P.Width = 106;
            // 
            // IMPORTE_AUT
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "0";
            this.IMPORTE_AUT.DefaultCellStyle = dataGridViewCellStyle2;
            this.IMPORTE_AUT.HeaderText = "Importe Autorizado";
            this.IMPORTE_AUT.Name = "IMPORTE_AUT";
            this.IMPORTE_AUT.ReadOnly = true;
            this.IMPORTE_AUT.Visible = false;
            this.IMPORTE_AUT.Width = 106;
            // 
            // ESTATUS_P
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ESTATUS_P.DefaultCellStyle = dataGridViewCellStyle3;
            this.ESTATUS_P.HeaderText = "Estatus";
            this.ESTATUS_P.Name = "ESTATUS_P";
            this.ESTATUS_P.ReadOnly = true;
            this.ESTATUS_P.Visible = false;
            this.ESTATUS_P.Width = 70;
            // 
            // C_COMENTARIOS
            // 
            this.C_COMENTARIOS.HeaderText = "Comentarios";
            this.C_COMENTARIOS.Name = "C_COMENTARIOS";
            this.C_COMENTARIOS.ReadOnly = true;
            this.C_COMENTARIOS.Visible = false;
            // 
            // C_IMAGEN
            // 
            this.C_IMAGEN.HeaderText = "";
            this.C_IMAGEN.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.C_IMAGEN.Name = "C_IMAGEN";
            this.C_IMAGEN.ReadOnly = true;
            this.C_IMAGEN.Visible = false;
            this.C_IMAGEN.Width = 30;
            // 
            // C_PAGO
            // 
            this.C_PAGO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.C_PAGO.HeaderText = "Pago";
            this.C_PAGO.Name = "C_PAGO";
            this.C_PAGO.ReadOnly = true;
            this.C_PAGO.Visible = false;
            this.C_PAGO.Width = 57;
            // 
            // F_DETALLE_PETICIONES
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_DETALLE_PETICIONES";
            this.Text = "Detalle de peticiones";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_DETALLE_PETICIONES_FormClosing);
            this.Load += new System.EventHandler(this.F_DETALLE_PETICIONES_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pAgregarPagos.ResumeLayout(false);
            this.pGuardarCambios.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVDetPagos)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pAgregarPagos;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Panel pGuardarCambios;
        private System.Windows.Forms.Button bCambios;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel txtUser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dGVDetPagos;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem marcarComoCanceladoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eliminarRenglonToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTO_PR_DET_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn REQUISICION_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROVEEDOR_CLAVE;
        private System.Windows.Forms.DataGridViewTextBoxColumn FOLIO_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn FECHA_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROVEEDOR_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn P_EMPRESA;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_PROVEEDOR_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FECHA_VEN;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMPORTE_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMPORTE_AUT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ESTATUS_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_COMENTARIOS;
        private System.Windows.Forms.DataGridViewImageColumn C_IMAGEN;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_PAGO;
    }
}