using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Resumen
    {
        public int resumenId { get; set; }
        public decimal total { get; set; }
        public decimal ejecutas { get; set; }
        public decimal porVencer { get; set; }
        public decimal vencidos { get; set; }
        public decimal pendientes { get; set; }
        public decimal porAvance { get; set; }
    }
}
