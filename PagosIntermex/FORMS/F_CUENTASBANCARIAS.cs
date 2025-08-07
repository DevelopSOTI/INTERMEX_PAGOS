using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FirebirdSql.Data.FirebirdClient;

namespace PagosIntermex
{
    public partial class F_CUENTASBANCARIAS : Form
    {

        public C_EMPRESAS[] empresa;
        public C_CUENTAS_BAN[] cuentasEmpresas = new C_CUENTAS_BAN[0];
        public string concepto_cp = "";
        public string cuenta_ordenante = "";
        public string formato_layout = "";

        public int moneta_id = 0;
        public int cuenta_ban_id = 0;
        public int concepto_ba_id = 0;

        Color dgvSuccess = Color.FromArgb(172, 235, 152);
        Color dgvSelected = Color.FromArgb(72, 187, 37);
        public string empresaSeleccionada = "";
        public string tiene_anticipo = "";
        public string tiene_pago = "";

        C_BANCOS[] bancos = new C_BANCOS[0];        

        public F_CUENTASBANCARIAS()
        {
            InitializeComponent();
        }

        private void CargaBancos()
        {
            List<C_BANCOS> listaBancos = new List<C_BANCOS>();

            C_BANCOS ban1 = new C_BANCOS();
            ban1.NOMBRE = "BANORTE";
            ban1.CLAVE_FISCAL = "072";
            listaBancos.Add(ban1);

            ban1 = new C_BANCOS();
            ban1.NOMBRE = "BBVA BANCOMER";
            ban1.CLAVE_FISCAL = "012";
            listaBancos.Add(ban1);

            ban1 = new C_BANCOS();
            ban1.NOMBRE = "HSBC";
            ban1.CLAVE_FISCAL = "021";
            listaBancos.Add(ban1);

            ban1 = new C_BANCOS();
            ban1.NOMBRE = "SANTANDER";
            ban1.CLAVE_FISCAL = "014";
            listaBancos.Add(ban1);
            /**
             * aqui se pueden agregar mas bancos 
             * antes de recorrer el cliclo tal cual se ha hecho
             **/

            foreach(C_BANCOS ban in listaBancos)
            {
                Array.Resize(ref bancos, bancos.Length + 1);
                bancos[bancos.Length - 1] = new C_BANCOS();
                bancos[bancos.Length - 1].NOMBRE = ban.NOMBRE;
                bancos[bancos.Length - 1].CLAVE_FISCAL = ban.CLAVE_FISCAL;

                cbBanco.Items.Add(bancos[bancos.Length - 1]);
            }
        }


        private class CuentasBancarias
        {
            public CuentasBancarias(int cuenta_ban_id, string nombre, string num_cuenta, int moneda_id)
            {
                CUENTA_BAN_ID = cuenta_ban_id;
                NOMBRE = nombre;
                NUM_CUENTA = num_cuenta;
                MONEDA_ID = moneda_id;
            }

            public int CUENTA_BAN_ID { get; set; }

            public string NOMBRE { get; set; }

            public string NUM_CUENTA { get; set; }

            public int MONEDA_ID { get; set; }

            public override string ToString()
            {
                return NOMBRE;
            }
        }

        private class ConceptosBancos
        {
            public ConceptosBancos(int concepto_ba_id, string nombre)
            {
                CONCEPTO_BA_ID = concepto_ba_id;
                NOMBRE = nombre;
            }

            public int CONCEPTO_BA_ID { get; set; }
            public string NOMBRE { get; set; }

            public override string ToString()
            {
                return NOMBRE;
            }
        }

        

        private void F_CUENTASBANCARIAS_Shown(object sender, EventArgs e)
        {
            if(empresa!= null)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < empresa.Length; i++)
                {
                    dataGridView1.Rows.Add();
                   // dataGridView1["EMPRESA_ID", dataGridView1.RowCount - 1].Value = empresa[i].EMPRESA_ID;
                    dataGridView1["NOMBRE", dataGridView1.RowCount - 1].Value = empresa[i].NOMBRE_CORTO;
                    dataGridView1["TIENE_ANTICIPO2", dataGridView1.RowCount - 1].Value = empresa[i].TIENE_ANTICIPO == true ? "S" : "N";
                    dataGridView1["TIENE_PAGO2", dataGridView1.RowCount - 1].Value = empresa[i].TIENE_PAGO == true ? "S" : "N";
                }

                dataGridView1.ClearSelection();
                cbBanco.Items.Clear();
                CargaBancos();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if ( (cbCuentaBancaria.SelectedIndex >= 0) && (cbFormatoArchivo.SelectedIndex >= 0))
            {
                if (tiene_pago == "S")
                {
                    if(cbConceptos.SelectedIndex < 0)
                    {
                        MessageBox.Show("Necesita indicar todos los datos para la generación de los pagos.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (tiene_anticipo == "S")
                {
                    if (cbAnticipo.SelectedIndex < 0)
                    {
                        MessageBox.Show("Necesita indicar todos los datos para la generación de los pagos.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                C_CUENTAS_BAN aux = new C_CUENTAS_BAN();
                CuentasBancarias cuenta = cbCuentaBancaria.SelectedItem as CuentasBancarias;
                ConceptosBancos conceptos = comboConcepto.SelectedItem as ConceptosBancos;
                C_BANCOS bancoSelecc = cbBanco.SelectedItem as C_BANCOS;
                aux.cuentaBancariaNombre = cbCuentaBancaria.SelectedIndex;
                aux.cuenta_ordenante = cuenta.NUM_CUENTA;
                aux.concepto_cp = cbConceptos.Text;
                if (tiene_anticipo == "S")
                    aux.concepto_anticipo = cbAnticipo.Text;
                aux.formato_layout = cbFormatoArchivo.Text;
                aux.banco = bancoSelecc.NOMBRE;
                aux.clave_fiscal = bancoSelecc.CLAVE_FISCAL;

                aux.moneta_id = cuenta.MONEDA_ID;
                aux.cuenta_ban_id = cuenta.CUENTA_BAN_ID;
                aux.concepto_ba_id = conceptos.CONCEPTO_BA_ID;
                aux.conceptosBancosNombre = comboConcepto.SelectedIndex;
                aux.EMPRESA = empresaSeleccionada;
                if (cbCompFiscal.Checked)
                    aux.requiereCompFiscal = true;
                else
                    aux.requiereCompFiscal = false;

                if (cbdispo.Visible == true)
                    aux.Disponibilidad = cbdispo.Text == "Mismo dia" ? "H" : "M";

                bool estaEnArreglo = false;

                for (int i = 0; i < cuentasEmpresas.Length; i++)
                {
                    if (cuentasEmpresas[i].EMPRESA == empresaSeleccionada)
                    {
                        cuentasEmpresas[i] = aux;
                        estaEnArreglo = true;
                    }
                }
                

                if (!estaEnArreglo)
                {
                    Array.Resize(ref cuentasEmpresas, cuentasEmpresas.Length + 1);
                    cuentasEmpresas[cuentasEmpresas.Length - 1] = new C_CUENTAS_BAN();

                    cuentasEmpresas[cuentasEmpresas.Length - 1] = aux;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.BackColor = dgvSuccess;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.SelectionBackColor = dgvSelected;
                }

                bool todosSelecc = false;

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].DefaultCellStyle.BackColor == dgvSuccess)
                    {
                        todosSelecc = true;
                    }
                    else
                    {
                        todosSelecc = false;
                        break;
                    }
                }

                if (todosSelecc)
                {
                    this.DialogResult = DialogResult.OK;

                    Close();
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.BackColor = dgvSuccess;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.SelectionBackColor = dgvSelected;
                }
            }
            else
            {
                MessageBox.Show("Necesita indicar todos los datos para la generación de los pagos.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            empresaSeleccionada = dataGridView1["NOMBRE", e.RowIndex].Value.ToString();
            tiene_anticipo = dataGridView1["TIENE_ANTICIPO2", e.RowIndex].Value.ToString();
            tiene_pago = dataGridView1["TIENE_PAGO2", e.RowIndex].Value.ToString();

            //traer todo los datos bancarios
            C_CONEXIONFIREBIRD conn = new C_CONEXIONFIREBIRD();
            FbCommand cmd;
            FbDataReader read;

            string select = "";
            cbConceptos.Items.Clear();
            cbCuentaBancaria.Items.Clear();
            comboConcepto.Items.Clear();
            cbFormatoArchivo.Items.Clear();
            cbAnticipo.Items.Clear();
            bool empresaSeleccio = false;
            for (int i = 0; i < cuentasEmpresas.Length; i++)
            {
                if (cuentasEmpresas[i].EMPRESA == empresaSeleccionada)
                {
                    cbBanco.SelectedIndex = cbBanco.FindString(cuentasEmpresas[i].banco);
                    empresaSeleccio = true;
                    
                    break;
                }
            }
            if (!empresaSeleccio)
                cbBanco.SelectedIndex = 0;

            if (tiene_pago == "S")
                panelPago.Visible = true;
            else
                panelPago.Visible = false;

            if (tiene_anticipo == "S")
                panelAnticipos.Visible = true;
            else
                panelAnticipos.Visible = false;

            

            if (conn.ConectarFB(empresaSeleccionada))
            {
                #region MOSTRAMOS LOS CONCEPTOS DEL MODULO DE CUENTAS POR PAGAR
                select = "SELECT * FROM conceptos_cp cp ";
                select += "WHERE cp.naturaleza = 'R' ";
                select += "  AND cp.tipo = 'P' ";
                select += "  AND cp.es_bancarizado = 'S' ";
                select += "  AND cp.crear_polizas = 'S' ";
                select += "  AND cp.tipo_poliza IS NOT NULL ";
                select += "  AND cp.cuenta_contable IS NOT NULL";

                try
                {
                    cmd = new FbCommand(select, conn.FBC);
                    read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        cbConceptos.Items.Add(read["NOMBRE"].ToString());
                    }
                    read.Close();
                    cmd.Dispose();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible mostrar los conceptos de pago.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                #endregion

                #region MOSTRAMOS LAS CUENTAS BANCARIAS CON CLAVE FISCAL 
                select = "SELECT ";
                select += "      cb.cuenta_ban_id, ";
                select += "      cb.nombre, ";
                select += "      cb.num_cuenta, ";
                select += "      cb.moneda_id ";
                select += " FROM cuentas_bancarias cb ";
                select += " JOIN bancos b ON(cb.banco_id = b.banco_id) ";
                select += "WHERE b.clave_fiscal in('021','014','012','072')";

                try
                {
                    cmd = new FbCommand(select, conn.FBC);
                    read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        // cbCuentaBancaria.Items.Add(new CuentasBancarias(read["NOMBRE"].ToString(), read["NUM_CUENTA"].ToString()));
                        cbCuentaBancaria.Items.Add(new CuentasBancarias(Convert.ToInt32(read["CUENTA_BAN_ID"].ToString()), read["NOMBRE"].ToString(), read["NUM_CUENTA"].ToString(), Convert.ToInt32(read["MONEDA_ID"].ToString())));
                    }
                    read.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible mostrar las cuentas bancarias.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                #region MOSTRAMOS LOS CONCEPTOS BANCARIOS
                try
                {
                    cmd = new FbCommand("SELECT * FROM conceptos_ba WHERE naturaleza = 'R' AND tipo_mov_fiscal = 'T'", conn.FBC);
                    read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        comboConcepto.Items.Add(new ConceptosBancos(Convert.ToInt32(read["CONCEPTO_BA_ID"].ToString()), read["NOMBRE"].ToString()));
                    }
                    read.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible mostrar los conceptos bancarios.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                #region MOSTRAMOS LOS CONCEPTOS DE ANTICIPOS
                try
                {
                    select = "SELECT * FROM conceptos_cp cp ";
                    select += "WHERE cp.naturaleza = 'R' ";
                    select += "  AND cp.tipo = 'P' ";

                    cmd = new FbCommand(select, conn.FBC);
                    read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        cbAnticipo.Items.Add(read["NOMBRE"].ToString());
                    }
                    read.Close();
                    cmd.Dispose();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible mostrar los conceptos de anticipos.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                #region RECORREMOS EL ARREGLO PARA SELECCIONAR LOS COMBOS EN CASO DE QUE HAYA INFORMACION POR EMPRESA
                for (int i = 0; i < cuentasEmpresas.Length; i++)
                {
                    if (cuentasEmpresas[i].EMPRESA == empresaSeleccionada)
                    {
                        cbConceptos.SelectedItem = cuentasEmpresas[i].concepto_cp;
                        cbCuentaBancaria.SelectedIndex = cuentasEmpresas[i].cuentaBancariaNombre;
                        comboConcepto.SelectedIndex = cuentasEmpresas[i].conceptosBancosNombre;
                        cbBanco.SelectedItem = cuentasEmpresas[i].banco;
                        MostrarLayouts(cbBanco.Text);
                        cbFormatoArchivo.SelectedIndex = cbFormatoArchivo.FindString(cuentasEmpresas[i].formato_layout);
                        cbAnticipo.SelectedItem = cuentasEmpresas[i].concepto_anticipo;
                    }
                }
                #endregion

                conn.Desconectar();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Al seleccionar esta opción unicamente se harán los Layouts de las empresas en color verde en pantalla\n¿Desea continuar?",
                "Mensaje de la aplicación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            else
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].DefaultCellStyle.BackColor == dgvSuccess)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
                    
                if(this.DialogResult != DialogResult.OK)
                    MessageBox.Show("No se capturo datos para la generación de Layout de alguna empresa, no se podra generar documento","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                Close();
            }
                
        }

        private void cbBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbCompFiscal.Visible = false;
            cbCompFiscal.Checked = false;
            cbFormatoArchivo.Items.Clear();
            switch (cbBanco.Text)
            {
                case "BANORTE": 

                    break;

                case "BBVA BANCOMER":
                    cbFormatoArchivo.Items.Add("Pagos Internacionales OPI");
                    cbFormatoArchivo.Items.Add("Traspaso Mismo Banco");
                    cbFormatoArchivo.Items.Add("Pagos Mismo Banco");
                    cbFormatoArchivo.Items.Add("Traspasos Interbancarios");
                    cbFormatoArchivo.Items.Add("Pagos Interbancarios");
                    cbFormatoArchivo.Items.Add("Pagos a convenios CIE");
                    cbCompFiscal.Visible = true;
                    break;

                case "HSBC":
                    cbFormatoArchivo.Items.Add("Pagos Internacionales OPI");
                    cbFormatoArchivo.Items.Add("MX - Transferencia a tercero");
                    cbFormatoArchivo.Items.Add("MX - SPEI");
                    break;

                case "SANTANDER":
                    cbFormatoArchivo.Items.Add("Interbancaria");
                    cbFormatoArchivo.Items.Add("Cuentas Santander sin comprobante fiscal");
                    break;
            }



            cbCuentaBancaria.Items.Clear();

            C_CONEXIONFIREBIRD conn = new C_CONEXIONFIREBIRD();
            FbCommand cmd;
            FbDataReader read;
            C_BANCOS claveFiscal = cbBanco.SelectedItem as C_BANCOS;
            #region MOSTRAMOS LAS CUENTAS BANCARIAS CON CLAVE FISCAL 
            string select = "SELECT ";
            select += "      cb.cuenta_ban_id, ";
            select += "      cb.nombre, ";
            select += "      cb.num_cuenta, ";
            select += "      cb.moneda_id ";
            select += " FROM cuentas_bancarias cb ";
            select += " JOIN bancos b ON(cb.banco_id = b.banco_id) ";
            select += "WHERE upper(b.clave_fiscal) = '" + claveFiscal.CLAVE_FISCAL + "'";

            if (conn.ConectarFB(empresaSeleccionada))
            {
                try
                {
                    cmd = new FbCommand(select, conn.FBC);
                    read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        // cbCuentaBancaria.Items.Add(new CuentasBancarias(read["NOMBRE"].ToString(), read["NUM_CUENTA"].ToString()));
                        cbCuentaBancaria.Items.Add(new CuentasBancarias(Convert.ToInt32(read["CUENTA_BAN_ID"].ToString()), read["NOMBRE"].ToString(), read["NUM_CUENTA"].ToString(), Convert.ToInt32(read["MONEDA_ID"].ToString())));
                    }
                    read.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible mostrar las cuentas bancarias.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                conn.Desconectar();
            }
            #endregion
        }

        private void cbFormatoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFormatoArchivo.Text == "Pagos Interbancarios" || cbFormatoArchivo.Text == "Traspasos Interbancarios")
            {
                txtdispo.Visible = true;
                cbdispo.Visible = true;
                cbdispo.SelectedIndex = 0;
            }
            else
            {
                txtdispo.Visible = false;
                cbdispo.Visible = false;
                cbdispo.SelectedIndex = 0;
            }
        }

        private void MostrarLayouts(string banco)
        {
            if (string.IsNullOrEmpty(banco))
                return;
            cbFormatoArchivo.Items.Clear();
            switch (cbBanco.Text)
            {
                case "BANORTE":

                    break;

                case "BBVA BANCOMER":
                    cbFormatoArchivo.Items.Add("Pagos Internacionales OPI");
                    cbFormatoArchivo.Items.Add("Traspaso Mismo Banco");
                    cbFormatoArchivo.Items.Add("Pagos Mismo Banco");
                    cbFormatoArchivo.Items.Add("Traspasos Interbancarios");
                    cbFormatoArchivo.Items.Add("Pagos Interbancarios");
                    cbFormatoArchivo.Items.Add("Pagos a convenios CIE");
                    cbCompFiscal.Visible = true;
                    break;

                case "HSBC":
                    cbFormatoArchivo.Items.Add("Pagos Internacionales OPI");
                    cbFormatoArchivo.Items.Add("MX - Transferencia a tercero");
                    cbFormatoArchivo.Items.Add("MX - SPEI");
                    break;

                case "SANTANDER":
                    cbFormatoArchivo.Items.Add("Interbancaria");
                    cbFormatoArchivo.Items.Add("Cuentas Santander sin comprobante fiscal");
                    break;
            }
        }
    }
}
