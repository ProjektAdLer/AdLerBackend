using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : ControllerBase
    {

        private static readonly HttpClient Client = new();

        [HttpPost(Name = "UserLogin")]
        public async Task<string> UserLogin(UserLoginDto data)
        {
            var test = await Client.GetAsync(
                $"https://moodle.cluuub.xyz/login/token.php?username={data.userName}&password={data.password}&service=moodle_mobile_app");

            var ret = await test.Content.ReadFromJsonAsync<UserTokenResponse>();

            return ret?.token == null ? throw new ArgumentException("Invalid username or password.") : ret.token;
        }
    }

    public class UserLoginDto
    {
        public string? userName { get; set; }
        public string? password { get; set; }
    }
}