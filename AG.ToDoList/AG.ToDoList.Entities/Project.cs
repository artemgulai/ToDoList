using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG.ToDoList.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        
        [JsonIgnore]
        public IEnumerable<Task> Tasks { get; set; }

        public override string ToString()
        {
            return $"ID {Id}: {Name}{(string.IsNullOrWhiteSpace(Comment) ? string.Empty : Environment.NewLine)}\t{Comment}";
        }
    }
}
