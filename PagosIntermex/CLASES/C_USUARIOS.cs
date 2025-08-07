using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PagosIntermex
{
    public class C_USUARIOS
    {
        public C_USUARIOS()
        {

        }

        public int Usuario_id { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string Requisitante { get; set; }
        public string Tesoreria { get; set; }
        public string Departamento { get; set; }
        public string Correo { get; set; }
        public string Privilegio { get; set; }
        public string Estatus { get; set; }
        public string Clave_Depto { get; set; }
        public string U_ROL { get; set; }

        public int NIVEL { get; set; }

        public int NIVEL_SUPREMO { get; set; }

        public override string ToString()
        {
            return Nombre.ToString();
        }

        public int GET_USUARIO_ID(string usuario)
        {
            int usuario_id = 0;
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    if (con.ConectarSQL())
                    {
                        string query = "SELECT Usuario_id from USUARIOS WHERE Usuario = '" + usuario + "'";
                        SqlCommand sc = new SqlCommand(query, con.SC);
                        SqlDataReader sdr = sc.ExecuteReader();

                        while (sdr.Read())
                        {
                            usuario_id = Convert.ToInt32(Convert.ToString(sdr["Usuario_id"]));

                        }
                        sc.Dispose();
                        sdr.Close();
                    }

                    con.Desconectar();
                }
            }
            catch
            {

            }

            return usuario_id;
        }

        public string TIPO_USUARIO()
        {
            string tipo_usuario = "";
            try
            {
                switch (U_ROL)
                {
                    case "C": tipo_usuario = "Capturista";break;
                    case "A": tipo_usuario = "Autorizador";break;
                }
            }
            catch 
            {

            }
            return tipo_usuario;
        }

        public int[] NIVEL_USUARIO(int usuario_id)
        {
            int[] nivel = new int[0];
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string query = " select n.NIVEL FROM NIVELES n ";
                    query += " LEFT join USUARIOS uu on uu.Clave_Depto = n.DEPTO ";
                    query += " left join USUARIOS u on u.Usuario_id = n.USUARIO_ID ";
                    query += " WHERE uu.Usuario_id = " + usuario_id + " or u.Usuario_id = " + usuario_id;
                    query += "  order by n.NIVEL asc ";

                    SqlCommand sc = new SqlCommand(query, con.SC);
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        Array.Resize(ref nivel, nivel.Length + 1);

                        nivel[nivel.Length-1] = Convert.ToInt32(Convert.ToString(sdr["NIVEL"]));
                    }
                    sdr.Close();
                    sc.Dispose();

                    con.Desconectar();

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error\n" + ex.Message,"Mensaje de Pagos");
            }
            return nivel;
        }

        public int NIVEL_DEPTO(string depto)
        {
            int nivel = 0;
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string query = " select n.NIVEL FROM NIVELES n ";
                    query += " WHERE n.DEPTO = '" + depto + "'";

                    SqlCommand sc = new SqlCommand(query, con.SC);
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                        nivel = Convert.ToInt32(Convert.ToString(sdr["NIVEL"]));
                    sdr.Close();
                    sc.Dispose();

                    con.Desconectar();

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error\n" + ex.Message, "Mensaje de Pagos");
            }
            return nivel;
        }
    }

}
