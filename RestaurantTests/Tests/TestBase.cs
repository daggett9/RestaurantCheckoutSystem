using RestaurantTests.Base;
using TechTalk.SpecFlow;

namespace RestaurantTests.Tests
{
    [Binding]
    public class TestBase
    {
        private readonly ScenarioContext _scenarioContext;

        public TestBase(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Console.WriteLine("\n----------------------------------------------------------------------");
            Console.WriteLine($"Setup and start of '{TestContext.CurrentContext.Test.Name}' test.");

            var testDependentData = new TestDependentData();
            _scenarioContext.ScenarioContainer.RegisterInstanceAs(testDependentData);
            _scenarioContext.ScenarioContainer.RegisterInstanceAs(new BaseTestHelper(testDependentData));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine($"End of '{TestContext.CurrentContext.Test.Name}' test.");
            Console.WriteLine("----------------------------------------------------------------------\n");
        }

        [BeforeStep]
        public void BeforeStep()
        {
            Console.WriteLine($"Test step: {_scenarioContext.StepContext.StepInfo.Text}");
        }
    }
}