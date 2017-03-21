namespace CleanKludge.Core.Authentication.Passwords
{
    public class PasswordHash
    {
        private readonly string _value;

        public static PasswordHash From(string hashedPassword)
        {
            return new PasswordHash(hashedPassword);
        }

        private PasswordHash(string value)
        {
            _value = value;
        }

        public static implicit operator string(PasswordHash passwordHash)
        {
            return passwordHash._value;
        }
    }
}