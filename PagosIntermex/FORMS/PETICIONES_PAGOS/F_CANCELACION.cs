using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagosIntermex
{
    public partial class F_CANCELACION : Form
    {
        public C_USUARIOS usuario { get; set; }

        public bool exito = false;
        public string motivo = "";
        public string folio = "";
        public F_CANCELACION()
        {
            InitializeComponent();
            
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCancelacion.Text.Trim()))
            {
                MessageBox.Show("El motivo de cancelación es obligatorio","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            if(txtPass.Text.Trim() != usuario.Contraseña)
            {
                MessageBox.Show("La contraseña del usuario: " + usuario.Usuario + "es incorrecta", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            motivo = txtCancelacion.Text.Trim();
            exito = true;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que desea cancelar esta operación?","Mensaje de la aplicación",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            Close();
        }

        private void F_CANCELACION_Shown(object sender, EventArgs e)
        {
            lbSelect.Text = "Se cancelara el documento con folio: " + folio;
        }
    }
}
