namespace PagosIntermex
{
    partial class F_VER_DETALLE_PETICION
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_VER_DETALLE_PETICION));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbInfo = new System.Windows.Forms.Label();
            this.dTPProximo = new System.Windows.Forms.DateTimePicker();
            this.chBProximoVencimeito = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtUsuario = new System.Windows.Forms.ToolStripLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvPagos = new System.Windows.Forms.DataGridView();
            this.DOCTO_PR_DET_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOCTO_PP_DET_ID2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USUARIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FOLIO_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FOLIO_REQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FECHA_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROVEEDOR_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P_EMPRESA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_PROVEEDOR_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FECHA_VEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IMPORTE_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IMPORTE_AUT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ESTATUS_P = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel1.Controls.Add(this.lbInfo);
            this.panel1.Controls.Add(this.dTPProximo);
            this.panel1.Controls.Add(this.chBProximoVencimeito);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1090, 62);
            this.panel1.TabIndex = 13;
            // 
            // lbInfo
            // 
            this.lbInfo.Location = new System.Drawing.Point(12, 9);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(835, 50);
            this.lbInfo.TabIndex = 3;
            this.lbInfo.Text = "label1";
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
            this.toolStripLabel1,
            this.txtUsuario});
            this.toolStrip1.Location = new System.Drawing.Point(0, 425);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1090, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(47, 22);
            this.toolStripLabel1.Text = "Usuario";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(0, 22);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvPagos);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(1090, 363);
            this.panel2.TabIndex = 16;
            // 
            // dgvPagos
            // 
            this.dgvPagos.AllowUserToAddRows = false;
            this.dgvPagos.AllowUserToDeleteRows = false;
            this.dgvPagos.AllowUserToResizeRows = false;
            this.dgvPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPagos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DOCTO_PR_DET_ID,
            this.DOCTO_PP_DET_ID2,
            this.USUARIO,
            this.FOLIO_P,
            this.FOLIO_REQ,
            this.FECHA_P,
            this.PROVEEDOR_P,
            this.P_EMPRESA,
            this.C_PROVEEDOR_ID,
            this.FECHA_VEN,
            this.IMPORTE_P,
            this.IMPORTE_AUT,
            this.ESTATUS_P});
            this.dgvPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPagos.Location = new System.Drawing.Point(3, 3);
            this.dgvPagos.Name = "dgvPagos";
            this.dgvPagos.ReadOnly = true;
            this.dgvPagos.RowHeadersVisible = false;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.dgvPagos.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPagos.Size = new System.Drawing.Size(1084, 357);
            this.dgvPagos.TabIndex = 3;
            // 
            // DOCTO_PR_DET_ID
            // 
            this.DOCTO_PR_DET_ID.HeaderText = "DOCTO_PR_DET_ID";
            this.DOCTO_PR_DET_ID.Name = "DOCTO_PR_DET_ID";
            this.DOCTO_PR_DET_ID.ReadOnly = true;
            this.DOCTO_PR_DET_ID.Visible = false;
            // 
            // DOCTO_PP_DET_ID2
            // 
            this.DOCTO_PP_DET_ID2.HeaderText = "DOCTO_PP_DET_ID";
            this.DOCTO_PP_DET_ID2.Name = "DOCTO_PP_DET_ID2";
            this.DOCTO_PP_DET_ID2.ReadOnly = true;
            this.DOCTO_PP_DET_ID2.Visible = false;
            // 
            // USUARIO
            // 
            this.USUARIO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.USUARIO.DefaultCellStyle = dataGridViewCellStyle1;
            this.USUARIO.HeaderText = "Usuario";
            this.USUARIO.Name = "USUARIO";
            this.USUARIO.ReadOnly = true;
            this.USUARIO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FOLIO_P
            // 
            this.FOLIO_P.HeaderText = "Folio Msp";
            this.FOLIO_P.Name = "FOLIO_P";
            this.FOLIO_P.ReadOnly = true;
            this.FOLIO_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FOLIO_P.Width = 85;
            // 
            // FOLIO_REQ
            // 
            this.FOLIO_REQ.HeaderText = "Folio Requisicion";
            this.FOLIO_REQ.Name = "FOLIO_REQ";
            this.FOLIO_REQ.ReadOnly = true;
            this.FOLIO_REQ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FECHA_P
            // 
            this.FECHA_P.HeaderText = "Fecha";
            this.FECHA_P.Name = "FECHA_P";
            this.FECHA_P.ReadOnly = true;
            this.FECHA_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FECHA_P.Visible = false;
            this.FECHA_P.Width = 80;
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
            this.C_PROVEEDOR_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.IMPORTE_P.DefaultCellStyle = dataGridViewCellStyle2;
            this.IMPORTE_P.HeaderText = "Importe Solicitado";
            this.IMPORTE_P.Name = "IMPORTE_P";
            this.IMPORTE_P.ReadOnly = true;
            this.IMPORTE_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IMPORTE_P.Width = 106;
            // 
            // IMPORTE_AUT
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.IMPORTE_AUT.DefaultCellStyle = dataGridViewCellStyle3;
            this.IMPORTE_AUT.HeaderText = "Importe Autorizado";
            this.IMPORTE_AUT.Name = "IMPORTE_AUT";
            this.IMPORTE_AUT.ReadOnly = true;
            this.IMPORTE_AUT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IMPORTE_AUT.Width = 106;
            // 
            // ESTATUS_P
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ESTATUS_P.DefaultCellStyle = dataGridViewCellStyle4;
            this.ESTATUS_P.HeaderText = "Estatus";
            this.ESTATUS_P.Name = "ESTATUS_P";
            this.ESTATUS_P.ReadOnly = true;
            this.ESTATUS_P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ESTATUS_P.Visible = false;
            this.ESTATUS_P.Width = 70;
            // 
            // F_VER_DETALLE_PETICION
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1090, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_VER_DETALLE_PETICION";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ver Trazabilidad de petición";
            this.Load += new System.EventHandler(this.F_VER_DETALLE_PETICION_Load);
            this.Shown += new System.EventHandler(this.F_VER_DETALLE_PETICION_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dTPProximo;
        private System.Windows.Forms.CheckBox chBProximoVencimeito;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel txtUsuario;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvPagos;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTO_PR_DET_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTO_PP_DET_ID2;
        private System.Windows.Forms.DataGridViewTextBoxColumn USUARIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn FOLIO_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn FOLIO_REQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn FECHA_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROVEEDOR_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn P_EMPRESA;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_PROVEEDOR_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FECHA_VEN;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMPORTE_P;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMPORTE_AUT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ESTATUS_P;
    }
}