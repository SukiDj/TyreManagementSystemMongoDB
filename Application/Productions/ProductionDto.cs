using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Productions
{
    public class ProductionDto
    {
        public Guid Id { get; set; }
        public string TyreCode { get; set; }
        public int QuantityProduced { get; set; }
        public DateTime ProductionDate { get; set; }
        public int Shift { get; set; }
        public string MachineNumber { get; set; }
        public string OperatorId { get; set; }
    }
}