using AG.ToDoList.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG.ToDoList.DAL.Interfaces
{
    public interface IProjectDao
    {
        Project Insert(Project project);

        Project SelectById(int id);

        IEnumerable<Project> SelectAll();

        int Update(Project project);

        int DeleteById(int id);
    }
}
