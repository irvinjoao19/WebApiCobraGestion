using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Personal
    {
        public int personalId { get; set; }
        public int empresaId { get; set; }
        public string nroDocumento { get; set; }
        public string apellido { get; set; }
        public string nombre { get; set; }
    }
}
