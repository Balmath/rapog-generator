using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RapogGenerator.Repositories
{
    class DocumentDbRepository
    {
        private readonly string rootDirectoryPath;

        public DocumentDbRepository(string rootDirectoryPath)
        {
            this.rootDirectoryPath = rootDirectoryPath;
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

        public Task<IEnumerable<string>> GetAllArticleDocumentPaths()
        {
            if (!Directory.Exists(rootDirectoryPath))
            {
                return Task.FromResult(Enumerable.Empty<string>());
            }

            var articleDocumentPaths = new List<string>();

            AddArticleDocumentFilePaths(rootDirectoryPath, rootDirectoryPath, articleDocumentPaths);

            return Task.FromResult<IEnumerable<string>>(articleDocumentPaths);
        }
    }
}