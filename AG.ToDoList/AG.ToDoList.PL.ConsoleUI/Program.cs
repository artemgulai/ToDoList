using AG.ToDoList.BLL.Interfaces;
using AG.ToDoList.Entities;
using AG.ToDoList.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AG.ToDoList.PL.ConsoleUI
{
    public class Program
    {
        private static ITaskLogic _taskLogic;
        private static IProjectLogic _projectLogic;

        static void Main(string[] args)
        {
            _taskLogic = DependencyResolver.TaskLogic;
            _projectLogic = DependencyResolver.ProjectLogic;
            RunApp();
        }
        
        private static void RunApp()
        {
            var projects = _projectLogic.SelectAll();
            while (true)
            {
                Console.Clear();
                ShowProjectsList(projects);
                Console.WriteLine();
                ProjectMenu();

                int select;
                if (!int.TryParse(Console.ReadLine(), out select))
                {
                    Console.WriteLine("WRONG INPUT");
                    Console.ReadLine();
                    continue;
                }

                switch (select)
                {
                    case 1:
                        AddProject();
                        Console.ReadLine();
                        break;
                    case 2:
                        RenameProject(projects);
                        Console.ReadLine();
                        break;
                    case 3:
                        ChangeCommentProject(projects);
                        Console.ReadLine();
                        break;
                    case 4:
                        RemoveProject(projects);
                        Console.ReadLine();
                        break;
                    case 5:
                    case 0:
                        {
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Wrong number. Try again.");
                            Console.ReadLine();
                            break;
                        }
                }

                projects = _projectLogic.SelectAll();
            }
        }

        /// <summary>
        /// Gets a project's name and comment from a user
        /// and inserts it into the DB
        /// </summary>
        private static void AddProject()
        {
            Console.WriteLine("Enter a project's name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter a comment to the project:");
            string comment = Console.ReadLine();

            var project = new Project
            {
                Name = name,
                Comment = comment
            };
            _projectLogic.Insert(project);
            Console.WriteLine($"The project has been added. Project's ID is {project.Id}");
        }

        private static void RenameProject(IEnumerable<Project> projects)
        {
            PrepareProjectChange(projects,"name",out var project,out var newProperty);
            if (project == null || string.IsNullOrWhiteSpace(newProperty))
            {
                return;
            }

            project.Name = newProperty;
            _projectLogic.Update(project);
            Console.WriteLine("Project's name is changed.");
        }

        private static void ChangeCommentProject(IEnumerable<Project> projects)
        {
            PrepareProjectChange(projects,"comment",out var project,out var newProperty);
            if (project == null || string.IsNullOrWhiteSpace(newProperty))
            {
                return;
            }

            project.Comment = newProperty;
            _projectLogic.Update(project);
            Console.WriteLine("Project's comment is changed.");
        }

        private static void PrepareProjectChange(IEnumerable<Project> projects, string propertyName, 
                                                out Project project, out string newPropertyValue)
        {
            Console.WriteLine($"Enter ID of a project you want to change {propertyName}:");
            int id = GetId();

            project = projects.FirstOrDefault(a => a.Id == id);

            if (project == null)
            {
                Console.WriteLine("No project with such ID.");
                newPropertyValue = "";
                return;
            }

            Console.WriteLine($"Enter the project's new {propertyName}:");
            newPropertyValue = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newPropertyValue))
            {
                Console.WriteLine($"Project's {propertyName} cannot be empty or consist only of whitespace");
                return;
            }
        }

        private static void RemoveProject(IEnumerable<Project> projects)
        {
            Console.WriteLine("Enter ID of a project you want to remove:");
            int id = GetId();
            var project = projects.FirstOrDefault(a => a.Id == id);
            if (project == null)
            {
                Console.WriteLine("No project with such ID.");
                return;
            }

            if (_projectLogic.DeleteById(id) == 1)
            {
                Console.WriteLine("Project has been deleted.");
                return;
            }

            Console.WriteLine("Project cannot be deleted.");
        }

        /// <summary>
        /// Gets Tasks of a specified Project by its ID.
        /// </summary>
        /// <param name="project">Project for which tasks are gotten.</param>
        /// <returns>A collection of Tasks.</returns>
        private static IEnumerable<Task>GetTasksByProject(Project project)
        {
            return _taskLogic.SelectByProjectId(project.Id);
        }

        private static IEnumerable<Project>GetAllProjects()
        {
            return _projectLogic.SelectAll();
        }

        /// <summary>
        /// Prints possible actions for project list.
        /// </summary>
        private static void ProjectMenu()
        {
            Console.WriteLine("Select what to do:");
            Console.WriteLine("1. Add project.");
            Console.WriteLine("2. Rename project.");
            Console.WriteLine("3. Change project's comment.");
            Console.WriteLine("4. Remove project.");
            Console.WriteLine("5. Go to project # ...");
            Console.WriteLine($"0. Exit.{Environment.NewLine}");
        }

        private static int GetId()
        {
            int number;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(),out number))
                {
                    Console.WriteLine("Wrong input. Try again.");
                    continue;
                }

                if (number <= 0)
                {
                    Console.WriteLine("ID should be positive. Try again.");
                    continue;
                }

                return number;
            }
        }


        /// <summary>
        /// Prints a list of all projects to the console.
        /// </summary>
        /// <param name="projects">A collection of Projects.</param>
        private static void ShowProjectsList(IEnumerable<Project> projects)
        {
            if (projects.Count() == 0)
            {
                Console.WriteLine("No projects.");
                return;
            }

            foreach (var project in projects)
            {
                Console.WriteLine(project);
            }
        }
    }
}
