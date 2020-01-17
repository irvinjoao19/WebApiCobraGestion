using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Obra
    {
        public string obraId { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public string direccion { get; set; }
        public string cliente { get; set; }
        public string fechaAsignacion { get; set; }
        public string fechaVencimiento { get; set; }
        public string usuarioCreacion { get; set; }
    }
}
