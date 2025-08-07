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
    public partial class F_VER_DETALLE_PETICION : Form
    {
        public int DOCTO_PP_DET_ID { get; set; }
        public C_USUARIOS usuario { get; set; }

        class T_PAGOS
        {
            public DateTime FECHA_HORA_CREACION { get; set; }
            public int DOCTO_PP_DET_ID { get; set; }
            public string FOLIO { get; set; }
            public double IMPORTE_AUTORIZADO { get; set; }
            public string EMPRESA { get; set; }
            public DateTime FECHA_CARGO { get; set; }
            public DateTime FECHA_VENCIMIENTO { get; set; }
            public string FOLIO_MICROSIP { get; set; }
            public string FOLIO_REQ { get; set; }
            public string PROVEEDOR_NOMBRE { get; set; }
            public string NIVEL { get; set; }
            public double MONTO_AUTORIZADO { get; set; }
            public string Usuario { get; set; }
            public string Departamento { get; set; }
            public string E_ENC { get; set; }
            public string E_DET { get; set; }

            public string ESTATUS_ENC { get; set; }
            public string MOTOTIVO_CAN { get; set; }
        }
        public F_VER_DETALLE_PETICION()
        {
            InitializeComponent();
        }

        private void F_VER_DETALLE_PETICION_Shown(object sender, EventArgs e)
        {
            dgvPagos.DoubleBuffered(true);

            C_ConexionSQL con = new C_ConexionSQL();

            try
            {
                if (con.ConectarSQL())
                {
                    SqlCommand sc;
                    SqlDataReader sdr;

                    string query = "    select ";
                    query += " pp.ESTATUS,pp.MOTIVO_CANCELACION, ";
                    query += " pp.FECHA_HORA_CREACION ";
                    query += " ,ppd.DOCTO_PP_DET_ID ";
                    query += " ,pp.FOLIO ";
                    query += " ,pr.ESTATUS_PROC E_ENC ";
                    query += " ,ppr.ESTATUS E_DET ";
                    query += " ,ppd.IMPORTE_AUTORIZADO ";
                    query += " ,ppd.EMPRESA ";
                    query += " ,ppd.FECHA_CARGO ";
                    query += " ,ppd.FECHA_VENCIMIENTO ";
                    query += " ,ppd.FOLIO_MICROSIP ";
                    query += " ,ppd.FOLIO_REQ ";
                    query += " ,ppd.PROVEEDOR_NOMBRE ";
                    query += " ,pad.NIVEL ";
                    query += " ,pad.MONTO_AUTORIZADO ";
                    query += " ,u.Nombre Usuario ";
                    query += " ,u.Departamento ";
                    query += "  from  ";
                    query += "    P_DOCTOS_PP pp  ";
                    query += "    join P_DOCTOS_PP_DET ppd on ppd.DOCTO_PP_ID = pp.DOCTO_PP_ID ";
                    query += "    JOIN P_DOCTOS_PR_DET ppr on ppr.DOCTO_PP_DET_ID = ppd.DOCTO_PP_DET_ID ";
                    query += "    JOIN P_DOCTOS_PR pr on pr.DOCTO_PR_ID = ppr.DOCTO_PR_ID ";
                    query += "    JOIN P_AUT_DOCTOS_PR pad on pad.DOCTO_PR_DET_ID = ppr.DOCTO_PR_DET_ID ";
                    query += "    JOIN USUARIOS u on u.Usuario_id = pad.USUARIO_ID ";
                    query += "    where pp.DOCTO_PP_ID = " + DOCTO_PP_DET_ID;
                    query += "   AND pad.ESTATUS != 'P' ";
                    //query += "   AND ppr.DOCTO_PP_ID = " + DOCTO_PP_DET_ID;
                    query += " order by pad.NIVEL desc, u.Usuario , ppd.FOLIO_MICROSIP";


                    sc = new SqlCommand(query, con.SC);
                    sdr = sc.ExecuteReader();
                    T_PAGOS[] pagos = new T_PAGOS[0];
                    while (sdr.Read())
                    {
                        Array.Resize(ref pagos, pagos.Length + 1);
                        pagos[pagos.Length - 1] = new T_PAGOS();

                        pagos[pagos.Length - 1].ESTATUS_ENC = Convert.ToString(sdr["ESTATUS"]);
                        pagos[pagos.Length - 1].MOTOTIVO_CAN = Convert.ToString(sdr["MOTIVO_CANCELACION"]);
                        pagos[pagos.Length - 1].FECHA_HORA_CREACION = Convert.ToDateTime(Convert.ToString(sdr["FECHA_HORA_CREACION"]));
                        pagos[pagos.Length - 1].DOCTO_PP_DET_ID = Convert.ToInt32(Convert.ToString(sdr["DOCTO_PP_DET_ID"]));
                        pagos[pagos.Length - 1].FOLIO = Convert.ToString(sdr["FOLIO"]);
                        string importe = Convert.ToString(sdr["IMPORTE_AUTORIZADO"]);
                        if (string.IsNullOrEmpty(importe))
                            pagos[pagos.Length - 1].IMPORTE_AUTORIZADO = 0;
                        else
                            pagos[pagos.Length - 1].IMPORTE_AUTORIZADO = Convert.ToDouble(importe);
                        pagos[pagos.Length - 1].EMPRESA = Convert.ToString(sdr["EMPRESA"]);
                        pagos[pagos.Length - 1].FECHA_CARGO = Convert.ToDateTime(Convert.ToString(sdr["FECHA_CARGO"]));
                        pagos[pagos.Length - 1].FECHA_VENCIMIENTO = Convert.ToDateTime(Convert.ToString(sdr["FECHA_VENCIMIENTO"]));
                        pagos[pagos.Length - 1].FOLIO_MICROSIP = Convert.ToString(sdr["FOLIO_MICROSIP"]);
                        pagos[pagos.Length - 1].FOLIO_REQ = Convert.ToString(sdr["FOLIO_REQ"]);
                        pagos[pagos.Length - 1].PROVEEDOR_NOMBRE = Convert.ToString(sdr["PROVEEDOR_NOMBRE"]);
                        pagos[pagos.Length - 1].NIVEL = Convert.ToString(sdr["NIVEL"]);
                        pagos[pagos.Length - 1].MONTO_AUTORIZADO = string.IsNullOrEmpty(Convert.ToString(sdr["MONTO_AUTORIZADO"])) ? 0 : Convert.ToDouble(Convert.ToString(sdr["MONTO_AUTORIZADO"]));
                        pagos[pagos.Length - 1].Usuario = Convert.ToString(sdr["Usuario"]);
                        pagos[pagos.Length - 1].Departamento = Convert.ToString(sdr["Departamento"]);
                        pagos[pagos.Length - 1].E_ENC = Convert.ToString(sdr["E_ENC"]);
                        pagos[pagos.Length - 1].E_DET = Convert.ToString(sdr["E_DET"]);

                    }

                    if (pagos.Length > 0)
                    {
                        //checamos si esta cancelado
                        if (pagos[0].ESTATUS_ENC == "X")
                        {
                            lbInfo.Text = "Esta petición con Folio: " + pagos[0].FOLIO + "\n";
                            lbInfo.Text += "Ha sido cancelada por el siguiente motivo: " + pagos[0].MOTOTIVO_CAN;
                            return;
                        }

                        lbInfo.Text = "Petición con Folio: " + pagos[0].FOLIO + "\n";
                        lbInfo.Text += "Fecha de Creación: " + pagos[0].FECHA_HORA_CREACION.ToString("dd/MM/yyyy");

                        //sacamos los detalles de la peticion de pago sin repetir uno
                        List<int> docto_det = new List<int>();
                        List<string> folios_msp = new List<string>();
                        docto_det.Add(pagos[0].DOCTO_PP_DET_ID);

                        for (int j = 0; j < pagos.Length; j++)
                        {
                            if (docto_det.FindIndex(x => x == pagos[j].DOCTO_PP_DET_ID) == -1)
                            {
                                docto_det.Add(pagos[j].DOCTO_PP_DET_ID);
                                folios_msp.Add(pagos[j].FOLIO_MICROSIP);
                            }
                        }
                        #region ENCABEZADO Y DETALLE DE PETICION
                        dgvPagos.Rows.Add();
                        dgvPagos.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
                        dgvPagos.Rows[0].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        dgvPagos.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Black;
                        dgvPagos.CellBorderStyle = DataGridViewCellBorderStyle.None;

                        dgvPagos["P_EMPRESA", dgvPagos.RowCount - 1].Value = "Petición: " + pagos[0].FOLIO;

                        for (int i = 0; i < docto_det.Count; i++)
                        {
                            for (int j = 0; j < pagos.Length; j++)
                            {
                                if (pagos[j].DOCTO_PP_DET_ID == docto_det[i])
                                {
                                    dgvPagos.Rows.Add();
                                    dgvPagos["DOCTO_PP_DET_ID2", dgvPagos.RowCount - 1].Value = pagos[j].DOCTO_PP_DET_ID;
                                    dgvPagos["P_EMPRESA", dgvPagos.RowCount - 1].Value = pagos[j].EMPRESA;
                                    dgvPagos["PROVEEDOR_P", dgvPagos.RowCount - 1].Value = pagos[j].PROVEEDOR_NOMBRE;
                                    dgvPagos["IMPORTE_P", dgvPagos.RowCount - 1].Value = pagos[j].IMPORTE_AUTORIZADO.ToString("C2");

                                    dgvPagos["FOLIO_P", dgvPagos.RowCount - 1].Value = pagos[j].FOLIO_MICROSIP;

                                    #region checamos si existe en las programaciones pintarlo de verde o amarillo
                                    query = "select * from ";
                                    query += " P_DOCTOS_PR_DET prd  ";
                                    query += " join P_AUT_DOCTOS_PR pad on pad.DOCTO_PR_DET_ID = prd.DOCTO_PR_DET_ID ";
                                    query += " where prd.DOCTO_PP_DET_ID = '" + pagos[j].DOCTO_PP_DET_ID + "'";
                                    query += "  and pad.MONTO_AUTORIZADO is null ";
                                    query += " and pad.ESTATUS != 'P'";
                                    sc = new SqlCommand(query, con.SC);
                                    sdr = sc.ExecuteReader();
                                    bool existe = false;
                                    while (sdr.Read())
                                    {
                                        existe = true;
                                        break;
                                    }

                                    //si es true se pinta amarillo si no verde
                                    if (existe)
                                    {
                                        dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(203, 171, 42);
                                        dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(169, 142, 35);
                                    }
                                    else
                                    {
                                        dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(100, 150, 100);
                                        dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(90, 135, 90);
                                    }
                                    sdr.Close();
                                    sc.Dispose();
                                    #endregion

                                    dgvPagos["FOLIO_REQ", dgvPagos.RowCount - 1].Value = pagos[j].FOLIO_REQ;
                                    dgvPagos["FECHA_VEN", dgvPagos.RowCount - 1].Value = pagos[j].FECHA_VENCIMIENTO.ToString("dd/MM/yyyy");
                                    i++;
                                    if (i > docto_det.Count - 1)
                                        break;
                                }
                            }
                        }



                        dgvPagos.Rows.Add();
                        dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGray;
                        dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionForeColor = Color.Black;

                        dgvPagos["PROVEEDOR_P", dgvPagos.RowCount - 1].Value = "Usuarios asignados a autorizar ";
                        dgvPagos["P_EMPRESA", dgvPagos.RowCount - 1].Value = "Departamento: " + pagos[0].Departamento;
                        #endregion

                        string nivel = pagos[0].NIVEL;
                        string usuario = "";
                        for (int i = 0; i < pagos.Length; i++)
                        {
                            dgvPagos.Rows.Add();

                            if (nivel == pagos[i].NIVEL)
                            {
                                dgvPagos["FOLIO_P", dgvPagos.RowCount - 1].Value = pagos[i].FOLIO_MICROSIP;
                                dgvPagos["P_EMPRESA", dgvPagos.RowCount - 1].Value = pagos[i].EMPRESA;
                                dgvPagos["PROVEEDOR_P", dgvPagos.RowCount - 1].Value = pagos[i].PROVEEDOR_NOMBRE;
                                dgvPagos["IMPORTE_P", dgvPagos.RowCount - 1].Value = pagos[i].IMPORTE_AUTORIZADO.ToString("C2");

                                if (pagos[i].MONTO_AUTORIZADO == 0)
                                {
                                    dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(203, 171, 42);
                                    dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(169, 142, 35);
                                }
                                else
                                {
                                    dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(100, 150, 100);
                                    dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(90, 135, 90);
                                }

                                dgvPagos["IMPORTE_AUT", dgvPagos.RowCount - 1].Value = pagos[i].MONTO_AUTORIZADO.ToString("C2");

                                if (usuario != pagos[i].Usuario)
                                {

                                    usuario = pagos[i].Usuario;
                                    dgvPagos["USUARIO", dgvPagos.RowCount - 1].Value = pagos[i].Usuario;
                                }
                            }
                            else
                            {
                                nivel = pagos[i].NIVEL;

                                dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGray;
                                dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionForeColor = Color.Black;

                                dgvPagos["PROVEEDOR_P", dgvPagos.RowCount - 1].Value = "Usuarios asignados a autorizar ";
                                dgvPagos["P_EMPRESA", dgvPagos.RowCount - 1].Value = "Departamento: " + pagos[0].Departamento;

                                dgvPagos.Rows.Add();
                                dgvPagos["FOLIO_P", dgvPagos.RowCount - 1].Value = pagos[i].FOLIO_MICROSIP;
                                dgvPagos["P_EMPRESA", dgvPagos.RowCount - 1].Value = pagos[i].EMPRESA;
                                dgvPagos["PROVEEDOR_P", dgvPagos.RowCount - 1].Value = pagos[i].PROVEEDOR_NOMBRE;
                                dgvPagos["IMPORTE_P", dgvPagos.RowCount - 1].Value = pagos[i].IMPORTE_AUTORIZADO.ToString("C2");

                                if (pagos[i].MONTO_AUTORIZADO == 0)
                                {
                                    dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(203, 171, 42);
                                    dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(169, 142, 35);
                                }
                                else
                                {
                                    dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(100, 150, 100);
                                    dgvPagos.Rows[dgvPagos.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(90, 135, 90);
                                }
                                dgvPagos["IMPORTE_AUT", dgvPagos.RowCount - 1].Value = pagos[i].MONTO_AUTORIZADO.ToString("C2");
                                if (usuario != pagos[i].Usuario)
                                {
                                    usuario = pagos[i].Usuario;
                                    dgvPagos["USUARIO", dgvPagos.RowCount - 1].Value = pagos[i].Usuario;
                                }
                            }

                        }
                    }
                    else
                    {
                        string estatus = "";
                        if (VerificarProgramacion(con,ref estatus))
                        {
                            if (estatus != "X")
                            {
                                lbInfo.Text = "Esta petición de pago aun esta en proceso de revisión.\n";
                                lbInfo.Text += "Aun no se ha creado la programación de pago de esta petición.\n";
                                lbInfo.Text += "Lo mas probable es que la programación de pago de esta petición haya sido cancelada por un Autorizador";
                            }
                            else
                            {
                                lbInfo.Text = "Esta petición Ha sido cancelada";
                            }
                        }
                    }
                }
                else
                {
                    lbInfo.Text = "Esta petición de pago aun no se envia a revisión.\n";
                }

                con.Desconectar();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void F_VER_DETALLE_PETICION_Load(object sender, EventArgs e)
        {
            txtUsuario.Text = usuario.Usuario;
        }

        private bool VerificarProgramacion(C_ConexionSQL con,ref string estatus)
        {
            bool band = false;
            try
            {
                string query = "    select ";
                query += " pp.FECHA_HORA_CREACION ";
                query += " ,pp.FOLIO ";
                
                query += " ,ppd.IMPORTE_AUTORIZADO ";
                query += " ,ppd.EMPRESA ";
                query += " ,ppd.FECHA_CARGO ";
                query += " ,ppd.FECHA_VENCIMIENTO ";
                query += " ,ppd.FOLIO_MICROSIP ";
                query += " ,ppd.FOLIO_REQ ";
                query += " ,ppd.PROVEEDOR_NOMBRE, ppd.ESTATUS ";
                query += "  from  ";
                query += "    P_DOCTOS_PP pp  ";
                query += "    join P_DOCTOS_PP_DET ppd on ppd.DOCTO_PP_ID = pp.DOCTO_PP_ID ";
                query += "    where pp.DOCTO_PP_ID = " + DOCTO_PP_DET_ID;
                query += "   AND ppd.ESTATUS = 'C' OR ppd.ESTATUS = 'X' ";

                SqlCommand sc = new SqlCommand(query, con.SC);
                SqlDataReader sdr = sc.ExecuteReader();
                while (sdr.Read())
                {
                    band = true;
                    estatus = Convert.ToString(sdr["ESTATUS"]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return band;
        }
    }
}
