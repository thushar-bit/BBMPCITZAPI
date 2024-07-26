using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/Test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("TESTBBMP")]
        public string GetHelloWorld(bool isconnected)
        {
            try
            {
                if (isconnected)
                {
                    return "React Page is CONNECTED TO API";
                }
                else
                {
                    return "React page is Not connected To API";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
