using System.Collections.Generic;

namespace Entidades
{
    public class Solicitud
    {
        public int solicitudId { get; set; }
        public int tipoMaterialSol { get; set; }
        public int tipoSolicitudId { get; set; }
        public int tipoMaterial { get; set; }
        public string nombreTipoMaterial { get; set; }
        public string nroInterno { get; set; }
        public string fechaAtencion { get; set; }
        public string obraTd { get; set; }
        public string codigoEstadoSol { get; set; }
        public string estadoSol { get; set; }
        public string codigoEstadoObra { get; set; }
        public string estadoObra { get; set; }
        public string descripcionObra { get; set; }
        public string direccionObra { get; set; }
        public string clienteObra { get; set; }
        public string fechaAsignacion { get; set; }
        public string fechaVencimiento { get; set; }
        public string observacion { get; set; }
        public string centroCosto { get; set; }
        public string cuadrillaCodigo { get; set; }
        public string dniCuadrilla { get; set; }
        public string pubEstadoCodigo { get; set; }
        public string numeroGuia { get; set; }
        public string usuario { get; set; }
        public int filtro { get; set; }
        public int tipo { get; set; }
        public int identity { get; set; }
        public string dniCoordinador { get; set; }
        public string nombreCoordinador { get; set; }
        public string dniPersonal { get; set; }
        public string nombrePersonal { get; set; }
        public string sucursalId { get; set; }
        public List<RegistroMaterialSolicitud> materiales { get; set; }
        public List<RegistroPhotoSolicitud> photos { get; set; }
    }
}
