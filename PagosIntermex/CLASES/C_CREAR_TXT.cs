using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosIntermex
{
    class C_CREAR_TXT
    {
        public string RUTA { get; set; }
        public string NOMBRE_ARCHIVO { get; set; }//conextension
        public void escribirLineaFichero(string ruta, string nombre, string[] escribir)
        {
            try
            {
                string path = ruta + "\\" + nombre;
                string[] linea = new string[0];


                for (int i = 0; i < escribir.Length; i++)
                {
                    Array.Resize(ref linea, linea.Length + 1);

                    linea[i] = escribir[i] + "\r\n";
                }

                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        for (int i = 0; i < linea.Length; i++)
                        {
                            sw.WriteLine(linea[i]);
                        }

                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        for (int i = 0; i < linea.Length; i++)
                        {
                            sw.WriteLine(linea[i]);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        public void borrarLayout()
        {
            string RutaFichero = RUTA;
            if (Directory.Exists(RutaFichero))
            {
                //File.Delete(RutaFichero);
                foreach (var item in Directory.GetFiles(RutaFichero, NOMBRE_ARCHIVO))
                {
                    File.SetAttributes(item, FileAttributes.Normal);
                    File.Delete(item);
                }
            }
        }

        public void CrearLayout()
        {


            string RutaFichero = RUTA;
            if (Directory.Exists(RutaFichero))
            {
                //File.Delete(RutaFichero);
                try
                {
                    File.Copy(Path.Combine(RutaFichero, NOMBRE_ARCHIVO), Path.Combine(RutaFichero, NOMBRE_ARCHIVO));
                }
                catch
                {

                    throw;
                }
            }
        }

        public void ADD_LINEA(string linea)
        {
            string RutaFichero = "";

            RutaFichero = RUTA;
            string[] lineas = new string[1];
            lineas[0] = linea;

            escribirLineaFichero(RutaFichero, NOMBRE_ARCHIVO, lineas);
            System.Threading.Thread.Sleep(30);

        }
    }
}
