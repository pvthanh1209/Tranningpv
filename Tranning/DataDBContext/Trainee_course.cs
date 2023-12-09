using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    [Table("trainee_course")]
    public class Trainee_course
    {
        [Key]
        public long trainee_id { get; set; }
        [ForeignKey("course_id")]
        [Column("course_id", TypeName = "int"), Required]
        public int course_id { get; set;}
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set;}
        public DateTime? deleted_at { get; set; }
        public long userid { get; set; }
    }
}
