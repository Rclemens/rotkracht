using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class TimeRegistration
    {

        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
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

    public class TimeRecord
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string LessonId { get; set; }
        public List<string> UserIdSet { get; set; } = new List<string>();
    }
}
