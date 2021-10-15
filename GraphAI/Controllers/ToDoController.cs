using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphAI.Core;
using Microsoft.CognitiveServices.Speech;

namespace GraphAI.Controllers
{
    public class ToDoController : Controller
    {

        private readonly GraphServiceClient _graphClient;
        private readonly ToDoManager _toDoManager;
        private readonly SpeechServiceManager _speechServiceManager;
        private readonly ILogger<HomeController> _logger;

        public ToDoController(
           ToDoManager toDoManager,
           GraphServiceClient graphClient,
           SpeechServiceManager speechServiceManager,
           ILogger<HomeController> logger)
        {
            _logger = logger;
            _graphClient = graphClient;
            _toDoManager = toDoManager;
            _speechServiceManager = speechServiceManager;
        }

        // <IndexSnippet>
        // Minimum permission scope needed for this view
        [AuthorizeForScopes(Scopes = new[] { "User.Read" })]
        public IActionResult Index()
        {
            return View();
        }
        // </IndexSnippet>

        // <RecordSnippet>
        // Minimum permission scope needed for this view
        [AuthorizeForScopes(Scopes = new[] { "Tasks.ReadWrite" })]
        public async Task<IActionResult> Record()
        {
            try
            {
                var taskTitle = await _speechServiceManager.CaptureAudioFromMicrophone();
                if (string.IsNullOrEmpty(taskTitle))
                {
                    return RedirectToAction("Index").WithError($"No audio was captured.");
                }

                var graphAITaskListid = await _toDoManager.CreateGraphAIListIfDoesNotExist();

                var todoTask = new TodoTask
                {
                    Title = taskTitle,
                    ODataType = null
                };

                var task = await _toDoManager.CreateTask(todoTask, graphAITaskListid);

                return RedirectToAction("Index").WithSuccess($"Item with title: '{taskTitle}' was added in the list GraphAI.");
            }
            catch (ServiceException ex)
            {
                if (ex.InnerException is MicrosoftIdentityWebChallengeUserException)
                {
                    throw;
                }

                return View()
                    .WithError("Error Adding Items in the list", ex.Message);
            }
        }
        // </RecordSnippet>
    }
}
