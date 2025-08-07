using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosIntermex
{
    public class C_EMPRESAS
    {
        public C_EMPRESAS()
        {

        }
        public string NOMBRE_CORTO { get; set; }
        public string NOMBRE { get; set; }
        public int EMPRESA_ID { get; set; }

        public bool TIENE_ANTICIPO { get; set; }
        public bool TIENE_PAGO { get; set; }
        public override string ToString()
        {
            return NOMBRE_CORTO.ToString();
        }
    }
}
