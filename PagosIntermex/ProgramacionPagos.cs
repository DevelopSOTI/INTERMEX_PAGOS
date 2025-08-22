using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace PagosIntermex
{
    class ProgramacionPagos
    {
        // MUESTRA LAS PROGRAMACIONES DE PAGO CAPTURADAS EN EL SISTEMA
        public DataTable EncabezadoPagos(string ESTATUS_PROC, string ESTATUS, List<DateTime> Fechas, C_EMPRESAS[] empresa, string usuario, out string Msg, string todos = "")
        {
            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

            C_ConexionSQL conn = new C_ConexionSQL();

            DataTable _Encabezado = new DataTable();

            string msg_local = "", consulta = "", fechas = "", _estatus = "", _where = "";
            DateTime _fechaInicio, 
                _fechaFin;
            try
            {
                if (Fechas.Count == 2)
                    //fechas = " (dpr.FECHA_PAGO BETWEEN '" + Fechas[0].ToString("dd.MM.yyyy") + "' AND '" + Fechas[1].ToString("dd.MM.yyyy") + "')";
                    fechas = " (dpr.FECHA_PAGO BETWEEN @FECHAINICIO AND @FECHAFIN)";

                if (ESTATUS.Length > 0)
                    _estatus = " dpr.estatus = @ESTATUS ";

                /* if (Fechas.Count == 2 && ESTATUS.Length > 0)
                    _where = " where " + _estatus + " AND " + fechas;
                else if (Fechas.Count == 2 && ESTATUS.Length == 0)
                    _where = " where " + fechas;
                else if (Fechas.Count == 0 && ESTATUS.Length == 1)
                    _where = " where " + _estatus;
                else if (Fechas.Count == 0 && ESTATUS.Length == 0)
                    _where = ""; // */

                if (Fechas.Count == 2 && ESTATUS.Length > 0)
                    _where = "  " + _estatus + " AND " + fechas;
                else if (Fechas.Count == 2 && ESTATUS.Length == 0)
                    _where = "  " + fechas;
                else if (Fechas.Count == 0 && ESTATUS.Length == 1)
                    _where = "  " + _estatus;
                else if (Fechas.Count == 0 && ESTATUS.Length == 0)
                    _where = "";

                // FbDataReader reader;

                if (registros.LeerRegistros(false))
                {
                    //if (conn.ConectarFB_MANUAL("SYSDBA", registros.FB_PASSWORD, registros.FB_ROOT, registros.FB_BD, registros.FB_SERVIDOR))
                    if(conn.ConectarSQL())
                    {
                        /* consulta = "  select dpr.DOCTO_PR_ID, Count (DOCTO_PR_DET_ID) as CANT_PAGOS,sum(DPRD.IMPORTE_AUTORIZADO) as IMP_AUTO,dpr.FOLIO, dpr.FECHA_PAGO, dpr.ESTATUS, dpr.USUARIO_CREADOR, " +
                            " dpr.FECHA_HORA_CREACION, dpr.EMPRESA, dpr.IMPORTE_PAGOS, dpr.IMPORTE_AUTORIZADO, dpr.ESTATUS_PROC " +
                            " from DOCTOS_PR as DPR" +
                            " inner join DOCTOS_PR_DET as DPRD on DPR.DOCTO_PR_ID=DPRD.DOCTO_PR_ID" +
                            " " + _where +
                            " group by dpr.DOCTO_PR_ID, dpr.FOLIO, dpr.FECHA_PAGO, dpr.ESTATUS, dpr.USUARIO_CREADOR, " +
                            " dpr.FECHA_HORA_CREACION, dpr.EMPRESA, dpr.IMPORTE_PAGOS, dpr.IMPORTE_AUTORIZADO, dpr.ESTATUS_PROC "; */

                        consulta = "SELECT ";
                        consulta += "      dpr.DOCTO_PR_ID, ";
                        consulta += "      COUNT (DPRD.DOCTO_PR_DET_ID) AS CANT_PAGOS, ";
                        consulta += "      SUM(DPRD.IMPORTE_AUTORIZADO) AS IMP_AUTO, ";
                        consulta += "      dpr.FOLIO, ";
                        consulta += "      dpr.FECHA_PAGO, ";
                        consulta += "      dpr.ESTATUS, ";
                        consulta += "      dpr.USUARIO_CREADOR, ";
                        consulta += "      dpr.FECHA_HORA_CREACION, ";
                        consulta += "      dpr.EMPRESA, ";
                        consulta += "      dpr.IMPORTE_PAGOS, ";
                        consulta += "      dpr.IMPORTE_AUTORIZADO, ";
                        consulta += "      dpr.IMPORTE_CAPTURISTA, ";
                        consulta += "      dpr.ESTATUS_PROC ";
                        consulta += " FROM P_DOCTOS_PR AS DPR ";
                        consulta += "INNER JOIN P_DOCTOS_PR_DET AS DPRD ON(DPR.DOCTO_PR_ID = DPRD.DOCTO_PR_ID) ";
                        consulta += " JOIN P_AUT_DOCTOS_PR AUT ON AUT.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                        consulta += " JOIN USUARIOS U ON U.Usuario_id = AUT.USUARIO_ID ";
                        if (_where != "")
                        {
                            if (todos != "S")
                                consulta += "WHERE (U.Usuario = '" + usuario + "' OR DPR.USUARIO_CREADOR = '" + usuario + "') AND ";
                            else
                                consulta += " WHERE ";
                        }
                        consulta += " " + _where + " ";
                        if (!string.IsNullOrEmpty(ESTATUS_PROC))
                        {
                            if (ESTATUS_PROC == "N")
                                consulta += " AND dpr.ESTATUS_PROC IS NULL ";
                            else
                                consulta += " AND dpr.ESTATUS_PROC = @ESTATUS_PROC ";
                        }
                        consulta += " GROUP BY dpr.DOCTO_PR_ID ";
                        consulta += "         ,dpr.FOLIO ";
                        consulta += "         ,dpr.FECHA_PAGO ";
                        consulta += "         ,dpr.ESTATUS ";
                        consulta += "         ,dpr.USUARIO_CREADOR ";
                        consulta += "         ,dpr.FECHA_HORA_CREACION ";
                        consulta += "         ,dpr.EMPRESA ";
                        consulta += "         ,dpr.IMPORTE_PAGOS ";
                        consulta += "         ,dpr.IMPORTE_AUTORIZADO ";
                        consulta += "         ,dpr.IMPORTE_CAPTURISTA ";
                        consulta += "         ,dpr.ESTATUS_PROC ";

                        SqlDataAdapter sqlData = new SqlDataAdapter(consulta, conn.SC);
                        /*sqlData.SelectCommand.Parameters.Add(new SqlParameter("@EMPRESA", SqlDbType.VarChar));
                        sqlData.SelectCommand.Parameters["@EMPRESA"].Value = empresa;*/
                        if (Fechas.Count == 2)
                        {
                            sqlData.SelectCommand.Parameters.Add(new SqlParameter("@FECHAINICIO",SqlDbType.Date));
                            sqlData.SelectCommand.Parameters.Add(new SqlParameter("@FECHAFIN", SqlDbType.Date));
                            sqlData.SelectCommand.Parameters["@FECHAINICIO"].Value = Fechas[0];
                            sqlData.SelectCommand.Parameters["@FECHAFIN"].Value = Fechas[1];
                        }
                        if (ESTATUS.Length > 0)
                        {
                            sqlData.SelectCommand.Parameters.Add(new SqlParameter("@ESTATUS", SqlDbType.VarChar));
                            sqlData.SelectCommand.Parameters["@ESTATUS"].Value = ESTATUS;
                        }
                        if(!string.IsNullOrEmpty(ESTATUS_PROC))
                        {
                            if (ESTATUS_PROC != "N")
                            {
                                sqlData.SelectCommand.Parameters.Add(new SqlParameter("@ESTATUS_PROC", SqlDbType.VarChar));
                                sqlData.SelectCommand.Parameters["@ESTATUS_PROC"].Value = ESTATUS_PROC;
                            }
                        }
                            sqlData.Fill(_Encabezado);
                        sqlData.Dispose();

                        conn.Desconectar();
                    }
                }
            }
            catch (Exception Ex)
            {
                conn.Desconectar();

                msg_local = Ex.Message;
            }

            Msg = msg_local;
            return _Encabezado;
        }

        public DataTable PagosAsignados(DateTime Fecha, out string Msg)
        {
            string DATE_SEARCH = Fecha.ToString("dd.mm.yyyy");
            string msg_local = "", consulta = "";

            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
            C_ConexionSQL conn = new C_ConexionSQL();
            DataTable _Encabezado = new DataTable();

            try
            {                
                if (registros.LeerRegistros(false))
                {
                    if (conn.ConectarSQL())
                    {
                        #region Versión 1
                        /* consulta = "SELECT " +
                                   "       DOCTO_PR_DET_ID, " +
                                   "       DOCTO_PR_ID, " +
                                   "       FOLIO_MICROSIP, " +
                                   "       FECHA_CARGO, " +
                                   "       PROVEEDOR_ID, " +
                                   "       PROVEEDOR_CLAVE, " +
                                   "       PROVEEDOR_NOMBRE, " +
                                   "       FECHA_VENCIMIENTO, " +
                                   "       TIPO " +
                                   "       ESTATUS " +
                                   "  FROM DOCTOS_PR_DET " +
                                   " WHERE ESTATUS = 'P' " + // PENDIENTES
                                   "    OR ESTATUS = 'C' " + // CORRECTOS
                                   "    OR ESTATUS = 'A' " + // AUTORIZADOS
                                   "    OR ESTATUS = 'B' " + // BANCOS
                                   "    OR ESTATUS = 'R' " + // RECHAZADOS
                                   "    OR ESTATUS = 'L' " + // LIBERADOS
                                   "    OR ESTATUS IS NULL "; // */
                        #endregion

                        #region Versión 2
                        /* consulta = "SELECT " +
                                   "       D.DOCTO_PR_DET_ID, " +
                                   "       D.DOCTO_PR_ID, " +
                                   "       D.FOLIO_MICROSIP, " +
                                   "       D.FECHA_CARGO, " +
                                   "       D.PROVEEDOR_ID, " +
                                   "       D.PROVEEDOR_CLAVE, " +
                                   "       D.PROVEEDOR_NOMBRE, " +
                                   "       D.FECHA_VENCIMIENTO, " +
                                   "       D.TIPO, " +
                                   "       D.ESTATUS " +
                                   "  FROM DOCTOS_PR_DET D " +
                                   "  JOIN DOCTOS_PR E ON(E.DOCTO_PR_ID = D.DOCTO_PR_ID) " +
                                   " WHERE E.ESTATUS_PROC != 'B' " +
                                   "   AND E.ESTATUS_PROC != 'L' " +
                                   "    OR E.ESTATUS_PROC IS NULL "; // */
                        #endregion

                        consulta = "SELECT " +
                                   "       D.DOCTO_PR_DET_ID, " +
                                   "       D.DOCTO_PR_ID, " +
                                   "       D.FOLIO_MICROSIP, " +
                                   "       D.FECHA_CARGO, " +
                                   "       D.PROVEEDOR_ID, " +
                                   "       D.PROVEEDOR_CLAVE, " +
                                   "       D.PROVEEDOR_NOMBRE, " +
                                   "       D.FECHA_VENCIMIENTO, " +
                                   "       D.TIPO, " +
                                   "       D.ESTATUS " +
                                   "  FROM DOCTOS_PR_DET D " +
                                   "  JOIN DOCTOS_PR E ON(E.DOCTO_PR_ID = D.DOCTO_PR_ID) " +
                                   " WHERE E.ESTATUS_PROC != 'B' " +
                                   "   AND E.ESTATUS_PROC != 'L' " +
                                   "    OR E.ESTATUS_PROC IS NULL " +
                                   "    OR D.ESTATUS = 'L' ";

                        SqlDataAdapter fbData = new SqlDataAdapter(consulta, conn.SC);
                        fbData.Fill(_Encabezado);
                        fbData.Dispose();

                        conn.Desconectar();
                    }
                }
            }
            catch (Exception Ex)
            {
                msg_local = Ex.Message;
                conn.Desconectar();
            }

            Msg = msg_local;
            return _Encabezado;
        }

        // MUESTRA LOS PAGOS DE UNA PROGRAMACIÓN INDICADA POR EL FOLIO
        public DataTable DetallePagos(string FOLIO, C_USUARIOS usuario,int NivelProgramacion, string Ver ,out string Msg, string esPeticion)
        {
            C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

            C_ConexionSQL conn = new C_ConexionSQL();
            DataTable _Encabezado = new DataTable();

            string msg_local = "", consulta = "";

            try
            {
                if (registros.LeerRegistros(false))
                {
                    if (conn.ConectarSQL())
                    {
                        if (usuario.Requisitante == "N" || usuario.Tesoreria.ToUpper() == "TRUE" || usuario.U_ROL == "A")
                        {
                            string _nivel = "";
                            if (Ver != "S")
                                _nivel = "  and pad.NIVEL = @NIVEL";
                            /*else
                                _nivel = " and PAD.NIVEL = (select top 1 ADPD.NIVEL from P_AUT_DOCTOS_PR as ADPD " +
                                    " inner join P_DOCTOS_PR_DET as DPD on ADPD.DOCTO_PR_DET_ID = DPD.DOCTO_PR_DET_ID " +
                                    " inner join P_DOCTOS_PR as DP on DPD.DOCTO_PR_ID = Dp.DOCTO_PR_ID " +
                                    " where DP.FOLIO = '"+FOLIO+"' order by ADPD.NIVEL asc)";*/
                            consulta = "SELECT " +
                                       "       dp.ESTATUS_PROC," +
                                       "       dpd.DOCTO_PR_DET_ID, " +
                                       "       dpd.DOCTO_PR_ID, " +
                                       "       dpd.FOLIO_MICROSIP, " +
                                       "       dpd.FOLIO_CREDITO, " +
                                       "       dpd.FECHA_CARGO, " +
                                       "       dpd.PROVEEDOR_ID, " +
                                       "       dpd.PROVEEDOR_CLAVE," +
                                       "       dpd.PROVEEDOR_NOMBRE, " +
                                       "       dpd.FECHA_VENCIMIENTO, " +
                                       "       dpd.IMPORTE_PAGOS, " +
                                       "       dpd.IMPORTE_AUTORIZADO, " +
                                       "       dpd.IMPORTE_CAPTURISTA, " +
                                       "       dpd.ESTATUS, " +
                                       "       dpd.TIPO, " +
                                       "       dpd.COMENTARIOS, dpd.EMPRESA " +
                                       "  FROM P_DOCTOS_PR_DET AS DPD" +
                                       " INNER JOIN p_doctos_pr AS dp ON(DPD.DOCTO_PR_ID = dp.DOCTO_PR_ID)" +
                                       " JOIN P_AUT_DOCTOS_PR pad on pad.DOCTO_PR_DET_ID = DPD.DOCTO_PR_DET_ID " +
                                       //" JOIN USUARIOS u on u.Usuario_id = pad.USUARIO_ID " +
                                       " WHERE dp.folio = @FOLIO" +
                                       //"  and u.Usuario = @USUARIO " +
                                       " and (pad.USUARIO_ID=@USUARIOID or dp.USUARIO_CREADOR=@USUARIO)" +
                                       _nivel+
                                       "  and pad.ESTATUS != 'P' " +
                                       " group by dp.ESTATUS_PROC,dpd.DOCTO_PR_DET_ID,dpd.DOCTO_PR_ID,dpd.FOLIO_MICROSIP, " +
                                        " dpd.FOLIO_CREDITO,dpd.FECHA_CARGO,dpd.PROVEEDOR_ID,dpd.PROVEEDOR_CLAVE, " +      
                                        " dpd.PROVEEDOR_NOMBRE,dpd.FECHA_VENCIMIENTO,dpd.IMPORTE_PAGOS,dpd.IMPORTE_AUTORIZADO," +       
                                        " dpd.IMPORTE_CAPTURISTA,dpd.ESTATUS,dpd.TIPO,dpd.COMENTARIOS, dpd.EMPRESA";
                            SqlDataAdapter sqlData = new SqlDataAdapter(consulta, conn.SC);
                            sqlData.SelectCommand.Parameters.Add(new SqlParameter("@FOLIO", SqlDbType.VarChar));
                            sqlData.SelectCommand.Parameters["@FOLIO"].Value = FOLIO;
                            sqlData.SelectCommand.Parameters.Add(new SqlParameter("@USUARIOID", SqlDbType.Int));
                            sqlData.SelectCommand.Parameters["@USUARIOID"].Value = usuario.Usuario_id;
                            sqlData.SelectCommand.Parameters.Add(new SqlParameter("@USUARIO", SqlDbType.VarChar));
                            sqlData.SelectCommand.Parameters["@USUARIO"].Value = usuario.Usuario;
                            if (Ver != "S")
                            {
                                sqlData.SelectCommand.Parameters.Add(new SqlParameter("@NIVEL", SqlDbType.Int));
                                sqlData.SelectCommand.Parameters["@NIVEL"].Value = NivelProgramacion;
                            }
                                sqlData.Fill(_Encabezado);
                            sqlData.Dispose();

                            if(esPeticion == "S")
                            {
                                consulta = "SELECT " +
                                       "       dp.ESTATUS_PROC," +
                                       "       dpd.DOCTO_PP_DET_ID as DOCTO_PR_DET_ID, " +
                                       "       dpd.DOCTO_PP_ID as DOCTO_PR_ID, " +
                                       "       dpd.FOLIO_MICROSIP, " +
                                       "       dpd.FOLIO_CREDITO, " +
                                       "       dpd.FECHA_CARGO, " +
                                       "       dpd.PROVEEDOR_ID, " +
                                       "       dpd.PROVEEDOR_CLAVE," +
                                       "       dpd.PROVEEDOR_NOMBRE, " +
                                       "       dpd.FECHA_VENCIMIENTO, " +
                                       "       dpd.IMPORTE_PAGOS, " +
                                       "       dpd.IMPORTE_AUTORIZADO, " +
                                       "       dpd.IMPORTE_CAPTURISTA, " +
                                       "       dpd.ESTATUS, " +
                                       "       dpd.TIPO, DPD.REQUISICION_ID, " +
                                       "       dpd.COMENTARIOS, dpd.EMPRESA " +
                                       "  FROM P_DOCTOS_PP_DET AS DPD" +
                                       " INNER JOIN p_doctos_pp AS dp ON(DPD.DOCTO_PP_ID = dp.DOCTO_PP_ID)" +
                                       " WHERE dp.folio = @FOLIO";
                                sqlData = new SqlDataAdapter(consulta, conn.SC);
                                sqlData.SelectCommand.Parameters.Add(new SqlParameter("@FOLIO", SqlDbType.VarChar));
                                sqlData.SelectCommand.Parameters["@FOLIO"].Value = FOLIO;
                                sqlData.Fill(_Encabezado);
                                sqlData.Dispose();
                            }
                        }
                        else
                        {
                            consulta = "SELECT " +
                                       "       dp.ESTATUS_PROC," +
                                       "       dpd.DOCTO_PP_DET_ID as DOCTO_PR_DET_ID, " +
                                       "       dpd.DOCTO_PP_ID as DOCTO_PR_ID, " +
                                       "       dpd.FOLIO_MICROSIP, " +
                                       "       dpd.FOLIO_CREDITO, " +
                                       "       dpd.FECHA_CARGO, " +
                                       "       dpd.PROVEEDOR_ID, " +
                                       "       dpd.PROVEEDOR_CLAVE," +
                                       "       dpd.PROVEEDOR_NOMBRE, " +
                                       "       dpd.FECHA_VENCIMIENTO, " +
                                       "       dpd.IMPORTE_PAGOS, " +
                                       "       dpd.IMPORTE_AUTORIZADO, " +
                                       "       dpd.IMPORTE_CAPTURISTA, " +
                                       "       dpd.ESTATUS, " +
                                       "       dpd.TIPO, DPD.REQUISICION_ID, " +
                                       "       dpd.COMENTARIOS, dpd.EMPRESA " +
                                       "  FROM P_DOCTOS_PP_DET AS DPD" +
                                       " INNER JOIN p_doctos_pp AS dp ON(DPD.DOCTO_PP_ID = dp.DOCTO_PP_ID)" +
                                       " WHERE dp.folio = @FOLIO";
                            SqlDataAdapter sqlData = new SqlDataAdapter(consulta, conn.SC);
                            sqlData.SelectCommand.Parameters.Add(new SqlParameter("@FOLIO", SqlDbType.VarChar));
                            sqlData.SelectCommand.Parameters["@FOLIO"].Value = FOLIO;
                            sqlData.Fill(_Encabezado);
                            sqlData.Dispose();
                        }
                        

                        conn.Desconectar();
                    }
                }
            }
            catch (Exception Ex)
            {
                conn.Desconectar();

                msg_local = Ex.Message;
            }

            Msg = msg_local;

            return _Encabezado;
        }

        public void EncabezadoPagosNO(DataGridView dataGridView1, C_USUARIOS usuario, bool estatus, bool fechas, string estatusCB ="", string fecha1= "",string fecha2 = "")
        {
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    if (con.ConectarSQL())
                    {
                        string consulta = "";
                        SqlCommand sc = new SqlCommand();
                        if (usuario.Requisitante == "S" && usuario.Tesoreria.ToUpper() == "FALSE")
                        {
                            if (usuario.U_ROL == "C")
                            {
                                #region
                                consulta = "SELECT ";
                                consulta += "      dpr.DOCTO_PP_ID, ";
                                consulta += "      COUNT (DPRD.DOCTO_PP_DET_ID) AS CANT_PAGOS, ";
                                consulta += "      SUM(DPRD.IMPORTE_AUTORIZADO) AS IMP_AUTO, ";
                                consulta += "      dpr.FOLIO, ";
                                consulta += "      dpr.FECHA_PAGO, ";
                                consulta += "      dpr.ESTATUS, ";
                                consulta += "      dpr.USUARIO_CREADOR, ";
                                consulta += "      dpr.FECHA_HORA_CREACION, ";
                                consulta += "      dpr.IMPORTE_PAGOS, ";
                                consulta += "      dpr.IMPORTE_AUTORIZADO, ";
                                consulta += "      dpr.IMPORTE_CAPTURISTA, ";
                                consulta += "      dpr.ESTATUS_PROC ";
                                consulta += " FROM P_DOCTOS_PP AS DPR ";
                                consulta += " INNER JOIN P_DOCTOS_PP_DET AS DPRD ON(DPR.DOCTO_PP_ID = DPRD.DOCTO_PP_ID) ";
                                consulta += " JOIN USUARIOS U ON U.Usuario_id = DPR.USUARIO_ID_CREADOR  ";
                                consulta += " WHERE (DPR.ESTATUS = 'A' OR DPR.ESTATUS = 'P') ";

                                if (usuario.Usuario != "")
                                {
                                    consulta += " AND U.USUARIO = '" + usuario.Usuario + "' ";
                                }
                                //EN caso de que el usuario este filtrando por fechas
                                if (fechas)
                                {
                                    consulta += " AND DPR.FECHA_PAGO BETWEEN @FECHA1 AND @FECHA2 ";
                                }
                                if (estatus)
                                {
                                    consulta += " AND ESTATUS_PROC = @ESTATUS_PROC";
                                }
                                consulta += " GROUP BY dpr.DOCTO_PP_ID ";
                                consulta += "         ,dpr.FOLIO ";
                                consulta += "         ,dpr.FECHA_PAGO ";
                                consulta += "         ,dpr.ESTATUS ";
                                consulta += "         ,dpr.USUARIO_CREADOR ";
                                consulta += "         ,dpr.FECHA_HORA_CREACION ";
                                consulta += "         ,dpr.IMPORTE_PAGOS ";
                                consulta += "         ,dpr.IMPORTE_AUTORIZADO ";
                                consulta += "         ,dpr.IMPORTE_CAPTURISTA ";
                                consulta += "         ,dpr.ESTATUS_PROC ";

                                sc = new SqlCommand(consulta, con.SC);
                                if (fechas)
                                {
                                    sc.Parameters.Add("@FECHA1", SqlDbType.Date).Value = Convert.ToDateTime(fecha1);
                                    sc.Parameters.Add("@FECHA2", SqlDbType.Date).Value = Convert.ToDateTime(fecha2);
                                }
                                if (estatus)
                                {
                                    sc.Parameters.Add("@ESTATUS_PROC", SqlDbType.VarChar).Value = ESTATUS_DOC(estatusCB);
                                }

                                #endregion
                            }
                            else
                            {
                                consulta = "SELECT ";
                                consulta += "      dpr.DOCTO_PR_ID DOCTO_PP_ID, ";
                                consulta += "      COUNT (DPRD.DOCTO_PR_DET_ID) AS CANT_PAGOS, ";
                                consulta += "      SUM(DPRD.IMPORTE_AUTORIZADO) AS IMP_AUTO, ";
                                consulta += "      dpr.FOLIO, ";
                                consulta += "      dpr.FECHA_PAGO, ";
                                consulta += "      dpr.ESTATUS, ";
                                consulta += "      dpr.USUARIO_CREADOR, ";
                                consulta += "      dpr.FECHA_HORA_CREACION, ";
                                consulta += "      dpr.IMPORTE_PAGOS, ";
                                consulta += "      dpr.IMPORTE_AUTORIZADO, ";
                                consulta += "      dpr.IMPORTE_CAPTURISTA, ";
                                consulta += "      dpr.ESTATUS_PROC, dpr.NIVEL ";
                                consulta += " FROM P_DOCTOS_PR AS DPR ";
                                consulta += " INNER JOIN P_DOCTOS_PR_DET AS DPRD ON(DPR.DOCTO_PR_ID = DPRD.DOCTO_PR_ID) ";
                                consulta += " join P_AUT_DOCTOS_PR pad on pad.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                                consulta += " join USUARIOS u on u.Usuario_id = pad.USUARIO_ID  ";
                                consulta += " where (u.Usuario = '" + usuario.Usuario + "' or DPR.USUARIO_CREADOR = '" + usuario.Usuario + "')";
                                consulta += " and DPR.ESTATUS_PROC = 'E' ";

                                //EN caso de que el usuario este filtrando por fechas
                                if (fechas)
                                {
                                    consulta += " AND DPR.FECHA_PAGO BETWEEN @FECHA1 AND @FECHA2 ";
                                }
                                if (estatus)
                                {
                                    consulta += " AND ESTATUS_PROC = @ESTATUS_PROC";
                                }
                                consulta += " GROUP BY dpr.DOCTO_PR_ID ";
                                consulta += "         ,dpr.FOLIO ";
                                consulta += "         ,dpr.FECHA_PAGO ";
                                consulta += "         ,dpr.ESTATUS ";
                                consulta += "         ,dpr.USUARIO_CREADOR ";
                                consulta += "         ,dpr.FECHA_HORA_CREACION ";
                                consulta += "         ,dpr.IMPORTE_PAGOS ";
                                consulta += "         ,dpr.IMPORTE_AUTORIZADO ";
                                consulta += "         ,dpr.IMPORTE_CAPTURISTA ";
                                consulta += "         ,dpr.ESTATUS_PROC, dpr.NIVEL ";

                                sc = new SqlCommand(consulta, con.SC);
                                if (fechas)
                                {
                                    sc.Parameters.Add("@FECHA1", SqlDbType.Date).Value = Convert.ToDateTime(fecha1);
                                    sc.Parameters.Add("@FECHA2", SqlDbType.Date).Value = Convert.ToDateTime(fecha2);
                                }
                                if (estatus)
                                {
                                    sc.Parameters.Add("@ESTATUS_PROC", SqlDbType.VarChar).Value = ESTATUS_DOC(estatusCB);
                                }

                            }
                        }
                        else
                        {
                            if(usuario.Tesoreria.ToUpper() == "TRUE" || usuario.U_ROL == "A")
                            {
                                consulta = "SELECT ";
                                consulta += "      dpr.DOCTO_PR_ID DOCTO_PP_ID, ";
                                consulta += "      COUNT (DPRD.DOCTO_PR_DET_ID) AS CANT_PAGOS, ";
                                consulta += "      SUM(DPRD.IMPORTE_AUTORIZADO) AS IMP_AUTO, ";
                                consulta += "      dpr.FOLIO, ";
                                consulta += "      dpr.FECHA_PAGO, ";
                                consulta += "      dpr.ESTATUS, ";
                                consulta += "      dpr.USUARIO_CREADOR, ";
                                consulta += "      dpr.FECHA_HORA_CREACION, ";
                                consulta += "      dpr.IMPORTE_PAGOS, ";
                                consulta += "      dpr.IMPORTE_AUTORIZADO, ";
                                consulta += "      dpr.IMPORTE_CAPTURISTA, ";
                                consulta += "      dpr.ESTATUS_PROC , dpr.NIVEL";
                                consulta += " FROM P_DOCTOS_PR AS DPR ";
                                consulta += " INNER JOIN P_DOCTOS_PR_DET AS DPRD ON(DPR.DOCTO_PR_ID = DPRD.DOCTO_PR_ID) ";
                                consulta += " join P_AUT_DOCTOS_PR pad on pad.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                                consulta += " join USUARIOS u on u.Usuario_id = pad.USUARIO_ID  ";
                                consulta += " where (u.Usuario = '" + usuario.Usuario + "' or DPR.USUARIO_CREADOR = '" + usuario.Usuario + "')";
                                consulta += " and DPR.ESTATUS_PROC = 'E' ";

                                //EN caso de que el usuario este filtrando por fechas
                                if (fechas)
                                {
                                    consulta += " AND DPR.FECHA_PAGO BETWEEN @FECHA1 AND @FECHA2 ";
                                }
                                if (estatus)
                                {
                                    consulta += " AND ESTATUS_PROC = @ESTATUS_PROC";
                                }
                                consulta += " GROUP BY dpr.DOCTO_PR_ID ";
                                consulta += "         ,dpr.FOLIO ";
                                consulta += "         ,dpr.FECHA_PAGO ";
                                consulta += "         ,dpr.ESTATUS ";
                                consulta += "         ,dpr.USUARIO_CREADOR ";
                                consulta += "         ,dpr.FECHA_HORA_CREACION ";
                                consulta += "         ,dpr.IMPORTE_PAGOS ";
                                consulta += "         ,dpr.IMPORTE_AUTORIZADO ";
                                consulta += "         ,dpr.IMPORTE_CAPTURISTA ";
                                consulta += "         ,dpr.ESTATUS_PROC , dpr.NIVEL";

                                sc = new SqlCommand(consulta, con.SC);
                                if (fechas)
                                {
                                    sc.Parameters.Add("@FECHA1", SqlDbType.Date).Value = Convert.ToDateTime(fecha1);
                                    sc.Parameters.Add("@FECHA2", SqlDbType.Date).Value = Convert.ToDateTime(fecha2);
                                }
                                if (estatus)
                                {
                                    sc.Parameters.Add("@ESTATUS_PROC", SqlDbType.VarChar).Value = ESTATUS_DOC(estatusCB);
                                }

                               // sc = new SqlCommand(consulta, con.SC);

                            }
                        }
                        SqlDataReader sdr = sc.ExecuteReader();
                        dataGridView1.Rows.Clear();

                        while (sdr.Read())
                        {
                            dataGridView1.Rows.Add();

                            dataGridView1["Folio", dataGridView1.RowCount - 1].Value = Convert.ToString(sdr["FOLIO"]);
                            dataGridView1["Estatus", dataGridView1.RowCount - 1].Value = EstatusDocumento(Convert.ToString(sdr["ESTATUS"]));
                            string aux = Convert.ToString(sdr["IMPORTE_PAGOS"]);
                            if (aux.Length == 0)
                                aux = "0.0";
                            dataGridView1["Importe_total", dataGridView1.RowCount - 1].Value = Convert.ToDouble(aux);
                            aux = Convert.ToString(sdr["IMP_AUTO"]);
                            if (aux.Length == 0)
                                aux = "0.0";
                            dataGridView1["ImporteAutorizado", dataGridView1.RowCount - 1].Value = Convert.ToDouble(aux);
                            dataGridView1["Numero_pagos", dataGridView1.RowCount - 1].Value = Convert.ToString(sdr["CANT_PAGOS"]);
                            dataGridView1["Fecha_pago", dataGridView1.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_PAGO"]).ToString("dd/MM/yyyy");
                            dataGridView1["Usuario", dataGridView1.RowCount - 1].Value = Convert.ToString(sdr["USUARIO_CREADOR"]);
                            dataGridView1["Fecha_creacion", dataGridView1.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_HORA_CREACION"]).ToString("dd/MM/yyyy HH:mm:ss");
                            aux = Convert.ToString(sdr["ESTATUS_PROC"]);
                            if (aux.Length == 0)
                                aux = "";
                            dataGridView1["ESTATUS_PROC", dataGridView1.RowCount - 1].Value = this.EstatusProceso(aux);

                            aux = Convert.ToString(sdr["IMPORTE_CAPTURISTA"]);
                            if (aux.Length == 0)
                                aux = "0.0";
                            dataGridView1["IMPORTE_CAPTURISTA", dataGridView1.RowCount - 1].Value = Convert.ToDouble(aux);

                            dataGridView1["Autorizacion", dataGridView1.RowCount - 1].Value = this.EstatusProceso(Convert.ToString(sdr["ESTATUS_PROC"]));
                            dataGridView1["DOCTO_PR_ID", dataGridView1.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_PP_ID"]);
                            dataGridView1["NIVEL", dataGridView1.RowCount - 1].Value = Convert.ToString(sdr["NIVEL"]);
                        }

                        

                        sdr.Close();
                        sc.Dispose();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error al consultar\n" + ex.Message,"Mensaje de la aplicación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }


        public void EncabezadoPagosPeticiones(DataGridView dgvProgramaciones, DataGridView dgvPeticiones, C_USUARIOS usuario, bool estatus, bool fechas, string estatusCB = "", string fecha1 = "", string fecha2 = "")
        {
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    C_ConexionSQL con = new C_ConexionSQL();
                    if (con.ConectarSQL())
                    {
                        string consulta = "";
                        SqlCommand sc = new SqlCommand();
                        SqlDataReader sdr;

                        //cheamos si es de tesoreria
                        if (usuario.Tesoreria.ToUpper() == "TRUE" || usuario.U_ROL == "A")
                        {
                            #region codigo es tesoreria
                            consulta = "SELECT ";
                            consulta += "      dpr.DOCTO_PR_ID DOCTO_PP_ID, ";
                            consulta += "      COUNT (DPRD.DOCTO_PR_DET_ID) AS CANT_PAGOS, ";
                            consulta += "      SUM(DPRD.IMPORTE_AUTORIZADO) AS IMP_AUTO, ";
                            consulta += "      dpr.FOLIO, ";
                            consulta += "      dpr.FECHA_PAGO, ";
                            consulta += "      dpr.ESTATUS, ";
                            consulta += "      dpr.USUARIO_CREADOR, ";
                            consulta += "      dpr.FECHA_HORA_CREACION, ";
                            consulta += "      dpr.IMPORTE_PAGOS, ";
                            consulta += "      dpr.IMPORTE_AUTORIZADO, ";
                            consulta += "      dpr.IMPORTE_CAPTURISTA, ";
                            consulta += "      dpr.ESTATUS_PROC ,dpr.NIVEL ";
                            consulta += " FROM P_DOCTOS_PR AS DPR ";
                            consulta += " INNER JOIN P_DOCTOS_PR_DET AS DPRD ON(DPR.DOCTO_PR_ID = DPRD.DOCTO_PR_ID) ";
                            consulta += " join P_AUT_DOCTOS_PR pad on pad.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                            consulta += " join USUARIOS u on u.Usuario_id = pad.USUARIO_ID  ";
                            consulta += " where (u.Usuario = '" + usuario.Usuario + "' or DPR.USUARIO_CREADOR = '" + usuario.Usuario + "')";
                            consulta += " and  pad.ESTATUS != 'P' ";

                            //EN caso de que el usuario este filtrando por fechas
                            if (fechas)
                            {
                                consulta += " AND DPR.FECHA_PAGO BETWEEN @FECHA1 AND @FECHA2 ";
                            }
                            if (estatus)
                            {
                                consulta += " AND ESTATUS_PROC = @ESTATUS_PROC";
                            }
                            consulta += " GROUP BY dpr.DOCTO_PR_ID ";
                            consulta += "         ,dpr.FOLIO ";
                            consulta += "         ,dpr.FECHA_PAGO ";
                            consulta += "         ,dpr.ESTATUS ";
                            consulta += "         ,dpr.USUARIO_CREADOR ";
                            consulta += "         ,dpr.FECHA_HORA_CREACION ";
                            consulta += "         ,dpr.IMPORTE_PAGOS ";
                            consulta += "         ,dpr.IMPORTE_AUTORIZADO ";
                            consulta += "         ,dpr.IMPORTE_CAPTURISTA ";
                            consulta += "         ,dpr.ESTATUS_PROC,dpr.NIVEL ";

                            sc = new SqlCommand(consulta, con.SC);
                            if (fechas)
                            {
                                sc.Parameters.Add("@FECHA1", SqlDbType.Date).Value = Convert.ToDateTime(fecha1);
                                sc.Parameters.Add("@FECHA2", SqlDbType.Date).Value = Convert.ToDateTime(fecha2);
                            }
                            if (estatus)
                            {
                                sc.Parameters.Add("@ESTATUS_PROC", SqlDbType.VarChar).Value = ESTATUS_DOC(estatusCB);
                            }

                            sdr = sc.ExecuteReader();
                            dgvProgramaciones.Rows.Clear();

                            while (sdr.Read())
                            {
                                dgvProgramaciones.Rows.Add();

                                dgvProgramaciones["Folio", dgvProgramaciones.RowCount - 1].Value = Convert.ToString(sdr["FOLIO"]);
                                dgvProgramaciones["Estatus", dgvProgramaciones.RowCount - 1].Value = EstatusDocumento(Convert.ToString(sdr["ESTATUS"]));
                                string aux = Convert.ToString(sdr["IMPORTE_PAGOS"]);
                                if (aux.Length == 0)
                                    aux = "0.0";
                                dgvProgramaciones["Importe_total", dgvProgramaciones.RowCount - 1].Value = Convert.ToDouble(aux);
                                aux = Convert.ToString(sdr["IMP_AUTO"]);
                                if (aux.Length == 0)
                                    aux = "0.0";
                                dgvProgramaciones["ImporteAutorizado", dgvProgramaciones.RowCount - 1].Value = Convert.ToDouble(aux);
                                dgvProgramaciones["Numero_pagos", dgvProgramaciones.RowCount - 1].Value = Convert.ToString(sdr["CANT_PAGOS"]);
                                dgvProgramaciones["Fecha_pago", dgvProgramaciones.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_PAGO"]).ToString("dd/MM/yyyy");
                                dgvProgramaciones["Usuario", dgvProgramaciones.RowCount - 1].Value = Convert.ToString(sdr["USUARIO_CREADOR"]);
                                dgvProgramaciones["Fecha_creacion", dgvProgramaciones.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_HORA_CREACION"]).ToString("dd/MM/yyyy HH:mm:ss");
                                aux = Convert.ToString(sdr["ESTATUS_PROC"]);
                                if (aux.Length == 0)
                                    aux = "";
                                dgvProgramaciones["ESTATUS_PROC", dgvProgramaciones.RowCount - 1].Value = this.EstatusProceso(aux);

                                aux = Convert.ToString(sdr["IMPORTE_CAPTURISTA"]);
                                if (aux.Length == 0)
                                    aux = "0.0";
                                dgvProgramaciones["IMPORTE_CAPTURISTA", dgvProgramaciones.RowCount - 1].Value = Convert.ToDouble(aux);

                                string autorizacion = this.EstatusProceso(Convert.ToString(sdr["ESTATUS_PROC"]));

                                dgvProgramaciones["Autorizacion", dgvProgramaciones.RowCount - 1].Value = autorizacion;
                                dgvProgramaciones["DOCTO_PR_ID", dgvProgramaciones.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_PP_ID"]);
                                dgvProgramaciones["NIVEL", dgvProgramaciones.RowCount - 1].Value = Convert.ToString(sdr["NIVEL"]);

                                switch (autorizacion)
                                {
                                    case "Autorizado":
                                        dgvProgramaciones.Rows[dgvProgramaciones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(51,202,127);
                                        dgvProgramaciones.Rows[dgvProgramaciones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 180, 112);
                                        break;
                                    case "Pendiente":

                                        Color Backcolor = new Color();
                                        Color Selectcolor = new Color();
                                        Color selectionForeColor = new Color();
                                        

                                        ChecarSiUsuarioAutorizo(Convert.ToString(sdr["NIVEL"]), Convert.ToString(sdr["FOLIO"]), autorizacion, usuario, ref Backcolor, ref Selectcolor, ref selectionForeColor);

                                        dgvProgramaciones.Rows[dgvProgramaciones.RowCount - 1].DefaultCellStyle.BackColor = Backcolor;
                                        dgvProgramaciones.Rows[dgvProgramaciones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Selectcolor;
                                        dgvProgramaciones.Rows[dgvProgramaciones.RowCount - 1].DefaultCellStyle.SelectionForeColor = selectionForeColor;
                                        break;
                                }
                            }
                            sdr.Close();
                            sc.Dispose();
                            #endregion

                            //checamos si ademas es requisitante
                            if (usuario.Requisitante == "S")
                            {
                                #region
                                consulta = "SELECT ";
                                consulta += "      dpr.DOCTO_PP_ID, ";
                                consulta += "      COUNT (DPRD.DOCTO_PP_DET_ID) AS CANT_PAGOS, ";
                                consulta += "      SUM(DPRD.IMPORTE_AUTORIZADO) AS IMP_AUTO, ";
                                consulta += "      dpr.FOLIO, ";
                                consulta += "      dpr.FECHA_PAGO, ";
                                consulta += "      dpr.ESTATUS, ";
                                consulta += "      dpr.USUARIO_CREADOR, ";
                                consulta += "      dpr.FECHA_HORA_CREACION, ";
                                consulta += "      dpr.IMPORTE_PAGOS, ";
                                consulta += "      dpr.IMPORTE_AUTORIZADO, ";
                                consulta += "      dpr.IMPORTE_CAPTURISTA, ";
                                consulta += "      dpr.ESTATUS_PROC ";
                                consulta += " FROM P_DOCTOS_PP AS DPR ";
                                consulta += " INNER JOIN P_DOCTOS_PP_DET AS DPRD ON(DPR.DOCTO_PP_ID = DPRD.DOCTO_PP_ID) ";
                                consulta += " JOIN USUARIOS U ON U.Usuario_id = DPR.USUARIO_ID_CREADOR  ";
                                consulta += " WHERE (DPR.ESTATUS = 'A' OR DPR.ESTATUS = 'P' OR DPR.ESTATUS = 'X' ) ";

                                if (usuario.Usuario != "")
                                {
                                    consulta += " AND U.USUARIO = '" + usuario.Usuario + "' ";
                                }
                                //EN caso de que el usuario este filtrando por fechas
                                if (fechas)
                                {
                                    consulta += " AND DPR.FECHA_PAGO BETWEEN @FECHA1 AND @FECHA2 ";
                                }
                                if (estatus)
                                {
                                    consulta += " AND ESTATUS_PROC = @ESTATUS_PROC";
                                }
                                consulta += " GROUP BY dpr.DOCTO_PP_ID ";
                                consulta += "         ,dpr.FOLIO ";
                                consulta += "         ,dpr.FECHA_PAGO ";
                                consulta += "         ,dpr.ESTATUS ";
                                consulta += "         ,dpr.USUARIO_CREADOR ";
                                consulta += "         ,dpr.FECHA_HORA_CREACION ";
                                consulta += "         ,dpr.IMPORTE_PAGOS ";
                                consulta += "         ,dpr.IMPORTE_AUTORIZADO ";
                                consulta += "         ,dpr.IMPORTE_CAPTURISTA ";
                                consulta += "         ,dpr.ESTATUS_PROC ";

                                sc = new SqlCommand(consulta, con.SC);
                                if (fechas)
                                {
                                    sc.Parameters.Add("@FECHA1", SqlDbType.Date).Value = Convert.ToDateTime(fecha1);
                                    sc.Parameters.Add("@FECHA2", SqlDbType.Date).Value = Convert.ToDateTime(fecha2);
                                }
                                if (estatus)
                                {
                                    sc.Parameters.Add("@ESTATUS_PROC", SqlDbType.VarChar).Value = ESTATUS_DOC(estatusCB);
                                }

                                #endregion

                                sdr = sc.ExecuteReader();
                                dgvPeticiones.Rows.Clear();

                                while (sdr.Read())
                                {
                                    dgvPeticiones.Rows.Add();

                                    dgvPeticiones["Folio_2", dgvPeticiones.RowCount - 1].Value = Convert.ToString(sdr["FOLIO"]);
                                    dgvPeticiones["Estatus_2", dgvPeticiones.RowCount - 1].Value = EstatusDocumento(Convert.ToString(sdr["ESTATUS"]));
                                    string aux = Convert.ToString(sdr["IMPORTE_PAGOS"]);
                                    if (aux.Length == 0)
                                        aux = "0.0";
                                    dgvPeticiones["Importe_total_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDouble(aux);
                                    aux = Convert.ToString(sdr["IMP_AUTO"]);
                                    if (aux.Length == 0)
                                        aux = "0.0";
                                    dgvPeticiones["ImporteAutorizado_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDouble(aux);
                                    dgvPeticiones["Numero_pagos_2", dgvPeticiones.RowCount - 1].Value = Convert.ToString(sdr["CANT_PAGOS"]);
                                    dgvPeticiones["Fecha_pago_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_PAGO"]).ToString("dd/MM/yyyy");
                                    dgvPeticiones["Usuario_2", dgvPeticiones.RowCount - 1].Value = Convert.ToString(sdr["USUARIO_CREADOR"]);
                                    dgvPeticiones["Fecha_creacion_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_HORA_CREACION"]).ToString("dd/MM/yyyy HH:mm:ss");
                                    aux = Convert.ToString(sdr["ESTATUS_PROC"]);
                                    if (aux.Length == 0)
                                        aux = "";

                                    string estatusProceso = this.EstatusProceso(aux);

                                    dgvPeticiones["ESTATUS_PROC_2", dgvPeticiones.RowCount - 1].Value = estatusProceso; 

                                    aux = Convert.ToString(sdr["IMPORTE_CAPTURISTA"]);
                                    if (aux.Length == 0)
                                        aux = "0.0";
                                    dgvPeticiones["IMPORTE_CAPTURISTA_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDouble(aux);

                                    dgvPeticiones["Autorizacion_2", dgvPeticiones.RowCount - 1].Value = this.EstatusProceso(Convert.ToString(sdr["ESTATUS_PROC"]));
                                    dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_PP_ID"]);



                                    switch (estatusProceso)
                                    {
                                        case "Autorizado":
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(51, 202, 127);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 180, 112);
                                            break;
                                        case "Petición en Revisión":
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(238, 255, 153);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 255, 71);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 23, 66);
                                            break;
                                        case "Pendiente de enviar":
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(251, 160, 75);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(251, 139, 35);
                                            //dgvProgramaciones.Rows[dgvProgramaciones.RowCount - 1].DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 23, 66);
                                            break;
                                        case "Cancelado":
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(235, 93, 71);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 61, 35);
                                            break;
                                    }
                                }



                                sdr.Close();
                                sc.Dispose();
                            }
                        }
                        else
                        {
                            //verificamos si es requisitante
                            if (usuario.Requisitante == "S")
                            {
                                #region
                                consulta = "SELECT ";
                                consulta += "      dpr.DOCTO_PP_ID, ";
                                consulta += "      COUNT (DPRD.DOCTO_PP_DET_ID) AS CANT_PAGOS, ";
                                consulta += "      SUM(DPRD.IMPORTE_AUTORIZADO) AS IMP_AUTO, ";
                                consulta += "      dpr.FOLIO, ";
                                consulta += "      dpr.FECHA_PAGO, ";
                                consulta += "      dpr.ESTATUS, ";
                                consulta += "      dpr.USUARIO_CREADOR, ";
                                consulta += "      dpr.FECHA_HORA_CREACION, ";
                                consulta += "      dpr.IMPORTE_PAGOS, ";
                                consulta += "      dpr.IMPORTE_AUTORIZADO, ";
                                consulta += "      dpr.IMPORTE_CAPTURISTA, ";
                                consulta += "      dpr.ESTATUS_PROC ";
                                consulta += " FROM P_DOCTOS_PP AS DPR ";
                                consulta += " INNER JOIN P_DOCTOS_PP_DET AS DPRD ON(DPR.DOCTO_PP_ID = DPRD.DOCTO_PP_ID) ";
                                consulta += " JOIN USUARIOS U ON U.Usuario_id = DPR.USUARIO_ID_CREADOR  ";
                                consulta += " WHERE (DPR.ESTATUS = 'A' OR DPR.ESTATUS = 'P' OR DPR.ESTATUS = 'X') ";

                                if (usuario.Usuario != "")
                                {
                                    consulta += " AND U.USUARIO = '" + usuario.Usuario + "' ";
                                }
                                //EN caso de que el usuario este filtrando por fechas
                                if (fechas)
                                {
                                    consulta += " AND DPR.FECHA_PAGO BETWEEN @FECHA1 AND @FECHA2 ";
                                }
                                if (estatus)
                                {
                                    consulta += " AND ESTATUS_PROC = @ESTATUS_PROC";
                                }
                                consulta += " GROUP BY dpr.DOCTO_PP_ID ";
                                consulta += "         ,dpr.FOLIO ";
                                consulta += "         ,dpr.FECHA_PAGO ";
                                consulta += "         ,dpr.ESTATUS ";
                                consulta += "         ,dpr.USUARIO_CREADOR ";
                                consulta += "         ,dpr.FECHA_HORA_CREACION ";
                                consulta += "         ,dpr.IMPORTE_PAGOS ";
                                consulta += "         ,dpr.IMPORTE_AUTORIZADO ";
                                consulta += "         ,dpr.IMPORTE_CAPTURISTA ";
                                consulta += "         ,dpr.ESTATUS_PROC ";

                                sc = new SqlCommand(consulta, con.SC);
                                if (fechas)
                                {
                                    sc.Parameters.Add("@FECHA1", SqlDbType.Date).Value = Convert.ToDateTime(fecha1);
                                    sc.Parameters.Add("@FECHA2", SqlDbType.Date).Value = Convert.ToDateTime(fecha2);
                                }
                                if (estatus)
                                {
                                    sc.Parameters.Add("@ESTATUS_PROC", SqlDbType.VarChar).Value = ESTATUS_DOC(estatusCB);
                                }

                                #endregion

                                sdr = sc.ExecuteReader();
                                dgvPeticiones.Rows.Clear();

                                while (sdr.Read())
                                {
                                    dgvPeticiones.Rows.Add();

                                    dgvPeticiones["Folio_2", dgvPeticiones.RowCount - 1].Value = Convert.ToString(sdr["FOLIO"]);
                                    dgvPeticiones["Estatus_2", dgvPeticiones.RowCount - 1].Value = EstatusDocumento(Convert.ToString(sdr["ESTATUS"]));
                                    string aux = Convert.ToString(sdr["IMPORTE_PAGOS"]);
                                    if (aux.Length == 0)
                                        aux = "0.0";
                                    dgvPeticiones["Importe_total_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDouble(aux);
                                    aux = Convert.ToString(sdr["IMP_AUTO"]);
                                    if (aux.Length == 0)
                                        aux = "0.0";
                                    dgvPeticiones["ImporteAutorizado_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDouble(aux);
                                    dgvPeticiones["Numero_pagos_2", dgvPeticiones.RowCount - 1].Value = Convert.ToString(sdr["CANT_PAGOS"]);
                                    dgvPeticiones["Fecha_pago_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_PAGO"]).ToString("dd/MM/yyyy");
                                    dgvPeticiones["Usuario_2", dgvPeticiones.RowCount - 1].Value = Convert.ToString(sdr["USUARIO_CREADOR"]);
                                    dgvPeticiones["Fecha_creacion_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDateTime(sdr["FECHA_HORA_CREACION"]).ToString("dd/MM/yyyy HH:mm:ss");
                                    aux = Convert.ToString(sdr["ESTATUS_PROC"]);
                                    if (aux.Length == 0)
                                        aux = "";

                                    string estatusProceso = this.EstatusProceso(aux);

                                    dgvPeticiones["ESTATUS_PROC_2", dgvPeticiones.RowCount - 1].Value = estatusProceso;

                                    aux = Convert.ToString(sdr["IMPORTE_CAPTURISTA"]);
                                    if (aux.Length == 0)
                                        aux = "0.0";
                                    dgvPeticiones["IMPORTE_CAPTURISTA_2", dgvPeticiones.RowCount - 1].Value = Convert.ToDouble(aux);

                                    dgvPeticiones["Autorizacion_2", dgvPeticiones.RowCount - 1].Value = this.EstatusProceso(Convert.ToString(sdr["ESTATUS_PROC"]));
                                    dgvPeticiones["DOCTO_PR_ID_2", dgvPeticiones.RowCount - 1].Value = Convert.ToString(sdr["DOCTO_PP_ID"]);

                                    switch (estatusProceso)
                                    {
                                        case "Autorizado":
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(51, 202, 127);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 180, 112);
                                            break;
                                        case "Petición en Revisión":
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(238, 255, 153);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 255, 71);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 23, 66);
                                            break;
                                        case "Pendiente de enviar":
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(251, 160, 75);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(251, 139, 35);
                                            //dgvProgramaciones.Rows[dgvProgramaciones.RowCount - 1].DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 23, 66);
                                            break;
                                        case "Cancelado":
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.BackColor = Color.FromArgb(235, 93, 71);
                                            dgvPeticiones.Rows[dgvPeticiones.RowCount - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 61, 35);
                                            break;
                                    }
                                }

                                sdr.Close();
                                sc.Dispose();
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private string EstatusProceso(string aux)
        {
            switch (aux) {
                case "A": aux = "Autorizado"; break;
                case "P": aux = "Pendiente de enviar"; break;
                case "X": aux = "Cancelado"; break;
                case "F": aux = "Finalizado"; break;
                case "L": aux = "Liberado"; break;
                case "B": aux = "Banco"; break;
                case "E": aux = "Pendiente"; break;
                case "R": aux = "Rechazado"; break;
                case "T": aux = "Proceso Pago"; break;
                case "H": aux = "Petición en Revisión"; break;
                default:  aux = "No asignado"; break;
            }
            return aux;
        }

        private string EstatusDocumento(string aux)
        {
            switch (aux)
            {
                case "A":
                    aux = "Activo";
                    break;
                case "X":
                    aux = "Cancelado";
                    break;
                case "R":
                    aux = "Rechazado";
                    break;
                case "P":
                    aux = "Pendiente";
                    break;
                case "F":
                    aux = "Finalizado";
                    break;
                case "T": aux = "Revisado";
                    break;
                default:
                    aux = "";
                    break;
            }

            return aux;
        }

        private string ESTATUS_DOC(string aux)
        {
            string ESTATU_PROC = "";
            if (aux == "Autorizado")
            {
                ESTATU_PROC = "A";
            }
            else if (aux == "Liberado")
            {
                ESTATU_PROC = "L";
            }
            else if (aux == "Pendiente")
            {
                ESTATU_PROC = "E";
            }
            else if (aux == "Pendiente por enviar")
            {
                ESTATU_PROC = "E";
            }
            else if (aux == "Cancelado")
            {
                ESTATU_PROC = "X";
            }
            else if (aux == "No asignado")
                ESTATU_PROC = "N";

            return ESTATU_PROC;
        }


        private bool ChecarSiUsuarioAutorizo(string nivel,string folio,string estatusProc ,C_USUARIOS usuarioLogueado, ref Color Backcolor, ref Color Selectcolor, ref Color selectionForeColor)
        {
            #region para mostrar autorizar o no
            bool bloqueado = false;
            Backcolor = Color.FromArgb(238, 255, 153);
            Selectcolor = Color.FromArgb(224, 255, 71);
            selectionForeColor = Color.FromArgb(60, 23, 66);

            if (nivel != "1")
            {
                if (AUTORIZADO(folio,usuarioLogueado) == "S")
                {
                    //false
                    Backcolor = Color.FromArgb(180, 212, 238);
                    Selectcolor = Color.FromArgb(139, 188, 229);                    
                   // selectionForeColor = Color.FromArgb(60, 23, 66);
                }
                else
                {
                    if (estatusProc != "Autorizado")
                    {
                        if (USUARIO_AUT_PROG(folio, usuarioLogueado))
                        {
                            //true
                            Backcolor = Color.FromArgb(238, 255, 153);
                            Selectcolor = Color.FromArgb(224, 255, 71);
                            selectionForeColor = Color.FromArgb(60, 23, 66);
                            bloqueado = true;
                        }
                        else
                        {
                            //false
                            Backcolor = Color.FromArgb(180, 212, 238);
                            Selectcolor = Color.FromArgb(139, 188, 229);
                        }
                    }
                }

            }
            else
            {
                if (usuarioLogueado.NIVEL_SUPREMO == 1)
                {
                    if (AUTORIZADO_NIVEL_1(folio,usuarioLogueado) == "S")
                    {
                        //false
                        Backcolor = Color.FromArgb(180, 212, 238);
                        Selectcolor = Color.FromArgb(139, 188, 229);
                    }
                    else
                    {
                        if (estatusProc != "Autorizado")
                        {
                            if (USUARIO_AUT_PROG(folio,usuarioLogueado))
                            {
                                //true
                                Backcolor = Color.FromArgb(238, 255, 153);
                                Selectcolor = Color.FromArgb(224, 255, 71);
                                selectionForeColor = Color.FromArgb(60, 23, 66);
                                bloqueado = true;
                            }
                            else
                            {
                                //false
                                Backcolor = Color.FromArgb(180, 212, 238);
                                Selectcolor = Color.FromArgb(139, 188, 229);
                            }
                        }
                    }
                }
                else
                {
                    Backcolor = Color.FromArgb(180, 212, 238);
                    Selectcolor = Color.FromArgb(139, 188, 229);
                }

                
            }
            #endregion
            return bloqueado;
        }

        private string AUTORIZADO(string folio, C_USUARIOS usuarioLogueado)
        {
            string autorizado = "N";
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string query = "SELECT ";
                    query += " case when COUNT(*)=0 then 'N' else 'S' end AS AUTORIZADO ";
                    query += "  from P_AUT_DOCTOS_PR as ADPR ";
                    query += " inner join P_DOCTOS_PR_DET as DPRD on ADPR.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                    query += " inner join P_DOCTOS_PR as DPR on DPRD.DOCTO_PR_ID = DPR.DOCTO_PR_ID ";
                    query += " where DPR.FOLIO = @FOLIO and ADPR.NIVEL= @NIVEL  and ADPR.ESTATUS='A' and ADPR.USUARIO_ID= @USUARIO ";

                    SqlCommand sc = new SqlCommand(query, con.SC);
                    sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                    sc.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = usuarioLogueado.NIVEL;
                    sc.Parameters.Add("@USUARIO", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                        autorizado = Convert.ToString(sdr["AUTORIZADO"]);
                    sc.Dispose();
                    sdr.Close();

                    if (autorizado == "N")
                    {
                        if (usuarioLogueado.NIVEL_SUPREMO != 0)
                        {
                            query = "SELECT ";
                            query += " case when COUNT(*)=0 then 'N' else 'S' end AS AUTORIZADO ";
                            query += "  from P_AUT_DOCTOS_PR as ADPR ";
                            query += " inner join P_DOCTOS_PR_DET as DPRD on ADPR.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                            query += " inner join P_DOCTOS_PR as DPR on DPRD.DOCTO_PR_ID = DPR.DOCTO_PR_ID ";
                            query += " where DPR.FOLIO = @FOLIO and ADPR.NIVEL= @NIVEL  and ADPR.ESTATUS='A' and ADPR.USUARIO_ID= @USUARIO ";

                            sc = new SqlCommand(query, con.SC);
                            sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                            sc.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = usuarioLogueado.NIVEL_SUPREMO;
                            sc.Parameters.Add("@USUARIO", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                            sdr = sc.ExecuteReader();
                            while (sdr.Read())
                                autorizado = Convert.ToString(sdr["AUTORIZADO"]);
                            sc.Dispose();
                            sdr.Close();
                        }
                    }

                    con.Desconectar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return autorizado;
        }

        private string AUTORIZADO_NIVEL_1(string folio,C_USUARIOS usuarioLogueado)
        {
            string autorizado = "N";
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {

                    if (usuarioLogueado.NIVEL_SUPREMO != 0)
                    {
                        string query = "SELECT ";
                        query += " case when COUNT(*)=0 then 'N' else 'S' end AS AUTORIZADO ";
                        query += "  from P_AUT_DOCTOS_PR as ADPR ";
                        query += " inner join P_DOCTOS_PR_DET as DPRD on ADPR.DOCTO_PR_DET_ID = DPRD.DOCTO_PR_DET_ID ";
                        query += " inner join P_DOCTOS_PR as DPR on DPRD.DOCTO_PR_ID = DPR.DOCTO_PR_ID ";
                        query += " where DPR.FOLIO = @FOLIO and ADPR.NIVEL= @NIVEL  and ADPR.ESTATUS='A' and ADPR.USUARIO_ID= @USUARIO ";

                        SqlCommand sc = new SqlCommand(query, con.SC);
                        sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                        sc.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = usuarioLogueado.NIVEL_SUPREMO;
                        sc.Parameters.Add("@USUARIO", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                        SqlDataReader sdr = sc.ExecuteReader();
                        while (sdr.Read())
                            autorizado = Convert.ToString(sdr["AUTORIZADO"]);
                        sc.Dispose();
                        sdr.Close();
                    }


                    con.Desconectar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return autorizado;
        }

        private bool USUARIO_AUT_PROG(string folio,C_USUARIOS usuarioLogueado)
        {
            bool autorizado = false;
            try
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string query = "select u.Usuario_id from P_DOCTOS_PR pd ";
                    query += " JOIN P_DOCTOS_PR_DET ppd on ppd.DOCTO_PR_ID = pd.DOCTO_PR_ID ";
                    query += "  JOIN P_AUT_DOCTOS_PR pad on pad.DOCTO_PR_DET_ID = ppd.DOCTO_PR_DET_ID ";
                    query += " JOIN USUARIOS u on u.Usuario_id = pad.USUARIO_ID ";
                    query += " where pd.FOLIO = @FOLIO ";
                    query += " AND u.Usuario_id = @USUARIO ";

                    SqlCommand sc = new SqlCommand(query, con.SC);
                    sc.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                    sc.Parameters.Add("@USUARIO", SqlDbType.Int).Value = usuarioLogueado.Usuario_id;
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        autorizado = true;
                        break;
                    }
                    sc.Dispose();
                    sdr.Close();
                    con.Desconectar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return autorizado;
        }
    }
}
