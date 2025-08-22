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
using Microsoft.WindowsAPICodePack.Taskbar;

namespace PagosIntermex
{
    public partial class F_PROGRAMACIONPAGOS : Form
    {
        class REQ_OC
        {
            public string FOLIO_REQ { get; set; }
            public string MSP_FOLIO { get; set; }
            public int REQUISICION_ID { get; set; }
        }

        DataSet ds = new DataSet();
        private string DATABASE_NAME = "";

        private int ANTICIPO_COUNT = 0;

        public C_PAGOS[] pagosModif1;
        public string FECHA_PAGO_MODIF { get; set; }

        public C_USUARIOS usuarioLogueado;

        public F_PROGRAMACIONPAGOS()
        {
            InitializeComponent();
        }


        public C_EMPRESAS[] NOMBRE_EMPESA { set; get; }

      
        
        public string MODIF { get; set; }
        public int DOCTO_PR_ID { get; set; }
        public string FOLIO_MODIF { get; set; }
        internal C_USUARIOS[] ListadoUsuarios { get; set; }
        private void F_PROGRAMACIONPAGOS_Load(object sender, EventArgs e)
        {
            dgvProgramas.DoubleBuffered(true);

            //if(usuarioLogueado.Requisitante == "S")
            {
                splitContainer1.Panel2.Enabled = false;
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();
            }

            if (usuarioLogueado.Tesoreria == "N")
            {
                button3.Text = "Crear Petición";
                btnSearchInMSP.Text = "Buscar Microsip";
            }
            else
            {
                button3.Text = "Crear Programación";
                btnSearchInMSP.Text = "Buscar Peticiones";
            }
            
        }

        private void F_PROGRAMACIONPAGOS_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool seleccionados = false;

            for (int r = 0; r < dgvProgramas.RowCount; r++)
            {
                if (Convert.ToBoolean(dgvProgramas.Rows[r].Cells["dgvp_check"].Value))
                {
                    seleccionados = true;
                    break;
                }
            }

            if (seleccionados)
            {
                if (MessageBox.Show("La programación de pagos no ah sido guardada, si sale todo el progreso se perdera.\n\n¿Desea salir aun asi?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }





        // BUSCAR MICROSIP
        private void btnSearchInMSP_Click(object sender, EventArgs e)
        {
            dgvProgramas.Rows.Clear();
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);

            
          
            string SELECT = "";

            //para cuando el usuario genere programaciones de pago en base a las peticiones
           // if ((usuarioLogueado.Requisitante == "N" || usuarioLogueado.Tesoreria == "S"))
            {
                dgvProgramas.Columns["FOLIO_CARGO_MSP"].Visible = true;
                dgvProgramas.Columns["CLAVE_DEPTO"].Visible = true;

                SELECT = "SELECT  ";
                SELECT += " pp.DOCTO_PP_ID DOCTO_ENC, ";
                SELECT += " pp.FOLIO, ";
                SELECT += " pp.IMPORTE_PAGOS, ";
                SELECT += " pp.FECHA_PAGO, ";
                SELECT += " ppd.*, ";
                SELECT += " u.Usuario ";
                SELECT += " ,u.Clave_Depto ";
                SELECT += "  FROM P_DOCTOS_PP pp ";
                SELECT += " JOIN P_DOCTOS_PP_DET ppd on ppd.DOCTO_PP_ID = pp.DOCTO_PP_ID ";
                SELECT += " JOIN USUARIOS u on u.Usuario_id = pp.USUARIO_ID_CREADOR ";
                //SELECT += " WHERE pp.ESTATUS_PROC = 'P'";
                SELECT += "  WHERE ppd.ESTATUS = 'C' and pp.ESTATUS = 'A' order by pp.FOLIO;";

                C_ConexionSQL con = new C_ConexionSQL();

                if (con.ConectarSQL())
                {
                    SqlCommand sc = new SqlCommand(SELECT, con.SC);
                    SqlDataReader sdr = sc.ExecuteReader();

                    string docto_pp_id = "";

                    while (sdr.Read())
                    {
                        

                        #region ENCABEZADO
                        if(docto_pp_id == "")
                        {
                            dgvProgramas.Rows.Add();
                            docto_pp_id = Convert.ToString(sdr["DOCTO_ENC"]);

                            dgvProgramas["DOCTO_PP_ID", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_ENC"]);
                            dgvProgramas["dgvp_folio", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["FOLIO"]);
                            dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(sdr["IMPORTE_PAGOS"].ToString()).ToString("#,##0.00");
                            dgvProgramas["dgvp_fecha", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(Convert.ToString(sdr["FECHA_PAGO"])).ToString("dd/MM/yyyy");
                            dgvProgramas["CLAVE_DEPTO", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["Clave_Depto"]);

                            dgvProgramas["dgvp_proveedor", dgvProgramas.RowCount - 1].Value = "*****Ver en detalle*****";
                            dgvProgramas["P_EMPRESA", dgvProgramas.RowCount - 1].Value = "*****Ver en detalle*****";
                            dgvProgramas["dgvp_vencimiento", dgvProgramas.RowCount - 1].Value = "*****Ver en detalle*****";

                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(82, 82, 82);
                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(102, 102, 102);
                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.ForeColor = Color.White;
                        }
                        #endregion

                        #region DETALLE

                        if (docto_pp_id == Convert.ToString(sdr["DOCTO_ENC"]))
                        {
                            dgvProgramas.Rows.Add();

                            
                            dgvProgramas["dgvp_check", dgvProgramas.RowCount - 1].ReadOnly = true;

                            dgvProgramas["FOLIO_REQ", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["FOLIO_REQ"]);
                            dgvProgramas["FOLIO_CARGO_MSP", dgvProgramas.RowCount - 1].Value = sdr["FOLIO_MICROSIP"].ToString();
                            dgvProgramas["dgvp_fecha", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(Convert.ToString(sdr["FECHA_CARGO"])).ToString("dd/MM/yyyy");
                            dgvProgramas["dgvp_proveedor", dgvProgramas.RowCount - 1].Value = sdr["PROVEEDOR_NOMBRE"].ToString();
                            dgvProgramas["dgvp_vencimiento", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_VENCIMIENTO"].ToString()).ToString("dd/MM/yyyy");

                            // dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(reader["SALDO_CARGO"].ToString()).ToString("#,##0.00");
                            dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(sdr["IMPORTE_CAPTURISTA"].ToString()).ToString("#,##0.00");

                            dgvProgramas["dgvp_proveedor_id", dgvProgramas.RowCount - 1].Value = sdr["PROVEEDOR_ID"].ToString();
                            dgvProgramas["dgvp_proveedor_clave", dgvProgramas.RowCount - 1].Value = sdr["PROVEEDOR_CLAVE"].ToString();
                            dgvProgramas["P_EMPRESA", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["EMPRESA"]);
                            dgvProgramas["DOCTO_PP_DET_ID", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_PP_DET_ID"]);
                            dgvProgramas["DOCTO_PP_ID_2", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_PP_ID"]);
                            dgvProgramas["CLAVE_DEPTO_DET", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["Clave_Depto"]);
                            dgvProgramas["Requisicion_id", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["Requisicion_id"]);

                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(153, 153, 153);
                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(163, 163, 163);
                        }
                        #endregion

                        if(docto_pp_id != Convert.ToString(sdr["DOCTO_ENC"]))
                        {
                            dgvProgramas.Rows.Add();
                            docto_pp_id = Convert.ToString(sdr["DOCTO_ENC"]);

                            dgvProgramas["DOCTO_PP_ID", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_ENC"]);
                            dgvProgramas["dgvp_folio", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["FOLIO"]);
                            dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(sdr["IMPORTE_PAGOS"].ToString()).ToString("#,##0.00");
                            dgvProgramas["dgvp_fecha", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(Convert.ToString(sdr["FECHA_PAGO"])).ToString("dd/MM/yyyy");
                            dgvProgramas["CLAVE_DEPTO", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["Clave_Depto"]);

                            dgvProgramas["dgvp_proveedor", dgvProgramas.RowCount - 1].Value = "*****Ver en detalle*****";
                            dgvProgramas["P_EMPRESA", dgvProgramas.RowCount - 1].Value = "*****Ver en detalle*****";
                            dgvProgramas["dgvp_vencimiento", dgvProgramas.RowCount - 1].Value = "*****Ver en detalle*****";

                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(82, 82, 82);
                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(102, 102, 102);
                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.ForeColor = Color.White;


                            dgvProgramas.Rows.Add();

                            dgvProgramas["FOLIO_REQ", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["FOLIO_REQ"]);
                            dgvProgramas["FOLIO_CARGO_MSP", dgvProgramas.RowCount - 1].Value = sdr["FOLIO_MICROSIP"].ToString();
                            dgvProgramas["dgvp_fecha", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(Convert.ToString(sdr["FECHA_CARGO"])).ToString("dd/MM/yyyy");
                            dgvProgramas["dgvp_proveedor", dgvProgramas.RowCount - 1].Value = sdr["PROVEEDOR_NOMBRE"].ToString();
                            dgvProgramas["dgvp_vencimiento", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_VENCIMIENTO"].ToString()).ToString("dd/MM/yyyy");

                            // dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(reader["SALDO_CARGO"].ToString()).ToString("#,##0.00");
                            dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(sdr["IMPORTE_CAPTURISTA"].ToString()).ToString("#,##0.00");

                            dgvProgramas["dgvp_proveedor_id", dgvProgramas.RowCount - 1].Value = sdr["PROVEEDOR_ID"].ToString();
                            dgvProgramas["dgvp_proveedor_clave", dgvProgramas.RowCount - 1].Value = sdr["PROVEEDOR_CLAVE"].ToString();
                            dgvProgramas["P_EMPRESA", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["EMPRESA"]);
                            dgvProgramas["DOCTO_PP_DET_ID", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_PP_DET_ID"]);
                            dgvProgramas["DOCTO_PP_ID_2", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_PP_ID"]);
                            dgvProgramas["CLAVE_DEPTO_DET", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["Clave_Depto"]);
                            dgvProgramas["Requisicion_id", dgvProgramas.RowCount - 1].Value = Convert.ToString(sdr["Requisicion_id"]);

                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(153, 153, 153);
                            dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(163, 163, 163);

                        }

                    }
                    con.Desconectar();
                }

            }
            //dgvProgramas.Rows.Clear();
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        // ANTICIPO DE PAGO
        private void button1_Click(object sender, EventArgs e)
        {
            // F_ANTICIPOPAGOS FEE = new F_ANTICIPOPAGOS();
            // FEE.ShowDialog();

            string folio = "ANT";

            F_ANTICIPOS FA = new F_ANTICIPOS();
            FA.EMPRESA = NOMBRE_EMPESA;
            FA.USUARIO = usuarioLogueado.Usuario;
            if (FA.ShowDialog() == DialogResult.OK)
            {
                if (FA.anticipo.Length > 0)
                {
                    

                    dgvProgramas.Rows.Insert(0, FA.anticipo.Length);

                    for (int i = 0; i < FA.anticipo.Length; i++)
                    {
                        folio = "REQ";
                        /*while ((folio.Length + FA.anticipo[i].Folio.Length) < 9)
                        {
                            folio += "0";
                        }*/

                        folio += FA.anticipo[i].Folio;

                        dgvProgramas["dgvp_check", i].Value = true;
                        dgvProgramas["dgvp_folio", i].Value = folio;
                        dgvProgramas["dgvp_fecha", i].Value = FA.FECHA.ToString("dd/MM/yyyy");
                        dgvProgramas["dgvp_proveedor", i].Value = FA.anticipo[i].Proveedor_Nombre;
                        dgvProgramas["dgvp_vencimiento", i].Value = FA.FECHA.ToString("dd/MM/yyyy");
                        dgvProgramas["dgvp_importe", i].Value = FA.anticipo[i].TOTAL.ToString("#,##0.00");
                        dgvProgramas["dgvp_proveedor_id", i].Value = FA.anticipo[i].Proveedor_ID;
                        dgvProgramas["dgvp_proveedor_clave", i].Value = FA.anticipo[i].Proveedor_Clave;
                        dgvProgramas["Requisicion_id", i].Value = FA.anticipo[i].Requisicion_id;
                        dgvProgramas["P_EMPRESA", i].Value = FA.anticipo[i].Empresa;

                        dgvProgramas.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(100, 255, 100);
                        dgvProgramas.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromArgb(50, 200, 50);
                    }

                    
                }
                
            }
        }

        // CAMBIO EN EL TEXTO INICIA BUSQUEDA DEL PROVEEDOR
        private void txt_proveedores_TextChanged(object sender, EventArgs e)
        {
            /*if (txt_proveedores.Text.Trim() != "")
            {
                for (int r = 0; r < dgvProgramas.RowCount; r++)
                {
                    if (dgvProgramas.Rows[r].Cells["dgvp_proveedor"].Value.ToString().ToUpper().Contains(txt_proveedores.Text.Trim().ToUpper()))
                    {
                        dgvProgramas.Rows[r].Visible = true;
                    }
                    else
                    {
                        dgvProgramas.Rows[r].Visible = false;
                    }
                }
            }
            else
            {
                for (int r = 0; r < dgvProgramas.RowCount; r++)
                {
                    dgvProgramas.Rows[r].Visible = true;
                }
            }*/

        }

        // MOSTRAR LO SELECCIONADO / MOSTRAR TODO
        private void btn_mostrarseleccionado_Click(object sender, EventArgs e)
        {
            if (btn_mostrarseleccionado.Text == "Mostrar lo seleccionado")
            {
                for (int r = 0; r < dgvProgramas.RowCount; r++)
                {
                    if (Convert.ToBoolean(dgvProgramas.Rows[r].Cells["dgvp_check"].Value))
                    {
                        dgvProgramas.Rows[r].Visible = true;
                    }
                    else
                    {
                        dgvProgramas.Rows[r].Visible = false;
                    }
                }

                btn_mostrarseleccionado.Text = "Mostrar todo";
            }
            else
            {
                for (int r = 0; r < dgvProgramas.RowCount; r++)
                {
                    dgvProgramas.Rows[r].Visible = true;
                }

                btn_mostrarseleccionado.Text = "Mostrar lo seleccionado";
            }
        }





        // FINALIZAR
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                C_USUARIOS funcion = new C_USUARIOS();
                int _CantPagosSelec = 0;
                int _CantUsuarios = 0;
                C_USUARIOS[] usuarios = new C_USUARIOS[0];

                double _ImporteTotal = 0;

                // CONTAMOS TODOS LOS CARGOS SELECCIONADOS PARA LA PROGRAMACIÓN DE PAGO
                for (int i = 0; i < dgvProgramas.Rows.Count; i++)
                {
                    // if (dgvProgramas["dgvp_check", i].Value != null)
                    if (Convert.ToBoolean(dgvProgramas["dgvp_check", i].Value))
                    {
                        //checamos si es programacion o peticion
                        if (usuarioLogueado.Tesoreria == "S")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", i].Value)))
                            {
                                _CantPagosSelec++;
                                _ImporteTotal += Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                            }
                        }
                        else
                        {
                            _CantPagosSelec++;
                            _ImporteTotal += Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                        }

                    }
                }

                #region NO ES REQUISITANTE
                // if (usuarioLogueado.Requisitante == "N")
                // if ((usuarioLogueado.Requisitante == "N" || usuarioLogueado.Tesoreria == "S"))
                {
                    //CONTAMOS TODOS LOS USUARIOS QUE TENDRAN QUE AUTORIZAR LA PROGRAMACION
                    /* for (int i = 0; i < dgvUsers.Rows.Count; i++)
                     {
                         // if (dgvProgramas["dgvp_check", i].Value != null)
                         if (Convert.ToBoolean(dgvUsers["checkUsuario", i].Value))
                         {
                             Array.Resize(ref usuarios, usuarios.Length + 1);
                             usuarios[usuarios.Length - 1] = new C_USUARIOS();

                             usuarios[usuarios.Length - 1].Usuario_id = Convert.ToInt32(dgvUsers["USUARIO_ID", i].Value);
                             usuarios[usuarios.Length - 1].Nombre = dgvUsers["NOMBRE", i].Value.ToString();

                             _CantUsuarios++;
                         }
                     }*/

                    string depto = "";

                    for (int i = 0; i < dgvProgramas.Rows.Count; i++)
                    {
                        // if (dgvProgramas["dgvp_check", i].Value != null)
                        if (Convert.ToBoolean(dgvProgramas["dgvp_check", i].Value))
                        {
                            if (depto == "")
                            {
                                depto = Convert.ToString(dgvProgramas["CLAVE_DEPTO_DET", i].Value);

                                if (depto != "")
                                {
                                    i++;

                                    if (i > dgvProgramas.RowCount-1)
                                        break;
                                }
                            }

                            if (depto != Convert.ToString(dgvProgramas["CLAVE_DEPTO_DET", i].Value) && !string.IsNullOrEmpty(Convert.ToString(dgvProgramas["CLAVE_DEPTO_DET", i].Value)))
                            {
                                MessageBox.Show("Solo se puede hacer una programación de pago por departamento.\nDepartamentos detectados:\n"
                                    + depto + "\n, Depertamento diferente:" + Convert.ToString(dgvProgramas["CLAVE_DEPTO_DET", i].Value), "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    if (depto != "")
                    {
                        F_USUARIOS_AUTORIZADORES ua = new F_USUARIOS_AUTORIZADORES();
                        ua.DEPTO = depto;
                        ua.ShowDialog();

                        usuarios = ua.usuarios;

                        if (ua.usuarios.Length <= 0)
                        {
                            MessageBox.Show("No se selecciono ningun usuario para autorizar los pagos", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se selecciono ninguna petición de pago", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // VALIDAMOS QUE AL MENOS UN CARGO SE HAYA SELECCIONADO
                    if (MODIF == "S" && _CantPagosSelec <= 0)
                    {
                        if (MessageBox.Show("¿Modificar usuarios?", "Mensaje de la aplicación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return;

                        C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                        C_ConexionSQL conexion_sql = new C_ConexionSQL();
                        SqlTransaction transaction;
                        if (registros.LeerRegistros(false))
                        {
                            if (conexion_sql.ConectarSQL())
                            {
                                transaction = conexion_sql.SC.BeginTransaction();

                                try
                                {

                                    #region MODIFICAMOS LOS USUARIOS
                                    SqlCommand sc;
                                    SqlDataReader sdr;
                                    //buscamos primero si hay para actualizar

                                    string select = "SELECT * FROM P_DOCTOS_PR_DET WHERE DOCTO_PR_ID = " + DOCTO_PR_ID;
                                    sc = new SqlCommand(select, conexion_sql.SC, transaction);
                                    sdr = sc.ExecuteReader();
                                    while (sdr.Read())
                                    {
                                        int docto_pr_det = Convert.ToInt32(Convert.ToString(sdr["DOCTO_PR_DET_ID"]));

                                        //checamos los usuarios seleccionados y buscamos en la tabla P_AUT_DOCTOS_PR
                                        for (int j = 0; j < usuarios.Length; j++)
                                        {
                                            string s = "SELECT * FROM P_AUT_DOCTOS_PR ";
                                            s += " WHERE DOCTO_PR_DET_ID = " + docto_pr_det;
                                            s += " AND USUARIO_ID = " + usuarios[j].Usuario_id;

                                            bool existe = false;
                                            SqlCommand user = new SqlCommand(s, conexion_sql.SC, transaction);
                                            SqlDataReader use = user.ExecuteReader();
                                            while (use.Read())
                                                existe = true;

                                            //si no existe se inserta
                                            if (!existe)
                                            {
                                                string upd = "INSERT INTO P_AUT_DOCTOS_PR (";
                                                upd += " DOCTO_PR_DET_ID,";
                                                upd += " USUARIO_ID,";
                                                upd += " ESTATUS";
                                                upd += " ) VALUES (";
                                                upd += " @DOCTO_PR_DET_ID,";
                                                upd += " @USUARIO_ID,";
                                                upd += " @ESTATUS)";
                                                SqlCommand upda = new SqlCommand(upd, conexion_sql.SC, transaction);
                                                upda.Parameters.Add("@DOCTO_PR_DET_ID", SqlDbType.Int).Value = docto_pr_det;
                                                upda.Parameters.Add("@USUARIO_ID", SqlDbType.Int).Value = usuarios[j].Usuario_id;
                                                upda.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = "C";

                                                upda.ExecuteNonQuery();
                                            }
                                        }
                                    }

                                    #endregion

                                    transaction.Commit();
                                    dgvProgramas.Rows.Clear();

                                    MODIF = "EXITO";

                                    Close();
                                    MessageBox.Show("Modificacion de usuarios correcta", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    else if (_CantPagosSelec > 0)
                    {
                        if (MODIF == "S")
                        {
                            if (MessageBox.Show("¿Desea guardar los cambios realizados?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                DateTime _FechaPgo = Convert.ToDateTime(FECHA_PAGO_MODIF);
                                if (MessageBox.Show("¿Desea usar la fecha " + _FechaPgo.ToString("dd/MM/yyyy") + "?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                {
                                    F_FECHA_PAGO _FFP = new F_FECHA_PAGO();
                                    _FFP.ShowDialog();
                                    if (_FFP._FechaAsignada)
                                    {
                                        _FechaPgo = _FFP._FechaPago;

                                    }
                                }

                                C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                                C_ConexionSQL conexion_sql = new C_ConexionSQL();
                                SqlTransaction transaction;
                                SqlCommand cmd;
                                SqlDataReader read;

                                if (registros.LeerRegistros(false))
                                {
                                    if (conexion_sql.ConectarSQL())
                                    {
                                        transaction = conexion_sql.SC.BeginTransaction();
                                        try
                                        {

                                            #region MODIFICAMOS EL ENCABEZADO
                                            string consulta = "",
                                               ESTATUS = "A";

                                            string update = "UPDATE P_DOCTOS_PR SET ";
                                            update += " FECHA_PAGO = @FECHA_PAGO, ";
                                            update += " ESTATUS = @ESTATUS, ";
                                            update += " USUARIO_MODIF = @USUARIO_MODIF, ";
                                            update += " FECHA_HORA_MODIF = @FECHA_HORA_MODIF, ";
                                            //update += " EMPRESA = @EMPRESA, ";
                                            update += " IMPORTE_PAGOS = @IMPORTE_PAGOS ";
                                            update += " WHERE DOCTO_PR_ID = @DOCTO_PR_ID";

                                            cmd = new SqlCommand(update, conexion_sql.SC, transaction);
                                            cmd.Parameters.Add("@FECHA_PAGO", SqlDbType.Date).Value = _FechaPgo;
                                            cmd.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = ESTATUS;
                                            cmd.Parameters.Add("@USUARIO_MODIF", SqlDbType.VarChar).Value = usuarioLogueado.Usuario;
                                            cmd.Parameters.Add("@FECHA_HORA_MODIF", SqlDbType.DateTime).Value = DateTime.Now;
                                            //cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = NOMBRE_EMPESA;
                                            cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = _ImporteTotal;
                                            cmd.Parameters.Add("@DOCTO_PR_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                            cmd.ExecuteNonQuery();
                                            #endregion

                                            #region GENERAMOS EL DETALLE
                                            for (int i = 0; i < dgvProgramas.Rows.Count; i++)
                                            {
                                                // if (dgvProgramas["dgvp_check", i].Value != null)
                                                if (Convert.ToBoolean(dgvProgramas["dgvp_check", i].Value))
                                                {
                                                    bool folio_existente = false;

                                                    consulta = "SELECT * FROM P_DOCTOS_PR_DET " +
                                                              " WHERE DOCTO_PR_ID =  @DOCTO_PR_ID  " +
                                                              "   AND FOLIO_MICROSIP = @FOLIO_MICROSIP " +
                                                              "   AND PROVEEDOR_ID = @PROVEEDOR_ID " +
                                                              "   AND TIPO = @TIPO " +
                                                    "   AND EMPRESA = @EMPRESA";

                                                    cmd = new SqlCommand(consulta, conexion_sql.SC, transaction);
                                                    cmd.Parameters.Add("@DOCTO_PR_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                    cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_folio", i].Value);
                                                    cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                    cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = 'P';
                                                    cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);

                                                    read = cmd.ExecuteReader();
                                                    while (read.Read())
                                                    {
                                                        folio_existente = true;
                                                    }
                                                    read.Close();
                                                    cmd.Dispose();


                                                    if (!folio_existente)
                                                    {
                                                        #region INSERTAMOS EN DOCTOS_PR_DET
                                                        consulta = "INSERT INTO P_DOCTOS_PR_DET" +
                                                                   "(" +
                                                                   "       DOCTO_PR_ID, " +
                                                                   "       FOLIO_MICROSIP, " +
                                                                   "       FECHA_CARGO, " +
                                                                   "       PROVEEDOR_ID, " +
                                                                   "       PROVEEDOR_CLAVE, " +
                                                                   "       PROVEEDOR_NOMBRE, " +
                                                                   "       FECHA_VENCIMIENTO, " +
                                                                   "       IMPORTE_PAGOS, " +
                                                                   "       TIPO, EMPRESA " +
                                                                   ") " +
                                                                   "VALUES" +
                                                                   "(" +
                                                                   "       @DOCTO_PR_ID, " +
                                                                   "       @FOLIO_MICROSIP, " +
                                                                   "       @FECHA_CARGO, " +
                                                                   "       @PROVEEDOR_ID, " +
                                                                   "       @PROVEEDOR_CLAVE, " +
                                                                   "       @PROVEEDOR_NOMBRE, " +
                                                                   "       @FECHA_VENCIMIENTO, " +
                                                                   "       @IMPORTE_PAGOS, " +
                                                                   "       @TIPO, @EMPRESA" +
                                                                   ")";

                                                        cmd = new SqlCommand(consulta, conexion_sql.SC, transaction);
                                                        cmd.Parameters.Add("@DOCTO_PR_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                        cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_folio", i].Value);
                                                        cmd.Parameters.Add("@FECHA_CARGO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_fecha", i].Value);
                                                        cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                        cmd.Parameters.Add("@PROVEEDOR_CLAVE", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_proveedor_clave", i].Value);
                                                        cmd.Parameters.Add("@PROVEEDOR_NOMBRE", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_proveedor", i].Value);
                                                        cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_vencimiento", i].Value);
                                                        cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                        cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = (dgvProgramas.Rows[i].DefaultCellStyle.BackColor != Color.FromArgb(100, 255, 100) ? "P" : "A");
                                                        cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();

                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region ACTUALIZAMOS EN DOCTOS_PR_DET

                                                        consulta = "UPDATE P_DOCTOS_PR_DET SET " +
                                                                   "       IMPORTE_PAGOS =  @IMPORTE_PAGOS, " +
                                                                   "       FECHA_VENCIMIENTO = @FECHA_VENCIMIENTO " +
                                                                   " WHERE DOCTO_PR_ID = @DOCTO_PR_ID " +
                                                                   "   AND FOLIO_MICROSIP = @FOLIO_MICROSIP " +
                                                                   "   AND PROVEEDOR_ID = @PROVEEDOR_ID " +
                                                                   "   AND TIPO = @TIPO AND EMPRESA = @EMPRESA ";

                                                        cmd = new SqlCommand(consulta, conexion_sql.SC, transaction);
                                                        cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                        cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_vencimiento", i].Value);
                                                        cmd.Parameters.Add("@DOCTO_PR_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                        cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_folio", i].Value);
                                                        cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                        cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = 'P';
                                                        cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();

                                                        #endregion
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region MODIFICAMOS LOS USUARIOS
                                            SqlCommand sc;
                                            SqlDataReader sdr;
                                            //buscamos primero si hay para actualizar

                                            string select = "SELECT * FROM P_DOCTOS_PR_DET WHERE DOCTO_PR_ID = " + DOCTO_PR_ID;
                                            sc = new SqlCommand(select, conexion_sql.SC, transaction);
                                            sdr = sc.ExecuteReader();
                                            while (sdr.Read())
                                            {
                                                int docto_pr_det = Convert.ToInt32(Convert.ToString(sdr["DOCTO_PR_DET_ID"]));

                                                //checamos los usuarios seleccionados y buscamos en la tabla P_AUT_DOCTOS_PR
                                                for (int j = 0; j < usuarios.Length; j++)
                                                {
                                                    string s = "SELECT * FROM P_AUT_DOCTOS_PR ";
                                                    s += " WHERE DOCTO_PR_DET_ID = " + docto_pr_det;
                                                    s += " AND USUARIO_ID = " + usuarios[j].Usuario_id;

                                                    bool existe = false;
                                                    SqlCommand user = new SqlCommand(s, conexion_sql.SC, transaction);
                                                    SqlDataReader use = user.ExecuteReader();
                                                    while (use.Read())
                                                        existe = true;

                                                    //si no existe se inserta
                                                    if (!existe)
                                                    {
                                                        string upd = "INSERT INTO P_AUT_DOCTOS_PR (";
                                                        upd += " DOCTO_PR_DET_ID,";
                                                        upd += " USUARIO_ID,";
                                                        upd += " ESTATUS";
                                                        upd += " ) VALUES (";
                                                        upd += " @DOCTO_PR_DET_ID,";
                                                        upd += " @USUARIO_ID,";
                                                        upd += " @ESTATUS)";
                                                        SqlCommand upda = new SqlCommand(upd, conexion_sql.SC, transaction);
                                                        upda.Parameters.Add("@DOCTO_PR_DET_ID", SqlDbType.Int).Value = docto_pr_det;
                                                        upda.Parameters.Add("@USUARIO_ID", SqlDbType.Int).Value = usuarios[j].Usuario_id;
                                                        upda.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = "C";

                                                        upda.ExecuteNonQuery();
                                                    }
                                                }
                                            }

                                            #endregion

                                            transaction.Commit();

                                            MessageBox.Show("Programación del folio " + FOLIO_MODIF + " realizada con exito", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                            dgvProgramas.Rows.Clear();

                                            MODIF = "EXITO";

                                            Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            transaction.Rollback();
                                        }

                                        conexion_sql.Desconectar();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("¿Finalizar programación y enviar para su autorización?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                #region SI NO ES REQUISITANTE
                                //if ((usuarioLogueado.Requisitante == "N" || usuarioLogueado.Tesoreria == "S"))
                                {
                                    F_FECHA_PAGO _FFP = new F_FECHA_PAGO();
                                    _FFP.ShowDialog();

                                    if (_FFP._FechaAsignada)
                                    {
                                        DateTime _FechaPgo = _FFP._FechaPago;

                                        // Datós para el encabezado
                                        string consulta = "",
                                               msj_local = "",
                                               DOCTO_PR_ID = "", // Usar Gen_Docto
                                               FOLIO = "", // Serie de base de datos y consecutivo
                                               ESTATUS = "A",
                                               USUARIO_CREADOR = usuarioLogueado.Usuario;

                                        DateTime FECHA_HORA_CREACION = DateTime.Now,
                                               FECHA_PAGO = _FechaPgo;

                                        C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                                        C_ConexionSQL conexion_fb = new C_ConexionSQL();
                                        SqlTransaction transaction;
                                        SqlCommand cmd;
                                        SqlDataReader read;

                                        if (registros.LeerRegistros(false))
                                        {
                                            if (conexion_fb.ConectarSQL())
                                            {
                                                transaction = conexion_fb.SC.BeginTransaction();

                                                try
                                                {
                                                    UtileriasC utilerias = new UtileriasC();

                                                    // GENERAMOS EL ENCABEZADO
                                                    //DOCTO_PR_ID = Convert.ToString(utilerias.GEN_DOCTO_ID(conexion_fb, transaction));
                                                    FOLIO = utilerias.OBTENER_FOLIO(conexion_fb, transaction, out msj_local);

                                                    #region INSERTAMOS EN DOCTOS_PR
                                                    consulta = "INSERT INTO P_DOCTOS_PR" +
                                                                "(" +
                                                                "       FOLIO, " +
                                                                "       FECHA_PAGO, " +
                                                                "       ESTATUS, " +
                                                                "       USUARIO_CREADOR, " +
                                                                "       FECHA_HORA_CREACION, ";
                                                    if (usuarioLogueado.U_ROL != "U")
                                                    {
                                                        consulta += " IMPORTE_CAPTURISTA, ";
                                                        consulta += " ESTATUS_PROC, ";
                                                    }
                                                    // "       EMPRESA, " +
                                                    consulta += "       IMPORTE_PAGOS,NIVEL " +
                                                              ") " +
                                                              "VALUES" +
                                                              "(" +
                                                              "       @FOLIO, " +
                                                              "       @FECHA_PAGO, " +
                                                              "       @ESTATUS, " +
                                                              "       @USUARIO_CREADOR, " +
                                                              "       @FECHA_HORA_CREACION, ";
                                                    if (usuarioLogueado.U_ROL != "U")
                                                    {
                                                        consulta += " @IMPORTE_CAPTURISTA, ";
                                                        consulta += " @ESTATUS_PROC, ";
                                                    }

                                                    //"       @EMPRESA, " +
                                                    consulta += "       @IMPORTE_PAGOS , @NIVEL " +
                                                                ");  SELECT @@IDENTITY AS 'Identity';";
                                                    cmd = new SqlCommand(consulta, conexion_fb.SC, transaction);
                                                    cmd.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = FOLIO;
                                                    cmd.Parameters.Add("@FECHA_PAGO", SqlDbType.Date).Value = FECHA_PAGO;
                                                    cmd.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = ESTATUS;
                                                    cmd.Parameters.Add("@USUARIO_CREADOR", SqlDbType.VarChar).Value = USUARIO_CREADOR;
                                                    cmd.Parameters.Add("@FECHA_HORA_CREACION", SqlDbType.DateTime).Value = FECHA_HORA_CREACION;
                                                    if (usuarioLogueado.U_ROL != "U")
                                                    {
                                                        cmd.Parameters.Add("@IMPORTE_CAPTURISTA", SqlDbType.Float).Value = _ImporteTotal;
                                                        cmd.Parameters.Add("@ESTATUS_PROC", SqlDbType.VarChar).Value = "E";
                                                    }
                                                    cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = _ImporteTotal;
                                                    cmd.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = funcion.NIVEL_DEPTO(depto);
                                                    SqlDataReader sdr = cmd.ExecuteReader();
                                                    while (sdr.Read())
                                                        DOCTO_PR_ID = Convert.ToString(sdr["Identity"]);
                                                    sdr.Close();
                                                    cmd.Dispose();

                                                    #endregion

                                                    // GENERAMOS EL DETALLE
                                                    for (int i = 0; i < dgvProgramas.Rows.Count; i++)
                                                    {
                                                        // if (dgvProgramas["dgvp_check", i].Value != null)
                                                        if (Convert.ToBoolean(dgvProgramas["dgvp_check", i].Value) && !string.IsNullOrEmpty(Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", i].Value)))
                                                        {
                                                            bool folio_existente = false;

                                                            consulta = "SELECT * FROM P_DOCTOS_PR_DET " +
                                                                       " WHERE DOCTO_PR_ID = @DOCTO_PR_ID  " +
                                                                       "   AND FOLIO_MICROSIP = @FOLIO_MICROSIP " +
                                                                       "   AND PROVEEDOR_ID = @PROVEEDOR_ID " +
                                                                       "   AND TIPO = @TIPO AND EMPRESA = @EMPRESA ";

                                                            cmd = new SqlCommand(consulta, conexion_fb.SC, transaction);
                                                            cmd.Parameters.Add("@DOCTO_PR_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                            cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["FOLIO_CARGO_MSP", i].Value);
                                                            cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                            if (Convert.ToString(dgvProgramas["FOLIO_CARGO_MSP", i].Value).Contains("REQ"))
                                                                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = 'A';
                                                            else
                                                                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = 'P';
                                                            cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);


                                                            read = cmd.ExecuteReader();
                                                            while (read.Read())
                                                            {
                                                                folio_existente = true;
                                                            }
                                                            read.Close();
                                                            cmd.Dispose();


                                                            if (!folio_existente)
                                                            {
                                                                #region INSERTAMOS EN DOCTOS_PR_DET

                                                                consulta = "INSERT INTO P_DOCTOS_PR_DET" +
                                                                          "(" +
                                                                          "       DOCTO_PR_ID, " +
                                                                          "       FOLIO_MICROSIP, " +
                                                                          "       FECHA_CARGO, " +
                                                                          "       PROVEEDOR_ID, " +
                                                                          "       PROVEEDOR_CLAVE, " +
                                                                          "       PROVEEDOR_NOMBRE, " +
                                                                          "       FECHA_VENCIMIENTO, " +
                                                                          "       IMPORTE_PAGOS, ";
                                                                //if (usuarioLogueado.U_ROL != "U")
                                                                {
                                                                    consulta += " IMPORTE_CAPTURISTA, ";
                                                                    consulta += " ESTATUS, ";
                                                                }
                                                                consulta += "       TIPO, EMPRESA, REQUISICION_ID, DOCTO_PP_ID,DOCTO_PP_DET_ID " +
                                                                          ") " +
                                                                          "VALUES" +
                                                                          "(" +
                                                                          "       @DOCTO_PR_ID , " +
                                                                          "       @FOLIO_MICROSIP, " +
                                                                          "       @FECHA_CARGO, " +
                                                                          "       @PROVEEDOR_ID, " +
                                                                          "       @PROVEEDOR_CLAVE, " +
                                                                          "       @PROVEEDOR_NOMBRE, " +
                                                                          "       @FECHA_VENCIMIENTO, " +
                                                                          "       @IMPORTE_PAGOS, ";
                                                                //if (usuarioLogueado.U_ROL != "U")
                                                                {
                                                                    consulta += " @IMPORTE_CAPTURISTA, ";
                                                                    consulta += " @ESTATUS, ";
                                                                }
                                                                consulta += "       @TIPO, @EMPRESA, @REQUISICION_ID, @DOCTO_PP_ID, @DOCTO_PP_DET_ID " +
                                                                            ")";

                                                                string fecha1, fecha2;
                                                                fecha1 = dgvProgramas["dgvp_fecha", i].Value.ToString();
                                                                fecha2 = dgvProgramas["dgvp_vencimiento", i].Value.ToString();

                                                                DateTime _fechaCargo = Convert.ToDateTime(fecha1);
                                                                DateTime _fechaVencimiento = Convert.ToDateTime(fecha2);

                                                                cmd = new SqlCommand(consulta, conexion_fb.SC, transaction);
                                                                cmd.Parameters.Add("@DOCTO_PR_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                                cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["FOLIO_CARGO_MSP", i].Value);
                                                                cmd.Parameters.Add("@FECHA_CARGO", SqlDbType.Date).Value = _fechaCargo; //Convert.ToDateTime(dgvProgramas["dgvp_fecha", i].Value);
                                                                cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                                cmd.Parameters.Add("@PROVEEDOR_CLAVE", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_proveedor_clave", i].Value);
                                                                cmd.Parameters.Add("@PROVEEDOR_NOMBRE", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_proveedor", i].Value);
                                                                cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = _fechaVencimiento;//Convert.ToDateTime(dgvProgramas["dgvp_vencimiento", i].Value);
                                                                cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                                if (usuarioLogueado.U_ROL != "U")
                                                                {
                                                                    cmd.Parameters.Add("@IMPORTE_CAPTURISTA", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                                    cmd.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = "C";
                                                                }
                                                                if(Convert.ToString(dgvProgramas["FOLIO_CARGO_MSP", i].Value).Contains("REQ"))
                                                                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "A";
                                                                else
                                                                    cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "P";
                                                                cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);

                                                                cmd.Parameters.Add("@REQUISICION_ID", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["REQUISICION_ID", i].Value);
                                                                cmd.Parameters.Add("@DOCTO_PP_ID", SqlDbType.Int).Value = Convert.ToInt32(Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", i].Value));
                                                                cmd.Parameters.Add("@DOCTO_PP_DET_ID", SqlDbType.Int).Value = Convert.ToInt32(Convert.ToString(dgvProgramas["DOCTO_PP_DET_ID", i].Value));
                                                                cmd.ExecuteNonQuery();
                                                                cmd.Dispose();

                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                #region ACTUALIZAMOS EN DOCTOS_PR_DET

                                                                consulta = "UPDATE P_DOCTOS_PR_DET SET " +
                                                                           "       IMPORTE_PAGOS = (IMPORTE_PAGOS + @IMPORTE_PAGOS), " +
                                                                           "       FECHA_VENCIMIENTO = @FECHA_VENCIMIENTO " +
                                                                           " WHERE DOCTO_PR_ID = @DOCTO_PR_ID " +
                                                                           "   AND FOLIO_MICROSIP = @FOLIO_MICROSIP " +
                                                                           "   AND PROVEEDOR_ID = @PROVEEDOR_ID " +
                                                                           "   AND TIPO = @TIPO AND EMPRESA = @EMPRESA ";


                                                                cmd = new SqlCommand(consulta, conexion_fb.SC, transaction);
                                                                cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                                cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_vencimiento", i].Value);
                                                                cmd.Parameters.Add("@DOCTO_PR_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                                cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["FOLIO_CARGO_MSP", i].Value);
                                                                cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                                cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);
                                                                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = 'C';

                                                                cmd.ExecuteNonQuery();
                                                                cmd.Dispose();

                                                                #endregion
                                                            }

                                                            //se ponen en estatus T los detalles de las peticiones que se seleccionaro para ser programacion de pago
                                                            string update = "UPDATE P_DOCTOS_PP_DET SET ESTATUS = 'T' WHERE DOCTO_PP_DET_ID = " + Convert.ToInt32(Convert.ToString(dgvProgramas["DOCTO_PP_DET_ID", i].Value));
                                                            SqlCommand scc = new SqlCommand(update, conexion_fb.SC, transaction);
                                                            scc.ExecuteNonQuery();
                                                            scc.Dispose();
                                                        }
                                                    }


                                                    //INSERTAMOS EL DETALLE EN LA TABLA DE AUTORIZACIONES CON LOS USUARIOS
                                                    string query = "SELECT * FROM P_DOCTOS_PR_DET WHERE DOCTO_PR_ID = " + DOCTO_PR_ID;
                                                    SqlCommand sc = new SqlCommand(query, conexion_fb.SC, transaction);
                                                    sdr = sc.ExecuteReader();
                                                    while (sdr.Read())
                                                    {
                                                        for (int j = 0; j < usuarios.Length; j++)
                                                        {
                                                            string insert = "INSERT INTO P_AUT_DOCTOS_PR (";
                                                            insert += " DOCTO_PR_DET_ID,";
                                                            insert += " USUARIO_ID,";
                                                            insert += " ESTATUS";
                                                            insert += " ,NIVEL";
                                                            insert += " ) VALUES (";
                                                            insert += " @DOCTO_PR_DET_ID,";
                                                            insert += " @USUARIO_ID,";
                                                            insert += " @ESTATUS,@NIVEL)";
                                                            SqlCommand inser = new SqlCommand(insert, conexion_fb.SC, transaction);
                                                            inser.Parameters.Add("@DOCTO_PR_DET_ID", SqlDbType.Int).Value = Convert.ToInt32(Convert.ToString(sdr["DOCTO_PR_DET_ID"]));
                                                            inser.Parameters.Add("@USUARIO_ID", SqlDbType.Int).Value = usuarios[j].Usuario_id;
                                                            inser.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = "C";
                                                            inser.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = funcion.NIVEL_DEPTO(depto);

                                                            inser.ExecuteNonQuery();
                                                        }


                                                    }

                                                    transaction.Commit();

                                                    MessageBox.Show("Programación generada para autorización", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                    dgvProgramas.Rows.Clear();

                                                    Close();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    transaction.Rollback();
                                                }

                                                conexion_fb.Desconectar();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se asignó la fecha de pago de la programación.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    else
                    {
                        if (_CantPagosSelec == 0)
                            MessageBox.Show("Necesita seleccionar al menos un cargo para la programación de pagos.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                            MessageBox.Show("Necesita seleccionar al menos un usuario para la programación de pagos.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                #endregion

            }catch(Exception ex)
            {
                MessageBox.Show("Error inesperado\n" + ex.Message,"Mensaje de aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

        }

        // CANCELAR
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }





        private void chBProximoVencimeito_CheckedChanged(object sender, EventArgs e)
        {
            dTPProximo.Enabled = chBProximoVencimeito.Checked;
        }

        private bool AgregarPendientesPorVencer(DateTime FechaFin, DataGridView DGV)
        {
            string msg_local = "";

            bool _exito = false;

            try
            {
                string DATE_SEARCH = FechaFin.ToString("dd.MM.yyyy");
                string SELECT = "";

                /* SELECT = "SELECT ";
                SELECT += "      DCP.FOLIO, ";
                SELECT += "      DCP.FECHA, ";
                SELECT += "      P.NOMBRE, ";
                SELECT += "      P.PROVEEDOR_ID, ";
                SELECT += "      CP.CLAVE_PROV, ";
                SELECT += "      VCCP.FECHA_VENCIMIENTO, ";
                SELECT += "      (SELECT SCCP.SALDO_CARGO FROM SALDO_CARGO_CP_S(DCP.DOCTO_CP_ID, '" + DATE_SEARCH + "', 0) SCCP) ";
                SELECT += " FROM DOCTOS_CP AS DCP ";
                SELECT += "INNER JOIN PROVEEDORES AS P ON(P.PROVEEDOR_ID = DCP.PROVEEDOR_ID) ";
                SELECT += " LEFT JOIN CLAVES_PROVEEDORES AS CP ON (P.PROVEEDOR_ID=CP.PROVEEDOR_ID) ";
                SELECT += "INNER JOIN VENCIMIENTOS_CARGOS_CP AS VCCP ON(VCCP.DOCTO_CP_ID = DCP.DOCTO_CP_ID) ";
                SELECT += "WHERE (SELECT SCCP.SALDO_CARGO FROM SALDO_CARGO_CP_S(DCP.DOCTO_CP_ID, '" + DATE_SEARCH + "', 0) SCCP) > 0 " +
                          "  AND VCCP.FECHA_VENCIMIENTO < '" + DATE_SEARCH + "' " +
                          "ORDER BY VCCP.FECHA_VENCIMIENTO ASC"; // */

                SELECT = "SELECT ";
                SELECT += "       DCP.FOLIO, ";
                SELECT += "       DCP.FECHA, ";
                SELECT += "       P.NOMBRE, ";
                SELECT += "       P.PROVEEDOR_ID, ";
                SELECT += "       CP.CLAVE_PROV, ";
                SELECT += "       VCCP.FECHA_VENCIMIENTO, ";
                SELECT += "       VCCPP.SALDO ";
                SELECT += "  FROM DOCTOS_CP AS DCP ";
                SELECT += " INNER JOIN PROVEEDORES AS P ON(P.PROVEEDOR_ID = DCP.PROVEEDOR_ID) ";
                SELECT += "  LEFT JOIN CLAVES_PROVEEDORES AS CP ON (P.PROVEEDOR_ID=CP.PROVEEDOR_ID) ";
                SELECT += " INNER JOIN VENCIMIENTOS_CARGOS_CP AS VCCP ON(VCCP.DOCTO_CP_ID = DCP.DOCTO_CP_ID) ";
                SELECT += " INNER JOIN VENCIMIENTOS_CARGO_CP(DCP.docto_cp_id, '" + DateTime.Now.ToString("dd.MM.yyyy") + "', 0) AS VCCPP ON(VCCP.FECHA_VENCIMIENTO = VCCPP.FECHA_VENCIMIENTO) ";
                SELECT += " WHERE VCCPP.SALDO > 0 " +
                          // "   AND VCCP.FECHA_VENCIMIENTO <= '" + DATE_SEARCH + "'" +
                          "   AND VCCP.FECHA_VENCIMIENTO <= @FECHA_VENCIMIENTO" +
                          " ORDER BY VCCP.FECHA_VENCIMIENTO DESC";

                C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                C_CONEXIONFIREBIRD conexion_fb = new C_CONEXIONFIREBIRD();
                FbCommand cmd;
                FbDataReader reader;

                if (registros.LeerRegistros(false))
                {
                    if (conexion_fb.ConectarFB(DATABASE_NAME))
                    {
                        try
                        {
                            cmd = new FbCommand(SELECT, conexion_fb.FBC);
                            cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = FechaFin;
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string _folio = reader["FOLIO"].ToString();
                                    int _existe = 0;

                                    for (int i = 0; i < DGV.RowCount; i++)
                                    {
                                        string _auxFolio = Convert.ToString(DGV["dgvp_folio", i].Value);
                                        if (_auxFolio == _folio)
                                            _existe++;
                                    }

                                    if (_existe == 0)
                                    {
                                        object[] _filaNueva = new object[] { false, reader["FOLIO"].ToString(),
                                                                                   Convert.ToDateTime(reader["FECHA"].ToString()).ToString("dd/MM/yyyy"),
                                                                                   reader["NOMBRE"].ToString(),
                                                                                   Convert.ToDateTime(reader["FECHA_VENCIMIENTO"].ToString()).ToString("dd/MM/yyyy"),

                                                                                   // Convert.ToDouble(reader["SALDO_CARGO"].ToString()).ToString("#,##0.00"),
                                                                                   Convert.ToDouble(reader["SALDO"].ToString()).ToString("#,##0.00"),

                                                                                   0,
                                                                                   reader["PROVEEDOR_ID"].ToString(),
                                                                                   reader["CLAVE_PROV"].ToString() };

                                        DGV.Rows.Insert(0, _filaNueva);
                                        // DGV.Rows[0].DefaultCellStyle.BackColor = Color.Khaki;

                                        DGV.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 111);
                                        DGV.Rows[0].DefaultCellStyle.SelectionBackColor = Color.FromArgb(191, 182, 0);
                                    }
                                }
                            }
                            reader.Close();
                            cmd.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No fue posible obtener los cargos por vencer de Microsip.\n\n" + ex.ToString(), "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        conexion_fb.Desconectar();
                    }
                }
            }
            catch (Exception Ex)
            {
                msg_local = Ex.Message;
            }

            return _exito;
        }

        private void dgvProgramas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Color fondo = dgvProgramas.CurrentRow.DefaultCellStyle.SelectionBackColor;

            if ((e.ColumnIndex == 0) && (fondo != Color.Red) && (fondo != Color.Green) && (fondo != Color.FromArgb(50, 200, 50)))
            {
                if (Convert.ToBoolean(dgvProgramas.CurrentRow.Cells["dgvp_check"].EditedFormattedValue))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dgvProgramas["DOCTO_PP_ID", e.RowIndex].Value)))
                    {
                        for (int i = 0; i < dgvProgramas.RowCount; i++)
                        {
                            if(Convert.ToString(dgvProgramas["DOCTO_PP_ID", e.RowIndex].Value) == Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", i].Value))
                            {
                                dgvProgramas["dgvp_check", i].Value = true;
                            }
                        }
                    }


                   // dgvProgramas.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                   // dgvProgramas.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Gray;
                }
                else
                {
                    //dgvProgramas.CurrentRow.DefaultCellStyle.BackColor = Color.Empty;
                    //dgvProgramas.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Empty;

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvProgramas["DOCTO_PP_ID", e.RowIndex].Value)))
                    {
                        for (int i = 0; i < dgvProgramas.RowCount; i++)
                        {
                            if (Convert.ToString(dgvProgramas["DOCTO_PP_ID", e.RowIndex].Value) == Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", i].Value))
                            {
                                dgvProgramas["dgvp_check", i].Value = false;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dgvProgramas.RowCount; i++)
                        {
                            if (Convert.ToString(dgvProgramas["DOCTO_PP_ID", i].Value) == Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", e.RowIndex].Value))
                            {
                                dgvProgramas["dgvp_check", i].Value = false;
                            }
                        }
                    }

                }
            }
        }

        private void dgvProgramas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Color fondo = dgvProgramas.CurrentRow.DefaultCellStyle.SelectionBackColor;

            if ((e.ColumnIndex == 0) && (fondo != Color.Red) && (fondo != Color.Green) && (fondo != Color.FromArgb(50, 200, 50)))
            {
                if (Convert.ToBoolean(dgvProgramas.CurrentRow.Cells["dgvp_check"].EditedFormattedValue))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dgvProgramas["DOCTO_PP_ID", e.RowIndex].Value)))
                    {
                        for (int i = 0; i < dgvProgramas.RowCount; i++)
                        {
                            if (Convert.ToString(dgvProgramas["DOCTO_PP_ID", e.RowIndex].Value) == Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", i].Value))
                            {
                                dgvProgramas["dgvp_check", i].Value = true;
                            }
                        }
                    }


                    // dgvProgramas.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                    // dgvProgramas.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Gray;
                }
                else
                {
                    //dgvProgramas.CurrentRow.DefaultCellStyle.BackColor = Color.Empty;
                    //dgvProgramas.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Empty;

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvProgramas["DOCTO_PP_ID", e.RowIndex].Value)))
                    {
                        for (int i = 0; i < dgvProgramas.RowCount; i++)
                        {
                            if (Convert.ToString(dgvProgramas["DOCTO_PP_ID", e.RowIndex].Value) == Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", i].Value))
                            {
                                dgvProgramas["dgvp_check", i].Value = false;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dgvProgramas.RowCount; i++)
                        {
                            if (Convert.ToString(dgvProgramas["DOCTO_PP_ID", i].Value) == Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", e.RowIndex].Value))
                            {
                                dgvProgramas["dgvp_check", i].Value = false;
                            }
                        }
                    }

                }
            }
        }

        private void F_PROGRAMACIONPAGOS_Shown(object sender, EventArgs e)
        {
           /* if (!string.IsNullOrEmpty(MODIF))
            {
                btnSearchInMSP.PerformClick();
            }*/

            //Primero obtenemos el día actual
            DateTime date = DateTime.Now;

            //Asi obtenemos el primer dia del mes actual
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);

            //Y de la siguiente forma obtenemos el ultimo dia del mes
            //agregamos 1 mes al objeto anterior y restamos 1 día.
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
            dtpFecha.Value = oPrimerDiaDelMes;
            txtUser.Text = usuarioLogueado.Usuario;

            C_FUNCIONES fun = new C_FUNCIONES();

            fun.GET_USUARIOS(dgvUsers);
            fun.GET_DEP_PRIVI(cbDpto, "Departamento");
            fun.GET_DEP_PRIVI(cbPrivilegio, "Privilegio");

            //para saber si se abrio la ventana en modo modificador
            if(MODIF == "S")
            {
                bntAgregarUsuarios.Visible = true;

                splitContainer1.Panel2.Enabled = false;
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();

            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (txt_proveedores.Text.Trim() != "")
            {
                for (int r = 0; r < dgvProgramas.RowCount; r++)
                {
                    if (dgvProgramas.Rows[r].Cells["dgvp_proveedor"].Value.ToString().ToUpper().Contains(txt_proveedores.Text.Trim().ToUpper()))
                    {
                        dgvProgramas.Rows[r].Visible = true;
                    }
                    else
                    {
                        dgvProgramas.Rows[r].Visible = false;
                    }
                }
            }
            else
            {
                for (int r = 0; r < dgvProgramas.RowCount; r++)
                {
                    dgvProgramas.Rows[r].Visible = true;
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void cbFiltrarUsuario_CheckedChanged(object sender, EventArgs e)
        {
            if(cbFiltrarUsuario.Checked)
            {
                cbDpto.Enabled = true;
                cbPrivilegio.Enabled = true;
            }
            else
            {
                cbDpto.Enabled = false;
                cbPrivilegio.Enabled = false;
            }
        }

        private void cbDpto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFiltrarUsuario.Checked)
            {
                for (int i = 0; i < dgvUsers.RowCount; i++)
                    dgvUsers.Rows[i].Visible = true;

                for (int i = 0; i < dgvUsers.RowCount; i++)
                {
                    if (cbPrivilegio.Text == "Seleccionar")
                    {
                        if (dgvUsers["DEPARTAMENTO", i].Value.ToString() == cbDpto.Text)
                        {
                            dgvUsers.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgvUsers.Rows[i].Visible = false;
                        }
                    }
                    else
                    {
                        if (cbDpto.Text == "Seleccionar")
                        {
                            if (dgvUsers["PRIVILEGIO", i].Value.ToString() == cbPrivilegio.Text)
                            {
                                dgvUsers.Rows[i].Visible = true;
                            }
                            else
                            {
                                dgvUsers.Rows[i].Visible = false;
                            }
                        }
                        else
                        {
                            if (dgvUsers["PRIVILEGIO", i].Value.ToString() == cbPrivilegio.Text && dgvUsers["DEPARTAMENTO", i].Value.ToString() == cbDpto.Text)
                            {
                                dgvUsers.Rows[i].Visible = true;
                            }
                            else
                            {
                                dgvUsers.Rows[i].Visible = false;
                            }
                        }
                    }
                }

                if(cbDpto.Text == "Seleccionar" && cbPrivilegio.Text == "Seleccionar")
                {
                    for (int i = 0; i < dgvUsers.RowCount; i++)
                        dgvUsers.Rows[i].Visible = true;
                }
            }
        }

        private void cbPrivilegio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFiltrarUsuario.Checked)
            {
                for (int i = 0; i < dgvUsers.RowCount; i++)
                    dgvUsers.Rows[i].Visible = true;

                for (int i = 0; i < dgvUsers.RowCount; i++)
                {
                    if (cbDpto.Text == "Seleccionar")
                    {
                        if (dgvUsers["PRIVILEGIO", i].Value.ToString() == cbPrivilegio.Text)
                        {
                            dgvUsers.Rows[i].Visible = true;
                        }
                        else
                        {
                            dgvUsers.Rows[i].Visible = false;
                        }
                    }
                    else
                    {
                        if (cbPrivilegio.Text == "Seleccionar")
                        {
                            if (dgvUsers["DEPARTAMENTO", i].Value.ToString() == cbDpto.Text)
                            {
                                dgvUsers.Rows[i].Visible = true;
                            }
                            else
                            {
                                dgvUsers.Rows[i].Visible = false;
                            }
                        }
                        else
                        {
                            if (dgvUsers["PRIVILEGIO", i].Value.ToString() == cbPrivilegio.Text && dgvUsers["DEPARTAMENTO", i].Value.ToString() == cbDpto.Text)
                            {
                                dgvUsers.Rows[i].Visible = true;
                            }
                            else
                            {
                                dgvUsers.Rows[i].Visible = false;
                            }
                        }
                    }
                }

                if (cbDpto.Text == "Seleccionar" && cbPrivilegio.Text == "Seleccionar")
                {
                    for (int i = 0; i < dgvUsers.RowCount; i++)
                        dgvUsers.Rows[i].Visible = true;
                }
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


       
    }
}
