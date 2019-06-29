using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using RapogGenerator.Shared;
using RapogGenerator.Shared.Repositories;
using RapogGenerator.Shared.Templates;

namespace RapogGenerator
{
    class Program
    {
        [Verb("generate", HelpText = "Generate the static website.")]
        class GenerateOptions
        {
            [Option('i', "inputDirectory", Required = true, HelpText = "Set the folder with the articles.")]
            public string InputDirectory { get; set; }

            [Option('t', "templatesDirectory", Required = true, HelpText = "Set the templates folder.")]
            public string TemplatesDirectory { get; set; }

            [Option('o', "outputDirectory", Default=".\\build\\", HelpText = "Set the output folder.")]
            public string OutputDirectory { get; set; }
        }

        [Verb("list", HelpText = "List the article documents.")]
        class ListOptions
        {
            [Option('i', "inputDirectory", Required = true, HelpText = "Set the folder with the articles.")]
            public string InputDirectory { get; set; }
        }

        [Verb("get", HelpText = "Display a specific article document details.")]
        class GetOptions
        {
            [Option('i', "inputDirectory", Required = true, HelpText = "Set the folder with the articles.")]
            public string InputDirectory { get; set; }

            [Option('p', "path", Required = true, HelpText = "Set the relative path of the article.")]
            public string Path { get; set; }
        }

        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<GenerateOptions, ListOptions, GetOptions>(args)
                .MapResult(
                    (GenerateOptions o) => GenerateWebsite(o),
                    (ListOptions o) => ListArticles(o),
                    (GetOptions o) => GetArticle(o),
                    errs => 1);
        }

        static int GenerateWebsite(GenerateOptions options)
        {
            var repository = new ArticleRepository(options.InputDirectory);

            var templatesEngine = new TemplatesEngine(options.TemplatesDirectory);
            templatesEngine.ArticleTemplateName = "ArticlePage";

            var generator = new Generator(repository, templatesEngine);

            generator.GenerateAsync(options.OutputDirectory).GetAwaiter().GetResult();

            return 0;
        }

        static int ListArticles(ListOptions options)
        {
            var repository = new ArticleRepository(options.InputDirectory);
            var articlePaths = repository.GetAllArticlePathsAsync().GetAwaiter().GetResult();;
            foreach (var articleDocumentPath in articlePaths)
            {
                Console.WriteLine(articleDocumentPath);
            }
            return 0;
        }

        static int GetArticle(GetOptions options)
        {
            try
            {
                var repository = new ArticleRepository(options.InputDirectory);
                var article = repository.GetArticleAsync(options.Path).GetAwaiter().GetResult();
                Console.WriteLine("Path: {0}", article.Path);
                Console.WriteLine("Title: {0}", article.Title);
                Console.WriteLine("Author: {0}", article.Author);
                Console.WriteLine("Category: {0}", article.Category);
                Console.WriteLine("Tags: {0}", article.Tags);
                Console.WriteLine("Date: {0}", article.Date);
                Console.WriteLine("Content: {0}", article.Content);
            }
            catch (AggregateException ae)
            {
                ae.Handle((e) =>
                {
                    if (e is IOException)
                    {
                        Console.WriteLine(e.Message);
                        return true;
                    }
                    return false;
                });
            }
            return 0;
        }
    }
}
