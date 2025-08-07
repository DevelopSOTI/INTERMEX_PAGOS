using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirebirdSql.Data.FirebirdClient;

namespace PagosIntermex
{
    class UtileriasC
    {
        public string OBTENER_FOLIO(C_ConexionSQL conn, SqlTransaction transaction, out string msg)
        {
            string _folio = "", msg_local = "";
            int _consecutivo = 0;

            try
            {
                // OBTIENE CONSECUTIVO
                SqlCommand cmd = new SqlCommand("SELECT concat(s_serie,s_consecutivo) as SERIE, s_consecutivo FROM p_series WHERE s_estatus = 'A'", conn.SC, transaction);
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    _folio = Convert.ToString(read["SERIE"].ToString());
                    _consecutivo = Convert.ToInt32(read["s_consecutivo"].ToString());
                }
                read.Close();
                cmd.Dispose();

                // INCREMENTA CONSECUTIVO
                _consecutivo++;

                // ACTUALIZA CONSECUTIVO
                // cmd = new FbCommand("UPDATE series " + " SET s_consecutivo = " + _consecutivo + " WHERE(s_id = 1)", conn.FBC, transaction);
                cmd = new SqlCommand("UPDATE p_series " + " SET s_consecutivo = " + _consecutivo + " WHERE(s_estatus = 'A')", conn.SC, transaction);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                msg_local = ex.Message;
            }

            msg = msg_local;

            return _folio;
        }

        public int GEN_DOCTO_ID(C_CONEXIONFIREBIRD conn, FbTransaction transaction)
        {
            int docto_id = 0;

            try
            {
                FbCommand cmd = new FbCommand("EXECUTE PROCEDURE gen_docto_id", conn.FBC, transaction);
                FbDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    docto_id = Convert.ToInt32(read["ID_DOCTO"].ToString());
                }
                read.Close();
                cmd.Dispose();
            }
            catch 
            {                
                docto_id = 0;
            }

            return docto_id;
        }

    }
}
