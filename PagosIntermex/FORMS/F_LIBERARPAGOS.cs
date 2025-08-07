using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;

namespace PagosIntermex
{
    public partial class F_LIBERARPAGOS : Form
    {
        private C_EMPRESAS[] empresa;
        private string doctopr = "";
        private string archivo = "";

        public F_LIBERARPAGOS()
        {
            InitializeComponent();
        }

        private class CuentasBancarias
        {
            public CuentasBancarias(int cuenta_ban_id, string nombre, string num_cuenta, int moneda_id)
            {
                CUENTA_BAN_ID = cuenta_ban_id;
                NOMBRE = nombre;
                NUM_CUENTA = num_cuenta;
                MONEDA_ID = moneda_id;
            }

            public int CUENTA_BAN_ID { get; set; }

            public string NOMBRE { get; set; }

            public string NUM_CUENTA { get; set; }

            public int MONEDA_ID { get; set; }

            public override string ToString()
            {
                return NOMBRE;
            }
        }

        private class ConceptosBancos
        {
            public ConceptosBancos(int concepto_ba_id, string nombre)
            {
                CONCEPTO_BA_ID = concepto_ba_id;
                NOMBRE = nombre;
            }

            public int CONCEPTO_BA_ID { get; set; }
            public string NOMBRE { get; set; }

            public override string ToString()
            {
                return NOMBRE;
            }
        }

        public C_EMPRESAS[] EMPRESA
        {
            set { empresa = value; }
            get { return empresa; }
        }

        public bool LIBERADOS { get; set; }

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





        private void F_LIBERARPAGOS_Shown(object sender, EventArgs e)
        {
            if (empresa != null)
            {
                #region CARGAMOS LOS COMBOBOX
                C_CONEXIONFIREBIRD conn = new C_CONEXIONFIREBIRD();
                FbCommand cmd;
                FbDataReader read;

                if (conn.ConectarFB(empresa[0].NOMBRE_CORTO))
                {
                    try
                    {
                        comboCuenta.Items.Clear();
                        comboCuenta.Items.Add(new CuentasBancarias(0, "Seleccionar...", "", 0));
                        comboCuenta.SelectedIndex = 0;

                        comboConcepto.Items.Clear();
                        comboConcepto.Items.Add(new ConceptosBancos(0, "Seleccionar..."));
                        comboConcepto.SelectedIndex = 0;

                        string select = "";

                        select = "SELECT ";
                        select += "      cb.cuenta_ban_id, ";
                        select += "      cb.nombre, ";
                        select += "      cb.num_cuenta, ";
                        select += "      cb.moneda_id ";
                        select += " FROM cuentas_bancarias cb ";
                        select += " JOIN bancos b ON(cb.banco_id = b.banco_id) ";
                        select += "WHERE b.clave_fiscal = '021'";

                        cmd = new FbCommand(select, conn.FBC);
                        read = cmd.ExecuteReader();
                        while (read.Read())
                        {
                            comboCuenta.Items.Add(new CuentasBancarias(Convert.ToInt32(read["CUENTA_BAN_ID"].ToString()), read["NOMBRE"].ToString(), read["NUM_CUENTA"].ToString(), Convert.ToInt32(read["MONEDA_ID"].ToString())));
                        }
                        read.Close();
                        cmd.Dispose();

                        cmd = new FbCommand("SELECT * FROM conceptos_ba WHERE naturaleza = 'R' AND tipo_mov_fiscal = 'T'", conn.FBC);
                        read = cmd.ExecuteReader();
                        while (read.Read())
                        {
                            comboConcepto.Items.Add(new ConceptosBancos(Convert.ToInt32(read["CONCEPTO_BA_ID"].ToString()), read["NOMBRE"].ToString()));
                        }
                        read.Close();
                        cmd.Dispose();

                        // comboFormato.Enabled = false;
                        // btnSeleccionar.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    conn.Desconectar();
                }
                #endregion
            }
        }





        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            if (comboFormato.SelectedIndex >= 0)
            {
                string formato = comboFormato.Text;

                // empresa = "";

                OpenFileDialog op = new OpenFileDialog();
                if (op.ShowDialog() == DialogResult.OK)
                {
                    archivo = op.FileName;

                    try
                    {
                        using (var reader = new StreamReader(@op.FileName))
                        {
                            int ren = 0;
                            int renglones = 0; // MX - Transferencia a tercero

                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(',');

                                switch (formato)
                                {
                                    case "MX - SPEI":
                                        #region MX - SPEI
                                        if (values.Length == 9)
                                        {
                                            try
                                            {
                                                /* var aux = values[7].Replace("\"", "").Split('-');
                                                empresa = aux[0];
                                                doctopr = aux[1]; */

                                                var aux = values[7].Replace("\"", "").Split(' ');
                                                doctopr = aux[0];
                                            }
                                            catch
                                            {
                                                MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                // empresa = "";
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            // empresa = "";
                                            return;
                                        }
                                        #endregion
                                        break;
                                    case "MX - Transferencia TEF local":
                                        #region MX - Transferencia TEF local
                                        if (ren == 0 && values.Length != 5)
                                        {
                                            MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            // empresa = "";
                                            return;
                                        }

                                        if (values.Length == 10)
                                        {
                                            /* var aux = values[6].Replace("\"", "").Split('-');
                                            empresa = aux[0];
                                            doctopr = aux[1]; */

                                            var aux = values[6].Replace("\"", "").Split(' ');
                                            doctopr = aux[0];
                                        }
                                        #endregion
                                        break;
                                    case "MX - Transferencia a tercero":
                                        #region MX - Transferencia a tercero
                                        if (values.Length == 11)
                                        {
                                            try
                                            {
                                                /* var aux = values[5].Replace("\"", "").Split('-');
                                                empresa = aux[0];
                                                doctopr = aux[1]; */

                                                var aux = values[5].Replace("\"", "").Split(' ');
                                                doctopr = aux[0];
                                            }
                                            catch
                                            {
                                                MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                // empresa = "";
                                                return;
                                            }
                                        }

                                        if (values.Length == 3)
                                        {
                                            renglones = Convert.ToInt32(values[1].Replace("\"", ""));
                                        }
                                        #endregion
                                        break;
                                }

                                ren++;
                            }

                            if (formato == "MX - Transferencia a tercero")
                            {
                                if (ren != (renglones + 1))
                                {
                                    MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    // empresa = "";
                                    return;
                                }
                            }
                        }

                        /* if (empresa != "")
                        {
                            #region CARGAMOS LOS COMBOBOX
                            C_CONEXIONFIREBIRD conn = new C_CONEXIONFIREBIRD();
                            FbCommand cmd;
                            FbDataReader read;

                            if (conn.ConectarFB(empresa))
                            {
                                try
                                {
                                    comboCuenta.Items.Clear();
                                    comboCuenta.Items.Add(new CuentasBancarias(0, "Seleccionar...", "", 0));
                                    comboCuenta.SelectedIndex = 0;

                                    comboConcepto.Items.Clear();
                                    comboConcepto.Items.Add(new ConceptosBancos(0, "Seleccionar..."));
                                    comboConcepto.SelectedIndex = 0;

                                    string select = "";

                                    select = "SELECT ";
                                    select += "      cb.cuenta_ban_id, ";
                                    select += "      cb.nombre, ";
                                    select += "      cb.num_cuenta, ";
                                    select += "      cb.moneda_id ";
                                    select += " FROM cuentas_bancarias cb ";
                                    select += " JOIN bancos b ON(cb.banco_id = b.banco_id) ";
                                    select += "WHERE b.clave_fiscal = '021'";

                                    cmd = new FbCommand(select, conn.FBC);
                                    read = cmd.ExecuteReader();
                                    while (read.Read())
                                    {
                                        comboCuenta.Items.Add(new CuentasBancarias(Convert.ToInt32(read["CUENTA_BAN_ID"].ToString()), read["NOMBRE"].ToString(), read["NUM_CUENTA"].ToString(), Convert.ToInt32(read["MONEDA_ID"].ToString())));
                                    }
                                    read.Close();
                                    cmd.Dispose();

                                    cmd = new FbCommand("SELECT * FROM conceptos_ba WHERE naturaleza = 'R' AND tipo_mov_fiscal = 'T'", conn.FBC);
                                    read = cmd.ExecuteReader();
                                    while (read.Read())
                                    {
                                        comboConcepto.Items.Add(new ConceptosBancos(Convert.ToInt32(read["CONCEPTO_BA_ID"].ToString()), read["NOMBRE"].ToString()));
                                    }
                                    read.Close();
                                    cmd.Dispose();

                                    comboFormato.Enabled = false;
                                    btnSeleccionar.Enabled = false;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                conn.Desconectar();
                            }
                            #endregion
                        } */
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Favor de indicar el formato del archivo que va a seleccionar.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if ((comboCuenta.SelectedIndex > 0) && (comboConcepto.SelectedIndex > 0))
            {
                CuentasBancarias cuenta = comboCuenta.SelectedItem as CuentasBancarias;
                ConceptosBancos concepto = comboConcepto.SelectedItem as ConceptosBancos;

                string formato = comboFormato.Text;
                string select = "";

                C_CONEXIONFIREBIRD conn = new C_CONEXIONFIREBIRD();
                FbTransaction transaction;
                FbCommand cmd;
                FbDataReader read;
                SqlCommand sc;
                SqlDataReader sdr;

                C_ConexionSQL conn_sql = new C_ConexionSQL();
                SqlTransaction transaction_sql;

                C_REGISTROSWINDOWS registros = new C_REGISTROSWINDOWS();
                registros.LeerRegistros(false);

                bool correcto = true;

                if (conn_sql.ConectarSQL())
                {
                    if (conn.ConectarFB(empresa[0].NOMBRE_CORTO))
                    {
                        transaction_sql = conn_sql.SC.BeginTransaction(); // INICIA LA TRANSACCIÓN CON LA BASE DE DATOS DEL SISTEMA
                        transaction = conn.FBC.BeginTransaction(); // INICIA LA TRANSACCIÓN CON LA BASE DE DATOS DE LA EMPRESA

                        try
                        {
                           // Services.RegistrosLiberados[] liberados = new Services.RegistrosLiberados[0];

                            C_LIBERARCREDITO la = new C_LIBERARCREDITO();

                            using (var reader = new StreamReader(@archivo))
                            {
                                int ren = 0;

                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    var values = line.Split(',');

                                    int docto_cp_id = 0;

                                    string folio = "";
                                    string clabe = "";
                                    string banco = "";
                                    string estatus = "";
                                    string aplicado = "";

                                    double importe = 0;

                                    switch (formato)
                                    {
                                        case "MX - SPEI":
                                            #region MX - SPEI
                                            if (values.Length == 9)
                                            {
                                                try
                                                {
                                                    var aux = values[7].Replace("\"", "").Split(' ');
                                                    folio = aux[1];

                                                    importe = Convert.ToDouble(values[5].Replace("\"", ""));
                                                }
                                                catch
                                                {
                                                    MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    correcto = false;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                correcto = false;
                                            }
                                            #endregion
                                            break;
                                        case "MX - Transferencia TEF local":
                                            #region MX - Transferencia TEF local
                                            if (ren == 0 && values.Length != 5)
                                            {
                                                MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                correcto = false;
                                            }

                                            if (values.Length == 10)
                                            {
                                                var aux = values[6].Replace("\"", "").Split(' ');
                                                folio = aux[1];

                                                importe = Convert.ToDouble(values[4].Replace("\"", ""));
                                            }
                                            #endregion
                                            break;
                                        case "MX - Transferencia a tercero":
                                            #region MX - Transferencia a tercero
                                            if (values.Length == 11)
                                            {
                                                try
                                                {
                                                    var aux = values[5].Replace("\"", "").Split(' ');
                                                    folio = aux[1];

                                                    importe = Convert.ToDouble(values[3].Replace("\"", ""));
                                                }
                                                catch
                                                {
                                                    MessageBox.Show("Formato invalido.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    correcto = false;
                                                }
                                            }
                                            #endregion
                                            break;
                                    }

                                    ren++;

                                    if (correcto)
                                    {
                                        if (folio != "")
                                        {
                                            #region OBTENEMOS EL ID, EL ESTATUS DEL CREDITO Y LOS DATOS DEL PROVEEDOR

                                            select = "SELECT ";
                                            select += "      dc.docto_cp_id, ";
                                            select += "      dc.proveedor_id, ";
                                            select += "      lp.clabe, ";
                                            select += "      lp.banco, ";
                                            select += "      db.estatus, ";
                                            select += "      db.aplicado ";
                                            select += " FROM doctos_cp dc ";
                                            select += " JOIN libres_proveedor lp ON(dc.proveedor_id = lp.proveedor_id) ";
                                            select += " LEFT JOIN doctos_entre_sis de ON(dc.docto_cp_id = de.docto_fte_id) ";
                                            select += " LEFT JOIN doctos_ba db ON(de.docto_dest_id = db.docto_ba_id) ";
                                            select += "WHERE dc.folio = '" + folio + "' ";
                                            select += "  AND dc.naturaleza_concepto = 'R' ";
                                            select += "  AND de.clave_sis_dest = 'BA' ";

                                            cmd = new FbCommand(select, conn.FBC, transaction);
                                            read = cmd.ExecuteReader();
                                            while (read.Read())
                                            {
                                                docto_cp_id = Convert.ToInt32(read["DOCTO_CP_ID"].ToString());
                                                clabe = read["CLABE"].ToString();
                                                banco = GET_CLAVE_BANCO(read["BANCO"].ToString());
                                                estatus = read["ESTATUS"].ToString();
                                                aplicado = read["APLICADO"].ToString();
                                            }
                                            read.Close();
                                            cmd.Dispose();

                                            #endregion

                                            if ((estatus == "P") && (aplicado == "N"))
                                            {
                                                if ((docto_cp_id != 0) && (clabe != "") && (banco != ""))
                                                {
                                                    if (la.Liberar(conn, transaction, docto_cp_id, importe, cuenta.CUENTA_BAN_ID, cuenta.MONEDA_ID, DateTime.Now, concepto.CONCEPTO_BA_ID, clabe, banco, folio))
                                                    {
                                                        // ACTUALIZAMOS EL RENGLON A LIBERADO Y GUARDAMOS EL FOLIO DEL CARGO PARA POSTERIORMENTE LIBERARLO EN EL PORTAL
                                                        if (!la.Liberar_Detalle(conn_sql, transaction_sql, folio, doctopr))
                                                        {
                                                            correcto = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        correcto = false;
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No se encontro el pago '" + folio + "'.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("El pago '" + folio + "' ya estaba liberado en Microsip.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                                // ACTUALIZAMOS EL RENGLON A LIBERADO Y GUARDAMOS EL FOLIO DEL CARGO PARA POSTERIORMENTE LIBERARLO EN EL PORTAL
                                                if (!la.Liberar_Detalle(conn_sql, transaction_sql, folio, doctopr))
                                                {
                                                    correcto = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            correcto = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            /*/ SI HAY PAGOS LIBERADOS LOS ACTUALIZA TAMBIEN EN EL PORTAL
                            if (correcto)
                            {
                                C_CONEXIONWEBSVC websvc = new C_CONEXIONWEBSVC();

                                if (liberados.Length > 0)
                                {
                                    if (!websvc.ActualizaDoctosDetCorporativo(conn_sql, transaction_sql, doctopr, empresa, liberados))
                                    {
                                        correcto = false;
                                    }
                                }
                            }*/

                            // SI TODO SIGUE CORRECTO
                            if (correcto)
                            {
                                #region INTENTAMOS ACTUALIZAR EL ENCABEZADO DE LA PROGRAMACIÓN A ESTATUS DE LIBERADO TANTO EN EL PORTAL COMO EN LA SUCURSAL

                                if (la.verifica_programacion_liberada(conn_sql, transaction_sql, doctopr))
                                {
                                    C_CONEXIONWEBSVC websvc = new C_CONEXIONWEBSVC();

                                    if (websvc.ActualizaDoctosCorporativo(conn_sql, transaction_sql, doctopr, "L"))
                                    {
                                        sc = new SqlCommand("UPDATE doctos_pr SET estatus_proc = 'L' WHERE folio = '" + doctopr + "'", conn_sql.SC, transaction_sql);
                                        sc.ExecuteNonQuery();
                                        sc.Dispose();

                                        transaction_sql.Commit();
                                        transaction.CommitRetaining();

                                        MessageBox.Show("Proceso terminado satisfactoriamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        transaction_sql.Rollback();
                                        transaction.RollbackRetaining();

                                        correcto = false;
                                    }
                                }
                                else
                                {
                                    transaction_sql.Commit();
                                    transaction.CommitRetaining();

                                    MessageBox.Show("Proceso terminado satisfactoriamente.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                #endregion
                            }
                            else
                            {
                                // SI "correcto" ES FALSO QUIERE DECIR QUE HUBO UN ERROR AL LEER EL ARCHIVO DEL BANCO
                                transaction_sql.Rollback();
                                transaction.RollbackRetaining();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            transaction_sql.Rollback();
                            transaction.RollbackRetaining();

                            correcto = false;
                        }

                        conn.Desconectar();
                    }
                    else
                    {
                        // NO SE PUDO CONECTAR A LA BASE DE DATOS DE LA EMPRESA
                        correcto = false;
                    }

                    conn_sql.Desconectar();
                }
                else
                {
                    // NO SE PUDO CONECTAR A LA BASE DE DATOS DEL SISTEMA
                    correcto = false;
                }

                if (correcto)
                {
                    LIBERADOS = true;
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Faltan datos por especificar.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();

            // comboFormato.Enabled = true;
            // btnSeleccionar.Enabled = true;

            // comboCuenta.Items.Clear();
            // comboConcepto.Items.Clear();
        }

    }
}
