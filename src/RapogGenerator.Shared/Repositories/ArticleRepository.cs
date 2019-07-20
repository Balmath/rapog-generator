using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RapogGenerator.Shared.Comparers;
using RapogGenerator.Shared.DocumentDB;
using RapogGenerator.Shared.Models;
#if WINDOWS_UWP
using System;
using Windows.Storage;
#endif

namespace RapogGenerator.Shared.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
#if WINDOWS_UWP
        private readonly StorageFolder rootStorageFolder;
#else
        private readonly string rootDirectoryPath;
#endif
        private readonly DocumentClient documentClient;

#if WINDOWS_UWP
        public ArticleRepository(StorageFolder rootStorageFolder)
        {
            this.rootStorageFolder = rootStorageFolder;
            documentClient = new DocumentClient(rootStorageFolder);
        }
#else
        public ArticleRepository(string rootDirectoryPath)
        {
            this.rootDirectoryPath = rootDirectoryPath;
            documentClient = new DocumentClient(rootDirectoryPath);
        }
#endif

#if WINDOWS_UWP
        private async Task AddArticleDocumentFilePaths(StorageFolder currentStorageFolder, IList<string> articleDocumentPaths)
        {
            foreach (var storageFile in await currentStorageFolder.GetFilesAsync())
            {
                if (storageFile.FileType.ToLower() == ".json")
                {
                    var articleDocumentPath = storageFile.Path.Replace(rootStorageFolder.Path, string.Empty);
                    articleDocumentPaths.Add(articleDocumentPath);
                }
            }

            foreach (var storageFolder in await currentStorageFolder.GetFoldersAsync())
            {
                await AddArticleDocumentFilePaths(storageFolder, articleDocumentPaths);
            }
        }
#else
        private void AddArticleDocumentFilePaths(string currentDirectoryPath, IList<string> articleDocumentPaths)
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
                AddArticleDocumentFilePaths(directoryPath, articleDocumentPaths);
            }
        }
#endif

        public async Task<IEnumerable<string>> GetAllArticlePathsAsync()
        {
            var articleDocumentPaths = new List<string>();

#if WINDOWS_UWP
            if (rootStorageFolder != null)
#else
            if (Directory.Exists(rootDirectoryPath))
#endif
            {
#if WINDOWS_UWP
                await AddArticleDocumentFilePaths(rootStorageFolder, articleDocumentPaths);
#else
                await Task.Run(() => AddArticleDocumentFilePaths(rootDirectoryPath, articleDocumentPaths));
#endif
            }

            return articleDocumentPaths;
        }

        public async Task<IEnumerable<string>> GetAllArticlePathsOrderedByDateAsync()
        {
            var orderedArticles = await GetAllArticlesOrderedByDate();

            return orderedArticles.Values.Select(a => a.Path);
        }

        public async Task<Article> GetArticleAsync(string articleDocumentPath)
        {
            var articleDocument = await documentClient.GetArticleAsync(articleDocumentPath);
            return new Article(
                articleDocumentPath,
                articleDocument.Title,
                articleDocument.Author,
                articleDocument.Category,
                articleDocument.Date,
                !string.IsNullOrWhiteSpace(articleDocument.Tags) ? articleDocument.Tags.Split(',').Select(s => s.Trim()).ToList()
                                                                 : Enumerable.Empty<string>(),
                articleDocument.Content,
                articleDocument.Comments.Select(cd => new Comment(cd.Author, cd.Date, cd.Content)));
        }

        private async Task<SortedList<DateTime, Article>> GetAllArticlesOrderedByDate()
        {
            var articlePaths = await GetAllArticlePathsAsync();

            var orderedArticles = new SortedList<DateTime, Article>(new DescendingComparer<DateTime>());

            foreach (var articlePath in articlePaths)
            {
                var article = await GetArticleAsync(articlePath);
                orderedArticles.Add(article.Date, article);
            }

            return orderedArticles;
        }
    }
}