using System;
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
}
