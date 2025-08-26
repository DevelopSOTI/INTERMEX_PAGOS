using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Windows.Forms;
using RestSharp;
using System.Reflection;
using System.Management;
using FirebirdSql.Data.FirebirdClient;
using System.Security.Cryptography;


namespace PagosIntermex
{
    internal class ApiLic
    {
        #region CLASE ENCRIPTADO
        public static class ExtensionMethods
        {
        }

        class C_Global
        {
            private static Random random = new Random();

            private const int Keysize = 128;
            private const int DerivationIterations = 1000;

            private string registros;
            private string password;
            private string sistema;

            public C_Global()
            {

            }

            public string REGISTROS
            {
                set { registros = value; }
                get { return registros; }
            }

            public string PASSWORD
            {
                set { password = value; }
                get { return password; }
            }

            public string SISTEMA
            {
                set { sistema = value; }
                get { return sistema; }
            }





            public T[,] ResizeArray<T>(T[,] original, int rows, int cols)
            {
                var newArray = new T[rows, cols];
                int minRows = Math.Min(rows, original.GetLength(0));
                int minCols = Math.Min(cols, original.GetLength(1));
                for (int i = 0; i < minRows; i++)
                {
                    for (int j = 0; j < minCols; j++)
                    {
                        newArray[i, j] = original[i, j];
                    }
                }
                return newArray;
            }





            #region FUNCIONES PARA CADENAS RANDOM

            public string RandomNumeric(int length)
            {
                const string chars = "0123456789";
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }

            public string RandomString(int length)
            {
                const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789@-/[]()&%$#!¡¿?|+*";
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }

            public string RandomLetterNumeric(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }

            public string RandomCharacter(int length)
            {
                const string chars = "@-/[]()&%$#!¡¿?|+*";
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }

            #endregion





            #region FUNCIONES PARA ENCRIPTADO Y DESENCRIPTADO

            private static byte[] Generate256BitsOfRandomEntropy()
            {
                var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    // Fill the array with cryptographically secure random bytes.
                    rngCsp.GetBytes(randomBytes);
                }
                return randomBytes;
            }
            private static byte[] Generate128BitsOfRandomEntropy()
            {
                var randomBytes = new byte[16]; // 16 bytes = 128 bits
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetBytes(randomBytes);
                }
                return randomBytes;
            }


            public string Encrypt(string plainText, string passPhrase)
            {
                // Generar salt y IV de forma segura
                byte[] salt = new byte[32]; // 256 bits
                byte[] iv = new byte[16];   // 128 bits
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                    rng.GetBytes(iv);
                }

                // Derivar clave con PBKDF2
                using (var keyDerivationFunction = new Rfc2898DeriveBytes(passPhrase, salt, 1000))
                {
                    byte[] key = keyDerivationFunction.GetBytes(32); // 256-bit key

                    using (var aes = Aes.Create())
                    {
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;
                        aes.Key = key;
                        aes.IV = iv;

                        using (var encryptor = aes.CreateEncryptor())
                        using (var ms = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            using (var sw = new StreamWriter(cryptoStream))
                            {
                                sw.Write(plainText);
                            }

                            // Prepend salt + IV + ciphertext
                            byte[] encrypted = ms.ToArray();
                            byte[] result = new byte[salt.Length + iv.Length + encrypted.Length];
                            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
                            Buffer.BlockCopy(iv, 0, result, salt.Length, iv.Length);
                            Buffer.BlockCopy(encrypted, 0, result, salt.Length + iv.Length, encrypted.Length);

                            return Convert.ToBase64String(result);
                        }
                    }
                }
            }

            public string Decrypt(string cipherText, string passPhrase)
            {
                byte[] fullCipher = Convert.FromBase64String(cipherText);

                if (fullCipher.Length < 48)
                    throw new ArgumentException("El texto cifrado es demasiado corto.");

                byte[] salt = new byte[32];
                byte[] iv = new byte[16];
                byte[] cipherBytes = new byte[fullCipher.Length - 48];

                Buffer.BlockCopy(fullCipher, 0, salt, 0, 32);
                Buffer.BlockCopy(fullCipher, 32, iv, 0, 16);
                Buffer.BlockCopy(fullCipher, 48, cipherBytes, 0, cipherBytes.Length);

                using (var keyDerivationFunction = new Rfc2898DeriveBytes(passPhrase, salt, 1000))
                {
                    byte[] key = keyDerivationFunction.GetBytes(32);

                    using (var aes = Aes.Create())
                    {
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;
                        aes.Key = key;
                        aes.IV = iv;

                        using (var decryptor = aes.CreateDecryptor())
                        using (var ms = new MemoryStream(cipherBytes))
                        using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        using (var sr = new StreamReader(cryptoStream))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }



            #endregion





            public string HDD_Serial()
            {
                string HDD = System.Environment.CurrentDirectory.Substring(0, 1);
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + HDD + ":\"");
                disk.Get();
                return disk["VolumeSerialNumber"].ToString();
            }

            public bool ValidaProducto()
            {
                bool result = false;

             //   C_CONEXIONFIREBIRD conn = new C_CONEXIONFIREBIRD(registros);
                FbTransaction transaction;
                FbCommand cmd;
                FbDataReader reader;

                string HDD = HDD_Serial();
                string HDD_Encrypt = "";

               /* if (conn.ConectarConfig())
                {
                    transaction = conn.FBC.BeginTransaction();

                    try
                    {
                        cmd = new FbCommand("SELECT * FROM licencias_soti_det ld JOIN licencias_soti ls ON(ld.id_licencia = ls.id_licencia) WHERE ls.sistema_microsip = '" + sistema + "'", conn.FBC, transaction);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            HDD_Encrypt = Decrypt(reader["FOLIO_INSTALACION"].ToString(), password);

                            if (HDD_Encrypt == HDD)
                            {
                                result = true;
                            }
                        }
                        cmd.Dispose();
                        reader.Close();
                    }
                    catch
                    {
                        result = false;
                    }

                    conn.Desconectar();
                }*/

                return result;
            }

        }
        #endregion

        public string NOMBRE_LICENCIA { get; set; }
        public string PASSWORD { get; set; }

        bool verificado = false;

        public ApiLic()
        {
            PASSWORD = "S0t1.C0M.mX:L1c3nC14$";
        }
        public class Licencia
        {
            
            public string NOMBRE_LICENCIA { get; set; } 
            public string ESTATUS_LICENCIA { get; set; }
            public string ESTATUS_CLIENTE { get; set; }
            public string ESTATUS_VERSION_MSP { get; set; }
            public string VERSION_BD_MIN { get; set; }
            public string VERSION_BD_MAX { get; set; }
            public string TIPO { get; set; }
            public string NO_LICENCIAS { get; set; } 
            public string FECHA_ENTREGA { get; set; }
            public string FECHA_INICIO { get; set; }
            public string FECHA_FIN { get; set; } 
            public string NOMBRE_USUARIO { get; set; } 
            public string ID_DISCO_DURO { get; set; } 
            public string ESTATUS_DET_LIC { get; set; } 
            public string SINCRONIZADO { get; set; } 
            public string RAZON_SOCIAL { get; set; } 
            public string err { get; set; } 
            public string mensaje { get; set; }
        }

        public class JSON
        {
            public string err { get; set; }
            public string mensaje { get; set; }

            public Licencia[] Licencia { get; set; }

            public JSON()
            {
                this.Licencia = new Licencia[0];
            }
        }

        public dynamic Post(string url, string json, string autorizacion = null)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                if (autorizacion != null)
                {
                    request.AddHeader("Authorization", autorizacion);
                }

                IRestResponse response = client.Execute(request);

                dynamic datos = JsonConvert.DeserializeObject(response.Content);

                return datos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        public dynamic Get(string json)
        {
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create("https://soti.com.mx/Licencias/ApiLicencias/index.php/licencias/licencia" + "?JSON=" + json);
            myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";    
            //myWebRequest.CookieContainer = myCookie;
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            myWebRequest.Proxy = null;
            
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream myStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);
            //Leemos los datos
            string Datos = HttpUtility.HtmlDecode(myStreamReader.ReadToEnd());

            dynamic data = JsonConvert.DeserializeObject(Datos);

            return data;
        }


        public bool VerificarProducto(string json)
        {            
            C_Global global = new C_Global();
            try
            {
                C_REGISTROSWINDOWS reg = new C_REGISTROSWINDOWS();
                if (reg.LeerRegistros(false))
                {
                    //checamos si tiene red la pc
                    bool RedActiva = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

                    //CHECAMOS EN REGISTROS SI YA SE REGISTRO EL VERIFICADO
                    if (string.IsNullOrEmpty(reg.LICENCIA_VERIFICADA.ToString().Trim()))
                    {
                        if (RedActiva)
                        {

                            #region  VERIFICAMOS LA CONEXION A INTERNET
                            System.Uri Url = new System.Uri("https://www.soti.com.mx");

                            System.Net.WebRequest WebRequest;
                            WebRequest = System.Net.WebRequest.Create(Url);
                            System.Net.WebResponse objetoResp;
                            try
                            {
                                objetoResp = WebRequest.GetResponse();
                                objetoResp.Close();
                                RedActiva = true;
                            }
                            catch (Exception ex)
                            {
                                RedActiva = false;
                                reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("NO", PASSWORD), false);
                                reg.EscribirRegistros("MENSAJE_LICENCIA", "No se puede verificar la licencia debido a que no tiene una conexión de internet", false);
                                verificado = false;
                            }

                            #endregion

                            if (RedActiva)
                            {
                                dynamic resp = Get(json);
                                string versionMsp = resp.licencia[0].VERSION_BD_MAX.ToString();

                                if (!string.IsNullOrEmpty(versionMsp))
                                {
                                    //SI ES -1 ES PORQUE AUN ESTA ACTIVA LA VERSION DE MICROSIP ACTUAL Y NO SE AH DEFINIDO CUAL ES LA ULTIMA VERSION VALIDA
                                    if (versionMsp == "-1")
                                    {
                                        verificado = true;

                                        reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("SI", PASSWORD), false);
                                        reg.EscribirRegistros("VERSION_VERIFICADA", global.Encrypt(versionMsp, PASSWORD), false);

                                    }
                                    else
                                    {
                                        //SI TIENE UN NUMERO FINITO HAY QUE COMPARAR CON LA VERSION DEL CONFIG
                                        C_CONEXIONFIREBIRD con = new C_CONEXIONFIREBIRD();
                                        if (con.ConectarFB_Metadatos())
                                        {

                                            int versionActualMsp = con.VersionActualMsp(con);


                                            if (versionActualMsp > 0)
                                            {
                                                if (versionActualMsp > Convert.ToInt32(versionMsp))
                                                {
                                                    verificado = false;


                                                    reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("SI", PASSWORD), false);
                                                    reg.EscribirRegistros("VERSION_VERIFICADA", global.Encrypt(versionMsp, PASSWORD), false);
                                                    reg.EscribirRegistros("MENSAJE_LICENCIA", "La versión actual de Microsip no es compatible con este sistema", false);

                                                }
                                                else
                                                {
                                                    reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("SI", PASSWORD), false);
                                                    reg.EscribirRegistros("VERSION_VERIFICADA", global.Encrypt(versionMsp, PASSWORD), false);
                                                    verificado = true;
                                                }
                                            }
                                            else
                                            {
                                                verificado = false;

                                                reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("NO", PASSWORD), false);
                                                reg.EscribirRegistros("MENSAJE_LICENCIA", "No se pudo conectar con el Metadatos", false);
                                            }

                                            con.Desconectar();
                                        }
                                    }
                                }
                                else
                                {
                                    verificado = false;

                                    reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("NO", PASSWORD), false);
                                    reg.EscribirRegistros("MENSAJE_LICENCIA", "No se pudo conectar con el servidor de licencias", false);
                                }
                            }

                        }
                        else
                        {
                            verificado = false;

                            reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("NO", PASSWORD), false);
                            reg.EscribirRegistros("MENSAJE_LICENCIA", "No se pudo conectar con el servidor de licencias. Verificar la conexión de internet", false);
                        }
                    }
                    else
                    {
                        if(global.Decrypt(reg.LICENCIA_VERIFICADA,PASSWORD) == "NO")
                        {
                            //vaciamos y hacemos recursividad
                            reg.EscribirRegistros("LICENCIA_VERIFICADA", String.Empty, false);
                            VerificarProducto(json);
                        }
                        else
                        {
                            C_CONEXIONFIREBIRD con = new C_CONEXIONFIREBIRD();
                            //vemos la version que esta en el registro de windows
                            string versionMsp = global.Decrypt(reg.VERSION_VERIFICADA, PASSWORD);

                            if (con.ConectarFB_Metadatos())
                            {
                                int versionActualMsp = con.VersionActualMsp(con);

                                if (versionActualMsp != 0)
                                {

                                    if(versionMsp == "-1")
                                    {
                                        reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("SI", PASSWORD), false);
                                        reg.EscribirRegistros("VERSION_VERIFICADA", global.Encrypt(versionActualMsp.ToString(), PASSWORD), false);
                                        verificado = true;
                                    }
                                    else if (versionActualMsp > Convert.ToInt32(versionMsp))
                                    {

                                        if (MessageBox.Show("La versión actual de Microsip no es compatible con este sistema\n¿Desea buscar una actualización en la red?",
                                            "Administrador de licencias", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                        {
                                            reg.EscribirRegistros("LICENCIA_VERIFICADA", String.Empty, false);
                                            VerificarProducto(json);
                                        }
                                        else
                                        {

                                            verificado = false;

                                            reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("SI", PASSWORD), false);
                                            reg.EscribirRegistros("VERSION_VERIFICADA", global.Encrypt(versionMsp, PASSWORD), false);
                                            reg.EscribirRegistros("MENSAJE_LICENCIA", "La versión actual de Microsip no es compatible con este sistema", false);
                                        }
                                    }
                                    else
                                    {
                                        reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("SI", PASSWORD), false);
                                        reg.EscribirRegistros("VERSION_VERIFICADA", global.Encrypt(versionActualMsp.ToString(), PASSWORD), false);
                                        verificado = true;
                                    }
                                }
                                else
                                {
                                    verificado = false;

                                    reg.EscribirRegistros("LICENCIA_VERIFICADA", global.Encrypt("NO", PASSWORD), false);
                                    reg.EscribirRegistros("MENSAJE_LICENCIA", "No se pudo conectar con el Metadatos", false);
                                }

                                con.Desconectar();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hubo un error al verificar el producto\n" + ex.Message,"Mensaje de Licencias",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            return verificado;
        }

    }
}
