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
        public void CalculatePearsonCoefficient_AllNonZeroElementsforEqualSizedArray_ShouldGiveExpectedtValue()
        {
            //Arrange
            int[] array1 = { 20, 24, 17 };
            int[] array2 = { 30, 20, 27 };
            //int[] array1 = { 5 };
            //int[] array2 = { 5 };
            double expectedRes = -0.739853246;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1,array2);

            //Assert
            Assert.AreEqual(expectedRes,res,tolerance);
        }
        [Test]
        public void CalculatePearsonCoefficient_Array1IsSmallerThanArray2_ShouldGiveExpectedtValue()
        {
            //Arrange
            int[] array1 = { 20, 24, 17 };
            int[] array2 = { 30, 20, 27, 2 };
            double expectedRes = -0.739853246;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }
        [Test]
        public void CalculatePearsonCoefficient_Array1IsLargerThanArray2_ShouldGiveExpectedtValue()
        {
            //Arrange
            int[] array1 = { 20, 24, 12, 17 };
            int[] array2 = { 30, 20, 27 };
            double expectedRes = 0.7403647469;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }
        [Test]
        public void CalculatePearsonCoefficient_ArrayContainsZeroValues_ShouldGiveExpectedtValue()
        {
            //Arrange
            int[] array1 = { 20, 0, 0, 17 };
            int[] array2 = { 30, 0, 27 ,0};
            double expectedRes = 0.0928491184;
            double tolerance = 1e-8;

            //Act
            double res = recommender.GetCorrelation(array1, array2);

            //Assert
            Assert.AreEqual(expectedRes, res, tolerance);
        }
    }
}