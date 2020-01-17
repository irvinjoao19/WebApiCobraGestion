using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Baremo
    {
        public string baremoId { get; set; }
        public string descripcion { get; set; }
        public string unidadMedida { get; set; }
        public string abreviatura { get; set; }
        public int actividadId { get; set; }
    }
}
