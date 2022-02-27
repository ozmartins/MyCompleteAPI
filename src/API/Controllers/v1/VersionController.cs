using Hard.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionController : MainController
    {
        private readonly ILogger<VersionController> _logger;

        public VersionController(INotifier notifier, ILogger<VersionController> logger) : base(notifier)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            _logger.LogInformation("GerVersion successfully executed");            
            return "Version=1.0";
        }
    }
}
