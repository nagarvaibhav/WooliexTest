using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WooliesX.Services.Interfaces;

namespace WooliesXTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public IUserService UserService { get; }

        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                return Ok(_userService.GetUserAndToken());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error In getting Token and User");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
