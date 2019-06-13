using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RapogGenerator.Shared.DocumentDB;
using RapogGenerator.Shared.Models;

namespace RapogGenerator.Shared.Repositories
{
    public class ArticleRepository
    {
        private readonly string rootDirectoryPath;
        private readonly DocumentClient documentClient;

        public ArticleRepository(string rootDirectoryPath)
        {
            this.rootDirectoryPath = rootDirectoryPath;
            documentClient = new DocumentClient(rootDirectoryPath);
        }

        private void AddArticleDocumentFilePaths(string rootDirectoryPath, string currentDirectoryPath, IList<string> articleDocumentPaths)
        {
            foreach (var filePath in Directory.EnumerateFiles(currentDirectoryPath))
            {
                if (Path.GetExtension(filePath).ToLower() == ".json")
                {
                    var articleDocumentPath = filePath.Replace(rootDirectoryPath, string.Empty);
                    articleDocumentPaths.Add(articleDocumentPath);
                }
            }

            foreach (var directoryPath in Directory.EnumerateDirectories(currentDirectoryPath))
            {
                AddArticleDocumentFilePaths(rootDirectoryPath, directoryPath, articleDocumentPaths);
            }
        }

        public Task<IEnumerable<string>> GetAllArticlePaths()
        {
            if (!Directory.Exists(rootDirectoryPath))
            {
                return Task.FromResult(Enumerable.Empty<string>());
            }

            var articleDocumentPaths = new List<string>();

            AddArticleDocumentFilePaths(rootDirectoryPath, rootDirectoryPath, articleDocumentPaths);

            return Task.FromResult<IEnumerable<string>>(articleDocumentPaths);
        }

        public async Task<Article> GetArticle(string articleDocumentPath)
        {
            var articleDocument = await documentClient.GetArticle(articleDocumentPath);
            return new Article(
                articleDocumentPath,
                articleDocument.Title,
                articleDocument.Author,
                articleDocument.Category,
                articleDocument.Date,
                articleDocument.Tags.Split(',').Select(s => s.Trim()).ToList(),
                articleDocument.Content,
                articleDocument.Comments.Select(cd => new Comment(cd.Author, cd.Date, cd.Content)));
        }
    }
}