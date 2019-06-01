using System;
using Newtonsoft.Json;

namespace RapogGenerator.DocumentDB
{
    class CommentDocument
    {
        [JsonProperty(PropertyName = "author", Required = Required.Always)]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "content", Required = Required.Always)]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "date", Required = Required.Always)]
        public DateTime Date { get; set; }
    }
}