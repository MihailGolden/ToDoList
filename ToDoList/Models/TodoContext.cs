using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    public class TodoContext: DbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<JobTask> Tasks { get; set; }

        public System.Data.Entity.DbSet<ToDoList.Models.Project> Projects { get; set; }
    }
}