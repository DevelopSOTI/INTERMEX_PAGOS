namespace PagosIntermex
{
    partial class F_DETALLEPROGRAMACIONPAGOS
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_DETALLEPROGRAMACIONPAGOS));
            this.label1 = new System.Windows.Forms.Label();
            this.dGVDetPagos = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.marcarComoCanceladoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarRenglonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.bCambios = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtUser = new System.Windows.Forms.ToolStripLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvUser = new System.Windows.Forms.DataGridView();
            this.AUTORIZADO_AUT = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.USER_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USUARIO_AUT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USUARIO_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NIVEL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pAgregarPagos = new System.Windows.Forms.Panel();
            this.pGuardarCambios = new System.Windows.Forms.Panel();
            this.pAutorizarPagos = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.DOCTO_PR_DET_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.dGVDetPagos)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pAgregarPagos.SuspendLayout();
            this.pGuardarCambios.SuspendLayout();
            this.pAutorizarPagos.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
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
            // dGVDetPagos
            // 
            this.dGVDetPagos.AllowUserToAddRows = false;
            this.dGVDetPagos.AllowUserToDeleteRows = false;
            this.dGVDetPagos.AllowUserToResizeRows = false;
            this.dGVDetPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dGVDetPagos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DOCTO_PR_DET_ID,
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
            this.dGVDetPagos.RowHeadersVisible = false;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.dGVDetPagos.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dGVDetPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVDetPagos.Size = new System.Drawing.Size(819, 404);
            this.dGVDetPagos.TabIndex = 1;
            this.dGVDetPagos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVDetPagos_CellClick);
            this.dGVDetPagos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVDetPagos_CellContentClick);
            this.dGVDetPagos.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVDetPagos_CellContentDoubleClick);
            this.dGVDetPagos.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVDetPagos_CellEndEdit);
            this.dGVDetPagos.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dGVDetPagos_CellMouseDown);
            this.dGVDetPagos.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dGVDetPagos_EditingControlShowing);
            this.dGVDetPagos.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVDetPagos_RowEnter);
            this.dGVDetPagos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dGVDetPagos_KeyPress);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.marcarComoCanceladoToolStripMenuItem,
            this.eliminarRenglonToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(203, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // marcarComoCanceladoToolStripMenuItem
            // 
            this.marcarComoCanceladoToolStripMenuItem.Name = "marcarComoCanceladoToolStripMenuItem";
            this.marcarComoCanceladoToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.marcarComoCanceladoToolStripMenuItem.Text = "Marcar como cancelado";
            this.marcarComoCanceladoToolStripMenuItem.Visible = false;
            this.marcarComoCanceladoToolStripMenuItem.Click += new System.EventHandler(this.marcarComoCanceladoToolStripMenuItem_Click);
            // 
            // eliminarRenglonToolStripMenuItem
            // 
            this.eliminarRenglonToolStripMenuItem.Name = "eliminarRenglonToolStripMenuItem";
            this.eliminarRenglonToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.eliminarRenglonToolStripMenuItem.Text = "Eliminar renglón";
            this.eliminarRenglonToolStripMenuItem.Visible = false;
            this.eliminarRenglonToolStripMenuItem.Click += new System.EventHandler(this.eliminarRenglonToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Comentarios del corporativo";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(7, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "Autorizar pagos";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(3, 328);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(283, 114);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // btnAgregar
            // 
            this.btnAgregar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Location = new System.Drawing.Point(8, 0);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(98, 31);
            this.btnAgregar.TabIndex = 7;
            this.btnAgregar.Text = "Agregar Pagos";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Visible = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtUser});
            this.toolStrip1.Location = new System.Drawing.Point(0, 445);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1114, 25);
            this.toolStrip1.TabIndex = 8;
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
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel2.Controls.Add(this.label2);
            this.panel2.ForeColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(3, 292);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(283, 33);
            this.panel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel1.Controls.Add(this.dgvUser);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(825, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 32, 3, 3);
            this.panel1.Size = new System.Drawing.Size(289, 445);
            this.panel1.TabIndex = 9;
            // 
            // dgvUser
            // 
            this.dgvUser.AllowUserToAddRows = false;
            this.dgvUser.AllowUserToDeleteRows = false;
            this.dgvUser.AllowUserToResizeRows = false;
            this.dgvUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AUTORIZADO_AUT,
            this.USER_NAME,
            this.USUARIO_AUT,
            this.USUARIO_ID,
            this.NIVEL});
            this.dgvUser.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvUser.Location = new System.Drawing.Point(4, 38);
            this.dgvUser.Name = "dgvUser";
            this.dgvUser.RowHeadersVisible = false;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.dgvUser.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUser.Size = new System.Drawing.Size(282, 248);
            this.dgvUser.TabIndex = 7;
            // 
            // AUTORIZADO_AUT
            // 
            this.AUTORIZADO_AUT.HeaderText = "Autorizado";
            this.AUTORIZADO_AUT.Name = "AUTORIZADO_AUT";
            // 
            // USER_NAME
            // 
            this.USER_NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.USER_NAME.HeaderText = "Nombre";
            this.USER_NAME.Name = "USER_NAME";
            // 
            // USUARIO_AUT
            // 
            this.USUARIO_AUT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.USUARIO_AUT.HeaderText = "Usuario";
            this.USUARIO_AUT.Name = "USUARIO_AUT";
            this.USUARIO_AUT.Visible = false;
            // 
            // USUARIO_ID
            // 
            this.USUARIO_ID.HeaderText = "Usuario_ID";
            this.USUARIO_ID.Name = "USUARIO_ID";
            this.USUARIO_ID.ReadOnly = true;
            this.USUARIO_ID.Visible = false;
            // 
            // NIVEL
            // 
            this.NIVEL.HeaderText = "NIVEL";
            this.NIVEL.Name = "NIVEL";
            this.NIVEL.ReadOnly = true;
            this.NIVEL.Visible = false;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel5.Controls.Add(this.label3);
            this.panel5.ForeColor = System.Drawing.Color.White;
            this.panel5.Location = new System.Drawing.Point(4, 2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(283, 33);
            this.panel5.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Usuarios quienes autorizarán";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel3.Controls.Add(this.pAgregarPagos);
            this.panel3.Controls.Add(this.pGuardarCambios);
            this.panel3.Controls.Add(this.pAutorizarPagos);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.ForeColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(2);
            this.panel3.Size = new System.Drawing.Size(825, 35);
            this.panel3.TabIndex = 10;
            // 
            // pAgregarPagos
            // 
            this.pAgregarPagos.Controls.Add(this.btnAgregar);
            this.pAgregarPagos.Dock = System.Windows.Forms.DockStyle.Right;
            this.pAgregarPagos.Location = new System.Drawing.Point(502, 2);
            this.pAgregarPagos.Name = "pAgregarPagos";
            this.pAgregarPagos.Size = new System.Drawing.Size(106, 31);
            this.pAgregarPagos.TabIndex = 3;
            this.pAgregarPagos.Visible = false;
            // 
            // pGuardarCambios
            // 
            this.pGuardarCambios.Controls.Add(this.bCambios);
            this.pGuardarCambios.Dock = System.Windows.Forms.DockStyle.Right;
            this.pGuardarCambios.Location = new System.Drawing.Point(608, 2);
            this.pGuardarCambios.Name = "pGuardarCambios";
            this.pGuardarCambios.Size = new System.Drawing.Size(108, 31);
            this.pGuardarCambios.TabIndex = 2;
            this.pGuardarCambios.Visible = false;
            // 
            // pAutorizarPagos
            // 
            this.pAutorizarPagos.Controls.Add(this.button1);
            this.pAutorizarPagos.Dock = System.Windows.Forms.DockStyle.Right;
            this.pAutorizarPagos.Location = new System.Drawing.Point(716, 2);
            this.pAutorizarPagos.Name = "pAutorizarPagos";
            this.pAutorizarPagos.Size = new System.Drawing.Size(107, 31);
            this.pAutorizarPagos.TabIndex = 1;
            this.pAutorizarPagos.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dGVDetPagos);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 35);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(3);
            this.panel4.Size = new System.Drawing.Size(825, 410);
            this.panel4.TabIndex = 11;
            // 
            // DOCTO_PR_DET_ID
            // 
            this.DOCTO_PR_DET_ID.HeaderText = "DOCTO_PR_DET_ID";
            this.DOCTO_PR_DET_ID.Name = "DOCTO_PR_DET_ID";
            this.DOCTO_PR_DET_ID.Visible = false;
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
            this.IMPORTE_AUT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IMPORTE_AUT.Width = 106;
            // 
            // ESTATUS_P
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ESTATUS_P.DefaultCellStyle = dataGridViewCellStyle3;
            this.ESTATUS_P.HeaderText = "Estatus";
            this.ESTATUS_P.Name = "ESTATUS_P";
            this.ESTATUS_P.ReadOnly = true;
            this.ESTATUS_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ESTATUS_P.Width = 70;
            // 
            // C_COMENTARIOS
            // 
            this.C_COMENTARIOS.HeaderText = "Comentarios";
            this.C_COMENTARIOS.Name = "C_COMENTARIOS";
            this.C_COMENTARIOS.ReadOnly = true;
            this.C_COMENTARIOS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.C_COMENTARIOS.Visible = false;
            // 
            // C_IMAGEN
            // 
            this.C_IMAGEN.HeaderText = "";
            this.C_IMAGEN.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.C_IMAGEN.Name = "C_IMAGEN";
            this.C_IMAGEN.ReadOnly = true;
            this.C_IMAGEN.Width = 30;
            // 
            // C_PAGO
            // 
            this.C_PAGO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.C_PAGO.HeaderText = "Pago";
            this.C_PAGO.Name = "C_PAGO";
            this.C_PAGO.ReadOnly = true;
            this.C_PAGO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.C_PAGO.Visible = false;
            this.C_PAGO.Width = 38;
            // 
            // F_DETALLEPROGRAMACIONPAGOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1114, 470);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1130, 400);
            this.Name = "F_DETALLEPROGRAMACIONPAGOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalle de programación de pagos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_DETALLEPROGRAMACIONPAGOS_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.F_DETALLEPROGRAMACIONPAGOS_FormClosed);
            this.Load += new System.EventHandler(this.F_DETALLEPROGRAMACIONPAGOS_Load);
            this.Shown += new System.EventHandler(this.F_DETALLEPROGRAMACIONPAGOS_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dGVDetPagos)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pAgregarPagos.ResumeLayout(false);
            this.pGuardarCambios.ResumeLayout(false);
            this.pAutorizarPagos.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dGVDetPagos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bCambios;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem marcarComoCanceladoToolStripMenuItem;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.ToolStripMenuItem eliminarRenglonToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pAgregarPagos;
        private System.Windows.Forms.Panel pGuardarCambios;
        private System.Windows.Forms.Panel pAutorizarPagos;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel txtUser;
        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AUTORIZADO_AUT;
        private System.Windows.Forms.DataGridViewTextBoxColumn USER_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn USUARIO_AUT;
        private System.Windows.Forms.DataGridViewTextBoxColumn USUARIO_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NIVEL;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTO_PR_DET_ID;
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