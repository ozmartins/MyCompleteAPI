using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace API.Extensions
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequirementsClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }        
    }
    public class RequirementsClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim; 

        public RequirementsClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if (!context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value.Contains(_claim.Value)))
            {
                context.Result = new StatusCodeResult(403);
                return;
            }
        }
    }
}
