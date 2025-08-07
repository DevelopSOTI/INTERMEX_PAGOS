using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosIntermex
{
    public class C_ANTICIPOS
    {
      public int Proveedor_ID { get; set; }
        public string Proveedor_Clave { get; set; }
        public string Proveedor_Nombre { get; set; }
        public int Requisicion_id { get; set; }
        public string Folio { get; set; }
        public double TOTAL { get; set; }
        public string Empresa { get; set; }
        public string Prioridad { get; set; }
        public string Estatus_general { get; set; }
        public C_ANTICIPOS()
        {

        }
    }
}
