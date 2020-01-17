using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Materiales
    {
        public int id { get; set; }
        public string tipoMaterial { get; set; }
        public string materialId { get; set; }
        public string descripcion { get; set; }
        public string unidadMedida { get; set; }
        public string abreviatura { get; set; }
        public decimal stock { get; set; }
        public string obra{ get; set; }
        public string cc { get; set; }
        public string almacenId { get; set; }
        public string guiaSalida { get; set; }
        public string fecha { get; set; }
        public int tipo { get; set; }
        public string exigeSerie { get; set; }
    }
}
