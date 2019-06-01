using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RapogGenerator.DocumentDB
{
    class ArticleDocument
    {
        [JsonProperty(PropertyName = "title", Required = Required.Always)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "author", Required = Required.Always)]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "category", Required = Required.Always)]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "date", Required = Required.Always)]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }

        [JsonProperty(PropertyName = "content", Required = Required.Always)]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public IEnumerable<CommentDocument> Comments { get; set; }
    }
}