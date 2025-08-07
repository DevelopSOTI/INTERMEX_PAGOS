using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace PagosIntermex
{
    class C_AUT_PAGOS
    {
        public C_AUT_PAGOS()
        {

        }
        public int DOCTO_PR_DET_ID { get; set; }
        public int USUARIO_ID { get; set; }
        public string NOMBRE { get; set; }
        public string ESTATUS { get; set; }
        public Color COLOR_RENGLON { get; set; }
        public Color COLOR_SELECCION { get; set; }

        public void SET_USERS(string folio, DataGridView dgv)
        {
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    if (con.ConectarSQL())
                    {
                        dgv.Rows.Clear();

                        string query = "select distinct u.Usuario, u.Nombre ";
                        query += " from P_DOCTOS_PR pdp ";
                        query += " JOIN P_DOCTOS_PR_DET pdpd on pdpd.DOCTO_PR_ID = pdp.DOCTO_PR_ID ";
                        query += " JOIN P_AUT_DOCTOS_PR pa on pa.DOCTO_PR_DET_ID = pdpd.DOCTO_PR_DET_ID ";
                        query += " JOIN USUARIOS u on u.Usuario_id = pa.USUARIO_ID ";
                        query += " where pdp.FOLIO = '" + folio + "'";

                        SqlCommand sc = new SqlCommand(query, con.SC);
                        SqlDataReader sdr = sc.ExecuteReader();

                        while (sdr.Read())
                        {
                            dgv.Rows.Add();
                            dgv["USUARIO_AUT",dgv.RowCount-1].Value = Convert.ToString(sdr["Usuario"]);
                            dgv["USER_NAME", dgv.RowCount - 1].Value = Convert.ToString(sdr["Nombre"]);
                        }

                        sc.Dispose();
                        sdr.Close();

                        con.Desconectar();
                    }
                }

            }
            catch
            {


            }

        }

        public void SET_USERS(int Nivel, DataGridView dgv)
        {
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    if (con.ConectarSQL())
                    {
                        dgv.Rows.Clear();

                        string query = "select U.Usuario_id,u.Usuario,U.Nombre,ISNULL(N.NIVEL,N2.NIVEL) AS NIVEL,U.Estatus,U.Requisitante " +
                            " from USUARIOS as U " +
                            " left join NIVELES as N on U.Clave_Depto = N.DEPTO " +
                            " left join NIVELES as N2 on U.Usuario_id = N2.Usuario_id " +
                            " where(N.NIVEL = " + Nivel + " or N2.NIVEL = " + Nivel + ")and Estatus = 'A' and U.U_ROL = 'A'";

                        SqlCommand sc = new SqlCommand(query, con.SC);
                        SqlDataReader sdr = sc.ExecuteReader();

                        while (sdr.Read())
                        {
                            dgv.Rows.Add();
                            dgv["USUARIO_AUT", dgv.RowCount - 1].Value = Convert.ToString(sdr["Usuario"]);
                            dgv["USER_NAME", dgv.RowCount - 1].Value = Convert.ToString(sdr["Nombre"]);
                            dgv["USUARIO_ID", dgv.RowCount - 1].Value = Convert.ToString(sdr["Usuario_id"]);
                            dgv["NIVEL", dgv.RowCount - 1].Value = Convert.ToString(sdr["NIVEL"]);
                        }


                        sc.Dispose();
                        sdr.Close();

                        con.Desconectar();
                    }
                }

            }
            catch
            {


            }

        }
        public void SET_USERS(int Nivel, string Folio,DataGridView dgv)
        {
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    if (con.ConectarSQL())
                    {
                        dgv.Rows.Clear();

                        string query = "select U.Usuario_id,u.Usuario,U.Nombre,N.NIVEL,U.Estatus,U.Requisitante " +
                            " from USUARIOS as U " +
                            " left join NIVELES as N on U.Usuario_id = N.Usuario_id " +
                            " where(N.NIVEL = " + Nivel + " )and Estatus = 'A' and U.U_ROL = 'A'";

                        SqlDataAdapter _da = new SqlDataAdapter(query, con.SC);
                        DataTable _usuario = new DataTable();
                        _da.Fill(_usuario);

                        if (_usuario.Rows.Count > 0)
                            foreach (DataRow _fila in _usuario.Rows)
                            {
                                dgv.Rows.Add();
                                dgv["USUARIO_AUT", dgv.RowCount - 1].Value = Convert.ToString(_fila["Usuario"]);
                                dgv["USER_NAME", dgv.RowCount - 1].Value = Convert.ToString(_fila["Nombre"]);
                                dgv["USUARIO_ID", dgv.RowCount - 1].Value = Convert.ToString(_fila["Usuario_id"]);
                                dgv["NIVEL", dgv.RowCount - 1].Value = Convert.ToString(_fila["NIVEL"]);
                            }
                        //else
                        //{
                            query = "select U.Usuario_id,u.Usuario,U.Nombre,N.NIVEL,U.Estatus,U.Requisitante " +
                            " from USUARIOS as U " +
                            " left join NIVELES as N on U.Clave_Depto = N.DEPTO " +
                            " where(N.NIVEL = " + Nivel +" )and Estatus = 'A' and U.U_ROL = 'A'";
                            _usuario = new DataTable();
                            _da= new SqlDataAdapter(query, con.SC);
                            _da.Fill(_usuario);

                            if (_usuario.Rows.Count > 0)
                                foreach (DataRow _fila in _usuario.Rows)
                                {
                                    dgv.Rows.Add();
                                    dgv["USUARIO_AUT", dgv.RowCount - 1].Value = Convert.ToString(_fila["Usuario"]);
                                    dgv["USER_NAME", dgv.RowCount - 1].Value = Convert.ToString(_fila["Nombre"]);
                                    dgv["USUARIO_ID", dgv.RowCount - 1].Value = Convert.ToString(_fila["Usuario_id"]);
                                    dgv["NIVEL", dgv.RowCount - 1].Value = Convert.ToString(_fila["NIVEL"]);
                                }
                        //}

                        //query = "select U.Usuario_id,u.Usuario,U.Nombre,ISNULL(N.NIVEL,N2.NIVEL) AS NIVEL,U.Estatus,U.Requisitante,N.NIVEL as UNO,N2.Nivel as DOS " +
                        //    " ,case when DPD.ESTATUS = 'A' then 'S' else 'N' end as ESTATUS_AUTORIZADO " +
                        //    " from USUARIOS as U " +
                        //    " left join NIVELES as N on U.Clave_Depto = N.DEPTO " +
                        //    " left join NIVELES as N2 on U.Usuario_id = N2.Usuario_id " +
                        //    " left join P_AUT_DOCTOS_PR DPD on U.Usuario_id = DPD.USUARIO_ID " +
                        //    " left JOIN P_DOCTOS_PR_DET pdpd on dpd.DOCTO_PR_DET_ID = pdpd.DOCTO_PR_DET_ID " +
                        //    " left join  P_DOCTOS_PR pdpr on pdpd.DOCTO_PR_ID = pdpr.DOCTO_PR_ID " +
                        //    " where(N.NIVEL = " + Nivel + " or N2.NIVEL = " + Nivel + ")and U.Estatus = 'A' and U.U_ROL = 'A' and pdpr.FOLIO = '"+Folio+"' " +
                        //    " group by U.Usuario_id, u.Usuario, U.Nombre, U.Estatus, U.Requisitante, N.NIVEL, N2.Nivel, DPD.ESTATUS";
                        query = "select U.Usuario_id,u.Usuario,U.Nombre,N.NIVEL AS NIVEL,U.Estatus,U.Requisitante " +
                            " ,DPD.ESTATUS,N.NIVEL " +
                            " from USUARIOS as U " +
                            " left join NIVELES as N on U.Usuario_id = N.USUARIO_ID " +
                            " left join P_AUT_DOCTOS_PR DPD on U.Usuario_id = DPD.USUARIO_ID " +
                            " left JOIN P_DOCTOS_PR_DET pdpd on dpd.DOCTO_PR_DET_ID = pdpd.DOCTO_PR_DET_ID " +
                            " left join  P_DOCTOS_PR pdpr on pdpd.DOCTO_PR_ID = pdpr.DOCTO_PR_ID " +
                            " where(N.NIVEL =  " + Nivel + ")and U.Estatus = 'A' and U.U_ROL = 'A' and DPD.ESTATUS in ('C','P')  and pdpr.FOLIO = '" + Folio + "' " +
                            " group by U.Usuario_id, u.Usuario, U.Nombre, U.Estatus, U.Requisitante, N.NIVEL, DPD.ESTATUS ";
                        
                        DataTable _usuarios = new DataTable();

                        _da = new SqlDataAdapter(query, con.SC);
                        _da.Fill(_usuarios);
                        if (_usuarios.Rows.Count > 0)
                        {
                            foreach (DataRow _fila in _usuarios.Rows)
                            {
                                string usuarioFila = Convert.ToString(_fila["Usuario_id"]);
                                for (int i = 0; i < dgv.Rows.Count; i++)
                                {
                                    string usuarioDGV = Convert.ToString(dgv["USUARIO_ID",i].Value);
                                    if (usuarioFila.Length > 0 && usuarioDGV.Length > 0)
                                        if (usuarioFila == usuarioDGV)
                                        {
                                            int _valor = 1;
                                            dgv["AUTORIZADO_AUT", i].Value = _valor;
                                            dgv["AUTORIZADO_AUT", i].ReadOnly = true;
                                        }
                                }
                            }
                        }
                        //else
                        //{
                            query = "select U.Usuario_id,u.Usuario,U.Nombre,N.NIVEL AS NIVEL,U.Estatus,U.Requisitante " +
                                " ,DPD.ESTATUS,N.NIVEL " +
                                " from USUARIOS as U " +
                                " left join NIVELES as N on U.Clave_Depto = N.DEPTO " +
                                " left join P_AUT_DOCTOS_PR DPD on U.Usuario_id = DPD.USUARIO_ID " +
                                " left JOIN P_DOCTOS_PR_DET pdpd on dpd.DOCTO_PR_DET_ID = pdpd.DOCTO_PR_DET_ID " +
                                " left join  P_DOCTOS_PR pdpr on pdpd.DOCTO_PR_ID = pdpr.DOCTO_PR_ID " +
                                " where(N.NIVEL =" + Nivel + ")and U.Estatus = 'A' and U.U_ROL = 'A' and DPD.ESTATUS in ('C','P') and pdpr.FOLIO = '" + Folio + "' " +
                                " group by U.Usuario_id, u.Usuario, U.Nombre, U.Estatus, U.Requisitante, N.NIVEL, DPD.ESTATUS";
                            _da = new SqlDataAdapter(query, con.SC);
                            _da.Fill(_usuarios);
                            if (_usuarios.Rows.Count > 0)
                            {
                                foreach (DataRow _fila in _usuarios.Rows)
                                {
                                    string usuarioFila = Convert.ToString(_fila["Usuario_id"]);
                                    for (int i = 0; i < dgv.Rows.Count; i++)
                                    {
                                        string usuarioDGV = Convert.ToString(dgv["USUARIO_ID", i].Value);
                                        if (usuarioFila.Length > 0 && usuarioDGV.Length > 0)
                                            if (usuarioFila == usuarioDGV)
                                            {
                                                int _valor = 1;
                                                dgv["AUTORIZADO_AUT", i].Value = _valor;
                                                dgv["AUTORIZADO_AUT", i].ReadOnly = true;
                                            }
                                    }
                                }
                            }
                        //}
                        _da.Dispose();
                        con.Desconectar();
                    }
                }
            }
            catch
            {


            }

        }
        public C_AUT_PAGOS[] GET_DETALLES(string folio,int Nivel)
        {

            C_AUT_PAGOS[] pagos = new C_AUT_PAGOS[0];
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false)) {
                    C_ConexionSQL con = new C_ConexionSQL();
                    if (con.ConectarSQL()) {
                        string query = "select pa.DOCTO_PR_DET_ID, pa.ESTATUS, u.Usuario, u.Usuario_id ";
                        query += " from P_DOCTOS_PR pdp ";
                        query += " JOIN P_DOCTOS_PR_DET pdpd on pdpd.DOCTO_PR_ID = pdp.DOCTO_PR_ID ";
                        query += " JOIN P_AUT_DOCTOS_PR pa on pa.DOCTO_PR_DET_ID = pdpd.DOCTO_PR_DET_ID ";
                        query += " JOIN USUARIOS u on u.Usuario_id = pa.USUARIO_ID ";
                        query += " where pdp.FOLIO = '" + folio + "' and pa.NIVEL="+Nivel;
                        query += " order by pa.DOCTO_PR_DET_ID desc ";

                        SqlCommand sc = new SqlCommand(query, con.SC);
                        SqlDataReader sdr = sc.ExecuteReader();

                        int docto_pr_det = 0;
                        while (sdr.Read())
                        {
                            Array.Resize(ref pagos, pagos.Length + 1);
                            pagos[pagos.Length - 1] = new C_AUT_PAGOS();

                            pagos[pagos.Length - 1].DOCTO_PR_DET_ID = Convert.ToInt32(Convert.ToString(sdr["DOCTO_PR_DET_ID"]));
                            pagos[pagos.Length - 1].USUARIO_ID = Convert.ToInt32(Convert.ToString(sdr["USUARIO_ID"]));
                            pagos[pagos.Length - 1].NOMBRE = Convert.ToString(sdr["Usuario"]);
                            pagos[pagos.Length - 1].ESTATUS = Convert.ToString(sdr["ESTATUS"]);

                        }
                        sc.Dispose();
                        sdr.Close();

                        #region ASIGNAR COLOR QUE ESTARA EN DGV
                        Color aux = new Color();
                        for (int i = 0; i < pagos.Length; i++)
                        {                            
                            if (docto_pr_det == 0)
                            {
                                docto_pr_det = pagos[i].DOCTO_PR_DET_ID;
                                aux = GetColor(con, docto_pr_det,Nivel);
                                pagos[i].COLOR_RENGLON = aux;

                                if (aux != Color.Empty)
                                {
                                    if (aux == Color.FromArgb(231, 212, 136))
                                        pagos[i].COLOR_SELECCION = Color.FromArgb(203, 171, 42);
                                    else
                                        pagos[i].COLOR_SELECCION = Color.FromArgb(100, 150, 100);
                                }
                                else
                                    pagos[i].COLOR_SELECCION = Color.Empty;
                                i++;
                            }

                            if (docto_pr_det != pagos[i].DOCTO_PR_DET_ID)
                            {
                                docto_pr_det = pagos[i].DOCTO_PR_DET_ID;
                                aux = GetColor(con, docto_pr_det,Nivel);
                            }

                            pagos[i].COLOR_RENGLON = aux;

                            if (aux != Color.Empty)
                            {
                                if (aux == Color.FromArgb(231, 212, 136))
                                    pagos[i].COLOR_SELECCION = Color.FromArgb(203, 171, 42);
                                else
                                    pagos[i].COLOR_SELECCION = Color.FromArgb(100, 150, 100);
                            }
                            else
                                pagos[i].COLOR_SELECCION = Color.Empty;
                        }
                        #endregion

                        con.Desconectar();
                    }
                }

            }
            catch
            {

                
            }

            return pagos;
        }

        public Color GetColor(C_ConexionSQL con, int docto_pr_det,int Nivel)
        {
            Color c = new Color();
            

            #region consulta para determinar el color del grid
            string color = " select TOTAL, AUTORIZADOS  FROM";
            color += " (select COUNT(*) TOTAL, ";
            color += " (select COUNT(*)  ";
            color += " from P_AUT_DOCTOS_PR p  ";
            color += " where p.DOCTO_PR_DET_ID = " + docto_pr_det;
            color += " and p.ESTATUS = 'A'  and NIVEL=" + Nivel + ") AUTORIZADOS ";
            color += " from P_AUT_DOCTOS_PR ";
            color += " where DOCTO_PR_DET_ID =  " + docto_pr_det + " and NIVEL=" + Nivel + ") as CP ";
            color += " group by CP.TOTAL, CP.AUTORIZADOS ";

            SqlCommand sc = new SqlCommand(color, con.SC);
            SqlDataReader sdr = sc.ExecuteReader();
            while (sdr.Read())
            {
                
                if (Convert.ToInt32(Convert.ToString(sdr["AUTORIZADOS"])) == 0)
                {
                    c = Color.Empty;
                }
                else if(Convert.ToInt32(Convert.ToString(sdr["TOTAL"])) == Convert.ToInt32(Convert.ToString(sdr["AUTORIZADOS"])))
                {
                    c = Color.FromArgb(150, 255, 150);
                }
                else
                {
                    c = Color.FromArgb(231, 212, 136);
                }
                
            }

            #endregion

            return c;
        }


        public bool DetalleAutorizado(int docto_pr_det,int nivel )
        {
            bool c = false;
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {

                    //consulta para saber si ya se autorizo aunque sea un pago en especifico 
                    string consulta = "select TOTAL - AUTORIZADOS DIF  FROM ";
                    consulta += " (select COUNT(*) TOTAL, (select COUNT(*) from P_AUT_DOCTOS_PR p  ";
                    consulta += " where p.DOCTO_PR_DET_ID = " + docto_pr_det + "  AND nivel=" + nivel + " and p.ESTATUS = 'A') AUTORIZADOS ";
                    consulta += " from P_AUT_DOCTOS_PR ";
                    consulta += " where DOCTO_PR_DET_ID =  " + docto_pr_det + "  AND nivel=" + nivel + ") as CP ";
                    consulta += " group by CP.TOTAL, CP.AUTORIZADOS ";

                    SqlCommand cmd = new SqlCommand(consulta, con.SC);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                        if (Convert.ToInt32(Convert.ToString(sdr["DIF"])) == 0)
                            c = true;                    
                    con.Desconectar();
                }
            }
            catch
            {

            }

            return c;
        }

        public DataTable DatosDetalle(string Folio,int Nivel,string Ver,out string msg)
        {
            DataTable _resultado = new DataTable();
            string msg_local = "";
            try{
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string _nivel= "",_nivel2="";
                    if (Ver == "N")
                    {
                        _nivel = " AND ADPD1.NIVEL = " + Nivel;
                        _nivel2= " and ADPD.NIVEL = " + Nivel;
                    }
                    //Obtenemos los usuarios asignados del nivel correspondiente
                    /* string consulta = "SELECT dp.ESTATUS_PROC,dpd.DOCTO_PR_DET_ID, dpd.DOCTO_PR_ID, dpd.FOLIO_MICROSIP,dpd.FOLIO_CREDITO, dpd.FECHA_CARGO, dpd.PROVEEDOR_ID, " + 
                         " dpd.PROVEEDOR_CLAVE,dpd.PROVEEDOR_NOMBRE, dpd.FECHA_VENCIMIENTO,dpd.IMPORTE_PAGOS, " + 
                         " (select count(*) from P_AUT_DOCTOS_PR AS ADPD1 where ADPD1.MONTO_AUTORIZADO IS NULL AND ADPD1.NIVEL = " + _nivel + " AND ADPD.DOCTO_PR_DET_ID = ADPD1.DOCTO_PR_DET_ID)AS PEND_AUTORIZAR, " +
                         " isnull((select top 1 MONTO_AUTORIZADO from P_AUT_DOCTOS_PR as ADPD1 order by MONTO_AUTORIZADO asc),-1) as MONTO_AUTORIZADO_MAS_BAJO" +
                         " FROM P_DOCTOS_PR_DET AS DPD " +
                         " left JOIN p_doctos_pr AS dp ON(DPD.DOCTO_PR_ID = dp.DOCTO_PR_ID) " +
                         " left join P_AUT_DOCTOS_PR as ADPD on(DPD.DOCTO_PR_DET_ID = ADPD.DOCTO_PR_DET_ID) " +
                         " left join USUARIOS as U on adpd.USUARIO_ID = U.Usuario_id " +
                         " where Dp.FOLIO = '"+Folio+"' and ADPD.NIVEL =  " + Nivel +
                         " group by dp.ESTATUS_PROC,dpd.DOCTO_PR_DET_ID, dpd.DOCTO_PR_ID, dpd.FOLIO_MICROSIP,dpd.FOLIO_CREDITO, dpd.FECHA_CARGO, dpd.PROVEEDOR_ID,  " +
                         " dpd.PROVEEDOR_CLAVE,dpd.PROVEEDOR_NOMBRE, dpd.FECHA_VENCIMIENTO,dpd.IMPORTE_PAGOS,ADPD.DOCTO_PR_DET_ID " +
                         " order by dpd.DOCTO_PR_DET_ID";*/
                    string consulta = "SELECT dp.ESTATUS_PROC,dpd.DOCTO_PR_DET_ID, dpd.DOCTO_PR_ID, dpd.FOLIO_MICROSIP,dpd.FOLIO_CREDITO, dpd.FECHA_CARGO, dpd.PROVEEDOR_ID, " +
                        " dpd.PROVEEDOR_CLAVE,dpd.PROVEEDOR_NOMBRE, dpd.FECHA_VENCIMIENTO,dpd.IMPORTE_PAGOS, " +
                        " (select count(*) from P_AUT_DOCTOS_PR AS ADPD1 where ADPD1.MONTO_AUTORIZADO IS NULL  " + _nivel + " AND ADPD.DOCTO_PR_DET_ID = ADPD1.DOCTO_PR_DET_ID)AS PEND_AUTORIZAR, " +
                        " isnull((select top 1 MONTO_AUTORIZADO from P_AUT_DOCTOS_PR as ADPD1 order by MONTO_AUTORIZADO asc),-1) as MONTO_AUTORIZADO_MAS_BAJO" +
                        " FROM P_DOCTOS_PR_DET AS DPD " +
                        " left JOIN p_doctos_pr AS dp ON(DPD.DOCTO_PR_ID = dp.DOCTO_PR_ID) " +
                        " left join P_AUT_DOCTOS_PR as ADPD on(DPD.DOCTO_PR_DET_ID = ADPD.DOCTO_PR_DET_ID) " +
                        " left join USUARIOS as U on adpd.USUARIO_ID = U.Usuario_id " +
                        " where Dp.FOLIO = '" + Folio + "' " + _nivel2 +
                        " group by dp.ESTATUS_PROC,dpd.DOCTO_PR_DET_ID, dpd.DOCTO_PR_ID, dpd.FOLIO_MICROSIP,dpd.FOLIO_CREDITO, dpd.FECHA_CARGO, dpd.PROVEEDOR_ID,  " +
                        " dpd.PROVEEDOR_CLAVE,dpd.PROVEEDOR_NOMBRE, dpd.FECHA_VENCIMIENTO,dpd.IMPORTE_PAGOS,ADPD.DOCTO_PR_DET_ID " +
                        " order by dpd.DOCTO_PR_DET_ID";
                    DataTable _datos = new DataTable();
                    SqlDataAdapter _da = new SqlDataAdapter(consulta, con.SC);
                    _da.Fill(_datos);
                    List<DataTable> _unanimes = new List<DataTable>();
                    //obtenemos todos los detalles por usuario
                    if (_datos.Rows.Count > 0)                    
                        _resultado = _datos;                    
                    else
                        msg_local = "El folio \"" + Folio + "\" no tiene usarios asignados para el Nivel " + Nivel;
                    _da.Dispose();
                    con.Desconectar();
                }
                else
                    msg_local = "No se pudo establecer la conexión con la base de datos, favor de intentar más tarde";
            }
            catch(Exception Ex){
                msg_local = Ex.Message;
            }
            msg = msg_local;
            return _resultado;
        }

        public DataTable DatosRequisicion(int Doctos_pr_id, out string msg)
        {
            DataTable _resultado = new DataTable();
            string msg_local = "";
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                     string consulta = "select REQUISICION_ID,FOLIO_MICROSIP from P_DOCTOS_PR_DET " +
                        " where DOCTO_PR_DET_ID = "+Doctos_pr_id;
                    
                    SqlDataAdapter _da = new SqlDataAdapter(consulta, con.SC);
                    _da.Fill(_resultado);
                    _da.Dispose();
                    con.Desconectar();
                }
                else
                    msg_local = "No se pudo establecer la conexión con la base de datos, favor de intentar más tarde";
            }
            catch (Exception Ex)
            {
                msg_local = Ex.Message;
            }
            msg = msg_local;
            return _resultado;
        }

        public bool InsertarUsuarioAutorizarDetalle(int DOCTO_PR_DET_ID,int USUARIO_ID,C_ConexionSQL conexion,SqlTransaction transaction, out string msg)
        {
            bool _exito = false;
            string msg_local = "";
            try
            {
                string consulta = "select * from P_AUT_DOCTOS_PR where DOCTO_PR_DET_ID=" + DOCTO_PR_DET_ID +
                                " and USUARIO_ID " + USUARIO_ID;
                SqlCommand cmd = new SqlCommand(consulta, conexion.SC, transaction);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    //Sino existe insertar el nuevo usuario
                    consulta = " INSERT INTO P_AUT_DOCTOS_PR " +
                    " (DOCTO_PR_DET_ID " +
                    " , USUARIO_ID " +
                    " , ESTATUS " +
                    " , MONTO_AUTORIZADO) " +
                    " VALUES " +
                    " (" + DOCTO_PR_DET_ID +
                    " , " + USUARIO_ID +
                    " , 'C' " +
                    " ,NULL)";

                    cmd = new SqlCommand(consulta, conexion.SC, transaction);
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        _exito = false;
                        msg_local = "No se pudo completar la operación favor de reintentar";
                    }
                    else
                        _exito = true;
                }
            }
            catch (Exception Ex)
            {
                msg_local = Ex.Message;
            }
            msg = msg_local;
            return _exito;
        }

        public double ObtenerMenorMontoAutorizado(string Folio,int Docto_pr_det_id, C_ConexionSQL conexion, SqlTransaction transaction, out string msg)
        {
            double _resultado = 0.0;
            string msg_local = "";
            try
            {
                string consulta = "select top 1 * from P_AUT_DOCTOS_PR  DPD " +
                    " left JOIN P_DOCTOS_PR_DET pdpd on dpd.DOCTO_PR_DET_ID = pdpd.DOCTO_PR_DET_ID " +
                    " left join  P_DOCTOS_PR pdpr on pdpd.DOCTO_PR_ID = pdpr.DOCTO_PR_ID " +
                    " where " +
                    " DPD.DOCTO_PR_DET_ID = " + Docto_pr_det_id +
                    " and pdpr.FOLIO = '" + Folio + "' " +
                    " and Dpd.ESTATUS = 'A' " +
                    " order by MONTO_AUTORIZADO asc";
                DataTable _res = new DataTable();
                SqlDataAdapter _da = new SqlDataAdapter(consulta, conexion.SC);
                _da.SelectCommand.Transaction = transaction;
                _da.Fill(_res);
                if (_res.Rows.Count == 1)
                {
                    _resultado = Convert.ToDouble(_res.Rows[0]["MONTO_AUTORIZADO"]);
                }
                _da.Dispose();
            }
            catch (Exception Ex)
            {
                msg_local = Ex.Message;
            }
            msg = msg_local;
            return _resultado;
        }
    }
}
