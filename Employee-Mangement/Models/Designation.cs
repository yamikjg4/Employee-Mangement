using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Mangement.Models
{
    public class Designation
    {
        public long designationid { get; set; }
        public string designationname { get; set; }
        public bool? flagdeleted { get; set; }
        public virtual ICollection<EmployeeModel> emp { get; set; }
    }
}
