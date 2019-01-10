using System;
namespace TodoFunction
{
    public static class Mappings
    {
        public static ToDoTableEntity ToTableEntity(this ToDo todo)
        {
            return new ToDoTableEntity
            {
                PartitionKey = "TODO",
                RowKey = todo.Id,
                CreateTime = todo.CreateTime,
                TaskDescription = todo.TaskDescription,
                IsCompleted = todo.IsCompleted
            };
        }

        public static ToDo ToToDo(this ToDoTableEntity tableEntity)
        {
            return new ToDo
            {
                Id = tableEntity.RowKey,
                CreateTime = tableEntity.CreateTime,
                IsCompleted = tableEntity.IsCompleted,
                TaskDescription = tableEntity.TaskDescription
            };
        }
    }
}
