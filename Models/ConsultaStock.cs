using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareaIngenieria.Models
{
    public class ConsultaStock
    {
        public int ProductSubcategoryID { get; set; }
        public string Name { get; set; }
        public int WorkOrderQty { get; set; }
        public int WorkOrderCost { get; set; }
        public int PurchaseOrderQty { get; set; }
        public int PurchaseOrderCost { get; set; }
    }
}
