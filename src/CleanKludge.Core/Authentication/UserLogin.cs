using CleanKludge.Core.Authentication.Passwords;
using Microsoft.AspNetCore.Identity;

namespace CleanKludge.Core.Authentication
{
    public class UserLogin
    {
        private readonly IPasswordHasher<UserLogin> _passwordHasher;
        private readonly Password _password;
        private readonly string _username;

        public static UserLogin From(string username, string password, IPasswordHasher<UserLogin> passwordHasher)
        {
            return new UserLogin(username, Password.From(password), passwordHasher);
        }

        private UserLogin(string username, Password password, IPasswordHasher<UserLogin> passwordHasher)
        {
            _username = username;
            _passwordHasher = passwordHasher;
            _password = password;
        }

        public AuthenticationResult AuthenticateAgainst(string username, PasswordHash passwordHash)
        {
            return username.Equals(_username) && _passwordHasher.VerifyHashedPassword(this, passwordHash, _password) != PasswordVerificationResult.Failed
                ? AuthenticationResult.Success(username)
                : AuthenticationResult.Failed(username);
        }
    }
}