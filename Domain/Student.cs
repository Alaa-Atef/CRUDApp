using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Student
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Student Name can't exceed 100 characters.")]
        public string Name { get; set; }
        [Range(5, 120)]
        public int Age { get; set; }
    }
}
