using PearsonCoreEngineModule;
namespace PearsonCoreEngine.UnitTest
{
    public class Tests
    {
        IRecommender recommender;
        [SetUp]
        public void Setup()
        {
            recommender = new PearsonRecommender();
        }

        [Test]
        public void CalculatePearsonCoefficient_IfAllNonZeroElementsforEqualSizedArray_ShouldGiveExpectedtValue()
        {
            //Arrange
            int[] array1 = { 1,2,3 };
            int[] array2 = { 1,2,4 };
            
            double expectedRes = 0.981980506;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1,array2);

            //Assert
            Assert.AreEqual(expectedRes,res,tolerance);
        }
        [Test]
        public void CalculatePearsonCoefficient_IfArray1IsSmallerThanArray2_ShouldGiveExpectedtValue()
        {
            //Arrange
            int[] array1 = { 1,2,3 };
            int[] array2 = { 1,2,3,4 };
            double expectedRes = 1.0;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }
        [Test]
        public void CalculatePearsonCoefficient_IfArray1IsLargerThanArray2_ShouldGiveExpectedtValue()
        {
            //Arrange
            int[] array1 = { 1,2,3,4 };
            int[] array2 = { 1,2,3 };
            double expectedRes = -0.050964719143;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }
        [Test]
        public void CalculatePearsonCoefficient_IfArrayContainsZeroAsElements_ShouldGiveExpectedtValue()
        {
            //Arrange
            int[] array1 = { 1, 0, 0, 2};
            int[] array2 = { 3, 0, 4 ,2};
            double expectedRes = -0.292770021884;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }

        [Test]
        public void CalculatePearsonCoefficient_IfExactSameArray_ShouldReturnOne()
        {
            //Arrange
            int[] array1 = {1,2,3 };
            int[] array2 = { 1,2,3 };
            double expectedRes = 1.0;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }
        [Test]
        public void CalculatePearsonCoefficient_IfCompletelyDifferentArray_ShouldReturnNegativeOne()
        {
            //Arrange
            int[] array1 = { 1, 2, 3 };
            int[] array2 = { 3,2,1};
            double expectedRes = -1.0;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }

        [Test]
        public void CalculatePearsonCoefficient_NoCorelation_ShouldReturnZero()
        {
            //Arrange
            int[] array1 = { 1, 2, 3 };
            int[] array2 = { 2, 2, 2 };
            double expectedRes = 0.0;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }

    }
}