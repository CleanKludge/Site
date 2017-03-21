using CleanKludge.Core.Articles;

namespace CleanKludge.Services.Content
{
    public class ContentService
    {
        private readonly IArticleRepository _contentRepository;

        public ContentService(IArticleRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public Articles Latest(bool onlyEnabled)
        {
            return Articles.AllFrom(_contentRepository, onlyEnabled)
                .GetLatest(5);
        }

        public GroupedArticles Grouped(Grouping grouping, Location location, bool includeDisabled)
        {
            return Articles.AllFrom(_contentRepository, includeDisabled)
                .ForSection(location)
                .GroupBy(grouping);
        }

        public Article For(ArticleIdentifier reference, Location location, bool includeDisabled)
        {
            var articleDto = _contentRepository.FetchOne(reference, includeDisabled);
            return Article.From(articleDto);
        }

        public void Delete(ArticleIdentifier reference)
        {
            _contentRepository.Delete(reference);
        }

        public void Save(Article article)
        {
            article.Save(_contentRepository);
        }
    }
}