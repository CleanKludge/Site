using System;
using LibGit2Sharp;

namespace CleanKludge.Data.Git.Errors
{
    public static class ExceptionBecause
    {
        public static Exception Unauthorized()
        {
            throw new UnauthorizedAccessException("Invalid git credentials.");
        }

        public static Exception ContentHasConflicts()
        {
            throw new CheckoutConflictException("Content has conflicts.");
        }
    }
}