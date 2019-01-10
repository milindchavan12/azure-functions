using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace TodoFunction
{
    public class ToDo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("n");
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ToDCreateModel
    {
        public string TaskDescription { get; set; }
    }

    public class ToDoUpdateModel
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ToDoTableEntity : TableEntity
    {
        public DateTime CreateTime { get; set; }
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
