using Demo.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(50, ErrorMessage = "Max Length is 50 chars")]
        [MinLength(5, ErrorMessage = "Min Length is 5 chars")]
        public string Name { get; set; }
        [Range(22, 35, ErrorMessage = "Age Must Be in Range From 22 To 35")]
        public int? Age { get; set; }
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address Must be Like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public Decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }    
        [Phone]
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }

        [ForeignKey(nameof(Department))] // Representation if Change Name or Allowing Null
        public int? DepartmentId { get; set; } // FK
        // FK Optional -> OnDelete -> Restrict
        // FK Required -> OnDelete -> Cascade   
        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
