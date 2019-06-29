using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RapogGenerator.Shared.DocumentDB.Tests
{
    public class DocumentClientTests
    {
        [Fact]
        public async Task GetArticleShouldReturnAnArticleWithAllTheContentOfTheJsonFile()
        {
            var documentClient = new DocumentClient(@"./DocumentDB");
            var articleDocument = await documentClient.GetArticleAsync("article.json");

            Assert.Equal("Central Park", articleDocument.Title);
            Assert.Equal("Marjolaine", articleDocument.Author);
            Assert.Equal("NY", articleDocument.Category);
            Assert.Equal(new DateTime(2010, 9, 26, 23, 20, 26), articleDocument.Date);
            Assert.Equal("New York", articleDocument.Tags);
            Assert.Equal("content", articleDocument.Content);
            var comments = articleDocument.Comments.ToList();
            Assert.Equal(2, comments.Count);
            Assert.Equal("comment 1", comments[0].Content);
            Assert.Equal("Paul", comments[0].Author);
            Assert.Equal(new DateTime(2010, 9, 27, 15, 39, 20), comments[0].Date);
            Assert.Equal("comment 2", comments[1].Content);
            Assert.Equal("Mathieu", comments[1].Author);
            Assert.Equal(new DateTime(2010, 9, 27, 23, 31, 54), comments[1].Date);
        }
    }
}
