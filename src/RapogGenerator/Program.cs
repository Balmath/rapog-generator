using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommandLine;
using RapogGenerator.Repositories;

[assembly:InternalsVisibleTo("RapogGenerator.Tests")]

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

        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<GenerateOptions, ListOptions>(args)
                .MapResult(
                    (GenerateOptions o) => 1,
                    (ListOptions o) => ListArticles(o),
                    errs => 1);
        }

        static int ListArticles(ListOptions options)
        {
            var repository = new DocumentDbRepository(options.InputDirectory);
            var task = repository.GetAllArticleDocumentPaths();
            task.Wait();
            foreach (var articleDocumentPath in task.Result)
            {
                Console.WriteLine(articleDocumentPath);
            }
            return 0;
        }
    }
}
