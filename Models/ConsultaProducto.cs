using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareaIngenieria.Models
{
    public class ConsultaProducto
    {
        public int ProductId { get; set; }
        public string Descripcion { get; set; }
        public decimal Total { get; set; }
        public int CantidadVendida { get; set; }
    }
}
