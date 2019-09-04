﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Administrator
    {
        public int Id { get; set; }

        [Required , MaxLength(128)]
        public string Name { get; set; }

        [Required, MaxLength(128),]
        public string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress) , MaxLength(128)]
        public string Email { get; set; }

        [Required, MaxLength(128)]
        public string Password { get; set; }

        public DateTime? Birthdate { get; set; }

        [MaxLength(200)]
        public string Education { get; set; }

        [MaxLength(20)]
        public string Country { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        [MaxLength(200)]
        public string PicUrl { get; set; }
    }
}