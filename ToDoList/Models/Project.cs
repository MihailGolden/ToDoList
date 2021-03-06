﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name {get; set; }

        public virtual ICollection<JobTask> JobTasks { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Project()
        {
            JobTasks = new List<JobTask>();
        }
    }
}