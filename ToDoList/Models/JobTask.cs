using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ToDoList.Models
{
    [Table("tasks")]
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

        [ScriptIgnore]
        public virtual Project Project { get; set; }
    }

}