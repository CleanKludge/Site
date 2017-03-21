using System.Security.Principal;

namespace CleanKludge.Core.Authentication.Users
{
    public class UnauthenticatedUser : IUser
    {
        private readonly string _userName;

        public static UnauthenticatedUser From(string userName)
        {
            return new UnauthenticatedUser(userName);
        }

        private UnauthenticatedUser(string userName)
        {
            _userName = userName;
        }

        public GenericPrincipal ToPrincipal(string authenticationType)
        {
            return new GenericPrincipal(new UnknownIdentity(authenticationType, _userName), new string[0]);
        }

        private class UnknownIdentity : IIdentity
        {
            public string AuthenticationType { get; }
            public bool IsAuthenticated => false;
            public string Name { get; }

            public UnknownIdentity(string authenticationType, string name)
            {
                AuthenticationType = authenticationType;
                Name = name;
            }
        }
    }
}