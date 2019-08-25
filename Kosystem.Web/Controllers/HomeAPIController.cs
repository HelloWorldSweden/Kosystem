using Kosystem.Core;
using Kosystem.Core.DTO;
using Kosystem.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Kosystem.Web.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class HomeAPIController : ControllerBase
    {
        private readonly IRoomService roomService;
        private readonly IHostingEnvironment environment;

        public HomeAPIController(IRoomService roomService, IHostingEnvironment environment)
        {
            this.roomService = roomService;
            this.environment = environment;
        }

        [HttpPost("user")]
        public async Task<IActionResult> JoinRoom([FromForm] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelStateErrorData());
                }

                Room room = await roomService.GetRoomByIdAsync(model.RoomId);
                if (room == null)
                {
                    ModelState.AddModelError(nameof(model.RoomId), $"Room does not exist.");
                    return BadRequest(ModelStateErrorData());
                }

                var user = new UserCreationDTO
                {
                    Name = model.UserName,
                    RoomId = model.RoomId
                };

                var userId = await roomService.RegisterUserAsync(user);
                return Ok(SuccessData(data: new
                {
                    userId
                }));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ExceptionData(e));
            }
        }

        private object ModelStateErrorData()
        {
            return new
            {
                success = false,
                error = "Validation error of model",
                errors = ModelState.Values.SelectMany(o => o.Errors).Select(o => o.ErrorMessage).ToArray()
            };
        }

        private object ExceptionData(Exception ex)
        {
            if (environment.IsDevelopment())
            {
                return new
                {
                    success = false,
                    error = ex.Message,
                    stacktrace = ex.StackTrace,
                    innerError = ex.InnerException?.Message,
                };
            } else
            {
                return new
                {
                    success = false,
                    error = "Internal server error.",
                };
            }
        }

        private object SuccessData()
        {
            return new
            {
                success = true
            };
        }

        private object SuccessData(object data)
        {
            return new
            {
                success = true,
                data,
            };
        }
    }
}