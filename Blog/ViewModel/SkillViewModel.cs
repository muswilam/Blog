﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Blog.Models;

namespace Blog.ViewModel
{
    public class SkillViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(128)]
        public string Type { get; set; }

        //nav props 
        public ICollection<Administrator> Administrators { get; set; }
    }
}