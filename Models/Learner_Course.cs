using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Learner_Course
    {
        [Required]
        [Key]
        public string LearnerId { get; set; }
        [ForeignKey("LearnerId")]
        public ApplicationUser learner { get; set; }
        [Key]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course course { get; set; }
    }
}
