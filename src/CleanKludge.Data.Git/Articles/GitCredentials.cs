using LibGit2Sharp;

namespace CleanKludge.Data.Git.Articles
{
    public class GitCredentials
    {
        private readonly string _email;
        private readonly string _name;

        public static GitCredentials From(GitOptions options)
        {
            return new GitCredentials(options.GitEmail, options.GitName);
        }

        public static GitCredentials From(string email, string name)
        {
            return new GitCredentials(email, name);
        }

        private GitCredentials(string email, string name)
        {
            _email = email;
            _name = name;
        }

        public static implicit operator Identity(GitCredentials credentials)
        {
            return new Identity(credentials._name, credentials._email);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((GitCredentials)obj);
        }

        protected bool Equals(GitCredentials other)
        {
            return string.Equals(_email.ToLower(), other._email.ToLower()) && string.Equals(_name.ToLower(), other._name.ToLower());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_email != null ? _email.GetHashCode() : 0) * 397) ^ (_name != null ? _name.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"{{ \"Name\": \"{_name}\", \"Email\":\"{_email}\" }}";
        }
    }
}