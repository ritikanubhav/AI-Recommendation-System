using Pearson.Entities;
using Pearson.DataLoaderModule;
using Pearson.AggrigatorModule;
using Pearson.IntrigatorModule;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Preference preference = new Preference
                {
                    //ISBN= "042518630X",
                    ISBN= "0440234743", 
                    //ISBN= "0399135782",
                    //ISBN = "0440223571",
                    //ISBN= "0245542957 ",

                    Age = 50,
                    State="washington"
                };

                Stopwatch sw = Stopwatch.StartNew();

                Console.WriteLine("\n---------------------------------------------------------------------");
                Console.WriteLine($"Getting Recommendations for Book with ISBN:{preference.ISBN} .....");
                Console.WriteLine("\n---------------------------------------------------------------------");

                //creating instance of AIRecommendationEngine to use recommend method
                AIRecommendationEngine aIRecommendationEngine = new AIRecommendationEngine();

                // calling recommendationEngine to get recommnedations
                List<Book> recommendedBooks =aIRecommendationEngine.Recommend(preference,10) ;

                Console.WriteLine("\n---------------------------------------------------------------------");

                if(recommendedBooks.Count > 0)
                {
                    Console.WriteLine("--------------------------Recommended Books--------------------------\n");
                    foreach (Book book in recommendedBooks)
                    {
                        Console.WriteLine($"\t\t{book.BookTitle}\t");
                    }
                }
                else
                {
                    Console.WriteLine("Sorry! No Recommendations for this book Available.");
                }
                
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine("Total Books Scanned in order to get the recommendations: " +aIRecommendationEngine.ScannedBooks);
                Console.WriteLine("Total time taken for Recommendations : "+sw.ElapsedMilliseconds+" ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
