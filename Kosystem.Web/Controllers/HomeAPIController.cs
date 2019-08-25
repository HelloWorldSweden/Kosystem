using Kosystem.Core;
using Kosystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;
using System.Threading.Tasks;

namespace Kosystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeAPIController : ControllerBase
    {
        private readonly IRoomService roomService;

        public HomeAPIController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        public async Task<JsonResult> JoinRoom(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ModelStateError();
            }

            Room room = await roomService.GetRoomByIdAsync(model.RoomId);
            if (room == null)
            {
                ModelState.AddModelError(nameof(model.RoomId), $"Room does not exist.");
                return ModelStateError();
            }

            User user = await roomService.GetUserByIdAsync(room, model.UserId);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.UserId), $"User already joined room.");
                return ModelStateError();
            } else
            {
                user = new User
                {
                    Id = model.UserId,
                    Name = model.UserName
                };
            }

            await roomService.RegisterUserAsync(room, user);
            return Success();
        }

        private JsonResult ModelStateError()
        {
            return new JsonResult(new
            {
                success = false,
                error = "Validation error of model",
                errors = ModelState.Values.SelectMany(o => o.Errors).Select(o => o.ErrorMessage).ToArray()
            });
        }

        private JsonResult Success()
        {
            return new JsonResult(new
            {
                success = true
            });
        }
    }
}