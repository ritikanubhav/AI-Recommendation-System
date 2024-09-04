using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pearson.Entities;

namespace Pearson.DataLoaderModule
{
    public class CsvDataLoader : IDataLoader
    {
        private const string BooksFilePath = "BX-CSV-Dump/BX-Books.csv";
        private const string UsersFilePath = "BX-CSV-Dump/BX-Users.csv";
        private const string BookRatingsFilePath = "BX-CSV-Dump/BX-Book-Ratings.csv";

        /// <summary>
        /// Loads data from multiple CSV files into the BookDetails object using parallel processing.
        /// </summary>
        /// <returns>A populated BookDetails object containing lists of books, users, and ratings.</returns>
        public BookDetails Load()
        {
            BookDetails bookDetails = new BookDetails();

            // Load data from CSV files into the respective lists in parallel
            Parallel.Invoke(
                () => bookDetails.Books = LoadBooks(), // Load books data
                () => bookDetails.Users = LoadUsers(), // Load users data
                () => bookDetails.UserRatings = LoadBookUserRatings() // Load book ratings data
            );

            return bookDetails;
        }

        /// <summary>
        /// Loads book data from the BX-Books.csv file.
        /// </summary>
        /// <returns>A list of Book objects.</returns>
        private List<Book> LoadBooks()
        {
            var books = new List<Book>();
            using (var reader = new StreamReader(BooksFilePath))
            {
                string headerLine = reader.ReadLine(); // Skip the header line
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    // Skip any lines that do not contain exactly 8 fields
                    if (fields.Length != 8)
                    {
                        continue;
                    }

                    // Remove extra spaces and quotes from each field
                    fields = fields.Select(field => field.Trim()).ToArray();
                    fields = fields.Select(field => field.Trim('"')).ToArray();

                    // Create a new Book object and populate its properties
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

                    // Add the book to the list
                    books.Add(book);
                }
            }
            //logging details to console about the Book Data loaded
            Console.WriteLine("Total books loaded: " +books.Count);

            return books;
        }

        /// <summary>
        /// Loads user data from the BX-Users.csv file.
        /// </summary>
        /// <returns>A list of User objects.</returns>
        private List<User> LoadUsers()
        {
            var users = new List<User>();
            using (var reader = new StreamReader(UsersFilePath))
            {
                string headerLine = reader.ReadLine(); // Skip the header line
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    // Skip any lines that do not contain exactly 3 fields
                    if (fields.Length != 3)
                    {
                        continue;
                    }

                    // Remove extra spaces and quotes from each field
                    fields = fields.Select(field => field.Trim()).ToArray();
                    fields = fields.Select(field => field.Trim('"')).ToArray();

                    // Create a new User object and populate its properties
                    var user = new User
                    {
                        UserId = int.Parse(fields[0])
                    };

                    // Split the location field into City, State, and Country
                    var locationParts = fields[1].Split(new[] { ',', ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

                    // Assign the location parts to the corresponding properties of the user
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

                    // Handle the Age field, setting a default value if missing or invalid
                    if (fields.Length < 3)
                    {
                        user.Age = 40; // Default to 40 if age is missing
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

                    // Add the user to the list
                    users.Add(user);
                }
            }
            //logging users data loaded to console
            Console.WriteLine("Total users loaded: " + users.Count);
            return users;
        }

        /// <summary>
        /// Loads book rating data from the BX-Book-Ratings.csv file.
        /// </summary>
        /// <returns>A list of BookUserRating objects.</returns>
        private List<BookUserRating> LoadBookUserRatings()
        {
            var ratings = new List<BookUserRating>();
            using (var reader = new StreamReader(BookRatingsFilePath))
            {
                string headerLine = reader.ReadLine(); // Skip the header line
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');

                    // Remove extra spaces and quotes from each field
                    fields = fields.Select(field => field.Trim()).ToArray();
                    fields = fields.Select(field => field.Trim('"')).ToArray();

                    // Create a new BookUserRating object and populate its properties
                    var rating = new BookUserRating
                    {
                        UserID = int.Parse(fields[0]),
                        ISBN = fields[1],
                        Rating = int.Parse(fields[2])
                    };

                    // Add the rating to the list
                    ratings.Add(rating);
                }
            }
            //logging user-rating data loaded to console
            Console.WriteLine("Total User Ratings loaded: " + ratings.Count + "\n");

            return ratings;
        }
    }
}
