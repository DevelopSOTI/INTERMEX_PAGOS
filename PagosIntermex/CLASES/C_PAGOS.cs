using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosIntermex
{
    public class C_PAGOS
    {
        public int DOCTO_PR_DET_ID { get; set; }
        public string FOLIO { get; set; }
        public string FECHA { get; set; }
        public string PROVEEDOR { get; set; }
        public string PROVEEDOR_CLAVE { get; set; }
        public int PROVEEDOR_ID { get; set; }
        public DateTime FECHA_VENC { get; set; }
        public double IMPORTE { get; set; }
        public double IMP_AUTORIZADO { get; set; }
        public string ESTATUS { get; set; }
        public double PAGO { get; set; }
        public string EMPRESA { get; set; }
        public string ACCION { get; set; }
        public string REQ_ID { get; set; }


        public C_PAGOS()
        {

        }
    }
}
