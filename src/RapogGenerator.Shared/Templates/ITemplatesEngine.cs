using RapogGenerator.Shared.Models;
using System.IO;

namespace RapogGenerator.Shared.Templates
{
    public interface ITemplatesEngine
    {
        void RenderArticle(Article article, TextWriter textWriter);

        void RenderPaginatedHome(PaginatedArticles paginatedArticles, TextWriter textWriter);
    }
}
