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
using static System.Reflection.Metadata.BlobBuilder;
namespace Pearson.IntrigatorModule
{
    public class AIRecommendationEngine
    {
        // creating instances for using their methods
        public int ScannedBooks=0;
        IDataLoader DataLoader=new CsvDataLoader();
        IRecommender Recommender=new PearsonRecommender();
        IRatingsAggrigator Aggrigator=new RatingsAggrigator();

        public List<Book> Recommend(Preference preference,int limit)
        {
            // list to store Recommended books
            Console.WriteLine("..................BooksData loading started..................");
                Stopwatch sw = Stopwatch.StartNew();
            List<Book> recommendedBooks = new List<Book>();
            BookDetails bookDetails= DataLoader.Load();
            Console.WriteLine("...............Loading completed successfully in "+ sw.ElapsedMilliseconds+" ms............");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Aggregator called for other arrays.............");
            var aggregate = Aggrigator.Aggrigate(bookDetails, preference);
            Console.WriteLine($"--------------------Aggregator finished:{aggregate.Count} items-----------------------");
            
            // base array for pearson calculation
            Console.WriteLine("Aggregator called for base array");
            int[] baseArray = aggregate[preference.ISBN].ToArray();
            Console.WriteLine("Aggregator ratings for base array");
            for(int i=0;i<baseArray.Length;i++)
            {
                Console.Write(baseArray[i]+" ");
            }
            Console.WriteLine();

            int count = 1;
            Console.WriteLine("----------Total books to be scanned: "+ bookDetails.Books.Count+"-----------");
            Console.WriteLine("---------------------------------------------------------");
           
            List<string> isbnlist = new List<string>();
            foreach(var kvp in aggregate)
            {
                string isbn=kvp.Key;
                if (isbn != preference.ISBN)
                {
                    int[] otherArray = aggregate[isbn].ToArray();
                    //Console.WriteLine("Calculation started for the array" + count);
                    double pearsonValue = Recommender.GetCorrelation(baseArray, otherArray);
                    //Console.WriteLine("calculation ended for the array with pearson value= "+pearsonValue);

                    //BookWithPearsonValue[book] = pearsonValue;
                    ScannedBooks++;
                    if (pearsonValue > 0.5)//good recommendation
                    {
                        isbnlist.Add(isbn);
                        //Console.WriteLine($"Recommendation {count}:\t Pearson coefficient:{pearsonValue}");
                        count++;
                    }
                    if (count > limit)
                    {
                        break;
                    }
                }
            }
            recommendedBooks= bookDetails.Books.Where(book => isbnlist.Contains(book.ISBN)).ToList();


            //sorting all books based on pearson value
            //var sortedRecommendations = BookWithPearsonValue.OrderByDescending(kvp => kvp.Value).ToList();

            //getting the books as per limit
            //for (int i = 0; i < limit; i++)
            //{
            //    recommendedBooks.Add(sortedRecommendations[i].Key);
            //}

            return recommendedBooks;
        }

    }
}
