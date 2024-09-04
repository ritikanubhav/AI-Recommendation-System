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
        //public static int booksDone = 0;

        /// <summary>
        /// Aggregates book ratings based on user preferences and returns a dictionary of ISBNs with corresponding ratings.
        /// </summary>
        /// <param name="bookDetails">The BookDetails object containing user and rating information.</param>
        /// <param name="preference">The user preference to filter ratings by state and age group.</param>
        /// <returns>A dictionary with ISBNs as keys and lists of ratings as values.</returns>
        public Dictionary<string, List<int>> Aggrigate(BookDetails bookDetails, Preference preference)
        {
            //dictionary to store ISBNs as keys and lists of ratings as values
            Dictionary<string, List<int>> RatingList = new Dictionary<string, List<int>>();

            // Filter the list of users based on the given preference (state and age group).
            var preferredUserList = (from u in bookDetails.Users
                                     where u.State == preference.State &&
                                     GetAgeGroup(u.Age) == GetAgeGroup(preference.Age)
                                     select u.UserId).ToHashSet();

            //logging Preferred User data to Console
            Console.WriteLine("Total Users of Same Age Group and State: "+preferredUserList.Count);

            var ratingList = new Dictionary<string, List<int>>();

            // Aggregate the ratings for each book, only including users that match the preference.
            foreach (var u in bookDetails.UserRatings)
            {
                if (preferredUserList.Contains(u.UserID))
                {
                    if (!ratingList.ContainsKey(u.ISBN))
                    {
                        ratingList.Add(u.ISBN, new List<int> { u.Rating });
                    }
                    else
                        ratingList[u.ISBN].Add(u.Rating);
                }
            }
            // log the relevant books data to console
            Console.WriteLine("Total Relevant books for my User: "+ratingList.Count);

            return ratingList;
        }

        /// <summary>
        /// Determines the age group label for a given age based on data from AgeGroup.txt.
        /// </summary>
        /// <param name="age">The age to determine the group for.</param>
        /// <returns>The age group label as a string.</returns>
        private string GetAgeGroup(int age)
        {
            // Load age group data from the AgeGroup.txt file into a list of tuples.
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

            // Check in which age group the given age belongs.
            foreach (var ageGroup in ageGroups)
            {
                if (age >= ageGroup.Item1 && age <= ageGroup.Item2)
                {
                    return ageGroup.Item3;
                }
            }
            return "Unknown";
        }
    }
}
