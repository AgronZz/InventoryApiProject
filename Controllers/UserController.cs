using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryApiProject.Auth;

namespace InventoryApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        // =====================================
        // 1. GET ALL USERS
        // =====================================
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAllUsers()
        {
            //Returns all users except passwords
            var users = UserConstants.Users.Select(u => new
            {
                u.Username,
                u.Email,
                u.FullName,
                u.Role
            });

            _logger.LogInformation("Fetched all users");

            return Ok(users);
        }

        // =====================================
        // 2. GET CURRENT USER
        // =====================================
        [HttpGet("me")]
        public IActionResult Me()
        {
            var username = User.Identity?.Name;  //Gets the username from the JWT token claims

            var user = UserConstants.Users.FirstOrDefault(u =>  //Finds the user in the list based on the username
                u.Username == username);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Username,
                user.Email,
                user.FullName,
                user.Role
            });
        }

        // =====================================
        // 3. GET USER BY USERNAME
        // =====================================
        [HttpGet("{username}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetByUsername(string username)
        {
            var user = UserConstants.Users.FirstOrDefault(u =>  //Finds the user in the list based on the username
                u.Username == username);

            if (user == null)  //if user is not found, log a warning and return 404 Not Found
                return NotFound();

            return Ok(new
            {
                user.Username,
                user.Email,
                user.FullName,
                user.Role
            });
        }

        // =====================================
        // 4. CREATE USER
        // =====================================
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(UserModel model)
        {
            var exists = UserConstants.Users.Any(u =>  //Checks if a user with the same username already exists in the list
                u.Username == model.Username);

            if (exists)  //if user already exists, log a warning and return 400 Bad Request
                return BadRequest("Username already exists");

            UserConstants.Users.Add(model);  //Adds the new user to the list

            _logger.LogInformation("User created: {Username}", model.Username);

            return Ok(model);
        }

        // =====================================
        // 5. DELETE USER
        // =====================================
        [HttpDelete("{username}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string username)
        {
            var user = UserConstants.Users.FirstOrDefault(u =>  //Finds the user in the list based on the username
                u.Username == username);

            if (user == null)  //if user is not found, log a warning and return 404 Not Found
                return NotFound();

            UserConstants.Users.Remove(user);  //Removes the user from the list

            _logger.LogWarning("User deleted: {Username}", username);

            return NoContent();
        }
    }
}