namespace CleanKludge.Core.Authentication.Passwords
{
    public class Password
    {
        private readonly string _value;

        public static Password From(string password)
        {
            return new Password(password);
        }

        private Password(string value)
        {
            _value = value;
        }

        public static implicit operator string(Password passwordHash)
        {
            return passwordHash._value;
        }
    }
}