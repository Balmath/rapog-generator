using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
#if WINDOWS_UWP
using System;
using Windows.Storage;
#else
#endif

namespace RapogGenerator.Shared.DocumentDB
{
    public class DocumentClient
    {
#if WINDOWS_UWP
        private readonly StorageFolder documentsStorageFolder;
#else
        private readonly string documentsDirectory;
#endif

#if WINDOWS_UWP
        public DocumentClient(StorageFolder documentsStorageFolder)
        {
            this.documentsStorageFolder = documentsStorageFolder;
        }
#else
        public DocumentClient(string documentsDirectory)
        {
            this.documentsDirectory = documentsDirectory;
        }
#endif

        public async Task<ArticleDocument> GetArticleAsync(string filePath)
        {
            var jsonArticle = string.Empty;
            var relativeFilePath = Path.IsPathRooted(filePath) ? filePath.Substring(1) : filePath;
#if WINDOWS_UWP
            var storageFile = await documentsStorageFolder.GetFileAsync(relativeFilePath);
            jsonArticle = await FileIO.ReadTextAsync(storageFile);
#else
            var articleFullFilePath = Path.Combine(documentsDirectory, relativeFilePath);
            using (var fileStream = File.OpenText(articleFullFilePath))
            {
                jsonArticle = await fileStream.ReadToEndAsync();
            }
#endif
            return JsonConvert.DeserializeObject<ArticleDocument>(jsonArticle);
        }
    }
}