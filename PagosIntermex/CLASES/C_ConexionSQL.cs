using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PagosIntermex
{
    class C_ConexionSQL
    {
        private const string caption = "Mensaje de aplicación";
        
        private string conectionString;
        private SqlConnection sc;        
        private C_REGISTROSWINDOWS reg;

        public C_ConexionSQL()
        {

        }

        public SqlConnection SC
        {
            set { sc = value; }
            get { return sc; }
        }

        public C_REGISTROSWINDOWS REG
        {
            set { reg = value; }
            get { return reg; }
        }





        public bool ConectarSQL()
        {
            reg = new C_REGISTROSWINDOWS();

            try
            {
                if (reg.LeerRegistros(true))
                {
                    conectionString = @"Data Source=" + reg.SQL_SERV + "; ";
                    conectionString += "Initial Catalog=" + reg.SQL_DATA + "; ";
                    conectionString += "User Id=" + reg.SQL_USER + "; ";
                    conectionString += "Password=" + reg.SQL_PASS + "; ";
                    conectionString += "Trusted_Connection=False; ";
                    conectionString += "MultipleActiveResultSets=True; ";
                    conectionString += "MultiSubnetFailover=Yes; ";
                    conectionString += "Asynchronous Processing=true; ";

                    sc = new SqlConnection(conectionString);
                    sc.Open();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible establecer conexión con el servidor.\n\n" + ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool ConectarSQL(string serv, string bd, string user, string pass)
        {
            try
            {
                conectionString = @"Data Source=" + serv + "; ";
                conectionString += "Initial Catalog=" + bd + "; ";
                conectionString += "User Id=" + user + "; ";
                conectionString += "Password=" + pass + "; ";
                conectionString += "Trusted_Connection=False; ";
                conectionString += "MultipleActiveResultSets=True; ";
                conectionString += "MultiSubnetFailover=Yes; ";
                conectionString += "Asynchronous Processing=true; ";

                
                sc = new SqlConnection(conectionString);
                sc.Open();

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible establecer conexión con el servidor.\n\n" + ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void Desconectar()
        {
            sc.Close();
        }

    }
}
