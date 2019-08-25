using Kosystem.Core;
using Kosystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Kosystem.Web.Hubs
{
    public class KoHub : Hub
    {
        private readonly IRoomService roomService;

        public KoHub(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        public Task Enqueue(string roomId, string userId)
        {
            return Task.CompletedTask;
        }

    }
}
