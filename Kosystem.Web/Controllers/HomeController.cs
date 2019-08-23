using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kosystem.Core;
using Microsoft.AspNetCore.Mvc;

namespace Kosystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoomService roomService;

        public HomeController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await roomService.ListRoomsAsync();
            return View(rooms);
        }

        [Route("/Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}