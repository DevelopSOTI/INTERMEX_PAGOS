namespace PagosIntermex
{
    partial class F_PROGRAMACIONPAGOS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_PROGRAMACIONPAGOS));
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDateVenc = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSearchInMSP = new System.Windows.Forms.Button();
            this.dgvProgramas = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.chBProximoVencimeito = new System.Windows.Forms.CheckBox();
            this.dTPProximo = new System.Windows.Forms.DateTimePicker();
            this.txt_proveedores = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_mostrarseleccionado = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bntAgregarUsuarios = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbFiltrarUsuario = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbPrivilegio = new System.Windows.Forms.ComboBox();
            this.cbDpto = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.pbEmpresas = new System.Windows.Forms.ToolStripProgressBar();
            this.txtNo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtUser = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtInfo = new System.Windows.Forms.ToolStripLabel();
            this.txtPagos = new System.Windows.Forms.ToolStripLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.checkUsuario = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.USUARIO_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NOMBRE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEPARTAMENTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRIVILEGIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USUARIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvp_check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DOCTO_PP_ID_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvProgramas)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
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
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(131, 63);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Anticipo de pago";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSearchInMSP
            // 
            this.btnSearchInMSP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchInMSP.Location = new System.Drawing.Point(9, 63);
            this.btnSearchInMSP.Name = "btnSearchInMSP";
            this.btnSearchInMSP.Size = new System.Drawing.Size(109, 23);
            this.btnSearchInMSP.TabIndex = 3;
            this.btnSearchInMSP.Text = "Buscar Peticiones";
            this.btnSearchInMSP.UseVisualStyleBackColor = true;
            this.btnSearchInMSP.Click += new System.EventHandler(this.btnSearchInMSP_Click);
            // 
            // dgvProgramas
            // 
            this.dgvProgramas.AllowUserToAddRows = false;
            this.dgvProgramas.AllowUserToDeleteRows = false;
            this.dgvProgramas.AllowUserToResizeRows = false;
            this.dgvProgramas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProgramas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvp_check,
            this.DOCTO_PP_ID_2,
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
            this.DOCTO_PP_ID});
            this.dgvProgramas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProgramas.Location = new System.Drawing.Point(0, 0);
            this.dgvProgramas.MultiSelect = false;
            this.dgvProgramas.Name = "dgvProgramas";
            this.dgvProgramas.RowHeadersVisible = false;
            this.dgvProgramas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProgramas.Size = new System.Drawing.Size(821, 305);
            this.dgvProgramas.TabIndex = 7;
            this.dgvProgramas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProgramas_CellContentClick);
            this.dgvProgramas.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProgramas_CellContentDoubleClick);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(97)))), ((int)(((byte)(238)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(907, 66);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(137, 41);
            this.button3.TabIndex = 5;
            this.button3.Text = "Crear Programación";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(969, 21);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "Cancelar";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
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
            this.chBProximoVencimeito.CheckedChanged += new System.EventHandler(this.chBProximoVencimeito_CheckedChanged);
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
            // txt_proveedores
            // 
            this.txt_proveedores.Location = new System.Drawing.Point(9, 38);
            this.txt_proveedores.Name = "txt_proveedores";
            this.txt_proveedores.Size = new System.Drawing.Size(282, 20);
            this.txt_proveedores.TabIndex = 8;
            this.txt_proveedores.TextChanged += new System.EventHandler(this.txt_proveedores_TextChanged);
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
            // btn_mostrarseleccionado
            // 
            this.btn_mostrarseleccionado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_mostrarseleccionado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_mostrarseleccionado.Location = new System.Drawing.Point(907, 23);
            this.btn_mostrarseleccionado.Name = "btn_mostrarseleccionado";
            this.btn_mostrarseleccionado.Size = new System.Drawing.Size(137, 23);
            this.btn_mostrarseleccionado.TabIndex = 10;
            this.btn_mostrarseleccionado.Text = "Mostrar lo seleccionado";
            this.btn_mostrarseleccionado.UseVisualStyleBackColor = true;
            this.btn_mostrarseleccionado.Visible = false;
            this.btn_mostrarseleccionado.Click += new System.EventHandler(this.btn_mostrarseleccionado_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel1.Controls.Add(this.bntAgregarUsuarios);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btn_mostrarseleccionado);
            this.panel1.Controls.Add(this.dTPProximo);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.chBProximoVencimeito);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1056, 116);
            this.panel1.TabIndex = 11;
            // 
            // bntAgregarUsuarios
            // 
            this.bntAgregarUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bntAgregarUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntAgregarUsuarios.Location = new System.Drawing.Point(764, 84);
            this.bntAgregarUsuarios.Name = "bntAgregarUsuarios";
            this.bntAgregarUsuarios.Size = new System.Drawing.Size(137, 23);
            this.bntAgregarUsuarios.TabIndex = 14;
            this.bntAgregarUsuarios.Text = "Agregar Usuarios";
            this.bntAgregarUsuarios.UseVisualStyleBackColor = true;
            this.bntAgregarUsuarios.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_proveedores);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.btnFiltrar);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(286, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(297, 98);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filtrar por Proveedor";
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbFiltrarUsuario);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbPrivilegio);
            this.groupBox2.Controls.Add(this.cbDpto);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(589, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(122, 98);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filtros Usuarios";
            this.groupBox2.Visible = false;
            // 
            // cbFiltrarUsuario
            // 
            this.cbFiltrarUsuario.AutoSize = true;
            this.cbFiltrarUsuario.Location = new System.Drawing.Point(9, 15);
            this.cbFiltrarUsuario.Name = "cbFiltrarUsuario";
            this.cbFiltrarUsuario.Size = new System.Drawing.Size(51, 17);
            this.cbFiltrarUsuario.TabIndex = 9;
            this.cbFiltrarUsuario.Text = "Filtrar";
            this.cbFiltrarUsuario.UseVisualStyleBackColor = true;
            this.cbFiltrarUsuario.CheckedChanged += new System.EventHandler(this.cbFiltrarUsuario_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Privilegio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Departamento";
            // 
            // cbPrivilegio
            // 
            this.cbPrivilegio.Enabled = false;
            this.cbPrivilegio.FormattingEnabled = true;
            this.cbPrivilegio.Location = new System.Drawing.Point(86, 65);
            this.cbPrivilegio.Name = "cbPrivilegio";
            this.cbPrivilegio.Size = new System.Drawing.Size(211, 21);
            this.cbPrivilegio.TabIndex = 1;
            this.cbPrivilegio.SelectedIndexChanged += new System.EventHandler(this.cbPrivilegio_SelectedIndexChanged);
            // 
            // cbDpto
            // 
            this.cbDpto.Enabled = false;
            this.cbDpto.FormattingEnabled = true;
            this.cbDpto.Location = new System.Drawing.Point(86, 38);
            this.cbDpto.Name = "cbDpto";
            this.cbDpto.Size = new System.Drawing.Size(211, 21);
            this.cbDpto.TabIndex = 0;
            this.cbDpto.SelectedIndexChanged += new System.EventHandler(this.cbDpto_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpFecha);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpDateVenc);
            this.groupBox1.Controls.Add(this.btnSearchInMSP);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 98);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fechas";
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 427);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1056, 25);
            this.toolStrip1.TabIndex = 13;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 116);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(1056, 311);
            this.panel2.TabIndex = 14;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvProgramas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvUsers);
            this.splitContainer1.Size = new System.Drawing.Size(1050, 305);
            this.splitContainer1.SplitterDistance = 821;
            this.splitContainer1.TabIndex = 8;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkUsuario,
            this.USUARIO_ID,
            this.NOMBRE,
            this.DEPARTAMENTO,
            this.PRIVILEGIO,
            this.USUARIO});
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.Location = new System.Drawing.Point(0, 0);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(225, 305);
            this.dgvUsers.TabIndex = 8;
            this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
            // 
            // checkUsuario
            // 
            this.checkUsuario.HeaderText = "";
            this.checkUsuario.Name = "checkUsuario";
            this.checkUsuario.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.checkUsuario.Width = 30;
            // 
            // USUARIO_ID
            // 
            this.USUARIO_ID.HeaderText = "USUARIO_ID";
            this.USUARIO_ID.Name = "USUARIO_ID";
            this.USUARIO_ID.Visible = false;
            // 
            // NOMBRE
            // 
            this.NOMBRE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NOMBRE.HeaderText = "Usuario";
            this.NOMBRE.Name = "NOMBRE";
            // 
            // DEPARTAMENTO
            // 
            this.DEPARTAMENTO.HeaderText = "DEPARTAMENTO";
            this.DEPARTAMENTO.Name = "DEPARTAMENTO";
            this.DEPARTAMENTO.Visible = false;
            // 
            // PRIVILEGIO
            // 
            this.PRIVILEGIO.HeaderText = "PRIVILEGIO";
            this.PRIVILEGIO.Name = "PRIVILEGIO";
            this.PRIVILEGIO.Visible = false;
            // 
            // USUARIO
            // 
            this.USUARIO.HeaderText = "USUARIOO";
            this.USUARIO.Name = "USUARIO";
            this.USUARIO.Visible = false;
            // 
            // dgvp_check
            // 
            this.dgvp_check.HeaderText = "";
            this.dgvp_check.Name = "dgvp_check";
            this.dgvp_check.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvp_check.Width = 30;
            // 
            // DOCTO_PP_ID_2
            // 
            this.DOCTO_PP_ID_2.HeaderText = "DOCTO_PP_ID_2";
            this.DOCTO_PP_ID_2.Name = "DOCTO_PP_ID_2";
            this.DOCTO_PP_ID_2.Visible = false;
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
            this.FOLIO_REQ.Visible = false;
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
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // F_PROGRAMACIONPAGOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1056, 452);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1000, 400);
            this.Name = "F_PROGRAMACIONPAGOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Programación de pagos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_PROGRAMACIONPAGOS_FormClosing);
            this.Load += new System.EventHandler(this.F_PROGRAMACIONPAGOS_Load);
            this.Shown += new System.EventHandler(this.F_PROGRAMACIONPAGOS_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProgramas)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDateVenc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSearchInMSP;
        private System.Windows.Forms.DataGridView dgvProgramas;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox chBProximoVencimeito;
        private System.Windows.Forms.DateTimePicker dTPProximo;
        private System.Windows.Forms.TextBox txt_proveedores;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_mostrarseleccionado;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel txtUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel txtInfo;
        private System.Windows.Forms.ToolStripProgressBar pbEmpresas;
        private System.Windows.Forms.ToolStripLabel txtNo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.ToolStripLabel txtPagos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbPrivilegio;
        private System.Windows.Forms.ComboBox cbDpto;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.CheckBox cbFiltrarUsuario;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn checkUsuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn USUARIO_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOMBRE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEPARTAMENTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRIVILEGIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn USUARIO;
        private System.Windows.Forms.Button bntAgregarUsuarios;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvp_check;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTO_PP_ID_2;
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
    }
}