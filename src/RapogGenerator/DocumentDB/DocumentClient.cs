using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RapogGenerator.DocumentDB
{
    class DocumentClient
    {
        private readonly string documentsDirectory;

        public DocumentClient(string documentsDirectory)
        {
            this.documentsDirectory = documentsDirectory;
        }

        public async Task<ArticleDocument> GetArticle(string filePath)
        {
            var articleFullFilePath = Path.Combine(documentsDirectory, filePath);
            using (var fileStream = File.OpenText(articleFullFilePath))
            {
                var jsonArticle = await fileStream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<ArticleDocument>(jsonArticle);
            }
        }
    }
}