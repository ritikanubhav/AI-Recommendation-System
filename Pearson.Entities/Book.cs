using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pearson.Entities
{
    public class Book
    {
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public int YearOfPublication { get; set; }
        public string ImgaeUrlSmall { get; set; }
        public string ImgaeUrlMedium { get; set; }
        public string ImgaeUrlLarge { get; set; }

        public List<BookUserRating> UserRating { get; set; } = new List<BookUserRating>();
    }
}
