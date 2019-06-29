using RapogGenerator.Shared.Models;
using RapogGenerator.Shared.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace RapogGenerator.Shared.Tests.Templates
{
    public class TemplatesEngineTests
    {
        [Fact]
        public void TemplateEngineShouldRenderCorrectlyAnArticle()
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

            var templatesEngine = new TemplatesEngine(@"./Templates")
            {
                ArticleTemplateName = "article"
            };

            var expected = @"Title: Title
Author: Author";

            using (var stringWriter = new StringWriter())
            {
                templatesEngine.RenderArticle(article, stringWriter);

                Assert.Equal(expected, stringWriter.ToString());
            }

        }
    }
}
