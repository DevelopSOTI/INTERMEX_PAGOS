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
    public partial class F_CONFIGCONEXIONES : Form
    {
        public F_CONFIGCONEXIONES()
        {
            InitializeComponent();
        }

        C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
        bool cambioFb = false, cambiosql = false;
        string sucursal = "";

        private void F_CONFIGCONEXIONES_Load(object sender, EventArgs e)
        {
            if (registros.LeerRegistros(false))
            {
                try
                {
                    tbPassword.Text = registros.FB_PASSWORD;
                    tbRoot.Text = registros.FB_ROOT;
                    tbServer.Text = registros.FB_SERVIDOR;

                    txtSqlBD.Text = registros.SQL_DATA;
                    txtServSql.Text = registros.SQL_SERV;
                    txtSqlPass.Text = registros.SQL_PASS;
                    txtSqlUser.Text = registros.SQL_USER;
                    DatosParticulares.Enabled = true;
                    ControlUsuarios.Enabled = true;
                }
                catch 
                {
                }

                if (
                    !string.IsNullOrEmpty(tbPassword.Text) ||
                    !string.IsNullOrEmpty(tbRoot.Text) ||
                    !string.IsNullOrEmpty(tbServer.Text) ||
                    !string.IsNullOrEmpty(txtServSql.Text) ||
                    !string.IsNullOrEmpty(txtSqlBD.Text) ||
                    !string.IsNullOrEmpty(txtSqlPass.Text) ||
                    !string.IsNullOrEmpty(txtSqlUser.Text))
                {

                    


                   /* C_ConexionSQL conn = new C_ConexionSQL();
                    SqlCommand cmd;
                    SqlDataReader read;

                    if (conn.ConectarSQL())
                    {
                        try
                        {
                            cmd = new SqlCommand("SELECT * FROM P_SUCURSAL WHERE S_ATRIBUTO = 'NOMBRE'", conn.SC);
                            read = cmd.ExecuteReader();
                            while (read.Read())
                            {
                               sucursal = read["S_VALOR"].ToString();
                            }
                            read.Close();
                            cmd.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No fue posible obtener el nombre de la sucursal.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        conn.Desconectar();
                    }*/
                }
            }
            else
            {
                registros.CrearRegistros(false);
            }
        }





        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            C_CONEXIONFIREBIRD CCFB = new C_CONEXIONFIREBIRD();

            if (CCFB.ConectarFB_MANUAL("SYSDBA", tbPassword.Text, tbRoot.Text, tbServer.Text))
            {
                MessageBox.Show("Conexión Con Microsip correcta.", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cambioFb = true;
                DatosParticulares.Enabled = true;
                //btnSaveChanges.Enabled = true;
                CCFB.Desconectar();
            }
            else
            {
                //btnSaveChanges.Enabled = false;
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (
                    string.IsNullOrEmpty(tbPassword.Text) ||
                    string.IsNullOrEmpty(tbRoot.Text) ||
                    string.IsNullOrEmpty(tbServer.Text) ||
                    string.IsNullOrEmpty(txtServSql.Text) ||
                    string.IsNullOrEmpty(txtSqlBD.Text) ||
                    string.IsNullOrEmpty(txtSqlPass.Text) ||
                    string.IsNullOrEmpty(txtSqlUser.Text) )
            {
                MessageBox.Show("Favor de llenar todos los campos", "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Desea guardar la configuración actual?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                registros.EscribirRegistros("FB_PASSWORD", tbPassword.Text,false);
                registros.EscribirRegistros("FB_ROOT", tbRoot.Text,false);
                registros.EscribirRegistros("FB_SERVIDOR", tbServer.Text,false);

                registros.EscribirRegistros("SQL_DATA", txtSqlBD.Text, false);
                registros.EscribirRegistros("SQL_PASS", txtSqlPass.Text, false);
                registros.EscribirRegistros("SQL_SERV", txtServSql.Text, false);
                registros.EscribirRegistros("SQL_USER", txtSqlUser.Text, false);
                //registros.EscribirRegistros("FB_DB", tbData.Text,false);


                C_ConexionSQL conn = new C_ConexionSQL();
                SqlTransaction tran;
                SqlCommand cmd;

                if (registros.LeerRegistros(false))
                {
                    if (conn.ConectarSQL())
                    {
                        tran = conn.SC.BeginTransaction();
                        try
                        {
                            cmd = new SqlCommand("UPDATE P_SUCURSAL SET S_VALOR = 'PRUEBA' WHERE S_ATRIBUTO = 'NOMBRE'", conn.SC,tran);
                            cmd.ExecuteNonQuery();
                            tran.Commit();
                            cmd.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No fue posible actualizar el nombre de la sucursal.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tran.Rollback();
                        }

                        conn.Desconectar();
                    }
                }

                MessageBox.Show("Datos guardados satisfactoriamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cambioFb = false;
                cambiosql = false;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }





        private void tbServer_TextChanged(object sender, EventArgs e)
        {
            // btnSaveChanges.Enabled = false;
        }

        private void tbRoot_TextChanged(object sender, EventArgs e)
        {
            // btnSaveChanges.Enabled = false;
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            // btnSaveChanges.Enabled = false;
        }

        private void tbDomain_TextChanged(object sender, EventArgs e)
        {
            // btnSaveChanges.Enabled = false;
        }

        private void F_CONFIGCONEXIONES_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(cambioFb || cambiosql)
            {
                if (MessageBox.Show("No ha guardado los cambios.\n¿Desea guardar cambios?", "Mensaje de la aplicación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    btnSaveChanges.PerformClick();
            }
        }

        private void DatosParticulares_Click(object sender, EventArgs e)
        {
            F_DATOS_PARTICULARES fd = new F_DATOS_PARTICULARES();
            fd.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            F_CONTROLUSUARIOS FCU = new F_CONTROLUSUARIOS();
            FCU.usuarioLogueado = new C_USUARIOS();
            //FCU.NOMBRE_EMPRESA = empresas;
            FCU.ShowDialog();
        }

        private void btnSql_Click(object sender, EventArgs e)
        {
            try
            {
                C_ConexionSQL sql = new C_ConexionSQL();
                if (sql.ConectarSQL(txtServSql.Text.Trim(), txtSqlBD.Text.Trim(), txtSqlUser.Text.Trim(), txtSqlPass.Text.Trim()))
                {
                    MessageBox.Show("Conexión correcta con el servidor SQL","Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cambiosql = true;
                    sql.Desconectar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hubo algun error inesperado.\n" + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
