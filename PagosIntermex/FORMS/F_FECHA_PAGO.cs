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
    public partial class F_FECHA_PAGO : Form
    {
        public DateTime _FechaPago;
        public bool _FechaAsignada;

        public F_FECHA_PAGO()
        {
            InitializeComponent();
        }

        private void F_FECHA_PAGO_Load(object sender, EventArgs e)
        {
            _FechaAsignada = false;
        }

        private void bFechaPAgo_Click(object sender, EventArgs e)
        {
            try
            {
                _FechaPago = dTPFechaPago.Value;

                DialogResult _resp = MessageBox.Show("Fecha de pago asignada: " + _FechaPago.ToString("dd/MM/yyyy") + "\n¿Es la fecha correcta?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (_resp == DialogResult.Yes)
                {
                    _FechaAsignada = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de apgos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void F_FECHA_PAGO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_FechaAsignada)
            {
                DialogResult _resp = MessageBox.Show("No se ha seleccionado una fecha de pago.\n¿Desea continuar y cerrar la ventana?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (_resp == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

    }
}
