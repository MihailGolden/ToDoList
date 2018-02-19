using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Core
{
    public class Task
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public List<Project> Projects { get; set; }
    }
}
