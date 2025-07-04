using ChatBot_API.Models;
using System.Security.Claims;
using ChatBot_API.Repositoty;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatBot_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ChatBot_API.Controllers
{
    [Authorize]
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITavilyService _tavilyService;
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatController(IUnitOfWork unitOfWork, ITavilyService tavilyService, IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _tavilyService = tavilyService;
            _hubContext = hubContext;
        }
 

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid user.");

            var userMessage = new ChatMessage
            {
                UserId = userId,
                SessionId = request.SessionId,
                Sender = "user",
                Message = request.Message,
                Timestamp = DateTime.UtcNow
            };

            try
            {
               
                await _unitOfWork.ChatMessages.AddAsync(userMessage);
                await _unitOfWork.SaveAsync();
            }
           
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"DB Error (user message): {ex.InnerException?.Message ?? ex.Message}");
            }

            try
            {
               
                var botReply = await _tavilyService.GetBotResponseAsync(request.Message);
                
                if (string.IsNullOrWhiteSpace(botReply))
                {
                    botReply = "Sorry, I couldn't generate a response right now.";
                }

                var botMessage = new ChatMessage
                {
                    UserId = userId,
                    SessionId = request.SessionId,
                    Sender = "bot",
                    Message = botReply,
                    Timestamp = DateTime.UtcNow
                };

                
                await _unitOfWork.ChatMessages.AddAsync(botMessage);
                await _unitOfWork.SaveAsync();

               
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", userMessage.Sender, userMessage.Message);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", botMessage.Sender, botMessage.Message);

                return Ok(new { user = userMessage, bot = botMessage });
            }

            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"DB Error (bot message): {ex.InnerException?.Message ?? ex.Message}");
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Tavily AI failed: {ex.Message}");
          
            }
        }




        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(int page = 1, int pageSize = 20)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = await _unitOfWork.ChatMessages.GetPaginatedMessagesAsync(userId, page, pageSize);

            return Ok(messages);
        }



        //[HttpPut("{id}")]
        //public async Task<IActionResult> EditMessage(int id, [FromBody] string updatedText)
        //{
        //    var message = await _unitOfWork.ChatMessages.GetByIdAsync(id);
        //    if (message == null || message.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        //        return NotFound();

        //    var edit = new MessageEdit
        //    {
        //        ChatMessageId = message.Id,
        //        PreviousMessage = message.Message,
        //        EditedAt = DateTime.UtcNow
        //    };

        //    message.Message = updatedText;

        //    await _unitOfWork.MessageEdits.AddAsync(edit);
        //    await _unitOfWork.SaveAsync();


        //    return Ok(message);
        //}


        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] string updatedText)
        {
            var message = await _unitOfWork.ChatMessages.GetByIdAsync(id);
            if (message == null || message.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            // Save edit history
            var edit = new MessageEdit
            {
                ChatMessageId = message.Id,
                PreviousMessage = message.Message,
                EditedAt = DateTime.UtcNow
            };

            // Update user message
            message.Message = updatedText;

            await _unitOfWork.MessageEdits.AddAsync(edit);
            await _unitOfWork.SaveAsync();

            // If it's a user message, update the corresponding bot reply
            if (message.Sender == "user")
            {
                // Find bot reply for this user message (you may need to link them in your model ideally)
                var botReply = await _unitOfWork.ChatMessages.GetBotReplyForUserMessage(message);

                if (botReply != null)
                {
                    _unitOfWork.ChatMessages.Delete(botReply); // or soft delete
                }

                //if (botReply != null)
                //{
                //    botReply.IsDeleted = true;
                //    await _unitOfWork.SaveAsync();
                //}


                // Generate new bot reply
                var newBotReplyText = await _tavilyService.GetBotResponseAsync(updatedText) ?? "Sorry, I couldn't regenerate a reply.";

                var newBotMessage = new ChatMessage
                {
                    UserId = message.UserId,
                    SessionId = message.SessionId,
                    Sender = "bot",
                    Message = newBotReplyText,
                    Timestamp = DateTime.UtcNow
                };

                await _unitOfWork.ChatMessages.AddAsync(newBotMessage);
                await _unitOfWork.SaveAsync();

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "user", message.Message);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "bot", newBotMessage.Message);

                return Ok(new { user = message, bot = newBotMessage });
            }

            return Ok(message);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.ChatMessages.GetByIdAsync(id);
            if (message == null || message.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            message.IsDeleted = true;
            await _unitOfWork.SaveAsync();
           
            return NoContent();
        }

        
        [Authorize(Roles = "admin")]

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> ApproveMessage(int id, bool updatedText)
        {
            var message = await _unitOfWork.ChatMessages.GetByIdAsync(id);
            if (message == null)
                return NotFound();

            message.IsApproved = updatedText;
            await _unitOfWork.SaveAsync();
         
            return Ok(message);
        }
    }
}
