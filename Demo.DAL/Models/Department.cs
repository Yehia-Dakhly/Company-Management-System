using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department
    {
        public int Id { get; set; } // PK
        [Required(ErrorMessage ="Name is Required")]
        [MaxLength(50)]
        public string Name { get; set; } //.Net 5 Allow Null
        [Required(ErrorMessage = "Code is Required")]
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; }
        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
        // Navigational Property [Many]
    }
}
