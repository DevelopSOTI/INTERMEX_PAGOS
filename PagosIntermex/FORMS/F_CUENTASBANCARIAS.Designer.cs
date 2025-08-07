namespace PagosIntermex
{
    partial class F_CUENTASBANCARIAS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_CUENTASBANCARIAS));
            this.label2 = new System.Windows.Forms.Label();
            this.cbCuentaBancaria = new System.Windows.Forms.ComboBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbFormatoArchivo = new System.Windows.Forms.ComboBox();
            this.cbConceptos = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboConcepto = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtUser = new System.Windows.Forms.ToolStripLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtdispo = new System.Windows.Forms.Label();
            this.cbCompFiscal = new System.Windows.Forms.CheckBox();
            this.cbdispo = new System.Windows.Forms.ComboBox();
            this.panelAnticipos = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.cbAnticipo = new System.Windows.Forms.ComboBox();
            this.panelPago = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.cbBanco = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.EMPRESA_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NOMBRE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIENE_ANTICIPO2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIENE_PAGO2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panelAnticipos.SuspendLayout();
            this.panelPago.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Cuenta bancaria";
            // 
            // cbCuentaBancaria
            // 
            this.cbCuentaBancaria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCuentaBancaria.FormattingEnabled = true;
            this.cbCuentaBancaria.Location = new System.Drawing.Point(6, 19);
            this.cbCuentaBancaria.Name = "cbCuentaBancaria";
            this.cbCuentaBancaria.Size = new System.Drawing.Size(330, 21);
            this.cbCuentaBancaria.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(264, 394);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 3;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Formato de archivo";
            // 
            // cbFormatoArchivo
            // 
            this.cbFormatoArchivo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFormatoArchivo.FormattingEnabled = true;
            this.cbFormatoArchivo.Location = new System.Drawing.Point(6, 68);
            this.cbFormatoArchivo.Name = "cbFormatoArchivo";
            this.cbFormatoArchivo.Size = new System.Drawing.Size(330, 21);
            this.cbFormatoArchivo.TabIndex = 2;
            this.cbFormatoArchivo.SelectedIndexChanged += new System.EventHandler(this.cbFormatoArchivo_SelectedIndexChanged);
            // 
            // cbConceptos
            // 
            this.cbConceptos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConceptos.FormattingEnabled = true;
            this.cbConceptos.Location = new System.Drawing.Point(6, 19);
            this.cbConceptos.Name = "cbConceptos";
            this.cbConceptos.Size = new System.Drawing.Size(330, 21);
            this.cbConceptos.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Concepto de pago";
            // 
            // comboConcepto
            // 
            this.comboConcepto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboConcepto.FormattingEnabled = true;
            this.comboConcepto.Location = new System.Drawing.Point(6, 146);
            this.comboConcepto.Name = "comboConcepto";
            this.comboConcepto.Size = new System.Drawing.Size(330, 21);
            this.comboConcepto.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Concepto bancario";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(737, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtUser});
            this.toolStrip1.Location = new System.Drawing.Point(0, 528);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(737, 25);
            this.toolStrip1.TabIndex = 10;
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
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panelAnticipos);
            this.panel1.Controls.Add(this.panelPago);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(386, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 504);
            this.panel1.TabIndex = 11;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.cbCuentaBancaria);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.cbFormatoArchivo);
            this.panel6.Controls.Add(this.txtdispo);
            this.panel6.Controls.Add(this.cbCompFiscal);
            this.panel6.Controls.Add(this.comboConcepto);
            this.panel6.Controls.Add(this.cbdispo);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 171);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(351, 184);
            this.panel6.TabIndex = 19;
            // 
            // txtdispo
            // 
            this.txtdispo.AutoSize = true;
            this.txtdispo.Location = new System.Drawing.Point(6, 99);
            this.txtdispo.Name = "txtdispo";
            this.txtdispo.Size = new System.Drawing.Size(72, 13);
            this.txtdispo.TabIndex = 15;
            this.txtdispo.Text = "Disponibilidad";
            this.txtdispo.Visible = false;
            // 
            // cbCompFiscal
            // 
            this.cbCompFiscal.AutoSize = true;
            this.cbCompFiscal.Location = new System.Drawing.Point(207, 99);
            this.cbCompFiscal.Name = "cbCompFiscal";
            this.cbCompFiscal.Size = new System.Drawing.Size(132, 17);
            this.cbCompFiscal.TabIndex = 13;
            this.cbCompFiscal.Text = "Requiere Comp. Fiscal";
            this.cbCompFiscal.UseVisualStyleBackColor = true;
            this.cbCompFiscal.Visible = false;
            // 
            // cbdispo
            // 
            this.cbdispo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbdispo.FormattingEnabled = true;
            this.cbdispo.Items.AddRange(new object[] {
            "Mismo dia",
            "Día Siguiente"});
            this.cbdispo.Location = new System.Drawing.Point(86, 96);
            this.cbdispo.Name = "cbdispo";
            this.cbdispo.Size = new System.Drawing.Size(115, 21);
            this.cbdispo.TabIndex = 14;
            this.cbdispo.Visible = false;
            // 
            // panelAnticipos
            // 
            this.panelAnticipos.Controls.Add(this.label7);
            this.panelAnticipos.Controls.Add(this.cbAnticipo);
            this.panelAnticipos.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAnticipos.Location = new System.Drawing.Point(0, 122);
            this.panelAnticipos.Name = "panelAnticipos";
            this.panelAnticipos.Size = new System.Drawing.Size(351, 49);
            this.panelAnticipos.TabIndex = 18;
            this.panelAnticipos.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Concepto de anticipo";
            // 
            // cbAnticipo
            // 
            this.cbAnticipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAnticipo.FormattingEnabled = true;
            this.cbAnticipo.Location = new System.Drawing.Point(6, 19);
            this.cbAnticipo.Name = "cbAnticipo";
            this.cbAnticipo.Size = new System.Drawing.Size(330, 21);
            this.cbAnticipo.TabIndex = 7;
            // 
            // panelPago
            // 
            this.panelPago.Controls.Add(this.label1);
            this.panelPago.Controls.Add(this.cbConceptos);
            this.panelPago.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPago.Location = new System.Drawing.Point(0, 75);
            this.panelPago.Name = "panelPago";
            this.panelPago.Size = new System.Drawing.Size(351, 47);
            this.panelPago.TabIndex = 17;
            this.panelPago.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.cbBanco);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 29);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(351, 46);
            this.panel4.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Banco:";
            // 
            // cbBanco
            // 
            this.cbBanco.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBanco.FormattingEnabled = true;
            this.cbBanco.Location = new System.Drawing.Point(6, 19);
            this.cbBanco.Name = "cbBanco";
            this.cbBanco.Size = new System.Drawing.Size(330, 21);
            this.cbBanco.TabIndex = 11;
            this.cbBanco.SelectedIndexChanged += new System.EventHandler(this.cbBanco_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 456);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(196, 31);
            this.button1.TabIndex = 10;
            this.button1.Text = "Generar unicamente Seleccionados";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(59)))), ((int)(((byte)(104)))));
            this.panel3.Controls.Add(this.label5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(351, 29);
            this.panel3.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(6, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(333, 26);
            this.label5.TabIndex = 10;
            this.label5.Text = "Debe seleccionar los parametros de pagos por empresa y dar click en \"Aceptar\"";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(386, 504);
            this.panel2.TabIndex = 12;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EMPRESA_ID,
            this.NOMBRE,
            this.TIENE_ANTICIPO2,
            this.TIENE_PAGO2});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(380, 498);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // EMPRESA_ID
            // 
            this.EMPRESA_ID.HeaderText = "EMPRESA_ID";
            this.EMPRESA_ID.Name = "EMPRESA_ID";
            this.EMPRESA_ID.ReadOnly = true;
            this.EMPRESA_ID.Visible = false;
            // 
            // NOMBRE
            // 
            this.NOMBRE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NOMBRE.HeaderText = "Empresa";
            this.NOMBRE.Name = "NOMBRE";
            this.NOMBRE.ReadOnly = true;
            this.NOMBRE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TIENE_ANTICIPO2
            // 
            this.TIENE_ANTICIPO2.HeaderText = "TIENE_ANTICIPO";
            this.TIENE_ANTICIPO2.Name = "TIENE_ANTICIPO2";
            this.TIENE_ANTICIPO2.ReadOnly = true;
            this.TIENE_ANTICIPO2.Visible = false;
            // 
            // TIENE_PAGO2
            // 
            this.TIENE_PAGO2.HeaderText = "TIENE_PAGO2";
            this.TIENE_PAGO2.Name = "TIENE_PAGO2";
            this.TIENE_PAGO2.ReadOnly = true;
            this.TIENE_PAGO2.Visible = false;
            // 
            // F_CUENTASBANCARIAS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(737, 553);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F_CUENTASBANCARIAS";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parametros para generar archivo de bancos";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.F_CUENTASBANCARIAS_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panelAnticipos.ResumeLayout(false);
            this.panelAnticipos.PerformLayout();
            this.panelPago.ResumeLayout(false);
            this.panelPago.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbCuentaBancaria;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbFormatoArchivo;
        private System.Windows.Forms.ComboBox cbConceptos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboConcepto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel txtUser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbBanco;
        private System.Windows.Forms.CheckBox cbCompFiscal;
        private System.Windows.Forms.Label txtdispo;
        private System.Windows.Forms.ComboBox cbdispo;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panelAnticipos;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbAnticipo;
        private System.Windows.Forms.Panel panelPago;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMPRESA_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOMBRE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TIENE_ANTICIPO2;
        private System.Windows.Forms.DataGridViewTextBoxColumn TIENE_PAGO2;
    }
}