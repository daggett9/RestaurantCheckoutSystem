namespace RestaurantTests.Base
{
    public class BaseTestHelper
    {
        public TestDependentData TestDependentData { get; }

        public BaseTestHelper(TestDependentData testDependentData)
        {
            TestDependentData = testDependentData;
        }
    }
}