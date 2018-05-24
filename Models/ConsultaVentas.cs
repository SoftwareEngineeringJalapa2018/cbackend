using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareaIngenieria.Models
{
    public class ConsultaVentas
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Stock { get; set; }
        public int QuantitySold { get; set; }
        public DateTime LastSoldDate { get; set; }
        public string BestCustomer { get; set; }
    }
}
