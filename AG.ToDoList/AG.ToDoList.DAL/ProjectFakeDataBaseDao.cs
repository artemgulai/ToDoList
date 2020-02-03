using AG.ToDoList.DAL.Interfaces;
using AG.ToDoList.Entities;
using AG.ToDoList.FakeDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG.ToDoList.DAL
{
    public class ProjectFakeDataBaseDao : IProjectDao
    {
        private DataBase _db = DataBase.Instance;

        public int DeleteById(int id)
        {
            var projects = _db.Projects;

            var removeResult = projects.Remove(id);
            if(removeResult)
            {
                var tasks = _db.Tasks;
                var idsToRemove = tasks.Where(a => a.Value.ProjectId == id).Select(a => a.Value.Id).ToList();
                foreach (var idToRemove in idsToRemove)
                {
                    tasks.Remove(idToRemove);
                }
                return 1;
            }

            return 0;
        }

        public Project Insert(Project project)
        {
            var projects = _db.Projects;

            int nextId = projects.Count() == 0 ? 1 : projects.Keys.Max() + 1;

            project.Id = nextId;

            projects.Add(nextId,project);

            return project;
        }

        public IEnumerable<Project> SelectAll()
        {
            return _db.Projects.Values;
        }

        public Project SelectById(int id)
        {
            _db.Projects.TryGetValue(id,out var project);
            return project;
        }

        public int Update(Project project)
        {
            var projects = _db.Projects;

            if (projects.ContainsKey(project.Id))
            {
                projects.Remove(project.Id);
                projects.Add(project.Id,project);
                return 1;
            }

            return 0;
        }
    }
}
