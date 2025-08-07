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
    public partial class F_ANTICIPOS : Form
    {
        public C_ANTICIPOS[] anticipo = new C_ANTICIPOS[0];
        public F_ANTICIPOS()
        {
            InitializeComponent();
        }

        public C_EMPRESAS[] EMPRESA { get; set; }

        public DateTime FECHA { get; set; }

        public double IMPORTE { get; set; }

        public int PROVEEDOR_ID { get; set; }

        public string PROVEEDOR_CLAVE { get; set; }

        public string PROVEEDOR_NOMBRE { get; set; }
        public string USUARIO { get; set; }

        private void F_ANTICIPOS_Load(object sender, EventArgs e)
        {
            dgvAnticipo.DoubleBuffered(true);
        }

        private void F_ANTICIPOS_Shown(object sender, EventArgs e)
        {
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);
            txtUser.Text = USUARIO;
            #region FUNCION PARA BUSCAR PROVEEDORES DE TODAS LAS EMPRESAS /*CODIGO COMENTADO*/
            /*C_FUNCIONES fun = new C_FUNCIONES();
            EMPRESA = fun.TraerEmpresas();
            C_CONEXIONFIREBIRD conn = new C_CONEXIONFIREBIRD();
            FbCommand cmd;
            FbDataReader read;

            string select = "";


            select = "SELECT ";
            select += "      p.proveedor_id, ";
            select += "      p.nombre, ";
            select += "      c.clave_prov ";
            select += " FROM proveedores p ";
            select += " LEFT JOIN claves_proveedores c ON(p.proveedor_id = c.proveedor_id) ";
            select += "WHERE p.estatus = 'N' ";
            select += "ORDER BY p.nombre";

            for (int h = 0; h < EMPRESA.Length; h++)
            {
                if (conn.ConectarFB(EMPRESA[h].NOMBRE_CORTO, false))
                {
                    try
                    {
                        Refresh();
                        this.Cursor = Cursors.WaitCursor;

                        cmd = new FbCommand(select, conn.FBC);
                        read = cmd.ExecuteReader();
                        while (read.Read())
                        {
                            dgv_proveedores.Rows.Add();

                            dgv_proveedores["c_proveedor_id", dgv_proveedores.RowCount - 1].Value = read["PROVEEDOR_ID"].ToString();
                            dgv_proveedores["c_proveedor_clave", dgv_proveedores.RowCount - 1].Value = read["CLAVE_PROV"].ToString();
                            dgv_proveedores["c_proveedor_nombre", dgv_proveedores.RowCount - 1].Value = read["NOMBRE"].ToString();
                        }
                        read.Close();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No fue posible obtener la lista de proveedores de Microsip.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    conn.Desconectar();
                }
            }*/
            #endregion

            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    dgvAnticipo.Rows.Clear();
                    SqlCommand sc;
                    SqlDataReader sdr;

                    #region NOS TRAEMOS LOS QUE TENGAN ADICION
                    string sql = "select";
                    sql += "	 DISTINCT";
                    sql += "  re.*, ";
                    sql += " ppm.Empresa,";
                    sql += " (select sum(rd.cantidad_requerida * rd.Precio_Unitario)";
                    sql += "  from REQ_DET rd";
                    sql += " where rd.Requisicion_id = re.Requisicion_id) TOTAL ";
                    sql += " from REQ_ENC re";
                    sql += " JOIN REQ_DET rd1 ON(re.Requisicion_id = rd1.Requisicion_id)";
                    sql += " JOIN PPTO_PROD_ADICION_MATERIAL ppm ON(rd1.ppto_prod_material_id = ppm.Ppto_Prod_Material_Id)";
                    sql += " where (re.Estatus_general = 'X' OR re.Estatus_general = 'O')";
                    sql += " AND re.Anticipo = 'S'";
                    sql += " and rd1.Adicion = 'S';";
                    

                    sc = new SqlCommand(sql, con.SC);
                    sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        string folio = Convert.ToString(sdr["Folio"]);

                        if (!AnticipoRegistrado(folio,con))
                        {

                            dgvAnticipo.Rows.Add();

                            dgvAnticipo["c_proveedor_id", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Proveedor_ID"]);
                            dgvAnticipo["c_proveedor_clave", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Proveedor_Clave"]);
                            dgvAnticipo["c_proveedor_nombre", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Proveedor_Nombre"]);
                            dgvAnticipo["Requisicion_id", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Requisicion_id"]);

                            dgvAnticipo["Folio", dgvAnticipo.RowCount - 1].Value = folio;
                            dgvAnticipo["TOTAL", dgvAnticipo.RowCount - 1].Value = Convert.ToDouble(Convert.ToString(sdr["TOTAL"])).ToString("N2");
                            dgvAnticipo["Empresa", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Empresa"]);
                            dgvAnticipo["Prioridad", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Prioridad"]);
                            dgvAnticipo["Estatus_general", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Estatus_general"]) == "X" ? "Pago Recurrente" : "Anticipo";

                            dgvAnticipo.Rows[dgvAnticipo.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(231, 233, 239);
                            dgvAnticipo.Rows[dgvAnticipo.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 221, 231);
                        }
                    }
                    sc.Dispose();
                    sdr.Close();
                    #endregion

                    #region NOS TRAEMOS LOS QUE NO TENGAN ADICION
                    sql = "select ";
                    sql += "DISTINCT";
                    sql += " re.*, ";
                    sql += " ppm.Empresa,";
                    sql += " (select sum(rd.cantidad_requerida * rd.Precio_Unitario)";
                    sql += " from REQ_DET rd";
                    sql += "  where rd.Requisicion_id = re.Requisicion_id) TOTAL ";
                    sql += " from REQ_ENC re";
                    sql += " JOIN REQ_DET rd1 ON(re.Requisicion_id = rd1.Requisicion_id)";
                    sql += " JOIN PPTO_PROD_MATERIALES ppm ON(rd1.ppto_prod_material_id = ppm.Ppto_Prod_Material_Id)";
                    sql += " where (re.Estatus_general = 'X' OR re.Estatus_general = 'O')";
                    sql += " AND re.Anticipo = 'S'";
                    sql += " and rd1.Adicion = 'N';";


                    sc = new SqlCommand(sql, con.SC);
                    sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {

                        string folio = Convert.ToString(sdr["Folio"]);

                        if (!AnticipoRegistrado(folio, con))
                        {
                            dgvAnticipo.Rows.Add();

                            dgvAnticipo["c_proveedor_id", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Proveedor_ID"]);
                            dgvAnticipo["c_proveedor_clave", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Proveedor_Clave"]);
                            dgvAnticipo["c_proveedor_nombre", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Proveedor_Nombre"]);
                            dgvAnticipo["Requisicion_id", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Requisicion_id"]);
                            dgvAnticipo["Folio", dgvAnticipo.RowCount - 1].Value = folio;
                            dgvAnticipo["TOTAL", dgvAnticipo.RowCount - 1].Value = Convert.ToDouble(Convert.ToString(sdr["TOTAL"])).ToString("N2");
                            dgvAnticipo["Empresa", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Empresa"]);
                            dgvAnticipo["Prioridad", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Prioridad"]);
                            dgvAnticipo["Estatus_general", dgvAnticipo.RowCount - 1].Value = Convert.ToString(sdr["Estatus_general"]) == "X" ? "Pago Recurrente" : "Anticipo";
                        }

                    }
                    sc.Dispose();
                    sdr.Close();
                    #endregion


                    con.Desconectar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hubo un error al buscar las requisiciones\n" + ex.Message, "Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            this.Cursor = Cursors.Default;
            /*pbEmpresas.Value = 0;
            pbEmpresas.Visible = false;
            txtNo.Visible = false;
            txtInfo.Text = "Se encontrarón " + (dgvProgramas.RowCount - 1).ToString() + " pagos en todas las empresas.";*/
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }
        

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            #region METODO ANTERIOR DONDE SE ASIGNABA EL TOTAL /*COMENTADO*/
            /*if (txt_importe.Text.Trim() != "")
            {
                PROVEEDOR_NOMBRE = Convert.ToString(dgvAnticipo.CurrentRow.Cells["c_proveedor_nombre"].Value);

                if (MessageBox.Show("¿Generar el anticipo para el proveedor '" + PROVEEDOR_NOMBRE + "'?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        FECHA = dtp_fecha.Value;

                        IMPORTE = Convert.ToDouble(txt_importe.Text);

                        PROVEEDOR_ID = Convert.ToInt32(dgvAnticipo.CurrentRow.Cells["c_proveedor_id"].Value);
                        PROVEEDOR_CLAVE = Convert.ToString(dgvAnticipo.CurrentRow.Cells["c_proveedor_clave"].Value);

                        DialogResult = DialogResult.OK;

                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Necesita indicar el importe del anticipo.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/
            #endregion

            try
            {
                bool hayAnticipos = false;
                //revisamos que haya seleccionado aunque sea un pago
                for (int i = 0; i < dgvAnticipo.RowCount; i++)
                {
                    if (Convert.ToBoolean(dgvAnticipo["CHECK", i].Value) )
                    {
                        Array.Resize(ref anticipo, anticipo.Length + 1);
                        anticipo[anticipo.Length - 1] = new C_ANTICIPOS();

                        anticipo[anticipo.Length - 1].Requisicion_id = Convert.ToInt32(dgvAnticipo["Requisicion_id", i].Value.ToString());
                        anticipo[anticipo.Length - 1].Proveedor_ID = Convert.ToInt32(dgvAnticipo["c_proveedor_id", i].Value);
                        anticipo[anticipo.Length - 1].Proveedor_Clave = Convert.ToString(dgvAnticipo["c_proveedor_clave", i].Value);
                        anticipo[anticipo.Length - 1].Proveedor_Nombre = Convert.ToString(dgvAnticipo["c_proveedor_nombre", i].Value);
                        anticipo[anticipo.Length - 1].Empresa = dgvAnticipo["Empresa", i].Value.ToString();
                        anticipo[anticipo.Length - 1].TOTAL = Convert.ToDouble(dgvAnticipo["TOTAL", i].Value);
                        anticipo[anticipo.Length - 1].Estatus_general = dgvAnticipo["Estatus_general", i].Value.ToString();
                        anticipo[anticipo.Length - 1].Prioridad = dgvAnticipo["Prioridad", i].Value.ToString();
                        anticipo[anticipo.Length - 1].Folio = dgvAnticipo["Folio", i].Value.ToString();
                        hayAnticipos = true;

                    }
                }

                if (!hayAnticipos)
                {
                    MessageBox.Show("No selecciono ningun anticipo", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("¿Generar el/los anticipo/s seleccionado/s?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        FECHA = Fecha.Value;

                        DialogResult = DialogResult.OK;

                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


            }
            catch(Exception ex)
            {
                MessageBox.Show("Error al capturar los anticipo\n" + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                
            }
        }


        private bool AnticipoRegistrado(string folio,C_ConexionSQL con)
        {
            bool hay = false;
            try
            {
                string query = "select * from P_DOCTOS_PP_DET ppd ";
                query += " where ppd.FOLIO_MICROSIP = 'REQ" + folio + "' AND ppd.ESTATUS != 'X' ";
                SqlCommand sc = new SqlCommand(query, con.SC);
                SqlDataReader sdr = sc.ExecuteReader();
                while (sdr.Read())
                {
                    hay = true;
                    break;
                }
                sc.Dispose();
                sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error\n" + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            return hay;
        }
    }
}
