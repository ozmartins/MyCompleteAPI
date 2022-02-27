using Hard.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionController : MainController
    {
        public VersionController(INotifier notifier) : base(notifier)
        {
        }

        [HttpGet]
        public string Get()
        {
            return "Version=2.0";
        }
    }
}
