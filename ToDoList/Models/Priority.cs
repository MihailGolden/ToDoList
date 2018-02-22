using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    public enum Change : sbyte { Up = 1, Down = -1 }

    public class Priority
    {
        public int ProjectId { get; set; }
        public int CurrentId { get; set; }
        public int CurrentPriority { get; set; }
        public Change Change { get; set; }
        public int TargetId { get; set; }
        public int TargetPriority { get; set; }
    }
}