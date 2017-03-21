using System;
using System.IO;
using CleanKludge.Core.Articles;

namespace CleanKludge.Data.File.Errors
{
    public static class ExceptionBecause
    {
        public static Exception InvalidArticlePath(string path)
        {
            throw new DirectoryNotFoundException(path);
        }

        public static Exception ArticleNotFound(ArticleIdentifier identifier)
        {
            throw new FileNotFoundException(identifier.ToString());
        }
    }
}