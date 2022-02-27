using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Extensions
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _acessor;

        public AspNetUser(IHttpContextAccessor acessor)
        {
            _acessor = acessor;
        }

        public Guid Id => Guid.Parse(_acessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public string Name => _acessor.HttpContext.User.Identity.Name;

        public string Email => _acessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

        public IEnumerable<Claim> Claims => _acessor.HttpContext.User.Claims;

        public bool IsAuthenticaed() => _acessor.HttpContext.User.Identity.IsAuthenticated;

        public bool PlayTheRole(string role) => _acessor.HttpContext.User.IsInRole(role);
    }
}
