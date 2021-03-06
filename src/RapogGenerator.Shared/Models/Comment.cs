using System;

namespace RapogGenerator.Shared.Models
{
    public class Comment
    {
        public Comment(string author, DateTime date, string content)
        {
            Author = author;
            Date = date;
            Content = content;
        }

        public string Author { get; }

        public DateTime Date { get; }
        
        public string Content { get; }
    }
}