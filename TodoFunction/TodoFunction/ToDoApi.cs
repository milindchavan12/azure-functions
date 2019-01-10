using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace TodoFunction
{
    public static class TodoApi
    {
        [FunctionName("CreateToDo")]
        public static async Task<IActionResult> CreateToDo([HttpTrigger(AuthorizationLevel.Anonymous,
            "post", Route = "todo")]HttpRequest req, 
            [Table("todos", Connection = "AzureWebJobsStorage")] IAsyncCollector<ToDoTableEntity> todoTable,
            [Queue("todos", Connection = "AzureWebJobsStorage")] IAsyncCollector<ToDo> todoQueue,
            ILogger log)
        {
            log.LogInformation("Creating new todo list item.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ToDCreateModel>(requestBody);

            var todo = new ToDo { TaskDescription = input.TaskDescription };
            await todoTable.AddAsync(todo.ToTableEntity());
            await todoQueue.AddAsync(todo);
            return new OkObjectResult(todo);
        }

        [FunctionName("GetToDos")]
        public static async Task<IActionResult> GetToDos([HttpTrigger(AuthorizationLevel.Anonymous,
            "get", Route = "todo")]HttpRequest req,
            [Table("todos", Connection = "AzureWebJobsStorage")] CloudTable todoTable, 
            ILogger log)
        {
            log.LogInformation("Getting todo list.");

            var query = new TableQuery<ToDoTableEntity>();
            var segment = await todoTable.ExecuteQuerySegmentedAsync(query, null);
            return new OkObjectResult(segment.Select(Mappings.ToToDo));
        }

        [FunctionName("GetToDoById")]
        public static async Task<IActionResult> GetToDoById([HttpTrigger(AuthorizationLevel.Anonymous,
            "get", Route = "todo/{id}")]HttpRequest req,
            [Table("todos", "TODO", "{id}", Connection = "AzureWebJobsStorage")] ToDoTableEntity todo,
            ILogger log, string id)
        {
            log.LogInformation("Getting todo item by Id");

            if(todo == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(todo.ToToDo());
        }

        [FunctionName("UpdateToDo")]
        public static async Task<IActionResult> UpdateToDo([HttpTrigger(AuthorizationLevel.Anonymous,
            "put", Route = "todo")]HttpRequest req,
            [Table("todos", Connection = "AzureWebJobsStorage")] CloudTable todoTable,
            ILogger log, string id)
        {
            log.LogInformation("updating todo list item.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<ToDoUpdateModel>(requestBody);
            var findOperation = TableOperation.Retrieve<ToDoTableEntity>("TODO", id);
            var findResult = await todoTable.ExecuteAsync(findOperation);

            if (findResult.Result == null)
            {
                return new NotFoundResult();
            }

            var existingRow = (ToDoTableEntity)findResult.Result;
            existingRow.IsCompleted = updated.IsCompleted;
            if (!string.IsNullOrEmpty(updated.TaskDescription))
            {
                existingRow.TaskDescription = updated.TaskDescription;
            }

            var replaceOperation = TableOperation.Replace(existingRow);
            await todoTable.ExecuteAsync(replaceOperation);
            return new OkObjectResult(existingRow.ToToDo());
        }

        [FunctionName("DeleteToDo")]
        public static async Task<IActionResult> DeleteToDo([HttpTrigger(AuthorizationLevel.Anonymous,
            "delete", Route = "todo/{id}")]HttpRequest req,
            [Table("todos", Connection = "AzureWebJobsStorage")] CloudTable todoTable,
            ILogger log, string id)
        {
            log.LogInformation("Deleting todo item by Id");

            var deleteOperation = TableOperation.Delete(new TableEntity
            {
                PartitionKey = "TODO",
                RowKey = id,
                ETag = "*"
            });

            try
            {
                await todoTable.ExecuteAsync(deleteOperation);
            }
            catch (StorageException e) when (e.RequestInformation.HttpStatusCode == 400)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }
    }
}
