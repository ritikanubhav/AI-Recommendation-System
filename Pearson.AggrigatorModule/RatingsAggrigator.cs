using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pearson.Entities;

namespace Pearson.AggrigatorModule
{
    public class RatingsAggrigator : IRatingsAggrigator
    {
        public static int booksDone = 0;
        public Dictionary<string, List<int>> Aggrigate(BookDetails bookDetails, Preference preference)
        {
            Dictionary<string,List<int>> RatingList = new Dictionary<string,List<int>>();

            //get all users who has same preference values
            // Create a new HashSet of integers

            //HashSet<int> preferredUserIds = new HashSet<int>();
            //foreach (var user in bookDetails.Users)
            //{
            //    if (GetAgeGroup(preference.Age) == GetAgeGroup(user.Age)&& user.State==preference.State)
            //    {
            //        preferredUserIds.Add(user.UserId);
            //        Console.WriteLine(user.UserId);
            //    }
            //}

            ////get all ratings for the book of preference isbn by prferredUsers
            //List<int> ratings = new List<int>();
            //Console.WriteLine("the rating are:");
            //foreach (var userRating in bookDetails.UserRatings)
            //{
            //    if (userRating.ISBN==preference.ISBN && preferredUserIds.Contains(userRating.UserID))
            //    {
            //        ratings.Add(userRating.Rating);
            //        Console.WriteLine(userRating.Rating);
            //    }
            //}

            //RatingList[preference.ISBN] = ratings;
            //return RatingList;

            //optimized using linq and hashset------------

            //Get all users who have the same preference values
            //var preferredUserIds = bookDetails.Users
            //    .Where(user => user.State == preference.State && GetAgeGroup(preference.Age) == GetAgeGroup(user.Age))
            //    .Select(user => user.UserId)
            //    .ToHashSet();

            //// Get all ratings for the book of preference.ISBN by preferredUsers
            //var ratings = bookDetails.UserRatings
            //    .Where(userRating => userRating.ISBN == preference.ISBN && preferredUserIds.Contains(userRating.UserID))
            //    .Select(userRating => userRating.Rating)
            //    .ToList();

            Stopwatch sw=Stopwatch.StartNew();

            var preferredUserList = (from u in bookDetails.Users
                            where u.State == preference.State &&
                            GetAgeGroup(u.Age) == GetAgeGroup(preference.Age)
                            select u.UserId).ToHashSet();

            var ratingList = new Dictionary<string, List<int>>();

            foreach(var u in bookDetails.UserRatings)
            {
                if(preferredUserList.Contains(u.UserID))
                {
                    if(!ratingList.ContainsKey(u.ISBN))
                    {
                        ratingList.Add(u.ISBN, new List<int> { u.Rating });
                    }
                    else
                        ratingList[u.ISBN].Add(u.Rating);
                }
            }

            //for(int i=0;i<60;i++)
            //{
            //    Stopwatch sw1= Stopwatch.StartNew();
            //    string isbn = bookDetails.Books[i].ISBN;
            //    var ratings = (from user in bookDetails.Users
            //                   join rating in bookDetails.UserRatings
            //                   on user.UserId equals rating.UserID
            //                   where rating.ISBN == isbn && user.State == preference.State
            //                   && GetAgeGroup(user.Age) == GetAgeGroup(preference.Age)
            //                   select rating.Rating).ToList();

            //    Console.WriteLine($"{booksDone++} :{sw1.ElapsedMilliseconds}");
            //    ratingList[isbn] = ratings;
            //}//);

            
            
            //foreach (var kv in ratingList)
            //{
            //    Console.WriteLine("Ratings in array:");
            //    foreach (var rating in kv.Value)
            //    {
            //        Console.Write(rating+"_");
            //    }
            //    Console.WriteLine();
            //}
            Console.WriteLine("aggrigation took :"+sw.ElapsedMilliseconds);
            return ratingList;
        }
        private string GetAgeGroup(int age)
        {
            //Load AgeGroupData from File AgeGroup.txt in list of tuple having 3 values

            List<Tuple<int, int, string>> ageGroups = new List<Tuple<int, int, string>>();

            foreach (var line in File.ReadAllLines("AgeGroup.txt"))
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    var rangeParts = parts[0].Split('-');
                    if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int startAge) && int.TryParse(rangeParts[1], out int endAge))
                    {
                        var label = parts[1].Trim();
                        ageGroups.Add(Tuple.Create(startAge, endAge, label));
                    }
                }
            }

            // check in which group the given age belongs to
            foreach (var ageGroup in ageGroups)
            {   
                if (age >= ageGroup.Item1 && age <= ageGroup.Item2)
                {
                    //Console.WriteLine(ageGroup.Item1 + " " + ageGroup.Item2 + " " + ageGroup.Item3);
                    return ageGroup.Item3;
                }
            }
            return "Unknown";
        }
    }
}
