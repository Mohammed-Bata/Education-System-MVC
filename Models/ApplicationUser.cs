using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        //Role Learner
        public ICollection<Learner_Course>? Learner_Courses { get; set; }
        //Role Instructor
        public ICollection<Course>? Courses { get; set; }
        [Required]
        [NotMapped]
        public string Role { get; set; }
        
    }
}
