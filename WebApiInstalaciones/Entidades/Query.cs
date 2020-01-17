using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Query
    {
        public string user { get; set; }
        public string pass { get; set; }
        public string areaId { get; set; }
        public string usuarioId { get; set; }
        public string cuadrillaId { get; set; }
        public string sucursalId { get; set; }
        public string centroCostoId { get; set; }
        public int filtro { get; set; }
        public string fechaInicial { get; set; }
        public string fechaFinal { get; set; }
        public string estado { get; set; }
        public string tipoProceso { get; set; }
        public string tipoCliente { get; set; }
        public string codigoArticulo { get; set; }
        public int tipoSolicitud { get; set; }
        public int tipoMaterialSolicitud { get; set; }
        public string almacenId { get; set; }
        public string fechaRegistro { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string search { get; set; }
        public string obraId { get; set; }
        public int solicitudId { get; set; }
        public string personalDni { get; set; }
    }
}
