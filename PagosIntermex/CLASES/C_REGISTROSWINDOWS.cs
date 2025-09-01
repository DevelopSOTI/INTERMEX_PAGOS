using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace PagosIntermex
{
    public static class ExtensionMethods
    {
       
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }

    class C_REGISTROSWINDOWS
    {
        private const string caption = "Mensaje de aplicación";
        private const string ruta_registros = "Software\\SOTI\\Intermex\\PAGOS_BANCOS";

        RegistryKey rk1 = Registry.CurrentUser;
        RegistryKey rk2 = Registry.CurrentUser;
        RegistryHive registryHive = RegistryHive.CurrentUser;

        private string fb_servidor, fb_pass, fb_ruta, websvc_dom, websvc_host, websvc_pass, websvc_user, fb_bd;

        public C_REGISTROSWINDOWS()
        {
            fb_servidor = fb_pass = fb_ruta = websvc_dom = websvc_host = websvc_pass = websvc_user = fb_bd = "";
        }


        public string LICENCIA_VERIFICADA { get; set; }
        public string VERSION_VERIFICADA { get; set; }

        public string MENSAJE_LICENCIA { get; set; }


        // Propiedades FBServer
        public string FB_PASSWORD
        {
            set { fb_pass = value; }
            get { return fb_pass; }
        }

        public string FB_BD
        {
            set { fb_bd = value; }
            get { return fb_bd; }
        }

        public string FB_SERVIDOR
        {
            set { fb_servidor = value; }
            get { return fb_servidor; }
        }

        public string FB_ROOT
        {
            set { fb_ruta = value; }
            get { return fb_ruta; }
        }

        public string WEBSVC_DOMINIO
        {
            set { websvc_dom = value; }
            get { return websvc_dom; }
        }


        public string SQL_SERV { get; set; }

        public string SQL_DATA { get; set; }

        public string SQL_USER { get; set; }

        public string SQL_PASS { get; set; }



        public bool SO64bits()
        {
            bool bits;
            if (Environment.Is64BitOperatingSystem == true)
            {
                bits = true;
            }
            else
            {
                bits = false;
            }

            return bits;
        }

        public bool SO32bits()
        {
            bool bits;
            if (Environment.Is64BitOperatingSystem == false)
            {
                bits = true;
            }
            else
            {
                bits = false;
            }
            return bits;
        }

      /*  public void LeerRegistros(string ruta_registros)
        {
            try
            {
                if (SO64bits() == true)
                    rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                else
                    rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                rk2 = rk1.OpenSubKey(ruta_registros, false);

                //REGISTROS FB
                FB_PASSWORD = (string)rk2.GetValue("FB_PASS");
                FB_SERVIDOR = (string)rk2.GetValue("FB_SERV");
                FB_ROOT = (string)rk2.GetValue("FB_RUTA");
                FB_BD = (string)rk2.GetValue("FB_BD");

                SQL_SERV = (string)rk2.GetValue("SQL_SERV");
                SQL_DATA = (string)rk2.GetValue("SQL_DATA");
                SQL_USER = (string)rk2.GetValue("SQL_USER");
                SQL_PASS = (string)rk2.GetValue("SQL_PASS");

                WEBSVC_DOMINIO = (string)rk2.GetValue("WEBSVC_DOM");
            }
            catch
            {
                MessageBox.Show("No fue posible leer los registros de Windows.", "Mensaje de pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/



        public bool LeerRegistros(bool mostrar_alerta)
        {
            try
            {
                if (SO64bits())
                {
                    rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                }
                else
                {
                    if (SO32bits())
                    {
                        rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                    }
                }

                rk2 = rk1.OpenSubKey(ruta_registros, false);

                if (rk2 != null)
                {
                FB_PASSWORD = (string)rk2.GetValue("FB_PASSWORD");
                FB_SERVIDOR = (string)rk2.GetValue("FB_SERVIDOR");
                FB_ROOT = (string)rk2.GetValue("FB_ROOT");
                FB_BD = (string)rk2.GetValue("FB_BD");

                SQL_SERV = (string)rk2.GetValue("SQL_SERV");
                SQL_DATA = (string)rk2.GetValue("SQL_DATA");
                SQL_USER = (string)rk2.GetValue("SQL_USER");
                SQL_PASS = (string)rk2.GetValue("SQL_PASS");

                    VERSION_VERIFICADA = (string)rk2.GetValue("VERSION_VERIFICADA");
                    LICENCIA_VERIFICADA = (string)rk2.GetValue("LICENCIA_VERIFICADA");
                    MENSAJE_LICENCIA = (string)rk2.GetValue("MENSAJE_LICENCIA");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (mostrar_alerta)
                {
                    MessageBox.Show("No fue posible leer los registros de Windows.\n\n" + ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }

      /*  public bool VerificarRegistros(string ruta_registros)
        {
            try
            {
                if (SO64bits() == true)
                    rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                else
                    rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                rk2 = rk1.OpenSubKey(ruta_registros, false);

                //REGISTROS FB
                FB_PASSWORD = (string)rk2.GetValue("FB_PASS");
                FB_SERVIDOR = (string)rk2.GetValue("FB_SERV");
                FB_ROOT = (string)rk2.GetValue("FB_RUTA");
                FB_BD = (string)rk2.GetValue("FB_BD");

                WEBSVC_DOMINIO = (string)rk2.GetValue("WEBSVC_DOM");

                return true;
            }
            catch
            {
                return false;
                // MessageBox.Show("No fue posible leer los registros de Windows.", "Mensaje de aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        public bool CrearRegistros(string ruta_registros)
        {
            try
            {
                Registry.CurrentUser.CreateSubKey(ruta_registros);
               // RegistrosDefault();

                return true;
            }
            catch
            {
                return false;
            }
        }





        public void EscribirRegistro_Pass_bd(string ruta_registro, string pass_bd)
        {
            if (SO64bits() == true)
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("FB_PASS", pass_bd);
            }
            else
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("FB_PASS", pass_bd);
            }
        }

        public void EscribirRegistro_Servidor_bd(string ruta_registro, string ruta_bd)
        {
            if (SO64bits() == true)
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("FB_SERV", ruta_bd);
            }
            else
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("FB_SERV", ruta_bd);
            }
        }

        public void EscribirRegistro_Ruta_bd(string ruta_registro, string ruta_bd)
        {
            if (SO64bits() == true)
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("FB_RUTA", ruta_bd);
            }
            else
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("FB_RUTA", ruta_bd);
            }
        }

        public void EscribirRegistro_Data_bd(string ruta_registro, string data_bd)
        {
            if (SO64bits() == true)
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("FB_BD", data_bd);
            }
            else
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("FB_BD", data_bd);
            }
        }



        public void EscribirRegistro_Dominio(string ruta_registro, string ruta_bd)
        {
            if (SO64bits() == true)
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("WEBSVC_DOM", ruta_bd);
            }
            else
            {
                rk1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                rk2 = rk1.OpenSubKey(ruta_registro, true);
                rk2.SetValue("WEBSVC_DOM", ruta_bd);
            }
        }

        public void RegistrosDefault()
        {
            C_REGISTROSWINDOWS cliente = new C_REGISTROSWINDOWS();// { id = 1, Nombre = "Cliente A" };
            Type _type = cliente.GetType();

            System.Reflection.PropertyInfo[] listaPropiedades = _type.GetProperties();

            foreach (System.Reflection.PropertyInfo propiedad in listaPropiedades)
            {
                EscribirRegistros(propiedad.Name, " ", false);
            }
        }

        public void EscribirRegistros(string nombre_registro, string valor_registro, bool mostrar_alerta)
        {
            try
            {
                if (SO64bits())
                {
                    //rk1 = Registry.CurrentUser.OpenSubKey("Software", true);
                    rk1 = RegistryKey.OpenBaseKey(registryHive, RegistryView.Registry64);
                    rk2 = rk1.OpenSubKey(ruta_registros, true);
                    rk2.SetValue(nombre_registro, valor_registro);
                }
                else
                {
                    if (SO32bits())
                    {
                        //rk1 = Registry.CurrentUser.OpenSubKey("Software", true);
                        rk1 = RegistryKey.OpenBaseKey(registryHive, RegistryView.Registry32);
                        rk2 = rk1.OpenSubKey(ruta_registros, true);
                        rk2.SetValue(nombre_registro, valor_registro);
                    }
                }
            }
            catch
            {
                if (mostrar_alerta)
                {
                    // MessageBox.Show("No fue posible escribir en los registros de Windows.\n\n" + ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public bool CrearRegistros(bool mostrar_alerta)
        {
            try
            {
                //Registry.CurrentUser.OpenSubKey("Software", true);
                // Registry.CurrentUser.CreateSubKey(ruta_registros);
                rk2 = rk1.CreateSubKey(ruta_registros);
                RegistrosDefault();
                /*if (SO64bits())
                {
                    Registry.CurrentUser.OpenSubKey("Software", true);

                    Registry.CurrentUser.CreateSubKey(ruta_registros);
                }
                else
                {
                    if (SO32bits())
                    {
                        Registry.CurrentUser.OpenSubKey("Software", true);

                        Registry.CurrentUser.CreateSubKey(ruta_registros);
                    }
                }*/

                return true;
            }
            catch
            {
                if (mostrar_alerta)
                {
                    // MessageBox.Show("No fue posible crear la nueva clave en los registros de Windows.\n\n" + ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }


        public void checarRegistrosLicencias()
        {
            try
            {
                var view = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, view))
                {
                    using (var key = baseKey.OpenSubKey(ruta_registros, true) ?? baseKey.CreateSubKey(ruta_registros, true))
                    {
                        if (key == null) return;

                        // Si no existe, crea en blanco
                        if (key.GetValue("VERSION_VERIFICADA") == null)
                            key.SetValue("VERSION_VERIFICADA", "", RegistryValueKind.String);

                        if (key.GetValue("LICENCIA_VERIFICADA") == null)
                            key.SetValue("LICENCIA_VERIFICADA", "", RegistryValueKind.String);

                        if (key.GetValue("MENSAJE_LICENCIA") == null)
                            key.SetValue("MENSAJE_LICENCIA", "", RegistryValueKind.String);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible verificar/crear los registros de licencia.\n\n" + ex.Message,
                                caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}