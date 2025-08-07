using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagosIntermex
{
    public partial class F_DETALLE_PETICIONES : Form
    {
        #region VARIABLES GLOBALES
        public C_USUARIOS usuarioLogueado;
        int renglon = 0;
        public string FOLIO { set; get; }

        public string MOVIMIENTO { set; get; }


        public string ESTATUS { set; get; }

        public C_EMPRESAS[] EMPRESA { set; get; }
        public string FECHA_PAGO { get; set; }
        public int DOCTO_PR_ID { get; set; }

        private bool eliminarPagos = false;
         public string NIVELGRID { set; get; }
        public string Ver { set; get; }
    
        C_AUT_PAGOS[] pagos;

        bool NO_ELIMINABLE = false;
        #endregion
        public F_DETALLE_PETICIONES()
        {
            InitializeComponent();
        }

        int renglonProg = 0;
        private void Cargar()
        {
            C_AUT_PAGOS f = new C_AUT_PAGOS();
            pagos = f.GET_DETALLES(FOLIO,usuarioLogueado.NIVEL);
            C_USUARIOS u = new C_USUARIOS();
            //USUARIO_ID = u.GET_USUARIO_ID(USUARIO);
            string msg_local = "";

            try
            {
                #region SI ES AUTORIZACIÓN O MODIFICACION AGREGA LA COLUMNA "Autorizar" AL PRINCIPIO

                if (MOVIMIENTO == "A")
                {
                    if (dGVDetPagos.Columns[0].Name != "Autorizar")
                    {
                        DataGridViewCheckBoxColumn nueva = new DataGridViewCheckBoxColumn();
                        nueva.Visible = true;
                        nueva.ReadOnly = false;
                        nueva.HeaderText = "Autorizar";
                        nueva.Name = "Autorizar";
                        // nueva.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        nueva.Width = 58;
                        dGVDetPagos.Columns.Insert(0, nueva);


                        //si es administrador se pone la columa del importe del capturista 
                        if (usuarioLogueado.U_ROL == "A")
                        {
                            dGVDetPagos.Columns["IMPORTE_P"].HeaderText = "Imp. Capturista";
                        }
                    }

                    dGVDetPagos.ReadOnly = false;

                    this.Text = "Asignación de importes autorizados";
                }
                else if (MOVIMIENTO == "M")
                {
                    if (dGVDetPagos.Columns[0].Name != "Autorizar")
                    {
                        DataGridViewCheckBoxColumn nueva = new DataGridViewCheckBoxColumn();
                        nueva.Visible = false;
                        nueva.ReadOnly = false;
                        nueva.HeaderText = "Autorizar";
                        nueva.Name = "Autorizar";
                        // nueva.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        nueva.Width = 58;
                        dGVDetPagos.Columns.Insert(0, nueva);

                        //si es administrador se pone la columa del importe del capturista 
                        if (usuarioLogueado.U_ROL == "A")
                        {
                            dGVDetPagos.Columns["IMPORTE_P"].HeaderText = "Imp. Capturista";
                        }

                    }

                    dGVDetPagos.ReadOnly = false;

                    this.Text = "Modificación de importes autorizados";
                    this.bCambios.Visible = true;
                    btnAgregar.Visible = true;
                    pGuardarCambios.Visible = true;
                    pAgregarPagos.Visible = true;

                    eliminarRenglonToolStripMenuItem.Visible = true;
                }

                #endregion



                ProgramacionPagos PP = new ProgramacionPagos();
                DataTable _datos = new DataTable();

                this.Text += " - \"" + FOLIO + "\"";

                int i = 0;

                _datos = PP.DetallePagos(FOLIO, usuarioLogueado,Convert.ToInt32(NIVELGRID),Ver, out msg_local,"S");
                dGVDetPagos.Rows.Clear();
                foreach (DataRow _fila in _datos.Rows)
                {
                    dGVDetPagos.Rows.Add();

                    ESTATUS = Convert.ToString(_fila["ESTATUS_PROC"]);

                    if ((ESTATUS == "B") || (ESTATUS == "L"))
                    {
                        dGVDetPagos.Columns["C_PAGO"].Visible = true;
                    }

                    dGVDetPagos["DOCTO_PR_DET_ID", i].Value = Convert.ToString(_fila["DOCTO_PR_DET_ID"]);
                    dGVDetPagos["DOCTO_PR_DET_ID", i].ReadOnly = true;

                    string folioMSP = Convert.ToString(_fila["FOLIO_MICROSIP"]);
                    dGVDetPagos["FOLIO_P", i].Value = folioMSP;
                    dGVDetPagos["FOLIO_P", i].ReadOnly = true;
                    if (folioMSP.Contains("REQ"))
                    {
                        dGVDetPagos.Rows[dGVDetPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(219, 213, 169);
                        dGVDetPagos.Rows[dGVDetPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(201, 192, 126);
                    }


                    dGVDetPagos["FECHA_P", i].Value = Convert.ToDateTime(_fila["FECHA_CARGO"]).ToString("dd/MMMM/yyyy");
                    dGVDetPagos["FECHA_P", i].ReadOnly = true;

                    dGVDetPagos["PROVEEDOR_P", i].Value = Convert.ToString(_fila["PROVEEDOR_NOMBRE"]);
                    dGVDetPagos["PROVEEDOR_P", i].ReadOnly = true;

                    dGVDetPagos["P_EMPRESA", i].Value = Convert.ToString(_fila["EMPRESA"]);
                    dGVDetPagos["P_EMPRESA", i].ReadOnly = true;

                    dGVDetPagos["C_PROVEEDOR_ID", i].Value = Convert.ToString(_fila["PROVEEDOR_ID"]);
                    dGVDetPagos["C_PROVEEDOR_ID", i].ReadOnly = true;

                    dGVDetPagos["FECHA_VEN", i].Value = Convert.ToDateTime(_fila["FECHA_VENCIMIENTO"]).ToString("dd/MMMM/yyyy");
                    dGVDetPagos["FECHA_VEN", i].ReadOnly = true;

                    if (usuarioLogueado.U_ROL == "C")
                    {
                        dGVDetPagos["IMPORTE_P", i].Value = Convert.ToDouble(Convert.ToString(_fila["IMPORTE_PAGOS"])).ToString("C2");
                        dGVDetPagos["IMPORTE_P", i].ReadOnly = true;
                    }
                    else
                    {
                        string auxs = Convert.ToString(_fila["IMPORTE_CAPTURISTA"]);
                        if (auxs.Length == 0)
                            auxs = "0.0";
                        dGVDetPagos["IMPORTE_P", i].Value = Convert.ToDouble(auxs).ToString("C2");
                        dGVDetPagos["IMPORTE_P", i].ReadOnly = true;
                    }

                    dGVDetPagos["C_COMENTARIOS", i].Value = Convert.ToString(_fila["COMENTARIOS"]);
                    dGVDetPagos["C_COMENTARIOS", i].ReadOnly = true;

                    dGVDetPagos["C_PAGO", i].Value = Convert.ToString(_fila["FOLIO_CREDITO"]);
                    dGVDetPagos["C_PAGO", i].ReadOnly = true;

                    dGVDetPagos["PROVEEDOR_CLAVE", i].Value = Convert.ToString(_fila["PROVEEDOR_CLAVE"]);
                    dGVDetPagos["PROVEEDOR_CLAVE", i].ReadOnly = true;
                    dGVDetPagos["REQUISICION_ID", i].Value = Convert.ToString(_fila["REQUISICION_ID"]);
                    dGVDetPagos["REQUISICION_ID", i].ReadOnly = true;

                    string aux = "";
                    if (usuarioLogueado.U_ROL == "C")
                        aux = Convert.ToString(_fila["IMPORTE_CAPTURISTA"]);
                    else
                        aux = Convert.ToString(_fila["IMPORTE_AUTORIZADO"]);

                    if (aux.Length == 0)
                    {
                        aux = "0.0";
                    }

                    if (Convert.ToDouble(aux) > 0 && (MOVIMIENTO == "A" || MOVIMIENTO == "M"))
                    {
                        int docto_pr_det = Convert.ToInt32(Convert.ToString(_fila["DOCTO_PR_DET_ID"]));
                        //verificamos que este detalle este autorizado por todos
                        dGVDetPagos["Autorizar", i].Value = true;
                        if (f.DetalleAutorizado(docto_pr_det,usuarioLogueado.NIVEL))
                        {
                            //dGVDetPagos["Autorizar", i].Value = true;

                            if (MOVIMIENTO == "A")
                            {
                                dGVDetPagos["Autorizar", i].ReadOnly = true;
                            }
                            else if (MOVIMIENTO == "M")
                            {
                                dGVDetPagos["Autorizar", i].ReadOnly = false;
                            }
                        }
                    }

                    dGVDetPagos["IMPORTE_AUT", i].Value = Convert.ToDouble(aux).ToString("N2");

                    aux = Convert.ToString(_fila["ESTATUS"]);

                    #region muchos if que tuvieron que ser switch :P
                    if (aux == "P")
                    {
                        aux = "Pendiente";
                        dGVDetPagos["C_IMAGEN", i].Style.NullValue = null;
                    }
                    else if (aux == "C")
                    {
                        aux = "Correcto";
                        dGVDetPagos["C_IMAGEN", i].Style.NullValue = null;
                        dGVDetPagos["C_IMAGEN", i].Value = Image.FromFile(@"Imágenes/wait.png");
                    }
                    else if (aux == "M")
                    {
                        aux = "Modificar";
                        dGVDetPagos["C_IMAGEN", i].Style.NullValue = null;
                    }
                    else if (aux == "A")
                    {
                        aux = "Autorizado";
                        dGVDetPagos["C_IMAGEN", i].Value = Image.FromFile(@"Imágenes/ok.png");
                    }
                    else if (aux == "R")
                    {
                        aux = "Rechazado";
                        dGVDetPagos["C_IMAGEN", i].Value = Image.FromFile(@"Imágenes/parar.png");
                    }
                    else if (aux == "B")
                    {
                        aux = "En banco";
                        dGVDetPagos["C_IMAGEN", i].Value = Image.FromFile(@"Imágenes/ok.png");
                    }
                    else if (aux == "L")
                    {
                        aux = "Liberado";
                        dGVDetPagos["C_IMAGEN", i].Style.NullValue = null;
                        dGVDetPagos["C_IMAGEN", i].Value = Image.FromFile(@"Imágenes/ok.png");
                    }
                    else if (aux == "X")
                    {
                        aux = "Cancelado";
                        dGVDetPagos["C_IMAGEN", i].Value = Image.FromFile(@"Imágenes/parar.png");
                    }
                    else if (aux == "")
                    {
                        aux = "En revisión";
                        dGVDetPagos["C_IMAGEN", i].Style.NullValue = null;
                    }
                    #endregion

                    dGVDetPagos["ESTATUS_P", i].Value = aux;
                    dGVDetPagos["ESTATUS_P", i].ReadOnly = true;


                    //PINTAMOS LOS RENGLONES DEL DGV
                    for (int j = 0; j < pagos.Length; j++)
                    {
                        if (Convert.ToInt32(Convert.ToString(_fila["DOCTO_PR_DET_ID"])) == pagos[j].DOCTO_PR_DET_ID)
                        {
                            dGVDetPagos.Rows[i].DefaultCellStyle.BackColor = pagos[j].COLOR_RENGLON;
                            dGVDetPagos.Rows[i].DefaultCellStyle.SelectionBackColor = pagos[j].COLOR_SELECCION;
                            break;
                        }
                    }

                    i++;
                }

            }
            catch (Exception Ex)
            {
                msg_local = Ex.Message;
            }

            if (msg_local.Length > 0)
            {
                MessageBox.Show(msg_local, "Error");
            }
        }

        private void F_DETALLE_PETICIONES_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (eliminarPagos)
            {
                if (MessageBox.Show("Se han eliminado pagos y no se ha guardado los cambios.\n¿Desea guardar cambios?"
                    , "Mensaje de la aplicación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bCambios.PerformClick();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void F_DETALLE_PETICIONES_Load(object sender, EventArgs e)
        {
            dGVDetPagos.DoubleBuffered(true);

            // Cargar los pagos del folio seleccionado
            Cargar();

            WindowState = FormWindowState.Maximized;
            txtUser.Text = usuarioLogueado.Usuario;

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                C_PAGOS[] pago = new C_PAGOS[0];

                for (int i = 0; i < dGVDetPagos.RowCount; i++)
                {
                    Array.Resize(ref pago, pago.Length + 1);
                    pago[pago.Length - 1] = new C_PAGOS();

                    pago[pago.Length - 1].FOLIO = dGVDetPagos["FOLIO_P", i].Value.ToString();
                    pago[pago.Length - 1].DOCTO_PR_DET_ID = Convert.ToInt32(dGVDetPagos["DOCTO_PR_DET_ID", i].Value.ToString());
                    pago[pago.Length - 1].FECHA = dGVDetPagos["FECHA_P", i].Value.ToString();
                    pago[pago.Length - 1].PROVEEDOR = dGVDetPagos["PROVEEDOR_P", i].Value.ToString();
                    pago[pago.Length - 1].PROVEEDOR_ID = Convert.ToInt32(dGVDetPagos["C_PROVEEDOR_ID", i].Value.ToString());
                    pago[pago.Length - 1].FECHA_VENC = Convert.ToDateTime(dGVDetPagos["FECHA_VEN", i].Value.ToString());
                    if (!string.IsNullOrEmpty(dGVDetPagos["IMPORTE_P", i].Value.ToString()))
                        pago[pago.Length - 1].IMPORTE = Convert.ToDouble(dGVDetPagos["IMPORTE_P", i].Value.ToString().Replace("$", ""));
                    pago[pago.Length - 1].IMP_AUTORIZADO = Convert.ToDouble(dGVDetPagos["IMPORTE_AUT", i].Value.ToString());
                    pago[pago.Length - 1].ESTATUS = dGVDetPagos["ESTATUS_P", i].Value.ToString();
                    if (!string.IsNullOrEmpty(dGVDetPagos["C_PAGO", i].Value.ToString()))
                        pago[pago.Length - 1].PAGO = Convert.ToDouble(dGVDetPagos["C_PAGO", i].Value.ToString());

                    if (dGVDetPagos["FOLIO_P", i].Value.ToString().Contains("REQ"))
                    {
                        pago[pago.Length - 1].PAGO = Convert.ToDouble(dGVDetPagos["IMPORTE_AUT", i].Value.ToString());
                        pago[pago.Length - 1].REQ_ID = dGVDetPagos["REQUISICION_ID", i].Value.ToString();
                    }

                    pago[pago.Length - 1].EMPRESA = dGVDetPagos["P_EMPRESA", i].Value.ToString();
                    pago[pago.Length - 1].PROVEEDOR_CLAVE = dGVDetPagos["PROVEEDOR_CLAVE", i].Value.ToString();
                }

                //para usuarios

                F_PETICIONESPAGOS fp = new F_PETICIONESPAGOS();
                fp.MODIF = "S";
                fp.NOMBRE_EMPESA = EMPRESA;
                fp.pagosModif1 = pago;
                fp.FECHA_PAGO_MODIF = FECHA_PAGO;
                fp.DOCTO_PR_ID = DOCTO_PR_ID;
                fp.FOLIO_MODIF = FOLIO;
                fp.usuarioLogueado = usuarioLogueado;
                fp.ShowDialog();

                if (fp.MODIF == "EXITO")
                {
                    Cargar();
                }
            }
            catch
            {

            }
        }

        private void bCambios_Click(object sender, EventArgs e)
        {
            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

            C_ConexionSQL conexion = new C_ConexionSQL();
            SqlTransaction transaction;
            SqlCommand cmd;
            // FbDataReader reader;

            string msg_local = "";
            string consulta = "";


            if (registros.LeerRegistros(false))
            {
                if (conexion.ConectarSQL())
                {
                    transaction = conexion.SC.BeginTransaction(); // INICIALIZA LA TRANSACCIÓN

                    try
                    {    //Validar que no tenga Ceros
                        bool _Ceros = false;
                        double _ImporteTotal = 0.0, _importePagos = 0;
                        int _seleccionado = 0;
                        DialogResult _resp = DialogResult.No;
                        List<List<string>> _Seleccionados = new List<List<string>>();
                        List<List<string>> _NoSeleccionados = new List<List<string>>();
                        for (int i = 0; i < dGVDetPagos.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dGVDetPagos["Autorizar", i].Value))
                            {
                                double importe = 0, importeTotal = 0;
                                string aux = Convert.ToString(dGVDetPagos["IMPORTE_AUT", i].Value);
                                List<string> _seleccion = new List<string>();
                                if (aux.Length == 0)
                                    aux = "0.0";
                                importe = Convert.ToDouble(aux);
                                if (importe <= 0)
                                    _Ceros = true;
                                _ImporteTotal += importe;

                                string aux2 = "";
                                if (dGVDetPagos.Rows[i].Visible)
                                    aux2 = Convert.ToString(dGVDetPagos["IMPORTE_P", i].Value).Replace("$", "");

                                if (aux2.Length == 0)
                                    aux2 = "0.0";
                                importeTotal = Convert.ToDouble(aux2);
                                _importePagos += importeTotal;

                                _seleccion.Add(Convert.ToString(dGVDetPagos["DOCTO_PR_DET_ID", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["FOLIO_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["FECHA_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["PROVEEDOR_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["FECHA_VEN", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["IMPORTE_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["IMPORTE_AUT", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["ESTATUS_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["P_EMPRESA", i].Value));
                                _seleccionado += 1;
                                _Seleccionados.Add(_seleccion);
                            }
                            else
                            {
                                double importe = 0, importeTotal = 0;
                                string aux = Convert.ToString(dGVDetPagos["IMPORTE_AUT", i].Value);
                                List<string> _seleccion = new List<string>();
                                if (aux.Length == 0)
                                    aux = "0.0";
                                importe = Convert.ToDouble(aux);
                                if (importe <= 0)
                                    _Ceros = true;

                                string aux2 = "";
                                if (dGVDetPagos.Rows[i].Visible)
                                    aux2 = Convert.ToString(dGVDetPagos["IMPORTE_P", i].Value).Replace("$", "");

                                if (aux2.Length == 0)
                                    aux2 = "0.0";
                                importeTotal = Convert.ToDouble(aux2);
                                _importePagos += importeTotal;

                                //_ImporteTotal += importe;
                                _seleccion.Add(Convert.ToString(dGVDetPagos["DOCTO_PR_DET_ID", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["FOLIO_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["FECHA_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["PROVEEDOR_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["FECHA_VEN", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["IMPORTE_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["IMPORTE_AUT", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["ESTATUS_P", i].Value));
                                _seleccion.Add(Convert.ToString(dGVDetPagos["P_EMPRESA", i].Value));
                                _seleccionado += 1;
                                _NoSeleccionados.Add(_seleccion);
                            }
                        }
                        if (_seleccionado > 0)
                        {
                            if (_Ceros == true)
                            {
                                _resp = MessageBox.Show("Importe total solicitado \"" + _ImporteTotal.ToString("C2") + "\"\n\rEl monto solicitado de uno o mas " +
                                   "pagos es cero\n\r¿Desea Continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            }
                            else
                            {
                                if (MessageBox.Show("Importe total solicitado \"" + _ImporteTotal.ToString("C2") + "\n\r¿Desea Continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    return;
                            }
                            if (_Ceros == false || _resp == DialogResult.Yes)
                            {
                                if (usuarioLogueado.Requisitante == "N")
                                {
                                    consulta = "update P_DOCTOS_PR " +
                                            " set ";
                                    if (usuarioLogueado.U_ROL == "A")
                                        consulta += " IMPORTE_AUTORIZADO = " + _ImporteTotal;
                                    else
                                        consulta += " IMPORTE_CAPTURISTA = " + _ImporteTotal;
                                    consulta += " ,ESTATUS_PROC = 'P' " +
                                            " ,USUARIO_AUTORIZO = '" + usuarioLogueado.Usuario + "' " +
                                            " ,FECHA_HORA_AUTORIZO = '" + DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + "' " +
                                            " ,IMPORTE_PAGOS = " + _importePagos +
                                            " where " +
                                            " FOLIO = '" + FOLIO + "' ";
                                    //" EMPRESA = '" + EMPRESA + "'";
                                }
                                else
                                {
                                    consulta = "update P_DOCTOS_PP " +
                                            " set ";
                                    consulta += " IMPORTE_AUTORIZADO = " + _ImporteTotal;
                                    consulta += " ,IMPORTE_CAPTURISTA = " + _ImporteTotal;
                                    consulta += " ,ESTATUS_PROC = 'P' " +
                                            // " ,USUARIO_AUTORIZO = '" + USUARIO + "' " +
                                            //  " ,FECHA_HORA_AUTORIZO = '" + DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + "' " +
                                            " ,IMPORTE_PAGOS = " + _importePagos +
                                            " where " +
                                            " FOLIO = '" + FOLIO + "' ";
                                    //" EMPRESA = '" + EMPRESA + "'";
                                }
                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                cmd.ExecuteNonQuery();

                                foreach (List<string> _Pag_selec in _Seleccionados)
                                {
                                    string aux = Convert.ToString(_Pag_selec[6]).Replace(",", "");
                                    if (usuarioLogueado.Requisitante == "N")
                                    {
                                        consulta = "update P_DOCTOS_PR_DET " +
                                            " set ";
                                        if (usuarioLogueado.U_ROL == "A")
                                            consulta += " IMPORTE_AUTORIZADO = " + aux;
                                        else
                                            consulta += " IMPORTE_CAPTURISTA = " + aux;
                                        consulta += " ,ESTATUS = 'C' " +
                                            " ,EMPRESA = @EMPRESA " +
                                            " where DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                    }
                                    else
                                    {
                                        consulta = "update P_DOCTOS_PP_DET " +
                                            " set ";
                                        consulta += " IMPORTE_AUTORIZADO = " + aux;

                                        consulta += " ,ESTATUS = 'C' " +
                                            " ,EMPRESA = @EMPRESA " +
                                            " where DOCTO_PP_DET_ID = '" + _Pag_selec[0] + "'";
                                    }
                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                    cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = _Pag_selec[_Pag_selec.Count - 1];
                                    cmd.ExecuteNonQuery();
                                }
                                foreach (List<string> _Pag_selec in _NoSeleccionados)
                                {
                                    string aux = "0.0";
                                    if (usuarioLogueado.Requisitante == "N")
                                    {
                                        consulta = "update P_DOCTOS_PR_DET " +
                                            " set ";
                                        if (usuarioLogueado.U_ROL == "A")
                                            consulta += " IMPORTE_AUTORIZADO = " + aux;
                                        else
                                            consulta += " IMPORTE_CAPTURISTA = " + aux;
                                        consulta += " ,ESTATUS = 'P' " +
                                            " ,EMPRESA = @EMPRESA " +
                                            " where DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                    }
                                    else
                                    {
                                        consulta = "update P_DOCTOS_PP_DET " +
                                            " set ";
                                        consulta += " IMPORTE_AUTORIZADO = " + aux;
                                        consulta += " ,ESTATUS = 'P' " +
                                            " ,EMPRESA = @EMPRESA " +
                                            " where DOCTO_PP_DET_ID = '" + _Pag_selec[0] + "'";
                                    }
                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                    cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = _Pag_selec[_Pag_selec.Count - 1];
                                    cmd.ExecuteNonQuery();
                                }
                                //cmd.Cancel();
                                transaction.Commit();
                                conexion.Desconectar();
                                eliminarPagos = false;
                                MessageBox.Show("Programación guardada", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ningún importe ha sido seleccionado", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        Cargar();
                        eliminarPagos = false;
                    }
                    catch (Exception Ex)
                    {
                        transaction.Rollback();
                        msg_local = Ex.Message;
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo conectar a la base de datos " + registros.FB_BD + ".");
                }
            }
            else
            {
                MessageBox.Show("No se pudieron leer los registros de Windows.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (msg_local.Length > 0)
            {
                MessageBox.Show(msg_local, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void eliminarRenglonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que desea eliminar el cargo con folio " +
               dGVDetPagos["FOLIO_P", dGVDetPagos.CurrentCell.RowIndex].Value.ToString() + "?", "Mensaje de la aplicación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (MOVIMIENTO == "M")
                {
                    if (Convert.ToBoolean(dGVDetPagos["Autorizar", renglon].EditedFormattedValue))
                    {
                        /* dGVDetPagos["IMPORTE_AUT", renglon].Value = dGVDetPagos["IMPORTE_P", renglon].Value.ToString().Replace("$", "").Replace(",", "");

                         dGVDetPagos.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(150, 255, 150);
                         dGVDetPagos.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 100);*/
                        dGVDetPagos["IMPORTE_AUT", renglon].Value = "0.00";

                        dGVDetPagos.CurrentRow.DefaultCellStyle.BackColor = Color.Empty;
                        dGVDetPagos.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Empty;
                    }
                }

                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    SqlTransaction tran;
                    if (con.ConectarSQL())
                    {
                        tran = con.SC.BeginTransaction();
                        try
                        {
                            string delete = "DELETE FROM P_DOCTOS_PP_DET WHERE DOCTO_PP_DET_ID = @DET";
                            SqlCommand sc = new SqlCommand(delete, con.SC, tran);
                            sc.Parameters.Add("@DET", SqlDbType.Int).Value = Convert.ToInt32(Convert.ToString(dGVDetPagos["DOCTO_PR_DET_ID", dGVDetPagos.CurrentCell.RowIndex].Value));
                            sc.ExecuteNonQuery();
                            sc.Dispose();

                            /*delete = "DELETE FROM P_AUT_DOCTOS_PR WHERE DOCTO_PP_DET_ID = @DET";
                            sc = new SqlCommand(delete, con.SC, tran);
                            sc.Parameters.Add("@DET", SqlDbType.Int).Value = Convert.ToInt32(Convert.ToString(dGVDetPagos["DOCTO_PR_DET_ID", renglon].Value));
                            sc.ExecuteNonQuery();*/

                            tran.Commit();
                            Cargar();
                            eliminarPagos = true;
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Ocurrio un error inesperado al borrar el cargo " + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tran.Rollback();
                        }
                        con.Desconectar();
                    }
                }
            }
        }

        private void dGVDetPagos_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    renglonProg = e.RowIndex;

                    dGVDetPagos.CurrentCell = dGVDetPagos.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dGVDetPagos.Rows[e.RowIndex].Selected = true;
                    dGVDetPagos.Focus();
                }
            }
        }
    }
}
