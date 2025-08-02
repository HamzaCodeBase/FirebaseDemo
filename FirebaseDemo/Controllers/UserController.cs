using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirebaseDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(FirebaseService firebaseService) : ControllerBase
    {
        [HttpGet]
        public async Task<Dictionary<string, object>> Get(string userId)
        {
            return await firebaseService.GetUserAsync(userId);
        }

        [HttpGet("all")]
        public async Task<List<Dictionary<string, object>>> GetAll()
        {
            return await firebaseService.GetAllUsersAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name, int age)
        {
            await firebaseService.AddUserAsync(name, age);
            return Ok("User added successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(string userId, string name, int age)
        {
            await firebaseService.UpdateUserAsync(userId, name, age);
            return Ok("User updated successfully.");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string userId)
        {
            await firebaseService.DeleteUserAsync(userId);
            return Ok("User deleted successfully.");
        }
    }
}
