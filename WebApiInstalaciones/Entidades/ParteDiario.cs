using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ParteDiario
    {
        public int parteDiarioId { get; set; }
        public string fecha { get; set; }  // date
        public string obraTd { get; set; }
        public string codigoEstadoPd { get; set; }
        public string estadoTd { get; set; }
        public string codigoEstadoObra { get; set; }
        public string estadoObra { get; set; }
        public string descripcion { get; set; }
        public string direccion { get; set; }
        public string cliente { get; set; }
        public string fechaAsignacion { get; set; }// date
        public string fechaVencimiento { get; set; }// date

        public string empresaCodigo { get; set; }
        public string areaCodigo { get; set; }
        public string codigoInterno { get; set; }
        public string costoCodigo { get; set; }
        public string cuadrillaCodigo { get; set; }
        public string dniCuadrilla { get; set; }
        public string suministro { get; set; }
        public string sed { get; set; }
        public string nroFicha { get; set; }
        public string observacion { get; set; }
        public string medencontrado_nro { get; set; }
        public string medencontrado_marca { get; set; }
        public string medencontrado_fase { get; set; }
        public string medencontrado_estado { get; set; }
        public string medencontrado_modelo { get; set; }
        public string medinstalado_nro { get; set; }
        public string medinstalado_marca { get; set; }
        public string medinstalado_fase { get; set; }
        public string medinstalado_estado { get; set; }
        public string medinstalado_modelo { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string estadoCodigo { get; set; }
        public string usuarioCreacion { get; set; }
        public string fechaMovil { get; set; }
        public string firmaMovil { get; set; }
        public string coordinadorDni { get; set; }
        public string descripcionCoordinador { get; set; }
        public string sucursalId { get; set; }
        public int tipo { get; set; }
        public int identity { get; set; }
        public int estado { get; set; }
        public List<RegistroBaremo> baremos { get; set; }
        public List<RegistroMaterial> materiales { get; set; }
        public List<RegistroPhoto> photos { get; set; }
    }
}

