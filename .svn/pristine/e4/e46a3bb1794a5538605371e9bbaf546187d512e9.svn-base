using CleanKludge.Core.Authentication;
using CleanKludge.Core.Authentication.Passwords;
using Microsoft.Extensions.Configuration;

namespace CleanKludge.Services.Authentication
{
    public class UserService
    {
        private readonly IConfigurationRoot _configuration;

        public UserService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationResult Validate(UserLogin userLogin)
        {
            var username = _configuration.GetValue<string>(Config.Username);
            var passwordHash = _configuration.GetValue<string>(Config.PasswordHash);

            return userLogin.AuthenticateAgainst(username, PasswordHash.From(passwordHash));
        }
    }
}