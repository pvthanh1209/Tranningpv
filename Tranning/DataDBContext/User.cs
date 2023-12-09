using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tranning.DataDBContext
{
    [Table("users")]
    public class Users
    {
        [Key]
        public long id { get; set; }
        public long role_id { get; set; }
        public string? extra_code { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set;}
        public int gender { get; set; } 
        public DateTime? birthday { get; set; }
        public string? avatar { get; set; }
        public DateTime? last_login { get; set; }
        public DateTime? last_logout { get; set; }
        public int status { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public string full_name { get; set; }
        public string? education { get; set; }
        public string? programming_language { get; set; }
        public int toeic_score { get; set; }
        public string? experience { get; set; }
        public string? department { get; set; }
        public int type { get; set; }
    }
}
