using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RapogGenerator.DocumentDB
{
    class ArticleDocument
    {
        [JsonProperty(PropertyName = "title", Required = Required.Always)]
        public string Title { get; }

        [JsonProperty(PropertyName = "author", Required = Required.Always)]
        public string Author { get; }

        [JsonProperty(PropertyName = "category", Required = Required.Always)]
        public string Category { get; }

        [JsonProperty(PropertyName = "date", Required = Required.Always)]
        public DateTime Date { get; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; }

        [JsonProperty(PropertyName = "content", Required = Required.Always)]
        public string Content { get; }

        [JsonProperty(PropertyName = "comments")]
        public IEnumerable<CommentDocument> Comments { get; set; }
    }
}