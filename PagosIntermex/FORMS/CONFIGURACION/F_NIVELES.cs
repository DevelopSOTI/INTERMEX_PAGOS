using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace PagosIntermex
{
    public partial class F_NIVELES : Form
    {
        public F_NIVELES()
        {
            InitializeComponent();
        }

        bool SeleccionDpto = true, seleccionUsuario = false;
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

        List<DEPTOS> listaDeptos = new List<DEPTOS>();
        List<C_USUARIOS> listaUsuarios = new List<C_USUARIOS>();

        class NIVEL
        {
            public int NIVE { get; set; }
            public int USUARIO_ID { get; set; }
            public string DEPTO { get; set; }
            public string USUARIO { get; set; }
        }

        NIVEL nivel = new NIVEL();

        private void F_NIVELES_Shown(object sender, EventArgs e)
        {
            Cargar();
        }

        private void Cargar()
        {
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    #region DEPARTAMENTOS
                    C_CONEXIONFIREBIRD con = new C_CONEXIONFIREBIRD();
                    if (con.ConectarFB("DEMO"))
                    {
                        FbCommand fb = new FbCommand("SELECT * FROM DEPTOS_CO ORDER BY NOMBRE", con.FBC);
                        FbDataReader fdr = fb.ExecuteReader();
                        DEPTOS[] dpto = new DEPTOS[0];
                        cbDptos.Items.Clear();
                        while (fdr.Read())
                        {
                            Array.Resize(ref dpto, dpto.Length + 1);
                            dpto[dpto.Length - 1] = new DEPTOS();

                            dpto[dpto.Length - 1].DEPTO_CO_ID = Convert.ToInt32(Convert.ToString(fdr["DEPTO_CO_ID"]));
                            dpto[dpto.Length - 1].NOMBRE = Convert.ToString(fdr["NOMBRE"]);
                            dpto[dpto.Length - 1].CLAVE_DEPTO = Convert.ToString(fdr["CLAVE"]);
                            cbDptos.Items.Add(dpto[dpto.Length - 1]);
                            listaDeptos.Add(dpto[dpto.Length - 1]);
                        }
                        con.Desconectar();

                        if (cbDptos.Items.Count > 0)
                            cbDptos.SelectedItem = 0;
                    }
                    #endregion

                    #region USUARIOS
                    C_ConexionSQL conSQL = new C_ConexionSQL();
                    if (conSQL.ConectarSQL())
                    {

                        string query = "SELECT * FROM USUARIOS WHERE ESTATUS ='A'";
                        SqlCommand sc = new SqlCommand(query, conSQL.SC);
                        SqlDataReader sdr = sc.ExecuteReader();
                        C_USUARIOS[] usuarios = new C_USUARIOS[0];
                        while (sdr.Read())
                        {
                            Array.Resize(ref usuarios, usuarios.Length + 1);
                            usuarios[usuarios.Length - 1] = new C_USUARIOS();

                            usuarios[usuarios.Length - 1].Usuario_id = Convert.ToInt32(Convert.ToString(sdr["Usuario_id"]));
                            usuarios[usuarios.Length - 1].Usuario = Convert.ToString(sdr["Usuario"]);
                            usuarios[usuarios.Length - 1].Nombre = Convert.ToString(sdr["Nombre"]);

                            cbUsuarios.Items.Add(usuarios[usuarios.Length - 1]);
                            listaUsuarios.Add(usuarios[usuarios.Length - 1]);
                        }


                        CargarGrid(conSQL);
                        conSQL.Desconectar();
                    }
                    #endregion

                    cbDptos.SelectedIndex = 1;
                    cbUsuarios.SelectedIndex = 0;
                    cbDptos.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error\n" + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void CargarGrid(C_ConexionSQL con)
        {
            try
            {
                dgvNiveles.Rows.Clear();
                SqlCommand sc;
                SqlDataReader sdr;

                sc = new SqlCommand("SELECT * FROM NIVELES",con.SC);
                sdr = sc.ExecuteReader();
                while (sdr.Read())
                {
                    dgvNiveles.Rows.Add();

                    dgvNiveles["Nivel", dgvNiveles.RowCount - 1].Value = Convert.ToString(sdr["NIVEL"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(sdr["DEPTO"])))
                    {
                        dgvNiveles["Nombre", dgvNiveles.RowCount - 1].Value = listaDeptos[listaDeptos.FindIndex(x => x.CLAVE_DEPTO == Convert.ToString(sdr["DEPTO"]))].NOMBRE;
                        dgvNiveles["Departamento", dgvNiveles.RowCount - 1].Value = Convert.ToString(sdr["DEPTO"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(sdr["USUARIO_ID"])))
                    {
                        dgvNiveles["User", dgvNiveles.RowCount - 1].Value = listaUsuarios[listaUsuarios.FindIndex(x => x.Usuario_id == Convert.ToInt32(Convert.ToString(sdr["USUARIO_ID"])))].Usuario;
                        dgvNiveles["Usuario_id", dgvNiveles.RowCount - 1].Value = Convert.ToString(sdr["USUARIO_ID"]);
                    }
                }
                sc.Dispose();
                sdr.Close();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SeleccionDpto)
            {
                seleccionUsuario = true;
                cbDptos.SelectedIndex = -1;
            }
            SeleccionDpto = false;
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbNivel.SelectedIndex >= 0)
            {
                try
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    SqlTransaction tran;
                    SqlCommand sc;
                    bool existe = false;

                    if (con.ConectarSQL())
                    {
                        #region Checar si existe
                        string query = " SELECT * FROM NIVELES ";
                        query += " WHERE NIVEL = " + cbNivel.Text;
                        if (cbUsuarios.SelectedIndex > 0)
                        {
                            C_USUARIOS user = cbUsuarios.SelectedItem as C_USUARIOS;
                            query += " AND USUARIO_ID = " + user.Usuario_id;
                        }
                        else
                        {
                            DEPTOS depto = cbDptos.SelectedItem as DEPTOS;
                            query += " AND DEPTO = '" + depto.CLAVE_DEPTO + "'";
                        }
                        sc = new SqlCommand(query, con.SC);
                        SqlDataReader sdr = sc.ExecuteReader();
                        while (sdr.Read())
                        {
                            existe = true;
                        }
                        sc.Dispose();
                        sdr.Close();
                        #endregion


                        tran = con.SC.BeginTransaction();

                        if (existe)
                        {
                            MessageBox.Show("Ya hay un nivel asignado con estas caracteristicas.\n¿Desea actualizar este nivel con los nuevos datos?",
                                "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tran.Rollback();
                            con.Desconectar();
                            return;
                        }
                        else {
                            string insert = "INSERT INTO NIVELES(NIVEL";
                            insert += cbUsuarios.SelectedIndex > 0 ? " ,USUARIO_ID) " : " ,DEPTO) ";
                            insert += " VALUES (@NIVEL";
                            insert += cbUsuarios.SelectedIndex > 0 ? " ,@USUARIO_ID) " : " ,@DEPTO) ";

                            sc = new SqlCommand(insert, con.SC, tran);
                            sc.Parameters.Add("@NIVEL", SqlDbType.Int).Value = cbNivel.Text;
                            if (cbUsuarios.SelectedIndex > 0)
                            {
                                C_USUARIOS user = cbUsuarios.SelectedItem as C_USUARIOS;
                                sc.Parameters.Add("@USUARIO_ID", SqlDbType.Int).Value = user.Usuario_id;
                            }
                            else
                            {
                                DEPTOS depto = cbDptos.SelectedItem as DEPTOS;
                                sc.Parameters.Add("@DEPTO", SqlDbType.VarChar).Value = depto.CLAVE_DEPTO;
                            }
                            sc.ExecuteNonQuery();
                            tran.Commit();
                            MessageBox.Show("Se inserto correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        CargarGrid(con);
                        con.Desconectar();
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                }

            }
            else
            {
                MessageBox.Show("No ha seleccionado el nivel","Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void dgvNiveles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            nivel = new NIVEL();
            try
            {
                nivel.NIVE = Convert.ToInt32(Convert.ToString(dgvNiveles["Nivel", e.RowIndex].Value));
                if (!string.IsNullOrEmpty(Convert.ToString(dgvNiveles["Usuario_id", e.RowIndex].Value)))
                {
                    nivel.USUARIO_ID = Convert.ToInt32(Convert.ToString(dgvNiveles["Usuario_id", e.RowIndex].Value));
                    nivel.USUARIO = listaUsuarios[listaUsuarios.FindIndex(x => x.Usuario_id == Convert.ToInt32(Convert.ToString(dgvNiveles["Usuario_id", e.RowIndex].Value)))].Usuario;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dgvNiveles["Departamento", e.RowIndex].Value)))
                {
                    nivel.DEPTO = Convert.ToString(dgvNiveles["Departamento", e.RowIndex].Value);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void eliminarNivelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Eliminar de forma permanente el nivel " + nivel.NIVE + " " + nivel.DEPTO + " " + nivel.USUARIO, "Mensaje de la aplicación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            C_ConexionSQL con = new C_ConexionSQL();
            SqlTransaction tran;
            if (con.ConectarSQL())
            {
                tran = con.SC.BeginTransaction();
                try
                {
                    string query = "DELETE FROM NIVELES WHERE NIVEL = " + nivel.NIVE;
                    query += !string.IsNullOrEmpty(nivel.DEPTO) ? " AND DEPTO = '" + nivel.DEPTO + "' " : " AND USUARIO_ID = " + nivel.USUARIO_ID;

                    SqlCommand sc = new SqlCommand(query, con.SC, tran);
                    sc.ExecuteNonQuery();
                    tran.Commit();
                    sc.Dispose();
                    tran.Dispose();
                    CargarGrid(con);
                    con.Desconectar();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Hubo un error\n" + ex.Message,"Mensaje de la aplicación");
                }
            }
        }

        private void cbDptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!seleccionUsuario)
            {
                SeleccionDpto = true;
                cbUsuarios.SelectedIndex = -1;
            }
            seleccionUsuario = false;
            
        }
    }
}
