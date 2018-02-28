using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ToDoList.Models
{
    [Table("projects")]
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name {get; set; }

        public virtual ICollection<JobTask> JobTasks { get; set; }

        [ScriptIgnore]
        public  ApplicationUser User { get; set; }

        public Project()
        {
            JobTasks = new List<JobTask>();
        }
    }
}