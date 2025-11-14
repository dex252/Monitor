using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monitor.Models.DTOs.Requests;
using Monitor.Models.Entities.Postgres;
using Monitor.Repositories.Inretfaces;

namespace Monitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddUser(CreateUserRequest userDto)
        {
            var user = new User()
            {
              Name =userDto.Name,
              Email = userDto.Email
            };

            var id = await _userRepository.AddUserAsync(user);
            return Ok(id);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

    }
}