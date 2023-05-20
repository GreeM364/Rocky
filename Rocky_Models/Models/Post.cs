﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky_Models.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string ShortText { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Image { get; set; }
        public int Like { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public ApplicationUser CreatedBy { get; set; }
    }
}