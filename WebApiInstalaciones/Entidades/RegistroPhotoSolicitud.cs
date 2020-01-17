using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RegistroPhotoSolicitud
    {
        public int registroPhotoId { get; set; }
        public int solicitudId { get; set; } // solicitudId
        public string nombre { get; set; }
        public string fecha { get; set; }
        public int tipo { get; set; }
        public int identity { get; set; }
        public int identityFoto { get; set; }
    }
}
