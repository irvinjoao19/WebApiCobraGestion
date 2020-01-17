using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RegistroMaterial
    {
        public int registroMaterialId { get; set; }
        public int parteDiarioId { get; set; } // solicitudId
        public int tipoMaterial { get; set; }
        public string almacenId { get; set; }
        public string codigoMaterial { get; set; }
        public decimal cantidadMovil { get; set; }
        public decimal cantidadOk { get; set; }
        public string fecha { get; set; }
        public string descripcion { get; set; }
        public string unidadMedida { get; set; }
        public string abreviatura { get; set; }
        public int tipo { get; set; }
        public decimal stock { get; set; }
        public int identity { get; set; }
        public int identityDetalle { get; set; }
        public int filtro { get; set; }
        public string guiaSalida { get; set; }
        public string nroSerie { get; set; }
        public string estado { get; set; }
    }
}
