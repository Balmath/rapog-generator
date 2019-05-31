using System;
using CommandLine;

namespace RapogGenerator
{
    class Program
    {
        public class Options
        {
            [Option('i', "inputFolder", Required = true, HelpText = "Set the folder with the articles.")]
            public string InputFolder { get; set; }

            [Option('o', "outputFolder", Default=".\\build\\", HelpText = "Set the output folder.")]
            public string OutputFolder { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
            });
        }
    }
}
