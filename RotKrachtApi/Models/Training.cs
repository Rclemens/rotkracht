using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotKrachtApi.Models
{
    internal class Training
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
