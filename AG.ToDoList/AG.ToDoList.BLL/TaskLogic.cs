using AG.ToDoList.BLL.Interfaces;
using AG.ToDoList.DAL.Interfaces;
using AG.ToDoList.Entities;
using System.Collections.Generic;

namespace AG.ToDoList.BLL
{
    public class TaskLogic : ITaskLogic
    {
        private ITaskDao _dao;

        public TaskLogic(ITaskDao dao)
        {
            _dao = dao;
        }

        public int DeleteById(int id)
        {
            return _dao.DeleteById(id);
        }

        public Task Insert(Task task)
        {
            return _dao.Insert(task);
        }

        public IEnumerable<Task> SelectAll()
        {
            return _dao.SelectAll();
        }

        public IEnumerable<Task> SelectByProjectId(int projectId)
        {
            return _dao.SelectByProjectId(projectId);
        }

        public Task SelectById(int id)
        {
            return _dao.SelectById(id);
        }

        public int Update(Task task)
        {
            return _dao.Update(task);
        }
    }
}
