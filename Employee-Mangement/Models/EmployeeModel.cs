using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Mangement.Models
{
    public class EmployeeModel
    {
        [Key]
        public long employeeid { get; set; }
        [Required]
        public string employeename { get; set; }
        [ForeignKey("des")]
        public long designationid { get; set; }
        
        public decimal salary { get; set; }
        public bool? flagdeleted { get; set; } 
        public virtual Designation des { get; set; }
    }
}
