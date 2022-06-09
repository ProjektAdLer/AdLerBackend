using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : ControllerBase
    {

        private static readonly HttpClient client = new HttpClient();

        [HttpPost(Name = "UserLogin")]
        public async Task<string> UserLogin(UserLoginDTO data)
        {
            var test = await client.GetAsync($"https://moodle.cluuub.xyz/login/token.php?username={data.userName}&password={data.password}&service=moodle_mobile_app");

            UserTokenResponse ret = test.Content.ReadFromJsonAsync<UserTokenResponse>().Result;


            if (ret.token == null) return "Falsche Daten!";

            return ret.token;
        }
    }

    public class UserLoginDTO
    {
        public string? userName { get; set; }
        public string? password { get; set; }
    }
}