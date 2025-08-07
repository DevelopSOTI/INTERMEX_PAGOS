using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagosIntermex
{
    class C_CONEXIONFIREBIRD
    {
        private string conectionString;
        private string user, password, database, root, server;

        private FbConnection fbc;
        private C_REGISTROSWINDOWS reg;

        public C_CONEXIONFIREBIRD()
        {
            user = password = database = root = server = "";
        }

        public string USER
        {
            set { user = value; }
            get { return user; }
        }

        public string PASSWORD
        {
            set { password = value; }
            get { return password; }
        }

        public string DATABASE
        {
            set { database = value; }
            get { return database; }
        }

        public string ROOT
        {
            set { root = value; }
            get { return root; }
        }

        public string SERVER
        {
            set { server = value; }
            get { return server; }
        }

        public string CONECTIONSTRING
        {
            set { conectionString = value; }
            get { return conectionString; }
        }

        public FbConnection FBC
        {
            set { fbc = value; }
            get { return fbc; }
        }

        public C_REGISTROSWINDOWS REG
        {
            set { reg = value; }
            get { return reg; }
        }

        /*public bool ConectarFB(string db)
        {
            try
            {
                reg = new C_REGISTROSWINDOWS();
                reg.LeerRegistros(false);

                conectionString = @"User=" + /*reg.FB_USUARIO* "SYSDBA" + "; Password=" + reg.FB_PASSWORD
                        + "; Database=" + reg.FB_ROOT + "\\" + db + ".FDB"
                        + "; Datasource=" + reg.FB_SERVIDOR + "; Dialect=3" + "; Charset=ISO8859_1"; // *

                fbc = new FbConnection(conectionString);
                fbc.Open();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }*/
        public bool ConectarFB(string empresa)
        {
            try
            {
                reg = new C_REGISTROSWINDOWS();

                if (reg.LeerRegistros(true))
                {
                    conectionString = @"User=SYSDBA;";
                    conectionString += "Password=" + reg.FB_PASSWORD + ";";
                    conectionString += "Database=" + reg.FB_ROOT + "\\" + empresa + ".FDB" + ";";
                    conectionString += "Datasource=" + reg.FB_SERVIDOR + ";";
                    conectionString += "Dialect=3;";
                    conectionString += "Charset=ISO8859_1;";

                    fbc = new FbConnection(conectionString);
                    fbc.Open();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible establecer conexión con la empresa '" + empresa + "'.\n\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        public bool ConectarFB(string empresa,bool band)
        {
            try
            {
                reg = new C_REGISTROSWINDOWS();

                if (reg.LeerRegistros(true))
                {
                    conectionString = @"User=SYSDBA;";
                    conectionString += "Password=" + reg.FB_PASSWORD + ";";
                    conectionString += "Database=" + reg.FB_ROOT + "\\" + empresa + ".FDB" + ";";
                    conectionString += "Datasource=" + reg.FB_SERVIDOR + ";";
                    conectionString += "Dialect=3;";
                    conectionString += "Charset=ISO8859_1;";

                    fbc = new FbConnection(conectionString);
                    fbc.Open();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if(band)
                MessageBox.Show("No fue posible establecer conexión con la empresa '" + empresa + "'.\n\n" + ex.Message, "Mensaje de la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }
        public bool ConectarFB_MANUAL(string USUARIO, string PASSWORD, string ROOT, string DATABASE, string SERVER)
        {
            try
            {
                conectionString = @"User=" + USUARIO + "; Password=" + PASSWORD
                        + "; Database=" + ROOT + "\\" + DATABASE + ".FDB"
                        + "; Datasource=" + SERVER + "; Dialect=3" + "; Charset=ISO8859_1"; // */

                fbc = new FbConnection(conectionString);
                fbc.Open();

                return true;
            }
            catch
            {
                MessageBox.Show("Error al conectar con los registros seleccionados: Por favor, verifique los parametros sean los indicados.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool ConectarFB_MANUAL(string USUARIO, string PASSWORD, string ROOT, string SERVER)
        {
            try
            {
                conectionString = @"User=" + USUARIO + "; Password=" + PASSWORD
                        + "; Database=" + ROOT + "\\System\\Config.FDB"
                        + "; Datasource=" + SERVER + "; Dialect=3" + "; Charset=ISO8859_1"; // */

                fbc = new FbConnection(conectionString);
                fbc.Open();

                return true;
            }
            catch
            {
                MessageBox.Show("Error al conectar con los registros seleccionados: Por favor, verifique los parametros sean los indicados.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void Desconectar()
        {
            fbc.Close();
        }

    }
}
