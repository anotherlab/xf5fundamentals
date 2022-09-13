using System;
using System.Collections.Generic;
using System.Text;

namespace ViewsDemo.Models
{
    public class Leader
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string ImageUrl { get; set; }


        public Leader()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
