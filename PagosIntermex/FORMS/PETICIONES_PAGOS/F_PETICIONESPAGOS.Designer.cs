namespace PagosIntermex
{
    partial class F_PETICIONESPAGOS
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_PETICIONESPAGOS));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_proveedores = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDateVenc = new System.Windows.Forms.DateTimePicker();
            this.btnSearchInMSP = new System.Windows.Forms.Button();
            this.dTPProximo = new System.Windows.Forms.DateTimePicker();
            this.button3 = new System.Windows.Forms.Button();
            this.chBProximoVencimeito = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.pbEmpresas = new System.Windows.Forms.ToolStripProgressBar();
            this.txtNo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtUser = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtInfo = new System.Windows.Forms.ToolStripLabel();
            this.txtPagos = new System.Windows.Forms.ToolStripLabel();
            this.dgvProgramas = new System.Windows.Forms.DataGridView();
            this.dgvp_check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvp_folio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FOLIO_CARGO_MSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FOLIO_REQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvp_fecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvp_proveedor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P_EMPRESA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvp_vencimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvp_importe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLAVE_DEPTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLAVE_DEPTO_DET = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvp_proveedor_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvp_proveedor_clave = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ESTATUS_PROC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MODIFICACION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Requisicion_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOCTO_PP_DET_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOCTO_PP_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOCTO_PP_ID_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpExcluir = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProgramas)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.dTPProximo);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.chBProximoVencimeito);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 116);
            this.panel1.TabIndex = 12;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_proveedores);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.btnFiltrar);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(549, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(297, 98);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filtrar por Proveedor";
            // 
            // txt_proveedores
            // 
            this.txt_proveedores.Location = new System.Drawing.Point(9, 38);
            this.txt_proveedores.Name = "txt_proveedores";
            this.txt_proveedores.Size = new System.Drawing.Size(282, 20);
            this.txt_proveedores.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Buscar un proveedor";
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.Location = new System.Drawing.Point(209, 65);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(82, 23);
            this.btnFiltrar.TabIndex = 7;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpExcluir);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpFecha);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpDateVenc);
            this.groupBox1.Controls.Add(this.btnSearchInMSP);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 98);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fechas";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(131, 63);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Anticipo de pago";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Traer Información de";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(131, 37);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(116, 20);
            this.dtpFecha.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fecha de vencimiento";
            // 
            // dtpDateVenc
            // 
            this.dtpDateVenc.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateVenc.Location = new System.Drawing.Point(9, 37);
            this.dtpDateVenc.Name = "dtpDateVenc";
            this.dtpDateVenc.Size = new System.Drawing.Size(109, 20);
            this.dtpDateVenc.TabIndex = 0;
            // 
            // btnSearchInMSP
            // 
            this.btnSearchInMSP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchInMSP.Location = new System.Drawing.Point(9, 63);
            this.btnSearchInMSP.Name = "btnSearchInMSP";
            this.btnSearchInMSP.Size = new System.Drawing.Size(109, 23);
            this.btnSearchInMSP.TabIndex = 3;
            this.btnSearchInMSP.Text = "Buscar Microsip";
            this.btnSearchInMSP.UseVisualStyleBackColor = true;
            this.btnSearchInMSP.Click += new System.EventHandler(this.btnSearchInMSP_Click);
            // 
            // dTPProximo
            // 
            this.dTPProximo.CustomFormat = "dd/MM/yyyy";
            this.dTPProximo.Enabled = false;
            this.dTPProximo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPProximo.Location = new System.Drawing.Point(793, 121);
            this.dTPProximo.Name = "dTPProximo";
            this.dTPProximo.Size = new System.Drawing.Size(100, 20);
            this.dTPProximo.TabIndex = 2;
            this.dTPProximo.Visible = false;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(97)))), ((int)(((byte)(238)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(878, 66);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(137, 41);
            this.button3.TabIndex = 5;
            this.button3.Text = "Crear Petición";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // chBProximoVencimeito
            // 
            this.chBProximoVencimeito.AutoSize = true;
            this.chBProximoVencimeito.Location = new System.Drawing.Point(899, 121);
            this.chBProximoVencimeito.Name = "chBProximoVencimeito";
            this.chBProximoVencimeito.Size = new System.Drawing.Size(123, 17);
            this.chBProximoVencimeito.TabIndex = 1;
            this.chBProximoVencimeito.Text = "Proximo vencimiento";
            this.chBProximoVencimeito.UseVisualStyleBackColor = true;
            this.chBProximoVencimeito.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbEmpresas,
            this.txtNo,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.txtUser,
            this.toolStripSeparator1,
            this.txtInfo,
            this.txtPagos});
            this.toolStrip1.Location = new System.Drawing.Point(0, 425);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1027, 25);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // pbEmpresas
            // 
            this.pbEmpresas.Name = "pbEmpresas";
            this.pbEmpresas.Size = new System.Drawing.Size(100, 22);
            this.pbEmpresas.Visible = false;
            // 
            // txtNo
            // 
            this.txtNo.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNo.Name = "txtNo";
            this.txtNo.Size = new System.Drawing.Size(0, 22);
            this.txtNo.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // txtInfo
            // 
            this.txtInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(0, 22);
            // 
            // txtPagos
            // 
            this.txtPagos.Name = "txtPagos";
            this.txtPagos.Size = new System.Drawing.Size(0, 22);
            // 
            // dgvProgramas
            // 
            this.dgvProgramas.AllowUserToAddRows = false;
            this.dgvProgramas.AllowUserToDeleteRows = false;
            this.dgvProgramas.AllowUserToResizeRows = false;
            this.dgvProgramas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProgramas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvp_check,
            this.dgvp_folio,
            this.FOLIO_CARGO_MSP,
            this.FOLIO_REQ,
            this.dgvp_fecha,
            this.dgvp_proveedor,
            this.P_EMPRESA,
            this.dgvp_vencimiento,
            this.dgvp_importe,
            this.CLAVE_DEPTO,
            this.CLAVE_DEPTO_DET,
            this.Column6,
            this.dgvp_proveedor_id,
            this.dgvp_proveedor_clave,
            this.ESTATUS_PROC,
            this.MODIFICACION,
            this.Requisicion_id,
            this.DOCTO_PP_DET_ID,
            this.DOCTO_PP_ID,
            this.DOCTO_PP_ID_2});
            this.dgvProgramas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProgramas.Location = new System.Drawing.Point(3, 3);
            this.dgvProgramas.MultiSelect = false;
            this.dgvProgramas.Name = "dgvProgramas";
            this.dgvProgramas.RowHeadersVisible = false;
            this.dgvProgramas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProgramas.Size = new System.Drawing.Size(1021, 303);
            this.dgvProgramas.TabIndex = 7;
            // 
            // dgvp_check
            // 
            this.dgvp_check.HeaderText = "";
            this.dgvp_check.Name = "dgvp_check";
            this.dgvp_check.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvp_check.Width = 30;
            // 
            // dgvp_folio
            // 
            this.dgvp_folio.HeaderText = "Folio";
            this.dgvp_folio.Name = "dgvp_folio";
            this.dgvp_folio.ReadOnly = true;
            this.dgvp_folio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FOLIO_CARGO_MSP
            // 
            this.FOLIO_CARGO_MSP.HeaderText = "Folio Cargo Msp";
            this.FOLIO_CARGO_MSP.Name = "FOLIO_CARGO_MSP";
            this.FOLIO_CARGO_MSP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FOLIO_CARGO_MSP.Visible = false;
            // 
            // FOLIO_REQ
            // 
            this.FOLIO_REQ.HeaderText = "Folio Req";
            this.FOLIO_REQ.Name = "FOLIO_REQ";
            this.FOLIO_REQ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvp_fecha
            // 
            this.dgvp_fecha.HeaderText = "Fecha";
            this.dgvp_fecha.Name = "dgvp_fecha";
            this.dgvp_fecha.ReadOnly = true;
            this.dgvp_fecha.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvp_proveedor
            // 
            this.dgvp_proveedor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvp_proveedor.HeaderText = "Proveedor";
            this.dgvp_proveedor.Name = "dgvp_proveedor";
            this.dgvp_proveedor.ReadOnly = true;
            this.dgvp_proveedor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // P_EMPRESA
            // 
            this.P_EMPRESA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.P_EMPRESA.HeaderText = "Empresa";
            this.P_EMPRESA.Name = "P_EMPRESA";
            this.P_EMPRESA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvp_vencimiento
            // 
            this.dgvp_vencimiento.HeaderText = "Fecha vencimiento";
            this.dgvp_vencimiento.Name = "dgvp_vencimiento";
            this.dgvp_vencimiento.ReadOnly = true;
            this.dgvp_vencimiento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvp_vencimiento.Width = 110;
            // 
            // dgvp_importe
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.NullValue = null;
            this.dgvp_importe.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvp_importe.HeaderText = "Importe";
            this.dgvp_importe.Name = "dgvp_importe";
            this.dgvp_importe.ReadOnly = true;
            this.dgvp_importe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CLAVE_DEPTO
            // 
            this.CLAVE_DEPTO.HeaderText = "CLAVE_DEPTO";
            this.CLAVE_DEPTO.Name = "CLAVE_DEPTO";
            this.CLAVE_DEPTO.Visible = false;
            // 
            // CLAVE_DEPTO_DET
            // 
            this.CLAVE_DEPTO_DET.HeaderText = "CLAVE_DEPTO_DET";
            this.CLAVE_DEPTO_DET.Name = "CLAVE_DEPTO_DET";
            this.CLAVE_DEPTO_DET.Visible = false;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Importe Autorizado";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Visible = false;
            this.Column6.Width = 106;
            // 
            // dgvp_proveedor_id
            // 
            this.dgvp_proveedor_id.HeaderText = "dgvp_proveedor_id";
            this.dgvp_proveedor_id.Name = "dgvp_proveedor_id";
            this.dgvp_proveedor_id.ReadOnly = true;
            this.dgvp_proveedor_id.Visible = false;
            // 
            // dgvp_proveedor_clave
            // 
            this.dgvp_proveedor_clave.HeaderText = "dgvp_proveedor_clave";
            this.dgvp_proveedor_clave.Name = "dgvp_proveedor_clave";
            this.dgvp_proveedor_clave.ReadOnly = true;
            this.dgvp_proveedor_clave.Visible = false;
            // 
            // ESTATUS_PROC
            // 
            this.ESTATUS_PROC.HeaderText = "ESTATUS_PROC";
            this.ESTATUS_PROC.Name = "ESTATUS_PROC";
            this.ESTATUS_PROC.ReadOnly = true;
            this.ESTATUS_PROC.Visible = false;
            // 
            // MODIFICACION
            // 
            this.MODIFICACION.HeaderText = "MODIFICACION";
            this.MODIFICACION.Name = "MODIFICACION";
            this.MODIFICACION.Visible = false;
            // 
            // Requisicion_id
            // 
            this.Requisicion_id.HeaderText = "Requisicion_id";
            this.Requisicion_id.Name = "Requisicion_id";
            this.Requisicion_id.Visible = false;
            // 
            // DOCTO_PP_DET_ID
            // 
            this.DOCTO_PP_DET_ID.HeaderText = "DOCTO_PP_DET_ID";
            this.DOCTO_PP_DET_ID.Name = "DOCTO_PP_DET_ID";
            this.DOCTO_PP_DET_ID.Visible = false;
            // 
            // DOCTO_PP_ID
            // 
            this.DOCTO_PP_ID.HeaderText = "DOCTO_PP_ID";
            this.DOCTO_PP_ID.Name = "DOCTO_PP_ID";
            this.DOCTO_PP_ID.Visible = false;
            // 
            // DOCTO_PP_ID_2
            // 
            this.DOCTO_PP_ID_2.HeaderText = "DOCTO_PP_ID_2";
            this.DOCTO_PP_ID_2.Name = "DOCTO_PP_ID_2";
            this.DOCTO_PP_ID_2.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvProgramas);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 116);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(1027, 309);
            this.panel2.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(274, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(230, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Excluir pagos vencidos anteriores a esta fecha:";
            // 
            // dtpExcluir
            // 
            this.dtpExcluir.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExcluir.Location = new System.Drawing.Point(277, 38);
            this.dtpExcluir.Name = "dtpExcluir";
            this.dtpExcluir.Size = new System.Drawing.Size(116, 20);
            this.dtpExcluir.TabIndex = 12;
            this.dtpExcluir.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // F_PETICIONESPAGOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_PETICIONESPAGOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_PETICIONESPAGOS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_PETICIONESPAGOS_FormClosing);
            this.Load += new System.EventHandler(this.F_PETICIONESPAGOS_Load);
            this.Shown += new System.EventHandler(this.F_PETICIONESPAGOS_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProgramas)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txt_proveedores;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDateVenc;
        private System.Windows.Forms.Button btnSearchInMSP;
        private System.Windows.Forms.DateTimePicker dTPProximo;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox chBProximoVencimeito;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripProgressBar pbEmpresas;
        private System.Windows.Forms.ToolStripLabel txtNo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel txtUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel txtInfo;
        private System.Windows.Forms.ToolStripLabel txtPagos;
        private System.Windows.Forms.DataGridView dgvProgramas;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvp_check;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvp_folio;
        private System.Windows.Forms.DataGridViewTextBoxColumn FOLIO_CARGO_MSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn FOLIO_REQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvp_fecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvp_proveedor;
        private System.Windows.Forms.DataGridViewTextBoxColumn P_EMPRESA;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvp_vencimiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvp_importe;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLAVE_DEPTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLAVE_DEPTO_DET;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvp_proveedor_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvp_proveedor_clave;
        private System.Windows.Forms.DataGridViewTextBoxColumn ESTATUS_PROC;
        private System.Windows.Forms.DataGridViewTextBoxColumn MODIFICACION;
        private System.Windows.Forms.DataGridViewTextBoxColumn Requisicion_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTO_PP_DET_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTO_PP_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTO_PP_ID_2;
        private System.Windows.Forms.DateTimePicker dtpExcluir;
        private System.Windows.Forms.Label label4;
    }
}