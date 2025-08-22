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
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Data.SqlClient;

namespace PagosIntermex
{
    public partial class F_PETICIONESPAGOS : Form
    {

        #region VARIABLES GLOBALES
        DataSet ds = new DataSet();
        private string DATABASE_NAME = "";

        private int ANTICIPO_COUNT = 0;

        public C_PAGOS[] pagosModif1;
        public string FECHA_PAGO_MODIF { get; set; }

        public C_USUARIOS usuarioLogueado;

        public C_EMPRESAS[] NOMBRE_EMPESA { set; get; }

        public string MODIF { get; set; }
        public int DOCTO_PR_ID { get; set; }
        public string FOLIO_MODIF { get; set; }
        #endregion

        class REQ_OC
        {
            public string FOLIO_REQ { get; set; }
            public string MSP_FOLIO { get; set; }
            public int REQUISICION_ID { get; set; }
        }

        public F_PETICIONESPAGOS()
        {
            InitializeComponent();
        }

        private void F_PETICIONESPAGOS_FormClosing(object sender, FormClosingEventArgs e)
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
        private void btnSearchInMSP_Click(object sender, EventArgs e)
        {
            dgvProgramas.Rows.Clear();
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);

            BuscaCargosMSP();
            
        }

        public void BuscaCargosMSP()
        {
            string DATE_SEARCH = dtpDateVenc.Value.ToString("dd.MM.yyyy");

            string SELECT = "SELECT ";
            SELECT += "       DCP.FOLIO FOLIO_CARGO, ";
            SELECT += "       DCP.FECHA, ";
            SELECT += "       P.NOMBRE, ";
            SELECT += "       P.PROVEEDOR_ID, ";
            SELECT += "       CP.CLAVE_PROV, ";
            SELECT += "       VCCP.FECHA_VENCIMIENTO, ";
            SELECT += "       (SELECT SCCP.SALDO_CARGO FROM SALDO_CARGO_CP_S(DCP.DOCTO_CP_ID,  '" + DATE_SEARCH + "', 0) SCCP) SALDO, ";
            SELECT += "  DM.FOLIO FOLIO_COMPRA, ";
            SELECT += "  DMM.tipo_docto TIPO_RECEP, ";
            SELECT += "  DMM.FOLIO FOLIO_RECEP, ";
            SELECT += "  DMMM.TIPO_DOCTO TIPO_OC, ";
            SELECT += "  DMMM.FOLIO FOLIO_OC ";
            SELECT += "  FROM DOCTOS_CP AS DCP ";
            SELECT += " JOIN DOCTOS_ENTRE_SIS DES ON DES.DOCTO_DEST_ID = DCP.DOCTO_CP_ID";
            SELECT += " JOIN DOCTOS_CM DM ON DM.DOCTO_CM_ID = DES.DOCTO_FTE_ID";
            SELECT += " LEFT JOIN DOCTOS_CM_LIGAS DML ON DML.DOCTO_CM_DEST_ID = DM.DOCTO_CM_ID";
            SELECT += " LEFT JOIN DOCTOS_CM DMM ON DMM.DOCTO_CM_ID = DML.DOCTO_CM_FTE_ID";
            SELECT += " LEFT JOIN DOCTOS_CM_LIGAS DMLL ON DMLL.DOCTO_CM_DEST_ID = DMM.DOCTO_CM_ID";
            SELECT += " LEFT JOIN DOCTOS_CM DMMM ON DMMM.DOCTO_CM_ID = DMLL.DOCTO_CM_FTE_ID";

            SELECT += " INNER JOIN PROVEEDORES AS P ON(P.PROVEEDOR_ID = DCP.PROVEEDOR_ID) ";
            SELECT += "  LEFT JOIN CLAVES_PROVEEDORES AS CP ON (P.PROVEEDOR_ID=CP.PROVEEDOR_ID) ";
            SELECT += " INNER JOIN VENCIMIENTOS_CARGOS_CP AS VCCP ON(VCCP.DOCTO_CP_ID = DCP.DOCTO_CP_ID) ";
            SELECT += " WHERE (SELECT SCCP.SALDO_CARGO FROM SALDO_CARGO_CP_S(DCP.DOCTO_CP_ID, '" + DATE_SEARCH + "', 0) SCCP) > 0  " +
                      "   AND VCCP.FECHA_VENCIMIENTO <= @FECHA_VENCIMIENTO" +
                      " AND DCP.FECHA <= @FECHA AND DMM.tipo_docto is not null " +
                      " ORDER BY VCCP.FECHA_VENCIMIENTO DESC";

            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

            C_CONEXIONFIREBIRD conexion_fb = new C_CONEXIONFIREBIRD();
            FbCommand cmd;
            FbDataReader reader;

            DataGridViewRow[] rows = new DataGridViewRow[0];
            C_FUNCIONES fun = new C_FUNCIONES();
            NOMBRE_EMPESA = fun.TraerEmpresas();

            if (registros.LeerRegistros(false))
            {
                pbEmpresas.Maximum = NOMBRE_EMPESA.Length + 1;
                pbEmpresas.Visible = true;
                txtNo.Visible = true;

                List<REQ_OC> lista = new List<REQ_OC>();
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string query = "select re.Folio FOLIO_REQ, dc.MSP_Folio MSP_FOLIO,  re.Requisicion_id REQUISICION_ID ";
                    query += " from DOCTOS_CM dc  ";
                    query += " join REQ_ENC re on re.Requisicion_id = dc.Requisicion_ID ";
                    query += " join USUARIOS u on u.Usuario_id = re.Usuario_creador_id ";
                    query += " where u.Usuario = '" + usuarioLogueado.Usuario + "'";

                    SqlCommand sc = new SqlCommand(query, con.SC);
                    SqlDataReader sdr = sc.ExecuteReader();
                    REQ_OC[] ro = new REQ_OC[0];
                    while (sdr.Read())
                    {
                        Array.Resize(ref ro, ro.Length + 1);
                        ro[ro.Length - 1] = new REQ_OC();

                        ro[ro.Length - 1].FOLIO_REQ = Convert.ToString(sdr["FOLIO_REQ"]);
                        ro[ro.Length - 1].MSP_FOLIO = Convert.ToString(sdr["MSP_FOLIO"]);
                        ro[ro.Length - 1].REQUISICION_ID = Convert.ToInt32(Convert.ToString(sdr["REQUISICION_ID"]));

                        lista.Add(ro[ro.Length - 1]);
                    }

                    if (pagosModif1 != null)
                    {
                        for (int k = 0; k < pagosModif1.Length; k++)
                        {
                            if (pagosModif1[k].FOLIO.Contains("REQ"))
                            {
                                dgvProgramas.Rows.Add();
                                dgvProgramas.Columns["FOLIO_REQ"].Visible = true;
                                // dgvProgramas["FOLIO_REQ", dgvProgramas.RowCount - 1].Value = pagosModif1[k].fo;
                                dgvProgramas["dgvp_folio", dgvProgramas.RowCount - 1].Value = pagosModif1[k].FOLIO;
                                dgvProgramas["dgvp_fecha", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(pagosModif1[k].FECHA).ToString("dd/MM/yyyy");
                                dgvProgramas["dgvp_proveedor", dgvProgramas.RowCount - 1].Value = pagosModif1[k].PROVEEDOR;
                                dgvProgramas["dgvp_vencimiento", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(pagosModif1[k].FECHA_VENC).ToString("dd/MM/yyyy");

                                // dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(reader["SALDO_CARGO"].ToString()).ToString("#,##0.00");
                                dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = pagosModif1[k].PAGO.ToString("#,##0.00");

                                dgvProgramas["dgvp_proveedor_id", dgvProgramas.RowCount - 1].Value = pagosModif1[k].PROVEEDOR_ID;
                                dgvProgramas["dgvp_proveedor_clave", dgvProgramas.RowCount - 1].Value = pagosModif1[k].PROVEEDOR_CLAVE;
                                dgvProgramas["P_EMPRESA", dgvProgramas.RowCount - 1].Value = pagosModif1[k].EMPRESA;
                                dgvProgramas["Requisicion_id", dgvProgramas.RowCount - 1].Value = pagosModif1[k].REQ_ID;

                                dgvProgramas["MODIFICACION", dgvProgramas.RowCount - 1].Value = "S";
                                dgvProgramas["dgvp_check", dgvProgramas.RowCount - 1].Value = 1;
                                dgvProgramas["dgvp_check", dgvProgramas.RowCount - 1].ReadOnly = true;

                                dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                                dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.Gray;


                            }
                        }
                    }



                    for (int h = 0; h < NOMBRE_EMPESA.Length; h++)
                    {
                        pbEmpresas.Value++;
                        this.Cursor = Cursors.WaitCursor;
                        txtNo.Text = "Empresa " + (h + 1).ToString() + " de " + (NOMBRE_EMPESA.Length).ToString() + " Empresas";
                        txtInfo.Text = "Conectando a la empresa: " + NOMBRE_EMPESA[h].NOMBRE_CORTO;
                        Refresh();
                        if (conexion_fb.ConectarFB(NOMBRE_EMPESA[h].NOMBRE_CORTO, false))
                        {
                            txtInfo.Text = "Buscando los cargos de la empresa: " + NOMBRE_EMPESA[h].NOMBRE_CORTO;
                            Refresh();

                            try
                            {
                                //ASIGNACION DE PARAMETROS
                                cmd = new FbCommand(SELECT, conexion_fb.FBC);
                                cmd.Parameters.Add("@FECHA_VENCIMIENTO", FbDbType.Date).Value = dtpDateVenc.Value;
                                cmd.Parameters.Add("@FECHA", FbDbType.Date).Value = dtpFecha.Value;



                                reader = cmd.ExecuteReader();

                               
                                

                                while (reader.Read())
                                {

                                    string orden_compra = Convert.ToString(reader["TIPO_RECEP"]) == "O" ? Convert.ToString(reader["FOLIO_RECEP"]) : Convert.ToString(reader["FOLIO_OC"]);

                                    //buscamos en el arreglo por si existe
                                    int index = lista.FindIndex(x => x.MSP_FOLIO == orden_compra);

                                    if (index > 0)
                                    {

                                        string folio_cargo = Convert.ToString(reader["FOLIO_CARGO"]);
                                        string nombre_proveedor = Convert.ToString(reader["NOMBRE"]);
                                        bool existe = false;
                                        #region verificamos que no se haya insertado en alguna programacion o peticion de pago el cargo
                                        string quer = "SELECT * FROM P_DOCTOS_PR_DET pr ";
                                        quer += " WHERE pr.FOLIO_MICROSIP = @FOLIOS ";
                                        quer += " AND PROVEEDOR_NOMBRE = @NOMBRES ";
                                        quer += " AND EMPRESA = @EMPRESAS ";
                                        quer += " AND ESTATUS != 'X'";
                                        SqlCommand sc2 = new SqlCommand(quer, con.SC);
                                        sc2.Parameters.Add("@FOLIOS", SqlDbType.VarChar).Value = folio_cargo;
                                        sc2.Parameters.Add("@NOMBRES", SqlDbType.VarChar).Value = nombre_proveedor;
                                        sc2.Parameters.Add("@EMPRESAS", SqlDbType.VarChar).Value = NOMBRE_EMPESA[h].NOMBRE_CORTO;

                                        SqlDataReader sdr2 = sc2.ExecuteReader();
                                        while (sdr2.Read())
                                            existe = true;

                                        quer = "SELECT * FROM P_DOCTOS_PP_DET pr ";
                                        quer += " WHERE pr.FOLIO_MICROSIP = @FOLIOS ";
                                        quer += " AND PROVEEDOR_NOMBRE = @NOMBRES ";
                                        quer += " AND EMPRESA = @EMPRESAS ";
                                        quer += " AND ESTATUS != 'X' ";
                                        sc2 = new SqlCommand(quer, con.SC);
                                        sc2.Parameters.Add("@FOLIOS", SqlDbType.VarChar).Value = folio_cargo;
                                        sc2.Parameters.Add("@NOMBRES", SqlDbType.VarChar).Value = nombre_proveedor;
                                        sc2.Parameters.Add("@EMPRESAS", SqlDbType.VarChar).Value = NOMBRE_EMPESA[h].NOMBRE_CORTO;

                                        sdr2 = sc2.ExecuteReader();
                                        while (sdr2.Read())
                                            existe = true;

                                        if (MODIF == "S")
                                        {
                                            if(pagosModif1 != null)
                                            {
                                                for (int i = 0; i < pagosModif1.Length; i++)
                                                {
                                                    if(folio_cargo == pagosModif1[i].FOLIO)
                                                    {
                                                        existe = false;
                                                        break;
                                                    }
                                                }
                                            }

                                            
                                        }

                                        #endregion

                                        if (!existe)
                                        {
                                            dgvProgramas.Rows.Add();
                                            dgvProgramas.Columns["FOLIO_REQ"].Visible = true;

                                            dgvProgramas["FOLIO_REQ", dgvProgramas.RowCount - 1].Value = lista[index].FOLIO_REQ;
                                            dgvProgramas["dgvp_folio", dgvProgramas.RowCount - 1].Value = folio_cargo;
                                            dgvProgramas["dgvp_fecha", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(reader["FECHA"].ToString()).ToString("dd/MM/yyyy");
                                            dgvProgramas["dgvp_proveedor", dgvProgramas.RowCount - 1].Value = nombre_proveedor;
                                            dgvProgramas["dgvp_vencimiento", dgvProgramas.RowCount - 1].Value = Convert.ToDateTime(reader["FECHA_VENCIMIENTO"].ToString()).ToString("dd/MM/yyyy");

                                            // dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(reader["SALDO_CARGO"].ToString()).ToString("#,##0.00");
                                            dgvProgramas["dgvp_importe", dgvProgramas.RowCount - 1].Value = Convert.ToDouble(reader["SALDO"].ToString()).ToString("#,##0.00");

                                            dgvProgramas["dgvp_proveedor_id", dgvProgramas.RowCount - 1].Value = reader["PROVEEDOR_ID"].ToString();
                                            dgvProgramas["dgvp_proveedor_clave", dgvProgramas.RowCount - 1].Value = reader["CLAVE_PROV"].ToString();
                                            dgvProgramas["P_EMPRESA", dgvProgramas.RowCount - 1].Value = NOMBRE_EMPESA[h].NOMBRE_CORTO;
                                            dgvProgramas["Requisicion_id", dgvProgramas.RowCount - 1].Value = lista[index].REQUISICION_ID;

                                            //esto solo sirve para cuando se modificara y se agregaran nuevos pagos
                                            if (pagosModif1 != null)
                                            {
                                                for (int k = 0; k < pagosModif1.Length; k++)
                                                {
                                                    if (Convert.ToString(reader["FOLIO_CARGO"]) == pagosModif1[k].FOLIO &&
                                                        pagosModif1[k].FECHA_VENC == Convert.ToDateTime(Convert.ToString(reader["FECHA_VENCIMIENTO"])) &&
                                                        pagosModif1[k].PROVEEDOR_ID == Convert.ToInt32(Convert.ToString(reader["PROVEEDOR_ID"])) &&
                                                        pagosModif1[k].EMPRESA == NOMBRE_EMPESA[h].NOMBRE_CORTO)
                                                    {
                                                        dgvProgramas["MODIFICACION", dgvProgramas.RowCount - 1].Value = "S";
                                                        dgvProgramas["dgvp_check", dgvProgramas.RowCount - 1].Value = 1;
                                                        dgvProgramas["dgvp_check", dgvProgramas.RowCount - 1].ReadOnly = true;

                                                        dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                                                        dgvProgramas.Rows[dgvProgramas.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.Gray;
                                                    }
                                                                                                      


                                                }
                                            }
                                        }
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("No fue posible obtener las requisiciones de las ordenes de compra.\n\n" + ex.ToString(), "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                            conexion_fb.Desconectar();
                        }
                        txtInfo.Text = "";
                        Refresh();
                    }

                    this.Cursor = Cursors.Default;
                    pbEmpresas.Value = 0;
                    pbEmpresas.Visible = false;
                    txtNo.Visible = false;
                    txtInfo.Text = "Se encontrarón " + (dgvProgramas.RowCount - 1).ToString() + " pagos en todas las empresas.";
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                    con.Desconectar();
                }
            }

        }  

        private void button3_Click(object sender, EventArgs e)
        {
            C_USUARIOS funcion = new C_USUARIOS();
            int _CantPagosSelec = 0;
            C_USUARIOS[] usuarios = new C_USUARIOS[0];

            double _ImporteTotal = 0;

            // CONTAMOS TODOS LOS CARGOS SELECCIONADOS PARA LA PROGRAMACIÓN DE PAGO
            for (int i = 0; i < dgvProgramas.Rows.Count; i++)
            {
                // if (dgvProgramas["dgvp_check", i].Value != null)
                if (Convert.ToBoolean(dgvProgramas["dgvp_check", i].Value))
                {
                    //checamos si es programacion o peticion
                    if (usuarioLogueado.Tesoreria.ToUpper() == "TRUE")
                    {
                        //if (!string.IsNullOrEmpty(Convert.ToString(dgvProgramas["DOCTO_PP_ID_2", i].Value)))
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


            #region SI ES REQUISITANTE


            // VALIDAMOS QUE AL MENOS UN CARGO SE HAYA SELECCIONADO
            if (_CantPagosSelec > 0)
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
                                       ESTATUS = "P";

                                    string update = "UPDATE P_DOCTOS_PP SET ";
                                    update += " FECHA_PAGO = @FECHA_PAGO, ";
                                    update += " ESTATUS = @ESTATUS, ";
                                    update += " USUARIO_MODIF = @USUARIO_MODIF, ";
                                    update += " FECHA_HORA_MODIF = @FECHA_HORA_MODIF, ";
                                    update += " IMPORTE_PAGOS = @IMPORTE_PAGOS ";
                                    update += " WHERE DOCTO_PP_ID = @DOCTO_PP_ID";

                                    cmd = new SqlCommand(update, conexion_sql.SC, transaction);
                                    cmd.Parameters.Add("@FECHA_PAGO", SqlDbType.Date).Value = _FechaPgo;
                                    cmd.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = ESTATUS;
                                    cmd.Parameters.Add("@USUARIO_MODIF", SqlDbType.VarChar).Value = usuarioLogueado.Usuario;
                                    cmd.Parameters.Add("@FECHA_HORA_MODIF", SqlDbType.DateTime).Value = DateTime.Now;
                                    cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = _ImporteTotal;
                                    cmd.Parameters.Add("@DOCTO_PP_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                    cmd.ExecuteNonQuery();
                                    #endregion

                                    #region GENERAMOS EL DETALLE
                                    for (int i = 0; i < dgvProgramas.Rows.Count; i++)
                                    {
                                        if (Convert.ToBoolean(dgvProgramas["dgvp_check", i].Value))
                                        {
                                            bool folio_existente = false;

                                            consulta = "SELECT * FROM P_DOCTOS_PP_DET ";
                                            consulta += " WHERE DOCTO_PP_ID =  @DOCTO_PP_ID  ";
                                            consulta += "   AND FOLIO_MICROSIP = @FOLIO_MICROSIP ";
                                            if (!dgvProgramas["dgvp_folio", i].Value.ToString().Contains("REQ"))
                                                consulta += "   AND FOLIO_REQ = @FOLIO_REQ ";
                                            consulta += "   AND PROVEEDOR_ID = @PROVEEDOR_ID ";
                                            consulta += "   AND TIPO = @TIPO ";
                                            consulta += "   AND EMPRESA = @EMPRESA";

                                            cmd = new SqlCommand(consulta, conexion_sql.SC, transaction);
                                            cmd.Parameters.Add("@DOCTO_PP_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                            cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_folio", i].Value);
                                            if (!dgvProgramas["dgvp_folio", i].Value.ToString().Contains("REQ"))
                                                cmd.Parameters.Add("@FOLIO_REQ", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["FOLIO_REQ", i].Value);
                                            cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                            if (!dgvProgramas["dgvp_folio", i].Value.ToString().Contains("REQ"))
                                                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = 'P';
                                            else
                                                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = 'A';
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
                                                consulta = "INSERT INTO P_DOCTOS_PP_DET" +
                                                           "(" +
                                                           "       DOCTO_PP_ID, " +
                                                           "       FOLIO_MICROSIP, " +
                                                           "       FOLIO_REQ, " +
                                                           "       REQUISICION_ID, " +
                                                           "       FECHA_CARGO, " +
                                                           "       PROVEEDOR_ID, " +
                                                           "       PROVEEDOR_CLAVE, " +
                                                           "       PROVEEDOR_NOMBRE, " +
                                                           "       FECHA_VENCIMIENTO, " +
                                                           "       IMPORTE_PAGOS, " +
                                                           "       IMPORTE_CAPTURISTA,IMPORTE_AUTORIZADO, " +
                                                           "       TIPO, EMPRESA,ESTATUS " +
                                                           ") " +
                                                           "VALUES" +
                                                           "(" +
                                                           "       @DOCTO_PP_ID, " +
                                                           "       @FOLIO_MICROSIP, " +
                                                           "       @FOLIO_REQ, " +
                                                           "       @REQUISICION_ID, " +
                                                           "       @FECHA_CARGO, " +
                                                           "       @PROVEEDOR_ID, " +
                                                           "       @PROVEEDOR_CLAVE, " +
                                                           "       @PROVEEDOR_NOMBRE, " +
                                                           "       @FECHA_VENCIMIENTO, " +
                                                           "       @IMPORTE_PAGOS, " +
                                                           "       @IMPORTE_CAPTURISTA, @IMPORTE_AUTORIZADO," +
                                                           "       @TIPO, @EMPRESA,'C'" +
                                                           ")";

                                                cmd = new SqlCommand(consulta, conexion_sql.SC, transaction);
                                                cmd.Parameters.Add("@DOCTO_PP_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_folio", i].Value);
                                                cmd.Parameters.Add("@FOLIO_REQ", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["FOLIO_REQ", i].Value);
                                                cmd.Parameters.Add("@REQUISICION_ID", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["Requisicion_id", i].Value);
                                                cmd.Parameters.Add("@FECHA_CARGO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_fecha", i].Value);
                                                cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                cmd.Parameters.Add("@PROVEEDOR_CLAVE", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_proveedor_clave", i].Value);
                                                cmd.Parameters.Add("@PROVEEDOR_NOMBRE", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_proveedor", i].Value);
                                                cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_vencimiento", i].Value);
                                                cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                cmd.Parameters.Add("@IMPORTE_CAPTURISTA", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                cmd.Parameters.Add("@IMPORTE_AUTORIZADO", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = (dgvProgramas.Rows[i].DefaultCellStyle.BackColor != Color.FromArgb(100, 255, 100) ? "P" : "A");
                                                cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();

                                                #endregion
                                            }
                                            else
                                            {
                                                #region ACTUALIZAMOS EN DOCTOS_PP_DET

                                                consulta = "UPDATE P_DOCTOS_PP_DET SET " +
                                                           "       IMPORTE_PAGOS =  @IMPORTE_PAGOS, " +
                                                           "       FECHA_VENCIMIENTO = @FECHA_VENCIMIENTO " +
                                                           " WHERE DOCTO_PP_ID = @DOCTO_PP_ID " +
                                                           "   AND FOLIO_MICROSIP = @FOLIO_MICROSIP " +
                                                           "   AND FOLIO_REQ = @FOLIO_REQ " +
                                                           "   AND PROVEEDOR_ID = @PROVEEDOR_ID " +
                                                           "   AND TIPO = @TIPO AND EMPRESA = @EMPRESA ";

                                                cmd = new SqlCommand(consulta, conexion_sql.SC, transaction);
                                                cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_vencimiento", i].Value);
                                                cmd.Parameters.Add("@DOCTO_PP_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_folio", i].Value);
                                                cmd.Parameters.Add("@FOLIO_REQ", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["FOLIO_REQ", i].Value);
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

                                    transaction.Commit();

                                    MessageBox.Show("Petición del folio " + FOLIO_MODIF + " realizada con exito", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

                                        #region INSERTAMOS EN DOCTOS_PP

                                        consulta = "INSERT INTO P_DOCTOS_PP" +
                                                    "(" +
                                                    "       FOLIO, " +
                                                    "       FECHA_PAGO, " +
                                                    "       ESTATUS, " +
                                                    "       USUARIO_ID_CREADOR, " +
                                                    "       USUARIO_CREADOR, " +
                                                    "       FECHA_HORA_CREACION, ";
                                        //if (usuarioLogueado.U_ROL != "U")
                                        {
                                            consulta += " IMPORTE_CAPTURISTA, ";
                                            consulta += " IMPORTE_AUTORIZADO, ";
                                            consulta += " ESTATUS_PROC, ";
                                        }
                                        // "       EMPRESA, " +
                                        consulta += "       IMPORTE_PAGOS " +
                                                  ") " +
                                                  "VALUES" +
                                                  "(" +
                                                  "       @FOLIO, " +
                                                  "       @FECHA_PAGO, " +
                                                  "       @ESTATUS, " +
                                                  "       @USUARIO_ID_CREADOR, " +
                                                  "       @USUARIO_CREADOR, " +
                                                  "       @FECHA_HORA_CREACION, ";
                                        //if (usuarioLogueado.U_ROL != "U")
                                        {
                                            consulta += " @IMPORTE_CAPTURISTA, ";
                                            consulta += " @IMPORTE_AUTORIZADO, ";
                                            consulta += " @ESTATUS_PROC, ";
                                        }

                                        //"       @EMPRESA, " +
                                        consulta += "       @IMPORTE_PAGOS  " +
                                                    ");  SELECT @@IDENTITY AS 'Identity';";
                                        cmd = new SqlCommand(consulta, conexion_fb.SC, transaction);
                                        cmd.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = FOLIO;
                                        cmd.Parameters.Add("@FECHA_PAGO", SqlDbType.Date).Value = FECHA_PAGO;
                                        cmd.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = "P";
                                        cmd.Parameters.Add("@USUARIO_ID_CREADOR", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                                        cmd.Parameters.Add("@USUARIO_CREADOR", SqlDbType.VarChar).Value = usuarioLogueado.Usuario;
                                        cmd.Parameters.Add("@FECHA_HORA_CREACION", SqlDbType.DateTime).Value = DateTime.Now;
                                        //if (usuarioLogueado.U_ROL != "U")
                                        {
                                            cmd.Parameters.Add("@IMPORTE_CAPTURISTA", SqlDbType.Float).Value = _ImporteTotal;
                                            cmd.Parameters.Add("@IMPORTE_AUTORIZADO", SqlDbType.Float).Value = _ImporteTotal;
                                            cmd.Parameters.Add("@ESTATUS_PROC", SqlDbType.VarChar).Value = "P";
                                        }
                                        cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = _ImporteTotal;
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
                                            if (Convert.ToBoolean(dgvProgramas["dgvp_check", i].Value))
                                            {
                                                bool folio_existente = false;

                                                consulta = "SELECT * FROM P_DOCTOS_PP_DET " +
                                                           " WHERE DOCTO_PP_ID = @DOCTO_PP_ID  " +
                                                           "   AND FOLIO_MICROSIP = @FOLIO_MICROSIP " +
                                                           "   AND PROVEEDOR_ID = @PROVEEDOR_ID " +
                                                           "   AND TIPO = @TIPO AND EMPRESA = @EMPRESA ";

                                                cmd = new SqlCommand(consulta, conexion_fb.SC, transaction);
                                                cmd.Parameters.Add("@DOCTO_PP_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
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
                                                    #region INSERTAMOS EN DOCTOS_PP_DET

                                                    consulta = "INSERT INTO P_DOCTOS_PP_DET" +
                                                              "(" +
                                                              "       DOCTO_PP_ID, " +
                                                              "       FOLIO_MICROSIP, " +
                                                              "       FOLIO_REQ, " +
                                                              "       REQUISICION_ID, " +
                                                              "       FECHA_CARGO, " +
                                                              "       PROVEEDOR_ID, " +
                                                              "       PROVEEDOR_CLAVE, " +
                                                              "       PROVEEDOR_NOMBRE, " +
                                                              "       FECHA_VENCIMIENTO, " +
                                                              "       IMPORTE_PAGOS, IMPORTE_AUTORIZADO,";
                                                   // if (usuarioLogueado.U_ROL != "U")
                                                    {
                                                        consulta += " IMPORTE_CAPTURISTA, ";
                                                        consulta += " ESTATUS, ";
                                                    }
                                                    consulta += "       TIPO, EMPRESA " +
                                                              ") " +
                                                              "VALUES" +
                                                              "(" +
                                                              "       @DOCTO_PP_ID , " +
                                                              "       @FOLIO_MICROSIP, " +
                                                              "       @FOLIO_REQ, " +
                                                              "       @REQUISICION_ID, " +
                                                              "       @FECHA_CARGO, " +
                                                              "       @PROVEEDOR_ID, " +
                                                              "       @PROVEEDOR_CLAVE, " +
                                                              "       @PROVEEDOR_NOMBRE, " +
                                                              "       @FECHA_VENCIMIENTO, " +
                                                              "       @IMPORTE_PAGOS, @IMPORTE_AUTORIZADO, ";
                                                  //  if (usuarioLogueado.U_ROL != "U")
                                                    {
                                                        consulta += " @IMPORTE_CAPTURISTA, ";
                                                        consulta += " @ESTATUS, ";
                                                    }
                                                    consulta += "       @TIPO, @EMPRESA" +
                                                                ")";

                                                    string fecha1, fecha2;
                                                    fecha1 = dgvProgramas["dgvp_fecha", i].Value.ToString();
                                                    fecha2 = dgvProgramas["dgvp_vencimiento", i].Value.ToString();

                                                    DateTime _fechaCargo = Convert.ToDateTime(fecha1);
                                                    DateTime _fechaVencimiento = Convert.ToDateTime(fecha2);

                                                    cmd = new SqlCommand(consulta, conexion_fb.SC, transaction);
                                                    cmd.Parameters.Add("@DOCTO_PP_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                    cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_folio", i].Value);
                                                    cmd.Parameters.Add("@FOLIO_REQ", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["FOLIO_REQ", i].Value);
                                                    //cmd.Parameters.Add("@REQUISICION_ID", SqlDbType.Int).Value = Convert.ToInt32(Convert.ToString(dgvProgramas["Requisicion_id", i].Value));
                                                    cmd.Parameters.Add("@FECHA_CARGO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_fecha", i].Value);
                                                    cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                    cmd.Parameters.Add("@PROVEEDOR_CLAVE", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_proveedor_clave", i].Value);
                                                    cmd.Parameters.Add("@PROVEEDOR_NOMBRE", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_proveedor", i].Value);
                                                    cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_vencimiento", i].Value);
                                                    cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                    //if (usuarioLogueado.U_ROL != "U")
                                                    {
                                                        cmd.Parameters.Add("@IMPORTE_CAPTURISTA", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                        cmd.Parameters.Add("@IMPORTE_AUTORIZADO", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                        cmd.Parameters.Add("@ESTATUS", SqlDbType.VarChar).Value = "C";
                                                    }
                                                    cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = (dgvProgramas.Rows[i].DefaultCellStyle.BackColor != Color.FromArgb(100, 255, 100) ? "P" : "A");
                                                    cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);
                                                    cmd.Parameters.Add("@REQUISICION_ID",SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["Requisicion_id",i].Value);

                                                    // cmd.Parameters.Add("@REQUISICION_ID", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["REQUISICION_ID", i].Value);
                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();

                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region ACTUALIZAMOS EN DOCTOS_PR_DET
                                                    consulta = "UPDATE P_DOCTOS_PP_DET SET " +
                                                               "       IMPORTE_PAGOS = (IMPORTE_PAGOS + @IMPORTE_PAGOS), " +
                                                               "       FECHA_VENCIMIENTO = @FECHA_VENCIMIENTO " +
                                                               " WHERE DOCTO_PR_ID = @DOCTO_PR_ID " +
                                                               "   AND FOLIO_MICROSIP = @FOLIO_MICROSIP " +
                                                               "   AND FOLIO_REQ = @FOLIO_REQ " +
                                                               "   AND PROVEEDOR_ID = @PROVEEDOR_ID " +
                                                               "   AND TIPO = @TIPO AND EMPRESA = @EMPRESA ";


                                                    cmd = new SqlCommand(consulta, conexion_fb.SC, transaction);
                                                    cmd.Parameters.Add("@IMPORTE_PAGOS", SqlDbType.Float).Value = Convert.ToDouble(dgvProgramas["dgvp_importe", i].Value);
                                                    cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.Date).Value = Convert.ToDateTime(dgvProgramas["dgvp_vencimiento", i].Value);
                                                    cmd.Parameters.Add("@DOCTO_PR_ID", SqlDbType.Int).Value = DOCTO_PR_ID;
                                                    cmd.Parameters.Add("@FOLIO_MICROSIP", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["dgvp_folio", i].Value);
                                                    cmd.Parameters.Add("@FOLIO_REQ", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["FOLIO_REQ", i].Value);
                                                    cmd.Parameters.Add("@PROVEEDOR_ID", SqlDbType.Int).Value = Convert.ToInt32(dgvProgramas["dgvp_proveedor_id", i].Value);
                                                    cmd.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = Convert.ToString(dgvProgramas["P_EMPRESA", i].Value);
                                                    cmd.Parameters.Add("@TIPO", SqlDbType.Float).Value = 'P';

                                                    cmd.ExecuteNonQuery();
                                                    cmd.Dispose();

                                                    #endregion
                                                }
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
                }
            }
            else
            {
                if (_CantPagosSelec == 0)
                    MessageBox.Show("Necesita seleccionar al menos un cargo para la programación de pagos.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Necesita seleccionar al menos un usuario para la programación de pagos.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            #endregion

        }

        private void F_PETICIONESPAGOS_Shown(object sender, EventArgs e)
        {
            dgvProgramas.DoubleBuffered(true);

            //Primero obtenemos el día actual
            DateTime date = DateTime.Now;

            //Asi obtenemos el primer dia del mes actual
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);

            //Y de la siguiente forma obtenemos el ultimo dia del mes
            //agregamos 1 mes al objeto anterior y restamos 1 día.
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
            dtpFecha.Value = oPrimerDiaDelMes;
            txtUser.Text = usuarioLogueado.Usuario;


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

        private void button1_Click(object sender, EventArgs e)
        {

            string folio = "ANT";

            F_ANTICIPOS FA = new F_ANTICIPOS();
            FA.EMPRESA = NOMBRE_EMPESA;
            FA.USUARIO = usuarioLogueado.Usuario;

            if (FA.ShowDialog() == DialogResult.OK)
            {
                if (FA.anticipo.Length > 0)
                {

                    #region evitar registros duplicados
                    List<C_ANTICIPOS> anticipo = new List<C_ANTICIPOS>();
                    for (int i = 0; i < FA.anticipo.Length; i++)
                    {
                        anticipo.Add(FA.anticipo[i]);
                    }

                    bool existe = false;
                    for (int i = 0; i < dgvProgramas.RowCount; i++)
                    {
                        for (int j = 0; j < anticipo.Count; j++)
                        {
                            if (dgvProgramas["dgvp_folio", i].Value.ToString() == "REQ" + anticipo[j].Folio)
                            {
                                existe = true;
                                anticipo.RemoveAt(j);
                                break;
                            }
                        }

                        
                    }
                    #endregion

                    if (anticipo.Count > 0)
                        dgvProgramas.Rows.Insert(0, anticipo.Count);

                    for (int i = 0; i < anticipo.Count; i++)
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
                        dgvProgramas["dgvp_proveedor", i].Value = anticipo[i].Proveedor_Nombre;
                        dgvProgramas["dgvp_vencimiento", i].Value = FA.FECHA.ToString("dd/MM/yyyy");
                        dgvProgramas["dgvp_importe", i].Value = anticipo[i].TOTAL.ToString("#,##0.00");
                        dgvProgramas["dgvp_proveedor_id", i].Value = anticipo[i].Proveedor_ID;
                        dgvProgramas["dgvp_proveedor_clave", i].Value = anticipo[i].Proveedor_Clave;
                        dgvProgramas["Requisicion_id", i].Value = anticipo[i].Requisicion_id;
                        dgvProgramas["P_EMPRESA", i].Value = anticipo[i].Empresa;

                        dgvProgramas.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(100, 255, 100);
                        dgvProgramas.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromArgb(50, 200, 50);
                    }


                }

            }
        }

        private void F_PETICIONESPAGOS_Load(object sender, EventArgs e)
        {

        }
    }
}
