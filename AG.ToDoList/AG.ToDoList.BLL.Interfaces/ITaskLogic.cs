using AG.ToDoList.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AG.ToDoList.BLL.Interfaces
{
    public interface ITaskLogic
    {
        Task Insert(Task task);

        Task SelectById(int id);

        IEnumerable<Task> SelectAll();

        IEnumerable<Task> SelectByProjectId(int projectId);

        int Update(Task task);

        int DeleteById(int id);
    }
}
