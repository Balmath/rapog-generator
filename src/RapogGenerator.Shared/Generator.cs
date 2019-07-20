using RapogGenerator.Shared.Models;
using RapogGenerator.Shared.Repositories;
using RapogGenerator.Shared.Templates;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RapogGenerator.Shared
{
    public class Generator
    {
        private const string HtmlExtension = ".html";
        private const string IndexPageName = "index";
        private const int ArticleCountByPage = 5;

        private readonly IArticleRepository _articleRepository;
        private readonly ITemplatesEngine _templatesEngine;

        public Generator(IArticleRepository articleRepository, ITemplatesEngine templatesEngine)
        {
            _articleRepository = articleRepository;
            _templatesEngine = templatesEngine;
        }

        public async Task GenerateAsync(string outputDirectory)
        {
            // Create the output directory if it doesn't exist
            Directory.CreateDirectory(outputDirectory);

            // Generate all the articles html pages
            await GenerateArticlePagesAsync(outputDirectory);

            // Generate the index page and the paginated pages for all articles
            await GenerateHomePaginatedPagesAsync(outputDirectory);
        }

        private async Task GenerateArticlePagesAsync(string outputDirectory)
        {
            var articlePaths = await _articleRepository.GetAllArticlePathsAsync();
            foreach (var articlePath in articlePaths)
            {
                var outputArticlePath = Path.Combine(outputDirectory, Path.ChangeExtension(articlePath, HtmlExtension));

                // Genearate the article directories if they don't exist
                var directoryPath = Path.GetDirectoryName(outputArticlePath);
                Directory.CreateDirectory(directoryPath);

                var article = await _articleRepository.GetArticleAsync(articlePath);

                // Render the article in the HTML file using the article page template
                using (var streamWriter = new StreamWriter(outputArticlePath))
                {
                    _templatesEngine.RenderArticle(article, streamWriter);
                }
            }
        }

        private async Task GenerateHomePaginatedPagesAsync(string outputDirectory)
        {
            var articleIndex = 0;
            var articlePaths = await _articleRepository.GetAllArticlePathsOrderedByDateAsync();
            var articleCount = articlePaths.Count();
            var pageCount = articleCount / ArticleCountByPage + (articleCount % ArticleCountByPage > 0 ? 1 : 0);
            while (articleIndex < articleCount)
            {
                // Generate the HTML file name: index.html, index1.html, ..., indexN.html
                var outputPagePath = outputDirectory + IndexPageName;
                var page = articleIndex / ArticleCountByPage;
                if (page > 0)
                {
                    outputPagePath += page;
                }
                outputPagePath += HtmlExtension;

                var pageArticles = await GetPageArticles(articlePaths, articleIndex);
                var paginatedArticles = new PaginatedArticles(pageArticles, page, pageCount);

                // Render the home page with the articles
                using (var streamWriter = new StreamWriter(outputPagePath))
                {
                    _templatesEngine.RenderPaginatedHome(paginatedArticles, streamWriter);
                }

                // Move the next articles
                articleIndex += ArticleCountByPage;
            }
        }

        private async Task<IEnumerable<Article>> GetPageArticles(IEnumerable<string> articlePaths, int articleIndex)
        {
            var pageArticles = new List<Article>();

            var articleCount = articlePaths.Count();
            var pageArticleCount = ArticleCountByPage;
            if (articleIndex + ArticleCountByPage > articleCount)
            {
                pageArticleCount = articleCount - articleIndex;
            }
            var pageArticlePaths = articlePaths.ToList().GetRange(articleIndex, pageArticleCount);
            foreach (var pageArticlePath in pageArticlePaths)
            {
                pageArticles.Add(await _articleRepository.GetArticleAsync(pageArticlePath));
            }

            return pageArticles;
        }
    }
}
