using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;

namespace PagosIntermex
{
    class C_CONEXIONWEBSVC
    {
       /* public ArrayList[] GET_MYSQL_INFORMATION(string URL, string VALUES, int LONG_ARRAY)
        {
            // VARIABLES LOCALES:
            ArrayList[] MY_VALUES = new ArrayList[LONG_ARRAY];
            for (int i = 0; i < LONG_ARRAY; i++)
            {
                MY_VALUES[i] = new ArrayList();
            }

            try
            {
                string REQUEST = SendRequest("http://" + URL + "?" + VALUES);
                /* if (REQUEST != null)
                {
                    MessageBox.Show(REQUEST, "Hey there!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } *

                // LEER EL JSON:
                int COUNT = 0;

                JsonTextReader reader = new JsonTextReader(new StringReader(REQUEST));
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        if (reader.TokenType.ToString() == "String")
                        {
                            MY_VALUES[COUNT++].Add(reader.Value);
                        }
                    }
                    else
                    {
                        COUNT = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible establecer conexión con el dominio.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return MY_VALUES;
        }

        private string SendRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                StringBuilder output = new StringBuilder();
                output.Append(reader.ReadToEnd());
                response.Close();

                return output.ToString();
            }
            catch // (WebException ex)
            {
                // MessageBox.Show("Error while receiving data from the server:\n" + ex.Message, "Something broke.. :(", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return null;
            }
        }

        */



        public void EnviarDoctoCorporativo(int docto_pr_id, C_USUARIOS usuario)
        {
            C_ConexionSQL conn = new C_ConexionSQL();
            SqlTransaction transaction;
            SqlCommand cmd;
            SqlDataReader read;

            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            reg.LeerRegistros(false);

            if (conn.ConectarSQL())
            {
                transaction = conn.SC.BeginTransaction();

                try
                {
                    if (usuario.Requisitante == "S" )
                    {
                        string folio = "";
                        string usuario_creador = "";
                        string fecha_hora_creacion = "";
                        string fecha_pago = "";
                        int rows = 0;
                        #region PROCESAMOS LOS DATOS DEL ENCABEZADO

                        bool pagoEnCeros = true;
                        cmd = new SqlCommand("SELECT * FROM p_doctos_pp WHERE docto_pp_id = " + docto_pr_id + " AND (IMPORTE_AUTORIZADO is not null OR IMPORTE_AUTORIZADO != 0 )", conn.SC, transaction);
                        read = cmd.ExecuteReader();
                        while (read.Read())
                        {
                            folio = read["FOLIO"].ToString();
                            usuario_creador = read["USUARIO_CREADOR"].ToString();
                            fecha_hora_creacion = Convert.ToDateTime(read["FECHA_HORA_CREACION"]).ToString("yyyy-MM-dd HH:mm:ss");
                            fecha_pago = Convert.ToDateTime(read["FECHA_PAGO"]).ToString("yyyy-MM-dd");
                            pagoEnCeros = false;
                        }
                        read.Close();
                        cmd.Dispose();

                        #endregion

                        if (pagoEnCeros)
                        {
                            MessageBox.Show("El importe autorizado no puede ser cero, modifique la petición de pago y asigne valores a los pagos de esta petición", "Mensaje de la aplicación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        cmd = new SqlCommand("SELECT * FROM p_doctos_pp_det WHERE docto_pp_id = " + docto_pr_id + " AND estatus = 'C'", conn.SC, transaction);
                        read = cmd.ExecuteReader();
                        while (read.Read())
                        {
                            rows++;
                        }
                        read.Close();
                        cmd.Dispose();


                        if (rows == 0)
                        {
                            MessageBox.Show("No hay pagos dentro de esta petición de pago", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        cmd = new SqlCommand("UPDATE p_doctos_pp SET estatus = 'A', estatus_proc = 'H' WHERE docto_pp_id = " + docto_pr_id, conn.SC, transaction);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        transaction.Commit();

                        MessageBox.Show("Su petición de pago ha sido enviada con exito.", "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    transaction.Rollback();
                }

                conn.Desconectar();
            }
        }

        public void ObtenerDoctosCorporativo()
        {
            ProgressBar p = new ProgressBar();
            p.Name = "ProgressBar";
            p.Dock = DockStyle.Fill;

            Form f = new Form();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.Text = "Descargando documentos del portal";
            f.ShowInTaskbar = false;
            f.MaximizeBox = false;
            f.MinimizeBox = false;
            f.TopMost = true;
            f.Padding = new Padding(3);
            f.Controls.Add(p);
            f.Width = 500;
            f.Height = 72;
            f.Show();
            f.Update();

            C_ConexionSQL conn = new C_ConexionSQL();
            SqlTransaction transaction;
            SqlCommand cmd;
            SqlDataReader read;

            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            reg.LeerRegistros(false);

            string update = "";
            string sucursal = "";
            string folio = "";

            int docto_pr_id = 0;

            if (conn.ConectarSQL())
            {
                transaction = conn.SC.BeginTransaction();

                try
                {
                    // OBTENEMOS EL NOMBRE DE LA SUCURSAL
                    cmd = new SqlCommand("SELECT * FROM p_sucursal WHERE S_ATRIBUTO = 'NOMBRE'", conn.SC, transaction);
                    read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        sucursal = read["S_VALOR"].ToString();
                    }
                    read.Close();
                    cmd.Dispose();

                    // OBTENEMOS LAS PROGRAMACIONES AUTORIZADAS O RECHAZADAS DE LA SUCURSAL ACTUAL
                    /*Services.Services s = new Services.Services();
                    s.Url = reg.WEBSVC_DOMINIO;
                    Services.DoctosPR[] doctos = s.ObtenerDoctosPR(sucursal);*/

                    /*if (doctos != null)
                    {
                        // COMENZAMOS A ACTUALIZAR EN LA BASE DE DATOS LOCAL
                        p.Maximum = doctos.Length;

                        for (int r = 0; r < doctos.Length; r++)
                        {
                            if ((folio == "") || (folio != doctos[r].Folio))
                            {
                                docto_pr_id = 0;

                                // OBTENEMOS LOS DATOS DEL NUEVO ENCABEZADO
                                cmd = new SqlCommand("SELECT * FROM p_doctos_pr WHERE folio = '" + doctos[r].Folio + "'", conn.SC, transaction);
                                read = cmd.ExecuteReader();
                                while (read.Read())
                                {
                                    docto_pr_id = Convert.ToInt32(read["DOCTO_PR_ID"].ToString());
                                }
                                read.Close();
                                cmd.Dispose();

                                if (docto_pr_id != 0)
                                {
                                    // ACTUALIZA EL NUEVO ENCABEZADO
                                    update = "UPDATE p_doctos_pr SET ";
                                    update += "      estatus = '" + doctos[r].EstatusEnc + "', ";
                                    update += "      estatus_proc = '" + doctos[r].EstatusEnc + "' ";
                                    update += "WHERE docto_pr_id = " + docto_pr_id;

                                    cmd = new SqlCommand(update, conn.SC, transaction);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

                            if (docto_pr_id != 0)
                            {
                                // ACTUALIZAMOS LOS DETALLES
                                update = "UPDATE p_doctos_pr_det SET ";
                                update += "      estatus = '" + doctos[r].EstatusDet + "', ";
                                update += "      importe_autorizado = " + doctos[r].ImporteAutorizado + ", ";
                                update += "      comentarios = '" + doctos[r].Comentarios + "' ";
                                update += "WHERE folio_microsip = '" + doctos[r].FolioMicrosip + "' ";
                                update += "  AND proveedor_id = '" + doctos[r].ProveedorID + "' ";
                                update += "  AND docto_pr_id = " + docto_pr_id.ToString();

                                cmd = new SqlCommand(update, conn.SC, transaction);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            folio = doctos[r].Folio;

                            p.Value++;
                            p.Update();
                        }

                        transaction.Commit();

                        MessageBox.Show("Descarga terminada satisfactoriamente.", "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se han enviado documentos a corporativo.", "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    transaction.Rollback();
                }

                conn.Desconectar();
            }

            f.Close();
        }

        public bool ActualizaDoctosCorporativo(C_ConexionSQL conn, SqlTransaction transaction, string folio, string estatus)
        {
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            reg.LeerRegistros(false);

            string sucursal = "";

            SqlCommand cmd;
            SqlDataReader read;

            try
            {
                // OBTENEMOS EL NOMBRE DE LA SUCURSAL
                cmd = new SqlCommand("SELECT * FROM p_sucursal WHERE S_ATRIBUTO = 'NOMBRE'", conn.SC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    sucursal = read["S_VALOR"].ToString();
                }
                read.Close();
                cmd.Dispose();

                /*  Services.Services s = new Services.Services();
                  s.Url = reg.WEBSVC_DOMINIO;
                  return s.ActualizaDoctosPR(folio, sucursal, estatus);*/

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool ActualizaDoctosDetCorporativo(C_ConexionSQL conn, SqlTransaction transaction, string folio, string empresa)
        {
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            reg.LeerRegistros(false);

            string sucursal = "";

            SqlCommand cmd;
            SqlDataReader read;

            try
            {
                // OBTENEMOS EL NOMBRE DE LA SUCURSAL
                cmd = new SqlCommand("SELECT * FROM p_sucursal WHERE S_ATRIBUTO = 'NOMBRE'", conn.SC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    sucursal = read["S_VALOR"].ToString();
                }
                read.Close();
                cmd.Dispose();

                /* Services.Services s = new Services.Services();
                 s.Url = reg.WEBSVC_DOMINIO;
                 return s.ActualizaDoctosPRDET(folio, empresa, sucursal, cargo);*/
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool CancelaDoctosDetCorporativo(C_ConexionSQL conn, SqlTransaction transaction, string folio, string empresa, string cargo)
        {
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            reg.LeerRegistros(false);

            string sucursal = "";

            SqlCommand cmd;
            SqlDataReader read;

            try
            {
                // OBTENEMOS EL NOMBRE DE LA SUCURSAL
                cmd = new SqlCommand("SELECT * FROM p_sucursal WHERE S_ATRIBUTO = 'NOMBRE'", conn.SC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    sucursal = read["S_VALOR"].ToString();
                }
                read.Close();
                cmd.Dispose();

                /*Services.Services s = new Services.Services();
                s.Url = reg.WEBSVC_DOMINIO;
                return s.CancelaDoctoPRDET(folio, empresa, sucursal, cargo);*/
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
