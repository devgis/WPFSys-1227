using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMSys.Model
{
    public class SalarySheet
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public Guid DePartmentId { get; set; }

    }
}
