using AG.ToDoList.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace AG.ToDoList.FakeDataBase
{
    /// <summary>
    /// Singleton implementation of fake DB
    /// </summary>
    public class DataBase
    {
        private string _pathToTasks = @".tasks.json";
        private string _pathToProjects = @".projects.json";
        private static DataBase _instance;

        private DataBase()
        {
            Tasks = ReadOrCreate<Task>(_pathToTasks);
            Projects = ReadOrCreate<Project>(_pathToProjects);
        }

        public Dictionary<int, Task> Tasks { get; }
        public Dictionary<int, Project> Projects { get; }
        public static DataBase Instance 
        {
            get => _instance == null ? _instance = new DataBase() : _instance;
        }

        /// <summary>
        /// Serializes a collection to JSON and saves into file
        /// </summary>
        /// <param name="path">Path to file for saving collection</param>
        /// <param name="collection">Collection to be serialized and saved</param>
        private void SaveCollection<T>(string path, IDictionary<int, T> collection)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (var writer = new StreamWriter(new FileStream(path,FileMode.Create)))
            {
                writer.Write(JsonConvert.SerializeObject(collection));
            }
        }

        /// <summary>
        /// Reads and deserializes a collection or create empty one
        /// if file doesn't exist
        /// </summary>
        /// <param name="path">Path to file for reading collection</param>
        /// <returns>Deserialized or created collectio</returns>
        private Dictionary<int, T> ReadOrCreate<T>(string path)
        {
            if (File.Exists(path))
            {
                using (var streamReader = new StreamReader(File.Open(path,FileMode.Open)))
                {
                    string fileContent = streamReader.ReadLine();
                    return JsonConvert.DeserializeObject<Dictionary<int,T>>(fileContent);
                }
            }

            return new Dictionary<int,T>();
        }

        ~DataBase()
        {
            SaveCollection(_pathToProjects,Projects);
            SaveCollection(_pathToTasks,Tasks);
        }
    }
}
