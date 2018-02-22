using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    public class JobTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public DateTime? Deadline { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public int Priority { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }

}