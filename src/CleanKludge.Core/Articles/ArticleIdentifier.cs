namespace CleanKludge.Core.Articles
{
    public class ArticleIdentifier
    {
        private readonly string _identifier;

        public static ArticleIdentifier From(string identifier)
        {
            return new ArticleIdentifier(identifier);
        }

        private ArticleIdentifier(string identifier)
        {
            _identifier = identifier;
        }

        protected bool Equals(ArticleIdentifier other)
        {
            return string.Equals(_identifier, other._identifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((ArticleIdentifier)obj);
        }

        public override int GetHashCode()
        {
            return _identifier?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return _identifier;
        }
    }
}