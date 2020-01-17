using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Filtro
    {
        public List<Sucursal> sucursales { get; set; }
        public List<Area> areas { get; set; }
        public List<CentroCostos> centros { get; set; }
    }
}
