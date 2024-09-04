using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pearson.Entities;
using Pearson.DataLoaderModule;
using Pearson.AggrigatorModule;
using PearsonCoreEngineModule;
using System.Diagnostics;

namespace Pearson.IntrigatorModule
{
    public class AIRecommendationEngine
    {
        // Creating instances of various modules for loading data, aggregating ratings, and calculating recommendations.
        public int ScannedBooks = 0;
        IDataLoader DataLoader = new CsvDataLoader();
        IRecommender Recommender = new PearsonRecommender();
        IRatingsAggrigator Aggrigator = new RatingsAggrigator();

        /// <summary>
        /// Recommends a list of books based on user preferences, using Pearson correlation to determine similarity.
        /// </summary>
        /// <param name="preference">User preferences including state, age group, and a specific book ISBN.</param>
        /// <param name="limit">The maximum number of books to recommend.</param>
        /// <returns>A list of recommended books.</returns>
        public List<Book> Recommend(Preference preference, int limit)
        {
            // List to store the recommended books.
            List<Book> recommendedBooks = new List<Book>();

            // Load book details and measure the time taken for loading.
            Console.WriteLine("..................1. Books Data loading started..................\n");
            Stopwatch sw = Stopwatch.StartNew();
            BookDetails bookDetails = DataLoader.Load();
            Console.WriteLine("...............Loading completed successfully in " + sw.ElapsedMilliseconds + " ms............\n");

            // Aggregate ratings based on user preferences and measure the time taken.
            Console.WriteLine("..................2. Aggregation Phase Started..................\n");
            sw.Restart();
            var aggregate = Aggrigator.Aggrigate(bookDetails, preference);
            Console.WriteLine($"\n...............Aggregation completed Successfully in " + sw.ElapsedMilliseconds + " ms............\n");

            // check if aggrigate returned contains preference book or not
            if(aggregate == null || !(aggregate.ContainsKey(preference.ISBN)))
            {
                return recommendedBooks; //returning empty recommendedBooks array
            }

            // Get the base array for Pearson correlation calculation using the preferred ISBN.
            int[] baseArray = aggregate[preference.ISBN].ToArray();
            
            // Variable to track the current number of recommendations.
            int recommendationCount = 0;

            // List to store the ISBNs of recommended books.
            List<string> isbnList = new List<string>();
            foreach (var kvp in aggregate)
            {
                string isbn = kvp.Key;
                if (isbn != preference.ISBN)
                {
                    int[] otherArray = aggregate[isbn].ToArray();

                    // Calculate the Pearson correlation coefficient between the base book and the current book.
                    double pearsonValue = Recommender.GetCorrelation(baseArray, otherArray);

                    ScannedBooks++;
                    if (pearsonValue > 0.75) // Consider it a good recommendation if the Pearson value is greater than 0.75.
                    {
                        isbnList.Add(isbn);
                        recommendationCount++;
                    }
                    if (recommendationCount >= limit)
                        break; // Stop if the recommendation limit is reached.
                }
            }

            // Retrieve the recommended books from the book details using the list of ISBNs.
            recommendedBooks = bookDetails.Books.Where(book => isbnList.Contains(book.ISBN)).ToList();

            return recommendedBooks;
        }

    }
}
