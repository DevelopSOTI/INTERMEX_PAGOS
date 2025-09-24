namespace PagosIntermex.FORMS.REPORTES
{

    partial class F_REP_PROGRAMACION
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.lblTotalImporte = new System.Windows.Forms.Label();
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.cmbProgramacion = new System.Windows.Forms.ComboBox();
            this.lblProgramacion = new System.Windows.Forms.Label();
            this.chkTodasProgramaciones = new System.Windows.Forms.CheckBox();
            this.dgvReporte = new System.Windows.Forms.DataGridView();
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.pnlFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).BeginInit();
            this.pnlTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFiltros
            // 
            this.pnlFiltros.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlFiltros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFiltros.Controls.Add(this.lblTotalImporte);
            this.pnlFiltros.Controls.Add(this.btnExportar);
            this.pnlFiltros.Controls.Add(this.btnGenerar);
            this.pnlFiltros.Controls.Add(this.cmbProgramacion);
            this.pnlFiltros.Controls.Add(this.lblProgramacion);
            this.pnlFiltros.Controls.Add(this.chkTodasProgramaciones);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Location = new System.Drawing.Point(0, 60);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Padding = new System.Windows.Forms.Padding(10);
            this.pnlFiltros.Size = new System.Drawing.Size(1400, 82);
            this.pnlFiltros.TabIndex = 0;
            // 
            // lblTotalImporte
            // 
            this.lblTotalImporte.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalImporte.AutoSize = true;
            this.lblTotalImporte.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalImporte.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTotalImporte.Location = new System.Drawing.Point(1200, 20);
            this.lblTotalImporte.Name = "lblTotalImporte";
            this.lblTotalImporte.Size = new System.Drawing.Size(92, 20);
            this.lblTotalImporte.TabIndex = 5;
            this.lblTotalImporte.Text = "Total: $0.00";
            this.lblTotalImporte.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExportar
            // 
            this.btnExportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(191)))));
            this.btnExportar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportar.Enabled = false;
            this.btnExportar.FlatAppearance.BorderSize = 0;
            this.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportar.ForeColor = System.Drawing.Color.White;
            this.btnExportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportar.Location = new System.Drawing.Point(760, 12);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.btnExportar.Size = new System.Drawing.Size(140, 40);
            this.btnExportar.TabIndex = 4;
            this.btnExportar.Text = "   Exportar";
            this.btnExportar.UseVisualStyleBackColor = false;
            this.btnExportar.Click += new System.EventHandler(this.BtnExportar_Click);
            // 
            // btnGenerar
            // 
            this.btnGenerar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnGenerar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerar.FlatAppearance.BorderSize = 0;
            this.btnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.ForeColor = System.Drawing.Color.White;
            this.btnGenerar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerar.Location = new System.Drawing.Point(600, 12);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.btnGenerar.Size = new System.Drawing.Size(140, 40);
            this.btnGenerar.TabIndex = 3;
            this.btnGenerar.Text = "   Generar";
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.BtnGenerar_Click);
            // 
            // cmbProgramacion
            // 
            this.cmbProgramacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProgramacion.Enabled = false;
            this.cmbProgramacion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbProgramacion.FormattingEnabled = true;
            this.cmbProgramacion.Location = new System.Drawing.Point(385, 18);
            this.cmbProgramacion.Name = "cmbProgramacion";
            this.cmbProgramacion.Size = new System.Drawing.Size(200, 25);
            this.cmbProgramacion.TabIndex = 2;
            // 
            // lblProgramacion
            // 
            this.lblProgramacion.AutoSize = true;
            this.lblProgramacion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgramacion.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblProgramacion.Location = new System.Drawing.Point(280, 21);
            this.lblProgramacion.Name = "lblProgramacion";
            this.lblProgramacion.Size = new System.Drawing.Size(93, 17);
            this.lblProgramacion.TabIndex = 1;
            this.lblProgramacion.Text = "Programación:";
            // 
            // chkTodasProgramaciones
            // 
            this.chkTodasProgramaciones.AutoSize = true;
            this.chkTodasProgramaciones.Checked = true;
            this.chkTodasProgramaciones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTodasProgramaciones.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTodasProgramaciones.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.chkTodasProgramaciones.Location = new System.Drawing.Point(15, 20);
            this.chkTodasProgramaciones.Name = "chkTodasProgramaciones";
            this.chkTodasProgramaciones.Size = new System.Drawing.Size(237, 21);
            this.chkTodasProgramaciones.TabIndex = 0;
            this.chkTodasProgramaciones.Text = "Mostrar todas las programaciones";
            this.chkTodasProgramaciones.UseVisualStyleBackColor = true;
            this.chkTodasProgramaciones.CheckedChanged += new System.EventHandler(this.ChkTodasProgramaciones_CheckedChanged);
            // 
            // dgvReporte
            // 
            this.dgvReporte.AllowUserToAddRows = false;
            this.dgvReporte.AllowUserToDeleteRows = false;
            this.dgvReporte.AllowUserToResizeRows = false;
            this.dgvReporte.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReporte.BackgroundColor = System.Drawing.Color.White;
            this.dgvReporte.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReporte.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReporte.ColumnHeadersHeight = 40;
            this.dgvReporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReporte.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReporte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReporte.EnableHeadersVisualStyles = false;
            this.dgvReporte.GridColor = System.Drawing.Color.LightGray;
            this.dgvReporte.Location = new System.Drawing.Point(0, 142);
            this.dgvReporte.MultiSelect = false;
            this.dgvReporte.Name = "dgvReporte";
            this.dgvReporte.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReporte.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvReporte.RowHeadersVisible = false;
            this.dgvReporte.RowHeadersWidth = 25;
            this.dgvReporte.RowTemplate.Height = 28;
            this.dgvReporte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReporte.ShowCellErrors = false;
            this.dgvReporte.ShowCellToolTips = false;
            this.dgvReporte.ShowEditingIcon = false;
            this.dgvReporte.ShowRowErrors = false;
            this.dgvReporte.Size = new System.Drawing.Size(1400, 608);
            this.dgvReporte.TabIndex = 1;
            // 
            // pnlTitulo
            // 
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Controls.Add(this.pictureBox1);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(1400, 60);
            this.pnlTitulo.TabIndex = 2;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(70, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(419, 32);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Reporte de Programación de Pagos";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PagosIntermex.Properties.Resources.Financial_advisor_logo_Right_and_left_Arrow_Logo;
            this.pictureBox1.Location = new System.Drawing.Point(15, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 750);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1400, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel1.Text = "Listo";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // F_REP_PROGRAMACION
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 772);
            this.Controls.Add(this.dgvReporte);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlTitulo);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.Name = "F_REP_PROGRAMACION";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Pagos - Reporte de Programación";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_REP_PROGRAMACION_FormClosing);
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).EndInit();
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Label lblTotalImporte;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.ComboBox cmbProgramacion;
        private System.Windows.Forms.Label lblProgramacion;
        private System.Windows.Forms.CheckBox chkTodasProgramaciones;
        private System.Windows.Forms.DataGridView dgvReporte;
        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    }
}