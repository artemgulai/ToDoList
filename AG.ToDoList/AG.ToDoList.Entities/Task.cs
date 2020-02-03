using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG.ToDoList.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsDone { get; set; }
        public bool IsExpired => DateTime.Now > DueDate;
        public int ProjectId { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}. {Title}{Environment.NewLine}{Comment}{(string.IsNullOrWhiteSpace(Comment) ? string.Empty : Environment.NewLine)}" +
                $"{DueDate}; done: {IsDone}; expired: {IsExpired}";
        }
    }
}
