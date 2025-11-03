using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagosIntermex
{
    internal class C_FUNCIONES
    {
        public bool CrearCamposParticulares(int emp_id)
        {
            bool creado = false;
            string empresa = "";
            C_CONEXIONFIREBIRD msp = new C_CONEXIONFIREBIRD();
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            if (reg.LeerRegistros(false))
            {
                if (msp.ConectarFB_MANUAL("SYSDBA", reg.FB_PASSWORD, reg.FB_ROOT, reg.FB_SERVIDOR))
                {
                    string query = "SELECT NOMBRE_CORTO ";
                    query += "  FROM EMPRESAS";
                    query += "  WHERE EMPRESA_ID = " + emp_id;
                    FbCommand fb = new FbCommand(query, msp.FBC);
                    FbDataReader fdr = fb.ExecuteReader();

                    while (fdr.Read())
                    {
                        empresa = Convert.ToString(fdr["NOMBRE_CORTO"]);
                    }
                    fb.Dispose();
                    fdr.Close();

                    msp.Desconectar();
                    msp = new C_CONEXIONFIREBIRD();

                    if (msp.ConectarFB(empresa))
                    {

                        fb = new FbCommand("SELECT * FROM LIBRES_PROVEEDOR", msp.FBC);
                        fdr = fb.ExecuteReader();
                        bool tiene_Campos_p = false;
                        while (fdr.Read())
                        {
                            try
                            {
                                Convert.ToString(fdr["CUENTA"]);
                                Convert.ToString(fdr["BANCO"]);
                                Convert.ToString(fdr["CLABE"]);

                                Convert.ToString(fdr["PAIS_BANCO"]);
                                Convert.ToString(fdr["NO_SUCURSAL"]);
                                Convert.ToString(fdr["ABA_BIC"]);

                                Convert.ToString(fdr["CONCEPTO_CIE"]);
                                Convert.ToString(fdr["CONVENIO_CIE"]);
                                Convert.ToString(fdr["BANCO_DIRECCION"]);

                                tiene_Campos_p = true;
                                break;

                            }
                            catch
                            {
                                tiene_Campos_p = false;
                                break;

                            }
                        }

                        if (!tiene_Campos_p)
                        {

                            FbTransaction tran = msp.FBC.BeginTransaction();
                            string campo = "";
                            try
                            {

                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = "ALTER TABLE LIBRES_PROVEEDOR ADD CUENTA VARCHAR(20)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'CUENTA','CUENTA','PROVEEDOR',";
                                        query += "20,'C',20,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "CUENTA\n";
                                    }
                                }
                                catch
                                {
                                    campo += "CUENTA\n";
                                }

                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = "ALTER TABLE LIBRES_PROVEEDOR ADD BANCO VARCHAR(10)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'BANCO','BANCO','PROVEEDOR',";
                                        query += "21,'C',10,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "BANCO\n";
                                    }

                                }
                                catch
                                {
                                    campo += "BANCO\n";
                                }

                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = " ALTER TABLE LIBRES_PROVEEDOR ADD CLABE VARCHAR(50)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'CLABE','CLABE','PROVEEDOR',";
                                        query += "22,'C',50,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "CLABE\n";
                                    }
                                }
                                catch
                                {
                                    campo += "CLABE\n";
                                }


                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = " ALTER TABLE LIBRES_PROVEEDOR ADD PAIS_BANCO VARCHAR(99)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'PAIS_BANCO','PAIS_BANCO','PROVEEDOR',";
                                        query += "22,'C',99,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "PAIS_BANCO\n";
                                    }
                                }
                                catch
                                {
                                    campo += "PAIS_BANCO\n";
                                }


                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = " ALTER TABLE LIBRES_PROVEEDOR ADD NO_SUCURSAL VARCHAR(4)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'NO_SUCURSAL','NO_SUCURSAL','PROVEEDOR',";
                                        query += "22,'C',4,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "NO_SUCURSAL\n";
                                    }
                                }
                                catch
                                {
                                    campo += "NO_SUCURSAL\n";
                                }


                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = " ALTER TABLE LIBRES_PROVEEDOR ADD ABA_BIC VARCHAR(15)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'ABA_BIC','ABA_BIC','PROVEEDOR',";
                                        query += "22,'C',15,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "ABA_BIC\n";
                                    }
                                }
                                catch
                                {
                                    campo += "ABA_BIC\n";
                                }

                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = " ALTER TABLE LIBRES_PROVEEDOR ADD CONCEPTO_CIE VARCHAR(30)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'CONCEPTO_CIE','CONCEPTO_CIE','PROVEEDOR',";
                                        query += "22,'C',30,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "CONCEPTO_CIE\n";
                                    }
                                }
                                catch
                                {
                                    campo += "CONCEPTO_CIE\n";
                                }


                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = " ALTER TABLE LIBRES_PROVEEDOR ADD CONVENIO_CIE VARCHAR(7)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'CONVENIO_CIE','CONVENIO_CIE','PROVEEDOR',";
                                        query += "22,'C',7,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "CONVENIO_CIE\n";
                                    }
                                }
                                catch
                                {
                                    campo += "CONVENIO_CIE\n";
                                }

                                try
                                {
                                    int gen_catalogo = GEN_CATALOGO_ID(msp, tran);
                                    if (gen_catalogo != 0)
                                    {
                                        query = " ALTER TABLE LIBRES_PROVEEDOR ADD BANCO_DIRECCION VARCHAR(40)";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();

                                        fb.Dispose();

                                        query = "INSERT INTO ATRIBUTOS(";
                                        query += "ATRIBUTO_ID,NOMBRE,NOMBRE_COLUMNA,";
                                        query += "CLAVE_OBJETO,POSICION,TIPO,LONGITUD,";
                                        query += "VALOR_MINIMO,VALOR_MAXIMO,VALOR_DEFAULT_NUMERICO,";
                                        query += "REQUERIDO)";
                                        query += " VALUES(";
                                        query += gen_catalogo + ",'BANCO_DIRECCION','BANCO_DIRECCION','PROVEEDOR',";
                                        query += "22,'C',40,0,0,0,'N')";
                                        fb = new FbCommand(query, msp.FBC, tran);
                                        fb.ExecuteNonQuery();
                                        fb.Dispose();
                                    }
                                    else
                                    {
                                        campo += "BANCO_DIRECCION\n";
                                    }
                                }
                                catch
                                {
                                    campo += "BANCO_DIRECCION\n";
                                }


                                if (string.IsNullOrEmpty(campo))
                                {
                                    tran.Commit();
                                    creado = true;
                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("No se pudieron crear algunos campos particulares porque ya estaban creados.\r\nCampos:\n" +
                                                                           campo + "\nSe insertaron los faltantes en la empresa " + empresa, "Advertencia",
                                                                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                                    tran.Commit();
                                    creado = true;
                                }

                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("violation of PRIMARY or UNIQUE KEY constraint "))
                                {
                                    System.Windows.Forms.MessageBox.Show("No se pudieron crear algunos campos particulares porque ya estaban creados.\r\nCampos:\n" +
                                        campo + "\nElimine los campos particulares de la empresa " + empresa + " y reintente nuevamente", "Advertencia", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                                }
                                tran.Rollback();
                                creado = false;
                            }
                        }


                    }
                    msp.Desconectar();

                }

                //si se crearon se inserta el campo en sql
                if (creado)
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    SqlTransaction trans;
                    if (con.ConectarSQL())
                    {
                        trans = con.SC.BeginTransaction();
                        try
                        {
                            string query = "INSERT INTO P_DPARTI_EMP(NOMBRE,EMP_ID_MSP,TIENE_DP)";
                            query += " VALUES(@NOMBRE,@EMP_ID,@TIENE_DP)";
                            SqlCommand sc = new SqlCommand(query, con.SC, trans);
                            sc.Parameters.Add("@NOMBRE", System.Data.SqlDbType.VarChar).Value = empresa;
                            sc.Parameters.Add("@EMP_ID", System.Data.SqlDbType.Int).Value = emp_id;
                            sc.Parameters.Add("@TIENE_DP", System.Data.SqlDbType.VarChar).Value = "S";
                            sc.ExecuteNonQuery();
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                        }
                        con.Desconectar();
                    }
                }
            }
            else
            {

            }


            return creado;
        }

        public C_EMPRESAS_DATOS_PARTICULARES[] TraerListaEmpresasDP()
        {
            C_EMPRESAS_DATOS_PARTICULARES[] emp = new C_EMPRESAS_DATOS_PARTICULARES[0];
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            C_ConexionSQL con = new C_ConexionSQL();
            if (reg.LeerRegistros(false))
            {
                if (con.ConectarSQL())
                {
                    string query = "SELECT * FROM P_DPARTI_EMP ";
                    SqlCommand fb = new SqlCommand(query, con.SC);
                    SqlDataReader fdr = fb.ExecuteReader();
                    while (fdr.Read())
                    {
                        Array.Resize(ref emp, emp.Length + 1);
                        emp[emp.Length - 1] = new C_EMPRESAS_DATOS_PARTICULARES();

                        emp[emp.Length - 1].NOMBRE = Convert.ToString(fdr["NOMBRE"]);
                        emp[emp.Length - 1].EMP_ID_MSP = Convert.ToInt32(Convert.ToString(fdr["EMP_ID_MSP"]));
                        emp[emp.Length - 1].TIENE_DP = Convert.ToString(fdr["TIENE_DP"]);


                    }
                    fb.Dispose();
                    fdr.Close();
                    con.Desconectar();
                }


            }

            return emp;
        }

        private int GEN_CATALOGO_ID(C_CONEXIONFIREBIRD con, FbTransaction tran)
        {
            int ret = 0;

            try
            {
                FbCommand fb = new FbCommand("execute procedure gen_catalogo_id", con.FBC, tran);
                FbDataReader fdr = fb.ExecuteReader();
                while (fdr.Read())
                    ret = Convert.ToInt32(Convert.ToString(fdr["CATALOGO_ID"]));
                fb.Dispose();
                fdr.Close();
            }
            catch
            {

            }

            return ret;
        }


        public C_EMPRESAS[] TraerEmpresas()
        {
            //empresas para las pruebas de oficina
            C_EMPRESAS[] prueba = new C_EMPRESAS[5];
           prueba[0] = new C_EMPRESAS();
            prueba[1] = new C_EMPRESAS();
            prueba[2] = new C_EMPRESAS();
            prueba[3] = new C_EMPRESAS();
            prueba[4] = new C_EMPRESAS();
            prueba[0].NOMBRE_CORTO = "WE KEEP ON MOVING";
            prueba[1].NOMBRE_CORTO = "SUPERV TECNICA NORTE PPTO";
            prueba[2].NOMBRE_CORTO = "IMX COMERCIAL SA CV";
            prueba[3].NOMBRE_CORTO = "ANTILA S RL CV";
            prueba[4].NOMBRE_CORTO = "KOM BUSINESS";
            // */

           // C_CONEXIONFIREBIRD conexion_fb = new C_CONEXIONFIREBIRD();
            C_ConexionSQL con = new C_ConexionSQL();
            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
            //FbCommand cmd;
            SqlCommand cmd;
            //FbDataReader reader;
            SqlDataReader reader;
            C_EMPRESAS[] emp = new C_EMPRESAS[0];
            if (registros.LeerRegistros(false))
            {
                //seleccionamos todas las empresas
              //  if (conexion_fb.ConectarFB_MANUAL("SYSDBA", registros.FB_PASSWORD, registros.FB_ROOT, registros.FB_SERVIDOR))
              if(con.ConectarSQL())
                {
                    //string QUERY = "SELECT NOMBRE_CORTO, EMPRESA_ID FROM EMPRESAS ORDER BY NOMBRE_CORTO";
                    string QUERY = "SELECT * FROM EMPRESAS_ACTIVAS WHERE EMPRESA_ESTATUS = 'A'";

                    cmd = new SqlCommand(QUERY, con.SC);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Array.Resize(ref emp, emp.Length + 1);
                            emp[emp.Length - 1] = new C_EMPRESAS();

                            emp[emp.Length - 1].NOMBRE_CORTO = Convert.ToString(reader["NOMBRE_EMPRESA"]);
                            emp[emp.Length - 1].EMPRESA_ID = Convert.ToInt32(Convert.ToString(reader["EMPRESA_MICROSIP_ID"]));


                           // */

                             /* Array.Resize(ref emp, emp.Length + 1);
                            emp[emp.Length - 1] = new C_EMPRESAS();

                            emp[emp.Length - 1].NOMBRE_CORTO = Convert.ToString(reader["NOMBRE_CORTO"]);
                            emp[emp.Length - 1].EMPRESA_ID = Convert.ToInt32(Convert.ToString(reader["EMPRESA_ID"])); // */

                        }
                    }
                    reader.Close();
                    cmd.Dispose();

                    con.Desconectar();
                }
            }
            return emp;
        }


        //traer listado de empresas de cada liberacion
        public C_EMPRESAS[] EmpresasPagos(int docto_pr_id)
        {
            C_EMPRESAS[] emp = new C_EMPRESAS[0];
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            if (reg.LeerRegistros(false))
            {
                C_ConexionSQL sql = new C_ConexionSQL();
                if (sql.ConectarSQL())
                {
                    SqlCommand sc;
                    SqlDataReader sdr;

                    string select = "SELECT distinct dpd.EMPRESA  ";
                    select += "from P_DOCTOS_PR_DET dpd ";
                    select += "join P_DOCTOS_PR dp  on dpd.DOCTO_PR_ID = dp.DOCTO_PR_ID";
                    select += " where dp.DOCTO_PR_ID = " + docto_pr_id;
                    //select += " AND (dpd.estatus = 'A' OR dpd.estatus = 'B')";
                    //select += " AND (dp.estatus_proc = 'A' OR dp.estatus_proc = 'B' ) ";

                    sc = new SqlCommand(select, sql.SC);
                    sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        Array.Resize(ref emp, emp.Length + 1);
                        emp[emp.Length - 1] = new C_EMPRESAS();

                        emp[emp.Length - 1].NOMBRE_CORTO = Convert.ToString(sdr["EMPRESA"]);
                        emp[emp.Length - 1].TIENE_PAGO = false;
                        emp[emp.Length - 1].TIENE_ANTICIPO = false;
                    }
                    sc.Dispose();
                    sdr.Close();

                    //checamos si alguna empresa tiene anticipos en sus pagos
                    for (int i = 0; i < emp.Length; i++)
                    {
                        select = "SELECT dpd.REQUISICION_ID ";
                        select += " from P_DOCTOS_PR_DET dpd ";
                        select += " join P_DOCTOS_PR dp  on dpd.DOCTO_PR_ID = dp.DOCTO_PR_ID";
                        select += " where dp.DOCTO_PR_ID = " + docto_pr_id;
                        select += " and dpd.EMPRESA = '" + emp[i].NOMBRE_CORTO + "'";
                        sc = new SqlCommand(select, sql.SC);
                        sdr = sc.ExecuteReader();

                        while (sdr.Read())
                        {
                            if (Convert.ToString(sdr["REQUISICION_ID"]) == "0")
                                emp[i].TIENE_PAGO = true;

                            emp[i].TIENE_ANTICIPO = true;
                        }
                    }

                    sql.Desconectar();
                }
            }


            return emp;
        }

        public C_USUARIOS[] GET_USUARIOS()
        {
            C_USUARIOS[] usuarios = new C_USUARIOS[0];
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            if (reg.LeerRegistros(false))
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    SqlCommand sc = new SqlCommand("SELECT * FROM USUARIOS WHERE ESTATUS = 'A'", con.SC);
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        Array.Resize(ref usuarios, usuarios.Length + 1);
                        usuarios[usuarios.Length - 1] = new C_USUARIOS();

                        usuarios[usuarios.Length - 1].Usuario_id = Convert.ToInt32(Convert.ToString(sdr["Usuario_id"]));
                        usuarios[usuarios.Length - 1].Nombre = Convert.ToString(sdr["Nombre"]);
                        usuarios[usuarios.Length - 1].Usuario = Convert.ToString(sdr["Usuario"]);
                        usuarios[usuarios.Length - 1].Contraseña = Convert.ToString(sdr["Contraseña"]);
                        usuarios[usuarios.Length - 1].Requisitante = Convert.ToString(sdr["Requisitante"]);
                        usuarios[usuarios.Length - 1].Departamento = Convert.ToString(sdr["Departamento"]);
                        usuarios[usuarios.Length - 1].Correo = Convert.ToString(sdr["Correo"]);
                        usuarios[usuarios.Length - 1].Privilegio = Convert.ToString(sdr["Privilegio"]);
                        usuarios[usuarios.Length - 1].Estatus = Convert.ToString(sdr["Estatus"]);
                        usuarios[usuarios.Length - 1].Clave_Depto = Convert.ToString(sdr["Clave_Depto"]);
                        usuarios[usuarios.Length - 1].U_ROL = Convert.ToString(sdr["U_ROL"]);
                    }


                    con.Desconectar();
                }
            }
            return usuarios;
        }

        public void GET_USUARIOS(DataGridView dgv)
        {
            try
            {
                C_USUARIOS[] usuarios = GET_USUARIOS();

                dgv.Rows.Clear();

                for (int i = 0; i < usuarios.Length; i++)
                {
                    if (usuarios[i].U_ROL == "A")
                    {
                        dgv.Rows.Add();
                        dgv["USUARIO_ID", dgv.RowCount - 1].Value = usuarios[i].Usuario_id;
                        dgv["NOMBRE", dgv.RowCount - 1].Value = usuarios[i].Nombre;
                        dgv["USUARIO", dgv.RowCount - 1].Value = usuarios[i].Usuario;
                        dgv["DEPARTAMENTO", dgv.RowCount - 1].Value = usuarios[i].Departamento;
                        dgv["PRIVILEGIO", dgv.RowCount - 1].Value = usuarios[i].Privilegio;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al traer a los usuarios\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void GET_DEP_PRIVI(ComboBox cb, string distinct)
        {
            try
            {
                cb.Items.Clear();
                cb.Items.Add("Seleccionar");
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    if (con.ConectarSQL())
                    {
                        SqlCommand sc = new SqlCommand("SELECT DISTINCT " + distinct + " FROM USUARIOS WHERE U_ROL = 'A'", con.SC);
                        SqlDataReader sdr = sc.ExecuteReader();
                        while (sdr.Read())
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(sdr[distinct])))
                                cb.Items.Add(Convert.ToString(sdr[distinct]));
                        }
                        cb.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al traer a los departamentos\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

      

    }
}
