using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(100)]
        [Required]
        public string Description { get; set; }
        [Required]
        public string InstructorId { get; set; }
        [ForeignKey("InstructorId")]
        [ValidateNever]
        public ApplicationUser instructor { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? category { get; set; }
        [Required]
        [ValidateNever]
        public string ImageUrl { get; set; }
        [ValidateNever]
        public ICollection<Learner_Course> learner_courses { get; set; }
    }
}
