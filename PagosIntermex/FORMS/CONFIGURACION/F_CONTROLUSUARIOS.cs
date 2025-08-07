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
    public partial class F_CONTROLUSUARIOS : Form
    {
        int SELECTED_ID = 0;
        int SELECTED_OPTION = 0;
        public C_EMPRESAS[] NOMBRE_EMPRESA;
        public C_USUARIOS usuarioLogueado;
        public class DEPTOS
        {
            public string NOMBRE { get; set; }
            public int DEPTO_CO_ID { get; set; }
            public string CLAVE_DEPTO { get; set; }

            public override string ToString()
            {
                return NOMBRE.ToString();
            }
        }

        public F_CONTROLUSUARIOS()
        {
            InitializeComponent();
        }

        private void SELECT_USUARIOS()
        {
            dgvUserData.Rows.Clear();

            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

            if (registros.LeerRegistros(false))
            {
                C_ConexionSQL conexion_fb = new C_ConexionSQL();
                SqlCommand cmd;
                SqlDataReader reader;

                if (conexion_fb.ConectarSQL())
                {
                    try
                    {
                        cmd = new SqlCommand("SELECT * FROM USUARIOS", conexion_fb.SC);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                dgvUserData.Rows.Add();

                                dgvUserData["Id", dgvUserData.RowCount - 1].Value = reader["Usuario_id"].ToString();
                                dgvUserData["Nombre", dgvUserData.RowCount - 1].Value = reader["Nombre"].ToString();
                                dgvUserData["Departamento", dgvUserData.RowCount - 1].Value = reader["Departamento"].ToString();
                                dgvUserData["Correo", dgvUserData.RowCount - 1].Value = reader["Correo"].ToString();
                                dgvUserData["User", dgvUserData.RowCount - 1].Value = reader["Usuario"].ToString();
                                dgvUserData["Pass", dgvUserData.RowCount - 1].Value = reader["Contraseña"].ToString();
                                dgvUserData["Estatus", dgvUserData.RowCount - 1].Value = reader["Estatus"].ToString() == "A" ? "Activo" : "Baja";
                                dgvUserData["REQUI", dgvUserData.RowCount - 1].Value = reader["Requisitante"].ToString() == "S" ? "Si" : "No";
                                dgvUserData["TESORERIA", dgvUserData.RowCount - 1].Value = reader["Tesoreria"].ToString() == "S" ? "Si" : "No";
                                switch (Convert.ToString(reader["U_ROL"]))
                                {
                                    case "A":
                                        dgvUserData["Rol", dgvUserData.RowCount - 1].Value = "Administrador";break;
                                    case "C":
                                        dgvUserData["Rol", dgvUserData.RowCount - 1].Value = "Capturista"; break;
                                    case "S":
                                        dgvUserData["Rol", dgvUserData.RowCount - 1].Value = "Sistemas"; break;
                                }
                            }
                        }
                        reader.Close();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    conexion_fb.Desconectar();
                }
            }
        }

        private bool SELECT_FUNCTION()
        {
            C_CONEXIONFIREBIRD conexion_fb = new C_CONEXIONFIREBIRD();
            FbCommand cmd;
            FbDataReader reader;

            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

            bool result = false;

            if (registros.LeerRegistros(false))
            {
                bool SUCCESS = false;

                #region VALIDAMOS QUE EL USUARIO EXISTA EN MICROSIP

                bool necesitaUsuarioMicrosip = true;

                if (cbRol.SelectedIndex > 0)
                {
                    necesitaUsuarioMicrosip = false;
                    SUCCESS = true;
                }

                if (necesitaUsuarioMicrosip)
                    if (conexion_fb.ConectarFB("System\\Config"))
                    {
                        try
                        {
                            string QUERY = "SELECT COUNT(*) AS INSIDENCES FROM USUARIOS WHERE NOMBRE = '" + tbUsername.Text + "'";

                            cmd = new FbCommand(QUERY, conexion_fb.FBC);
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (int.Parse(reader["INSIDENCES"].ToString()) >= 1)
                                    {
                                        // EL USUARIO SI EXISTE
                                        SUCCESS = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("El nombre de usuario no existe en Microsip. Introduzca un nombre de usuario que exista dentro de la lista de usuarios de Microsip.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        tbUsername.Text = "";
                                        tbPassword.Text = "";
                                        tbConfirmPassword.Text = "";
                                    }
                                }
                            }
                            reader.Close();
                            cmd.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No fue posible identificar si el usuario esta registrado en Microsip.\n\n" + ex.ToString(), "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        conexion_fb.Desconectar();
                    }

                #endregion

                if (SUCCESS)
                {
                    switch (SELECTED_OPTION)
                    {
                        case 1:
                            #region CREAR UN USUARIO NUEVO

                            C_ConexionSQL conSql = new C_ConexionSQL();
                            if (conSql.ConectarSQL())
                            {
                                try
                                {
                                    string U_USERNAME = tbUsername.Text,
                                           U_FULLNAME = tbFullUsername.Text,
                                           U_PASSWORD = tbPassword.Text,
                                           U_EMAIL = tbCorreoElectronico.Text,
                                           Requi = cbRequi.Text == "Si" ? "S" :"N",
                                           Teso = cbTeso.Text == "Si" ? "S" : "N",
                                           U_ID = "";
                                    string U_ROL = "";
                                    DEPTOS depto = cbDptos.SelectedItem as DEPTOS;
                                    string departamento = depto.NOMBRE;
                                    string clave = depto.CLAVE_DEPTO;

                                    switch (cbRol.SelectedIndex)
                                    {
                                        case 0: U_ROL = "C"; break;
                                        case 1: U_ROL = "A"; break;
                                        case 2: U_ROL = "S"; break;
                                    }


                                    /* string QUERY = "EXECUTE PROCEDURE GEN_USER_ID";

                                     cmd = new FbCommand(QUERY, conexion_fb.FBC);
                                     reader = cmd.ExecuteReader();
                                     if (reader.HasRows)
                                     {
                                         while (reader.Read())
                                         {
                                             U_ID = reader["U_ID"].ToString();
                                         }
                                     }
                                     reader.Close();
                                     cmd.Dispose();*/

                                    string QUERY = "INSERT INTO USUARIOS (Usuario, Contraseña, Requisitante,Tesoreria, Departamento, Nombre, Estatus, Correo, Usuario_Creador,Fecha_Creacion,Fecha_Ultima_Modificacion,Usuario_Modificador, Clave_Depto, U_ROL) VALUES (@Usuario, @Pass, @Requi,@Teso,@Departamento, @Nombre, 'A', @Correo, 1,@FECHA,@FECHA2,1, @Clave, '" + U_ROL + "')";

                                    SqlCommand cmd1 = new SqlCommand(QUERY, conSql.SC);
                                    cmd1.Parameters.Add("@Usuario", SqlDbType.VarChar).Value = U_USERNAME;
                                    cmd1.Parameters.Add("@Pass", SqlDbType.VarChar).Value = U_PASSWORD;
                                    cmd1.Parameters.Add("@Requi", SqlDbType.VarChar).Value = Requi;
                                    cmd1.Parameters.Add("@Teso", SqlDbType.VarChar).Value = Teso;
                                    cmd1.Parameters.Add("@Departamento", SqlDbType.VarChar).Value = departamento;
                                    cmd1.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = U_FULLNAME;
                                    cmd1.Parameters.Add("@Correo", SqlDbType.VarChar).Value = U_EMAIL;
                                    cmd1.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = DateTime.Now;
                                    cmd1.Parameters.Add("@Fecha2", SqlDbType.DateTime).Value = DateTime.Now;
                                    cmd1.Parameters.Add("@Clave", SqlDbType.VarChar).Value = clave;
                                    cmd1.ExecuteNonQuery();
                                    cmd1.Dispose();

                                    MessageBox.Show("Los datos se han guardado exitosamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    borrarDatos();
                                    result = true;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString(), "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                if (necesitaUsuarioMicrosip)
                                    conexion_fb.Desconectar();
                                conSql.Desconectar();
                            }

                            #endregion
                            break;
                        case 2:
                            #region ACTUALIZAR AL USUARIO

                            C_ConexionSQL conSql1 = new C_ConexionSQL();
                            if (conSql1.ConectarSQL())
                            {
                                try
                                {
                                    string U_USERNAME = tbUsername.Text,
                                           U_FULLNAME = tbFullUsername.Text,
                                           U_PASSWORD = tbPassword.Text,
                                           U_EMAIL = tbCorreoElectronico.Text,
                                           Requi = cbRequi.Text == "Si" ? "S" : "N",
                                           Teso = cbTeso.Text == "Si" ? "S" : "N",
                                           U_ID = SELECTED_ID.ToString(),
                                           U_ESTATUS = cbEstatus.SelectedItem.ToString() == "Activo" ? "A" : "B";
                                           string U_ROL = "";
                                    DEPTOS depto = cbDptos.SelectedItem as DEPTOS;
                                    string departamento = depto.NOMBRE;
                                    string clave = depto.CLAVE_DEPTO;

                                    switch (cbRol.SelectedIndex)
                                    {
                                        case 0: U_ROL = "C"; break;
                                        case 1: U_ROL = "A"; break;
                                        case 2: U_ROL = "S"; break;
                                    }

                                    string QUERY = "UPDATE USUARIOS SET Usuario = @Usuario, Contraseña = @Pass, Requisitante = @Requi, Tesoreria = @Teso, Nombre = @Nombre, Estatus = '" + U_ESTATUS + "', Correo = @Correo, U_ROL = '" + U_ROL + "', Departamento = @Depa, Clave_Depto = @Clave WHERE Usuario_id = " + U_ID;

                                    SqlCommand cmd2 = new SqlCommand(QUERY, conSql1.SC);
                                    cmd2.Parameters.Add("@Usuario", SqlDbType.VarChar).Value = U_USERNAME;
                                    cmd2.Parameters.Add("@Pass", SqlDbType.VarChar).Value = U_PASSWORD;
                                    cmd2.Parameters.Add("@Requi", SqlDbType.VarChar).Value = Requi;
                                    cmd2.Parameters.Add("@Teso", SqlDbType.VarChar).Value = Teso;
                                    cmd2.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = U_FULLNAME;
                                    cmd2.Parameters.Add("@Correo", SqlDbType.VarChar).Value = U_EMAIL;
                                    cmd2.Parameters.Add("@Depa", SqlDbType.VarChar).Value = departamento;
                                    cmd2.Parameters.Add("@Clave", SqlDbType.VarChar).Value = clave;
                                    cmd2.ExecuteNonQuery();
                                    cmd2.Dispose();

                                    MessageBox.Show("Los datos se han modificado exitosamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    
                                    borrarDatos();

                                    result = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString(), "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                if (necesitaUsuarioMicrosip)
                                    conexion_fb.Desconectar();

                                conSql1.Desconectar();
                            }

                            #endregion
                            break;
                    }
                }
            }

            return result;
        }

        private void borrarDatos()
        {
            tbConfirmPassword.Clear();
            tbCorreoElectronico.Clear();
            tbFullUsername.Clear();
            tbPassword.Clear();
            tbUsername.Clear();
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            cbDptos.SelectedIndex = 0;
        }

        private void C_CONTROLUSUARIOS_Load(object sender, EventArgs e)
        {
            cbEstatus.SelectedIndex = 0;
            cbRol.SelectedIndex = 0;
            cbRequi.SelectedIndex = 0;
            cbTeso.SelectedIndex = 0;
        }

        private void F_CONTROLUSUARIOS_Shown(object sender, EventArgs e)
        {
            SELECT_USUARIOS();
            CargarDeptos();
            txtUser.Text = usuarioLogueado.Usuario;
            //txtEmpresa.Text = NOMBRE_EMPRESA;
        }





        private void crearUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            SELECTED_OPTION = 1; // CREAR USUARIO
            SELECTED_ID = 0;

            // Datos del usuario
            groupBox2.Enabled = true;            

            tbFullUsername.Text = "";
            tbCorreoElectronico.Text = "";
            cbEstatus.SelectedIndex = 0;
            cbRol.SelectedIndex = 0;

            // Datos para el inicio de sesión
            groupBox3.Enabled = true;

            tbUsername.Text = "";
            tbPassword.Text = "";
            tbConfirmPassword.Text = "";

           
        }

        private void modificarUsuarioSeleccionadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SELECTED_OPTION = 2; // MODIFICAR USUARIO
                SELECTED_ID = int.Parse(dgvUserData[0, dgvUserData.CurrentCell.RowIndex].Value.ToString());

                // Datos del usuario
                groupBox2.Enabled = true;

                tbFullUsername.Text = Convert.ToString(dgvUserData["Nombre", dgvUserData.CurrentCell.RowIndex].Value);
                tbCorreoElectronico.Text = Convert.ToString(dgvUserData["Correo", dgvUserData.CurrentCell.RowIndex].Value);
                cbEstatus.SelectedIndex = (Convert.ToString(dgvUserData["Estatus", dgvUserData.CurrentCell.RowIndex].Value) == "Activo") ? 0 : 1;
                cbRol.SelectedIndex = (Convert.ToString(dgvUserData["Rol", dgvUserData.CurrentCell.RowIndex].Value) == "Capturista") ? 0 : 1;
                cbDptos.Text = Convert.ToString(dgvUserData["Departamento", dgvUserData.CurrentCell.RowIndex].Value);
                cbRequi.SelectedIndex = (Convert.ToString(dgvUserData["REQUI", dgvUserData.CurrentCell.RowIndex].Value) == "Si") ? 0 : 1;
                cbTeso.SelectedIndex = (Convert.ToString(dgvUserData["TESORERIA", dgvUserData.CurrentCell.RowIndex].Value) == "Si") ? 0 : 1;

                // Datos para el inicio de sesión
                groupBox3.Enabled = true;

                tbUsername.Text = Convert.ToString(dgvUserData["User", dgvUserData.CurrentCell.RowIndex].Value);
                tbPassword.Text = Convert.ToString(dgvUserData["Pass", dgvUserData.CurrentCell.RowIndex].Value);
                tbConfirmPassword.Text = Convert.ToString(dgvUserData["Pass", dgvUserData.CurrentCell.RowIndex].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error al cargar los datos del usuario.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Datos del usuario
                groupBox2.Enabled = false;

                tbFullUsername.Text = "";
                tbCorreoElectronico.Text = "";
                cbEstatus.SelectedIndex = 0;
                cbRol.SelectedIndex = 0;

                // Datos para el inicio de sesión
                groupBox3.Enabled = false;

                tbUsername.Text = "";
                tbPassword.Text = "";
                tbConfirmPassword.Text = "";
            }
        }





        private void btnCheckCoincidences_Click(object sender, EventArgs e)
        {

            if(string.IsNullOrEmpty(cbDptos.Text))
            {
                MessageBox.Show("No ha seleccionado el departamento","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            if ((tbFullUsername.Text.Trim() != "") && (tbCorreoElectronico.Text.Trim() != "") && (tbUsername.Text.Trim() != "") && (tbPassword.Text.Trim() != "") && (tbConfirmPassword.Text.Trim() != ""))
            {
                if (tbPassword.Text == tbConfirmPassword.Text)
                {
                    if (MessageBox.Show("¿Desea guardar los datos del usuario?", "Mensaje de pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (SELECT_FUNCTION())
                        {
                            SELECT_USUARIOS();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Las contrasenas no coinciden, introduzca nuevamente las contraseñas y asegurese que sean iguales.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    tbPassword.Text = "";
                    tbConfirmPassword.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Necesita proporcionar todos los datos del usuario.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarDeptos()
        {
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            if (reg.LeerRegistros(false))
            {
                C_CONEXIONFIREBIRD con = new C_CONEXIONFIREBIRD();
                if (con.ConectarFB("DEMO"))
                {
                    FbCommand fb = new FbCommand("SELECT * FROM DEPTOS_CO", con.FBC);
                    FbDataReader fdr = fb.ExecuteReader();
                    DEPTOS[] dpto = new DEPTOS[0];
                    cbDptos.Items.Clear();
                    while (fdr.Read())
                    {
                        Array.Resize(ref dpto, dpto.Length+1);
                        dpto[dpto.Length - 1] = new DEPTOS();

                        dpto[dpto.Length - 1].DEPTO_CO_ID = Convert.ToInt32(Convert.ToString(fdr["DEPTO_CO_ID"]));
                        dpto[dpto.Length - 1].NOMBRE = Convert.ToString(fdr["NOMBRE"]);
                        dpto[dpto.Length - 1].CLAVE_DEPTO = Convert.ToString(fdr["CLAVE"]);
                        cbDptos.Items.Add(dpto[dpto.Length-1]);
                    }
                    con.Desconectar();

                    if (cbDptos.Items.Count > 0)
                        cbDptos.SelectedItem = 0;
                }
            }
        }

        private void nuevoUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crearUsuarioToolStripMenuItem.PerformClick();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void asignaciónNivelesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_NIVELES niveles = new F_NIVELES();
            niveles.ShowDialog();
        }
    }
}