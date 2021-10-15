using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphAI.Core
{
    public class ToDoManager
    {
        private readonly string DumpListName = "GraphAI";

        private readonly GraphServiceClient _graphClient;

        public ToDoManager(GraphServiceClient graphServiceClient)
        {
            _graphClient = graphServiceClient;
        }

        public async Task<ITodoListsCollectionPage> GetAllTaskLists()
        {
            return await _graphClient
                    .Me
                    .Todo
                    .Lists
                    .Request()
                    .GetAsync();
        }

        public async Task<string> GetTaskListId(string listName, ITodoListsCollectionPage todoLists = default)
        {
            if (todoLists.Count == 0)
            {
                todoLists = await this.GetAllTaskLists();
            }

            if (todoLists.Any(e => e.DisplayName.Contains(listName)))
            {
                var taskListId = todoLists.CurrentPage.Where(e => e.DisplayName == listName).FirstOrDefault();
                return taskListId.Id;
            }

            return string.Empty;
        }

        public async Task<string> CreateGraphAIListIfDoesNotExist()
        {
            var allLists = await this.GetAllTaskLists();

            if (!allLists.Any(e => e.DisplayName.Contains(DumpListName)))
            {
                var todoTaskList = new TodoTaskList { DisplayName = DumpListName, ODataType = null };
                var createdTasklist = await this.CreateTaskLisk(todoTaskList);
                return createdTasklist.Id;
            }

            return await this.GetTaskListId(DumpListName, allLists);
        }

        public async Task<TodoTaskList> CreateTaskLisk(TodoTaskList todoTaskList)
        {
            return await 
                _graphClient
                .Me
                .Todo
                .Lists
                .Request()
                .AddAsync(todoTaskList);
        }

        public async Task<TodoTask> CreateTask(TodoTask todoTask, string taskListId)
        {
            return await 
                _graphClient
                .Me
                .Todo
                .Lists[taskListId]
                .Tasks
                .Request()
                .AddAsync(todoTask);
        }
    }
}
