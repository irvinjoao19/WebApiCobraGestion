using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario
    {
        public string usuarioId { get; set; }
        public string nombre { get; set; }
        public string mensaje { get; set; }
        public string dniCuadrillaId { get; set; }
        public string cuadrillaId { get; set; }    
        public Filtro filtro { get; set; }
    }
}
