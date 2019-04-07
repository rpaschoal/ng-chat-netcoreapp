using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NgChatSignalR.Models;

namespace NgChatSignalR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Sending the userId from the request body as this is just a demo. 
        // On your application you probably want to fetch this from your authentication context and not receive it as a parameter
        public IActionResult ListFriends([FromBody] dynamic payload)
        {
            return Json(GroupChatHub.ConnectedParticipants((string)payload.currentUserId));

            // Use the following for group chats
            // Make sure you have [pollFriendsList] set to true for this simple group chat example to work as
            // broadcasting with group was not implemented here
            // return Json(GroupChatHub.ConnectedParticipants((string)payload.currentUserId));
        }

        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm(Name = "ng-chat-participant-id")] string userId)
        {
            // Storing file in temp path
            var filePath = Path.Combine(Path.GetTempPath(), file.FileName);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            var baseUri = new Uri($"{Request.Scheme}://{Request.Host}{Request.PathBase}");
            var fileUri = new Uri(baseUri, $"Uploads/{file.FileName}");

            return Ok(new
            {
                type = 2, // MessageType.File = 2
                //fromId: ngChatSenderUserId, fromId will be set by the angular component after receiving the http response
                toId = userId,
                message = file.FileName,
                mimeType = file.ContentType,
                fileSizeInBytes = file.Length,
                downloadUrl = fileUri.ToString()
            });
        }

        [Route("Uploads/{fileName}")]
        public async Task<IActionResult> Uploads(string fileName)
        {
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            var memory = new MemoryStream();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                if (stream == null)
                    return NotFound();

                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, "application/octet-stream");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
