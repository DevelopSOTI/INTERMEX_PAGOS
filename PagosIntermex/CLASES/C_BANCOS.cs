using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosIntermex
{
    class C_BANCOS
    {
        public string NOMBRE { get; set; }
        public string CLAVE_FISCAL { get; set; }

        public override string ToString()
        {
            return NOMBRE.ToString();
        }
    }
}
