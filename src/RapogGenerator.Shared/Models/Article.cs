using System;
using System.Collections.Generic;

namespace RapogGenerator.Shared.Models
{
    public class Article
    {
        public Article(string path, string title, string author, string category, DateTime date, IEnumerable<string> tags, string content, IEnumerable<Comment> comments)
        {
            Path = path;
            Title = title;
            Author = author;
            Category = Category;
            Date = date;
            Tags = tags;
            Content = content;
            Comments = comments;
        }

        public string Path { get; }

        public string Title { get; }

        public string Author { get; }

        public string Category { get; }

        public DateTime Date { get; }

        public IEnumerable<string> Tags { get; }

        public string Content { get; }

        public IEnumerable<Comment> Comments { get; }
    }
}