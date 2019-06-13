using System;
using System.IO;
using CommandLine;
using RapogGenerator.Shared.Repositories;

namespace RapogGenerator
{
    class Program
    {
        [Verb("generate", HelpText = "Generate the static website.")]
        class GenerateOptions
        {
            [Option('i', "inputDirectory", Required = true, HelpText = "Set the folder with the articles.")]
            public string InputDirectory { get; set; }

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
                    (GenerateOptions o) => 1,
                    (ListOptions o) => ListArticles(o),
                    (GetOptions o) => GetArticle(o),
                    errs => 1);
        }

        static int ListArticles(ListOptions options)
        {
            var repository = new ArticleRepository(options.InputDirectory);
            var task = repository.GetAllArticlePaths();
            task.Wait();
            foreach (var articleDocumentPath in task.Result)
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
                var task = repository.GetArticle(options.Path);
                task.Wait();
                var article = task.Result;
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
