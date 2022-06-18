using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Mangement.Models
{
    public class JoinModel
    {
        public long employeeid { get; set; }
        public string employeename { get; set; }
        public long designationid { get; set; }
        public decimal salary { get; set; }
        public string designationname { get; set; }

    }
}
