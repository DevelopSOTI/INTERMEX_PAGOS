namespace PagosIntermex
{
    partial class F_SERIES
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_SERIES));
            this.dgvseries = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.butSerie = new System.Windows.Forms.Button();
            this.txtSerie = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butDefault = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvseries)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvseries
            // 
            this.dgvseries.AllowUserToAddRows = false;
            this.dgvseries.AllowUserToDeleteRows = false;
            this.dgvseries.AllowUserToResizeRows = false;
            this.dgvseries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvseries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvseries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvseries.Location = new System.Drawing.Point(12, 100);
            this.dgvseries.MultiSelect = false;
            this.dgvseries.Name = "dgvseries";
            this.dgvseries.ReadOnly = true;
            this.dgvseries.RowHeadersVisible = false;
            this.dgvseries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvseries.Size = new System.Drawing.Size(303, 291);
            this.dgvseries.TabIndex = 0;
            this.dgvseries.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvseries_CellClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Serie";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Consecutivo";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Default";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // butSerie
            // 
            this.butSerie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSerie.Location = new System.Drawing.Point(193, 26);
            this.butSerie.Name = "butSerie";
            this.butSerie.Size = new System.Drawing.Size(121, 23);
            this.butSerie.TabIndex = 1;
            this.butSerie.Text = "Agregar serie";
            this.butSerie.UseVisualStyleBackColor = true;
            this.butSerie.Click += new System.EventHandler(this.butSerie_Click);
            // 
            // txtSerie
            // 
            this.txtSerie.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSerie.Location = new System.Drawing.Point(85, 28);
            this.txtSerie.MaxLength = 3;
            this.txtSerie.Name = "txtSerie";
            this.txtSerie.Size = new System.Drawing.Size(101, 20);
            this.txtSerie.TabIndex = 2;
            this.txtSerie.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSerie_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Insertar serie";
            // 
            // butDefault
            // 
            this.butDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butDefault.Location = new System.Drawing.Point(193, 55);
            this.butDefault.Name = "butDefault";
            this.butDefault.Size = new System.Drawing.Size(121, 23);
            this.butDefault.TabIndex = 4;
            this.butDefault.Text = "Usar serie default";
            this.butDefault.UseVisualStyleBackColor = true;
            this.butDefault.Click += new System.EventHandler(this.butDefault_Click);
            // 
            // F_SERIES
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(327, 403);
            this.Controls.Add(this.butDefault);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSerie);
            this.Controls.Add(this.butSerie);
            this.Controls.Add(this.dgvseries);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "F_SERIES";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurar series del sistema";
            ((System.ComponentModel.ISupportInitialize)(this.dgvseries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvseries;
        private System.Windows.Forms.Button butSerie;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.TextBox txtSerie;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button butDefault;
    }
}