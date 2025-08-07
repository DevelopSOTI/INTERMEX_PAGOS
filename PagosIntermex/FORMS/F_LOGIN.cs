using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FirebirdSql.Data.FirebirdClient;

namespace PagosIntermex
{
    public partial class F_LOGIN : Form
    {
        private ArrayList ID_EMPRESAS = new ArrayList();

        string[] MSG_ERROR = new string[3];
        C_USUARIOS usuario = new C_USUARIOS();
        C_EMPRESAS[] emp = new C_EMPRESAS[0];


        public F_LOGIN()
        {
            InitializeComponent();

            MSG_ERROR[0] = "El usuario no existe en el sistema, verifique que los campos escritos sean correctos.";
            MSG_ERROR[1] = "La contraseña es incorrecta.";
            MSG_ERROR[2] = "El usuario se encuentra actualmente dado de baja.";

           // MessageBox.Show(DateTime.Now.ToShortTimeString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

            if (registros.LeerRegistros(false))
            {
                C_CONEXIONWEBSVC CCWEBSVC = new C_CONEXIONWEBSVC();

                ArrayList[] MY_DATA = CCWEBSVC.GET_MYSQL_INFORMATION(registros.WEBSVC_DOMINIO + "/SIMSA_FUNCTIONS/SIMSA_GetAllUsernames.php",
                    "conn_hostname=" + registros.WEBSVC_HOST +
                    "&conn_username=" + registros.WEBSVC_MYSQLUSER +
                    "&conn_password=" + registros.WEBSVC_MYSQLPASSWORD,
                    2);
            } // */
        }

        private void F_LOGIN_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            C_REGISTROSWINDOWS CRW = new C_REGISTROSWINDOWS();
            if (!CRW.LeerRegistros(false))
            {
                CRW.CrearRegistros(false);

                F_CONFIGCONEXIONES FCC = new F_CONFIGCONEXIONES();
                FCC.ShowDialog();
            }

            notifyIcon1.Icon =
   new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + @"\Icon.ico");
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Sistema de Pagos";
        }





        // BOTON PARA INICIAR SESION
        private void button1_Click(object sender, EventArgs e)
        {
            /*if(cbEmpresas.SelectedIndex <= 0)
            {
                MessageBox.Show("No ha seleccionado la empresa","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            } */   
            string _usuario, _contraseña, _empresa_nombre;
            int _empresa_ID;

            _usuario = tbUser.Text;
            _contraseña = tbPassword.Text;

           /* EMPRESAS emp = cbEmpresas.SelectedItem as EMPRESAS;

            _empresa_ID = emp.EMPRESA_ID;
            _empresa_nombre = emp.NOMBRE_CORTO;*/


            bool SUCCESS = false;

            if (cbEmpresas.SelectedIndex == 0)
            {
                MessageBox.Show("Necesita indicar la empresa Microsip con la que quiere trabajar.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
            if (registros.LeerRegistros(false))
            {
                C_ConexionSQL conexion_sql = new C_ConexionSQL();
                SqlCommand cmd;
                SqlDataReader reader;

                if (conexion_sql.ConectarSQL())
                {
                    try
                    {
                        string QUERY = "  SELECT COUNT(*) AS INSIDENCES,* ";
                        QUERY += " FROM USUARIOS WHERE Usuario = '" + _usuario + "' AND Contraseña = '" + _contraseña + "' ";
                        QUERY += " AND Estatus = 'A' ";
                        QUERY += "GROUP BY [Nombre],[Usuario],[Contraseña],[Requisitante],[Tesoreria] ";
                        QUERY += " ,[Departamento],[Correo],[Privilegio],[Estatus],[Usuario_Creador]";
                        QUERY += " ,[Fecha_Creacion],[Fecha_Ultima_Modificacion],[Usuario_Modificador] ";
                        QUERY += " ,[Clave_Depto],[U_ROL],Usuario_id ";

                        cmd = new SqlCommand(QUERY, conexion_sql.SC);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (int.Parse(reader["INSIDENCES"].ToString()) > 0)
                                {
                                    //ENCONTRO EL USUARIO Y LA CONTRASENA COINCIDIO.
                                    usuario.Usuario_id = Convert.ToInt32(Convert.ToString(reader["Usuario_id"]));
                                    usuario.Nombre = Convert.ToString(reader["Nombre"]);
                                    usuario.Usuario = Convert.ToString(reader["Usuario"]);
                                    usuario.Contraseña = Convert.ToString(reader["Contraseña"]);
                                    usuario.Requisitante = Convert.ToString(reader["Requisitante"]);
                                    usuario.Tesoreria = Convert.ToString(reader["Tesoreria"]);
                                    usuario.Departamento = Convert.ToString(reader["Departamento"]);
                                    usuario.Correo = Convert.ToString(reader["Correo"]);
                                    usuario.Privilegio = Convert.ToString(reader["Privilegio"]);
                                    usuario.Clave_Depto = Convert.ToString(reader["Clave_Depto"]);      
                                    usuario.U_ROL = Convert.ToString(reader["U_ROL"]);
                                    int[] nivelesUsuario = usuario.NIVEL_USUARIO(Convert.ToInt32(Convert.ToString(reader["Usuario_id"])));
                                    if(nivelesUsuario.Length > 1)
                                    {
                                        usuario.NIVEL_SUPREMO = nivelesUsuario[0];
                                        usuario.NIVEL = nivelesUsuario[1];
                                    }
                                    else
                                    {
                                        usuario.NIVEL = nivelesUsuario[0];
                                    }
                                    SUCCESS = true;
                                }
                            }
                        }
                        reader.Close();
                        cmd.Dispose();

                        if (SUCCESS)
                        {
                            this.Visible = false;

                            F_PROGRAMAS FEE = new F_PROGRAMAS();
                            FEE.usuarioLogueado = usuario;
                            FEE.empresas = emp;
                            notifyIcon1.Visible = false;
                            FEE.ShowDialog();
                            usuario = new C_USUARIOS();
                            this.Visible = true;
                        }
                        else
                        {
                            int NO_ERROR = 0;

                            QUERY = "SELECT COUNT(*) AS INSIDENCES FROM USUARIOS WHERE Usuario = '" + _usuario + "' AND Estatus = 'A'";
                            cmd = new SqlCommand(QUERY, conexion_sql.SC);
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                // CONTRASENA INCORRECTA
                                while (reader.Read())
                                {
                                    if (int.Parse(reader["INSIDENCES"].ToString()) > 0)
                                    {
                                        NO_ERROR = 1;
                                    }
                                }
                            }
                            reader.Close();
                            cmd.Dispose();

                            if (NO_ERROR == 0)
                            {
                                QUERY = "SELECT COUNT(*) AS INSIDENCES FROM USUARIOS WHERE Usuario = '" + _usuario + "' AND Estatus = 'B'";
                                cmd = new SqlCommand(QUERY, conexion_sql.SC);
                                reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    // USUARIO DADO DE BAJA
                                    while (reader.Read())
                                    {
                                        if (int.Parse(reader["INSIDENCES"].ToString()) > 0)
                                        {
                                            NO_ERROR = 2;
                                        }
                                    }
                                }
                            }

                            MessageBox.Show(MSG_ERROR[NO_ERROR], "Mensaje del pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No fue posible iniciar sesion.\n\n" + ex.ToString(), "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    conexion_sql.Desconectar();
                }
            }
        }

        // BOTON DE CERRAR
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            F_CONFIGCONEXIONES FCC = new F_CONFIGCONEXIONES();
            FCC.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            F_CONTROLUSUARIOS users = new F_CONTROLUSUARIOS();
            users.ShowDialog();
        }





        private void tbUser_Leave(object sender, EventArgs e)
        {
            if (tbUser.Text != "SYSDBA")
            {
                BUSCAR_EMPRESAS();
            }
        }

        private void tbUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbUser.Text != "SYSDBA")
                {
                    BUSCAR_EMPRESAS();
                }

                tbPassword.Focus();
            }
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbEmpresas.Focus();

                if ((tbUser.Text == "SYSDBA") && tbPassword.Text == "masterkey")
                {
                    linkLabel2.Visible = true;
                }
            }
        }



        private void BUSCAR_EMPRESAS()
        {
            if (tbUser.Text.Trim() != "")
            {
                // Variables locales:
                C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

                if (registros.LeerRegistros(false))
                {
                    string USERNAME = tbUser.Text.ToString();
                    cbEmpresas.Items.Clear();
                    ID_EMPRESAS.Clear();

                    C_CONEXIONFIREBIRD conexion_fb = new C_CONEXIONFIREBIRD();
                    FbCommand cmd;
                    FbDataReader reader;

                    try
                    {
                        //buscamos si es administrador o capturista en SQL

                        C_ConexionSQL sql = new C_ConexionSQL();
                        bool esAdmin = false;
                        if (sql.ConectarSQL())
                        {
                            SqlCommand sc = new SqlCommand("SELECT * FROM USUARIOS WHERE USUARIO = '" + USERNAME + "'",sql.SC);
                            SqlDataReader sdr = sc.ExecuteReader();
                            string u_rol = "";
                            while (sdr.Read())
                            {
                                u_rol = Convert.ToString(sdr["U_ROL"]);
                                usuario.U_ROL = u_rol;
                            }

                            if (u_rol == "A")
                                esAdmin = true;

                            sdr.Close();
                            sc.Dispose();

                            /*switch (usuario.U_ROL)
                            {
                                case "A": TIPO_USUARIO = "Administrador";break;
                                case "C": TIPO_USUARIO = "Capturista";break;
                                case "S": TIPO_USUARIO = "Sistemas";break;
                            }*/
                            sql.Desconectar();
                        }

                        //seleccionamos todas las empresas
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
                                    emp[emp.Length - 1] = new C_EMPRESAS();

                                    emp[emp.Length - 1].NOMBRE_CORTO = Convert.ToString(reader["NOMBRE_CORTO"]);
                                    emp[emp.Length - 1].EMPRESA_ID = Convert.ToInt32(Convert.ToString(reader["EMPRESA_ID"]));

                                }
                            }
                            reader.Close();
                            cmd.Dispose();

                            conexion_fb.Desconectar();
                        }

                        /* if (esAdmin)
                         {
                             if (conexion_fb.ConectarFB_MANUAL("SYSDBA", registros.FB_PASSWORD, registros.FB_ROOT, registros.FB_SERVIDOR))
                             {
                                 EMPRESAS[] emp = new EMPRESAS[0];
                                 string QUERY = "SELECT NOMBRE_CORTO, EMPRESA_ID FROM EMPRESAS ORDER BY NOMBRE_CORTO";
                                 cbEmpresas.Items.Add("Seleccionar...");

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

                                         cbEmpresas.Items.Add(emp[emp.Length - 1]);

                                     }
                                 }
                                 reader.Close();
                                 cmd.Dispose();

                                 cbEmpresas.SelectedIndex = 0;
                             }
                         }
                         else if (conexion_fb.ConectarFB_MANUAL("SYSDBA",registros.FB_PASSWORD,registros.FB_ROOT,registros.FB_SERVIDOR))
                         {
                             // BUSCAR SI LA LISTA DE EMPRESAS SERA ESPECIFICA O TOTAL:
                             string QUERY = "SELECT ACCESO_EMPRESAS, USUARIO_ID FROM USUARIOS WHERE NOMBRE = '" + USERNAME + "'";
                             string RESULT = "";
                             string USER_ID = "";

                             cmd = new FbCommand(QUERY, conexion_fb.FBC);
                             reader = cmd.ExecuteReader();
                             if (reader.HasRows)
                             {
                                 while (reader.Read())
                                 {
                                     RESULT = reader["ACCESO_EMPRESAS"].ToString();
                                     USER_ID = reader["USUARIO_ID"].ToString();

                                 }
                             }
                             reader.Close();
                             cmd.Dispose();

                             switch (RESULT)
                             {
                                 case "L": // LISTA DE EMPRESAS
                                     QUERY = "SELECT E.NOMBRE_CORTO, E.EMPRESA_ID FROM EMPRESAS AS E INNER JOIN EMPRESAS_USUARIOS AS EU ON (EU.EMPRESA_ID = E.EMPRESA_ID) WHERE EU.USUARIO_ID = " + USER_ID + " ORDER BY E.NOMBRE_CORTO";
                                     break;
                                 case "T": // TODAS LAS EMPESAS
                                     QUERY = "SELECT NOMBRE_CORTO, EMPRESA_ID FROM EMPRESAS ORDER BY NOMBRE_CORTO";
                                     break;
                                 default:
                                     QUERY = "";
                                     break;
                             }

                             if (QUERY != "")
                             {
                                 cbEmpresas.Items.Add("Seleccionar...");

                                 EMPRESAS[] emp = new EMPRESAS[0];
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

                                         cbEmpresas.Items.Add(emp[emp.Length - 1]);
                                     }
                                 }
                                 reader.Close();
                                 cmd.Dispose();

                                 cbEmpresas.SelectedIndex = 0;
                             }
                             else
                             {
                                 MessageBox.Show("El usuario '" + USERNAME + "' no esta registrado en Microsip o no tiene acceso a ninguna empresa.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                 tbUser.Text = "";
                             }
                         }*/
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No fue posible obtener el listado de empresas a las que el usuario tiene acceso.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            GraphicsPath graphics = new GraphicsPath();
            Rectangle rec = button1.ClientRectangle;
            rec.Inflate(0, 30);
            graphics.AddEllipse(rec);
            button1.Region = new Region(graphics);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void tbUser_TextChanged(object sender, EventArgs e)
        {
           // if (tbUser.Text == "USUARIO")
               // dgvBusq.Rows[i].Visible = true;
            
        }

        private void tbUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbUser.Text == "USUARIO")
            {
                tbUser.Text = "";
                tbUser.ForeColor = Color.White;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            TopMost = true;
        }
    }
}
