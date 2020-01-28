using AG.ToDoList.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AG.ToDoList.BLL.Interfaces;
using AG.ToDoList.Entities;

namespace AG.ToDoList.BLL
{
    public class ProjectLogic : IProjectLogic
    {
        private IProjectDao _dao;

        public ProjectLogic(IProjectDao dao)
        {
            _dao = dao;
        }

        public int DeleteById(int id)
        {
            return _dao.DeleteById(id);
        }

        public Project Insert(Project project)
        {
            return _dao.Insert(project);
        }

        public IEnumerable<Project> SelectAll()
        {
            return _dao.SelectAll();
        }

        public Project SelectById(int id)
        {
            return _dao.SelectById(id);
        }

        public int Update(Project project)
        {
            return _dao.Update(project);
        }
    }
}
