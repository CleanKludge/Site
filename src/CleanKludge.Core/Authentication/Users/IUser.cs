using System.Security.Principal;

namespace CleanKludge.Core.Authentication.Users
{
    public interface IUser
    {
        GenericPrincipal ToPrincipal(string authenticationType);
    }
}