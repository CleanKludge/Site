using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CleanKludge.Core.Authentication.Users
{
    public class AuthenticatedUser : IUser
    {
        private readonly string _userName;

        public static AuthenticatedUser From(string userName)
        {
            return new AuthenticatedUser(userName);
        }

        private AuthenticatedUser(string userName)
        {
            _userName = userName;
        }

        public GenericPrincipal ToPrincipal(string authenticationType)
        {
            return new GenericPrincipal(new GenericIdentity(_userName, CookieAuthenticationDefaults.AuthenticationScheme), new string[0]);
        }
    }
}