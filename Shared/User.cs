using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlazorApp.Shared
{
    public class User : TableEntity, ICloneable, IEquatable<User>
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("email")]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.MinValue;

        public object Clone()
        {
            return (User)this.MemberwiseClone();
        }

        public bool Equals(User other)
        {
            return 
                Email == other.Email 
                && FirstName == other.FirstName 
                && LastName == other.LastName 
                && PhoneNumber == other.PhoneNumber;
        }
    }
}
