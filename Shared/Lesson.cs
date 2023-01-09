using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class Lesson : TableEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        [JsonProperty("datekey")]
        public string DateKey
        {
            get
            {
                return $"{Date.Year}{Date.Month}";
            }
        }
    }
}
