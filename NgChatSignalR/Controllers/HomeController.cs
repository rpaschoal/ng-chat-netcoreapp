using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NgChatSignalR.Models;

namespace NgChatSignalR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("ng-chat signalr demo running.");
        }

        // Sending the userId from the request body as this is just a demo. 
        // On your application you probably want to fetch this from your authentication context and not receive it as a parameter
        public IActionResult ListFriends([FromBody] dynamic payload)
        {
            return Json(Chat.ConnectedParticipants((string)payload.currentUserId));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
