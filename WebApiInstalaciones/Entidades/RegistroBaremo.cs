using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RegistroBaremo
    {
        public int registroBaremoId { get; set; }
        public int parteDiarioId { get; set; }
        public string codigoBaremo { get; set; }
        public decimal cantidadMovil { get; set; }
        public decimal cantidadOk { get; set; }
        public string fecha { get; set; }
        public string descripcion { get; set; }
        public string unidadMedida { get; set; }
        public string abreviatura { get; set; }        
        public int tipo { get; set; }
        public int identity { get; set; }
        public int identityDetalle { get; set; }
        public int actividadId { get; set; }
        public string estado { get; set; }
    }
}
