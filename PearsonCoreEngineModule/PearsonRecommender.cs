using System;

namespace PearsonCoreEngineModule
{
    public class PearsonRecommender : IRecommender
    {
        /// <summary>
        /// Calculates the Pearson correlation coefficient for two integer arrays.
        /// </summary>
        /// <param name="baseData">First array of integers.</param>
        /// <param name="otherData">Second array of integers.</param>
        /// <returns>Pearson correlation coefficient as a double.</returns>
        public double GetCorrelation(int[] baseData, int[] otherData)
        {
            int size = baseData.Length;
            // both array must be of equal length for pearson calculation
            int[] baseArray = new int[size];
            int[] otherArray = new int[size];

            // Adjust the arrays for Pearson calculation 
            for (int i = 0; i < size; i++)
            {
                // If otherData is shorter, fill the remaining elements of otherArray with 1
                // and increment corresponding elements in baseArray by 1.
                if (i < otherData.Length)
                {
                    baseArray[i] = baseData[i];
                    otherArray[i] = otherData[i];
                }
                else
                {
                    baseArray[i] = baseData[i] + 1;
                    otherArray[i] = 1;
                }

                // If an element in baseArray is 0, increment both baseArray and otherArray at that index by 1.
                // Do not increment if the otherArray element is already 10.
                if (baseArray[i] == 0)
                {
                    baseArray[i]++;
                    if (otherArray[i] < 10)
                        otherArray[i]++;
                }

                // Similarly, if an element in otherArray is 0, increment both arrays,
                // unless the baseArray element is already 10.
                if (otherArray[i] == 0)
                {
                    otherArray[i]++;
                    if (baseArray[i] < 10)
                        baseArray[i]++;
                }
            }

            //---> Pearson Formula Calculations:
            // X: baseArray, Y: otherArray

            double result = 0;             // Final Pearson correlation coefficient
            double n = size;               // N: Number of elements
            double sumOfXY = 0;            // Sum of products of corresponding elements in X and Y
            double sumOfX = 0;             // Sum of elements in X
            double sumOfY = 0;             // Sum of elements in Y
            double sumOfSquaresOfX = 0;    // Sum of squares of elements in X
            double sumOfSquaresOfY = 0;    // Sum of squares of elements in Y

            // Calculate sums and sums of squares
            for (int i = 0; i < n; i++)
            {
                sumOfXY += baseArray[i] * otherArray[i];
                sumOfX += baseArray[i];
                sumOfY += otherArray[i];
                sumOfSquaresOfX += baseArray[i] * baseArray[i];
                sumOfSquaresOfY += otherArray[i] * otherArray[i];
            }

            // R1 = (N * Sum(XY)) – (Sum(X) * Sum(Y))
            double r1 = (n * sumOfXY) - (sumOfX * sumOfY);

            // R2 = ((N * Sum(X^2)) – (Sum(X)^2)) * ((N * Sum(Y^2)) – (Sum(Y)^2))
            double r2 = ((n * sumOfSquaresOfX) - (sumOfX * sumOfX)) * ((n * sumOfSquaresOfY) - (sumOfY * sumOfY));

            // To avoid division by zero, handle the case where r2 is zero
            if (r2 == 0)
                return 0;

            // R = R1 / sqrt(R2)
            result = r1 / Math.Sqrt(r2);

            return result;
        }
    }
}
