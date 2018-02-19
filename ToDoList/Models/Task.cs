using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required,]
        public string Name { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}