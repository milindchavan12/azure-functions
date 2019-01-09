using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TodoFunction
{
    public static class TodoApi
    {
        static List<ToDo> items = new List<ToDo>();
        [FunctionName("CreateToDo")]
        public static async Task<IActionResult> CreateToDo([HttpTrigger(AuthorizationLevel.Anonymous,
                            "post", Route = "todo")]HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating new todo list item.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ToDCreateModel>(requestBody);

            var todo = new ToDo { TaskDescription = input.TaskDescription };
            items.Add(todo);

            return new OkObjectResult(todo);
        }

        [FunctionName("GetToDos")]
        public static async Task<IActionResult> GetToDos([HttpTrigger(AuthorizationLevel.Anonymous,
                            "get", Route = "todo")]HttpRequest req, ILogger log)
        {
            log.LogInformation("Getting todo list.");

            return new OkObjectResult(items);
        }

        [FunctionName("GetToDoById")]
        public static async Task<IActionResult> GetToDoById([HttpTrigger(AuthorizationLevel.Anonymous,
                            "get", Route = "todo/{id}")]HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("Getting todo item by Id");

            var todo = items.FirstOrDefault(t => t.Id == id);
            if(todo == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(items);
        }

        [FunctionName("UpdateToDo")]
        public static async Task<IActionResult> UpdateToDo([HttpTrigger(AuthorizationLevel.Anonymous,
                            "put", Route = "todo")]HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("updating todo list item.");

            var todo = items.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<ToDoUpdateModel>(requestBody);

            todo.IsCompleted = updated.IsCompleted;
            if (!string.IsNullOrEmpty(updated.TaskDescription))
            {
                todo.TaskDescription = updated.TaskDescription;
            }
            return new OkObjectResult(todo);
        }

        [FunctionName("DeleteToDo")]
        public static async Task<IActionResult> DeleteToDo([HttpTrigger(AuthorizationLevel.Anonymous,
                            "delete", Route = "todo/{id}")]HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("Deleting todo item by Id");

            var todo = items.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            items.Remove(todo);
            return new OkResult();
        }
    }
}
