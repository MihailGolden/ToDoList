using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Interfaces
{
    interface ITaskRepository: IDisposable
    {
        IEnumerable<Task> GetTaskList();
        Task GetTask(int id);
        void Create(Task item);
        void Update(Task item);
        void Delete(int id);
        void Save();
    }
}
