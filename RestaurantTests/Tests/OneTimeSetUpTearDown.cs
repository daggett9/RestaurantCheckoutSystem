using RestaurantTests.Managers;
using TechTalk.SpecFlow;
using Utils.Configuration;

namespace RestaurantTests.Tests
{
    [SetUpFixture]
    [Binding]
    public class OneTimeSetUpTearDown
    {
        public static RestaurantConfiguration RestaurantConfiguration { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Console.WriteLine($"{nameof(OneTimeSetUp)} block has started.");
            RestaurantConfiguration = ConfigManager.Instance.RestaurantConfiguration;
            Console.WriteLine($"{nameof(OneTimeSetUp)} block has ended.");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Console.WriteLine($"{nameof(OneTimeTearDown)} block has started.");

            Console.WriteLine($"{nameof(OneTimeTearDown)} block has ended.");
        }
    }
}