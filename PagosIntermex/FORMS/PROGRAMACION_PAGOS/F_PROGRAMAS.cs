using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using PagosIntermex.FORMS.REPORTES;

namespace PagosIntermex
{
    public partial class F_PROGRAMAS : Form
    {
        public int _empresaID;

        public string _empresaNombres;
        public C_EMPRESAS[] empresas;
        public C_USUARIOS usuarioLogueado;
        private string ESTATU_PROC = "";
        int renglon = 0;
        int rengloProg = 0;

        class DOCTO_PR_DET
        {
            public string FOLIO_MICROSIP { get; set; }
            public string EMPRESA { get; set; }
            public string PROVEEDOR_NOMBRE { get; set; }

            public int DOCTO_PP_DET_ID { get; set; }
        }

        public F_PROGRAMAS()
        {
            InitializeComponent();
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            btnProgramacion.FlatAppearance.BorderColor = Color.FromArgb(46, 59, 104);
            bntPeticiones.FlatAppearance.BorderColor = Color.FromArgb(46, 59, 104);
            
            txtUser.Text = usuarioLogueado.Usuario;
            //txtEmpresa.Text = _empresaNombre;
            dataGridView1.DoubleBuffered(true);

            this.Text = "Sistema de pagos multiempresa";
            this.WindowState = FormWindowState.Maximized;


            ProgramacionPagos pp = new ProgramacionPagos();
            bool estatus = chBEstatus.Checked;
            bool fechas = chBFechas.Checked;
            pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones,usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());

            //en caso de que el usuario sea programador de pagos
            if (usuarioLogueado.Tesoreria == "S" || usuarioLogueado.U_ROL == "A")
            {
                button1.Text = "Programar Pagos";
                btnProgramacion.PerformClick();
                panelProgramaciones.Visible = true;
                panelCrearProgramacion.Visible = true;

               

                //verificamos que sea requisitante
                if (usuarioLogueado.Requisitante == "S")
                {
                    panelPeticiones.Visible = true;
                    chbVerAmbas.Visible = true;
                    panelCrearPeticion.Visible = true;


                }
                else
                {
                    panelPeticiones.Visible = false;
                    panelCrearPeticion.Visible = false;
                }
            }
            else
            {
                panelProgramaciones.Visible = false;
                panelCrearProgramacion.Visible = false;
                //verificamos que sea requisitante
                if (usuarioLogueado.Requisitante == "S")
                {
                    button1.Text = "Crear Peticiones";
                    bntPeticiones.PerformClick();
                    panelPeticiones.Visible = true;
                    panelCrearPeticion.Visible = true;
                }
                else
                {
                    panelPeticiones.Visible = false;
                    panelCrearPeticion.Visible = false;
                }
            }
            tipoUsuario.Text = usuarioLogueado.TIPO_USUARIO();
            txtNivel.Text = usuarioLogueado.NIVEL.ToString() == "0"?"5": usuarioLogueado.NIVEL.ToString();
            txtRequi.Text = usuarioLogueado.Requisitante == "S" ? "Si" : "No";
            txtTesoreria.Text = usuarioLogueado.Tesoreria == "S" ? "Si" : "No";

            notifyIcon1.Icon =
   new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + @"\Icon.ico");
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Sistema de Pagos";

            if (dataGridView1.RowCount > 0)
                dataGridView1.Rows[0].Selected = true;

        }


        private void F_PROGRAMAS_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Icon =
   new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + @"\Icon.ico");
            notifyIcon1.Visible = false;
            notifyIcon1.Text = "Sistema de Pagos";
        }



        private void CargarGrid2(string Estatus, List<DateTime> Fechas, string todos = "")
        {
            string msg_local = "";
            int i = 0;

            ProgramacionPagos PP = new ProgramacionPagos();
            DataTable _DATOS = PP.EncabezadoPagos(ESTATU_PROC,Estatus, Fechas, empresas,usuarioLogueado.Usuario, out msg_local, todos);

            // _no_pagos = // buscar el numero de pagos
            dataGridView1.Rows.Clear();

            if (_DATOS.Rows.Count > 0)
            {
                foreach (DataRow Fila in _DATOS.Rows)
                {
                    // Asingar datos al grid DOCTO_PR_ID, FOLIO, FECHA_PAGO, ESTATUS, USUARIO_CREADOR, FECHA_HORA_CREACION, EMPRESA
                    dataGridView1.Rows.Add();
                    dataGridView1["Folio", i].Value = Convert.ToString(Fila["FOLIO"]);
                    dataGridView1["Estatus", i].Value = EstatusDocumento( Convert.ToString(Fila["ESTATUS"]));
                    string aux = Convert.ToString(Fila["IMPORTE_PAGOS"]);
                    if (aux.Length == 0)
                        aux = "0.0";
                    dataGridView1["Importe_total", i].Value = Convert.ToDouble(aux);
                    aux = Convert.ToString(Fila["IMP_AUTO"]);
                    if (aux.Length == 0)
                        aux = "0.0";
                    dataGridView1["ImporteAutorizado", i].Value = Convert.ToDouble(aux);
                    dataGridView1["Numero_pagos", i].Value = Convert.ToString(Fila["CANT_PAGOS"]);
                    dataGridView1["Fecha_pago", i].Value = Convert.ToDateTime(Fila["FECHA_PAGO"]).ToString("dd/MM/yyyy");
                    dataGridView1["Usuario", i].Value = Convert.ToString(Fila["USUARIO_CREADOR"]);
                    dataGridView1["Fecha_creacion", i].Value = Convert.ToDateTime(Fila["FECHA_HORA_CREACION"]).ToString("dd/MM/yyyy HH:mm:ss");
                    aux = Convert.ToString(Fila["ESTATUS_PROC"]);
                    if (aux.Length == 0)
                        aux = "";
                    dataGridView1["ESTATUS_PROC", i].Value = this.EstatusProceso(aux);

                    aux = Convert.ToString(Fila["IMPORTE_CAPTURISTA"]);
                    if (aux.Length == 0)
                        aux = "0.0";
                    dataGridView1["IMPORTE_CAPTURISTA", i].Value = Convert.ToDouble(aux);

                    dataGridView1["Autorizacion", i].Value = this.EstatusProceso(Convert.ToString(Fila["ESTATUS_PROC"]));
                    dataGridView1["DOCTO_PR_ID", i].Value = Convert.ToString(Fila["DOCTO_PR_ID"]);
                    i++;
                }


                if (rbUsuarioActivo.Checked)
                {
                    for (int j = 0; j < dataGridView1.RowCount; j++)
                    {
                        if (dataGridView1["Usuario", j].Value.ToString() == usuarioLogueado.Usuario)
                        {
                            dataGridView1.Rows[j].Visible = true;
                        }
                        else
                        {
                            dataGridView1.Rows[j].Visible = false;
                        }
                    }
                }
                    
            }

            if (msg_local.Length > 0)
            {
                MessageBox.Show(msg_local, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string EstatusProceso(string aux)
        {
            if (aux == "A")
                aux = "Autorizado";
            else if (aux == "P")
                aux = "Pendiente de enviar";
            else if (aux == "C")
                aux = "Cancelado";
            else if (aux == "F")
                aux = "Finalizado";
            else if (aux == "L")
                aux = "Liberado";
            else if (aux == "B")
                aux = "Banco";
            else if (aux == "E")
                aux = "Pendiente";
            else if (aux == "R")
                aux = "Rechazado";
            else if (aux == "")
                aux = "No asignado";
            return aux;
        }


        private string EstatusDocumento(string aux)
        {
            switch (aux)
            {
                case "A":
                    aux = "Activo";
                    break;
                case "C":
                    aux = "Cancelado";
                    break;
                case "R":
                    aux = "Rechazado";
                    break;
                case "P":
                    aux = "Pendiente";
                    break;
                case "F":
                    aux = "Finalizado";
                    break;
                default:
                    aux = "";
                    break;
            }

            /* if (aux == "A")
                aux = "Activo";
            else if (aux == "C")
                aux = "Cancelado";
            else if (aux == "R")
                aux = "Rechazado";
            else if (aux == "P")
                aux = "Pendiente";
            else if (aux == "F")
                aux = "Finalizado";
            else if (aux == "")
                aux = "No asignado"; */

            return aux;
        }


        // Frecuencias de pago (COMENTADO PORQUE NO LO QUISIERON)
        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // F_FRECUENCIAPROGRAMA FEE = new F_FRECUENCIAPROGRAMA();
            // FEE.ShowDialog();
        }

        // Configurar conexiones
        private void conexionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_CONFIGCONEXIONES f7 = new F_CONFIGCONEXIONES();
            f7.ShowDialog();
        }

        // Control de usuarios
        private void controlDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_CONTROLUSUARIOS FCU = new F_CONTROLUSUARIOS();
            FCU.usuarioLogueado = usuarioLogueado;
            FCU.NOMBRE_EMPRESA = empresas;
            FCU.ShowDialog();
        }

        // Configurar series del sistema
        private void configurarSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_SERIES fs = new F_SERIES();
            fs.ShowDialog();
        }

        #region EVENTOS CHECKBOX
        private void chBEstatus_CheckedChanged(object sender, EventArgs e)
        {
            if (chBEstatus.Checked == true)
                cBEstatus.Enabled = true;
            else if (chBEstatus.Checked == false)
            {
                ESTATU_PROC = "";
                cBEstatus.Enabled = false;
            }
        }

        private void chBFechas_CheckedChanged(object sender, EventArgs e)
        {
            if (chBFechas.Checked == true)
            {
                dTPInicio.Enabled = true;
                dTPFin.Enabled = true;
            }
            else if (chBFechas.Checked == false)
            {
                dTPInicio.Enabled = false;
                dTPFin.Enabled = false;
            }
        }
        #endregion


        #region RADIOBUTTONS
        private void rbUsuarioActivo_CheckedChanged(object sender, EventArgs e)
        {
            if (usuarioLogueado.Requisitante == "N")
            {

            }
            else
            {
                if (rbUsuarioActivo.Checked)
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1["Usuario", i].Value.ToString() == usuarioLogueado.Usuario)
                        {
                            dataGridView1.Rows[i].Visible = true;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Visible = false;
                        }
                    }
                }
            }
        }

        private void rbTodasProgramaciones_CheckedChanged(object sender, EventArgs e)
        {
            if (usuarioLogueado.Requisitante == "N")
            {

            }
            else
            {
                if (rbTodasProgramaciones.Checked)
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        dataGridView1.Rows[i].Visible = true;

                    }
                }
            }
        }
        #endregion

        #region BOTONES
        private void bBuscar_Click(object sender, EventArgs e)
        {
            if (usuarioLogueado.Requisitante == "N")
            {
                string msg_local = "";
                try
                {
                    string aux = Convert.ToString(cBEstatus.SelectedItem), _estatus = "A";
                    List<DateTime> _fehas = new List<DateTime>();
                    if (chBEstatus.Checked == true)
                    {
                        if (aux.Length > 0)
                        {
                            if (aux == "Autorizado")
                            {
                                _estatus = "A";
                                ESTATU_PROC = "A";
                            }
                            else if (aux == "Cancelado")
                            {
                                _estatus = "X";
                                ESTATU_PROC = "X";
                            }
                            else if (aux == "Liberado")
                            {
                                _estatus = "A";
                                ESTATU_PROC = "L";
                            }
                            else if (aux == "Pendiente")
                            {
                                _estatus = "A";
                                ESTATU_PROC = "E";
                            }
                            else if (aux == "Pendiente por enviar")
                            {
                                _estatus = "A";
                                ESTATU_PROC = "P";
                            }
                            else if (aux == "No asignado")
                                ESTATU_PROC = "N";

                        }
                    }
                    if (chBFechas.Checked == true)
                    {
                        _fehas.Add(Convert.ToDateTime(dTPInicio.Value.ToString("dd/MM/yyyy") + " 00:00:00"));
                        _fehas.Add(Convert.ToDateTime(dTPFin.Value.ToString("dd/MM/yyyy") + " 23:59:59"));
                    }

                    /*if (rbTodasProgramaciones.Checked)
                        CargarGrid(_estatus, _fehas, "S");
                    else
                        CargarGrid(_estatus, _fehas);*/

                    ProgramacionPagos pp = new ProgramacionPagos();
                    bool estatus = chBEstatus.Checked;
                    bool fechas = chBFechas.Checked;
                    pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
                }
                catch (Exception Ex)
                {
                    msg_local = Ex.Message;
                }
                if (msg_local.Length > 0)
                {
                    MessageBox.Show(msg_local, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                ProgramacionPagos pp = new ProgramacionPagos();
                bool estatus = chBEstatus.Checked;
                bool fechas = chBFechas.Checked;
                pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado,estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
            }
        }



        // BOTON PROGRAMAR PAGOS
        private void button1_Click(object sender, EventArgs e)
        {
            F_PROGRAMACIONPAGOS FEE = new F_PROGRAMACIONPAGOS();
            //FEE.SET_DATABASE_NAME = _empresaNombre;
            FEE.NOMBRE_EMPESA = empresas;
            FEE.usuarioLogueado = usuarioLogueado;
            FEE.ShowDialog();

            string aux = cBEstatus.SelectedText;
            string _estatus = "A";

            List<DateTime> _fehas = new List<DateTime>();

            if (chBEstatus.Checked == true)
            {
                if (aux.Length > 0)
                {
                    if (aux == "Activos")
                        _estatus = "A";
                    else if (aux == "Pendientes")
                        _estatus = "P";
                    else if (aux == "Cancelados")
                        _estatus = "C";
                    else if (aux == "Finalizados")
                        _estatus = "F";
                }

            }

            if (chBFechas.Checked == true)
            {
                _fehas.Add(Convert.ToDateTime(dTPInicio.Value.ToString("dd/MM/yyyy") + " 00:00:00"));
                _fehas.Add(Convert.ToDateTime(dTPFin.Value.ToString("dd/MM/yyyy") + " 23:59:59"));
            }

            //CargarGrid(_estatus, _fehas);
            ProgramacionPagos pp = new ProgramacionPagos();
            bool estatus = chBEstatus.Checked;
            bool fechas = chBFechas.Checked;
            pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
        }

        #endregion

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                verProgramaciónToolStripMenuItem.PerformClick();
            }
        }

        #region MENU STRIP dgv programaciones

        // Autorización de programaciones de pagos
        private void autorizaciónDeProgramacionesDePagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Genera un reporte donde se muestra por programación quienes autorizarón o rechazarón los pagos por un periodo de tiempo asi como los comentarios.");
        }

        // Estatus de pagos
        private void estatusDePagosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Genera un reporte donde se ve el avance de los pagos tanto en Microsip cono en el banco por un periodo indicado.");
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {

                if (dataGridView1.RowCount > 0)
                {
                    generarPagosToolStripMenuItem.Text = "Generar documento para pagos y creditos en Microsip";

                    verProgramaciónToolStripMenuItem.Enabled = true;
                    generarPagosToolStripMenuItem.Enabled = true;
                    asignarImporteAutorizadoToolStripMenuItem.Enabled = true;
                    modificarProgramaciónToolStripMenuItem.Enabled = true;
                    eliminarProgramaciónDePagoToolStripMenuItem.Enabled = true;

                    string estatus = dataGridView1.CurrentRow.Cells["ESTATUS_PROC"].Value.ToString();
                    string estautsGeneral = Convert.ToString(dataGridView1.CurrentRow.Cells["Estatus"].Value);

                    if (estatus == "No asignado")
                    {
                        generarPagosToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = true;
                    }

                    if (estatus == "Pendiente de enviar")
                    {
                        generarPagosToolStripMenuItem.Enabled = false;
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = true;


                    }

                    if (estatus == "Cancelado")
                    {
                        generarPagosToolStripMenuItem.Enabled = false;
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = false;
                    }

                    if (estatus == "Pendiente")
                    {
                        generarPagosToolStripMenuItem.Enabled = false;
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = true;
                       // eliminarProgramaciónDePagoToolStripMenuItem.Enabled = false;

                        if (usuarioLogueado.U_ROL == "A")
                        {

                            asignarImporteAutorizadoToolStripMenuItem.Enabled = true;
                            asignarImporteAutorizadoToolStripMenuItem.Text = "Autorizar pago";
                        }
                    }

                    if (estatus == "Autorizado")
                    {
                        if (usuarioLogueado.NIVEL == 1 || usuarioLogueado.NIVEL_SUPREMO == 1)
                        {
                          //  asignarImporteAutorizadoToolStripMenuItem.Enabled;
                        }

                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = false;
                      //  eliminarProgramaciónDePagoToolStripMenuItem.Enabled = false;


                    }

                    if (estatus == "Rechazado")
                    {
                        generarPagosToolStripMenuItem.Enabled = false;
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = false;
                      //  eliminarProgramaciónDePagoToolStripMenuItem.Enabled = false;
                    }

                    if (estatus == "Banco")
                    {
                        generarPagosToolStripMenuItem.Text = "Generar documento para pagos";
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = false;
                       eliminarProgramaciónDePagoToolStripMenuItem.Enabled = false;
                    }

                    if (estatus == "Liberado")
                    {
                        generarPagosToolStripMenuItem.Text = "Generar documento para pagos";
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = false;
                        eliminarProgramaciónDePagoToolStripMenuItem.Enabled = false;
                    }

                    if (estatus == "Finalizado")
                    {
                        generarPagosToolStripMenuItem.Enabled = false;
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = false;
                        eliminarProgramaciónDePagoToolStripMenuItem.Enabled = false;
                    }

                    if (estatus == "Petición en Revisión")
                    {
                        verProgramaciónToolStripMenuItem.Enabled = true;
                        generarPagosToolStripMenuItem.Enabled = false;
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem.Enabled = false;
                       // eliminarProgramaciónDePagoToolStripMenuItem.Enabled = false;
                    }
                }
                else
                {
                    verProgramaciónToolStripMenuItem.Enabled = false;
                    generarPagosToolStripMenuItem.Enabled = false;
                    asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                    modificarProgramaciónToolStripMenuItem.Enabled = false;
                    eliminarProgramaciónDePagoToolStripMenuItem.Enabled = false;
                }


                #region para mostrar autorizar o no
                string nivelProgActual = dataGridView1.CurrentRow.Cells["NIVEL"].Value.ToString();

                if (nivelProgActual != "1")
                {
                    if (AUTORIZADO(
                    Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentCell.RowIndex].Value)) == "S")
                    {
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        if (dataGridView1.CurrentRow.Cells["ESTATUS_PROC"].Value.ToString() != "Autorizado")
                        {
                            if (USUARIO_AUT_PROG(Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentCell.RowIndex].Value)))
                                asignarImporteAutorizadoToolStripMenuItem.Enabled = true;
                            else
                                asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        }
                    }
                   
                }
                else
                {
                    if (usuarioLogueado.NIVEL_SUPREMO == 1)
                    {
                        if (AUTORIZADO_NIVEL_1(
                        Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentCell.RowIndex].Value)) == "S")
                        {
                            asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            if (dataGridView1.CurrentRow.Cells["ESTATUS_PROC"].Value.ToString() != "Autorizado")
                            {
                                if (USUARIO_AUT_PROG(Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentCell.RowIndex].Value)))
                                    asignarImporteAutorizadoToolStripMenuItem.Enabled = true;
                                else
                                    asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        asignarImporteAutorizadoToolStripMenuItem.Enabled = false;
                    }
                }
                #endregion
                //asignarImporteAutorizadoToolStripMenuItem.Enabled = true;

            }
            catch
            {

            }
        }

        // Ver detalle de la programación
        private void verProgramaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_DETALLEPROGRAMACIONPAGOS FEE = new F_DETALLEPROGRAMACIONPAGOS();
            FEE.FOLIO = Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentRow.Index].Value);
            FEE.EMPRESA = empresas;
            FEE.usuarioLogueado = usuarioLogueado;
            FEE.Ver = "S";
            FEE.NIVEL_DATAGRID= Convert.ToString(dataGridView1["NIVEL", dataGridView1.CurrentCell.RowIndex].Value);
            FEE.ShowDialog();
        }

        // Generar documento para pagos y creditos en Microsip
        private void generarPagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string folio = Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentRow.Index].Value);
                string estatus = dataGridView1.CurrentRow.Cells["ESTATUS_PROC"].Value.ToString();
                string mensaje = "¿Desea generar los pagos en Microsip y el documento de importación para el banco del documento seleccionado " + folio + "?";

                if ((estatus == "Banco") || (estatus == "Liberado"))
                {
                    mensaje = "¿Desea volver a generar el documento de importación para el banco del documento seleccionado " + folio + "?";
                }

                if (MessageBox.Show(mensaje, "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FolderBrowserDialog folder = new FolderBrowserDialog();
                    C_FUNCIONES fun = new C_FUNCIONES();
                    C_EMPRESAS[] empresasPagos = fun.EmpresasPagos(Convert.ToInt32(dataGridView1["DOCTO_PR_ID", dataGridView1.CurrentRow.Index].Value));
                    F_CUENTASBANCARIAS fo = new F_CUENTASBANCARIAS();
                    fo.empresa = empresasPagos;
                    bool seleccionoFolder = false;
                    string path = "";

                    if (fo.ShowDialog() == DialogResult.OK)
                    {
                        C_CUENTAS_BAN[] arregloCuentas = fo.cuentasEmpresas;

                        for (int i = 0; i < arregloCuentas.Length; i++)
                        {
                            if ((arregloCuentas[i].concepto_cp != "" || arregloCuentas[i].concepto_anticipo != "") && (arregloCuentas[i].cuenta_ordenante != ""))
                            {

                                SaveFileDialog sa = new SaveFileDialog();
                                string FileName = "";
                                string extension = "";
                                string nombreTXT = "";
                                bool esCSV = false;
                                //declarar formato si es csv o txt
                                switch (arregloCuentas[i].banco)
                                {
                                    case "BBVA BANCOMER":
                                        sa.Filter = "TXT (rellenado con 0) | *.txt";
                                        sa.DefaultExt = "txt";
                                        FileName = "Cuenta " + arregloCuentas[i].cuenta_ordenante.Substring(arregloCuentas[i].cuenta_ordenante.Length - 4, 4) + " Fecha " + DateTime.Now.ToString("yyyyMMdd HHmmss");
                                        extension = ".txt";
                                        break;

                                    case "HSBC":
                                        sa.Filter = "CSV (delimitado por comas) | *.csv";
                                        sa.DefaultExt = "csv";
                                        FileName = "Cuenta " + arregloCuentas[i].cuenta_ordenante.Substring(arregloCuentas[i].cuenta_ordenante.Length - 4, 4) + " Fecha " + DateTime.Now.ToString("yyyyMMdd HHmmss"); 
                                        extension = ".csv";
                                        esCSV = true;
                                        break;

                                    case "SANTANDER":
                                        sa.Filter = "TXT (rellenado con 0) | *.txt";
                                        sa.DefaultExt = "txt";
                                        FileName = "Cuenta " + arregloCuentas[i].cuenta_ordenante.Substring(arregloCuentas[i].cuenta_ordenante.Length - 4, 4) + " Fecha " + DateTime.Now.ToString("yyyyMMdd HHmmss");
                                        extension = ".txt";
                                        break;
                                }
                                //para guardar en un directorio en especifico donde creara carpetas separadas por empresa y fecha
                                if (!seleccionoFolder)
                                {
                                    if (folder.ShowDialog() == DialogResult.OK)
                                    {
                                        path = getPath(arregloCuentas[i].EMPRESA, folder);
                                        seleccionoFolder = true;
                                    }
                                }
                                else
                                {
                                    path = getPath(arregloCuentas[i].EMPRESA, folder);
                                }

                                sa.FileName = path + "\\" + FileName + extension;
                                nombreTXT = path + "\\" + FileName + extension;

                                C_CREAR_TXT layoutTXT = new C_CREAR_TXT();
                                layoutTXT.RUTA = path;
                                layoutTXT.NOMBRE_ARCHIVO = FileName + extension;

                                StringBuilder csv = new StringBuilder();

                                // PROGRAMACION_ID, FOLIO
                                int fila = dataGridView1.CurrentRow.Index, PROGRAMACION_ID = Convert.ToInt32(dataGridView1["DOCTO_PR_ID", fila].Value);

                                C_AGREGACREDITO ac = new C_AGREGACREDITO();
                                string error = "";
                                if (ac.GenerarLiberarCreditos(PROGRAMACION_ID, folio, usuarioLogueado.Usuario, ref csv, arregloCuentas[i],layoutTXT, ref error))
                                {
                                    if (esCSV)
                                        File.WriteAllText(sa.FileName, csv.ToString());


                                    
                                }
                                else
                                {
                                    if(error != "")
                                        MessageBox.Show("No se pudo generar el credito o no se pudo realizar el layout del pago\n" + error, "Mensaje de la aplicacion",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                                    else
                                        MessageBox.Show("No se pudo generar el credito o no se pudo realizar el layout del pago", "Mensaje de la aplicacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                            }
                            else
                            {
                                MessageBox.Show("No se selecciono ninguna cuenta ordenante o concepto de pago.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                        //CargarGrid("A", new List<DateTime>());
                        ProgramacionPagos pp = new ProgramacionPagos();
                        bool estatuss = chBEstatus.Checked;
                        bool fechas = chBFechas.Checked;
                        pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatuss, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrio un error inesperado\n" + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        // Asignar Importe Autorizado
        private void asignarImporteAutorizadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string aux = Convert.ToString(dataGridView1["ESTATUS_PROC", dataGridView1.CurrentCell.RowIndex].Value);

            if (aux == "P")
            {
                MessageBox.Show("No se puede autorizar una programación ya autorizada.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                F_DETALLEPROGRAMACIONPAGOS _FDPP = new F_DETALLEPROGRAMACIONPAGOS();
                _FDPP.usuarioLogueado = usuarioLogueado;
                _FDPP.MOVIMIENTO = "A";
                _FDPP.FOLIO = Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentRow.Index].Value);
                _FDPP.EMPRESA = this.empresas;
                _FDPP.NIVEL_DATAGRID = Convert.ToString(dataGridView1["NIVEL", dataGridView1.CurrentCell.RowIndex].Value);
                _FDPP.ShowDialog();

                // CargarGrid("A", new List<DateTime>());
                ProgramacionPagos pp = new ProgramacionPagos();
                bool estatus = chBEstatus.Checked;
                bool fechas = chBFechas.Checked;
                pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
            }

            
        }

        private string AUTORIZADO(string folio)
        {
            string autorizado = "N";
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if(con.ConectarSQL())
                {
                    string query = "SELECT ";
                    query += " case when COUNT(*)=0 then 'N' else 'S' end AS AUTORIZADO ";
                    query += "  from P_AUT_DOCTOS_PR as ADPR ";
                    query += " inner join P_DOCTOS_PR_DET as DPRD on ADPR.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                    query += " inner join P_DOCTOS_PR as DPR on DPRD.DOCTO_PR_ID = DPR.DOCTO_PR_ID ";
                    query += " where DPR.FOLIO = @FOLIO and ADPR.NIVEL= @NIVEL  and ADPR.ESTATUS='A' and ADPR.USUARIO_ID= @USUARIO ";

                    SqlCommand sc = new SqlCommand(query, con.SC);
                    sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                    sc.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = usuarioLogueado.NIVEL;
                    sc.Parameters.Add("@USUARIO", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                        autorizado = Convert.ToString(sdr["AUTORIZADO"]);
                    sc.Dispose();
                    sdr.Close();

                    if (autorizado == "N")
                    {
                        if (usuarioLogueado.NIVEL_SUPREMO != 0)
                        {
                            query = "SELECT ";
                            query += " case when COUNT(*)=0 then 'N' else 'S' end AS AUTORIZADO ";
                            query += "  from P_AUT_DOCTOS_PR as ADPR ";
                            query += " inner join P_DOCTOS_PR_DET as DPRD on ADPR.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                            query += " inner join P_DOCTOS_PR as DPR on DPRD.DOCTO_PR_ID = DPR.DOCTO_PR_ID ";
                            query += " where DPR.FOLIO = @FOLIO and ADPR.NIVEL= @NIVEL  and ADPR.ESTATUS='A' and ADPR.USUARIO_ID= @USUARIO ";

                            sc = new SqlCommand(query, con.SC);
                            sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                            sc.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = usuarioLogueado.NIVEL_SUPREMO;
                            sc.Parameters.Add("@USUARIO", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                            sdr = sc.ExecuteReader();
                            while (sdr.Read())
                                autorizado = Convert.ToString(sdr["AUTORIZADO"]);
                            sc.Dispose();
                            sdr.Close();
                        }
                    }

                    con.Desconectar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error\n" + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            return autorizado;
        }

        private string AUTORIZADO_NIVEL_1(string folio)
        {
            string autorizado = "N";
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {

                    if (usuarioLogueado.NIVEL_SUPREMO != 0)
                    {
                        string query = "SELECT ";
                        query += " case when COUNT(*)=0 then 'N' else 'S' end AS AUTORIZADO ";
                        query += "  from P_AUT_DOCTOS_PR as ADPR ";
                        query += " inner join P_DOCTOS_PR_DET as DPRD on ADPR.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                        query += " inner join P_DOCTOS_PR as DPR on DPRD.DOCTO_PR_ID = DPR.DOCTO_PR_ID ";
                        query += " where DPR.FOLIO = @FOLIO and ADPR.NIVEL= @NIVEL  and ADPR.ESTATUS='A' and ADPR.USUARIO_ID= @USUARIO ";

                        SqlCommand sc = new SqlCommand(query, con.SC);
                        sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                        sc.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = usuarioLogueado.NIVEL_SUPREMO;
                        sc.Parameters.Add("@USUARIO", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                        SqlDataReader sdr = sc.ExecuteReader();
                        while (sdr.Read())
                            autorizado = Convert.ToString(sdr["AUTORIZADO"]);
                        sc.Dispose();
                        sdr.Close();
                    }


                    con.Desconectar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return autorizado;
        }

        private bool USUARIO_AUT_PROG(string folio)
        {
            bool autorizado = false;
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string query = "select u.Usuario_id from P_DOCTOS_PR pd ";
                    query += " JOIN P_DOCTOS_PR_DET ppd on ppd.DOCTO_PR_ID = pd.DOCTO_PR_ID ";
                    query += "  JOIN P_AUT_DOCTOS_PR pad on pad.DOCTO_PR_DET_ID = ppd.DOCTO_PR_DET_ID ";
                    query += " JOIN USUARIOS u on u.Usuario_id = pad.USUARIO_ID ";
                    query += " where pd.FOLIO = @FOLIO ";
                    query += " AND u.Usuario_id = @USUARIO ";

                    SqlCommand sc = new SqlCommand(query, con.SC);
                    sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                    sc.Parameters.Add("@USUARIO", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        autorizado = true;
                        break;
                    }
                    sc.Dispose();
                    sdr.Close();
                    con.Desconectar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return autorizado;
        }

        // Modificar programación
        private void modificarProgramaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg_local = "";

            try
            {
                //MessageBox.Show("Abrir del detalle  de la programacion con la funcion de modificcacion");
                F_DETALLEPROGRAMACIONPAGOS _FDPP = new F_DETALLEPROGRAMACIONPAGOS();
                _FDPP.usuarioLogueado = usuarioLogueado;
                _FDPP.MOVIMIENTO = "M";
                _FDPP.FOLIO = Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentRow.Index].Value);
                _FDPP.EMPRESA = empresas;
                _FDPP.FECHA_PAGO = Convert.ToString(dataGridView1["Fecha_pago", dataGridView1.CurrentRow.Index].Value);
                _FDPP.DOCTO_PR_ID = Convert.ToInt32(Convert.ToString(dataGridView1["DOCTO_PR_ID", dataGridView1.CurrentRow.Index].Value));
                _FDPP.ShowDialog();

                //CargarGrid("A", new List<DateTime>());
                ProgramacionPagos pp = new ProgramacionPagos();
                bool estatus = chBEstatus.Checked;
                bool fechas = chBFechas.Checked;
                pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
            }
            catch (Exception Ex)
            {
                msg_local = Ex.Message;
            }

            if (msg_local.Length > 0)
            {
                MessageBox.Show(msg_local, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }      


        private void programarPagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1.PerformClick();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void abrirSistemaPagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }


        // Eliminar programación de pago
        private void eliminarProgramaciónDePagoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string docto_pr_id = Convert.ToString(dataGridView1["DOCTO_PR_ID", dataGridView1.CurrentRow.Index].Value);
            string folio = Convert.ToString(dataGridView1["Folio", dataGridView1.CurrentRow.Index].Value);

            bool eliminado = false;

            if (docto_pr_id != "")
            {
                if (MessageBox.Show("Se eliminara la programación de pago '" + folio +"' esta acción no se podra revertir.\n\n¿Desea continuar?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    F_CANCELACION canc = new F_CANCELACION();
                    canc.usuario = usuarioLogueado;
                    canc.folio = folio;
                    canc.ShowDialog();

                    if (canc.exito)
                    {
                        C_ConexionSQL conn = new C_ConexionSQL();
                        SqlTransaction transaction;
                        SqlCommand cmd;

                        C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();

                        if (reg.LeerRegistros(false))
                        {
                            if (conn.ConectarSQL())
                            {
                                transaction = conn.SC.BeginTransaction();

                                try
                                {
                                    DOCTO_PR_DET[] doctos = new DOCTO_PR_DET[0];
                                    List<int> doctos_pr_det = new List<int>();

                                    #region OBTENEMOS LOS FOLIOS MICROSIP DE LOS DETALLES
                                    string query = "SELECT * FROM P_DOCTOS_PR_DET prd ";
                                    query += " WHERE prd.DOCTO_PR_ID = " + docto_pr_id;
                                    cmd = new SqlCommand(query, conn.SC, transaction);
                                    SqlDataReader sdr = cmd.ExecuteReader();
                                    while (sdr.Read())
                                    {
                                        Array.Resize(ref doctos, doctos.Length + 1);
                                        doctos[doctos.Length - 1] = new DOCTO_PR_DET();

                                        doctos[doctos.Length-1].FOLIO_MICROSIP = Convert.ToString(sdr["FOLIO_MICROSIP"]);
                                        doctos[doctos.Length - 1].EMPRESA = Convert.ToString(sdr["EMPRESA"]);
                                        doctos[doctos.Length - 1].PROVEEDOR_NOMBRE = Convert.ToString(sdr["PROVEEDOR_NOMBRE"]);
                                        doctos[doctos.Length - 1].DOCTO_PP_DET_ID = Convert.ToInt32(Convert.ToString(sdr["DOCTO_PP_DET_ID"]));
                                    }
                                    sdr.Close();
                                    cmd.Dispose();
                                    #endregion

                                    #region CANCELAR LOS DETALLES

                                    query = " SELECT * FROM P_DOCTOS_PR_DET ";
                                    query += " WHERE DOCTO_PR_ID = " + docto_pr_id;
                                    cmd = new SqlCommand(query, conn.SC, transaction);
                                    sdr = cmd.ExecuteReader();
                                    while (sdr.Read())
                                        doctos_pr_det.Add(Convert.ToInt32(Convert.ToString(sdr["DOCTO_PR_DET_ID"])));
                                    sdr.Close();
                                    cmd.Dispose();



                                    query = "UPDATE P_DOCTOS_PR_DET SET ESTATUS = 'X' ";
                                    query += " where DOCTO_PR_ID = " + docto_pr_id;
                                    cmd = new SqlCommand(query, conn.SC, transaction);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    #endregion

                                    #region CANCELAR ENCABEZADO DOCTOS_PR
                                    query = "UPDATE P_DOCTOS_PR SET ESTATUS = 'X' ";
                                    query += " , ESTATUS_PROC = 'X' ";
                                    query += " , MOTIVO_CANCELACION = @MOTIVO ";
                                    query += " where DOCTO_PR_ID = " + docto_pr_id;
                                    cmd = new SqlCommand(query, conn.SC, transaction);
                                    cmd.Parameters.Add("@MOTIVO", SqlDbType.VarChar).Value = canc.motivo;
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                    #endregion

                                    #region REVIVIMOS LOS DETALLES DE LAS PETICIONES

                                    for (int i = 0; i < doctos.Length; i++)
                                    {
                                        query = " UPDATE P_DOCTOS_PP_DET SET ";
                                        query += " ESTATUS = 'C' ";
                                        query += " WHERE FOLIO_MICROSIP = @FOLIO ";
                                        query += " AND PROVEEDOR_NOMBRE = @NOMBRE ";
                                        query += " AND EMPRESA = @EMPRESA ";
                                        query += " AND DOCTO_PP_DET_ID = @DOCTO ";
                                        cmd = new SqlCommand(query, conn.SC, transaction);
                                        cmd.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = doctos[i].FOLIO_MICROSIP;
                                        cmd.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = doctos[i].PROVEEDOR_NOMBRE;
                                        cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = doctos[i].EMPRESA;
                                        cmd.Parameters.Add("@DOCTO", SqlDbType.VarChar).Value = doctos[i].DOCTO_PP_DET_ID;
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                    #endregion

                                    #region ELIMINAMOS A LOS USUARIOS
                                    for (int i = 0; i < doctos_pr_det.Count; i++)
                                    {
                                        string eliminar = "DELETE FROM P_AUT_DOCTOS_PR ";
                                        eliminar += " WHERE DOCTO_PR_DET_ID = " + doctos_pr_det[i];
                                        cmd = new SqlCommand(eliminar, conn.SC, transaction);
                                        cmd.ExecuteNonQuery();
                                    }
                                    #endregion


                                    transaction.Commit();

                                    eliminado = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("No fue posible eliminar la programación de pagos '" + folio + "'.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    transaction.Rollback();
                                }

                                conn.Desconectar();
                            }
                        }

                        if (eliminado)
                        {
                            MessageBox.Show("Se elimino la programación de pago.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //CargarGrid("", new List<DateTime>());
                            ProgramacionPagos pp = new ProgramacionPagos();
                            bool estatus = chBEstatus.Checked;
                            bool fechas = chBFechas.Checked;
                            pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Programación invalida.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        private string getPath(string empresaOrdenante, FolderBrowserDialog folder)
        {
            string path = "";

            string empresa = folder.SelectedPath + "\\" + empresaOrdenante;
            path = folder.SelectedPath + "\\" + empresaOrdenante + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
            //crear directorios
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(empresa);
                Directory.CreateDirectory(path);

            }
            return path;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnProgramacion_Click(object sender, EventArgs e)
        {
            if (!chbVerAmbas.Checked)
            {
                btnProgramacion.BackColor = Color.FromArgb(17, 97, 238);
                btnProgramacion.FlatAppearance.BorderColor = Color.White;
                bntPeticiones.FlatAppearance.BorderColor = Color.FromArgb(46, 59, 104);
                bntPeticiones.BackColor = Color.FromArgb(46, 59, 104);

                splitContainer1.Panel2.Enabled = false;
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();

                splitContainer1.Panel1.Enabled = true;
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel1.Show();

            }
        }

        private void bntPeticiones_Click(object sender, EventArgs e)
        {
            if (!chbVerAmbas.Checked)
            {
                bntPeticiones.BackColor = Color.FromArgb(17, 97, 238);
                bntPeticiones.FlatAppearance.BorderColor = Color.White;
                btnProgramacion.FlatAppearance.BorderColor = Color.FromArgb(46, 59, 104);
                btnProgramacion.BackColor = Color.FromArgb(46, 59, 104);



                splitContainer1.Panel1.Enabled = false;
                splitContainer1.Panel1Collapsed = true;
                splitContainer1.Panel1.Hide();

                splitContainer1.Panel2.Enabled = true;
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.Panel2.Show();

            }
        }

        private void chbVerAmbas_CheckedChanged(object sender, EventArgs e)
        {
            if (chbVerAmbas.Checked)
            {
                bntPeticiones.FlatAppearance.BorderColor = Color.White;
                btnProgramacion.FlatAppearance.BorderColor = Color.White;
                btnProgramacion.Enabled = false;
                bntPeticiones.Enabled = false;

                splitContainer1.Panel1.Enabled = true;
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel1.Show();

                splitContainer1.Panel2.Enabled = true;
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.Panel2.Show();
            }
            else
            {
                bntPeticiones.FlatAppearance.BorderColor = Color.FromArgb(46, 59, 104);
                btnProgramacion.Enabled = true;
                bntPeticiones.Enabled = true;

                splitContainer1.Panel2.Enabled = false;
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();

                splitContainer1.Panel1.Enabled = true;
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel1.Show();
            }
        }

        private void btnPeticion_Click(object sender, EventArgs e)
        {
            F_PETICIONESPAGOS FEE = new F_PETICIONESPAGOS();
            //FEE.SET_DATABASE_NAME = _empresaNombre;
            FEE.NOMBRE_EMPESA = empresas;
            FEE.usuarioLogueado = usuarioLogueado;
            FEE.ShowDialog();

            string aux = cBEstatus.SelectedText;
            string _estatus = "A";

            List<DateTime> _fehas = new List<DateTime>();

            if (chBEstatus.Checked == true)
            {
                if (aux.Length > 0)
                {
                    if (aux == "Activos")
                        _estatus = "A";
                    else if (aux == "Pendientes")
                        _estatus = "P";
                    else if (aux == "Cancelados")
                        _estatus = "C";
                    else if (aux == "Finalizados")
                        _estatus = "F";
                }

            }

            if (chBFechas.Checked == true)
            {
                _fehas.Add(Convert.ToDateTime(dTPInicio.Value.ToString("dd/MM/yyyy") + " 00:00:00"));
                _fehas.Add(Convert.ToDateTime(dTPFin.Value.ToString("dd/MM/yyyy") + " 23:59:59"));
            }

            //CargarGrid(_estatus, _fehas);
            ProgramacionPagos pp = new ProgramacionPagos();
            bool estatus = chBEstatus.Checked;
            bool fechas = chBFechas.Checked;
            pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());

        }

        #region MENUSTRIP DGV PETICIONES
        private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {
            try
            {

                if (dgvPeticiones.RowCount > 0)
                {
                    generarPagosToolStripMenuItem2.Text = "Generar documento para pagos y creditos en Microsip";

                    verProgramaciónToolStripMenuItem2.Enabled = true;
                    generarPagosToolStripMenuItem2.Enabled = true;
                    modificarProgramaciónToolStripMenuItem2.Enabled = true;
                    eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = true;

                    string estatus = dgvPeticiones.CurrentRow.Cells["ESTATUS_PROC_2"].Value.ToString();
                    string estautsGeneral = Convert.ToString(dgvPeticiones.CurrentRow.Cells["Estatus_2"].Value);

                    if (estatus == "No asignado")
                    {
                        generarPagosToolStripMenuItem2.Enabled = false;
                        modificarProgramaciónToolStripMenuItem2.Enabled = true;
                    }

                    if (estatus == "Pendiente de enviar")
                    {
                        generarPagosToolStripMenuItem2.Enabled = false;
                        modificarProgramaciónToolStripMenuItem2.Enabled = true;
                        enviarPeticion.Enabled = true;

                    }

                    if (estatus == "Cancelado")
                    {
                        generarPagosToolStripMenuItem2.Enabled = false;
                        modificarProgramaciónToolStripMenuItem2.Enabled = false;
                        eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;
                        enviarPeticion.Enabled = false;
                    }

                    if (estatus == "Pendiente")
                    {
                        generarPagosToolStripMenuItem.Enabled = false;
                        modificarProgramaciónToolStripMenuItem2.Enabled = true;
                       // eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;

                    }

                    if (estatus == "Autorizado")
                    {
                        modificarProgramaciónToolStripMenuItem2.Enabled = false;
                        eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;
                    }

                    if (estatus == "Rechazado")
                    {
                        generarPagosToolStripMenuItem2.Enabled = false;
                        modificarProgramaciónToolStripMenuItem2.Enabled = false;
                       // eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;
                    }

                    if (estatus == "Banco")
                    {
                        generarPagosToolStripMenuItem.Text = "Generar documento para pagos";
                        modificarProgramaciónToolStripMenuItem2.Enabled = false;
                        //eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;
                    }

                    if (estatus == "Liberado")
                    {
                        generarPagosToolStripMenuItem2.Text = "Generar documento para pagos";
                        modificarProgramaciónToolStripMenuItem2.Enabled = false;
                        //eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;
                    }

                    if (estatus == "Finalizado")
                    {
                        generarPagosToolStripMenuItem2.Enabled = false;
                        modificarProgramaciónToolStripMenuItem2.Enabled = false;
                        //eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;
                    }

                    if (estatus == "Petición en Revisión")
                    {
                        verProgramaciónToolStripMenuItem2.Enabled = true;
                        generarPagosToolStripMenuItem2.Enabled = false;
                        modificarProgramaciónToolStripMenuItem2.Enabled = false;
                        enviarPeticion.Enabled = false;
                        //eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;
                    }
                }
                else
                {
                    verProgramaciónToolStripMenuItem2.Enabled = false;
                    generarPagosToolStripMenuItem2.Enabled = false;
                    modificarProgramaciónToolStripMenuItem2.Enabled = false;
                   // eliminarProgramaciónDePagoToolStripMenuItem2.Enabled = false;
                }
            }
            catch
            {

            }
        }

        private void verProgramaciónToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int docto_pr_id = Convert.ToInt32(Convert.ToString(dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.CurrentRow.Index].Value));

            F_VER_DETALLE_PETICION ver = new F_VER_DETALLE_PETICION();
            ver.DOCTO_PP_DET_ID = docto_pr_id;
            ver.usuario = usuarioLogueado;
            ver.ShowDialog();
        }

        private void modificarProgramaciónToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string msg_local = "";

            try
            {
                //MessageBox.Show("Abrir del detalle  de la programacion con la funcion de modificcacion");
                F_DETALLE_PETICIONES _FDPP = new F_DETALLE_PETICIONES();
                _FDPP.usuarioLogueado = usuarioLogueado;
                _FDPP.MOVIMIENTO = "M";
                _FDPP.FOLIO = Convert.ToString(dgvPeticiones["Folio_2", dgvPeticiones.CurrentRow.Index].Value);
                _FDPP.EMPRESA = empresas;
                _FDPP.FECHA_PAGO = Convert.ToString(dgvPeticiones["Fecha_pago_2", dgvPeticiones.CurrentRow.Index].Value);
                _FDPP.DOCTO_PR_ID = Convert.ToInt32(Convert.ToString(dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.CurrentRow.Index].Value));
                //_FDPP.NIVELGRID = Convert.ToString(dataGridView1["NIVEL", dataGridView1.CurrentCell.RowIndex].Value);
                _FDPP.ShowDialog();

                //CargarGrid("A", new List<DateTime>());
                ProgramacionPagos pp = new ProgramacionPagos();
                bool estatus = chBEstatus.Checked;
                bool fechas = chBFechas.Checked;
                pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
            }
            catch (Exception Ex)
            {
                msg_local = Ex.Message;
            }

            if (msg_local.Length > 0)
            {
                MessageBox.Show(msg_local, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void generarPagosToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                string folio = Convert.ToString(dgvPeticiones["Folio_2", dgvPeticiones.CurrentRow.Index].Value);
                string estatus = dgvPeticiones.CurrentRow.Cells["ESTATUS_PROC_2"].Value.ToString();
                string mensaje = "¿Desea generar los pagos en Microsip y el documento de importación para el banco del documento seleccionado " + folio + "?";

                if ((estatus == "Banco") || (estatus == "Liberado"))
                {
                    mensaje = "¿Desea volver a generar el documento de importación para el banco del documento seleccionado " + folio + "?";
                }

                if (MessageBox.Show(mensaje, "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FolderBrowserDialog folder = new FolderBrowserDialog();
                    C_FUNCIONES fun = new C_FUNCIONES();
                    C_EMPRESAS[] empresasPagos = fun.EmpresasPagos(Convert.ToInt32(dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.CurrentRow.Index].Value));
                    F_CUENTASBANCARIAS fo = new F_CUENTASBANCARIAS();
                    fo.empresa = empresasPagos;
                    bool seleccionoFolder = false;
                    string path = "";

                    if (fo.ShowDialog() == DialogResult.OK)
                    {
                        C_CUENTAS_BAN[] arregloCuentas = fo.cuentasEmpresas;

                        for (int i = 0; i < arregloCuentas.Length; i++)
                        {
                            if ((arregloCuentas[i].concepto_cp != "" || arregloCuentas[i].concepto_anticipo != "") && (arregloCuentas[i].cuenta_ordenante != ""))
                            {

                                SaveFileDialog sa = new SaveFileDialog();
                                string FileName = "";
                                string extension = "";
                                string nombreTXT = "";
                                bool esCSV = false;
                                //declarar formato si es csv o txt
                                switch (arregloCuentas[i].banco)
                                {
                                    case "BBVA BANCOMER":
                                        sa.Filter = "TXT (rellenado con 0) | *.txt";
                                        sa.DefaultExt = "txt";
                                        FileName = "Cuenta " + arregloCuentas[i].cuenta_ordenante.Substring(arregloCuentas[i].cuenta_ordenante.Length - 4, 4) + " Fecha " + DateTime.Now.ToString("yyyyMMdd HHmmss");
                                        extension = ".txt";
                                        break;

                                    case "HSBC":
                                        sa.Filter = "CSV (delimitado por comas) | *.csv";
                                        sa.DefaultExt = "csv";
                                        FileName = "Cuenta " + arregloCuentas[i].cuenta_ordenante.Substring(arregloCuentas[i].cuenta_ordenante.Length - 4, 4) + " Fecha " + DateTime.Now.ToString("yyyyMMdd HHmmss");
                                        extension = ".csv";
                                        esCSV = true;
                                        break;

                                    case "SANTANDER":
                                        sa.Filter = "TXT (rellenado con 0) | *.txt";
                                        sa.DefaultExt = "txt";
                                        FileName = "Cuenta " + arregloCuentas[i].cuenta_ordenante.Substring(arregloCuentas[i].cuenta_ordenante.Length - 4, 4) + " Fecha " + DateTime.Now.ToString("yyyyMMdd HHmmss");
                                        extension = ".txt";
                                        break;
                                }
                                //para guardar en un directorio en especifico donde creara carpetas separadas por empresa y fecha
                                if (!seleccionoFolder)
                                {
                                    if (folder.ShowDialog() == DialogResult.OK)
                                    {
                                        path = getPath(arregloCuentas[i].EMPRESA, folder);
                                        seleccionoFolder = true;
                                    }
                                }
                                else
                                {
                                    path = getPath(arregloCuentas[i].EMPRESA, folder);
                                }

                                sa.FileName = path + "\\" + FileName + extension;
                                nombreTXT = path + "\\" + FileName + extension;

                                C_CREAR_TXT layoutTXT = new C_CREAR_TXT();
                                layoutTXT.RUTA = path;
                                layoutTXT.NOMBRE_ARCHIVO = FileName + extension;

                                StringBuilder csv = new StringBuilder();

                                // PROGRAMACION_ID, FOLIO
                                int fila = dgvPeticiones.CurrentRow.Index, PROGRAMACION_ID = Convert.ToInt32(dgvPeticiones["DOCTO_PR_ID_2", fila].Value);

                                C_AGREGACREDITO ac = new C_AGREGACREDITO();
                                string error = "";
                                if (ac.GenerarLiberarCreditos(PROGRAMACION_ID, folio, usuarioLogueado.Usuario, ref csv, arregloCuentas[i], layoutTXT, ref error))
                                {
                                    if (esCSV)
                                        File.WriteAllText(sa.FileName, csv.ToString());

                                }
                                else
                                {
                                    if (error != "")
                                        MessageBox.Show("No se pudo generar el credito o no se pudo realizar el layout del pago\n" + error, "Mensaje de la aplicacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    else
                                        MessageBox.Show("No se pudo generar el credito o no se pudo realizar el layout del pago", "Mensaje de la aplicacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                            }
                            else
                            {
                                MessageBox.Show("No se selecciono ninguna cuenta ordenante o concepto de pago.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                        //CargarGrid("A", new List<DateTime>());
                        ProgramacionPagos pp = new ProgramacionPagos();
                        bool estatuss = chBEstatus.Checked;
                        bool fechas = chBFechas.Checked;
                        pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatuss, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error inesperado\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void eliminarProgramaciónDePagoToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string docto_pr_id = Convert.ToString(dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.CurrentRow.Index].Value);
            string folio = Convert.ToString(dgvPeticiones["Folio_2", dgvPeticiones.CurrentRow.Index].Value);
            string estatus2 = Convert.ToString(dgvPeticiones["Estatus_2", dgvPeticiones.CurrentRow.Index].Value);

            bool eliminado = false;

            if (docto_pr_id != "")
            {
                

                if (MessageBox.Show("Se eliminara la programación de pago '" + folio + "' esta acción no se podra revertir.\n\n¿Desea continuar?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    F_CANCELACION canc = new F_CANCELACION();
                    canc.usuario = usuarioLogueado;
                    canc.folio = folio;
                    canc.ShowDialog();

                    if (canc.exito)
                    {

                        C_ConexionSQL conn = new C_ConexionSQL();
                        SqlTransaction transaction;
                        SqlCommand cmd;

                        C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();

                        if (reg.LeerRegistros(false))
                        {
                            if (conn.ConectarSQL())
                            {
                                transaction = conn.SC.BeginTransaction();

                                try
                                {
                                    if (estatus2 == "Activo")
                                    {
                                        if (MessageBox.Show("Esta petición ya esta siendo revisada, al eliminarla tendra que reiniciar el proceso de pago desde el inicio.\n¿Continuar?",
                                            "Mensaje de la aplicación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                            == DialogResult.No)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                //si ya esta en proceso de pago se debe hacer el siguiente proceso:
                                                //se busca los detalles de esta peticion
                                                List<string> folio_microsip = new List<string>();
                                                List<int> docto_pr_det_id = new List<int>();
                                                List<int> docto_pp_det_id = new List<int>();

                                                string query = "select ppd.DOCTO_PP_DET_ID ";
                                                query += " ,ppd.FOLIO_MICROSIP ";
                                                query += " from P_DOCTOS_PP pp ";
                                                query += " join P_DOCTOS_PP_DET ppd on ppd.DOCTO_PP_ID = pp.DOCTO_PP_ID ";
                                                query += " where ppd.DOCTO_PP_ID = " + docto_pr_id;
                                                query += " and ppd.ESTATUS = 'T'";
                                                SqlCommand sc = new SqlCommand(query, conn.SC, transaction);
                                                SqlDataReader sdr = sc.ExecuteReader();

                                                while (sdr.Read())
                                                {
                                                    folio_microsip.Add(Convert.ToString(sdr["FOLIO_MICROSIP"]));
                                                    docto_pp_det_id.Add(Convert.ToInt32(Convert.ToString(sdr["DOCTO_PP_DET_ID"])));
                                                }
                                                sc.Dispose();
                                                sdr.Close();

                                                if (folio_microsip.Count > 0)
                                                {
                                                    List<int> docto_pr = new List<int>();

                                                    #region se busca los detalles de doctos_pr_det para cambiar sus estatus
                                                    for (int i = 0; i < folio_microsip.Count; i++)
                                                    {
                                                        //guaramos el id del encabezado para usarlo despues
                                                        string select = "SELECT * ";
                                                        select += " FROM P_DOCTOS_PR_DET ";
                                                        select += " WHERE FOLIO_MICROSIP = '" + folio_microsip[i] + "'";
                                                        select += " AND DOCTO_PP_DET_ID = " + docto_pp_det_id[i];

                                                        sc = new SqlCommand(select, conn.SC, transaction);
                                                        sdr = sc.ExecuteReader();
                                                        while (sdr.Read())
                                                        {
                                                            docto_pr.Add(Convert.ToInt32(Convert.ToString(sdr["DOCTO_PR_ID"])));
                                                            docto_pr_det_id.Add(Convert.ToInt32(Convert.ToString(sdr["DOCTO_PR_DET_ID"])));
                                                        }
                                                        sc.Dispose();
                                                        sdr.Close();

                                                        //se cambia el estatus a X en los detalles de doctos pr
                                                        string cancelar = "UPDATE P_DOCTOS_PR_DET ";
                                                        cancelar += " SET ESTATUS = 'X' ";
                                                        cancelar += " WHERE FOLIO_MICROSIP = @FOLIO ";
                                                        cancelar += " AND DOCTO_PP_DET_ID = @DET";

                                                        sc = new SqlCommand(cancelar, conn.SC, transaction);
                                                        sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio_microsip[i];
                                                        sc.Parameters.Add("@DET", SqlDbType.VarChar).Value = docto_pp_det_id[i];
                                                        sc.ExecuteNonQuery();

                                                    }
                                                    #endregion

                                                    #region verificamos si el encabezado tiene mas detalles si es 0 actualizamos el estatus del encabezado a X
                                                    docto_pr = docto_pr.Distinct().ToList();

                                                    for (int i = 0; i < docto_pr.Count; i++)
                                                    {
                                                        string cancelar = " select count(ppd.ESTATUS ) TOTAL ";
                                                        cancelar += " from P_DOCTOS_PR_DET ppd ";
                                                        cancelar += " where ppd.DOCTO_PR_ID = " + docto_pr[i];
                                                        cancelar += " AND ppd.ESTATUS = 'C'";

                                                        sc = new SqlCommand(cancelar, conn.SC, transaction);
                                                        sdr = sc.ExecuteReader();
                                                        while (sdr.Read())
                                                        {
                                                            //si da 0 se actualiza el encabezado
                                                            if (Convert.ToInt32(Convert.ToString(sdr["TOTAL"])) == 0)
                                                            {
                                                                string encabezado = "UPDATE P_DOCTOS_PR SET ";
                                                                encabezado += " ESTATUS = 'X' ";
                                                                encabezado += " ,ESTATUS_PROC = 'X' ";
                                                                encabezado += " ,MOTIVO_CANCELACION = @MOTIVO ";
                                                                encabezado += " WHERE DOCTO_PR_ID = " + docto_pr[i];
                                                                SqlCommand sc2 = new SqlCommand(encabezado, conn.SC, transaction);
                                                                sc2.Parameters.Add("@MOTIVO", SqlDbType.VarChar).Value = canc.motivo;
                                                                sc2.ExecuteNonQuery();
                                                                sc2.Dispose();
                                                            }
                                                        }
                                                        sc.Dispose();
                                                        sdr.Close();
                                                    }
                                                    #endregion

                                                    #region se eliminan los usuarios autorizadores
                                                    for (int i = 0; i < docto_pr_det_id.Count; i++)
                                                    {
                                                        string eliminar = "DELETE FROM P_AUT_DOCTOS_PR ";
                                                        eliminar += " WHERE DOCTO_PR_DET_ID = " + docto_pr_det_id[i];
                                                        sc = new SqlCommand(eliminar, conn.SC, transaction);
                                                        sc.ExecuteNonQuery();
                                                    }
                                                    #endregion

                                                    eliminado = true;
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No se encontro el detalle de esta petición", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                transaction.Rollback();
                                                MessageBox.Show("Error al eliminar pago\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                return;
                                            }
                                        }
                                    }

                                    cmd = new SqlCommand("UPDATE p_doctos_pp_det SET ESTATUS = 'X' WHERE docto_pp_id = " + docto_pr_id, conn.SC, transaction);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    cmd = new SqlCommand("UPDATE p_doctos_pp SET ESTATUS = 'X', ESTATUS_PROC = 'X' ,MOTIVO_CANCELACION = @MOTIVO WHERE docto_pp_id = " + docto_pr_id, conn.SC, transaction);
                                    cmd.Parameters.Add("@MOTIVO", SqlDbType.VarChar).Value = canc.motivo;
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();

                                    transaction.Commit();

                                    eliminado = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("No fue posible eliminar la programación de pagos '" + folio + "'.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    transaction.Rollback();
                                }

                                conn.Desconectar();
                            }
                        }

                        if (eliminado)
                        {
                            MessageBox.Show("Se elimino la programación de pago.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //CargarGrid("", new List<DateTime>());
                            ProgramacionPagos pp = new ProgramacionPagos();
                            bool estatus = chBEstatus.Checked;
                            bool fechas = chBFechas.Checked;
                            pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
                        }
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Programación invalida.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void enviarPeticion_Click(object sender, EventArgs e)
        {
            try
            {
                string _folio = "";
                _folio = Convert.ToString(dgvPeticiones["Folio_2", dgvPeticiones.CurrentRow.Index].Value);

                DialogResult _resp = MessageBox.Show("¿Desea enviar a revisión el folio seleccionado \"" + _folio + "\"?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (_resp == DialogResult.Yes)
                {
                    C_CONEXIONWEBSVC websvc = new C_CONEXIONWEBSVC();
                    websvc.EnviarDoctoCorporativo(Convert.ToInt32(dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.CurrentRow.Index].Value), usuarioLogueado);
                }

                //CargarGrid("A", new List<DateTime>());
                ProgramacionPagos pp = new ProgramacionPagos();
                bool estatus = chBEstatus.Checked;
                bool fechas = chBFechas.Checked;
                pp.EncabezadoPagosPeticiones(dataGridView1, dgvPeticiones, usuarioLogueado, estatus, fechas, cBEstatus.Text, dTPInicio.Value.ToString(), dTPFin.Value.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrio un error.\n"+ ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void dgvPeticiones_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    renglon = e.RowIndex;

                    dgvPeticiones.CurrentCell = dgvPeticiones.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvPeticiones.Rows[e.RowIndex].Selected = true;
                    dgvPeticiones.Focus();
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    rengloProg = e.RowIndex;

                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.Focus();
                }
            }
        }

        private void dgvPeticiones_DoubleClick(object sender, EventArgs e)
        {
            int docto_pr_id = Convert.ToInt32(Convert.ToString(dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.CurrentRow.Index].Value));

            F_VER_DETALLE_PETICION ver = new F_VER_DETALLE_PETICION();
            ver.DOCTO_PP_DET_ID = docto_pr_id;
            ver.usuario = usuarioLogueado;
            ver.ShowDialog();
        }

        private void detalleDeProgramaciónDePagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_REP_PROGRAMACION repo = new F_REP_PROGRAMACION();
            repo.ShowDialog();
        }

        private void repPago1_Click(object sender, EventArgs e)
        {
            int docto_pr_id = Convert.ToInt32(Convert.ToString(dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.CurrentRow.Index].Value));
            F_REP_PROGRAMACION repo = new F_REP_PROGRAMACION(docto_pr_id);
            repo.ShowDialog();
        }
    }
}
