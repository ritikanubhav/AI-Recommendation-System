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
                // create a menu for these books, take choice and limit for recommendations ,
                //  age and state also and
                // set isbn of preference accordingly
                // and then generate recommendations

                //0399135782      The Kitchen God's Wife
                //0345402871      Airframe
                //0375759778      Prague: A Novel
                //0060168013      Pigs in Heaven
                //0553582747      From the Corner of His Eye
                //042518630X      Purity in Death
                //0440223571      This Year It Will Be Different: And Other Stories
                //0060914068      Love, Medicine and Miracles
                //0156047624      All the King's Men
                //0245542957      Pacific Northwest

                Stopwatch sw = Stopwatch.StartNew();
                Preference preference = new Preference
                {
                    ISBN= "0440234743",
                    //ISBN = "0448401738",
                    Age = 40,
                    State="washington"
                };

                Console.WriteLine("\n---------------------------------------------------------------------");
                Console.WriteLine($"Getting Recommendations for Book with ISBN:{preference.ISBN} .....");
                Console.WriteLine("\n---------------------------------------------------------------------");
                AIRecommendationEngine aIRecommendationEngine = new AIRecommendationEngine();
                List<Book> recommendedBooks =aIRecommendationEngine.Recommend(preference,10) ;
                Console.WriteLine("\n---------------------------------------------------------------------");
                Console.WriteLine("--------------------------Recommended Books--------------------------");
                foreach (Book book in recommendedBooks)
                {
                    Console.WriteLine($"\t{book.ISBN}\t{book.BookTitle}\t");
                }
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine("Total time taken for Recommendations : "+sw.ElapsedMilliseconds+" ms");
                Console.WriteLine("Total Books Scanned in order to get the recommendations: " +aIRecommendationEngine.ScannedBooks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
