using Hard.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionController : MainController
    {
        public VersionController(INotifier notifier) : base(notifier)
        {
        }

        [HttpGet]
        public string Get()
        {
            return "Version=1.0";
        }
    }
}
