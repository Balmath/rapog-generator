using RapogGenerator.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapogGenerator.Shared.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<string>> GetAllArticlePaths();

        Task<Article> GetArticle(string articleDocumentPath);
    }
}
