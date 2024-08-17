using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pearson.Entities;
namespace Pearson.AggrigatorModule
{
    public interface IRatingsAggrigator
    {
        public Dictionary<string,List<int>> Aggrigate(BookDetails bookDetail,Preference preference);

    }
}
