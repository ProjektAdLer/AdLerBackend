using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : ControllerBase
    {

        private static readonly HttpClient client = new HttpClient();

        [HttpGet(Name = "UserLogin")]
        public async Task<string> GetAsync()
        {
            var test = await client.GetAsync("https://moodle.cluuub.xyz/login/token.php?username=student&password=wve2rxz7wfm3BPH-ykh&service=moodle_mobile_app");

            UserTokenResponse ret = test.Content.ReadFromJsonAsync<UserTokenResponse>().Result;

            return ret.token;
        }
    }
}