using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ForgetPassswordViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
