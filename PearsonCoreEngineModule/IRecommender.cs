using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonCoreEngineModule
{
    public interface IRecommender
    {
        public double GetCorrelation(int[] baseData, int[] otherData);
    }
}
