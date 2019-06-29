using RapogGenerator.Shared.Repositories;
using RapogGenerator.Shared.Templates;
using System.IO;
using System.Threading.Tasks;

namespace RapogGenerator.Shared
{
    public class Generator
    {
        private const string HtmlExtension = ".html";

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
            var articlePaths = await _articleRepository.GetAllArticlePathsAsync();
            foreach (var articlePath in articlePaths)
            {
                var outputArticlePath = Path.Combine(outputDirectory, Path.ChangeExtension(articlePath, HtmlExtension));

                // Genearate the article directories if they don't exist
                var directoryPath = Path.GetDirectoryName(outputArticlePath);
                Directory.CreateDirectory(directoryPath);

                var article = await _articleRepository.GetArticleAsync(articlePath);

                // Render the article in the html file using the article page template
                using (var streamWriter = new StreamWriter(outputArticlePath))
                {
                    _templatesEngine.RenderArticle(article, streamWriter);
                }
            }
        }
    }
}
