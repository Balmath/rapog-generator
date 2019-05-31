using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace RapogGenerator.DocumentDB.Tests
{
    [TestClass]
    public class DocumentClientTests
    {
        [TestMethod]
        [DeploymentItem("article.json")]
        public async Task GetArticleShouldReturnAnArticleWithAllTheContentOfTheJsonFile()
        {
            var documentClient = new DocumentClient(@".\");
            var articleDocument = await documentClient.GetArticle("article.json");
        }
    }
}
