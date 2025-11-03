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

using FirebirdSql.Data.FirebirdClient;

namespace PagosIntermex
{
    public partial class F_DETALLEPROGRAMACIONPAGOS : Form
    {

        public C_USUARIOS usuarioLogueado;
        public F_DETALLEPROGRAMACIONPAGOS()
        {
            InitializeComponent();           
        }
        int renglon = 0;
        public string FOLIO { set; get; }

        public string MOVIMIENTO { set; get; }


        public string ESTATUS { set; get; }

        public C_EMPRESAS[] EMPRESA { set; get; }
        public string FECHA_PAGO { get; set; }
        public int DOCTO_PR_ID { get; set; }
        public string NIVEL_DATAGRID { get; internal set; }

        private bool eliminarPagos = false;
        public string Ver  { set ;get; }

        C_AUT_PAGOS[] pagos;

        bool NO_ELIMINABLE = false;


        private const int NIVEL_FINAL = 0;  // Nivel más alto de autorización
        private const int NIVEL_INICIAL = 5; // Nivel más bajo (ejemplo)

        // Método auxiliar para validar niveles
        private bool EsNivelFinal(int nivel)
        {
            return nivel == NIVEL_FINAL;
        }

        private bool TieneSiguienteNivel(int nivel)
        {
            return nivel > NIVEL_FINAL;
        }


        private void Cargar()
        {
            C_AUT_PAGOS f = new C_AUT_PAGOS();
            pagos = f.GET_DETALLES(FOLIO,Convert.ToInt32(NIVEL_DATAGRID));

            // CORRECCIÓN: Validar antes de restar
            int nivelUsuarios = Convert.ToInt32(NIVEL_DATAGRID);
            if (TieneSiguienteNivel(nivelUsuarios))
            {
                f.SET_USERS(nivelUsuarios - 1, FOLIO, dgvUser);
            }
            else
            {
                // Si es nivel final (0), no hay usuarios que cargar
                dgvUser.Visible = false;
                label3.Visible = false;
            }
            C_USUARIOS u = new C_USUARIOS();
            //USUARIO_ID = u.GET_USUARIO_ID(USUARIO);
            string msg_local = "";
            string _ver = "";
            if (Ver == "S")
                _ver = "S";
            DataTable _datosDetalle= f.DatosDetalle(FOLIO, Convert.ToInt32(NIVEL_DATAGRID), _ver,out msg_local);

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
                    this.button1.Visible = true;
                    pAutorizarPagos.Visible = true;
                }
                else if (MOVIMIENTO == "M")
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

                        if(usuarioLogueado.Requisitante == "S")
                        {
                            dgvUser.Visible = false;
                            label3.Visible = false;
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

                _datos = PP.DetallePagos(FOLIO, usuarioLogueado,Convert.ToInt32(NIVEL_DATAGRID),_ver, out msg_local,"N");
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

                    dGVDetPagos["FOLIO_P", i].Value = Convert.ToString(_fila["FOLIO_MICROSIP"]);
                    dGVDetPagos["FOLIO_P", i].ReadOnly = true;

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
                        if (f.DetalleAutorizado(docto_pr_det, Convert.ToInt32(NIVEL_DATAGRID)))
                        {
                            dGVDetPagos["Autorizar", i].Value = true;

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

                if (dGVDetPagos.Rows.Count > 0)
                    MostrarUsuarios(0);
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

        private bool ValidaPagoCanceladoMicrosip(string pago, int proveedor_id)
        {
            C_CONEXIONFIREBIRD conn = new C_CONEXIONFIREBIRD();
            FbCommand cmd;
            FbDataReader read;

            bool result = false;

            string select = "";
            string cancelado = "";

            if (conn.ConectarFB(EMPRESA[0].NOMBRE_CORTO))
            {
                try
                {
                    select = "SELECT * FROM DOCTOS_CP ";
                    select += "WHERE FOLIO = '" + pago + "' ";
                    select += "  AND PROVEEDOR_ID = " + proveedor_id + " ";
                    select += "  AND NATURALEZA_CONCEPTO = 'R' ";

                    cmd = new FbCommand(select, conn.FBC);
                    read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        cancelado = read["CANCELADO"].ToString();
                    }
                    read.Close();
                    cmd.Dispose();

                    if (cancelado == "S")
                    {
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubo un error al validar el pago '" + pago + "' en Microsip.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                conn.Desconectar();
            }

            return result;
        }





        private void F_DETALLEPROGRAMACIONPAGOS_Load(object sender, EventArgs e)
        {
            // VALIDACIÓN CRÍTICA: Verificar coherencia de niveles
            int nivelDataGrid = Convert.ToInt32(NIVEL_DATAGRID);
            int nivelUsuario = usuarioLogueado.NIVEL;

            if (nivelDataGrid < NIVEL_FINAL)
            {
                MessageBox.Show("El nivel de autorización es inválido.",
                               "Error de configuración",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                this.Close();
                return;
            }
            
                
                if (nivelUsuario < nivelDataGrid)
                {
                    MessageBox.Show("No tiene permisos para autorizar en este nivel.\n" +
                                   $"Su nivel: {nivelUsuario}\n" +
                                   $"Nivel requerido: {nivelDataGrid}",
                                   "Acceso denegado",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }
            


            dGVDetPagos.DoubleBuffered(true);
            Cargar();
            WindowState = FormWindowState.Maximized;
            txtUser.Text = usuarioLogueado.Usuario;


        }

        private void dGVDetPagos_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dGVDetPagos.CurrentCell.ColumnIndex == dGVDetPagos.Columns["IMPORTE_AUT"].Index)
            {
                TextBox txt = e.Control as TextBox;

                if (txt != null)
                {
                    txt.KeyPress -= new KeyPressEventHandler(dGVDetPagos_KeyPress);
                    txt.KeyPress += new KeyPressEventHandler(dGVDetPagos_KeyPress);
                }               
            }
            
        }

        private void dGVDetPagos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dGVDetPagos.CurrentCell.ColumnIndex ==dGVDetPagos.Columns["IMPORTE_AUT"].Index)
            {
                if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
                {
                    e.Handled = true;
                    return;
                }

                // revisar que solo haya un punto permitido //checks to make sure only 1 decimal is allowed
                if (e.KeyChar == 46)
                {
                    if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                        e.Handled = true;
                }
            }
        }





        // Autorizar pagos
        private void button1_Click2(object sender, EventArgs e)
        {
            // Validar que no tenga Ceros
            bool _Ceros = false;

            double _ImporteTotal = 0.0;

            int _seleccionado = 0;
            int _seleccionado2 = 0;

            DialogResult _resp = DialogResult.No;
            List<List<string>> _Seleccionados = new List<List<string>>();
            List<string[,]> _usuarios = new List<string[,]>();

            #region PRIMERO VALIDAMOS QUE AL MENOS HAYA SELECCIONADO ALGUN PAGO

            for (int i = 0; i < dGVDetPagos.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dGVDetPagos["Autorizar", i].Value))
                {
                    double importe = 0;
                    string aux = Convert.ToString(dGVDetPagos["IMPORTE_AUT", i].Value);

                    List<string> _seleccion = new List<string>();

                    if (aux.Length == 0)
                    {
                        aux = "0.0";
                    }

                    importe = Convert.ToDouble(aux);

                    if (importe <= 0)
                    {
                        _Ceros = true;
                    }

                    _ImporteTotal += importe;

                    _seleccion.Add(Convert.ToString(dGVDetPagos["DOCTO_PR_DET_ID", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["FOLIO_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["FECHA_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["PROVEEDOR_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["FECHA_VEN", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["IMPORTE_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["IMPORTE_AUT", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["ESTATUS_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["P_EMPRESA", i].Value));

                    _seleccionado++;

                    _Seleccionados.Add(_seleccion);
                }
            }

            #endregion

            #region DESPÚES VALIDAMOS QUE AL MENOS HAYA SELECCIONADO ALGUN USUARIO

            /* for (int i = 0; i < dgvUser.Rows.Count; i++)
             {
                 if (Convert.ToBoolean(dgvUser["AUTORIZADO_AUT", i].Value))
                 {
                     string[,] _usuario = new string [,] { { Convert.ToString(dgvUser["USUARIO_ID", i].Value) },{ Convert.ToString(dgvUser["NIVEL", i].Value) } };
                     _usuarios.Add(_usuario);
                     _seleccionado2++;
                 }
             }*/

            #region VALIDACIÓN MEJORADA DE USUARIOS

            int nivelActual = Convert.ToInt32(NIVEL_DATAGRID);
            bool requiereUsuariosSeleccionados = TieneSiguienteNivel(nivelActual);

            // Validar usuarios seleccionados solo si NO es el nivel final
            if (requiereUsuariosSeleccionados)
            {
                for (int i = 0; i < dgvUser.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvUser["AUTORIZADO_AUT", i].Value))
                    {
                        string[,] _usuario = new string[,] {
                    { Convert.ToString(dgvUser["USUARIO_ID", i].Value) },
                    { Convert.ToString(dgvUser["NIVEL", i].Value) }
                };
                        _usuarios.Add(_usuario);
                        _seleccionado2++;
                    }
                }

                // Solo validar si requiere usuarios
                if (_seleccionado2 == 0)
                {
                    MessageBox.Show("Debe seleccionar al menos un usuario del siguiente nivel.",
                                  "Mensaje de pagos",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }
            }

            #endregion

            #endregion
            if (_seleccionado > 0)
            {
              //  if (_seleccionado2 > 0 || Convert.ToInt32(NIVEL_DATAGRID)==1)
                {
                    if (usuarioLogueado.U_ROL == "A")
                    {
                        if (MessageBox.Show("¿Desea autorizar los pagos seleccionados?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                            C_ConexionSQL conexion = new C_ConexionSQL();
                            SqlTransaction transaction;
                            SqlCommand cmd;

                            string msg_local = "";
                            string consulta = "";

                            if (registros.LeerRegistros(false))
                            {
                                if (conexion.ConectarSQL())
                                {
                                    transaction = conexion.SC.BeginTransaction();

                                    try
                                    {
                                        if (_Ceros == true)
                                        {
                                            _resp = MessageBox.Show("Importe total autorizado \"" + _ImporteTotal.ToString("C2") + "\"\n\rEl monto autorizado de uno o mas " +
                                               "pagos es cero\n\rFavor de agregar un monto correcto", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Question);
                                        }

                                        if (_Ceros == false || _resp == DialogResult.Yes)
                                        {
                                            #region ACTUALIZA TABLA DE AUTORIZACIONES
                                            if (usuarioLogueado.U_ROL == "A" && usuarioLogueado.Usuario_id != 0)
                                            {

                                                foreach (List<string> _Pag_selec in _Seleccionados)
                                                {
                                                    double _montoAutorizado = 0.0;
                                                    _montoAutorizado = Convert.ToDouble(_Pag_selec[6]);
                                                    consulta = " UPDATE P_AUT_DOCTOS_PR SET ";
                                                    consulta += " MONTO_AUTORIZADO =" + _montoAutorizado;
                                                    consulta += " ,ESTATUS ='A'";
                                                    consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                    consulta += " AND USUARIO_ID = " + usuarioLogueado.Usuario_id;//Agregar monto Acrtualizar el nivel del encabezado a -1 y agregar nuevo registro con nivel -1 topar nivel a .1
                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    if (cmd.ExecuteNonQuery() == 1)
                                                    {
                                                        //Insertar nuevo registro con usuarios de nivel superiror 
                                                        foreach (string[,] _usuario in _usuarios)
                                                        {
                                                            //Buscar si exite el usuario para la programacion indicada
                                                            consulta = "select * from P_AUT_DOCTOS_PR where DOCTO_PR_DET_ID=" + _Pag_selec[0] +
                                                                " and USUARIO_ID =" + _usuario[0, 0] + " AND NIVEL=" + _usuario[1, 0];

                                                            SqlDataAdapter _da = new SqlDataAdapter(consulta, conexion.SC);
                                                            _da.SelectCommand.Transaction = transaction;
                                                            DataTable _existeUsuario = new DataTable();
                                                            _da.Fill(_existeUsuario);
                                                            if (_existeUsuario.Rows.Count == 0)
                                                            {
                                                                //Sino existe insertar el nuevo usuario
                                                                consulta = " INSERT INTO P_AUT_DOCTOS_PR " +
                                                                " (DOCTO_PR_DET_ID " +
                                                                " , USUARIO_ID " +
                                                                " , ESTATUS " +
                                                                " , MONTO_AUTORIZADO" +
                                                                " , NIVEL) " +
                                                                " VALUES " +
                                                                " (" + _Pag_selec[0] +
                                                                " , " + _usuario[0, 0] +
                                                                " , 'P' " +
                                                                " ,NULL" +
                                                                " ," + _usuario[1, 0] + ")";

                                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                if (cmd.ExecuteNonQuery() == 0)
                                                                {
                                                                    transaction.Rollback();
                                                                    cmd.Dispose();
                                                                    conexion.Desconectar();
                                                                    MessageBox.Show("No se pudo completar la operación favor de reintentar", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                                    return;
                                                                }
                                                            }
                                                            _da.Dispose();
                                                        }
                                                    }
                                                    cmd.Dispose();
                                                }

                                            }
                                            bool actualizarEncabezado = false;

                                            List<int> DetallesAprobados = new List<int>();
                                            /*
                                            foreach (List<string> _Pag_selec in _Seleccionados)
                                            {
                                                //consulta para saber si ya se autorizo aunque sea un pago en especifico
                                                consulta = "select TOTAL - AUTORIZADOS DIF  FROM ";
                                                consulta += " (select COUNT(*) TOTAL, (select COUNT(*) from P_AUT_DOCTOS_PR p  ";
                                                consulta += " where p.DOCTO_PR_DET_ID = " + _Pag_selec[0] + " and p.ESTATUS = 'A' and NIVEL= " + Convert.ToInt32(NIVEL_DATAGRID) + ") AUTORIZADOS ";
                                                consulta += " from P_AUT_DOCTOS_PR ";
                                                consulta += " where DOCTO_PR_DET_ID =  " + _Pag_selec[0] + " and NIVEL= " + Convert.ToInt32(NIVEL_DATAGRID) + ") as CP ";
                                                consulta += " group by CP.TOTAL, CP.AUTORIZADOS ";

                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                SqlDataReader sdr = cmd.ExecuteReader();
                                                while (sdr.Read())
                                                {
                                                    int _dif = Convert.ToInt32(Convert.ToString(sdr["DIF"]));
                                                    if (_dif == 0)
                                                    {
                                                        actualizarEncabezado = true;
                                                        DetallesAprobados.Add(Convert.ToInt32(_Pag_selec[0]));
                                                        consulta = " UPDATE P_AUT_DOCTOS_PR SET ";
                                                        consulta += " ESTATUS = 'A'";
                                                        consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                        consulta += " AND USUARIO_ID = " + usuarioLogueado.Usuario_id;
                                                        cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                        if (cmd.ExecuteNonQuery() > 0 && Convert.ToInt32(NIVEL_DATAGRID) > 1)
                                                            foreach (string[,] _usuario in _usuarios)
                                                            {
                                                                consulta = " UPDATE P_AUT_DOCTOS_PR SET ";
                                                                consulta += " ESTATUS = 'C'";
                                                                consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                                consulta += " AND USUARIO_ID = " + _usuario[0, 0];
                                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                if (cmd.ExecuteNonQuery() > 0)
                                                                {
                                                                    consulta = " UPDATE P_DOCTOS_PR SET ";
                                                                    consulta += " NIVEL = " + (Convert.ToInt32(NIVEL_DATAGRID) - 1);
                                                                    consulta += " WHERE FOLIO = '" + FOLIO + "'";
                                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }
                                                    }
                                                }
                                                cmd.Dispose();
                                                sdr.Close();
                                            }
                                            */

                                            foreach (List<string> _Pag_selec in _Seleccionados)
                                            {
                                                consulta = "SELECT TOTAL - AUTORIZADOS AS DIF FROM ";
                                                consulta += " (SELECT COUNT(*) TOTAL, ";
                                                consulta += " (SELECT COUNT(*) FROM P_AUT_DOCTOS_PR p ";
                                                consulta += " WHERE p.DOCTO_PR_DET_ID = " + _Pag_selec[0];
                                                consulta += " AND p.ESTATUS = 'A' AND NIVEL = " + NIVEL_DATAGRID + ") AUTORIZADOS ";
                                                consulta += " FROM P_AUT_DOCTOS_PR ";
                                                consulta += " WHERE DOCTO_PR_DET_ID = " + _Pag_selec[0];
                                                consulta += " AND NIVEL = " + NIVEL_DATAGRID + ") AS CP ";
                                                consulta += " GROUP BY CP.TOTAL, CP.AUTORIZADOS";

                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                SqlDataReader sdr = cmd.ExecuteReader();

                                                while (sdr.Read())
                                                {
                                                    int _dif = Convert.ToInt32(Convert.ToString(sdr["DIF"]));
                                                    if (_dif == 0)
                                                    {
                                                        actualizarEncabezado = true;
                                                        DetallesAprobados.Add(Convert.ToInt32(_Pag_selec[0]));

                                                        // CORRECCIÓN: Manejar correctamente el siguiente nivel
                                                        if (TieneSiguienteNivel(Convert.ToInt32(NIVEL_DATAGRID)))
                                                        {
                                                            // Si hay siguiente nivel, crear registros para usuarios del nivel anterior
                                                            foreach (string[,] _usuario in _usuarios)
                                                            {
                                                                consulta = " UPDATE P_AUT_DOCTOS_PR SET ";
                                                                consulta += " ESTATUS = 'C'";
                                                                consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                                consulta += " AND USUARIO_ID = " + _usuario[0, 0];

                                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                if (cmd.ExecuteNonQuery() > 0)
                                                                {
                                                                    consulta = " UPDATE P_DOCTOS_PR SET ";
                                                                    consulta += " NIVEL = " + (Convert.ToInt32(NIVEL_DATAGRID) - 1);
                                                                    consulta += " WHERE FOLIO = '" + FOLIO + "'";
                                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // Es el nivel final, marcar como completamente autorizado
                                                            consulta = " UPDATE P_DOCTOS_PR SET ";
                                                            consulta += " ESTATUS_PROC = 'A', ";
                                                            consulta += " NIVEL = " + NIVEL_FINAL;
                                                            consulta += " WHERE FOLIO = '" + FOLIO + "'";
                                                            cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                            cmd.ExecuteNonQuery();
                                                        }
                                                    }
                                                }
                                                cmd.Dispose();
                                                sdr.Close();
                                            }
                                            #endregion

                                            ///se vienen lineas de cambios desde C_aut_pagos f hasta el dispose
                                            /*C_AUT_PAGOS f = new C_AUT_PAGOS();
                                            if (actualizarEncabezado && Convert.ToInt32(NIVEL_DATAGRID) == 1)
                                            {
                                                #region ACTUALIZA ENCABEZADO
                                                _ImporteTotal = 0.0;
                                                foreach (List<string> _Pag_selec in _Seleccionados)
                                                {
                                                    _ImporteTotal += f.ObtenerMenorMontoAutorizado(FOLIO, Convert.ToInt32(_Pag_selec[0]), conexion, transaction, out msg_local);
                                                    if (msg_local.Length > 0)
                                                    {
                                                        transaction.Rollback();
                                                        conexion.Desconectar();
                                                        MessageBox.Show("Error al intentar obtener los montos autorizados", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        return;
                                                    }
                                                }

                                                consulta = "UPDATE P_DOCTOS_PR SET ";
                                                if (usuarioLogueado.U_ROL == "A")
                                                {
                                                    consulta += "       IMPORTE_AUTORIZADO = " + _ImporteTotal + ", ";
                                                    consulta += "       ESTATUS_PROC = 'A', ";
                                                }
                                                else
                                                {
                                                    consulta += "       IMPORTE_CAPTURISTA = " + _ImporteTotal + ", ";
                                                    consulta += "       ESTATUS_PROC = 'P', ";
                                                }
                                                consulta += "       USUARIO_AUTORIZO = '" + usuarioLogueado.Usuario + "', " +
                                                         // "       FECHA_HORA_AUTORIZO = '" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "' " +
                                                         "       FECHA_HORA_AUTORIZO = @FechaAutorizo " +
                                                         " WHERE FOLIO = '" + FOLIO + "' ";
                                                //    "   AND EMPRESA = '" + EMPRESA + "'";
                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                cmd.Parameters.Add("@FechaAutorizo", SqlDbType.DateTime).Value = DateTime.Now;
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                                */
                                            C_AUT_PAGOS f = new C_AUT_PAGOS();
                                            if (actualizarEncabezado)
                                            {
                                                if (EsNivelFinal(Convert.ToInt32(NIVEL_DATAGRID)))
                                                {
                                                    // Lógica para nivel final (autorización completa)
                                                    _ImporteTotal = 0.0;
                                                    foreach (List<string> _Pag_selec in _Seleccionados)
                                                    {
                                                        _ImporteTotal += f.ObtenerMenorMontoAutorizado(FOLIO,
                                                                                                       Convert.ToInt32(_Pag_selec[0]),
                                                                                                       conexion,
                                                                                                       transaction,
                                                                                                       out msg_local);
                                                    }

                                                    // Actualizar a estado final autorizado
                                                    consulta = "UPDATE P_DOCTOS_PR SET ";
                                                    consulta += "IMPORTE_AUTORIZADO = " + _ImporteTotal + ", ";
                                                    consulta += "ESTATUS_PROC = 'A', ";
                                                    consulta += "USUARIO_AUTORIZO = '" + usuarioLogueado.Usuario + "', ";
                                                    consulta += "FECHA_HORA_AUTORIZO = @FechaAutorizo ";
                                                    consulta += "WHERE FOLIO = '" + FOLIO + "'";

                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    cmd.Parameters.Add("@FechaAutorizo", SqlDbType.DateTime).Value = DateTime.Now;
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();

                                                    //actualizamos el estatus del encabezado de P_DOCTOS_PP a 'A'
                                                    consulta = "UPDATE P_DOCTOS_PP SET" +
                                                    "       ESTATUS_PROC = 'A'" +
                                                    /*"       USUARIO_AUTORIZO = '" + usuarioLogueado.Usuario + "', " +
                                                    "       FECHA_HORA_AUTORIZO = '" + DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + "' " +*/
                                                    " WHERE DOCTO_PP_ID in (select DPP.DOCTO_PP_ID from P_DOCTOS_PP as DPP " +
                                                    " inner join P_DOCTOS_PP_DET as DPPD on DPP.DOCTO_PP_ID = DPPD.DOCTO_PP_ID " +
                                                    " inner join P_DOCTOS_PR_DET as DPRD on DPPD.FOLIO_MICROSIP = DPRD.FOLIO_MICROSIP " +
                                                    " inner join P_DOCTOS_PR as DPR on DPRD.DOCTO_PR_ID = DPR.DOCTO_PR_ID " +
                                                    " where " +
                                                    " DPR.FOLIO = '" + FOLIO + "' " +
                                                    " group by DPP.DOCTO_PP_ID)";
                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();

                                                    // Actualizar estatus de  P_DOCTOS_PR_DET y P_DOCTOS_PP_DET que no fueron autorizadas a estatus "X"
                                                    string _idsSeleccionados = "";
                                                    foreach (List<string> _Pag_selec in _Seleccionados)
                                                    {
                                                        //Obtener el folio para validar si es una requisicion
                                                        // De no tenerlo en el arreglo hacer un método para buscarlo
                                                        string _folioReq = _Pag_selec[1];
                                                        if (_folioReq.Contains("REQ"))
                                                        {
                                                            DataTable _requisicion = new DataTable();
                                                            _requisicion = f.DatosRequisicion(Convert.ToInt32(_Pag_selec[0]), out msg_local);
                                                            if (_requisicion.Rows.Count > 0)
                                                            {
                                                                //Cambiar el estatus de la requisicion a "S" ->surtido
                                                                string _reqID = Convert.ToString(_requisicion.Rows[0]["REQUISICION_ID"]); //Obtener el id de la requisicion
                                                                if (_reqID.Length > 0)
                                                                {
                                                                    consulta = "UPDATE REQ_ENC " +
                                                                      " SET Estatus_general = 'S'" +
                                                                      " WHERE Requisicion_id = " + _reqID;
                                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                    cmd.ExecuteNonQuery();
                                                                    cmd.Dispose();
                                                                }

                                                            }
                                                        }
                                                        _idsSeleccionados += _Pag_selec[0] + ",";
                                                    }
                                                    /*Actualizar DOCTOS_PR_DET de estatus "C" o "P" a "X"   */
                                                    _idsSeleccionados = _idsSeleccionados.Substring(0, _idsSeleccionados.Length - 1);
                                                    consulta = "update P_DOCTOS_PR_DET " +
                                                        " set ESTATUS = 'X' " +
                                                        " where DOCTO_PR_ID = (" +
                                                        "  select DPRD.DOCTO_PR_ID from P_DOCTOS_PR as PDPR " +
                                                        " INNER JOIN P_DOCTOS_PR_DET AS DPRD on PDPR.DOCTO_PR_ID = DPRD.DOCTO_PR_ID " +
                                                        " where " +
                                                        " PDPR.FOLIO = '" + FOLIO + "' and DPRD.DOCTO_PR_DET_ID NOT IN (" + _idsSeleccionados + ") group by DPRD.DOCTO_PR_ID)";

                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    if (cmd.ExecuteNonQuery() > 0)
                                                    {
                                                    }
                                                    /*else
                                                    {
                                                        cmd.Dispose();
                                                        transaction.Rollback();
                                                        conexion.Desconectar();
                                                        MessageBox.Show("Error al intentar actualizar el estatus de la tabla P_DOCTOS_PR_DET", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        return;
                                                    }*/
                                                    /*Actualizar DOCTOS_PP_DET de estatus "T" a "X"   */
                                                    consulta = "update P_DOCTOS_PP_DET " +
                                                        " set ESTATUS = 'X' " +
                                                        " where DOCTO_PP_DET_ID in " +
                                                        " (select DPPD.DOCTO_PP_DET_ID from P_DOCTOS_PP_DET AS DPPD " +
                                                        " INNER JOIN P_DOCTOS_PR_DET AS PDPD ON PDPD.FOLIO_MICROSIP = DPPD.FOLIO_MICROSIP and PDPD.DOCTO_PP_DET_ID = DPPD.DOCTO_PP_DET_ID  " +
                                                        " INNER JOIN P_DOCTOS_PR as PDPR on PDPD.DOCTO_PR_ID = PDPR.DOCTO_PR_ID " +
                                                        " where " +
                                                        " PDPR.FOLIO = '" + FOLIO + "' and PDPD.ESTATUS = 'X')";

                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    if (cmd.ExecuteNonQuery() == 0)
                                                    {
                                                        /* cmd.Dispose();
                                                         transaction.Rollback();
                                                         conexion.Desconectar();
                                                         MessageBox.Show("Error al intentar actualizar el estatus de la tabla P_DOCTOS_PP_DET", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                         return;*/
                                                    }

                                                    cmd.Dispose();
                                                }
                                                #region ACTUALIZA DETALLE



                                                foreach (List<string> _Pag_selec in _Seleccionados)
                                                {
                                                    string aux = //Convert.ToString(_Pag_selec[6]).Replace(",", "");
                                                    aux = Convert.ToString(f.ObtenerMenorMontoAutorizado(FOLIO, Convert.ToInt32(_Pag_selec[0]), conexion, transaction, out msg_local));
                                                    C_AUT_PAGOS _f = new C_AUT_PAGOS();
                                                    if (DetallesAprobados.Count > 0)
                                                    {
                                                        for (int i = 0; i < DetallesAprobados.Count; i++)
                                                        {
                                                            if (DetallesAprobados[i] == Convert.ToInt32(_Pag_selec[0]))
                                                            {
                                                                consulta = "UPDATE P_DOCTOS_PR_DET SET ";
                                                                if (usuarioLogueado.U_ROL == "A")
                                                                {
                                                                    consulta += "       IMPORTE_AUTORIZADO = " + aux + ", ";
                                                                    consulta += "       ESTATUS = 'A' ";

                                                                }
                                                                else
                                                                {
                                                                    consulta += "       IMPORTE_CAPTURISTA = " + aux + ", ";
                                                                    consulta += "       ESTATUS = 'C' ";
                                                                }
                                                                consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                cmd.ExecuteNonQuery();
                                                                /*if (cmd.ExecuteNonQuery() > 0)
                                                                    foreach (string[,] _usuario in _usuarios)
                                                                        _f.InsertarUsuarioAutorizarDetalle(Convert.ToInt32(_Pag_selec[0]),Convert.ToInt32(_usuario[0,0]),conexion,transaction,out msg_local);*/
                                                                cmd.Dispose();
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                consulta = "UPDATE P_DOCTOS_PR_DET SET ";
                                                                if (usuarioLogueado.U_ROL == "A")
                                                                {
                                                                    consulta += "       IMPORTE_AUTORIZADO = " + aux + ", ";
                                                                    consulta += "       ESTATUS = 'P' ";

                                                                }
                                                                else
                                                                {
                                                                    consulta += "       IMPORTE_CAPTURISTA = " + aux + ", ";
                                                                    consulta += "       ESTATUS = 'C' ";
                                                                }
                                                                consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                cmd.ExecuteNonQuery();
                                                                cmd.Dispose();
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        consulta = "UPDATE P_DOCTOS_PR_DET SET ";
                                                        if (usuarioLogueado.U_ROL == "A")
                                                        {
                                                            consulta += "       IMPORTE_AUTORIZADO = " + aux + ", ";

                                                            consulta += "       ESTATUS = 'P' ";

                                                        }
                                                        else
                                                        {
                                                            consulta += "       IMPORTE_CAPTURISTA = " + aux + ", ";
                                                            consulta += "       ESTATUS = 'C' ";
                                                        }
                                                        consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                        cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();
                                                    }






                                                    /*consulta = "UPDATE P_DOCTOS_PR_DET SET ";
                                                    if (usuarioLogueado.U_ROL == "A")
                                                    {
                                                        consulta += "       IMPORTE_AUTORIZADO = " + aux + ", ";

                                                        if (actualizarEncabezado)
                                                        {
                                                            consulta += "       ESTATUS = 'A' ";
                                                        }
                                                        else
                                                        {
                                                            consulta += "       ESTATUS = 'P' ";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        consulta += "       IMPORTE_CAPTURISTA = " + aux + ", ";
                                                        consulta += "       ESTATUS = 'C' ";
                                                    }
                                                    consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();*/
                                                }
                                                #endregion




                                                transaction.Commit();
                                                conexion.Desconectar();

                                                Close();
                                            }

                                            Cargar();
                                        }
                                    }
                                    catch (Exception Ex)
                                    {
                                        transaction.Rollback();
                                        msg_local = Ex.Message;
                                    }

                                    conexion.Desconectar();
                                }
                            }

                            if (msg_local.Length > 0)
                            {
                                MessageBox.Show(msg_local, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("¿Desea solicitar la peticion de los pagos seleccionados?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                            C_ConexionSQL conexion = new C_ConexionSQL();
                            SqlTransaction transaction;
                            SqlCommand cmd;

                            string msg_local = "";
                            string consulta = "";

                            if (registros.LeerRegistros(false))
                            {
                                if (conexion.ConectarSQL())
                                {
                                    transaction = conexion.SC.BeginTransaction();

                                    try
                                    {
                                        if (_Ceros == true)
                                        {
                                            _resp = MessageBox.Show("Importe total autorizado \"" + _ImporteTotal.ToString("C2") + "\"\n\rEl monto autorizado de uno o mas " +
                                               "pagos es cero\n\r¿Desea Continuar?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        }

                                        if (_Ceros == false || _resp == DialogResult.Yes)
                                        {

                                            #region ACTUALIZA ENCABEZADO
                                            consulta = "UPDATE P_DOCTOS_PP SET ";

                                            consulta += "       IMPORTE_AUTORIZADO = " + _ImporteTotal + ", ";
                                            consulta += "       ESTATUS_PROC = 'T' ";

                                            // consulta += "       USUARIO_AUTORIZO = '" + USUARIO + "' " +
                                            // "       FECHA_HORA_AUTORIZO = '" + DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + "' " +
                                            consulta += " WHERE FOLIO = '" + FOLIO + "' ";
                                            cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                            #endregion

                                            #region ACTUALIZA DETALLE
                                            foreach (List<string> _Pag_selec in _Seleccionados)
                                            {
                                                string aux = Convert.ToString(_Pag_selec[6]).Replace(",", "");


                                                consulta = "UPDATE P_DOCTOS_PP_DET SET ";

                                                consulta += "       IMPORTE_AUTORIZADO = " + aux + ", ";
                                                consulta += "       ESTATUS = 'A' ";


                                                consulta += " WHERE DOCTO_PP_DET_ID = '" + _Pag_selec[0] + "'";
                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                                break;



                                            }


                                            #endregion

                                            transaction.Commit();
                                            conexion.Desconectar();

                                            //Close();
                                        }

                                        Cargar();
                                        Close();
                                    }
                                    catch (Exception Ex)
                                    {
                                        transaction.Rollback();
                                        msg_local = Ex.Message;
                                    }

                                    conexion.Desconectar();
                                }
                            }

                            if (msg_local.Length > 0)
                            {
                                MessageBox.Show(msg_local, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                    }
                }
              //  else                    MessageBox.Show("Ningún usuario ha sido seleccionado.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Ningún pago ha sido seleccionado.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Validar que no tenga Ceros
            bool _Ceros = false;

            double _ImporteTotal = 0.0;

            int _seleccionado = 0;
            int _seleccionado2 = 0;

            DialogResult _resp = DialogResult.No;
            List<List<string>> _Seleccionados = new List<List<string>>();
            List<string[,]> _usuarios = new List<string[,]>();

            #region PRIMERO VALIDAMOS QUE AL MENOS HAYA SELECCIONADO ALGUN PAGO

            for (int i = 0; i < dGVDetPagos.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dGVDetPagos["Autorizar", i].Value))
                {
                    double importe = 0;
                    string aux = Convert.ToString(dGVDetPagos["IMPORTE_AUT", i].Value);

                    List<string> _seleccion = new List<string>();

                    if (aux.Length == 0)
                    {
                        aux = "0.0";
                    }

                    importe = Convert.ToDouble(aux);

                    if (importe <= 0)
                    {
                        _Ceros = true;
                    }

                    _ImporteTotal += importe;

                    _seleccion.Add(Convert.ToString(dGVDetPagos["DOCTO_PR_DET_ID", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["FOLIO_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["FECHA_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["PROVEEDOR_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["FECHA_VEN", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["IMPORTE_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["IMPORTE_AUT", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["ESTATUS_P", i].Value));
                    _seleccion.Add(Convert.ToString(dGVDetPagos["P_EMPRESA", i].Value));

                    _seleccionado++;

                    _Seleccionados.Add(_seleccion);
                }
            }

            #endregion

            #region VALIDACIÓN MEJORADA DE USUARIOS

            int nivelActual = Convert.ToInt32(NIVEL_DATAGRID);
            bool requiereUsuariosSeleccionados = TieneSiguienteNivel(nivelActual);

            // Validar usuarios seleccionados solo si NO es el nivel final
            if (requiereUsuariosSeleccionados)
            {
                for (int i = 0; i < dgvUser.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvUser["AUTORIZADO_AUT", i].Value))
                    {
                        string[,] _usuario = new string[,] {
                    { Convert.ToString(dgvUser["USUARIO_ID", i].Value) },
                    { Convert.ToString(dgvUser["NIVEL", i].Value) }
                };
                        _usuarios.Add(_usuario);
                        _seleccionado2++;
                    }
                }

                // Solo validar si requiere usuarios
                if (_seleccionado2 == 0)
                {
                    MessageBox.Show("Debe seleccionar al menos un usuario del siguiente nivel.",
                                  "Mensaje de pagos",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }
            }

            #endregion

            if (_seleccionado > 0)
            {
                if (usuarioLogueado.U_ROL == "A")
                {
                    if (MessageBox.Show("¿Desea autorizar los pagos seleccionados?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                        C_ConexionSQL conexion = new C_ConexionSQL();
                        SqlTransaction transaction;
                        SqlCommand cmd;

                        string msg_local = "";
                        string consulta = "";

                        if (registros.LeerRegistros(false))
                        {
                            if (conexion.ConectarSQL())
                            {
                                transaction = conexion.SC.BeginTransaction();

                                try
                                {
                                    if (_Ceros == true)
                                    {
                                        _resp = MessageBox.Show("Importe total autorizado \"" + _ImporteTotal.ToString("C2") + "\"\n\rEl monto autorizado de uno o mas " +
                                           "pagos es cero\n\rFavor de agregar un monto correcto", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Question);
                                    }

                                    if (_Ceros == false || _resp == DialogResult.Yes)
                                    {
                                        #region ACTUALIZA TABLA DE AUTORIZACIONES
                                        if (usuarioLogueado.U_ROL == "A" && usuarioLogueado.Usuario_id != 0)
                                        {

                                            foreach (List<string> _Pag_selec in _Seleccionados)
                                            {
                                                double _montoAutorizado = 0.0;
                                                _montoAutorizado = Convert.ToDouble(_Pag_selec[6]);
                                                consulta = " UPDATE P_AUT_DOCTOS_PR SET ";
                                                consulta += " MONTO_AUTORIZADO =" + _montoAutorizado;
                                                consulta += " ,ESTATUS ='A'";
                                                consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                consulta += " AND USUARIO_ID = " + usuarioLogueado.Usuario_id;
                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                if (cmd.ExecuteNonQuery() == 1)
                                                {
                                                    //Insertar nuevo registro con usuarios de nivel superior 
                                                    foreach (string[,] _usuario in _usuarios)
                                                    {
                                                        //Buscar si existe el usuario para la programacion indicada
                                                        consulta = "select * from P_AUT_DOCTOS_PR where DOCTO_PR_DET_ID=" + _Pag_selec[0] +
                                                            " and USUARIO_ID =" + _usuario[0, 0] + " AND NIVEL=" + _usuario[1, 0];

                                                        SqlDataAdapter _da = new SqlDataAdapter(consulta, conexion.SC);
                                                        _da.SelectCommand.Transaction = transaction;
                                                        DataTable _existeUsuario = new DataTable();
                                                        _da.Fill(_existeUsuario);
                                                        if (_existeUsuario.Rows.Count == 0)
                                                        {
                                                            //Sino existe insertar el nuevo usuario
                                                            consulta = " INSERT INTO P_AUT_DOCTOS_PR " +
                                                            " (DOCTO_PR_DET_ID " +
                                                            " , USUARIO_ID " +
                                                            " , ESTATUS " +
                                                            " , MONTO_AUTORIZADO" +
                                                            " , NIVEL) " +
                                                            " VALUES " +
                                                            " (" + _Pag_selec[0] +
                                                            " , " + _usuario[0, 0] +
                                                            " , 'P' " +
                                                            " ,NULL" +
                                                            " ," + _usuario[1, 0] + ")";

                                                            cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                            if (cmd.ExecuteNonQuery() == 0)
                                                            {
                                                                transaction.Rollback();
                                                                cmd.Dispose();
                                                                conexion.Desconectar();
                                                                MessageBox.Show("No se pudo completar la operación favor de reintentar", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                                return;
                                                            }
                                                        }
                                                        _da.Dispose();
                                                    }
                                                }
                                                cmd.Dispose();
                                            }

                                        }

                                        bool actualizarEncabezado = false;
                                        List<int> DetallesAprobados = new List<int>();

                                        foreach (List<string> _Pag_selec in _Seleccionados)
                                        {
                                            consulta = "SELECT TOTAL - AUTORIZADOS AS DIF FROM ";
                                            consulta += " (SELECT COUNT(*) TOTAL, ";
                                            consulta += " (SELECT COUNT(*) FROM P_AUT_DOCTOS_PR p ";
                                            consulta += " WHERE p.DOCTO_PR_DET_ID = " + _Pag_selec[0];
                                            consulta += " AND p.ESTATUS = 'A' AND NIVEL = " + NIVEL_DATAGRID + ") AUTORIZADOS ";
                                            consulta += " FROM P_AUT_DOCTOS_PR ";
                                            consulta += " WHERE DOCTO_PR_DET_ID = " + _Pag_selec[0];
                                            consulta += " AND NIVEL = " + NIVEL_DATAGRID + ") AS CP ";
                                            consulta += " GROUP BY CP.TOTAL, CP.AUTORIZADOS";

                                            cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                            SqlDataReader sdr = cmd.ExecuteReader();

                                            while (sdr.Read())
                                            {
                                                int _dif = Convert.ToInt32(Convert.ToString(sdr["DIF"]));
                                                if (_dif == 0)
                                                {
                                                    actualizarEncabezado = true;
                                                    DetallesAprobados.Add(Convert.ToInt32(_Pag_selec[0]));
                                                }
                                            }
                                            sdr.Close();
                                            cmd.Dispose();
                                        }

                                        // CORRECCIÓN: Manejar correctamente el siguiente nivel
                                        if (actualizarEncabezado)
                                        {
                                            if (TieneSiguienteNivel(Convert.ToInt32(NIVEL_DATAGRID)))
                                            {
                                                // Si hay siguiente nivel, crear registros para usuarios del nivel anterior
                                                foreach (string[,] _usuario in _usuarios)
                                                {
                                                    foreach (int detalleId in DetallesAprobados)
                                                    {
                                                        consulta = " UPDATE P_AUT_DOCTOS_PR SET ";
                                                        consulta += " ESTATUS = 'C'";
                                                        consulta += " WHERE DOCTO_PR_DET_ID = " + detalleId;
                                                        consulta += " AND USUARIO_ID = " + _usuario[0, 0];

                                                        cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();
                                                    }
                                                }

                                                consulta = " UPDATE P_DOCTOS_PR SET ";
                                                consulta += " NIVEL = " + (Convert.ToInt32(NIVEL_DATAGRID) - 1) + ", ";
                                                consulta += " ESTATUS_PROC = 'P' ";
                                                consulta += " WHERE FOLIO = '" + FOLIO + "'";
                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                            }
                                            else
                                            {
                                                // Es el nivel final, marcar como completamente autorizado
                                                consulta = " UPDATE P_DOCTOS_PR SET ";
                                                consulta += " ESTATUS_PROC = 'A', ";
                                                consulta += " NIVEL = " + NIVEL_FINAL;
                                                consulta += " WHERE FOLIO = '" + FOLIO + "'";
                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                            }
                                        }
                                        #endregion

                                        C_AUT_PAGOS f = new C_AUT_PAGOS();
                                        if (actualizarEncabezado)
                                        {
                                            if (EsNivelFinal(Convert.ToInt32(NIVEL_DATAGRID)))
                                            {
                                                #region NIVEL FINAL - AUTORIZACIÓN COMPLETA

                                                // Lógica para nivel final (autorización completa)
                                                _ImporteTotal = 0.0;
                                                foreach (List<string> _Pag_selec in _Seleccionados)
                                                {
                                                    _ImporteTotal += f.ObtenerMenorMontoAutorizado(FOLIO,
                                                                                                   Convert.ToInt32(_Pag_selec[0]),
                                                                                                   conexion,
                                                                                                   transaction,
                                                                                                   out msg_local);
                                                    if (msg_local.Length > 0)
                                                    {
                                                        transaction.Rollback();
                                                        conexion.Desconectar();
                                                        MessageBox.Show("Error al intentar obtener los montos autorizados", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        return;
                                                    }
                                                }

                                                // Actualizar a estado final autorizado
                                                consulta = "UPDATE P_DOCTOS_PR SET ";
                                                consulta += "IMPORTE_AUTORIZADO = " + _ImporteTotal + ", ";
                                                consulta += "ESTATUS_PROC = 'A', ";
                                                consulta += "USUARIO_AUTORIZO = '" + usuarioLogueado.Usuario + "', ";
                                                consulta += "FECHA_HORA_AUTORIZO = @FechaAutorizo ";
                                                consulta += "WHERE FOLIO = '" + FOLIO + "'";

                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                cmd.Parameters.Add("@FechaAutorizo", SqlDbType.DateTime).Value = DateTime.Now;
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();

                                                //actualizamos el estatus del encabezado de P_DOCTOS_PP a 'A'
                                                consulta = "UPDATE P_DOCTOS_PP SET" +
                                                "       ESTATUS_PROC = 'A'" +
                                                " WHERE DOCTO_PP_ID in (select DPP.DOCTO_PP_ID from P_DOCTOS_PP as DPP " +
                                                " inner join P_DOCTOS_PP_DET as DPPD on DPP.DOCTO_PP_ID = DPPD.DOCTO_PP_ID " +
                                                " inner join P_DOCTOS_PR_DET as DPRD on DPPD.FOLIO_MICROSIP = DPRD.FOLIO_MICROSIP " +
                                                " inner join P_DOCTOS_PR as DPR on DPRD.DOCTO_PR_ID = DPR.DOCTO_PR_ID " +
                                                " where " +
                                                " DPR.FOLIO = '" + FOLIO + "' " +
                                                " group by DPP.DOCTO_PP_ID)";
                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();

                                                // Actualizar estatus de  P_DOCTOS_PR_DET y P_DOCTOS_PP_DET que no fueron autorizadas a estatus "X"
                                                string _idsSeleccionados = "";
                                                foreach (List<string> _Pag_selec in _Seleccionados)
                                                {
                                                    //Obtener el folio para validar si es una requisicion
                                                    string _folioReq = _Pag_selec[1];
                                                    if (_folioReq.Contains("REQ"))
                                                    {
                                                        DataTable _requisicion = new DataTable();
                                                        _requisicion = f.DatosRequisicion(Convert.ToInt32(_Pag_selec[0]), out msg_local);
                                                        if (_requisicion.Rows.Count > 0)
                                                        {
                                                            //Cambiar el estatus de la requisicion a "S" ->surtido
                                                            string _reqID = Convert.ToString(_requisicion.Rows[0]["REQUISICION_ID"]);
                                                            if (_reqID.Length > 0)
                                                            {
                                                                consulta = "UPDATE REQ_ENC " +
                                                                  " SET Estatus_general = 'S'" +
                                                                  " WHERE Requisicion_id = " + _reqID;
                                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                                cmd.ExecuteNonQuery();
                                                                cmd.Dispose();
                                                            }
                                                        }
                                                    }
                                                    _idsSeleccionados += _Pag_selec[0] + ",";
                                                }

                                                /*Actualizar DOCTOS_PR_DET de estatus "C" o "P" a "X"*/
                                                if (_idsSeleccionados.Length > 0)
                                                {
                                                    _idsSeleccionados = _idsSeleccionados.Substring(0, _idsSeleccionados.Length - 1);
                                                    consulta = "update P_DOCTOS_PR_DET " +
                                                        " set ESTATUS = 'X' " +
                                                        " where DOCTO_PR_ID = (" +
                                                        "  select DPRD.DOCTO_PR_ID from P_DOCTOS_PR as PDPR " +
                                                        " INNER JOIN P_DOCTOS_PR_DET AS DPRD on PDPR.DOCTO_PR_ID = DPRD.DOCTO_PR_ID " +
                                                        " where " +
                                                        " PDPR.FOLIO = '" + FOLIO + "' and DPRD.DOCTO_PR_DET_ID NOT IN (" + _idsSeleccionados + ") group by DPRD.DOCTO_PR_ID)";

                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();

                                                    /*Actualizar DOCTOS_PP_DET de estatus "T" a "X"*/
                                                    consulta = "update P_DOCTOS_PP_DET " +
                                                        " set ESTATUS = 'X' " +
                                                        " where DOCTO_PP_DET_ID in " +
                                                        " (select DPPD.DOCTO_PP_DET_ID from P_DOCTOS_PP_DET AS DPPD " +
                                                        " INNER JOIN P_DOCTOS_PR_DET AS PDPD ON PDPD.FOLIO_MICROSIP = DPPD.FOLIO_MICROSIP and PDPD.DOCTO_PP_DET_ID = DPPD.DOCTO_PP_DET_ID  " +
                                                        " INNER JOIN P_DOCTOS_PR as PDPR on PDPD.DOCTO_PR_ID = PDPR.DOCTO_PR_ID " +
                                                        " where " +
                                                        " PDPR.FOLIO = '" + FOLIO + "' and PDPD.ESTATUS = 'X')";

                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();
                                                }

                                                #endregion
                                            }
                                            else
                                            {
                                                #region NIVEL INTERMEDIO - PASAR AL SIGUIENTE NIVEL

                                                // Nivel intermedio, solo actualizar para pasar al siguiente
                                                _ImporteTotal = 0.0;
                                                foreach (List<string> _Pag_selec in _Seleccionados)
                                                {
                                                    _ImporteTotal += f.ObtenerMenorMontoAutorizado(FOLIO,
                                                                                                   Convert.ToInt32(_Pag_selec[0]),
                                                                                                   conexion,
                                                                                                   transaction,
                                                                                                   out msg_local);
                                                    if (msg_local.Length > 0)
                                                    {
                                                        transaction.Rollback();
                                                        conexion.Desconectar();
                                                        MessageBox.Show("Error al intentar obtener los montos autorizados",
                                                                      "Información",
                                                                      MessageBoxButtons.OK,
                                                                      MessageBoxIcon.Information);
                                                        return;
                                                    }
                                                }

                                                consulta = "UPDATE P_DOCTOS_PR SET ";
                                                consulta += "IMPORTE_AUTORIZADO = " + _ImporteTotal + ", ";
                                                consulta += "ESTATUS_PROC = 'P', ";
                                                consulta += "NIVEL = " + (Convert.ToInt32(NIVEL_DATAGRID) - 1) + ", ";
                                                consulta += "USUARIO_AUTORIZO = '" + usuarioLogueado.Usuario + "', ";
                                                consulta += "FECHA_HORA_AUTORIZO = @FechaAutorizo ";
                                                consulta += "WHERE FOLIO = '" + FOLIO + "'";

                                                cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                cmd.Parameters.Add("@FechaAutorizo", SqlDbType.DateTime).Value = DateTime.Now;
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();

                                                #endregion
                                            }

                                            #region ACTUALIZA DETALLE

                                            foreach (List<string> _Pag_selec in _Seleccionados)
                                            {
                                                string aux = Convert.ToString(f.ObtenerMenorMontoAutorizado(FOLIO,
                                                                                                            Convert.ToInt32(_Pag_selec[0]),
                                                                                                            conexion,
                                                                                                            transaction,
                                                                                                            out msg_local));

                                                if (DetallesAprobados.Count > 0)
                                                {
                                                    bool encontrado = false;
                                                    for (int i = 0; i < DetallesAprobados.Count; i++)
                                                    {
                                                        if (DetallesAprobados[i] == Convert.ToInt32(_Pag_selec[0]))
                                                        {
                                                            encontrado = true;
                                                            consulta = "UPDATE P_DOCTOS_PR_DET SET ";
                                                            if (usuarioLogueado.U_ROL == "A")
                                                            {
                                                                consulta += "IMPORTE_AUTORIZADO = " + aux + ", ";
                                                                consulta += "ESTATUS = 'A' ";
                                                            }
                                                            else
                                                            {
                                                                consulta += "IMPORTE_CAPTURISTA = " + aux + ", ";
                                                                consulta += "ESTATUS = 'C' ";
                                                            }
                                                            consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                            cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                            cmd.ExecuteNonQuery();
                                                            cmd.Dispose();
                                                            break;
                                                        }
                                                    }

                                                    if (!encontrado)
                                                    {
                                                        consulta = "UPDATE P_DOCTOS_PR_DET SET ";
                                                        if (usuarioLogueado.U_ROL == "A")
                                                        {
                                                            consulta += "IMPORTE_AUTORIZADO = " + aux + ", ";
                                                            consulta += "ESTATUS = 'P' ";
                                                        }
                                                        else
                                                        {
                                                            consulta += "IMPORTE_CAPTURISTA = " + aux + ", ";
                                                            consulta += "ESTATUS = 'C' ";
                                                        }
                                                        consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                        cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();
                                                    }
                                                }
                                                else
                                                {
                                                    consulta = "UPDATE P_DOCTOS_PR_DET SET ";
                                                    if (usuarioLogueado.U_ROL == "A")
                                                    {
                                                        consulta += "IMPORTE_AUTORIZADO = " + aux + ", ";
                                                        consulta += "ESTATUS = 'P' ";
                                                    }
                                                    else
                                                    {
                                                        consulta += "IMPORTE_CAPTURISTA = " + aux + ", ";
                                                        consulta += "ESTATUS = 'C' ";
                                                    }
                                                    consulta += " WHERE DOCTO_PR_DET_ID = '" + _Pag_selec[0] + "'";
                                                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();
                                                }
                                            }

                                            #endregion
                                        }

                                        transaction.Commit();
                                        conexion.Desconectar();

                                        MessageBox.Show("Pagos autorizados correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Close();
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    transaction.Rollback();
                                    msg_local = Ex.Message;
                                }

                                conexion.Desconectar();
                            }
                        }

                        if (msg_local.Length > 0)
                        {
                            MessageBox.Show(msg_local, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("¿Desea solicitar la peticion de los pagos seleccionados?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                        C_ConexionSQL conexion = new C_ConexionSQL();
                        SqlTransaction transaction;
                        SqlCommand cmd;

                        string msg_local = "";
                        string consulta = "";

                        if (registros.LeerRegistros(false))
                        {
                            if (conexion.ConectarSQL())
                            {
                                transaction = conexion.SC.BeginTransaction();

                                try
                                {
                                    if (_Ceros == true)
                                    {
                                        _resp = MessageBox.Show("Importe total autorizado \"" + _ImporteTotal.ToString("C2") + "\"\n\rEl monto autorizado de uno o mas " +
                                           "pagos es cero\n\r¿Desea Continuar?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    }

                                    if (_Ceros == false || _resp == DialogResult.Yes)
                                    {

                                        #region ACTUALIZA ENCABEZADO
                                        consulta = "UPDATE P_DOCTOS_PP SET ";

                                        consulta += "       IMPORTE_AUTORIZADO = " + _ImporteTotal + ", ";
                                        consulta += "       ESTATUS_PROC = 'T' ";

                                        consulta += " WHERE FOLIO = '" + FOLIO + "' ";
                                        cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                        #endregion

                                        #region ACTUALIZA DETALLE
                                        foreach (List<string> _Pag_selec in _Seleccionados)
                                        {
                                            string aux = Convert.ToString(_Pag_selec[6]).Replace(",", "");

                                            consulta = "UPDATE P_DOCTOS_PP_DET SET ";

                                            consulta += "       IMPORTE_AUTORIZADO = " + aux + ", ";
                                            consulta += "       ESTATUS = 'A' ";

                                            consulta += " WHERE DOCTO_PP_DET_ID = '" + _Pag_selec[0] + "'";
                                            cmd = new SqlCommand(consulta, conexion.SC, transaction);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();
                                            break;
                                        }

                                        #endregion

                                        transaction.Commit();
                                        conexion.Desconectar();

                                        MessageBox.Show("Petición enviada correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Close();
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    transaction.Rollback();
                                    msg_local = Ex.Message;
                                }

                                conexion.Desconectar();
                            }
                        }

                        if (msg_local.Length > 0)
                        {
                            MessageBox.Show(msg_local, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Ningún pago ha sido seleccionado.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Guardar cambios
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
                                _resp = MessageBox.Show("Importe total autorizado \"" + _ImporteTotal.ToString("C2") + "\"\n\rEl monto autorizado de uno o mas " +
                                   "pagos es cero\n\r¿Desea Continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            }
                            else
                            {
                                if (MessageBox.Show("Importe total autorizado \"" + _ImporteTotal.ToString("C2") + "\"\n\rEl monto autorizado de uno o mas " +
                                   "pagos es cero\n\r¿Desea Continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
                                    cmd.Parameters.Add("@EMPRESA",SqlDbType.VarChar).Value = _Pag_selec[_Pag_selec.Count - 1];
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
                                MessageBox.Show("Programación guardada","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Information);
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





        private void dGVDetPagos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {
                if (Convert.ToBoolean(dGVDetPagos["Autorizar", e.RowIndex].EditedFormattedValue))
                {
                    if (dGVDetPagos["IMPORTE_AUT", e.RowIndex].Value.ToString().Replace("$", "").Replace(",", "") == "0.00")
                    {
                        dGVDetPagos["IMPORTE_AUT", e.RowIndex].Value = dGVDetPagos["IMPORTE_P", e.RowIndex].Value.ToString().Replace("$", "").Replace(",", "");


                        dGVDetPagos.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(150, 255, 150);
                        dGVDetPagos.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 100);
                        dGVDetPagos["IMPORTE_AUT", e.RowIndex].Style.BackColor = Color.Empty;
                    }
                }
                else
                {
                    if (dGVDetPagos.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(231, 212, 136))
                    {
                        dGVDetPagos["IMPORTE_AUT", e.RowIndex].Value = "0.00";

                        dGVDetPagos.CurrentRow.DefaultCellStyle.BackColor = Color.Empty;
                        dGVDetPagos.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Empty;
                    }
                }
            }
            else
            {
                try
                {
                    /*for (int j = 0; j < dgvUser.RowCount; j++)
                        dgvUser["AUTORIZADO_AUT", j].Value = false;
                    NO_ELIMINABLE = false;
                    for (int i = 0; i < pagos.Length; i++)
                    {
                        if (Convert.ToInt32(dGVDetPagos["DOCTO_PR_DET_ID", e.RowIndex].Value.ToString()) == pagos[i].DOCTO_PR_DET_ID)
                        {
                            for (int j = 0; j < dgvUser.RowCount; j++)
                            {
                                if (dgvUser["USUARIO_AUT", j].Value.ToString() == pagos[i].NOMBRE)
                                {
                                    if (pagos[i].ESTATUS == "A")
                                    {
                                        dgvUser["AUTORIZADO_AUT", j].Value = 1;
                                        NO_ELIMINABLE = true;
                                    }
                                    else
                                    {
                                        dgvUser["AUTORIZADO_AUT", j].Value = false;
                                    }
                                }
                            }

                        }
                    }*/
                }
                catch
                {

                }
            }
        }

        private void dGVDetPagos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                if (Convert.ToBoolean(dGVDetPagos["Autorizar", e.RowIndex].EditedFormattedValue))
                {
                    if (dGVDetPagos["IMPORTE_AUT", e.RowIndex].Value.ToString().Replace("$", "").Replace(",", "") == "0.00")
                    {
                        dGVDetPagos["IMPORTE_AUT", e.RowIndex].Value = dGVDetPagos["IMPORTE_P", e.RowIndex].Value.ToString().Replace("$", "").Replace(",", "");

                        dGVDetPagos.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(150, 255, 150);
                        dGVDetPagos.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 100);
                    }
                }
                else
                {
                    if (dGVDetPagos.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(231, 212, 136))
                    {
                        dGVDetPagos["IMPORTE_AUT", e.RowIndex].Value = "0.00";

                        dGVDetPagos.CurrentRow.DefaultCellStyle.BackColor = Color.Empty;
                        dGVDetPagos.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Empty;
                    }
                }
            }
            else
            {
                try
                {
                    for (int j = 0; j < dgvUser.RowCount; j++)
                        dgvUser["AUTORIZADO_AUT", j].Value = false;

                    NO_ELIMINABLE = false;
                    for (int i = 0; i < pagos.Length; i++)
                    {
                        if (Convert.ToInt32(dGVDetPagos["DOCTO_PR_DET_ID", e.RowIndex].Value.ToString()) == pagos[i].DOCTO_PR_DET_ID)
                        {
                            for (int j = 0; j < dgvUser.RowCount; j++)
                            {
                                if (dgvUser["USUARIO_AUT", j].Value.ToString() == pagos[i].NOMBRE)
                                {
                                    if (pagos[i].ESTATUS == "A")
                                    {
                                        dgvUser["AUTORIZADO_AUT", j].Value = 1;
                                        NO_ELIMINABLE = true;
                                    }
                                    else
                                    {
                                        dgvUser["AUTORIZADO_AUT", j].Value = false;
                                    }
                                }
                            }

                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void dGVDetPagos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            richTextBox1.Text = Convert.ToString(dGVDetPagos["C_COMENTARIOS", e.RowIndex].Value);
           
        }





        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                string estatus = Convert.ToString(dGVDetPagos["ESTATUS_P", dGVDetPagos.CurrentRow.Index].Value);

                // if (estatus == "En banco")
                if (estatus == "Liberado")
                {
                    marcarComoCanceladoToolStripMenuItem.Enabled = true;
                }
                else
                {
                    marcarComoCanceladoToolStripMenuItem.Enabled = false;

                }

                if (NO_ELIMINABLE)
                    marcarComoCanceladoToolStripMenuItem.Enabled = false;
                else
                    marcarComoCanceladoToolStripMenuItem.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error al obtener el estatus del pago.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                marcarComoCanceladoToolStripMenuItem.Enabled = false;
            }
        }

        private void marcarComoCanceladoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int docto_pr_det_id = Convert.ToInt32(dGVDetPagos["DOCTO_PR_DET_ID", dGVDetPagos.CurrentRow.Index].Value);
                int proveedor_id = Convert.ToInt32(dGVDetPagos["C_PROVEEDOR_ID", dGVDetPagos.CurrentRow.Index].Value);

                string folio = Convert.ToString(dGVDetPagos["FOLIO_P", dGVDetPagos.CurrentRow.Index].Value);
                string pago = Convert.ToString(dGVDetPagos["C_PAGO", dGVDetPagos.CurrentRow.Index].Value);

                if (ValidaPagoCanceladoMicrosip(pago, proveedor_id))
                {
                    if (MessageBox.Show("Se marcara el pago '" + folio + "' como cancelado, este proceso no podra ser revertido y se vera reflejado en el portal.\n\n¿Desea continuar con la cancelación?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                        C_ConexionSQL conn = new C_ConexionSQL();
                        SqlTransaction transaction;
                        SqlCommand cmd;

                        C_CONEXIONWEBSVC websvc = new C_CONEXIONWEBSVC();

                        if (registros.LeerRegistros(false))
                        {
                            if (conn.ConectarSQL())
                            {
                                transaction = conn.SC.BeginTransaction();

                                #region INTENTA CANCELAR EL PAGO EN EL DOMINIO Y LUEGO LOCALMENTE, EN CASO DE ERROR ABORTA

                                try
                                {
                                    if (websvc.CancelaDoctosDetCorporativo(conn, transaction, FOLIO, EMPRESA[0].NOMBRE_CORTO, folio))
                                    {
                                        cmd = new SqlCommand("UPDATE p_doctos_pr_det SET estatus = 'X' WHERE docto_pr_det_id = " + docto_pr_det_id, conn.SC, transaction);
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();

                                        transaction.Commit();

                                        Cargar();

                                        MessageBox.Show("Pago cancelado satisfactoriamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        MessageBox.Show("No fue posible cancelar el pago en el dominio.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                #endregion

                                conn.Desconectar();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se pudieron leer los registros de Windows.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El pago '" + pago + "' sigue activo para poderlo cancelar en el sistema necesita primero cancelarlo en Microsip.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error al obtener el ID del pago.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    if(!string.IsNullOrEmpty(dGVDetPagos["IMPORTE_P", i].Value.ToString()))
                    pago[pago.Length - 1].IMPORTE = Convert.ToDouble(dGVDetPagos["IMPORTE_P", i].Value.ToString().Replace("$",""));
                    pago[pago.Length - 1].IMP_AUTORIZADO = Convert.ToDouble(dGVDetPagos["IMPORTE_AUT", i].Value.ToString());
                    pago[pago.Length - 1].ESTATUS = dGVDetPagos["ESTATUS_P", i].Value.ToString();
                    if (!string.IsNullOrEmpty(dGVDetPagos["C_PAGO", i].Value.ToString()))
                        pago[pago.Length - 1].PAGO = Convert.ToDouble(dGVDetPagos["C_PAGO", i].Value.ToString());

                    pago[pago.Length - 1].EMPRESA = dGVDetPagos["P_EMPRESA", i].Value.ToString();
                }

                C_USUARIOS[] usuarios = new C_USUARIOS[0];
                for (int i = 0; i < dgvUser.RowCount; i++)
                {
                    Array.Resize(ref usuarios, usuarios.Length + 1);
                    usuarios[usuarios.Length - 1] = new C_USUARIOS();

                    usuarios[usuarios.Length - 1].Usuario = Convert.ToString(dgvUser["USUARIO_AUT", i].Value);
                }
                //para usuarios

                F_PROGRAMACIONPAGOS fp = new F_PROGRAMACIONPAGOS();
                fp.MODIF = "S";
                fp.NOMBRE_EMPESA = EMPRESA;
                fp.pagosModif1 = pago;
                fp.FECHA_PAGO_MODIF = FECHA_PAGO;
                fp.DOCTO_PR_ID = DOCTO_PR_ID;
                fp.FOLIO_MODIF = FOLIO;
                fp.ListadoUsuarios = usuarios;
                fp.usuarioLogueado = usuarioLogueado;
                fp.ShowDialog();

                if(fp.MODIF == "EXITO")
                {
                    Cargar();
                }
            }
            catch
            {

            }
        }

        private void eliminarRenglonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que desea eliminar el cargo con folio " +
                dGVDetPagos["FOLIO_P",renglon].Value.ToString() + "?","Mensaje de la aplicación",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
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
                            string delete = "DELETE FROM P_DOCTOS_PR_DET WHERE DOCTO_PR_DET_ID = @DET";
                            SqlCommand sc = new SqlCommand(delete, con.SC, tran);
                            sc.Parameters.Add("@DET", SqlDbType.Int).Value = Convert.ToInt32(Convert.ToString(dGVDetPagos["DOCTO_PR_DET_ID", renglon].Value));
                            sc.ExecuteNonQuery();
                            sc.Dispose();

                            delete = "DELETE FROM P_AUT_DOCTOS_PR WHERE DOCTO_PR_DET_ID = @DET";
                            sc = new SqlCommand(delete, con.SC, tran);
                            sc.Parameters.Add("@DET", SqlDbType.Int).Value = Convert.ToInt32(Convert.ToString(dGVDetPagos["DOCTO_PR_DET_ID", renglon].Value));
                            sc.ExecuteNonQuery();

                            tran.Commit();
                            dGVDetPagos.Rows[renglon].Visible = false;
                            eliminarPagos = true;
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Ocurrio un error inesperado al borrar el cargo " + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            tran.Rollback();
                        }
                        con.Desconectar();
                    }
                }
            }
        }

        private void dGVDetPagos_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            if (e.Button == MouseButtons.Right)
            {
                renglon = e.RowIndex;

                dGVDetPagos.CurrentCell = dGVDetPagos.Rows[e.RowIndex].Cells[e.ColumnIndex];

                dGVDetPagos.Rows[e.RowIndex].Selected = true;
                dGVDetPagos.Focus();

               
            }
        }

        private void F_DETALLEPROGRAMACIONPAGOS_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void F_DETALLEPROGRAMACIONPAGOS_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dGVDetPagos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //MostrarUsuarios(e.RowIndex);
            
        }

        private void MostrarUsuarios(int index)
        {
            try
            {
                /*for (int j = 0; j < dgvUser.RowCount; j++)
                {
                    dgvUser["AUTORIZADO_AUT", j].Value = false;
                }
                NO_ELIMINABLE = false;

                for (int i = 0; i < pagos.Length; i++)
                {
                    if (Convert.ToInt32(dGVDetPagos["DOCTO_PR_DET_ID", index].Value.ToString()) == pagos[i].DOCTO_PR_DET_ID)
                    {
                        for (int j = 0; j < dgvUser.RowCount; j++)
                        {
                            if (dgvUser["USUARIO_AUT", j].Value.ToString() == pagos[i].NOMBRE)
                            {
                                if (pagos[i].ESTATUS == "A")
                                {
                                    dgvUser["AUTORIZADO_AUT", j].Value = 1;
                                    NO_ELIMINABLE = true;
                                }
                                else
                                {
                                    dgvUser["AUTORIZADO_AUT", j].Value = false;
                                }
                            }
                        }

                    }
                }*/
            }
            catch
            {

            }
        }

        private void F_DETALLEPROGRAMACIONPAGOS_Shown(object sender, EventArgs e)
        {
            /*if (usuarioLogueado.U_ROL == "C" && usuarioLogueado.Requisitante=="S")
            {
                button1.Text = "Enviar Petición";
                Refresh();
                dgvUser.Visible = false;
                label3.Visible = false;
            }*/
        }

        private void dGVDetPagos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int _indice = e.RowIndex;
            if (_indice> -1)
            {
                string msg_local = "";
                C_AUT_PAGOS f = new C_AUT_PAGOS();
                C_ConexionSQL conexion=new C_ConexionSQL();
                if (conexion.ConectarSQL())
                {
                    SqlTransaction transaction = conexion.SC.BeginTransaction();
                    double _cantidadSolicitada = Convert.ToDouble(Convert.ToString(dGVDetPagos["IMPORTE_P", _indice].Value).Replace("$", "")),
                        _cantidadAutorizada = Convert.ToDouble(dGVDetPagos["IMPORTE_AUT", _indice].Value),
                        _cantidadMenorAutorizada = f.ObtenerMenorMontoAutorizado(FOLIO, Convert.ToInt32(dGVDetPagos["DOCTO_PR_DET_ID", _indice].Value), conexion, transaction, out msg_local);
                    if (msg_local.Length == 0)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        conexion.Desconectar();
                        if (_cantidadAutorizada > _cantidadSolicitada)
                        {
                            MessageBox.Show("La cantidad autorizada no puede ser mayor a la solicitada", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dGVDetPagos["IMPORTE_AUT", _indice].Value = _cantidadSolicitada;
                        }
                        else if (_cantidadMenorAutorizada>0 && _cantidadAutorizada > _cantidadMenorAutorizada)
                        {
                            MessageBox.Show("La cantidad autorizada no puede ser mayor a la menor solicitada previamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dGVDetPagos["IMPORTE_AUT", _indice].Value = _cantidadMenorAutorizada;
                        }
                    }else
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                        conexion.Desconectar();
                    }
                }
                else
                    msg_local = "No se puede establecer comunicación con el servidor";
            }
        }
    }
}
