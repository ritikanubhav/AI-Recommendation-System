using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CsvHelper;
using Pearson.Entities;

namespace Pearson.DataLoaderModule
{
    public class CsvDataLoader : IDataLoader
    {

        private const string BooksFilePath = "BX-CSV-Dump/BX-Books.csv";
        private const string UsersFilePath = "BX-CSV-Dump/BX-Users.csv";
        private const string BookRatingsFilePath = "BX-CSV-Dump/BX-Book-Ratings.csv";
        public BookDetails Load()
        {
            BookDetails bookDetails = new BookDetails();

            //////load data from BX-Books.csv(in BX-CSV-Dump) in list of Books of bookDetails
            //bookDetails.Books = LoadBooks();

            //////load data from BX-Users.csv(in BX-CSV-Dump) in list of Users of bookDetails
            //bookDetails.Users = LoadUsers();

            //////load data from BX-Books-Ratings.csv(in BX-CSV-Dump) in list of BookUserRatings of bookDetails
            //bookDetails.UserRatings = LoadBookUserRatings();

            //usig multithreading -parallel
            Parallel.Invoke(
                () => bookDetails.Books = LoadBooks(),
                () => bookDetails.Users = LoadUsers(),
                () => bookDetails.UserRatings = LoadBookUserRatings()
            );
            return bookDetails;
        }

        private List<Book> LoadBooks()
        {
            var books = new List<Book>();
            using (var reader = new StreamReader(BooksFilePath))
            {
                string headerLine = reader.ReadLine(); // Skip header
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    if(fields.Length !=8)
                    { continue; }

                    // Strip quotes and extraspaces from fields if present
                    fields = fields.Select(field => field.Trim()).ToArray();
                    fields = fields.Select(field => field.Trim('"')).ToArray();
                    

                    var book = new Book
                    {
                        ISBN = fields[0],
                        BookTitle = fields[1],
                        Author = fields[2],
                        YearOfPublication = int.Parse(fields[3]),
                        Publisher = fields[4],
                        ImgaeUrlSmall = fields[5],
                        ImgaeUrlMedium = fields[6],
                        ImgaeUrlLarge = fields[7],
                    };
                    books.Add(book);
                }
            }
            return books;
        }

        private List<User> LoadUsers()
        {
            var users = new List<User>();
            using (var reader = new StreamReader(UsersFilePath))
            {
                string headerLine = reader.ReadLine(); // Skip header
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');
                    if(fields.Length !=3)
                    { continue; }

                    // Strip quotes and extraspaces from fields if present
                    fields = fields.Select(field => field.Trim()).ToArray();
                    fields = fields.Select(field => field.Trim('"')).ToArray();

                    var user = new User
                    {
                        UserId = int.Parse(fields[0])
                    };

                    // Split Location into City, State, and Country
                    var locationParts = fields[1].Split(new[] { ',',' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    if (locationParts.Length == 3)
                    {
                        user.City = locationParts[0];
                        user.State = locationParts[1];
                        user.Country = locationParts[2];
                    }
                    else if (locationParts.Length == 2)
                    {
                        user.City = locationParts[0];
                        user.State = locationParts[1];
                        user.Country = string.Empty; // Default to empty if country is missing
                    }
                    else if (locationParts.Length == 1)
                    {
                        user.City = locationParts[0];
                        user.State = string.Empty;
                        user.Country = string.Empty;
                    }
                    // Handle Age if not present
                    if (fields.Length<3)
                    {
                        user.Age = 40;//default to 40
                    }
                    else
                    {
                        if (int.TryParse(fields[2], out int age))
                        {
                            user.Age = age;
                        }
                        else
                        {
                            user.Age = 40; // Default to 40 if parsing fails
                        }
                    }
                    users.Add(user);
                }
            }
            return users;
        }

        private List<BookUserRating> LoadBookUserRatings()
        {
            var ratings = new List<BookUserRating>();
            using (var reader = new StreamReader(BookRatingsFilePath))
            {
                string headerLine = reader.ReadLine(); // Skip header
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    // Strip quotes and extraspaces from fields if present
                    fields = fields.Select(field => field.Trim()).ToArray();
                    fields = fields.Select(field => field.Trim('"')).ToArray();

                    var rating = new BookUserRating
                    {
                        UserID = int.Parse(fields[0]),
                        ISBN = fields[1],
                        Rating = int.Parse(fields[2])
                    };
                    ratings.Add(rating);
                }
            }
            return ratings;
        }
    }
}
