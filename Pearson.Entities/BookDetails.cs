using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pearson.Entities
{
    public class BookDetails
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public List<BookUserRating> UserRatings { get; set; }= new List<BookUserRating>();
        public List<User> Users { get; set; }= new List<User>();
    }
}
