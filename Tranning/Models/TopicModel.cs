﻿using System.ComponentModel.DataAnnotations;
using Tranning.DataDBContext;
using Tranning.Validations;

namespace Tranning.Models
{
    public class TopicModel
    {
        public List<TopicDetail> ListTopicDetail { get; set; }
    }
    public class TopicDetail
    {
        public long id { get; set; }

        [Required(ErrorMessage = "Please enter the course ID.")]
        public int course_id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string? videos { get; set; }
        public string? documents { get; set; }
        public string? attach_file { get; set; }

        [Required(ErrorMessage = "Please choose a file.")]
        public IFormFile fileVideos { get; set; }

        [Required(ErrorMessage = "Please choose a status.")]
        public string status { get; set; }

        [Required(ErrorMessage = "Please choose a file.")]
        [AllowedExtensionFile(new string[] { ".png", ".jpg", ".jpeg", ".gif" })]
        public IFormFile fileImage { get; set; }
        public IFormFile? fileDocument { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
    public class TopicMapping: Topic
    {
        public string CourseName { get; set; }
    }
}
