using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Final_E_Commerce.Controllers
{
    
    public class MessagesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser>? _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;
        public MessagesController(SignInManager<AppUser>? signInManager, UserManager<AppUser>? usermanager, AppDbContext context)
        {
            _signInManager = signInManager;
            _usermanager = usermanager;
            _context = context;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser=await _usermanager.GetUserAsync(User);
            bool everMessaged = await _context?.Communications?.AnyAsync(c=>c.AppUserId==currentUser.Id||c.OtherAppUserId==currentUser.Id);
            if (everMessaged)
            {
                everMessaged = true;
                List<ReceivedMsgVM> receivedMsgVMs= new List<ReceivedMsgVM>();
                List<Communication> communication = await _context.Communications.Where(c => c.AppUserId == currentUser.Id || c.OtherAppUserId== currentUser.Id).ToListAsync();
                
                foreach (var msg in communication)
                {
                    ReceivedMsgVM receivedVM = new ReceivedMsgVM();
                    if (msg.AppUserId==currentUser.Id)
                    {
                        receivedVM.Sender = await _usermanager.FindByIdAsync(msg.OtherAppUserId);
                    }
                    else if (msg.OtherAppUserId==currentUser.Id)
                    {
                        receivedVM.Sender= await _usermanager.FindByIdAsync(msg.AppUserId);
                        
                    }
                    int? unreadMessagesInThisCom = _context?.ChatMessages?.Where(c => c.OtherId == currentUser.Id && c.ReadByReceiver != true && c.CommunicationId == msg.Id).Count();
                    ChatMessage? lastUnreadMsg=await _context?.ChatMessages?.Where(c => c.OtherId == currentUser.Id && c.ReadByReceiver != true && c.CommunicationId == msg.Id).OrderByDescending(c=>c.Date).Take(1).FirstOrDefaultAsync();
                    ChatMessage? lastMsg = await _context?.ChatMessages?.Where(c => (c.OtherId == currentUser.Id && c.CommunicationId == msg.Id)||(c.AppuserId==currentUser.Id&&c.CommunicationId==msg.Id)).OrderBy(m=>m.Id).LastOrDefaultAsync();
                    receivedVM.UnreadMessageCount = unreadMessagesInThisCom;
                    receivedVM.LastMessageDate = lastMsg.Date;
                    receivedMsgVMs.Add(receivedVM);
                }
                return View(receivedMsgVMs);
            }
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Chat(string receiverId)
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            var receiver = await _usermanager.FindByIdAsync(receiverId);
            if (receiver==null||receiverId==null)
            {
                return Ok($"User with the id of {receiverId} not found");
            }
            ViewBag.Username = currentUser.UserName;
            ViewBag.CurrentUserId = currentUser.Id;
            ViewBag.ReceiverId = receiverId;
            ViewBag.ReceiverName = receiver.Fullname;
            ChatVM chatVM = new ChatVM();
            Communication? communication = await _context?.Communications?.FirstOrDefaultAsync(c => (c.AppUserId == currentUser.Id && c.OtherAppUserId == receiverId) || (c.OtherAppUserId == currentUser.Id && c.AppUserId == receiverId));
            if (communication != null)
            {
                List<ChatMessage>? messages = await _context?.ChatMessages?.OrderBy(c => c.Date).Where(c=>c.CommunicationId==communication.Id).ToListAsync();
                bool unreadMessages = await _context?.ChatMessages?.OrderByDescending(c => c.Date)
                    .Where(c => c.CommunicationId == communication.Id && c.OtherId == currentUser.Id && c.ReadByReceiver != true)
                    .AnyAsync();
                if (unreadMessages)
                {
                    var unreadMessagesByThisUser = await _context?.ChatMessages?
                        .Where(c => c.CommunicationId == communication.Id && c.OtherId == currentUser.Id && c.ReadByReceiver != true)
                        .ToListAsync();
                    foreach (var item in unreadMessagesByThisUser)
                    {
                        item.ReadByReceiver = true;
                    }
                    await _context.SaveChangesAsync();
                }
                chatVM.ChatMessages = messages;
                return View(chatVM);
            }
            return View(chatVM);
        }

        //realkhalid 887?
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MessageRead(string username, string receiverId)
        {
            AppUser user = await _usermanager.FindByNameAsync(username);
            Communication? communication = await _context?.Communications?
                .FirstOrDefaultAsync(c => (c.AppUserId == user.Id && c.OtherAppUserId == receiverId) || (c.OtherAppUserId == user.Id && c.AppUserId == receiverId));
            var unreadMessagesByThisUser = await _context?.ChatMessages?
                        .Where(c => c.CommunicationId == communication.Id && c.OtherId == user.Id && c.ReadByReceiver != true)
                        .ToListAsync();
            foreach (var item in unreadMessagesByThisUser)
            {
                item.ReadByReceiver = true;
            }
            await _context.SaveChangesAsync();
            return Ok("Last message read by user is set to be 'read' and saved in database");
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(string text, string receiverId)
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            bool didMessageToEachOther = await _context?.Communications?.AnyAsync(c => (c.AppUserId == currentUser.Id && c.OtherAppUserId == receiverId) || (c.OtherAppUserId == currentUser.Id && c.AppUserId == receiverId));
            if (!didMessageToEachOther)
            {
                Communication newCommunication = new Communication();
                newCommunication.AppUserId = currentUser.Id;
                newCommunication.OtherAppUserId = receiverId;
                await _context.Communications.AddAsync(newCommunication);
                await _context.SaveChangesAsync();


                ChatMessage newMessage = new ChatMessage();
                newMessage.Message= text;
                newMessage.Date= DateTime.Now.AddHours(12);
                newMessage.CommunicationId=newCommunication.Id;
                newMessage.AppuserId = currentUser.Id;
                newMessage.OtherId = receiverId;
                newMessage.ReadByReceiver = false;
                await _context.ChatMessages.AddAsync(newMessage);
                await _context.SaveChangesAsync();
                return Ok("new communication created and message attached to it successfully");
            }
            else
            {
                Communication? communication = await _context?.Communications?.FirstOrDefaultAsync(c => (c.AppUserId == currentUser.Id && c.OtherAppUserId == receiverId) || (c.OtherAppUserId == currentUser.Id && c.AppUserId == receiverId));
                ChatMessage message = new ChatMessage();
                message.Date = DateTime.Now.AddHours(12);
                message.Message= text;
                message.CommunicationId = communication.Id;
                message.AppuserId = currentUser.Id;
                message.ReadByReceiver = false;
                message.OtherId= receiverId;
                await _context.ChatMessages.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok("message successfully attached to the existing communication");
            }
        }
    }
}
