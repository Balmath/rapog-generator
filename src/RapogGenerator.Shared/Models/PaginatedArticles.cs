using System;
using System.Collections.Generic;
using System.Text;

namespace RapogGenerator.Shared.Models
{
    public class PaginatedArticles
    {
        public PaginatedArticles(IEnumerable<Article> articles, int page, int pageCount)
        {
            Articles = articles;
            Page = page;
            PageCount = pageCount;
        }

        public IEnumerable<Article> Articles { get; }

        public int Page { get; }

        public int PageCount { get; }
    }
}
