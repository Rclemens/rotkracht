using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Globalization;

namespace BlazorApp.Shared
{
    public class Training : TableEntity
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
                //var itemDate = Date;
                //DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(itemDate);
                //if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                //{
                //    itemDate = itemDate.AddDays(3);
                //}

                //// Return the week of our adjusted day
                //var weekNumber = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(itemDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString();
                return $"{Date.Year}{Date.Month}";        
            }
        }
    }
}
