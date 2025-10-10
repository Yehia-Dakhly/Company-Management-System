using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public Decimal Salary {  get; set; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [ForeignKey(nameof(Department))] // Representation if Change Name or Allowing Null
        public int? DepartmentId { get; set; } // FK
        // FK Optional -> OnDelete -> Restrict
        // FK Required -> OnDelete -> Cascade   
        [InverseProperty("Employees")]
        public Department Department { get; set; }
        public string ImageName { get; set; }
    }
}
