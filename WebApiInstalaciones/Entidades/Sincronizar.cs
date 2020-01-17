using System.Collections.Generic;

namespace Entidades
{
    public class Sincronizar
    {
        public List<Almacen> almacens { get; set; }
        public List<Parametro> parametros { get; set; }
        public List<ParteDiario> parteDiarios { get; set; }
        public List<Baremo> baremos { get; set; }
        public List<Materiales> materiales { get; set; }
        public List<Obra> obras { get; set; }
        public List<Estado> estados { get; set; }
        public List<Articulo> articulos { get; set; }
        public Resumen resumen { get; set; }
        public List<Solicitud> solicitudes { get; set; }
        public List<Personal> personals { get; set; }
        public List<Coordinador> coordinadors { get; set; }
        public List<TipoDevolucion> devoluciones { get; set; }
        public List<Actividad> actividades { get; set; }
        public List<Medidor> medidores { get; set; }
    }
}
