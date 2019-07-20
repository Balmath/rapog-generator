using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;
using RapogGenerator.Shared.Models;
using System;
using System.IO;
using System.Reflection;

namespace RapogGenerator.Shared.Templates
{
    public class TemplatesEngine : ITemplatesEngine
    {
        private const char StartDelimiter = '{';
        private const char EndDelimiter = '}';
        private const string ArticleParameterName = "article";
        private const string PaginatedArticlesParameterName = "paginatedArticles";

        private TemplateGroup templateGroup;

        public TemplatesEngine(string rootTemplatePath)
        {
            templateGroup = new TemplateRawGroupDirectory(rootTemplatePath, StartDelimiter, EndDelimiter);
        }

        public string ArticleTemplateName { get; set; }

        public string PaginatedHomeTemplateName { get; set; }

        public void RenderArticle(Article article, TextWriter textWriter)
        {
            var template = templateGroup.GetInstanceOf(ArticleTemplateName);
            template.Add(ArticleParameterName, article);
            template.Write(textWriter, new TemplateErrorListener());
        }

        public void RenderPaginatedHome(PaginatedArticles paginatedArticles, TextWriter textWriter)
        {
            var template = templateGroup.GetInstanceOf(PaginatedHomeTemplateName);
            template.Add(PaginatedArticlesParameterName, paginatedArticles);
            template.Write(textWriter, new TemplateErrorListener());
        }

        private class TemplateErrorListener : ITemplateErrorListener
        {
            public void CompiletimeError(TemplateMessage msg)
            {
                throw new ArgumentException(msg.ToString());
            }

            public void InternalError(TemplateMessage msg)
            {
                throw new ArgumentException(msg.ToString());
            }

            public void IOError(TemplateMessage msg)
            {
                throw new ArgumentException(msg.ToString());
            }

            public void RuntimeError(TemplateMessage msg)
            {
                throw new ArgumentException(msg.ToString());
            }
        }
    }
}
