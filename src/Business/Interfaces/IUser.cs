using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Business.Interfaces
{
    public interface IUser
    {
        public Guid Id { get; }
        public string Name{ get; }
        public string Email { get; }
        public IEnumerable<Claim> Claims { get; }
        public bool IsAuthenticaed();
        public bool PlayTheRole(string role);
    }
}
