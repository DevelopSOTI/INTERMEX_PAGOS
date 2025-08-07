using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosIntermex
{
    public class C_CUENTAS_BAN
    {
        public string concepto_cp {get;set;}

        public string concepto_anticipo { get; set; }
        public string cuenta_ordenante { get; set; }
        public string banco { get; set; }
        public string clave_fiscal { get; set; }
        public string formato_layout { get; set; }

        public int moneta_id { get; set; }
        public int cuenta_ban_id { get; set; }
        public int concepto_ba_id { get; set; }

        public int cuentaBancariaNombre { get; set; }
        public int conceptosBancosNombre { get; set; }

        public bool requiereCompFiscal { get; set; }
        public string Disponibilidad { get; set; }


        public string EMPRESA { get; set; }

        public C_CUENTAS_BAN()
        {
            moneta_id = cuenta_ban_id = concepto_ba_id = 0;
        }
    }
}
