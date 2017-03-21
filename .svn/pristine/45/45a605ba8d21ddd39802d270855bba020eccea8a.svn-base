using System.Collections.Generic;
using System.Linq;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles.Extensions;

namespace CleanKludge.Core.Articles
{
    public class Article
    {
        private readonly IArticleDto _dto;

        public static Article From(ContentArticle model)
        {
            return From(new ArticleDto
            {
                Tags = model.Tags,
                Author = model.Author,
                Created = model.Created,
                Description = model.Summary,
                Identifier = ArticleIdentifier.From(model.Identifier),
                Location = model.Location.ToCoreType(),
                Title = model.Title,
                Sections = model.Sections.Select(x => new SectionDto
                    {
                        Type = x.Type.ToCoreType(),
                        Content = x.Content,
                        Properties = x.Properties?.ToDictionary(y => y.Key, y => y.Value) ?? new Dictionary<string, string>()
                    })
                    .Cast<ISectionDto>()
                    .ToList()
            });
        }

        public static Article From(IArticleDto dto)
        {
            return new Article(dto);
        }

        private Article(IArticleDto dto)
        {
            _dto = dto;
        }

        public string KeyFor(Grouping grouping)
        {
            return grouping == Grouping.Date ? _dto.Created.ToString("MMM yyyy") : _dto.Tags[0];
        }

        public bool In(Location location)
        {
            return _dto.Location == location;
        }

        public int CompareAgeTo(Article article)
        {
            return _dto.Created.CompareTo(article._dto.Created);
        }

        public IArticleDto ToDto()
        {
            return _dto;
        }

        public void Save(IArticleRepository repository)
        {
            repository.Save(_dto);
        }

        public ContentSummary ToSummaryResponse()
        {
            return new ContentSummary
            {
                Area =_dto.Location.ToString(),
                Identifier = _dto.Identifier.ToString(),
                Title = _dto.Title,
                Created = _dto.Created,
                Description = _dto.Description,
                Enabled = _dto.Enabled,
                Tags = _dto.Tags,
                Author = _dto.Author
            };
        }

        public ContentArticle ToArticleResponse()
        {
            return new ContentArticle
            {
                Created = _dto.Created,
                Location = _dto.Location.ToApiType(),
                Identifier = _dto.Identifier?.ToString(),
                Author = _dto.Author,
                Summary = _dto.Description,
                Enabled = _dto.Enabled,
                Tags = _dto.Tags,
                Title = _dto.Title,
                Sections = _dto.Sections?.Select(x => new ArticleSection
                    {
                        Content = x.Content,
                        Type = x.Type.ToApiType(),
                        Properties = x.Properties?.ToDictionary(y => y.Key, y => y.Value) ?? new Dictionary<string, string>()
                    })
                    .ToList()
            };
        }
    }
}