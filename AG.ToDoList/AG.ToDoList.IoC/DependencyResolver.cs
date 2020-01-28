using AG.ToDoList.BLL;
using AG.ToDoList.BLL.Interfaces;
using AG.ToDoList.DAL;
using AG.ToDoList.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG.ToDoList.IoC
{
    public static class DependencyResolver
    {
        public static IProjectDao ProjectDao { get; }
        public static ITaskDao TaskDao { get; }

        public static IProjectLogic ProjectLogic { get; }
        public static ITaskLogic TaskLogic { get; }

        static DependencyResolver()
        {
            ProjectDao = new ProjectFakeDataBaseDao();
            TaskDao = new TaskFakeDataBaseDao();
            ProjectLogic = new ProjectLogic(ProjectDao);
            TaskLogic = new TaskLogic(TaskDao);
        }
    }
}
