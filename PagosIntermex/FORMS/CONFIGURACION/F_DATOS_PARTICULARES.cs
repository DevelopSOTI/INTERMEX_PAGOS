using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagosIntermex
{
    public partial class F_DATOS_PARTICULARES : Form
    {
        private class EMPRESAS
        {
            public EMPRESAS()
            {

            }
            public string NOMBRE_CORTO { get; set; }
            public string NOMBRE { get; set; }
            public int EMPRESA_ID { get; set; }
            public override string ToString()
            {
                return NOMBRE_CORTO.ToString();
            }
        }

        public F_DATOS_PARTICULARES()
        {
            InitializeComponent();
            CargarEmpresas();
        }

        private void F_DATOS_PARTICULARES_Shown(object sender, EventArgs e)
        {

        }

        private void CargarEmpresas()
        {
            // Variables locales:
            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
            C_FUNCIONES func = new C_FUNCIONES();
            if (registros.LeerRegistros(false))
            {
                EMPRESAS[] emp = new EMPRESAS[0];
                #region traemos la lista de sql
                C_EMPRESAS_DATOS_PARTICULARES[] empSql = func.TraerListaEmpresasDP();
                #endregion

                #region buscamos en firebid las empresas
                C_CONEXIONFIREBIRD conexion_fb = new C_CONEXIONFIREBIRD();
                
                FbCommand cmd;
                FbDataReader reader;

                if (conexion_fb.ConectarFB_MANUAL("SYSDBA", registros.FB_PASSWORD, registros.FB_ROOT, registros.FB_SERVIDOR))
                {                    
                    string QUERY = "SELECT NOMBRE_CORTO, EMPRESA_ID FROM EMPRESAS ORDER BY NOMBRE_CORTO";

                    cmd = new FbCommand(QUERY, conexion_fb.FBC);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Array.Resize(ref emp, emp.Length + 1);
                            emp[emp.Length - 1] = new EMPRESAS();

                            emp[emp.Length - 1].NOMBRE_CORTO = Convert.ToString(reader["NOMBRE_CORTO"]);
                            emp[emp.Length - 1].EMPRESA_ID = Convert.ToInt32(Convert.ToString(reader["EMPRESA_ID"]));

                            /*dGVEmpresas.Rows.Add();

                            dGVEmpresas["EMP_ID_MSP", dGVEmpresas.RowCount - 1].Value = Convert.ToString(reader["EMPRESA_ID"]);
                            dGVEmpresas["NOMBRE_CORTO", dGVEmpresas.RowCount - 1].Value = Convert.ToString(reader["NOMBRE_CORTO"]);*/
                           /* if (func.TieneDatosParticulares(conexion_fb,Convert.ToInt32(Convert.ToString(reader["EMPRESA_ID"]))))
                            {
                                dGVEmpresas["TIENE", dGVEmpresas.RowCount - 1].Value = 1;
                                dGVEmpresas["TIENE", dGVEmpresas.RowCount - 1].ReadOnly = true;
                            }
                            else
                            {
                                dGVEmpresas["TIENE", dGVEmpresas.RowCount - 1].ReadOnly = false;

                            }*/

                        }
                    }
                    reader.Close();
                    cmd.Dispose();
                    conexion_fb.Desconectar();
                }

                #endregion

                for (int i = 0; i < emp.Length; i++)
                {
                    dGVEmpresas.Rows.Add();

                    dGVEmpresas["EMP_ID_MSP", dGVEmpresas.RowCount - 1].Value = emp[i].EMPRESA_ID;
                    dGVEmpresas["NOMBRE_CORTO", dGVEmpresas.RowCount - 1].Value = emp[i].NOMBRE_CORTO;

                    for (int j = 0; j < empSql.Length; j++)
                    {
                        if (empSql[j].EMP_ID_MSP == emp[i].EMPRESA_ID)
                        {
                            if (empSql[j].TIENE_DP == "S")
                            {

                                dGVEmpresas["TIENE", dGVEmpresas.RowCount - 1].Value = 1;
                                dGVEmpresas["TIENE", dGVEmpresas.RowCount - 1].ReadOnly = true;
                                dGVEmpresas["T", dGVEmpresas.RowCount - 1].Value = 1;

                            }
                            else
                            {
                                dGVEmpresas["TIENE", dGVEmpresas.RowCount - 1].ReadOnly = false;
                                dGVEmpresas["T", dGVEmpresas.RowCount - 1].Value = 0;
                            }

                        }
                    }
                }
            }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            bool seleccionados = false;
            for (int i = 0; i < dGVEmpresas.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(Convert.ToString(dGVEmpresas["TIENE", i].Value)))
                {
                    continue;
                }
                seleccionados = true;
                break;
            }

            if (seleccionados)
            {
                if (MessageBox.Show("¿Crear datos particulares a las empresas seleccionadas?", "Mensaje de la aplicación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else
            {
                MessageBox.Show("Nada que guardar","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            //recorremos el grid y empezamos a crear los datos de las empresas seleccionada
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                C_FUNCIONES func = new C_FUNCIONES();
                if (reg.LeerRegistros(false))
                {

                    for (int i = 0; i < dGVEmpresas.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dGVEmpresas["TIENE", i].Value)))
                        {
                            continue;
                        }

                        if (Convert.ToString(dGVEmpresas["TIENE", i].Value) != "1")
                            if (func.CrearCamposParticulares(Convert.ToInt32(dGVEmpresas["EMP_ID_MSP", i].Value)))
                                dGVEmpresas["TIENE", dGVEmpresas.RowCount - 1].ReadOnly = true;
                            else
                            {
                                dGVEmpresas["TIENE", dGVEmpresas.RowCount - 1].Value = false;
                            }

                    }

                    MessageBox.Show("Datos guardados correctamente","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrio un error inesperado al crear los datos particulares.\n" + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            } 

        }


        
    }
}
