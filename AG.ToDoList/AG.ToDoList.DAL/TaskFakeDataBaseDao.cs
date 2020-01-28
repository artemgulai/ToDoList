using AG.ToDoList.DAL.Interfaces;
using AG.ToDoList.FakeDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AG.ToDoList.Entities;

namespace AG.ToDoList.DAL
{
    public class TaskFakeDataBaseDao : ITaskDao
    {
        private DataBase _db = DataBase.Instance;

        public int DeleteById(int id)
        {
            var tasks = _db.Tasks;

            return tasks.Remove(id) ? 1 : 0;
        }

        public Task Insert(Task task)
        {
            var tasks = _db.Tasks;
            
            if (_db.Projects.ContainsKey(task.ProjectId))
            {
                int nextId = tasks.Count() == 0 ? 1 : tasks.Keys.Max() + 1;
                task.Id = nextId;
                tasks.Add(nextId,task);
            }
            return task;
        }

        public IEnumerable<Task> SelectAll()
        {
            return _db.Tasks.Values;
        }

        public IEnumerable<Task> SelectByProjectId(int projectId)
        {
            return _db.Tasks.Values.Where(a => a.ProjectId == projectId);
        }

        public Task SelectById(int id)
        {
            _db.Tasks.TryGetValue(id,out var task);
            return task;
        }

        public int Update(Task task)
        {
            var tasks = _db.Tasks;
            
            if (tasks.ContainsKey(task.Id))
            {
                tasks.Remove(task.Id);
                tasks.Add(task.Id,task);
                return 1;
            }

            return 0;
        }
    }
}
