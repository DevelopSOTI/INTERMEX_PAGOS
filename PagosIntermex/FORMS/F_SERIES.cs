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
    public partial class F_SERIES : Form
    {
        public F_SERIES()
        {
            InitializeComponent();
            CargarSeries();
        }

        private void butDefault_Click(object sender, EventArgs e)
        {

            C_ConexionSQL conn = new C_ConexionSQL();
            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
            SqlCommand cmd,cmd2;
            string query = "";
            registros.LeerRegistros(false);

            //FbTransaction tran = conn.FBC.BeginTransaction();
            try
            {
                if (conn.ConectarSQL())
                {


                    if (MessageBox.Show("¿Desea que " + dgvseries[0, dgvseries.CurrentRow.Index].Value.ToString() + " sea el serie que se utilice ahora?", "Mensaje de Multipagos", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        query = "update P_SERIES set s_estatus = 'B' where s_serie != '" + dgvseries[0, dgvseries.CurrentRow.Index].Value + "'";
                        cmd = new SqlCommand(query, conn.SC);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();


                        query = "update P_SERIES set s_estatus = 'A' where s_serie = '" + dgvseries[0, dgvseries.CurrentRow.Index].Value + "'";
                        cmd2 = new SqlCommand(query, conn.SC);
                        cmd2.ExecuteNonQuery();
                        cmd2.Dispose();
                    }
                    CargarSeries();
                    conn.Desconectar();
                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show("No se pudo establecer la serie por default, consulta con tu administrador del sistema\r\n" + ex.Message,"Mensaje de Multipagos",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                //tran.RollbackRetaining();
                //tran.Dispose();
            }
        }

        private void dgvseries_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CargarSeries()
        {
            dgvseries.Rows.Clear();
            C_ConexionSQL conn = new C_ConexionSQL();
            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
            SqlCommand cmd;
            SqlDataReader reader;
            registros.LeerRegistros(false);

            try
            {

                if (conn.ConectarSQL())
                {
                    string query = "select s.S_SERIE, S_CONSECUTIVO, S_ESTATUS FROM P_SERIES s";

                    cmd = new SqlCommand(query, conn.SC);
                    reader = cmd.ExecuteReader();
                    int i = 0;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dgvseries.Rows.Add();
                            dgvseries["SERIE", i].Value = Convert.ToString(reader["S_SERIE"]);
                            dgvseries["CONSEC", i].Value = Convert.ToString(reader["S_CONSECUTIVO"]);

                            if (Convert.ToString(reader["S_ESTATUS"]) == "B")
                                dgvseries["DEFAULT", i].Value = "NO";
                            else
                                dgvseries["DEFAULT", i].Value = "SI";

                            i++;
                        }
                    }
                    reader.Close();
                    cmd.Dispose();
                    conn.Desconectar();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Hubo un problema al conectar con la base de datos\r\n" + ex.Message,"Mensaje de Multipagos",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

        }

        private void butSerie_Click(object sender, EventArgs e)
        {
            C_ConexionSQL conn = new C_ConexionSQL();
            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
            SqlCommand cmd, cmd2;
            SqlDataReader reader;
            int s_id =0;
            int s_consecutivo = 1;
            string query;
            bool band = true;
            registros.LeerRegistros(false);
            if (txtSerie.Text != "" && txtSerie.Text.Length == 3)
            {
                //consultamos en la base de datos que no exista ese registro
                if (conn.ConectarSQL())
                {
                    query = "select s.S_SERIE from P_SERIES s";
                    cmd = new SqlCommand(query, conn.SC);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (txtSerie.Text == reader.GetString(0))
                        {
                            band = false;
                            break;
                        }
                    }

                    if (band == true)
                    {
                        try
                        {

                           /* query = "execute procedure gen_serie_id";
                            cmd = new FbCommand(query, conn.FBC);
                            reader = cmd.ExecuteReader();

                            while (reader.Read())
                                s_id = Convert.ToInt32(reader.GetInt32(0));

                            cmd.Dispose();
                            reader.Close();*/

                            SqlTransaction tran = conn.SC.BeginTransaction();


                            try
                            {
                                if (MessageBox.Show("¿Desea que " + txtSerie.Text + " sea el serie que se utilice ahora?", "Mensaje de Multipagos", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    //insertamos el nuevo serie
                                    query = "insert into P_SERIES(s_serie,s_consecutivo,s_estatus) values('" + txtSerie.Text + "'," + s_consecutivo + ",'A')";
                                    cmd2 = new SqlCommand(query, conn.SC, tran);
                                    cmd2.ExecuteNonQuery();
                                    // tran.CommitRetaining();
                                    tran.Commit();
                                    cmd2.Dispose();
                                    tran.Dispose();

                                    SqlTransaction tran2 = conn.SC.BeginTransaction();
                                    try
                                    {
                                        //actualizamos el estatus de todos los demas
                                        query = "update P_SERIES set s_estatus = 'B' where s_serie != '" + txtSerie.Text + "'";
                                        cmd = new SqlCommand(query, conn.SC, tran2);
                                        cmd.ExecuteNonQuery();
                                        tran2.Commit();
                                        cmd.Dispose();
                                        conn.SC.Close();
                                        tran2.Dispose();


                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("No se pudo establecer la serie por default, consulta con tu administrador del sistema\r\n" + ex.Message, "Mensaje de Multipagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        tran2.Rollback();
                                    }
                                }

                                else
                                {
                                    //insertamos el nuevo serie
                                    query = "insert into P_SERIES(s_serie,s_consecutivo,s_estatus) values('" + txtSerie.Text + "'," + s_consecutivo + ",'B')";
                                    cmd2 = new SqlCommand(query, conn.SC, tran);
                                    cmd2.ExecuteNonQuery();
                                    tran.Commit();
                                    cmd2.Dispose();
                                    tran.Dispose();
                                }


                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Hubo un error al insertar el serie, verifique los datos\r\n" + ex.Message, "Mensaje de Multipagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tran.Rollback();
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hubo un error al conectar, verifique los datos\r\n" + ex.Message, "Mensaje de Multipagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        MessageBox.Show("No se puede ingresar un serie que ya esta dado de alta", "Mensaje de Multipagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Escriba un serie de al menos 3 caracteres", "Mensaje de Multipagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            CargarSeries();
        }

        private void txtSerie_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && e.KeyChar != (char)8)
            {

                e.Handled = true;

            }
        }
    }
}
