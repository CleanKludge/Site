using System.Collections.Generic;
using CleanKludge.Core.Articles;

namespace CleanKludge.Data.File.Articles
{
    public class GitRepository : IArticleRepository
    {
        private readonly FileRepository _fileRepository;

        public GitRepository(FileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public IArticleDto FetchOne(ArticleIdentifier identifier, bool includeDisabled)
        {
            return _fileRepository.FetchOne(identifier, includeDisabled);
        }

        public IList<IArticleDto> FetchAll(bool includeDisabled)
        {
            return _fileRepository.FetchAll(includeDisabled);
        }

        public void Save(IArticleDto dto)
        {
            _fileRepository.Save(dto);
        }

        public void Delete(ArticleIdentifier reference)
        {
            _fileRepository.Delete(reference);
        }
    }
}