using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonCoreEngineModule
{
    public class PearsonRecommender:IRecommender
    {
        public double GetCorrelation(int[] baseData, int[] otherData)
        {
            int size=baseData.Length;
            int[]baseArray=new int[size];
            int[]otherArray=new int[size];

            //if any array element value is 0 add 1 to both
            // but not to the elemnet whose value is already 10
            for(int i=0;i<size; i++)
            {
                //if other array is smaller in size ( the rest of element is 0 in it) add 1 to both corresponding elements
                if (i<otherData.Length)
                {
                    baseArray[i] = baseData[i];
                    otherArray[i] = otherData[i];
                }
                else
                {
                    baseArray[i] += 1;
                    otherArray[i] = 1;
                }

                if(baseArray[i]==0)
                {
                    baseArray[i]++;
                    if(otherArray[i]!=10)
                        otherArray[i]++;
                }
                if (otherArray[i] == 0)
                {
                    otherArray[i]++;
                    if (baseArray[i]!=10)
                        baseArray[i]++;
                }
            }

            //---> Pearson Formula Calculations:
            //X:baseData array and Y:otherData array

            double result = 0; //final pearson coefficient result
            double n = size;//N 
            double sumOfXY = 0;//Sum(XY) 
            double sumOfX = 0;//Sum(X) 
            double sumOfY = 0;//Sum(Y) 
            double sumOfSquaresOfX = 0;//Sum(X^2) 
            double sumOfSquaresOfY= 0;//Sum(Y^2)

            //calculating the above values
            for (int i = 0; i < n; i++)
            {
                sumOfXY += baseArray[i] * otherArray[i];
                sumOfX += baseArray[i];
                sumOfY += otherArray[i];
                sumOfSquaresOfX += (baseArray[i]* baseArray[i]);
                sumOfSquaresOfY += (otherArray[i] * otherArray[i]);
            }

            //R1 = (N * Sum(XY)) – (Sum(X) * Sum(Y))
            double r1 = (n * sumOfXY) - (sumOfX*sumOfY);

            //R2 = ((N * Sum(X^2)) – (Sum(X) * Sum(X))) *((N * Sum(Y^2)) – (Sum(Y) *Sum(Y))
            double r2= ((n * sumOfSquaresOfX) - (sumOfX*sumOfX))*((n*sumOfSquaresOfY)-(sumOfY*sumOfY));
            if (r2 == 0)
                r2 = 1;

            //R = R1 / Sqrt(R2)
            result=r1/Math.Sqrt(r2);

            return result;
        }
    }
}
