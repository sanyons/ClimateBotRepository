using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateBot.Models
{
    public class NewsArticle
    {
        // SOLID: Single Responsibility Principle (SRP)
        // Esta clase solo se encarga de representar un artículo de noticias
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string PublishedDate { get; set; }
        public string Source { get; set; }
    }
}
}
