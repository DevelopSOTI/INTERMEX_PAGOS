using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FirebirdSql.Data.FirebirdClient;

namespace PagosIntermex
{
    class C_LIBERARCREDITO
    {
        private class DOCTOS_CO
        {
            // public DOCTOS_CO(int tipo_poliza_id, DateTime fecha, string descripcion)
            public DOCTOS_CO()
            {
                /* TIPO_POLIZA_ID = tipo_poliza_id;
                FECHA = fecha;
                DESCRIPCION = descripcion; */
            }

            public int TIPO_POLIZA_ID { get; set; }

            public DateTime FECHA { get; set; }

            public string DESCRIPCION { get; set; }
        }

        private class DOCTOS_CO_DET
        {
            // public DOCTOS_CO_DET(int cuenta_id, int depto_co_id, string tipo_asiento, double importe, double importe_mn, string refer, string descripcion, int posicion, DateTime fecha)
            public DOCTOS_CO_DET()
            {
                /* CUENTA_ID = cuenta_id;
                DEPTO_CO_ID = depto_co_id;
                TIPO_ASIENTO = tipo_asiento;
                IMPORTE = importe;
                IMPORTE_MN = importe_mn;
                REFER = refer;
                DESCRIPCION = descripcion;
                POSICION = posicion;
                FECHA = fecha; */
            }

            public int CUENTA_ID { get; set; }

            public int DEPTO_CO_ID { get; set; }

            public string TIPO_ASIENTO { get; set; }

            public double IMPORTE { get; set; }

            public double IMPORTE_MN { get; set; }

            public string REFER { get; set; }

            public string DESCRIPCION { get; set; }

            public int POSICION { get; set; }

            public DateTime FECHA { get; set; }
        }





        private int GEN_DOCTO_ID(C_CONEXIONFIREBIRD conn, FbTransaction transaction)
        {
            int docto_id = 0;

            try
            {
                FbCommand cmd = new FbCommand("EXECUTE PROCEDURE gen_docto_id", conn.FBC, transaction);
                FbDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    docto_id = Convert.ToInt32(read["DOCTO_ID"].ToString());
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

        // funcion para calcular el consecutivo de la tabla tipos_polizas_det, retorna el consecutivo con formato 0000000n
        private string Sig_Folio_Poliza(C_CONEXIONFIREBIRD conn, FbTransaction transaction, int tipo_poliza_id, DateTime fecha)
        {
            string tipoPoliza = "";
            string query = "";
            string query2 = "";

            string mes = fecha.ToString("MM");
            string año = fecha.ToString("yyyy");

            int consecutivo = 0;
            string sigFolio = "";
            int tipo_poliza_det_id = 0;
            int gen_catalogo_id = 0;

            // MesAño(ref mes, ref año, fecha);

            FbCommand fbc;
            FbDataReader fdr;

            //se hace la consulta para ver el tipo de poliza con el id del tipo de poliza
            query = "SELECT tipo_consec from tipos_polizas ";
            query += "WHERE tipo_poliza_id = " + tipo_poliza_id;

            fbc = new FbCommand(query, conn.FBC, transaction);
            fdr = fbc.ExecuteReader();
            while (fdr.Read())
            {
                tipoPoliza = fdr.GetString(0);
            }


            if (tipoPoliza == "M")
            {
                query = "select tpd.consecutivo from tipos_polizas_det tpd ";
                query += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " and tpd.ano = '" + año + "' and tpd.mes = '" + mes + "'";
                FbCommand fbc1 = new FbCommand(query, conn.FBC, transaction);
                fdr = fbc1.ExecuteReader();

                // si es falso significa que el registro en la tabla tipos de polizas det no existe y se inserta un nuevo registro con folio 1
                if (fdr.Read() == false)
                {
                    query = "EXECUTE PROCEDURE gen_catalogo_id";
                    fbc.CommandText = query;
                    fdr = fbc.ExecuteReader();
                    while (fdr.Read())
                        gen_catalogo_id = fdr.GetInt32(0);

                    query = "insert into tipos_polizas_det (tipo_poliza_det_id, tipo_poliza_id,ano,mes,consecutivo)";
                    query += "values (" + gen_catalogo_id + "," + tipo_poliza_id + "," + año + "," + mes + "," + 1 + ")";
                    fbc.CommandText = query;
                    fbc.ExecuteNonQuery();
                    fbc.Dispose();
                    sigFolio = "000000001";



                }
                else //si existe la poliza solo se modifica el consecutivo pero primero sacamos el id de tipo_poliza_det_id
                {
                    query2 = "select tpd.tipo_poliza_det_id from tipos_polizas_det tpd ";
                    query2 += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " and tpd.ano = '" + año + "' and tpd.mes = '" + mes + "'";
                    FbCommand tpd_id = new FbCommand(query2, conn.FBC, transaction);
                    FbDataReader rtpd_id = tpd_id.ExecuteReader();
                    while (rtpd_id.Read())
                        tipo_poliza_det_id = rtpd_id.GetInt32(0);

                    query = "select tpd.consecutivo from tipos_polizas_det tpd ";
                    query += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " and tpd.ano = '" + año + "' and tpd.mes = '" + mes + "'";
                    FbCommand fb2 = new FbCommand(query, conn.FBC, transaction);
                    FbDataReader fdr3 = fb2.ExecuteReader();

                    while (fdr3.Read())
                        consecutivo = fdr.GetInt32(0);

                    consecutivo += 1;

                    query = "update tipos_polizas_det set consecutivo = " + consecutivo + " where tipo_poliza_det_id = " + tipo_poliza_det_id;
                    fb2.CommandText = query;
                    fb2.ExecuteNonQuery();
                    fb2.Dispose();
                    sigFolio = Sig_Folio(consecutivo);


                }


            }

            else
                if (tipoPoliza == "E")
            {
                query = "select tpd.consecutivo from tipos_polizas_det tpd ";
                query += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " and tpd.ano = '" + año + "'";
                FbCommand fbc1 = new FbCommand(query, conn.FBC, transaction);
                fdr = fbc1.ExecuteReader();

                if (fdr.Read() == false)
                {
                    query = "EXECUTE PROCEDURE gen_catalogo_id";
                    fbc.CommandText = query;
                    fdr = fbc.ExecuteReader();
                    while (fdr.Read())
                        gen_catalogo_id = fdr.GetInt32(0);

                    query = "insert into tipos_polizas_det (tipo_poliza_det_id, tipo_poliza_id,ano,mes,consecutivo)";
                    query += "values (" + gen_catalogo_id + "," + tipo_poliza_id + "," + año + "," + 0 + "," + 1 + ")";
                    fbc.CommandText = query;
                    fbc.ExecuteNonQuery();
                    fbc.Dispose();

                    sigFolio = "000000001";



                }
                else
                {
                    query2 = "select tpd.tipo_poliza_det_id from tipos_polizas_det tpd ";
                    query2 += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " and tpd.ano = '" + año + "'";
                    FbCommand tpd_id = new FbCommand(query2, conn.FBC, transaction);
                    FbDataReader rtpd_id = tpd_id.ExecuteReader();
                    while (rtpd_id.Read())
                        tipo_poliza_det_id = rtpd_id.GetInt32(0);


                    query = "select tpd.consecutivo from tipos_polizas_det tpd ";
                    query += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " and tpd.ano = '" + año + "'";
                    FbCommand fb2 = new FbCommand(query, conn.FBC, transaction);
                    FbDataReader fdr3 = fb2.ExecuteReader();

                    while (fdr3.Read())
                        consecutivo = fdr.GetInt32(0);

                    consecutivo += 1;

                    query = "update tipos_polizas_det set consecutivo = " + consecutivo + " where tipo_poliza_det_id = " + tipo_poliza_det_id;
                    fb2.CommandText = query;
                    fb2.ExecuteNonQuery();
                    fb2.Dispose();
                    sigFolio = Sig_Folio(consecutivo);


                }
            }

            else
                if (tipoPoliza == "P")
            {
                query = "select tpd.consecutivo from tipos_polizas_det tpd ";
                query += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " ";
                query += "  and tpd.ano = 0 ";
                query += "  and tpd.mes = 0 ";
                FbCommand fbc1 = new FbCommand(query, conn.FBC, transaction);
                fdr = fbc1.ExecuteReader();

                if (fdr.Read() == false)
                {
                    query = "EXECUTE PROCEDURE gen_catalogo_id";
                    fbc.CommandText = query;
                    fdr = fbc.ExecuteReader();
                    while (fdr.Read())
                        gen_catalogo_id = fdr.GetInt32(0);

                    query = "insert into tipos_polizas_det (tipo_poliza_det_id, tipo_poliza_id,ano,mes,consecutivo)";
                    query += "values (" + gen_catalogo_id + "," + tipo_poliza_id + "," + 0 + "," + 0 + "," + 1 + ")";
                    fbc.CommandText = query;
                    fbc.ExecuteNonQuery();
                    fbc.Dispose();
                    sigFolio = "000000001";
                }
                else
                {
                    query2 = "select tpd.tipo_poliza_det_id from tipos_polizas_det tpd ";
                    query2 += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " ";
                    query2 += "  and tpd.ano = 0 ";
                    query2 += "  and tpd.mes = 0 ";
                    FbCommand tpd_id = new FbCommand(query2, conn.FBC, transaction);
                    FbDataReader rtpd_id = tpd_id.ExecuteReader();
                    while (rtpd_id.Read())
                        tipo_poliza_det_id = rtpd_id.GetInt32(0);


                    query = "select tpd.consecutivo from tipos_polizas_det tpd ";
                    query += "where tpd.tipo_poliza_id = " + tipo_poliza_id + " ";
                    query += "  and tpd.ano = 0 ";
                    query += "  and tpd.mes = 0 ";
                    FbCommand fb2 = new FbCommand(query, conn.FBC, transaction);
                    FbDataReader fdr3 = fb2.ExecuteReader();

                    while (fdr3.Read())
                        consecutivo = fdr.GetInt32(0);

                    consecutivo += 1;

                    query = "update tipos_polizas_det set consecutivo = " + consecutivo + " where tipo_poliza_det_id = " + tipo_poliza_det_id;
                    fb2.CommandText = query;
                    fb2.ExecuteNonQuery();
                    fb2.Dispose();
                    sigFolio = Sig_Folio(consecutivo);


                }
            }

            return sigFolio;

        }

        private string Sig_Folio(int folio)
        {
            string ceros = "";

            int lon = folio.ToString().Length;

            for (int i = lon - 1; i < 8; i++)
            {
                ceros += "0";
            }

            return ceros + folio.ToString();
        }

        private double Get_Tipo_Cambio(C_CONEXIONFIREBIRD conn, FbTransaction transaction, int moneda_id)
        {
            double result = 1;
            string select = "";

            try
            {
                select = "SELECT FIRST 1 tipo_cambio FROM historia_cambiaria ";
                select += "WHERE moneda_id = " + moneda_id + " ";
                select += "ORDER BY fecha DESC";

                FbCommand cmd = new FbCommand(select, conn.FBC, transaction);
                FbDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    result = Convert.ToDouble(read["TIPO_CAMBIO"].ToString());
                }
                read.Close();
                cmd.Dispose();
            }
            catch
            {
                // NO HACER NADA
            }

            return result;
        }

        private bool Delete_Poliza(C_CONEXIONFIREBIRD conn, FbTransaction transaction, int docto_co_id, int docto_ba_id, ref DOCTOS_CO dc, ref DOCTOS_CO_DET[] dcd)
        {
            bool result = false;

            FbCommand cmd;
            FbDataReader read;

            try
            {
                cmd = new FbCommand("SELECT * FROM doctos_co WHERE docto_co_id = " + docto_co_id, conn.FBC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    dc.TIPO_POLIZA_ID = Convert.ToInt32(read["TIPO_POLIZA_ID"].ToString());
                    dc.FECHA = Convert.ToDateTime(read["FECHA"].ToString());
                    dc.DESCRIPCION = read["DESCRIPCION"].ToString();
                }
                read.Close();
                cmd.Dispose();

                int r = 0;

                cmd = new FbCommand("SELECT * FROM doctos_co_det WHERE docto_co_id = " + docto_co_id + " ORDER BY posicion", conn.FBC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    dcd[r] = new DOCTOS_CO_DET();

                    dcd[r].CUENTA_ID = read["CUENTA_ID"].ToString() != "" ? Convert.ToInt32(read["CUENTA_ID"].ToString()) : 0;
                    dcd[r].DEPTO_CO_ID = Convert.ToInt32(read["DEPTO_CO_ID"].ToString());
                    dcd[r].TIPO_ASIENTO = read["TIPO_ASIENTO"].ToString();
                    dcd[r].IMPORTE = Convert.ToDouble(read["IMPORTE"].ToString());
                    dcd[r].IMPORTE_MN = Convert.ToDouble(read["IMPORTE_MN"].ToString());
                    dcd[r].REFER = read["REFER"].ToString();
                    dcd[r].DESCRIPCION = read["DESCRIPCION"].ToString();
                    dcd[r].POSICION = Convert.ToInt32(read["POSICION"].ToString());
                    dcd[r].FECHA = Convert.ToDateTime(read["FECHA"].ToString());

                    r++;
                }
                read.Close();
                cmd.Dispose();

                cmd = new FbCommand("DELETE FROM doctos_co_det WHERE docto_co_id = " + docto_co_id, conn.FBC, transaction);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd = new FbCommand("DELETE FROM doctos_co WHERE docto_co_id = " + docto_co_id, conn.FBC, transaction);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd = new FbCommand("DELETE FROM doctos_entre_sis WHERE docto_dest_id = " + docto_co_id + " AND docto_fte_id = " + docto_ba_id, conn.FBC, transaction);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible eliminar la poliza " + docto_co_id + ".\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        private int Insert_Poliza(C_CONEXIONFIREBIRD conn, FbTransaction transaction, int docto_ba_id, int moneda_id, double tipo_cambio, double importe, DOCTOS_CO dc, DOCTOS_CO_DET[] dcd)
        {
            string insert = "";

            int docto_co_id = 0;
            int docto_co_det_id = 0;

            FbCommand cmd;

            try
            {
                #region INSERT DOCTOS_CO

                docto_co_id = GEN_DOCTO_ID(conn, transaction);

                insert = "INSERT INTO DOCTOS_CO";
                insert += "(";
                insert += "      docto_co_id, ";
                insert += "      tipo_poliza_id, ";
                insert += "      poliza, ";
                insert += "      fecha, ";
                insert += "      moneda_id, ";
                insert += "      tipo_cambio, ";
                insert += "      estatus, ";
                insert += "      cancelado, ";
                insert += "      aplicado, ";
                insert += "      ajuste, ";
                insert += "      integ_co, ";
                insert += "      descripcion, ";
                insert += "      forma_emitida, ";
                insert += "      sistema_origen, ";
                insert += "      integ_ba, ";
                insert += "      usuario_creador, ";
                insert += "      fecha_hora_creacion, ";
                insert += "      usuario_ult_modif, ";
                insert += "      fecha_hora_ult_modif ";
                insert += ") ";
                insert += "VALUES";
                insert += "(";
                insert += "      " + docto_co_id + ", "; //**************************************** docto_co_id
                insert += "      " + dc.TIPO_POLIZA_ID + ", "; //********************************** tipo_poliza_id
                insert += "      '-', "; //******************************************************** poliza
                insert += "      '" + dc.FECHA.ToString("dd.MM.yyyy") + "', "; //****************** fecha
                insert += "      " + moneda_id + ", "; //****************************************** moneda_id
                insert += "      " + tipo_cambio + ", "; //**************************************** tipo_cambio
                insert += "      'R', "; //******************************************************** estatus
                insert += "      'N', "; //******************************************************** cancelado
                insert += "      'N', "; //******************************************************** aplicado
                insert += "      'N', "; //******************************************************** ajuste
                insert += "      'N', "; //******************************************************** integ_co
                insert += "      '" + dc.DESCRIPCION + "', "; //*********************************** descripcion
                insert += "      'N', "; //******************************************************** forma_emitida
                insert += "      'BA', "; //******************************************************* sistema_origen
                insert += "      'N', "; //******************************************************** integ_ba
                insert += "      'SYSDBA', "; //*************************************************** usuario_creador
                insert += "      '" + DateTime.Now.ToString("dd.MM.yyyy") + "', "; //************** fecha_hora_creacion
                insert += "      'SYSDBA', "; //*************************************************** usuario_ult_modif
                insert += "      '" + DateTime.Now.ToString("dd.MM.yyyy") + "' "; //*************** fecha_hora_ult_modif
                insert += ")";

                cmd = new FbCommand(insert, conn.FBC, transaction);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                #endregion

                for (int r = 0; r < dcd.Length; r++)
                {
                    #region INSERT DOCTOS_CO_DET

                    docto_co_det_id = GEN_DOCTO_ID(conn, transaction);

                    insert = "INSERT INTO DOCTOS_CO_DET";
                    insert += "(";
                    insert += "      docto_co_det_id, ";
                    insert += "      docto_co_id, ";
                    insert += "      cuenta_id, ";
                    insert += "      depto_co_id, ";
                    insert += "      tipo_asiento, ";
                    insert += "      importe, ";
                    insert += "      importe_mn, ";
                    insert += "      refer, ";
                    insert += "      descripcion, ";
                    insert += "      posicion, ";
                    insert += "      fecha, ";
                    insert += "      cancelado, ";
                    insert += "      aplicado, ";
                    insert += "      ajuste, ";
                    insert += "      moneda_id ";
                    insert += ")";
                    insert += "VALUES";
                    insert += "(";
                    insert += "      " + docto_co_det_id + ", "; //******************************** docto_co_det_id
                    insert += "      " + docto_co_id + ", "; //************************************ docto_co_id
                    if (dcd[r].CUENTA_ID != 0)
                    {
                        insert += "  " + dcd[r].CUENTA_ID + ", "; //******************************* cuenta_id
                    }
                    else
                    {
                        insert += "  null, "; //*************************************************** cuenta_id
                    }
                    insert += "      " + dcd[r].DEPTO_CO_ID + ", "; //***************************** depto_co_id
                    insert += "      '" + dcd[r].TIPO_ASIENTO + "', "; //************************** tipo_asiento
                    insert += "      " + importe + ", "; //**************************************** importe
                    insert += "      " + importe + ", "; //**************************************** importe_mn
                    insert += "      '" + dcd[r].REFER + "', "; //********************************* refer
                    insert += "      '" + dcd[r].DESCRIPCION + "', "; //*************************** descripcion
                    insert += "      " + dcd[r].POSICION + ", "; //******************************** posicion
                    insert += "      '" + dcd[r].FECHA.ToString("dd.MM.yyyy") + "', "; //********** fecha
                    insert += "      'N', "; //**************************************************** cancelado
                    insert += "      'N', "; //**************************************************** aplicado
                    insert += "      'N', "; //**************************************************** ajuste
                    insert += "      " + moneda_id + " "; //*************************************** moneda_id
                    insert += ")";

                    cmd = new FbCommand(insert, conn.FBC, transaction);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    #endregion
                }

                #region INSERT DOCTOS_ENTRE_SIS

                insert = "INSERT INTO DOCTOS_ENTRE_SIS";
                insert += "(";
                insert += "      docto_dest_id, ";
                insert += "      clave_sis_dest, ";
                insert += "      clave_sis_fte, ";
                insert += "      docto_fte_id, ";
                insert += "      tipo_docto ";
                insert += ") ";
                insert += "VALUES";
                insert += "(";
                insert += "      " + docto_co_id + ", "; //********** docto_dest_id
                insert += "      'CO', "; //************************* clave_sis_dest
                insert += "      'BA', "; //************************* clave_sis_fte
                insert += "      " + docto_ba_id + ", "; //********** docto_fte_id
                insert += "      'P' "; //*************************** tipo_docto
                insert += ")";

                cmd = new FbCommand(insert, conn.FBC, transaction);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible volver a crear la poliza.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                docto_co_id = 0;
            }

            return docto_co_id;
        }





        public bool Liberar(C_CONEXIONFIREBIRD conn, FbTransaction transaction, int docto_fte_id, double importe, int cuenta_ban_id, int cuenta_moneda_id, DateTime fecha_aplicacion, int concepto_ba_id, string cuenta_ban_tercero, string banco_tercero, string referencia)
        {
            FbCommand fbc;
            FbDataReader fdr;

            string query = "";
            string integ_co = "N";
            string poliza = "";

            int docto_ba_id = 0;
            int docto_co_id = 0;
            int tipo_poliza_id = 0;
            int docto_co_moneda_id = 0;
            int docto_ba_info_ban_id = 0;
            int doctos_co_det_info_ban = 0;
            int cuenta_id = 0;
            int beneficiario_id = 0;
            int docto_co_det_id = 0;

            double importe_mn = 0;
            double tipo_cambio = 1;



            #region Doctos_entre_sis - verificamos si hay una relacion entre el modulo de CP y BA en caso de que si haya se retorna el docto_fte_id si no se retorna 0

            query = "SELECT docto_dest_id FROM doctos_entre_sis ";
            query += "WHERE docto_fte_id = " + docto_fte_id + " ";
            query += "  AND clave_sis_dest = 'BA' ";
            query += "  AND clave_sis_fte = 'CP' ";

            fbc = new FbCommand(query, conn.FBC, transaction);
            fdr = fbc.ExecuteReader();
            while (fdr.Read())
            {
                docto_ba_id = Convert.ToInt32(fdr["DOCTO_DEST_ID"].ToString());
            }
            fdr.Close();
            fbc.Dispose();

            #endregion



            if (docto_ba_id != 0)
            {
                #region Get_Poliza_movto_id - verifica que exista una poliza

                query = "SELECT ";
                query += "      gpmb.*, ";
                query += "      dc.moneda_id ";
                query += " FROM get_poliza_movto_ba(" + docto_ba_id + ") gpmb ";
                query += " JOIN doctos_co dc ON(gpmb.docto_co_id = dc.docto_co_id) ";

                fbc = new FbCommand(query, conn.FBC, transaction);
                fdr = fbc.ExecuteReader();
                while (fdr.Read())
                {
                    docto_co_id = Convert.ToInt32(fdr["DOCTO_CO_ID"].ToString());
                    tipo_poliza_id = Convert.ToInt32(fdr["TIPO_POLIZA_ID"].ToString());
                    docto_co_moneda_id = Convert.ToInt32(fdr["MONEDA_ID"].ToString());

                    integ_co = "S";
                }
                fbc.Dispose();
                fdr.Close();

                #endregion

                DOCTOS_CO dc = new DOCTOS_CO();
                DOCTOS_CO_DET[] dcd = new DOCTOS_CO_DET[2];

                // SI LA MONEDA DE LA CUENTA ES DIFERENTE A LA DE LA POLIZA NECESITAMOS EL TIPO DE CAMBIO PARA HACER LA CONVERSIÓN A LA MONEDA DE LA CUENTA BANCARIA
                if (cuenta_moneda_id != docto_co_moneda_id)
                {
                    tipo_cambio = Get_Tipo_Cambio(conn, transaction, docto_co_moneda_id);
                    importe_mn = importe * tipo_cambio;

                    if (Delete_Poliza(conn, transaction, docto_co_id, docto_ba_id, ref dc, ref dcd))
                    {
                        docto_co_id = 0;
                    }
                    else
                    {
                        return false;
                    }
                }

                // AHORA OBTENEMOS EL TIPO DE CAMBIO DE LA MONEDA DE LA CUENTA BANCARIA
                tipo_cambio = Get_Tipo_Cambio(conn, transaction, cuenta_moneda_id);

                // PRIMERO SE DEBE ACTUALIZAR EL CONCEPTO Y EL MOVIMIENTO FISCAL
                #region ACTUALIZAR EN DOCTOS_BA

                query = "UPDATE doctos_ba SET ";
                query += "      concepto_ba_id = " + concepto_ba_id + ", ";
                query += "      cuenta_ban_id = " + cuenta_ban_id + ", ";
                query += "      estatus = 'N', ";
                query += "      tipo_mov_fiscal = 'T', ";
                query += "      refer = '" + referencia + "' ";
                if (cuenta_moneda_id != docto_co_moneda_id)
                {
                    query += "  , ";
                    query += "  importe = " + importe_mn + ", ";
                    query += "  moneda_id = " + cuenta_moneda_id + ", ";
                    query += "  tipo_cambio = " + tipo_cambio + " ";
                }
                query += "WHERE docto_ba_id = " + docto_ba_id;

                fbc = new FbCommand(query, conn.FBC, transaction);
                fbc.ExecuteNonQuery();
                fbc.Dispose();

                #endregion

                // INSERTAMOS UN RENGLON CON EL IMPORTE, CUENTA_BAN_TERCERO, BAN_TERCERO, REFERENCIA
                #region INSERTAR EN DOCTOS_BA_INFO_BAN

                docto_ba_info_ban_id = GEN_DOCTO_ID(conn, transaction);

                query = "INSERT INTO doctos_ba_info_ban";
                query += "( ";
                query += "      docto_ba_info_ban_id,  ";
                query += "      docto_ba_id, ";
                query += "      metodo_pago, ";
                query += "      importe, ";
                query += "      cuenta_ban_tercero, ";
                query += "      banco_tercero, ";
                query += "      refer ";
                query += ") ";
                query += "VALUES";
                query += "( ";
                query += "      " + docto_ba_info_ban_id + ", "; //******************** docto_ba_info_ban_id
                query += "      " + docto_ba_id + ", "; //********************* docto_ba_id
                query += "      null, "; //************************************ metodo_pago
                query += "      " + importe + ", "; //************************* importe
                query += "      '" + cuenta_ban_tercero + "', "; //************ cuenta_ban_tercero
                query += "      '" + banco_tercero + "', "; //***************** banco_tercero
                query += "      '" + referencia + "' "; //********************* refer
                query += ")";

                fbc = new FbCommand(query, conn.FBC, transaction);
                fbc.ExecuteNonQuery();
                fbc.Dispose();

                #endregion

                // VOLVEMOS A CREAR LA POLIZA ELIMINADA PERO AHORA CON EL IMPORTE EN MONEDA NACIONAL
                if (cuenta_moneda_id != docto_co_moneda_id)
                {
                    docto_co_id = Insert_Poliza(conn, transaction, docto_ba_id, cuenta_moneda_id, tipo_cambio, importe_mn, dc, dcd);
                }

                // Si el tipo_poliza_id vale 0 significa que no existe una poliza
                if (docto_co_id != 0)
                {
                    #region ACTUALIZAR POLIZA

                    // OBTENEMOS EL ID DE LA CUENTA QUE SE ASIGNARA A LA POLIZA
                    query = "SELECT cc.cuenta_id FROM cuentas_bancarias cb ";
                    query += " JOIN cuentas_co cc ON(cb.cuenta_contable = cc.cuenta_pt) ";
                    query += "WHERE cb.cuenta_ban_id = " + cuenta_ban_id;

                    fbc = new FbCommand(query, conn.FBC, transaction);
                    fdr = fbc.ExecuteReader();
                    while (fdr.Read())
                    {
                        cuenta_id = fdr.GetInt32(0);
                    }
                    fdr.Close();
                    fbc.Dispose();

                    if (cuenta_id != 0)
                    {
                        // sacamos el beneficiario de la tabla de doctos_ba
                        query = "SELECT beneficiario_id FROM doctos_ba WHERE docto_ba_id = " + docto_ba_id;
                        fbc = new FbCommand(query, conn.FBC, transaction);
                        fdr = fbc.ExecuteReader();
                        while (fdr.Read())
                        {
                            beneficiario_id = fdr.GetInt32(0);
                        }
                        fbc.Dispose();
                        fdr.Close();

                        // se saca el docto_co_det_id de la tabla doctos_co_det
                        query = "SELECT docto_co_det_id FROM doctos_co_det WHERE docto_co_id = " + docto_co_id + " AND cuenta_id IS NULL";
                        fbc = new FbCommand(query, conn.FBC, transaction);
                        fdr = fbc.ExecuteReader();
                        while (fdr.Read())
                        {
                            docto_co_det_id = fdr.GetInt32(0);
                        }
                        fbc.Dispose();
                        fdr.Close();

                        // insertamos en la tabla doctos_co_det_info_ban
                        doctos_co_det_info_ban = GEN_DOCTO_ID(conn, transaction);

                        query = "INSERT INTO doctos_co_det_info_ban";
                        query += "( ";
                        query += "      docto_co_det_info_ban_id, ";
                        query += "      docto_co_det_id, ";
                        query += "      tipo_mov_fiscal, ";
                        query += "      importe, ";
                        query += "      beneficiario_id, ";
                        query += "      cuenta_ban_id, ";
                        query += "      cuenta_ban_tercero, ";
                        query += "      banco_tercero, ";
                        query += "      refer ";
                        query += ") ";
                        query += "VALUES";
                        query += "( ";
                        query += "      " + doctos_co_det_info_ban + ", "; //** docto_co_det_info_ban_id
                        query += "      " + docto_co_det_id + ", "; //********* docto_co_det_id
                        query += "      'T', "; //***************************** tipo_mov_fiscal
                        query += "      " + importe + ", "; //***************** importe
                        query += "      " + beneficiario_id + ", "; //********* beneficiario_id
                        query += "      " + cuenta_ban_id + ", "; //*********** cuenta_ban_id
                        query += "      '" + cuenta_ban_tercero + "', "; //**** cuenta_ban_tercero
                        query += "      '" + banco_tercero + "', "; //********* banco_tercero
                        query += "      '" + referencia + "' "; //************* refer
                        query += ")";

                        fbc = new FbCommand(query, conn.FBC, transaction);
                        fbc.ExecuteNonQuery();
                        fbc.Dispose();


                        // actualizamos la tabla doctos_co_det
                        query = "UPDATE doctos_co_det SET ";
                        query += "      cuenta_id = " + cuenta_id + " ";
                        query += "WHERE docto_co_det_id = " + docto_co_det_id;
                        fbc = new FbCommand(query, conn.FBC, transaction);
                        fbc.ExecuteNonQuery();
                        fbc.Dispose();


                        // en doctos_co se pone la poliza con el metodo sigFolio, la fecha de aplicado y el estatus cambia a P
                        poliza = Sig_Folio_Poliza(conn, transaction, tipo_poliza_id, fecha_aplicacion);

                        query = "UPDATE doctos_co SET ";
                        query += "      poliza = '" + poliza + "', ";
                        query += "      fecha = '" + fecha_aplicacion.ToString("dd.MM.yyyy") + "', ";
                        query += "      estatus = 'P', ";
                        query += "      integ_co = 'S' ";
                        query += "WHERE docto_co_id = " + docto_co_id;

                        fbc = new FbCommand(query, conn.FBC, transaction);
                        fbc.ExecuteNonQuery();
                        fbc.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("No se encontro el identificador de la cuenta contable de la cuenta bancaria.", "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    #endregion
                }

                // ACTUALIZAMOS EL ENCABEZADO DEL RETIRO PARA APLICARLO
                query = "UPDATE doctos_ba SET ";
                query += "      aplicado = 'S', ";
                query += "      integ_co = '" + integ_co + "' ";
                query += "WHERE docto_ba_id = " + docto_ba_id;
                fbc = new FbCommand(query, conn.FBC, transaction);
                fbc.ExecuteNonQuery();
                fbc.Dispose();

                return true;
            }
            else
            {
                MessageBox.Show("Hay un Error localizando el id de Doctos_ba", "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool Liberar_Detalle(C_ConexionSQL conn, SqlTransaction transaction, int docto_pr_det_id, string folio_credito)
        {
            string update = "";
            string select = "";

            SqlCommand cmd;
            SqlDataReader read;

            try
            {
                // ACTUALIZAMOS EL RENGLON A LIBERADO
                update = "UPDATE p_doctos_pr_det SET ";
                update += "      estatus = 'L' ";
                update += "WHERE docto_pr_det_id = " + docto_pr_det_id + " ";

                cmd = new SqlCommand(update, conn.SC, transaction);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                /*/ OBTENEMOS Y GUARDAMOS EL FOLIO DEL CARGO LIBERADO PARA POSTERIORMENTE LIBERARLO EN EL PORTAL
                select = "SELECT * FROM p_doctos_pr_det ";
                select += "WHERE docto_pr_det_id = " + docto_pr_det_id + " ";

                cmd = new SqlCommand(select, conn.SC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    Array.Resize(ref liberados, liberados.Length + 1);

                    liberados[liberados.Length - 1] = new Services.RegistrosLiberados();
                    liberados[liberados.Length - 1].Cargos = read["FOLIO_MICROSIP"].ToString();
                }
                read.Close();
                cmd.Dispose();*/

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible liberar el pago '" + folio_credito + "'.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        public bool Liberar_Detalle(C_ConexionSQL conn, SqlTransaction transaction, string folio_credito, string folio_programacion)
        {
            string update = "";
            string select = "";

            SqlCommand cmd;
            SqlDataReader read;

            try
            {
                // ACTUALIZAMOS EL RENGLON A LIBERADO
                update = "UPDATE p_doctos_pr_det SET ";
                update += "      estatus = 'L' ";
                update += "WHERE folio_credito = '" + folio_credito + "' ";
                update += "  AND docto_pr_id = (SELECT docto_pr_id FROM doctos_pr WHERE folio = '" + folio_programacion + "')";

                cmd = new SqlCommand(update, conn.SC, transaction);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                /*/ OBTENEMOS Y GUARDAMOS EL FOLIO DEL CARGO LIBERADO PARA POSTERIORMENTE LIBERARLO EN EL PORTAL
                select = "SELECT * FROM p_doctos_pr_det ";
                select += "WHERE folio_credito = '" + folio_credito + "' ";
                select += "  AND docto_pr_id = (SELECT docto_pr_id FROM doctos_pr WHERE folio = '" + folio_programacion + "')";

                cmd = new SqlCommand(select, conn.SC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    Array.Resize(ref liberados, liberados.Length + 1);

                    liberados[liberados.Length - 1] = new Services.RegistrosLiberados();
                    liberados[liberados.Length - 1].Cargos = read["FOLIO_MICROSIP"].ToString();
                }
                read.Close();
                cmd.Dispose();*/

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible liberar el pago '" + folio_credito + "'.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        public bool verifica_programacion_liberada(C_ConexionSQL conn, SqlTransaction transaction, string folio_programacion)
        {
            int registros_pendientes = 0;

            string select = "";

            try
            {
                select = "SELECT ";
                select += "      COUNT(*) as COUNT";
                select += " FROM p_doctos_pr_det dpd ";
                select += " JOIN p_doctos_pr dp ON(dpd.docto_pr_id = dp.docto_pr_id) ";
                select += "WHERE dp.folio = '" + folio_programacion + "' ";
                select += "  AND (dpd.estatus = 'A' OR dpd.estatus = 'B') ";

                SqlCommand cmd = new SqlCommand(select, conn.SC, transaction);
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    registros_pendientes = Convert.ToInt32(read["COUNT"].ToString());
                }
                read.Close();
                cmd.Dispose();

                return registros_pendientes == 0 ? true : false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error al verificar si la programación '" + folio_programacion + "' tiene pagos pendientes por liberar.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
