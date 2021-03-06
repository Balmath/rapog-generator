﻿using RapogGenerator.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapogGenerator.Shared.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<string>> GetAllArticlePathsAsync();

        Task<IEnumerable<string>> GetAllArticlePathsOrderedByDateAsync();

        Task<Article> GetArticleAsync(string articleDocumentPath);
    }
}
