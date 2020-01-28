using AG.ToDoList.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG.ToDoList.BLL.Interfaces
{
    public interface IProjectLogic
    {
        Project Insert(Project project);

        Project SelectById(int id);

        IEnumerable<Project> SelectAll();

        int Update(Project project);

        int DeleteById(int id);
    }
}
