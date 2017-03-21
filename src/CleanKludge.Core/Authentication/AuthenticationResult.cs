using CleanKludge.Core.Authentication.Users;

namespace CleanKludge.Core.Authentication
{
    public class AuthenticationResult
    {
        public bool IsLockedOut => false;
        public bool Succeeded { get; }
        public IUser User { get; }

        public static AuthenticationResult Failed(string username)
        {
            return new AuthenticationResult(UnauthenticatedUser.From(username), false);
        }

        public static AuthenticationResult Success(string username)
        {
            return new AuthenticationResult(AuthenticatedUser.From(username), true);
        }

        private AuthenticationResult(IUser user, bool succeeded)
        {
            User = user;
            Succeeded = succeeded;
        }
    }
}