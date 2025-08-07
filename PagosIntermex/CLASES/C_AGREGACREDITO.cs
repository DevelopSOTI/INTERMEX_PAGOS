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
    class C_AGREGACREDITO
    {
        C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();

        public C_AGREGACREDITO()
        {
            registros.LeerRegistros(false);
        }

        public int DOCTO_PR_DET_ID { get; set; }
        public int DOCTO_PR_ID { get; set; }
        public string FOLIO_MICROSIP { get; set; }
        public DateTime FECHA_CARGO { get; set; }
        public int PROVEEDOR_ID { get; set; }
        public string PROVEEDOR_CLAVE { get; set; }
        public string PROVEEDOR_NOMBRE { get; set; }
        public DateTime FECHA_VENCIMIENTO { get; set; }
        public double IMPORTE_PAGOS { get; set; }
        public double IMPORTE_AUTORIZADO { get; set; }
        public string TIPO { get; set; }
        public string ESTATUS { get; set; }
        public string EMPRESA { get; set; }
        public string FOLIO_CREDITO { get; set; }

        public string IVA { get; set; }
        public string ESTATUS_DET { get; set; }

        public string REQUISICION_ID { get; set; }

        private class Proveedor
        {
            public Proveedor()
            {

            }

            public string NOMBRE { get; set; }

            public string RFC_CURP { get; set; }

            public string CUENTA { get; set; }

            public string CLABE { get; set; }

            public string BANCO { get; set; }

            public string CLAVE_FISCAL { get; set; }

            public string PAIS_BANCO { get; set; }
            public string NO_SUCURSAL { get; set; }

            public string ABA_BIC { get; set; }
            public string CONCEPTO_CIE { get; set; }
            public string CONVENIO_CIE { get; set; }
            public string BANCO_DIRECCION { get; set; }

            public string PAIS_PROV { get; set; }
            public string DIRECCION_PROV { get; set; }
            public string TELEFONO_PROV { get; set; }
        }





        public string Sig_Folio_CP(string serie, string folio)
        {
            if (serie == "@")
            {
                serie = "";
            }

            string ceros = "";
            int folio_original;

            folio_original = Convert.ToInt32(folio);

            int folio_sig = folio_original + 1;




            // int lon = folio_original.ToString().Length;
            int lon = serie.Length + folio_original.ToString().Length;

            for (int i = lon - 1; i < 8; i++)
            {
                ceros += "0";
            }

            // return ceros + folio_sig.ToString();
            return serie + ceros + folio_sig.ToString();
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





        private int OBTENER_CONCEPTO_CP_ID(C_CONEXIONFIREBIRD conn, FbTransaction transaction, string concepto)
        {
            int concepto_cp_id = 0;

            try
            {
                FbCommand cmd = new FbCommand("SELECT * FROM conceptos_cp WHERE nombre = '" + concepto + "' AND naturaleza = 'R'", conn.FBC, transaction);
                FbDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    concepto_cp_id = Convert.ToInt32(read["CONCEPTO_CP_ID"].ToString());
                }
                read.Close();
                cmd.Dispose();
            }
            catch
            {
                concepto_cp_id = 0;
            }

            return concepto_cp_id;
        }

        private int OBTENER_DOCTO_CP_ID(C_CONEXIONFIREBIRD conn, FbTransaction transaction, string folio, int proveedor_id, ref string descripcion)
        {
            int docto_cp_id = 0;

            string select = "";

            try
            {
                select = "SELECT ";
                select += "      * ";
                select += " FROM doctos_cp ";
                select += "WHERE folio = '" + folio + "' ";
                select += "  AND proveedor_id = " + proveedor_id + " ";
                select += "  AND naturaleza_concepto = 'C' ";

                FbCommand cmd = new FbCommand(select, conn.FBC, transaction);
                FbDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    docto_cp_id = Convert.ToInt32(read["DOCTO_CP_ID"].ToString());
                    descripcion = read["DESCRIPCION"].ToString();
                }
                read.Close();
                cmd.Dispose();
            }
            catch
            {
                docto_cp_id = 0;
            }

            return docto_cp_id;
        }

        private bool OBTENER_PROVEEDOR(C_CONEXIONFIREBIRD conn, FbTransaction transaction, int proveedor_id, ref Proveedor proveedor)
        {
            proveedor = new Proveedor();

            FbCommand cmd;
            FbDataReader read;

            bool result = false;

            string select = "";

            try
            {
                select = "SELECT ";
                select += "      p.nombre, ";
                select += "      p.rfc_curp, ";
                select += "      l.cuenta, ";
                select += "      l.clabe, ";
                select += "      l.banco, ";
                select += "      l.PAIS_BANCO, ";
                select += "      l.NO_SUCURSAL, ";
                select += "      l.ABA_BIC, ";
                select += "      l.CONCEPTO_CIE, ";
                select += "      l.CONVENIO_CIE, ";
                select += "      l.BANCO_DIRECCION, ";
                select += "      m.clave_fiscal, ";
                select += "      pais.NOMBRE as PAIS,";
                select += "      p.CALLE, ";
                select += "      p.TELEFONO1 ";
                select += " FROM proveedores p ";
                select += " JOIN libres_proveedor l ON(p.proveedor_id = l.proveedor_id) ";
                select += " JOIN monedas m ON(p.moneda_id = m.moneda_id) ";
                select += " JOIN paises pais ON(p.PAIS_ID = p.PAIS_ID) ";
                select += "WHERE p.proveedor_id = " + proveedor_id;

                cmd = new FbCommand(select, conn.FBC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    proveedor.NOMBRE = read["NOMBRE"].ToString();
                    proveedor.RFC_CURP = read["RFC_CURP"].ToString();
                    proveedor.CUENTA = read["CUENTA"].ToString();
                    proveedor.CLABE = read["CLABE"].ToString();
                    proveedor.BANCO = read["BANCO"].ToString();
                    proveedor.CLAVE_FISCAL = read["CLAVE_FISCAL"].ToString();

                    proveedor.PAIS_BANCO = Convert.ToString(read["PAIS_BANCO"]);
                    proveedor.NO_SUCURSAL = Convert.ToString(read["NO_SUCURSAL"]);

                    proveedor.ABA_BIC = Convert.ToString(read["ABA_BIC"]);
                    proveedor.CONCEPTO_CIE = Convert.ToString(read["CONCEPTO_CIE"]);
                    proveedor.CONVENIO_CIE = Convert.ToString(read["CONVENIO_CIE"]);
                    proveedor.BANCO_DIRECCION = Convert.ToString(read["BANCO_DIRECCION"]);

                    proveedor.PAIS_PROV = Convert.ToString(read["PAIS"]);
                    proveedor.DIRECCION_PROV = Convert.ToString(read["CALLE"]);

                    if (string.IsNullOrEmpty(Convert.ToString(read["TELEFONO1"])))
                        proveedor.TELEFONO_PROV = "";
                    else
                        proveedor.TELEFONO_PROV = Convert.ToString(read["TELEFONO1"]);
                }

                read.Close();
                cmd.Dispose();

                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error al buscar los datos del proveedor.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        private string OBTENER_RFC_EMPRESA(C_CONEXIONFIREBIRD conn, FbTransaction transaction)
        {
            FbCommand cmd;
            FbDataReader read;

            string result = "";

            try
            {
                cmd = new FbCommand("SELECT * FROM registry WHERE nombre = 'Rfc'", conn.FBC, transaction);
                read = cmd.ExecuteReader();
                while (read.Read())
                {
                    result = read["VALOR"].ToString();
                }
                read.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error al buscar el RFC de la empresa.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }





        private string GET_CLAVE_BANCO(string banco)
        {
            string clave_fiscal = "";

            switch (banco)
            {
                case "ABC CAPITAL": clave_fiscal = "138"; break;
                case "ACCIVAL": clave_fiscal = "614"; break;
                case "ACTINVER": clave_fiscal = "133"; break;
                case "AFIRME": clave_fiscal = "062"; break;
                case "AKALA": clave_fiscal = "638"; break;
                case "AMERICAN EXPRESS": clave_fiscal = "103"; break;
                case "ASEA": clave_fiscal = "652"; break;
                case "AUTOFIN": clave_fiscal = "128"; break;
                case "AZTECA": clave_fiscal = "127"; break;
                case "B&B": clave_fiscal = "610"; break;
                case "BAJIO": clave_fiscal = "030"; break;
                case "BAMSA": clave_fiscal = "106"; break;
                case "BANAMEX": clave_fiscal = "002"; break;
                case "BANCO FAMSA": clave_fiscal = "131"; break;
                case "BANCOMEXT": clave_fiscal = "006"; break;
                case "BANCOPPEL": clave_fiscal = "137"; break;
                case "BANJERCITO": clave_fiscal = "019"; break;
                case "BANOBRAS": clave_fiscal = "009"; break;
                case "BANORTE": clave_fiscal = "072"; break;
                case "BANREGIO": clave_fiscal = "058"; break;
                case "BANSEFI": clave_fiscal = "166"; break;
                case "BANSI": clave_fiscal = "160"; break;
                case "BARCLAYS": clave_fiscal = "129"; break;
                case "BBASE": clave_fiscal = "145"; break;
                case "BBVA": clave_fiscal = "012"; break;
                case "BBVA BANCOMER": clave_fiscal = "012"; break;
                case "BANCOMER": clave_fiscal = "012"; break;
                case "BMONEX": clave_fiscal = "112"; break;
                case "BMULTIVA": clave_fiscal = "132"; break;
                case "BULLTICK": clave_fiscal = "632"; break;
                case "CB ACTINVER": clave_fiscal = "621"; break;
                case "CB INTERCAM": clave_fiscal = "630"; break;
                case "CB JPMORGAN": clave_fiscal = "640"; break;
                case "CBDEUTSCHE": clave_fiscal = "626"; break;
                case "CI BOLSA": clave_fiscal = "631"; break;
                case "CIBANCO": clave_fiscal = "143"; break;
                case "CLS": clave_fiscal = "901"; break;
                case "COMPARTAMOS": clave_fiscal = "130"; break;
                case "CONSUBANCO": clave_fiscal = "140"; break;
                case "CREDIT SUISSE": clave_fiscal = "126"; break;
                case "DEUTSCHE": clave_fiscal = "124"; break;
                case "ESTRUCTURADORES": clave_fiscal = "606"; break;
                case "EVERCORE": clave_fiscal = "648"; break;
                case "FINAMEX": clave_fiscal = "616"; break;
                case "FINCOMUN": clave_fiscal = "634"; break;
                case "GBM": clave_fiscal = "601"; break;
                case "HDI SEGUROS": clave_fiscal = "636"; break;
                case "HIPOTECARIA FEDERAL": clave_fiscal = "168"; break;
                case "HSBC": clave_fiscal = "021"; break;
                case "INBURSA": clave_fiscal = "036"; break;
                case "INDEVAL": clave_fiscal = "902"; break;
                case "ING": clave_fiscal = "116"; break;
                case "INTERACCIONES": clave_fiscal = "037"; break;
                case "INTERBANCO": clave_fiscal = "136"; break;
                case "INVEX": clave_fiscal = "059"; break;
                case "IXE": clave_fiscal = "032"; break;
                case "JP MORGAN": clave_fiscal = "110"; break;
                case "KUSPIT": clave_fiscal = "653"; break;
                case "LIBERTAD": clave_fiscal = "670"; break;
                case "MAPFRE": clave_fiscal = "619"; break;
                case "MASARI": clave_fiscal = "602"; break;
                case "MERRILL LYNCH": clave_fiscal = "615"; break;
                case "MIFEL": clave_fiscal = "042"; break;
                case "MONEXCB": clave_fiscal = "600"; break;
                case "NAFIN": clave_fiscal = "135"; break;
                case "OACTIN": clave_fiscal = "622"; break;
                case "OPCIONES EMPRESARIALES DEL NOROESTE": clave_fiscal = "659"; break;
                case "ORDER": clave_fiscal = "637"; break;
                case "PROFUTURO": clave_fiscal = "620"; break;
                case "REFORMA": clave_fiscal = "642"; break;
                case "SANTANDER": clave_fiscal = "014"; break;
                case "SCOTIABANK": clave_fiscal = "044"; break;
                case "SEGMTY": clave_fiscal = "651"; break;
                case "SKANDIA": clave_fiscal = "623"; break;
                // case "SKANDIA": clave_fiscal = "649"; break;
                case "SOFIEXPRESS": clave_fiscal = "655"; break;
                case "STERLING": clave_fiscal = "633"; break;
                case "STP": clave_fiscal = "646"; break;
                case "SU CASITA": clave_fiscal = "629"; break;
                case "TELECOMM": clave_fiscal = "647"; break;
                case "THE ROYAL BANK": clave_fiscal = "102"; break;
                case "TIBER": clave_fiscal = "607"; break;
                case "TOKYO": clave_fiscal = "108"; break;
                case "UBS BANK": clave_fiscal = "639"; break;
                case "UNAGRA": clave_fiscal = "656"; break;
                case "UNICA": clave_fiscal = "618"; break;
                case "VALMEX": clave_fiscal = "617"; break;
                case "VALUE": clave_fiscal = "605"; break;
                case "VE POR MAS": clave_fiscal = "113"; break;
                case "VECTOR": clave_fiscal = "608"; break;
                case "VOLKSWAGEN": clave_fiscal = "141"; break;
                case "WAL-MART": clave_fiscal = "134"; break;
                case "ZURICH": clave_fiscal = "627"; break;
                case "ZURICHVI": clave_fiscal = "628"; break;
                case "BANCO EXTRANJERO": clave_fiscal = "998"; break;
                case "N/A": clave_fiscal = "999"; break;
            }

            return clave_fiscal;
        }





        // public bool NuevoCredito(C_CONEXIONFIREBIRD conn, FbTransaction transaction, int concepto_cp_id, int proveedor_id, DateTime fecha, double importe, string tipo, int docto_cp_acr_id, string descripcion, string usuario_creador, ref string folio)
        public bool NuevoCredito(C_CONEXIONFIREBIRD conn, FbTransaction transaction, int concepto_cp_id, int proveedor_id, DateTime fecha, double importe, string tipo, int docto_cp_acr_id, string descripcion, string usuario_creador, ref int docto_id_cp, ref string folio)
        {
            FbCommand fbc;
            FbDataReader fdr;

            string insert = "";
            string sucursal = "";

            // int docto_id_cp = 0;
            docto_id_cp = 0;
            int docto_id_cp_det = 0;

            // SE VALIDA QUE LOS DATOS LLEGUEN CORRECTAMENTE
            // if (concepto_cp_id != 0 && proveedor_id != 0 && importe != 0 && docto_cp_acr_id != 0)
            if (concepto_cp_id != 0 && proveedor_id != 0 && importe != 0)
            {
                string select_serie = "SELECT FIRST 1 SERIE FROM FOLIOS_CONCEPTOS WHERE sistema = 'CP' AND concepto_id = " + concepto_cp_id + " ORDER BY folio_concepto_id DESC";

                #region OBTENEMOS EL NUEVO FOLIO SEGUN EL CONCEPTO

                try
                {
                    #region 2019

                    // SE EJECUTA EL PROCEDURE PARA OBTENER EL FOLIO
                    /* fbc = new FbCommand("EXECUTE PROCEDURE get_sigfol_cp(" + concepto_cp_id + ", '*')", conn.FBC, transaction);
                    fdr = fbc.ExecuteReader();
                    while (fdr.Read())
                    {
                        folio = Sig_Folio_CP(fdr.GetString(0));
                    }
                    fdr.Close();
                    fbc.Dispose();

                    // SE EJECUTA EL PROCEDURE PARA ACTUALIZAR EL FOLIO
                    fbc = new FbCommand("EXECUTE PROCEDURE get_sigfol_cp(" + concepto_cp_id + ", " + folio + " )", conn.FBC, transaction);
                    fbc.ExecuteNonQuery();
                    fbc.Dispose(); // */

                    #endregion

                    // SE EJECUTA EL PROCEDURE PARA OBTENER EL FOLIO
                    // fbc = new FbCommand("EXECUTE PROCEDURE get_sigfol_concepto('CP', " + concepto_cp_id + ", '@', 0)", conn.FBC, transaction);
                    fbc = new FbCommand("EXECUTE PROCEDURE get_sigfol_concepto('CP', " + concepto_cp_id + ", (" + select_serie + "), 0)", conn.FBC, transaction);
                    fdr = fbc.ExecuteReader();
                    while (fdr.Read())
                    {
                        // folio = Sig_Folio_CP(fdr.GetString(0));
                        folio = fdr.GetString(0);
                    }
                    fdr.Close();
                    fbc.Dispose();

                    // OBTENEMOS LA SERIE
                    string serie = "";

                    fbc = new FbCommand(select_serie, conn.FBC, transaction);
                    fdr = fbc.ExecuteReader();
                    while (fdr.Read())
                    {
                        serie = fdr["SERIE"].ToString();
                    }
                    fdr.Close();
                    fbc.Dispose();

                    folio = Sig_Folio_CP(serie, folio);

                    // SE EJECUTA EL PROCEDURE PARA ACTUALIZAR EL FOLIO
                    // fbc = new FbCommand("EXECUTE PROCEDURE get_sigfol_concepto('CP', " + concepto_cp_id + ", '@', " + folio + ")", conn.FBC, transaction);
                    fbc = new FbCommand("EXECUTE PROCEDURE get_sigfol_concepto('CP', " + concepto_cp_id + ", (" + select_serie + "), " + (folio).Replace(serie, "") + ")", conn.FBC, transaction);
                    fbc.ExecuteNonQuery();
                    fbc.Dispose(); // */
                }
                catch
                {
                    MessageBox.Show("No fue posible generar el folio del nuevo documento.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                #endregion

                #region OBTENEMOS EL ID DE LA SUCURSAL MATRIZ (SE AGREGO PARA MICROSIP 2020)

                try
                {
                    // SE OBTIENE EL ID DE LA SUCURSAL MATRIZ
                    fbc = new FbCommand("SELECT sucursal_id FROM sucursales WHERE nombre = 'Matriz'", conn.FBC, transaction);
                    fdr = fbc.ExecuteReader();
                    while (fdr.Read())
                    {
                        sucursal = fdr["SUCURSAL_ID"].ToString();
                    }
                    fdr.Close();
                    fbc.Dispose();

                    if (sucursal == "")
                    {
                        MessageBox.Show("No se encontro la sucursal matriz.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("No fue posible obtener la sucursal matriz.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                } // */

                #endregion

                #region Insertar en Doctos_cp
                try
                {
                    docto_id_cp = GEN_DOCTO_ID(conn, transaction);

                    if (docto_id_cp != 0)
                    {
                        insert = "INSERT INTO doctos_cp";
                        insert += "( ";
                        insert += "      docto_cp_id, ";
                        insert += "      concepto_cp_id, ";
                        insert += "      sucursal_id, "; // MICROSIP 2020
                        insert += "      folio, ";
                        insert += "      naturaleza_concepto, ";
                        insert += "      fecha, ";
                        insert += "      proveedor_id, ";
                        insert += "      tipo_cambio, ";
                        insert += "      cancelado, ";
                        insert += "      aplicado, ";
                        insert += "      descripcion, ";
                        insert += "      forma_emitida, ";
                        insert += "      contabilizado, ";
                        insert += "      contabilizado_gyp, ";
                        insert += "      pctje_dscto_ppag, ";
                        insert += "      exportado, ";
                        insert += "      sistema_origen, ";
                        insert += "      integ_ba, ";
                        insert += "      contabilizado_ba, ";
                        insert += "      tiene_cfd, ";
                        insert += "      usuario_creador, ";
                        insert += "      usuario_ult_modif ";
                        insert += ") ";
                        insert += "VALUES";
                        insert += "( ";
                        insert += "      " + docto_id_cp + ", "; //************************** docto_cp_id
                        insert += "      " + concepto_cp_id + ", "; //*********************** concepto_cp_id
                        insert += "      " + sucursal + ", "; //***************************** sucursal_id (MICROSIP 2020)
                        insert += "      '" + folio + "' , "; //***************************** folio
                        insert += "      'R', "; //****************************************** naturaleza_concepto
                        insert += "      '" + fecha.ToString("dd.MM.yyyy") + "', "; //******* fecha
                        insert += "      '" + proveedor_id + "', "; //*********************** proveedor_id
                        insert += "      " + 1 + ", "; //************************************ tipo_cambio
                        insert += "      'N', "; //****************************************** cancelado
                        insert += "      'N', "; //****************************************** aplicado
                        // insert += "      '" + descripcion + "', "; //************************ descripcion
                        insert += "      @descripcion, "; //************************ descripcion
                        insert += "      'N', "; //****************************************** forma_emitida
                        insert += "      'N', "; //****************************************** contabilizado
                        insert += "      'N', "; //****************************************** contabilizado_gyp
                        insert += "      " + 0 + ", "; //************************************ pctje_dscto_ppag
                        insert += "      'N', "; //****************************************** exportado
                        insert += "      'CP', "; //***************************************** sistema_origen
                        insert += "      'S', "; //****************************************** integ_ba
                        insert += "      'S', "; //****************************************** contabilizado_ba
                        insert += "      'N', "; //****************************************** tiene_cfd
                        insert += "      '" + usuario_creador + "', "; //******************** usuario_creador
                        insert += "      '" + usuario_creador + "' "; //********************* usuario_ult_modif
                        insert += ")";

                        fbc = new FbCommand(insert, conn.FBC, transaction);
                        fbc.Parameters.Add("@descripcion", FbDbType.VarChar).Value = descripcion;
                        fbc.ExecuteNonQuery();
                        fbc.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("No fue posible generar el pago.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible generar el pago.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                #region Insertar en importes_Doctos_cp
                try
                {
                    docto_id_cp_det = GEN_DOCTO_ID(conn, transaction);

                    if (docto_id_cp_det != 0)
                    {
                        insert = "INSERT INTO importes_doctos_cp";
                        insert += "( ";
                        insert += "      impte_docto_cp_id, ";
                        insert += "      docto_cp_id, ";
                        insert += "      cancelado, ";
                        insert += "      aplicado, ";
                        insert += "      tipo_impte, ";
                        insert += "      docto_cp_acr_id, ";
                        insert += "      importe, ";
                        insert += "      impuesto, ";
                        insert += "      iva_retenido, ";
                        insert += "      isr_retenido,";
                        insert += "      dscto_ppag ";
                        insert += ") ";
                        insert += "VALUES";
                        insert += "( ";
                        insert += "      " + docto_id_cp_det + ", "; //************ impte_docto_cp_id
                        insert += "      " + docto_id_cp + ", "; //**************** docto_cp_id
                        insert += "      'N', "; //******************************** cancelado
                        insert += "      'N', "; //******************************** aplicado
                        if (tipo == "P")
                        {
                            insert += "      'R', "; //******************************** tipo_impte
                            insert += "      " + docto_cp_acr_id + ", "; //************ docto_cp_acr_id
                        }
                        else
                        {
                            insert += "      'A', "; //******************************** tipo_impte
                            insert += "      null, "; //************ docto_cp_acr_id
                        }
                        insert += "      " + importe + ", "; //******************** importe
                        insert += "      0, "; //********************************** impuesto
                        insert += "      0, "; //********************************** iva_retenido
                        insert += "      0, "; //********************************** isr_retenido
                        insert += "      0 "; //*********************************** dscto_ppag
                        insert += ")";

                        fbc = new FbCommand(insert, conn.FBC, transaction);
                        fbc.ExecuteNonQuery();
                        fbc.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("No fue posible generar los importes del pago.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible generar los importes del pago.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                // Aplicar el documento en DOCTOS_CP
                try
                {
                    fbc = new FbCommand("UPDATE doctos_cp SET aplicado = 'S' WHERE docto_cp_id = " + docto_id_cp, conn.FBC, transaction);
                    fbc.ExecuteNonQuery();
                    fbc.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No fue posible aplicar el pago.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Generar el documento en DOCTOS_BA
                try
                {
                    fbc = new FbCommand("EXECUTE PROCEDURE genera_docto_ba_cp(" + docto_id_cp + ")", conn.FBC, transaction);
                    fbc.ExecuteNonQuery();
                    fbc.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo generar el documento en el modulo de bancos de Microsip.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return true;
            }
            else
            {
                MessageBox.Show("Faltan datos para poder generar el documento.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public bool GenerarLiberarCreditos(int docto_pr_id, string folio, string usuario, ref StringBuilder csv, C_CUENTAS_BAN cuentas, C_CREAR_TXT layoutTXT, ref string error)
        {
            C_ConexionSQL conn_sql = new C_ConexionSQL();
            C_CONEXIONFIREBIRD conn_ms = new C_CONEXIONFIREBIRD();
            SqlTransaction transaction_sql;
            FbTransaction transaction_ms;
            FbCommand cmd;
            FbDataReader read;
            SqlCommand sc;
            SqlDataReader sdr;

            C_AGREGACREDITO[] creditos = new C_AGREGACREDITO[0];

            string select = "";
            string estatus_proc = "";
            string estatusDet = "";

            bool completado = true;

            if (conn_sql.ConectarSQL())
            {
                transaction_sql = conn_sql.SC.BeginTransaction();

                try
                {
                    // OBTENEMOS EL ESTATUS DE LA PROGRAMACIÓN
                    string query = "SELECT dpd.ESTATUS ESTATUS_DET, dp.* ";
                    query += "  FROM P_DOCTOS_PR_DET  dpd ";
                    query += "  join P_DOCTOS_PR dp  on dpd.DOCTO_PR_ID = dp.DOCTO_PR_ID  ";
                    query += " where dpd.DOCTO_PR_ID = " + docto_pr_id;
                    sc = new SqlCommand(query, conn_sql.SC, transaction_sql);
                    sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        estatus_proc = sdr["ESTATUS_PROC"].ToString();
                    }
                    sdr.Close();
                    sc.Dispose();

                    if ((estatus_proc == "A") || (estatus_proc == "B") || (estatus_proc == "L"))
                    {
                        #region PRIMERO OBTENEMOS TODOS LOS CARGOS DE LA PROGRAMACIÓN SELECCIONADA

                        select = "SELECT ";
                        select += "      dd.*, dd.ESTATUS ESTATUS_DET, ";
                        select += "      dp.empresa ";
                        select += " FROM p_doctos_pr_det dd ";
                        select += " JOIN p_doctos_pr dp ON(dd.docto_pr_id = dp.docto_pr_id) ";
                        select += "WHERE dd.docto_pr_id = " + docto_pr_id;
                        select += "  AND (dp.estatus_proc = 'A' OR dp.estatus_proc = 'B' OR dp.estatus_proc = 'L') ";
                        select += "  AND (dd.estatus = 'A' OR dd.estatus = 'B' OR dd.estatus = 'L') ";
                        select += " AND dd.EMPRESA = '" + cuentas.EMPRESA + "'";

                        sc = new SqlCommand(select, conn_sql.SC, transaction_sql);
                        sdr = sc.ExecuteReader();
                        while (sdr.Read())
                        {
                            Array.Resize(ref creditos, creditos.Length + 1);
                            creditos[creditos.Length - 1] = new C_AGREGACREDITO();

                            creditos[creditos.Length - 1].DOCTO_PR_DET_ID = Convert.ToInt32(sdr["DOCTO_PR_DET_ID"].ToString());
                            creditos[creditos.Length - 1].DOCTO_PR_ID = Convert.ToInt32(sdr["DOCTO_PR_ID"].ToString());
                            creditos[creditos.Length - 1].FOLIO_MICROSIP = sdr["FOLIO_MICROSIP"].ToString();
                            creditos[creditos.Length - 1].FECHA_CARGO = Convert.ToDateTime(sdr["FECHA_CARGO"].ToString());
                            creditos[creditos.Length - 1].PROVEEDOR_ID = Convert.ToInt32(sdr["PROVEEDOR_ID"].ToString());
                            creditos[creditos.Length - 1].PROVEEDOR_CLAVE = sdr["PROVEEDOR_CLAVE"].ToString();
                            creditos[creditos.Length - 1].PROVEEDOR_NOMBRE = sdr["PROVEEDOR_NOMBRE"].ToString();
                            creditos[creditos.Length - 1].FECHA_VENCIMIENTO = Convert.ToDateTime(sdr["FECHA_VENCIMIENTO"].ToString());
                            creditos[creditos.Length - 1].IMPORTE_PAGOS = Convert.ToDouble(sdr["IMPORTE_PAGOS"].ToString());
                            creditos[creditos.Length - 1].IMPORTE_AUTORIZADO = Convert.ToDouble(sdr["IMPORTE_AUTORIZADO"].ToString());
                            creditos[creditos.Length - 1].TIPO = sdr["TIPO"].ToString();
                            creditos[creditos.Length - 1].ESTATUS = sdr["ESTATUS"].ToString();
                            creditos[creditos.Length - 1].EMPRESA = sdr["EMPRESA"].ToString();
                            creditos[creditos.Length - 1].FOLIO_CREDITO = sdr["FOLIO_CREDITO"].ToString();
                            creditos[creditos.Length - 1].ESTATUS_DET = Convert.ToString(sdr["ESTATUS_DET"]);
                            creditos[creditos.Length - 1].REQUISICION_ID = Convert.ToString(sdr["REQUISICION_ID"]);
                        }
                        sdr.Close();
                        sc.Dispose();

                        #endregion

                        if (creditos.Length > 0)
                        {
                            string empresa = creditos[0].EMPRESA;

                            if (conn_ms.ConectarFB(empresa))
                            {
                                transaction_ms = conn_ms.FBC.BeginTransaction();

                                try
                                {
                                    int concepto_cp_id = 0;
                                    int concepto_anticipo = 0;

                                    if (!string.IsNullOrEmpty(cuentas.concepto_cp))
                                        concepto_cp_id = OBTENER_CONCEPTO_CP_ID(conn_ms, transaction_ms, cuentas.concepto_cp);

                                    if(!string.IsNullOrEmpty(cuentas.concepto_anticipo))
                                        concepto_anticipo = OBTENER_CONCEPTO_CP_ID(conn_ms, transaction_ms, cuentas.concepto_anticipo);

                                    if (concepto_cp_id != 0 || concepto_anticipo != 0)
                                    {
                                        //Services.RegistrosLiberados[] liberados = new Services.RegistrosLiberados[0];

                                        C_LIBERARCREDITO la = new C_LIBERARCREDITO();

                                        for (int r = 0; r < creditos.Length; r++)
                                        {
                                            string descripcion = "";
                                            int docto_cp_acr_id = OBTENER_DOCTO_CP_ID(conn_ms, transaction_ms, creditos[r].FOLIO_MICROSIP, creditos[r].PROVEEDOR_ID, ref descripcion);

                                            if ((docto_cp_acr_id != 0) || (creditos[r].TIPO == "A"))
                                            {
                                                Proveedor proveedor = new Proveedor();

                                                bool proveedor_datos = OBTENER_PROVEEDOR(conn_ms, transaction_ms, creditos[r].PROVEEDOR_ID, ref proveedor);
                                                string clave_banco = GET_CLAVE_BANCO(proveedor.BANCO);

                                                if (creditos[r].ESTATUS_DET != "L")
                                                {
                                                    if (clave_banco != "")
                                                    {
                                                        // SI TODAVIA ESTA EN ESTATUS DE "Autorizado" QUIERE DECIR QUE NO HA SIDO INGRESADO A MICROSIP
                                                        if (estatus_proc == "A")
                                                        {
                                                            string folio_credito = "";
                                                            int docto_cp_id = 0;

                                                            #region GENERA EL NUEVO CREDITO Y LO LIBERA EN CASO DE MARCAR ERROR CANCELA TODO EL PROCESO

                                                            int concepto_credito = string.IsNullOrEmpty(creditos[r].REQUISICION_ID) ? concepto_cp_id : concepto_anticipo;


                                                            if (NuevoCredito(conn_ms, transaction_ms, concepto_credito, creditos[r].PROVEEDOR_ID, DateTime.Now, creditos[r].IMPORTE_AUTORIZADO, creditos[r].TIPO, docto_cp_acr_id, descripcion, usuario, ref docto_cp_id, ref folio_credito))
                                                            {
                                                                // ACTUALIZAMOS EL CREDITO PARA INDICAR QUE YA FUE GENERADO SU ARCHIVO
                                                                sc = new SqlCommand("UPDATE p_doctos_pr_det SET estatus = 'B', folio_credito = '" + folio_credito + "' WHERE docto_pr_det_id = " + creditos[r].DOCTO_PR_DET_ID, conn_sql.SC, transaction_sql);
                                                                sc.ExecuteNonQuery();
                                                                sc.Dispose();

                                                                creditos[r].FOLIO_CREDITO = folio_credito;

                                                                if (!string.IsNullOrEmpty(creditos[r].REQUISICION_ID))
                                                                {
                                                                    sc = new SqlCommand("UPDATE REQ_ENC SET Estatus_general = 'S', Folio_MSP = '" + folio_credito + "' WHERE Requisicion_id = " + creditos[r].REQUISICION_ID, conn_sql.SC, transaction_sql);
                                                                    sc.ExecuteNonQuery();
                                                                    sc.Dispose();
                                                                }

                                                                // LIBERAMOS EL CREDITO GENERADO EN MICROSIP
                                                                if (la.Liberar(conn_ms, transaction_ms, docto_cp_id, creditos[r].IMPORTE_AUTORIZADO, cuentas.cuenta_ban_id, cuentas.moneta_id, DateTime.Now, cuentas.concepto_ba_id, proveedor.CLABE, clave_banco, folio_credito))
                                                                {
                                                                    // ACTUALIZAMOS EL RENGLON A LIBERADO Y GUARDAMOS EL FOLIO DEL CARGO PARA POSTERIORMENTE LIBERARLO EN EL PORTAL
                                                                    if (!la.Liberar_Detalle(conn_sql, transaction_sql, creditos[r].DOCTO_PR_DET_ID, folio_credito))
                                                                    {
                                                                        completado = false;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    completado = false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                completado = false;
                                                                break;
                                                            }

                                                            #endregion
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("No se encontro la clave fiscal del banco '" + proveedor.BANCO + "' en el proveedor '" + proveedor.NOMBRE + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                        completado = false;
                                                        break;
                                                    }
                                                }

                                                #region VALIDAMOS Y AGREGAMOS EL O LOS RENGLONES AL ARCHIVO DE BANCOS

                                                string rfc_ordenante = OBTENER_RFC_EMPRESA(conn_ms, transaction_ms);
                                                string linea = "";

                                                if (proveedor_datos && (rfc_ordenante != ""))
                                                {
                                                    if ((proveedor.BANCO != "") && (proveedor.CLABE != ""))
                                                    {
                                                        rfc_ordenante = rfc_ordenante.Replace(".", "").Replace(",", "").Replace("-", "").Replace(" ", "");

                                                        proveedor.NOMBRE = proveedor.NOMBRE.Replace(".", "").Replace(",", "").Replace("-", "");
                                                        proveedor.NOMBRE = proveedor.NOMBRE.Replace("Ñ", "N");
                                                        proveedor.NOMBRE = proveedor.NOMBRE.Replace("Á", "A");
                                                        proveedor.NOMBRE = proveedor.NOMBRE.Replace("É", "E");
                                                        proveedor.NOMBRE = proveedor.NOMBRE.Replace("Í", "I");
                                                        proveedor.NOMBRE = proveedor.NOMBRE.Replace("Ó", "O");
                                                        proveedor.NOMBRE = proveedor.NOMBRE.Replace("Ú", "U");
                                                        proveedor.NOMBRE = proveedor.NOMBRE.Replace("Ü", "U");


                                                        descripcion = descripcion.Replace(".", "")
                                                            .Replace(",", "")
                                                            .Replace("-", "")
                                                            .Replace("Ñ", "N")
                                                            .Replace("Á", "A")
                                                            .Replace("É", "E")
                                                            .Replace("Í", "I")
                                                            .Replace("Ó", "O")
                                                            .Replace("Ú", "U")
                                                            .Replace("Ü", "U");

                                                        if (!string.IsNullOrEmpty(proveedor.BANCO_DIRECCION))
                                                        {
                                                            proveedor.BANCO_DIRECCION = proveedor.BANCO_DIRECCION.Replace(".", "")
                                                            .Replace(",", "")
                                                            .Replace("-", "")
                                                            .Replace("Ñ", "N")
                                                            .Replace("Á", "A")
                                                            .Replace("É", "E")
                                                            .Replace("Í", "I")
                                                            .Replace("Ó", "O")
                                                            .Replace("Ú", "U")
                                                            .Replace("Ü", "U");
                                                        }

                                                        if (!string.IsNullOrEmpty(proveedor.DIRECCION_PROV))
                                                        {
                                                            proveedor.DIRECCION_PROV = proveedor.DIRECCION_PROV.Replace(".", "")
                                                            .Replace(",", "")
                                                            .Replace("-", "")
                                                            .Replace("Ñ", "N")
                                                            .Replace("Á", "A")
                                                            .Replace("É", "E")
                                                            .Replace("Í", "I")
                                                            .Replace("Ó", "O")
                                                            .Replace("Ú", "U")
                                                            .Replace("Ü", "U")
                                                            .Replace(System.Environment.NewLine,"");
                                                        }

                                                        if (!string.IsNullOrEmpty(proveedor.BANCO_DIRECCION))
                                                        {
                                                            proveedor.BANCO_DIRECCION = proveedor.BANCO_DIRECCION.Replace(".", "")
                                                            .Replace(",", "")
                                                            .Replace("-", "")
                                                            .Replace("Ñ", "N")
                                                            .Replace("Á", "A")
                                                            .Replace("É", "E")
                                                            .Replace("Í", "I")
                                                            .Replace("Ó", "O")
                                                            .Replace("Ú", "U")
                                                            .Replace("Ü", "U");
                                                        }


                                                        if (proveedor.NOMBRE.Length > 30)
                                                        {
                                                            proveedor.NOMBRE = proveedor.NOMBRE.Substring(0, 30);
                                                        }

                                                        proveedor.RFC_CURP = proveedor.RFC_CURP.Replace(".", "").Replace(",", "").Replace("-", "").Replace(" ", "");

                                                        
                                                        
                                                        switch (cuentas.formato_layout)
                                                        {
                                                            #region BBVA

                                                            #region LAYOUT  Pagos Internacionales OPI
                                                            case "Pagos Internacionales OPI":

                                                                #region VALIDACION
                                                                if (
                                                                    string.IsNullOrEmpty(cuentas.cuenta_ordenante) ||
                                                                    string.IsNullOrEmpty(proveedor.CUENTA) ||
                                                                    string.IsNullOrEmpty(creditos[r].IMPORTE_AUTORIZADO.ToString()) ||
                                                                    string.IsNullOrEmpty(proveedor.ABA_BIC) ||
                                                                    string.IsNullOrEmpty(proveedor.BANCO) ||
                                                                    string.IsNullOrEmpty(proveedor.PAIS_BANCO) ||
                                                                    string.IsNullOrEmpty(proveedor.BANCO_DIRECCION) ||
                                                                    string.IsNullOrEmpty(proveedor.NOMBRE) ||
                                                                    string.IsNullOrEmpty(proveedor.PAIS_PROV) ||
                                                                    string.IsNullOrEmpty(proveedor.DIRECCION_PROV)

                                                                    )
                                                                {

                                                                    string elProveedor = "Error en Layout Pagos Internacionales OPI\n El proveedor " + proveedor.NOMBRE;

                                                                    if (string.IsNullOrEmpty(proveedor.CUENTA))
                                                                        error = elProveedor + " no tiene el numero de cuenta";
                                                                    if (string.IsNullOrEmpty(proveedor.ABA_BIC))
                                                                        error = elProveedor + " no tiene registrado el ABA/BIC";
                                                                    if (string.IsNullOrEmpty(proveedor.PAIS_BANCO))
                                                                        error = elProveedor + " no tiene registrado el Pais del banco";
                                                                    if (string.IsNullOrEmpty(proveedor.BANCO_DIRECCION))
                                                                        error = elProveedor + " no tiene registrada la direccion del banco";
                                                                    if (string.IsNullOrEmpty(proveedor.PAIS_PROV))
                                                                        error = elProveedor + " no tiene registrado su pais origen";
                                                                    if (string.IsNullOrEmpty(proveedor.DIRECCION_PROV))
                                                                        error = elProveedor + " no tiene una direccion valida";
                                                                    

                                                                    completado = false;
                                                                    break;
                                                                }
                                                                #endregion

                                                                #region ASUNTO ORDENANTE
                                                                string cerosAsuntoOrd = "";
                                                                if (cuentas.cuenta_ordenante.Length != 18)
                                                                {
                                                                    for (int i = cuentas.cuenta_ordenante.Length; i < 18; i++)
                                                                    {
                                                                        cerosAsuntoOrd += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region ASUNTO BENEFICIARIO
                                                                string esapciosBene = "";
                                                                if (proveedor.CUENTA.Length != 35)
                                                                {
                                                                    if (proveedor.CUENTA.Length > 35)
                                                                    {
                                                                        completado = false;
                                                                        break;
                                                                    }
                                                                    for (int i = proveedor.CUENTA.Length; i < 35; i++)
                                                                    {
                                                                        esapciosBene += " ";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region IMPORTE
                                                                string cerosImporte = "";
                                                                if (creditos[r].IMPORTE_AUTORIZADO.ToString().Length != 16)
                                                                {
                                                                    for (int i = creditos[r].IMPORTE_AUTORIZADO.ToString().Length; i < 16; i++)
                                                                    {
                                                                        cerosImporte += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region DETALLE PAGO
                                                                string espaciosDescripcion = "";
                                                                if (descripcion.Length != 50)
                                                                {
                                                                    if (descripcion.Length > 50)
                                                                        descripcion = descripcion.Substring(0, 50);
                                                                    else
                                                                    {
                                                                        for (int i = descripcion.Length; i < 50; i++)
                                                                        {
                                                                            espaciosDescripcion += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region ABA/BIC
                                                                string aba = "";
                                                                for (int i = proveedor.ABA_BIC.Length; i < 15; i++)
                                                                {
                                                                    aba += " ";
                                                                }
                                                                #endregion

                                                                #region BANCO BENEFICIARIO
                                                                string banBen = "";
                                                                if (proveedor.BANCO.Length != 30)
                                                                {
                                                                    if(proveedor.BANCO.Length > 30)
                                                                        banBen = proveedor.BANCO.Substring(0, 30);

                                                                    for (int i = proveedor.BANCO.Length; i < 30; i++)
                                                                    {
                                                                        banBen += " ";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region PAIS BANCO
                                                                string paisBan = "";
                                                                if(proveedor.PAIS_BANCO.Length != 30)
                                                                {
                                                                    if(proveedor.PAIS_BANCO.Length > 30)
                                                                        proveedor.PAIS_BANCO = proveedor.PAIS_BANCO.Substring(0, 30);
                                                                    for (int i = proveedor.PAIS_BANCO.Length; i < 30; i++)
                                                                    {
                                                                        paisBan += " ";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region DX BANCO
                                                                string dxBan = "";
                                                                if(proveedor.BANCO_DIRECCION.Length != 40)
                                                                {
                                                                    if (proveedor.BANCO_DIRECCION.Length > 40)
                                                                        proveedor.BANCO_DIRECCION = proveedor.BANCO_DIRECCION.Substring(0, 40);

                                                                    for (int i = proveedor.BANCO_DIRECCION.Length; i < 40; i++)
                                                                    {
                                                                        dxBan += " ";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region NOMBRE PROV
                                                                string provNom = "";
                                                                if(proveedor.NOMBRE.Length != 30)
                                                                {
                                                                    if(proveedor.NOMBRE.Length>30)
                                                                        proveedor.NOMBRE = proveedor.NOMBRE.Substring(0, 30);

                                                                    for (int i = proveedor.NOMBRE.Length; i < 30; i++)
                                                                    {
                                                                        provNom += " ";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region PAIS BENEF
                                                                string paisProv = "";
                                                                if(proveedor.PAIS_PROV.Length != 30)
                                                                {
                                                                    if (proveedor.PAIS_PROV.Length > 30)
                                                                        proveedor.PAIS_PROV = proveedor.PAIS_PROV.Substring(0, 30);

                                                                    for (int i = proveedor.PAIS_PROV.Length; i < 30; i++)
                                                                    {
                                                                        paisProv += " ";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region DX BENEF
                                                                string direProv = "";
                                                                if (proveedor.DIRECCION_PROV.Length != 40)
                                                                {
                                                                    if (proveedor.DIRECCION_PROV.Length > 40)
                                                                        proveedor.DIRECCION_PROV = proveedor.DIRECCION_PROV.Substring(0, 40);

                                                                    for (int i = proveedor.DIRECCION_PROV.Length; i < 40; i++)
                                                                    {
                                                                        direProv += " ";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region TELEFONO BENEF
                                                                string cerosTelProv = "";

                                                                for (int i = proveedor.TELEFONO_PROV.Length; i < 12; i++)
                                                                {
                                                                    cerosTelProv += "0";
                                                                }
                                                                
                                                                #endregion

                                                                linea = "OPI";
                                                                linea += cerosAsuntoOrd + cuentas.cuenta_ordenante; //******************** 1.- Cuenta Ordenante
                                                                linea += proveedor.CUENTA + esapciosBene; //****************************** 2.- Asunto Beneficiario
                                                                linea += cerosImporte + creditos[r].IMPORTE_AUTORIZADO.ToString("#.00");// 3.- Importe de la Operacion
                                                                linea += proveedor.CLAVE_FISCAL;//**************************************** 4.- divisa
                                                                linea += descripcion + espaciosDescripcion;//***************************** 5.- Detalle del pago
                                                                linea += proveedor.ABA_BIC + aba;//*************************************** 6.- ABA/BIC
                                                                linea += proveedor.BANCO + banBen;//************************************** 7.- Banco beneficiario
                                                                linea += proveedor.PAIS_BANCO + paisBan;//******************************** 8.- Pais banco
                                                                linea += proveedor.BANCO_DIRECCION + dxBan;//***************************** 9.- DX BANCO
                                                                linea += proveedor.NOMBRE + provNom;//************************************ 10.-  TITULAR ASUNTO BEN
                                                                linea += proveedor.PAIS_PROV + paisProv;//******************************** 11.- Pais Benef
                                                                linea += proveedor.DIRECCION_PROV + direProv;//*************************** 12.- Direccion Benef
                                                                linea += cerosTelProv + proveedor.TELEFONO_PROV;//************************ 13.- Telefono Benef

                                                                //escribimos en el archivo txt
                                                                layoutTXT.ADD_LINEA(linea);
                                                                break;
                                                            #endregion

                                                            #region Traspaso Mismo Banco
                                                            case "Traspaso Mismo Banco":

                                                                #region ASUNTO BENEFICIARIO
                                                                string cerosBen = "";
                                                                if(proveedor.CUENTA.Length != 18)
                                                                {
                                                                    if(proveedor.CUENTA.Length > 18)
                                                                    {
                                                                        error = "El proveedor " + proveedor.NOMBRE + " tiene una cuenta no valida";
                                                                        completado = false;
                                                                        break;
                                                                    }
                                                                    for (int i = proveedor.CUENTA.Length; i < 18; i++)
                                                                    {
                                                                        cerosBen += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region CUENTA ORDENANTE
                                                                if (cuentas.cuenta_ordenante.Length != 18)
                                                                {
                                                                    string ceroCuenta = "";
                                                                    for (int i = cuentas.cuenta_ordenante.Length; i < 18; i++)
                                                                    {
                                                                        ceroCuenta += "0";
                                                                    }

                                                                    cuentas.cuenta_ordenante = ceroCuenta + cuentas.cuenta_ordenante;
                                                                }
                                                                #endregion

                                                                #region IMPORTE
                                                                cerosImporte = "";
                                                                if (creditos[r].IMPORTE_AUTORIZADO.ToString().Length != 16)
                                                                {
                                                                    for (int i = creditos[r].IMPORTE_AUTORIZADO.ToString().Length; i < 16; i++)
                                                                    {
                                                                        cerosImporte += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region MOTIVO DE PAGO
                                                                espaciosDescripcion = "";
                                                                if(descripcion.Length != 30)
                                                                {
                                                                    if(descripcion.Length > 30)
                                                                        descripcion = descripcion.Substring(0, 30);
                                                                    else
                                                                    {
                                                                        for (int i = descripcion.Length; i < 30; i++)
                                                                        {
                                                                            espaciosDescripcion += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                linea = "TNN";
                                                                linea += cerosBen + proveedor.CUENTA; //********************************** 1.- Asunto beneficiario
                                                                linea += cuentas.cuenta_ordenante; //************************************* 2.- Cuenta Ordenante
                                                                linea += proveedor.CLAVE_FISCAL == "MXN" ? "MXP" : proveedor.CLAVE_FISCAL; // 3.- Divisa Ordenante
                                                                linea += cerosImporte + creditos[r].IMPORTE_AUTORIZADO.ToString("#.00");// 4.- Importe de la Operacion
                                                                linea += descripcion + espaciosDescripcion; //**************************** 5.- Motivo de Pago

                                                                #region COMP FISCAL

                                                                string cerosCLABE = "";
                                                                if (proveedor.RFC_CURP.Length != 18)
                                                                {
                                                                    if (proveedor.RFC_CURP.Length > 18)
                                                                        proveedor.RFC_CURP = proveedor.CLABE.Substring(0, 18);
                                                                    else
                                                                    {
                                                                        for (int i = proveedor.RFC_CURP.Length; i < 18; i++)
                                                                        {
                                                                            cerosCLABE += " ";
                                                                        }
                                                                    }
                                                                }

                                                                string iva = GET_IVA_PAGO(creditos[r].FOLIO_MICROSIP, creditos[r].PROVEEDOR_ID, creditos[r].FECHA_CARGO, creditos[r].EMPRESA);

                                                                //ponemos formato de 15 digitos como lo solicita el layout
                                                                string cerosIVA = "";
                                                                for (int i = iva.Length; i < 15; i++)
                                                                {
                                                                    cerosIVA += "0";
                                                                }

                                                                linea += cuentas.requiereCompFiscal == true ? "1" : "0"; //********* 1. Comprobante fiscal
                                                                linea += proveedor.RFC_CURP + cerosCLABE;
                                                                linea += cerosIVA + iva;
                                                                #endregion

                                                                //escribimos en el archivo txt
                                                                layoutTXT.ADD_LINEA(linea);
                                                                break;
                                                            #endregion

                                                            #region Pagos Mismo Banco
                                                            case "Pagos Mismo Banco":

                                                                #region ASUNTO BENEFICIARIO
                                                                cerosBen = "";
                                                                if (proveedor.CUENTA.Length != 18)
                                                                {
                                                                    if (proveedor.CUENTA.Length > 18)
                                                                    {
                                                                        error = "El proveedor " + proveedor.NOMBRE + " tiene una cuenta no valida";
                                                                        completado = false;
                                                                        break;
                                                                    }
                                                                    for (int i = proveedor.CUENTA.Length; i < 18; i++)
                                                                    {
                                                                        cerosBen += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region CUENTA ORDENANTE
                                                                if (cuentas.cuenta_ordenante.Length != 18)
                                                                {
                                                                    string ceroCuenta = "";
                                                                    for (int i = cuentas.cuenta_ordenante.Length; i < 18; i++)
                                                                    {
                                                                        ceroCuenta += "0";
                                                                    }

                                                                    cuentas.cuenta_ordenante = ceroCuenta + cuentas.cuenta_ordenante;
                                                                }
                                                                #endregion

                                                                #region IMPORTE
                                                                cerosImporte = "";
                                                                if (creditos[r].IMPORTE_AUTORIZADO.ToString().Length != 16)
                                                                {
                                                                    for (int i = creditos[r].IMPORTE_AUTORIZADO.ToString().Length; i < 16; i++)
                                                                    {
                                                                        cerosImporte += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region MOTIVO PAGO
                                                                espaciosDescripcion = "";
                                                                if (descripcion.Length != 30)
                                                                {
                                                                    if (descripcion.Length > 30)
                                                                        descripcion = descripcion.Substring(0, 30);
                                                                    else
                                                                    {
                                                                        for (int i = descripcion.Length; i < 30; i++)
                                                                        {
                                                                            espaciosDescripcion += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                linea = "PTC";
                                                                linea += cerosBen + proveedor.CUENTA; //********************************** 1.- Asunto beneficiario
                                                                linea += cuentas.cuenta_ordenante; //************************************* 2.- Cuenta Ordenante
                                                                linea += proveedor.CLAVE_FISCAL == "MXN" ? "MXP" : proveedor.CLAVE_FISCAL; // 3.- Divisa Ordenante
                                                                linea += cerosImporte + creditos[r].IMPORTE_AUTORIZADO.ToString("#.00");// 4.- Importe de la Operacion
                                                                linea += descripcion + espaciosDescripcion; //**************************** 5.- Motivo de Pago


                                                                #region COMP FISCAL

                                                                cerosCLABE = "";
                                                                if (proveedor.RFC_CURP.Length != 18)
                                                                {
                                                                    if (proveedor.RFC_CURP.Length > 18)
                                                                        proveedor.RFC_CURP = proveedor.CLABE.Substring(0, 18);
                                                                    else
                                                                    {
                                                                        for (int i = proveedor.RFC_CURP.Length; i < 18; i++)
                                                                        {
                                                                            cerosCLABE += " ";
                                                                        }
                                                                    }
                                                                }

                                                                iva = GET_IVA_PAGO(creditos[r].FOLIO_MICROSIP, creditos[r].PROVEEDOR_ID, creditos[r].FECHA_CARGO, creditos[r].EMPRESA);

                                                                //ponemos formato de 15 digitos como lo solicita el layout
                                                                cerosIVA = "";
                                                                for (int i = iva.Length; i < 15; i++)
                                                                {
                                                                    cerosIVA += "0";
                                                                }

                                                                linea += cuentas.requiereCompFiscal == true ? "1" : "0"; //********* 1. Comprobante fiscal
                                                                linea += proveedor.RFC_CURP + cerosCLABE;
                                                                linea += cerosIVA + iva;
                                                                #endregion

                                                                //escribimos en el archivo txt
                                                                layoutTXT.ADD_LINEA(linea);
                                                                break;
                                                            #endregion

                                                            #region Traspasos Interbancarios
                                                            case "Traspasos Interbancarios":

                                                                #region ASUNTO BENEFICIARIO
                                                                
                                                                if (proveedor.CLABE.Length != 18)
                                                                {
                                                                    error = "El proveedor " + proveedor.NOMBRE + " tiene una CLABE no valida";
                                                                    completado = false;
                                                                    break;
                                                                }
                                                                #endregion

                                                                #region CUENTA ORDENANTE
                                                                string cerosCuenOrd = "";
                                                                if (cuentas.cuenta_ordenante.Length != 17)
                                                                {
                                                                    for (int i = cuentas.cuenta_ordenante.Length; i < 18; i++)
                                                                    {
                                                                        cerosCuenOrd += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region IMPORTE
                                                                string cerosImporteT = "";
                                                                if (creditos[r].IMPORTE_AUTORIZADO.ToString().Length != 15)
                                                                {
                                                                    for (int i = creditos[r].IMPORTE_AUTORIZADO.ToString().Length; i < 16; i++)
                                                                    {
                                                                        cerosImporteT += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region NOMBRE PROVEEDOR
                                                                string espaciosDescripcionT = "";
                                                                if (proveedor.NOMBRE.Length != 30)
                                                                {
                                                                    if (proveedor.NOMBRE.Length > 30)
                                                                        proveedor.NOMBRE = proveedor.NOMBRE.Substring(0, 30);
                                                                    else
                                                                    {
                                                                        for (int i = proveedor.NOMBRE.Length; i < 30; i++)
                                                                        {
                                                                            espaciosDescripcionT += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region MOTIVO PAGO
                                                                string motivoEspacios = "";

                                                                if (descripcion.Length > 30)
                                                                    descripcion = descripcion.Substring(0, 30);

                                                                for (int i = descripcion.Length; i < 30; i++)
                                                                {
                                                                    motivoEspacios += " ";
                                                                }
                                                                #endregion

                                                                linea = "TSC";
                                                                linea += proveedor.CLABE; //************************************************ 1.- Asunto beneficiario
                                                                linea += cerosCuenOrd + cuentas.cuenta_ordenante; //************************************** 2.- Cuenta Ordenante
                                                                linea += proveedor.CLAVE_FISCAL == "MXN" ? "MXP" : proveedor.CLAVE_FISCAL; // 3.- Divisa Ordenante
                                                                linea += cerosImporteT + creditos[r].IMPORTE_AUTORIZADO.ToString("#.00");// 4.- Importe de la Operacion
                                                                linea += proveedor.NOMBRE + espaciosDescripcionT; //*********************** 5.- Titular Asunto Beneficiario
                                                                linea += "40"; //**************se pone 40 por que sera por medio de CLABE   6.- Tipo de cuenta
                                                                linea += cuentas.clave_fiscal; //****************************************** 7.- Numero de Banco del Asunto Benef.
                                                                linea += descripcion + motivoEspacios; //********************************** 8.- Motivo de pago
                                                                linea += DateTime.Now.ToString("yyMMdd "); //******************************* 9.- Referencia numerica (fecha)
                                                                linea += cuentas.Disponibilidad; //**************************************** 10 Disponibilidad H mismo dia M dia siguiente


                                                                #region COMP FISCAL

                                                                cerosCLABE = "";
                                                                if (proveedor.RFC_CURP.Length != 18)
                                                                {
                                                                    if (proveedor.RFC_CURP.Length > 18)
                                                                        proveedor.RFC_CURP = proveedor.CLABE.Substring(0, 18);
                                                                    else
                                                                    {
                                                                        for (int i = proveedor.RFC_CURP.Length; i < 18; i++)
                                                                        {
                                                                            cerosCLABE += " ";
                                                                        }
                                                                    }
                                                                }

                                                                iva = GET_IVA_PAGO(creditos[r].FOLIO_MICROSIP, creditos[r].PROVEEDOR_ID, creditos[r].FECHA_CARGO, creditos[r].EMPRESA);

                                                                //ponemos formato de 15 digitos como lo solicita el layout
                                                                cerosIVA = "";
                                                                for (int i = iva.Length; i < 15; i++)
                                                                {
                                                                    cerosIVA += "0";
                                                                }

                                                                linea += cuentas.requiereCompFiscal == true ? "1" : "0"; //********* 1. Comprobante fiscal
                                                                linea += proveedor.RFC_CURP + cerosCLABE;
                                                                linea += cerosIVA + iva;
                                                                #endregion

                                                                //escribimos en el archivo txt
                                                                layoutTXT.ADD_LINEA(linea);
                                                                break;

                                                            #endregion

                                                            #region Pagos Interbancarios
                                                            case "Pagos Interbancarios":

                                                                #region ASUNTO BENEFICIARIO

                                                                if (proveedor.CLABE.Length != 18)
                                                                {
                                                                    error = "El proveedor " + proveedor.NOMBRE + " tiene una CLABE no valida";
                                                                    completado = false;
                                                                    break;
                                                                }
                                                                #endregion

                                                                #region CUENTA ORDENANTE
                                                                cerosCuenOrd = "";
                                                                if (cuentas.cuenta_ordenante.Length != 17)
                                                                {
                                                                    for (int i = cuentas.cuenta_ordenante.Length; i < 18; i++)
                                                                    {
                                                                        cerosCuenOrd += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region IMPORTE
                                                                cerosImporteT = "";
                                                                if (creditos[r].IMPORTE_AUTORIZADO.ToString().Length != 15)
                                                                {
                                                                    for (int i = creditos[r].IMPORTE_AUTORIZADO.ToString().Length; i < 16; i++)
                                                                    {
                                                                        cerosImporteT += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region NOMBRE PROVEEDOR
                                                                espaciosDescripcionT = "";
                                                                if (proveedor.NOMBRE.Length != 30)
                                                                {
                                                                    if (proveedor.NOMBRE.Length > 30)
                                                                        proveedor.NOMBRE = proveedor.NOMBRE.Substring(0, 30);
                                                                    else
                                                                    {
                                                                        for (int i = proveedor.NOMBRE.Length; i < 30; i++)
                                                                        {
                                                                            espaciosDescripcionT += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region MOTIVO PAGO
                                                                motivoEspacios = "";

                                                                if (descripcion.Length > 30)
                                                                    descripcion = descripcion.Substring(0, 30);

                                                                for (int i = descripcion.Length; i < 30; i++)
                                                                {
                                                                    motivoEspacios += " ";
                                                                }
                                                                #endregion


                                                                linea = "PSC";
                                                                linea += proveedor.CLABE; //************************************************ 1.- Asunto beneficiario
                                                                linea += cerosCuenOrd + cuentas.cuenta_ordenante; //************************************** 2.- Cuenta Ordenante
                                                                linea += proveedor.CLAVE_FISCAL == "MXN" ? "MXP" : proveedor.CLAVE_FISCAL; // 3.- Divisa Ordenante
                                                                linea += cerosImporteT + creditos[r].IMPORTE_AUTORIZADO.ToString("#.00");// 4.- Importe de la Operacion
                                                                linea += proveedor.NOMBRE + espaciosDescripcionT; //*********************** 5.- Titular Asunto Beneficiario
                                                                linea += "40"; //**************se pone 40 por que sera por medio de CLABE   6.- Tipo de cuenta
                                                                linea += cuentas.clave_fiscal; //****************************************** 7.- Numero de Banco del Asunto Benef.
                                                                linea += descripcion + motivoEspacios; //********************************** 8.- Motivo de pago
                                                                linea += DateTime.Now.ToString("yyMMdd "); //******************************* 9.- Referencia numerica (fecha)
                                                                linea += cuentas.Disponibilidad; //**************************************** 10 Disponibilidad H mismo dia M dia siguiente

                                                                #region COMP FISCAL

                                                                cerosCLABE = "";
                                                                if (proveedor.RFC_CURP.Length != 18)
                                                                {
                                                                    if (proveedor.RFC_CURP.Length > 18)
                                                                        proveedor.RFC_CURP = proveedor.CLABE.Substring(0, 18);
                                                                    else
                                                                    {
                                                                        for (int i = proveedor.RFC_CURP.Length; i < 18; i++)
                                                                        {
                                                                            cerosCLABE += " ";
                                                                        }
                                                                    }
                                                                }

                                                                iva = GET_IVA_PAGO(creditos[r].FOLIO_MICROSIP, creditos[r].PROVEEDOR_ID, creditos[r].FECHA_CARGO, creditos[r].EMPRESA);

                                                                //ponemos formato de 15 digitos como lo solicita el layout
                                                                cerosIVA = "";
                                                                for (int i = iva.Length; i < 15; i++)
                                                                {
                                                                    cerosIVA += "0";
                                                                }

                                                                linea += cuentas.requiereCompFiscal == true ? "1" : "0"; //********* 1. Comprobante fiscal
                                                                linea += proveedor.RFC_CURP + cerosCLABE;
                                                                linea += cerosIVA + iva;
                                                                #endregion

                                                                //escribimos en el archivo txt
                                                                layoutTXT.ADD_LINEA(linea);
                                                                break;

                                                            #endregion

                                                            #region PAGOS A CONVENIOS CIE
                                                            case "Pagos a convenios CIE":

                                                                #region VALIDACION
                                                                if (
                                                                    string.IsNullOrEmpty(cuentas.cuenta_ordenante) ||
                                                                    string.IsNullOrEmpty(proveedor.CONCEPTO_CIE) ||
                                                                    string.IsNullOrEmpty(creditos[r].IMPORTE_AUTORIZADO.ToString()) ||
                                                                    string.IsNullOrEmpty(proveedor.CONVENIO_CIE) ||
                                                                    string.IsNullOrEmpty(proveedor.BANCO) ||
                                                                    string.IsNullOrEmpty(proveedor.NOMBRE)

                                                                    )
                                                                {
                                                                    string nombreProveedor = "Error en Pagos a convenios CIE\n El proveedor " + proveedor.NOMBRE;
                                                                    if (string.IsNullOrEmpty(proveedor.CONCEPTO_CIE))
                                                                        error = nombreProveedor + " no tiene registrado el concepto CIE";
                                                                    if (string.IsNullOrEmpty(proveedor.CONVENIO_CIE))
                                                                        error = nombreProveedor + " no tiene registrdo el convenio CIE";
                                                                    if (string.IsNullOrEmpty(proveedor.BANCO))
                                                                        error = nombreProveedor + " no tiene registrado el banco";
                                                                    completado = false;
                                                                    break;
                                                                }
                                                                #endregion

                                                                #region CONCEPTO CIE
                                                                string cerosConCie = "";
                                                                if(proveedor.CONCEPTO_CIE.Length != 30)
                                                                {
                                                                    for (int i = proveedor.CONCEPTO_CIE.Length; i < 30; i++)
                                                                    {
                                                                        cerosConCie += " ";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region CONVENIO CIE
                                                                string cerosConvCie = "";
                                                                if (proveedor.CONVENIO_CIE.Length != 7)
                                                                {
                                                                    for (int i = proveedor.CONVENIO_CIE.Length; i < 7; i++)
                                                                    {
                                                                        cerosConvCie += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region ASUNTO ORDENANTE
                                                                cerosAsuntoOrd = "";
                                                                if (cuentas.cuenta_ordenante.Length != 18)
                                                                {
                                                                    for (int i = cuentas.cuenta_ordenante.Length; i < 18; i++)
                                                                    {
                                                                        cerosAsuntoOrd += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region IMPORTE
                                                                cerosImporte = "";
                                                                if (creditos[r].IMPORTE_AUTORIZADO.ToString().Length != 16)
                                                                {
                                                                    for (int i = creditos[r].IMPORTE_AUTORIZADO.ToString().Length; i < 16; i++)
                                                                    {
                                                                        cerosImporte += "0";
                                                                    }
                                                                }
                                                                #endregion

                                                                #region MOTIVO PAGO
                                                                motivoEspacios = "";

                                                                if (descripcion.Length > 30)
                                                                    descripcion = descripcion.Substring(0, 30);

                                                                for (int i = descripcion.Length; i < 30; i++)
                                                                {
                                                                    motivoEspacios += " ";
                                                                }
                                                                #endregion

                                                                #region REFERNCIA CIE
                                                                string referenciaCIE = cuentas.cuenta_ordenante + " " + DateTime.Now.ToString("yyyy MM dd");
                                                                string espaciosRefCie = "";
                                                                if(referenciaCIE.Length != 20)
                                                                {
                                                                    if(referenciaCIE.Length>20)
                                                                        referenciaCIE = referenciaCIE.Substring(0, 20);

                                                                    for (int i = referenciaCIE.Length; i < 20; i++)
                                                                    {
                                                                        espaciosRefCie += " ";
                                                                    }
                                                                }
                                                                #endregion


                                                                linea = "CIL";
                                                                linea += proveedor.CONCEPTO_CIE + cerosConCie; //************************************ 1.- CONCEPTO CIE 
                                                                linea += cerosConvCie + proveedor.CONVENIO_CIE; //*********************************** 2.- CONVENIO CIE 
                                                                linea += cerosAsuntoOrd + cuentas.cuenta_ordenante; //******************************* 3.- Asunto Ordenante
                                                                linea += cerosImporte + creditos[r].IMPORTE_AUTORIZADO.ToString("#.00");//*********** 4.- Importe de la Operacion
                                                                linea += descripcion + motivoEspacios;//********************************************* 5.- Motivo Pago
                                                                linea += referenciaCIE + espaciosRefCie; //****************************************** 6.- Refencia CIE

                                                                //escribimos en el archivo txt
                                                                layoutTXT.ADD_LINEA(linea);
                                                                break;
                                                            #endregion

                                                            #endregion

                                                            #region HSCB
                                                            case "MX - SPEI":
                                                                #region LAYOUT MX - SPEI

                                                                // LA CLABE DEBE TENER 20 CARACTERES EN CASO DE QUE FALTEN SE RELLENAN CON CEROS A LA IZQUIERDA
                                                                while (proveedor.CLABE.Length < 20)
                                                                {
                                                                    proveedor.CLABE = "0" + proveedor.CLABE;
                                                                }

                                                                linea = cuentas.cuenta_ordenante + ","; //************************************* Cuenta Ordenante
                                                                linea += rfc_ordenante + ","; //*************************************** RFC del Ordenante
                                                                linea += proveedor.CLABE + ","; //************************************* Cuenta Beneficiaria
                                                                linea += clave_banco + ","; //***************************************** Banco Beneficiario
                                                                linea += proveedor.NOMBRE + ","; //************************************ Nombre del Beneficiario
                                                                linea += creditos[r].IMPORTE_AUTORIZADO.ToString("#.00") + ","; //***** Monto

                                                                if (docto_cp_acr_id.ToString().Length > 7)
                                                                {
                                                                    linea += docto_cp_acr_id.ToString().Substring(0, 7) + ","; //************************************* Referencia Numérica
                                                                }
                                                                else
                                                                {
                                                                    linea += docto_cp_acr_id.ToString() + ","; //************************************* Referencia Numérica
                                                                }

                                                                linea += folio + " " + creditos[r].FOLIO_CREDITO + ","; //************* Concepto de Pago
                                                                linea += creditos[r].FOLIO_CREDITO; //********************************* Referencia del Ordenante

                                                                csv.AppendLine(linea);

                                                                #endregion
                                                                break;
                                                            case "MX - Transferencia TEF local":
                                                                #region LAYOUT MX - Transferencia TEF local

                                                                // LA CLABE DEBE TENER 20 CARACTERES EN CASO DE QUE FALTEN SE RELLENAN CON CEROS A LA IZQUIERDA
                                                                while (proveedor.CLABE.Length < 20)
                                                                {
                                                                    proveedor.CLABE = "0" + proveedor.CLABE;
                                                                }

                                                                linea = cuentas.cuenta_ordenante + ","; //************************************* Cuenta ordenante
                                                                linea += DateTime.Now.AddDays(1).ToString("ddMMyyyy") + ","; //******** Fecha valor
                                                                linea += proveedor.RFC_CURP + ","; //********************************** RFC
                                                                linea += proveedor.CLAVE_FISCAL + ","; //****************************** Moneda
                                                                linea += descripcion + ""; //****************************************** Número de Lote

                                                                csv.AppendLine(linea);

                                                                linea = "CLA,"; //***************************************************** Tipo de Cuenta
                                                                linea += clave_banco + ","; //***************************************** Código de banco
                                                                linea += proveedor.CLABE + ","; //************************************* Cuenta Beneficiaria
                                                                linea += proveedor.NOMBRE + ","; //************************************ Nombre del Beneficiario
                                                                linea += creditos[r].IMPORTE_AUTORIZADO.ToString("#.00") + ","; //***** Monto

                                                                // linea += docto_cp_acr_id + ","; //************************************* Referencia Numérica
                                                                if (docto_cp_acr_id.ToString().Length > 7)
                                                                {
                                                                    linea += docto_cp_acr_id.ToString().Substring(0, 7) + ","; //************************************* Referencia Numérica
                                                                }
                                                                else
                                                                {
                                                                    linea += docto_cp_acr_id.ToString() + ","; //************************************* Referencia Numérica
                                                                }

                                                                linea += folio + " " + creditos[r].FOLIO_CREDITO + ","; //************* Referencia Alfanumérica
                                                                linea += ","; //******************************************************* RFC del Beneficiario*
                                                                linea += ","; //******************************************************* IVA*
                                                                linea += ""; //******************************************************** Correo Electrónico del Beneficiario*

                                                                csv.AppendLine(linea);

                                                                #endregion
                                                                break;
                                                            case "MX - Transferencia a tercero":
                                                                #region LAYOUT MX - Transferencia a tercero

                                                                // LA CLABE DEBE TENER 20 CARACTERES EN CASO DE QUE FALTEN SE RELLENAN CON CEROS A LA IZQUIERDA
                                                                while (proveedor.CLABE.Length < 20)
                                                                {
                                                                    proveedor.CLABE = "0" + proveedor.CLABE;
                                                                }

                                                                if (proveedor.CUENTA != "")
                                                                {
                                                                    linea = (r + 1) + ","; //****************************************************************** Número de transacciones
                                                                    linea += cuentas.cuenta_ordenante + ","; //******************************************************** Cuenta Ordenante
                                                                    linea += proveedor.CUENTA + ","; //******************************************************** Cuenta Beneficiaria
                                                                    linea += creditos[r].IMPORTE_AUTORIZADO.ToString("#.00") + ","; //************************* Monto
                                                                    linea += (proveedor.CLAVE_FISCAL == "MXN" ? "1" : "2") + ","; //*************************** Moneda
                                                                    linea += folio + " " + creditos[r].FOLIO_CREDITO + " " + docto_cp_acr_id + ","; //********* Referencia*
                                                                    linea += proveedor.NOMBRE + ","; //******************************************************** Nombre del Beneficiario
                                                                    linea += "0,"; //************************************************************************** Comprobante Fiscar (CF)
                                                                    linea += proveedor.RFC_CURP + ","; //****************************************************** RFC del Beneficiario
                                                                    linea += ","; //*************************************************************************** IVA*
                                                                    linea += ""; //**************************************************************************** Correo Electrónico del Beneficiario*

                                                                    csv.AppendLine(linea);
                                                                }

                                                                #endregion
                                                                break;
                                                            #endregion

                                                            #region SANTANDER

                                                            #region Interbancaria
                                                            case "Interbancaria":

                                                                #region 1 CUENTA DE CARGO
                                                                string espacioCuentaOrd = "";
                                                                if (cuentas.cuenta_ordenante.Length != 16)
                                                                {
                                                                    if(cuentas.cuenta_ordenante.Length > 16)
                                                                    {
                                                                        error = "La cuenta ordenante esta incorrecta debe de ser de 16 digitos maximo";
                                                                        completado = false;
                                                                        break;
                                                                    }    

                                                                    if(cuentas.cuenta_ordenante.Length < 16)
                                                                    {
                                                                        for (int i = cuentas.cuenta_ordenante.Length; i < 16; i++)
                                                                        {
                                                                            espacioCuentaOrd += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region 2 CUENTA DE ABONO
                                                                string espacioCuentaProv = "";
                                                                if(proveedor.CUENTA.Length != 20)
                                                                {
                                                                    if(proveedor.CUENTA.Length > 20)
                                                                    {
                                                                        error = "El proveedor " + proveedor.NOMBRE + " tiene una cuenta erronea";
                                                                        completado = false;
                                                                        break;
                                                                    }

                                                                    if(proveedor.CUENTA.Length < 20)
                                                                    {
                                                                        for (int i = proveedor.CUENTA.Length; i < 20; i++)
                                                                        {
                                                                            espacioCuentaProv += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                                  
                                                                //3 BANCO RECEPTOR
                                                                string claveTrans = GET_CLAVE_TRANS(proveedor.BANCO);

                                                                #region 4 BENEFICIARIO
                                                                string espaciosNombProv = "";
                                                                if(proveedor.NOMBRE.Length != 40)
                                                                {
                                                                    if (proveedor.NOMBRE.Length > 40)
                                                                        proveedor.NOMBRE = proveedor.NOMBRE.Substring(0, 40);

                                                                    if (proveedor.NOMBRE.Length < 40)
                                                                    {
                                                                        for (int i = proveedor.NOMBRE.Length; i < 40; i++)
                                                                        {
                                                                            espaciosNombProv += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region 5 IMPORTE
                                                                cerosImporte = "";
                                                                for (int i = creditos[r].IMPORTE_AUTORIZADO.ToString().Length; i < 15; i++)
                                                                {
                                                                    cerosImporte += " ";
                                                                }
                                                                #endregion

                                                                //5 sucursal
                                                                string sucursal = proveedor.NO_SUCURSAL;

                                                                #region 8 CONCEPTO
                                                                string espaciosConcepto = "";

                                                                for (int i = descripcion.Length; i < 130; i++)
                                                                {
                                                                    espaciosConcepto += " ";
                                                                }
                                                                #endregion

                                                                linea = cuentas.cuenta_ordenante + espacioCuentaOrd;                        // 1.- CUENTA CARGO
                                                                linea += proveedor.CUENTA + espacioCuentaProv;                              // 2.- CUENTA ABONO
                                                                linea += claveTrans;                                                        // 3.- BANCO RECEPTOR
                                                                linea += proveedor.NOMBRE + espaciosNombProv;                               // 4.- BENEFICIARIO
                                                                linea += sucursal;                                                          // 5.- SUCURSAL
                                                                linea += creditos[r].IMPORTE_AUTORIZADO.ToString("##.00").Replace(".", "") + cerosImporte;   // 6.- IMPORTE
                                                                linea += "01001";                                                           // 7.- PLAZA BANXICO
                                                                linea += descripcion + espaciosConcepto;                                    // 8.- CONCEPTO
                                                                linea += DateTime.Now.ToString("yyMMdd ");                                // 9.- REFERENCIA ORDENANTE FECHA AÑO MES DIA

                                                                //escribimos en el archivo txt
                                                                layoutTXT.ADD_LINEA(linea);

                                                                break;
                                                            #endregion

                                                            #region Cuentas Santander sin comprobante fiscal
                                                            case "Cuentas Santander sin comprobante fiscal":

                                                                #region 1 CUENTA DE CARGO
                                                                espacioCuentaOrd = "";
                                                                if (cuentas.cuenta_ordenante.Length != 16)
                                                                {
                                                                    if (cuentas.cuenta_ordenante.Length > 16)
                                                                    {
                                                                        error = "La cuenta ordenante esta incorrecta, debe ser de 16 digitos como maximo";
                                                                        completado = false;
                                                                        break;
                                                                    }

                                                                    if (cuentas.cuenta_ordenante.Length < 16)
                                                                    {
                                                                        for (int i = cuentas.cuenta_ordenante.Length; i < 16; i++)
                                                                        {
                                                                            espacioCuentaOrd += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region 2 CUENTA DE ABONO
                                                                espacioCuentaProv = "";
                                                                if (proveedor.CUENTA.Length != 16)
                                                                {
                                                                    if (proveedor.CUENTA.Length > 16)
                                                                    {
                                                                        error = "El proveedor " + proveedor.NOMBRE + " tiene una cuenta erronea";
                                                                        completado = false;
                                                                        break;
                                                                    }

                                                                    if (proveedor.CUENTA.Length < 16)
                                                                    {
                                                                        for (int i = proveedor.CUENTA.Length; i < 16; i++)
                                                                        {
                                                                            espacioCuentaProv += " ";
                                                                        }
                                                                    }
                                                                }
                                                                #endregion

                                                                #region 3 IMPORTE
                                                                cerosImporte = "";
                                                                for (int i = creditos[r].IMPORTE_AUTORIZADO.ToString().Length; i < 13; i++)
                                                                {
                                                                    cerosImporte += "0";
                                                                }
                                                                #endregion

                                                                #region 4 CONCEPTO
                                                                espaciosConcepto = "";

                                                                if (descripcion.Length > 40)
                                                                    descripcion = descripcion.Substring(0, 40);

                                                                for (int i = descripcion.Length; i < 40; i++)
                                                                {
                                                                    espaciosConcepto += " ";
                                                                }
                                                                #endregion

                                                                linea = cuentas.cuenta_ordenante + espacioCuentaOrd;                        // 1.- CUENTA CARGO
                                                                linea += proveedor.CUENTA + espacioCuentaProv;                              // 2.- CUENTA ABONO
                                                                linea += cerosImporte + creditos[r].IMPORTE_AUTORIZADO.ToString("##.00").Replace(".","");   // 3.- IMPORTE
                                                                linea += descripcion + espaciosConcepto;                                    // 4.- CONCEPTO
                                                                linea += DateTime.Now.ToString("ddMMyyyy");                                 // 5.- FECHA DE APLICACION FECHA AÑO MES DIA

                                                                //escribimos en el archivo txt
                                                                layoutTXT.ADD_LINEA(linea);
                                                                break;
                                                                #endregion

                                                            #endregion
                                                        }

                                                        if ((proveedor.CUENTA == "") && (cuentas.formato_layout == "MX - Transferencia a tercero"))
                                                        {
                                                            MessageBox.Show("Falta la cuenta para pagos del proveedor '" + proveedor.NOMBRE + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                            completado = false;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Faltan indicar el banco o la clabe para pagos del proveedor '" + proveedor.NOMBRE + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                        completado = false;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    error = "EL proveedor de datos o el RFC Ordenante estan vacios";
                                                    completado = false;
                                                    break;
                                                }

                                                #endregion
                                            }
                                            else
                                            {
                                                MessageBox.Show("No se encontro el cargo '" + creditos[r].FOLIO_MICROSIP + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                completado = false;
                                                
                                                break;
                                            }
                                        }

                                        // FINALIZA EL LAYOUT DE PAGO A TERCEROS
                                        if (completado)
                                        {
                                            if (cuentas.formato_layout == "MX - Transferencia a tercero")
                                            {
                                                csv.AppendLine("1," + creditos.Length + ",Transferencia");
                                            }
                                        }

                                        C_CONEXIONWEBSVC websvc = new C_CONEXIONWEBSVC();

                                        // SI HAY PAGOS LIBERADOS LOS ACTUALIZA TAMBIEN EN EL PORTAL
                                       /* if (completado)
                                        {
                                            if (liberados.Length > 0)
                                            {
                                                if (!websvc.ActualizaDoctosDetCorporativo(conn_sql, transaction_sql, folio, empresa, liberados))
                                                {
                                                    completado = false;
                                                }
                                            }
                                        }*/

                                        // SI TODO SIGUE CORRECTO SE PROCEDE A LIBERAR EL ENCABEZADO SI YA NO TIENE PAGOS PENDIENTES
                                        if (completado)
                                        {
                                            #region INTENTAMOS ACTUALIZAR EL ENCABEZADO DE LA PROGRAMACIÓN A ESTATUS DE LIBERADO TANTO EN EL PORTAL COMO EN LA SUCURSAL

                                            if (la.verifica_programacion_liberada(conn_sql, transaction_sql, folio))
                                            {
                                                if (websvc.ActualizaDoctosCorporativo(conn_sql, transaction_sql, folio, "L"))
                                                {
                                                    sc = new SqlCommand("UPDATE p_doctos_pr SET estatus_proc = 'L' WHERE folio = '" + folio + "'", conn_sql.SC, transaction_sql);
                                                    sc.ExecuteNonQuery();
                                                    sc.Dispose();

                                                    transaction_sql.Commit();
                                                    transaction_ms.CommitRetaining();

                                                    MessageBox.Show("Proceso terminado satisfactoriamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                                else
                                                {
                                                    transaction_sql.Rollback();
                                                    transaction_ms.RollbackRetaining();

                                                    completado = false;
                                                }
                                            }
                                            else
                                            {
                                                transaction_sql.Commit();
                                                transaction_ms.CommitRetaining();

                                                MessageBox.Show("Proceso terminado satisfactoriamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            // SI "correcto" ES FALSO QUIERE DECIR QUE HUBO UN ERROR AL LEER EL ARCHIVO DEL BANCO
                                            transaction_sql.Rollback();
                                            transaction_ms.RollbackRetaining();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se encontro el concepto '" + cuentas.concepto_cp + "' en la empresa '" + empresa + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        completado = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Hubo un error al procesar uno de los pagos de la programación seleccionada.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    transaction_sql.Rollback();
                                    transaction_ms.RollbackRetaining();
                                    completado = false;
                                }
                                transaction_ms.Dispose();
                                conn_ms.Desconectar();
                            }
                            else
                            {
                                // SI NO SE CONECTA A LA EMPRESA MARCA LA BANDERA COMO FALSO
                                completado = false;
                                error = "No se pudo conectar a la empresa " + empresa;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No hay pagos autorizados en la programación seleccionada.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transaction_sql.Rollback();
                            completado = false;
                        }
                    }
                    else
                    {
                        #region CUALQUIER OTRO ESTATUS QUE NO SEA "A" O "B" NO PUEDE SER PROCESADO

                        if ((estatus_proc == "P") || (estatus_proc == ""))
                        {
                            MessageBox.Show("La programación aun no esta aprobada.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        if (estatus_proc == "R")
                        {
                            MessageBox.Show("La programación fue rechazada en su totalidad.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        if (estatus_proc == "E")
                        {
                            MessageBox.Show("La programación no ha sido autorizada.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        #endregion

                        transaction_sql.Rollback();
                        completado = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubo un error al procesar la programación seleccionada.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    transaction_sql.Rollback();
                    completado = false;
                }

                conn_sql.Desconectar();
            }
            else
            {
                // SI NO SE CONECTA A LA BASE DE DATOS DEL SISTEMA MARCA LA BANDERA COMO FALSO
                completado = false;
                error = "No se pudo realizar la conexión con la base de datos SQL";
            }

            return completado;
        }

        private string GET_CLAVE_TRANS(string bANCO)
        {
            string clave = "";
            C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
            if (reg.LeerRegistros(false))
            {
                C_ConexionSQL con = new C_ConexionSQL();
                if (con.ConectarSQL())
                {
                    string query = "SELECT * FROM P_BANCOS";
                    query += " WHERE NOMBRE = '" + bANCO + "'";
                    SqlCommand sc = new SqlCommand(query, con.SC);
                    SqlDataReader sdr = sc.ExecuteReader();
                    while (sdr.Read())
                    {
                        clave = Convert.ToString(sdr["CLAVE_TRANS"]);
                    }
                }
            }
            return clave;
        }

        private string GET_IVA_PAGO(string folioMsp, int proveedorID, DateTime FechaCargo, string empresa)
        {
            string iva = "0";

            C_CONEXIONFIREBIRD con = new C_CONEXIONFIREBIRD();
            if (con.ConectarFB(empresa))
            {
                FbCommand fb;
                FbDataReader fdr;
                double num = 0;
                string query = "SELECT ";
                query += " COALESCE( sum(idcp.impuesto), 0) as IVA ";
                //query += " ,idcp.PCTJE_IMPUESTO as IVA ";
                query += " FROM doctos_cp cp ";
                query += " join importes_doctos_cp idc on idc.docto_cp_id = cp.docto_cp_id ";
                query += " join importes_doctos_cp_imptos idcp on idcp.impte_docto_cp_id = idc.impte_docto_cp_id ";
                query += " WHERE ";
                query += " cp.folio = '" + folioMsp + "' ";
                query += " AND  cp.proveedor_id = @provID ";
                query += " AND cp.fecha = @FECHA ";
                fb = new FbCommand(query, con.FBC);
                fb.Parameters.Add("@provID",FbDbType.Integer).Value = proveedorID;
                fb.Parameters.Add("@FECHA", FbDbType.Date).Value = FechaCargo;

                fdr = fb.ExecuteReader();
                while (fdr.Read())
                    num = Convert.ToDouble(Convert.ToString(fdr["IVA"]));

                iva = num.ToString("##.00");
                con.Desconectar();
            }

            return iva;
        }

        public bool GenerarCreditos(int docto_pr_id, string folio, string usuario, string concepto_cp, string cuenta_ordenante, string formato_layout, ref StringBuilder csv)
        {
            return false;

            #region FUNCION ORIGINAL DONDE SOLO SE GENERABA EL CREDITO SIN LIBERARLO [COMENTADO]

            /* C_CONEXIONFIREBIRD conn_fb = new C_CONEXIONFIREBIRD();
            C_CONEXIONFIREBIRD conn_ms = new C_CONEXIONFIREBIRD();
            FbTransaction transaction_fb;
            FbTransaction transaction_ms;
            FbCommand cmd;
            FbDataReader read;

            C_AGREGACREDITO[] creditos = new C_AGREGACREDITO[0];

            string select = "";
            string estatus_proc = "";

            bool completado = true;

            if (conn_fb.ConectarFB(registros.FB_BD))
            {
                transaction_fb = conn_fb.FBC.BeginTransaction();

                try
                {
                    cmd = new FbCommand("SELECT * FROM doctos_pr WHERE docto_pr_id = " + docto_pr_id, conn_fb.FBC, transaction_fb);
                    read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        estatus_proc = read["ESTATUS_PROC"].ToString();
                    }
                    read.Close();
                    cmd.Dispose();


                    if ((estatus_proc == "A") || (estatus_proc == "B"))
                    {
                        #region PRIMERO OBTENEMOS TODOS LOS CARGOS DE LA PROGRAMACIÓN SELECCIONADA

                        select = "SELECT ";
                        select += "      dd.*, ";
                        select += "      dp.empresa ";
                        select += " FROM doctos_pr_det dd ";
                        select += " JOIN doctos_pr dp ON(dd.docto_pr_id = dp.docto_pr_id) ";
                        select += "WHERE dd.docto_pr_id = " + docto_pr_id;
                        select += "  AND (dp.estatus_proc = 'A' OR dp.estatus_proc = 'B') ";
                        select += "  AND (dd.estatus = 'A' OR dd.estatus = 'B') ";

                        cmd = new FbCommand(select, conn_fb.FBC, transaction_fb);
                        read = cmd.ExecuteReader();
                        while (read.Read())
                        {
                            Array.Resize(ref creditos, creditos.Length + 1);
                            creditos[creditos.Length - 1] = new C_AGREGACREDITO();

                            creditos[creditos.Length - 1].DOCTO_PR_DET_ID = Convert.ToInt32(read["DOCTO_PR_DET_ID"].ToString());
                            creditos[creditos.Length - 1].DOCTO_PR_ID = Convert.ToInt32(read["DOCTO_PR_ID"].ToString());
                            creditos[creditos.Length - 1].FOLIO_MICROSIP = read["FOLIO_MICROSIP"].ToString();
                            creditos[creditos.Length - 1].FECHA_CARGO = Convert.ToDateTime(read["FECHA_CARGO"].ToString());
                            creditos[creditos.Length - 1].PROVEEDOR_ID = Convert.ToInt32(read["PROVEEDOR_ID"].ToString());
                            creditos[creditos.Length - 1].PROVEEDOR_CLAVE = read["PROVEEDOR_CLAVE"].ToString();
                            creditos[creditos.Length - 1].PROVEEDOR_NOMBRE = read["PROVEEDOR_NOMBRE"].ToString();
                            creditos[creditos.Length - 1].FECHA_VENCIMIENTO = Convert.ToDateTime(read["FECHA_VENCIMIENTO"].ToString());
                            creditos[creditos.Length - 1].IMPORTE_PAGOS = Convert.ToDouble(read["IMPORTE_PAGOS"].ToString());
                            creditos[creditos.Length - 1].IMPORTE_AUTORIZADO = Convert.ToDouble(read["IMPORTE_AUTORIZADO"].ToString());
                            creditos[creditos.Length - 1].TIPO = read["TIPO"].ToString();
                            creditos[creditos.Length - 1].ESTATUS = read["ESTATUS"].ToString();
                            creditos[creditos.Length - 1].EMPRESA = read["EMPRESA"].ToString();
                            creditos[creditos.Length - 1].FOLIO_CREDITO = read["FOLIO_CREDITO"].ToString();
                        }
                        read.Close();
                        cmd.Dispose();

                        #endregion

                        if (creditos.Length > 0)
                        {
                            string empresa = creditos[0].EMPRESA;

                            if (conn_ms.ConectarFB(empresa))
                            {
                                transaction_ms = conn_ms.FBC.BeginTransaction();

                                try
                                {
                                    int concepto_cp_id = OBTENER_CONCEPTO_CP_ID(conn_ms, transaction_ms, concepto_cp);

                                    if (concepto_cp_id != 0)
                                    {
                                        for (int r = 0; r < creditos.Length; r++)
                                        {
                                            string descripcion = "";
                                            int docto_cp_acr_id = OBTENER_DOCTO_CP_ID(conn_ms, transaction_ms, creditos[r].FOLIO_MICROSIP, creditos[r].PROVEEDOR_ID, ref descripcion);

                                            if ((docto_cp_acr_id != 0) || (creditos[r].TIPO == "A"))
                                            {
                                                if (estatus_proc == "A")
                                                {
                                                    string folio_credito = "";

                                                    if (NuevoCredito(conn_ms, transaction_ms, concepto_cp_id, creditos[r].PROVEEDOR_ID, DateTime.Now, creditos[r].IMPORTE_AUTORIZADO, creditos[r].TIPO, docto_cp_acr_id, descripcion, usuario, ref folio_credito))
                                                    {
                                                        // ACTUALIZAMOS EL CREDITO PARA INDICAR QUE YA FUE GENERADO SU ARCHIVO
                                                        cmd = new FbCommand("UPDATE doctos_pr_det SET estatus = 'B', folio_credito = '" + folio_credito + "' WHERE docto_pr_det_id = " + creditos[r].DOCTO_PR_DET_ID, conn_fb.FBC, transaction_fb);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();

                                                        creditos[r].FOLIO_CREDITO = folio_credito;
                                                    }
                                                    else
                                                    {
                                                        completado = false;
                                                        break;
                                                    }
                                                }

                                                #region VALIDAMOS Y AGREGAMOS EL O LOS RENGLONES AL ARCHIVO DE BANCOS

                                                Proveedor proveedor = new Proveedor();

                                                string rfc_ordenante = OBTENER_RFC_EMPRESA(conn_ms, transaction_ms);
                                                string linea = "";

                                                if (OBTENER_PROVEEDOR(conn_ms, transaction_ms, creditos[r].PROVEEDOR_ID, ref proveedor) && (rfc_ordenante != ""))
                                                {
                                                    if ((proveedor.CUENTA != "") && (proveedor.BANCO != "") && (proveedor.CLABE != ""))
                                                    {
                                                        string clave_banco = GET_CLAVE_BANCO(proveedor.BANCO);

                                                        if (clave_banco != "")
                                                        {
                                                            rfc_ordenante = rfc_ordenante.Replace(".", "").Replace(",", "").Replace("-", "").Replace(" ", "");
                                                            proveedor.NOMBRE = proveedor.NOMBRE.Replace(".", "").Replace(",", "").Replace("-", "");
                                                            if (proveedor.NOMBRE.Length > 30)
                                                            {
                                                                proveedor.NOMBRE = proveedor.NOMBRE.Substring(0, 30);
                                                            }
                                                            proveedor.RFC_CURP = proveedor.RFC_CURP.Replace(".", "").Replace(",", "").Replace("-", "").Replace(" ", "");

                                                            // LA CLABE DEBE TENER 20 CARACTERES EN CASO DE QUE FALTEN SE RELLENAN CON CEROS A LA IZQUIERDA
                                                            while (proveedor.CLABE.Length < 20)
                                                            {
                                                                proveedor.CLABE = "0" + proveedor.CLABE;
                                                            }

                                                            switch (formato_layout)
                                                            {
                                                                case "MX - SPEI":
                                                                    #region LAYOUT MX - SPEI

                                                                    linea = cuenta_ordenante + ","; //************************************* Cuenta Ordenante
                                                                    linea += rfc_ordenante + ","; //*************************************** RFC del Ordenante
                                                                    linea += proveedor.CLABE + ","; //************************************* Cuenta Beneficiaria
                                                                    linea += clave_banco + ","; //***************************************** Banco Beneficiario
                                                                    linea += proveedor.NOMBRE + ","; //************************************ Nombre del Beneficiario
                                                                    linea += creditos[r].IMPORTE_AUTORIZADO.ToString("#.00") + ","; //***** Monto
                                                                    linea += docto_cp_acr_id + ","; //************************************* Referencia Numérica
                                                                    linea += folio + " " + creditos[r].FOLIO_CREDITO + ","; //************* Concepto de Pago
                                                                    linea += creditos[r].FOLIO_CREDITO; //********************************* Referencia del Ordenante

                                                                    csv.AppendLine(linea);

                                                                    #endregion
                                                                    break;
                                                                case "MX - Transferencia TEF local":
                                                                    #region LAYOUT MX - Transferencia TEF local

                                                                    linea = cuenta_ordenante + ","; //************************************* Cuenta ordenante
                                                                    linea += DateTime.Now.AddDays(1).ToString("ddMMyyyy") + ","; //******** Fecha valor
                                                                    linea += proveedor.RFC_CURP + ","; //********************************** RFC
                                                                    linea += proveedor.CLAVE_FISCAL + ","; //****************************** Moneda
                                                                    linea += descripcion + ""; //****************************************** Número de Lote

                                                                    csv.AppendLine(linea);

                                                                    linea = "CLA,"; //***************************************************** Tipo de Cuenta
                                                                    linea += clave_banco + ","; //***************************************** Código de banco
                                                                    linea += proveedor.CLABE + ","; //************************************* Cuenta Beneficiaria
                                                                    linea += proveedor.NOMBRE + ","; //************************************ Nombre del Beneficiario
                                                                    linea += creditos[r].IMPORTE_AUTORIZADO.ToString("#.00") + ","; //***** Monto
                                                                    linea += docto_cp_acr_id + ","; //************************************* Referencia Numérica
                                                                    linea += folio + " " + creditos[r].FOLIO_CREDITO + ","; //************* Referencia Alfanumérica
                                                                    linea += ","; //******************************************************* RFC del Beneficiario*
                                                                    linea += ","; //******************************************************* IVA*
                                                                    linea += ""; //******************************************************** Correo Electrónico del Beneficiario*

                                                                    csv.AppendLine(linea);

                                                                    #endregion
                                                                    break;
                                                                case "MX - Transferencia a tercero":
                                                                    #region LAYOUT MX - Transferencia a tercero

                                                                    linea = (r + 1) + ","; //****************************************************************** Número de transacciones
                                                                    linea += cuenta_ordenante + ","; //******************************************************** Cuenta Ordenante
                                                                    linea += proveedor.CUENTA + ","; //******************************************************** Cuenta Beneficiaria
                                                                    linea += creditos[r].IMPORTE_AUTORIZADO.ToString("#.00") + ","; //************************* Monto
                                                                    linea += (proveedor.CLAVE_FISCAL == "MXN" ? "1" : "2") + ","; //*************************** Moneda
                                                                    linea += folio + " " + creditos[r].FOLIO_CREDITO + " " + docto_cp_acr_id + ","; //********* Referencia*
                                                                    linea += proveedor.NOMBRE + ","; //******************************************************** Nombre del Beneficiario
                                                                    linea += "0,"; //************************************************************************** Comprobante Fiscar (CF)
                                                                    linea += proveedor.RFC_CURP + ","; //****************************************************** RFC del Beneficiario
                                                                    linea += ","; //*************************************************************************** IVA*
                                                                    linea += ""; //**************************************************************************** Correo Electrónico del Beneficiario*

                                                                    csv.AppendLine(linea);

                                                                    #endregion
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("No se encontro la clave fiscal del banco '" + proveedor.BANCO + "' en el proveedor '" + proveedor.NOMBRE + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                            completado = false;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Faltan los datos para pagos del proveedor '" + proveedor.NOMBRE + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                        completado = false;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    completado = false;
                                                    break;
                                                }

                                                #endregion
                                            }
                                            else
                                            {
                                                MessageBox.Show("No se encontro el cargo '" + creditos[r].FOLIO_MICROSIP + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                completado = false;
                                                break;
                                            }
                                        }

                                        // LAYOUT PAGO A TERCEROS
                                        if (completado)
                                        {
                                            if (formato_layout == "MX - Transferencia a tercero")
                                            {
                                                csv.AppendLine("1," + creditos.Length + ",Transferencia");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se encontro el concepto '" + concepto_cp + "' en la empresa '" + empresa + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        completado = false;
                                    }

                                    #region FINALIZA Y APLICA LOS CAMBIOS SI NO HUBO ERROR O LOS DESHACE SI HUBO ALGUN ERROR
                                    if (completado)
                                    {
                                        C_CONEXIONWEBSVC websvc = new C_CONEXIONWEBSVC();

                                        if (websvc.ActualizaDoctosCorporativo(conn_fb, transaction_fb, folio, "B"))
                                        {
                                            // ACTUALIZAMOS LA PROGRAMACIÓN A ESTATOS DE BANCOS
                                            cmd = new FbCommand("UPDATE doctos_pr SET estatus_proc = 'B' WHERE docto_pr_id = " + docto_pr_id, conn_fb.FBC, transaction_fb);
                                            cmd.ExecuteNonQuery();
                                            cmd.Dispose();

                                            transaction_fb.CommitRetaining();
                                            transaction_ms.CommitRetaining();

                                            MessageBox.Show("Proceso terminado satisfactoriamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            transaction_fb.RollbackRetaining();
                                            transaction_ms.RollbackRetaining();
                                            completado = false;
                                        }
                                    }
                                    else
                                    {
                                        transaction_fb.RollbackRetaining();
                                        transaction_ms.RollbackRetaining();
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Hubo un error al procesar uno de los pagos de la programación seleccionada.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    transaction_fb.RollbackRetaining();
                                    transaction_ms.RollbackRetaining();
                                    completado = false;
                                }

                                conn_ms.Desconectar();
                            }
                            else
                            {
                                // SI NO SE CONECTA A LA EMPRESA MARCA LA BANDERA COMO FALSO
                                completado = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No hay pagos autorizados en la programación seleccionada.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transaction_fb.RollbackRetaining();
                            completado = false;
                        }
                    }
                    else
                    {
                        if ((estatus_proc == "P") || (estatus_proc == ""))
                        {
                            MessageBox.Show("La programación aun no esta aprobada.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        if (estatus_proc == "R")
                        {
                            MessageBox.Show("La programación fue rechazada en su totalidad.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        if (estatus_proc == "E")
                        {
                            MessageBox.Show("La programación no ha sido autorizada.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        transaction_fb.RollbackRetaining();
                        completado = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubo un error al procesar la programación seleccionada.\n\n" + ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    transaction_fb.RollbackRetaining();
                    completado = false;
                }

                conn_fb.Desconectar();
            }
            else
            {
                // SI NO SE CONECTA A LA BASE DE DATOS DEL SISTEMA MARCA LA BANDERA COMO FALSO
                completado = false;
            }

            return completado; // */

            #endregion
        }

    }
}
