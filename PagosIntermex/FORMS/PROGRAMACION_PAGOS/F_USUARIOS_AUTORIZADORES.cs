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
    public partial class F_USUARIOS_AUTORIZADORES : Form
    {
        public C_USUARIOS[] usuarios = new C_USUARIOS[0];
        public string DEPTO { get; internal set; }

        public F_USUARIOS_AUTORIZADORES()
        {
            InitializeComponent();
        }

        private void F_USUARIOS_AUTORIZADORES_Shown(object sender, EventArgs e)
        {
            if(DEPTO != "")
            {
                lbselecc.Text = "Seleccionar usuarios autorizadores del departamento con clave " + DEPTO;
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string query = "SELECT * ";
                    query += "FROM USUARIOS u ";
                    query += " WHERE u.Clave_Depto = '" + DEPTO + "'";
                    query += " AND u.U_ROL = 'A' AND Estatus = 'A'";

                    SqlCommand sc = new SqlCommand(query,con.SC);
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        dgvUsers.Rows.Add();
                        dgvUsers["USUARIO_ID", dgvUsers.RowCount - 1].Value = Convert.ToInt32(Convert.ToString(sdr["Usuario_id"]));
                        dgvUsers["NOMBRE", dgvUsers.RowCount - 1].Value = Convert.ToString(sdr["Nombre"]);
                        dgvUsers["USUARIO", dgvUsers.RowCount - 1].Value = Convert.ToString(sdr["Usuario"]);
                        dgvUsers["DEPARTAMENTO", dgvUsers.RowCount - 1].Value = Convert.ToString(sdr["Departamento"]);
                        dgvUsers["PRIVILEGIO", dgvUsers.RowCount - 1].Value = Convert.ToString(sdr["Privilegio"]);

                       
                    }



                    con.Desconectar();
                }
            }
            else
            {
                MessageBox.Show("No hay departamento seleccionado","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            usuarios = new C_USUARIOS[0];
            for (int i = 0; i < dgvUsers.Rows.Count; i++)
            {
                // if (dgvProgramas["dgvp_check", i].Value != null)
                if (Convert.ToBoolean(dgvUsers["checkUsuario", i].Value))
                {
                    Array.Resize(ref usuarios, usuarios.Length + 1);
                    usuarios[usuarios.Length - 1] = new C_USUARIOS();

                    usuarios[usuarios.Length - 1].Usuario_id = Convert.ToInt32(dgvUsers["USUARIO_ID", i].Value);
                    usuarios[usuarios.Length - 1].Nombre = dgvUsers["NOMBRE", i].Value.ToString();                    
                }
            }

            if (usuarios.Length > 0)
            {
                if (MessageBox.Show("¿Seguro que desea continuar?", "Mensaje de la aplicación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                Close();
            }
        }
    }
}
