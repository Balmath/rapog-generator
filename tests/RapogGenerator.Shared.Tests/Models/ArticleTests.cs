using RapogGenerator.Shared.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace RapogGenerator.Shared.Tests.Models
{
    public class ArticleTests
    {
        [Fact]
        public void ArticleShouldBeCorrectlyInitialized()
        {
            var path = "/path";
            var title = "Title";
            var author = "Author";
            var category = "Category";
            var date = DateTime.Now;
            var tags = new List<string>() { "Tag 1", "Tag 2" };
            var content = "Content";
            var comments = new List<Comment> { new Comment("Author", DateTime.Now, "Content") };

            var article = new Article(path, title, author, category, date, tags, content, comments);

            Assert.Equal(path, article.Path);
            Assert.Equal(title, article.Title);
            Assert.Equal(author, article.Author);
            Assert.Equal(category, article.Category);
            Assert.Equal(date, article.Date);
            Assert.Equal(tags, article.Tags);
            Assert.Equal(content, article.Content);
            Assert.Equal(comments, article.Comments);
        }
    }
}
