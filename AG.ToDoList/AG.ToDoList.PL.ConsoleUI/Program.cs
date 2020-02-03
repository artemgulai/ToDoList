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
                Console.WriteLine("Projects:");
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
                        {
                            var project = GetProjectById(projects);
                            EditTasks(project);
                            break;
                        }
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

        private static Project GetProjectById(IEnumerable<Project> projects)
        {
            while(true)
            {
                Console.WriteLine("Enter the ID of a project:");
                var id = GetId();
                var project = projects.FirstOrDefault(a => a.Id == id);
                if (project == null)
                {
                    Console.WriteLine("No project with such ID.");
                    continue;
                }
                return project;
            }
        }

        private static void EditTasks(Project project)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Project:{Environment.NewLine}{project}");
                var tasks = GetTasksByProject(project);
                ShowTasksList(tasks);
                Console.WriteLine();
                TaskMenu();

                int select;
                if (!int.TryParse(Console.ReadLine(),out select))
                {
                    Console.WriteLine("WRONG INPUT");
                    Console.ReadLine();
                    continue;
                }

                switch (select)
                {
                    case 1:
                        AddTask(project);
                        Console.ReadLine();
                        break;
                    case 2:
                        RenameTask(tasks);
                        Console.ReadLine();
                        break;
                    case 3:
                        ChangeTaskComment(tasks);
                        Console.ReadLine();
                        break;
                    case 4:
                        ChangeDueDate(tasks);
                        Console.ReadLine();
                        break;
                    case 5:
                        {
                            ChangeDoneMark(tasks);
                            Console.ReadLine();
                            break;
                        }
                    case 6:
                        {
                            RemoveTask(tasks);
                            Console.ReadLine();
                            break;
                        }
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

            }
        }

        private static void RenameTask(IEnumerable<Task> tasks)
        {
            PrepareTaskChange(tasks,"title",out var task,out var newTitle);

            if (task == null)
            {
                return;
            }

            task.Title = newTitle;

            if (_taskLogic.Update(task) == 1)
            {
                Console.WriteLine("Task's title has been changed.");
            }
            else
            {
                Console.WriteLine("Cannot change task's title.");
            }
        }

        private static void ChangeTaskComment(IEnumerable<Task> tasks)
        {
            PrepareTaskChange(tasks,"comment",out var task,out var newComment);

            if(task == null)
            {
                return;
            }

            task.Comment = newComment;

            if (_taskLogic.Update(task) == 1)
            {
                Console.WriteLine("Task's comment has been changed.");
            }
            else
            {
                Console.WriteLine("Cannot change task's comment.");
            }
        }

        private static void ChangeDueDate(IEnumerable<Task> tasks)
        {
            var task = SelectTaskFromProject(tasks);
            if (task == null)
            {
                Console.WriteLine("No task with such ID.");
                return;
            }

            DateTime newDueDate;
            Console.WriteLine("Enter date and time in the following format: DD-MM-YYYY HH:MM");
            while (!DateTime.TryParse(Console.ReadLine(), out newDueDate))
            {
                Console.WriteLine("Wrong date format. Try again.");
            }

            task.DueDate = newDueDate;
            if (_taskLogic.Update(task) == 1)
            {
                Console.WriteLine("Task's due date has been changed.");
            }
            else
            {
                Console.WriteLine("Cannot change task's due date.");
            }
        }

        private static void ChangeDoneMark(IEnumerable<Task> tasks)
        {
            var task = SelectTaskFromProject(tasks);
            if (task == null)
            {
                Console.WriteLine("No task with such ID.");
                return;
            }

            task.IsDone = !task.IsDone;
            if (_taskLogic.Update(task) == 1)
            {
                Console.WriteLine("Task's done mark has been changed.");
            }
            else
            {
                Console.WriteLine("Cannot change task's done mark.");
            }
        }

        private static void PrepareTaskChange(IEnumerable<Task> tasks, string propertyName,
                                              out Task task, out string newPropertyValue)
        {
            task = SelectTaskFromProject(tasks);
            if (task == null)
            {
                Console.WriteLine("No task with such ID.");
                newPropertyValue = "";
                return;
            }
            Console.WriteLine($"Enter the task's new {propertyName}:");
            while (string.IsNullOrWhiteSpace(newPropertyValue = Console.ReadLine()) && propertyName.Equals("title"))
            {
                Console.WriteLine($"Task's {propertyName} cannot be empty or consist only of whitespace. Try again.");
            }
        }

        private static Task SelectTaskFromProject(IEnumerable<Task> tasks)
        {
            Console.WriteLine("Enter ID of a task you want to change:");
            var id = GetId();

            return tasks.FirstOrDefault(a => a.Id == id);
        }

        private static void AddTask(Project project)
        {
            var task = CreateTask();
            task.ProjectId = project.Id;

            _taskLogic.Insert(task);
            Console.WriteLine($"The task has been added. Task's ID is {task.Id}.");
        }

        private static Task CreateTask()
        {
            Console.WriteLine("Enter task's title:");
            string title;
            while (string.IsNullOrWhiteSpace(title = Console.ReadLine()))
            {
                Console.WriteLine("Title cannot be empty or consist only of whitespace.");
            }

            Console.WriteLine("Enter task's comment:");
            string comment = Console.ReadLine();
            //while (string.IsNullOrWhiteSpace(comment = Console.ReadLine()))
            //{
            //    Console.WriteLine("Comment cannot be empty or consist only of whitespace.");
            //}

            DateTime dueDate;
            Console.WriteLine("Enter date and time in the following format: DD-MM-YYYY HH:MM");
            while (true)
            {
                string stringDate = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(stringDate))
                {
                    dueDate = DateTime.Now.AddDays(1);
                    break;
                }

                if (!DateTime.TryParse(stringDate,out dueDate))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                break;
            }

            return new Task
            {
                Title = title,
                Comment = comment,
                DueDate = dueDate,
                IsDone = false
            };
        }

        private static void RemoveTask(IEnumerable<Task> tasks)
        {
            Console.WriteLine("Enter ID of a task you want to remove:");
            var id = GetId();

            var task = tasks.FirstOrDefault(a => a.Id == id);
            if (task == null)
            {
                Console.WriteLine("No task with such ID.");
                return;
            }

            if (_taskLogic.DeleteById(id) == 1)
            {
                Console.WriteLine("Task has been deleted.");
                return;
            }

            Console.WriteLine("Task cannot be deleted.");
        }

        private static void TaskMenu()
        {
            Console.WriteLine("Select what to do:");
            Console.WriteLine("1. Add task.");
            Console.WriteLine("2. Rename task.");
            Console.WriteLine("3. Change task's comment.");
            Console.WriteLine("4. Change due date");
            Console.WriteLine("5. Change \"done\" mark");
            Console.WriteLine("6. Remove task.");
            Console.WriteLine($"0. Exit.{Environment.NewLine}");
        }

        /// <summary>
        /// Gets a project's name and comment from a user
        /// and inserts it into the DB
        /// </summary>
        private static void AddProject()
        {
            Console.WriteLine("Enter a project's name:");
            string name;
            while (string.IsNullOrWhiteSpace(name = Console.ReadLine()))
            {
                Console.WriteLine("Project's name cannot be empty or consist only of whitespace.");
            }

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

        private static void ShowTasksList(IEnumerable<Task> tasks)
        {
            if (tasks.Count() == 0)
            {
                Console.WriteLine("No tasks.");
                return;
            }

            Console.WriteLine($"{Environment.NewLine}Tasks:");
            foreach (var task in tasks)
            {
                Console.WriteLine(task.ToString() + Environment.NewLine);
            }
        }
    }
}
