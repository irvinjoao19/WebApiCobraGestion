using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class CentroCostos
    {
        public string centroId { get; set; }
        public string orden { get; set; }
        public string descripcion { get; set; }
        public string sucursalId { get; set; }
        public string nombreSucursal { get; set; }
        //public List<Cuadrilla> cuadrillas { get; set; }
    }
}
