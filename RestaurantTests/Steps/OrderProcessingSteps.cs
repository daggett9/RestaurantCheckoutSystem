using NewEndpointMockup;
using RestaurantTests.Base;
using RestaurantTests.Tests;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Utils.Configuration;
using Utils.DataModels;
using Utils.Enums;
using Utils.Extensions;

namespace RestaurantTests.Steps
{
    [Binding]
    public class OrderProcessingSteps
    {
        private readonly BaseTestHelper _baseTestHelper;
        private readonly RestaurantConfiguration _config;

        public OrderProcessingSteps(BaseTestHelper baseTestHelper)
        {
            _baseTestHelper = baseTestHelper;
            _config = OneTimeSetUpTearDown.RestaurantConfiguration;
        }

        [Given("customer placed new orders")]
        public void CustomerPlacedANewOrders(Table orderTable)
        {
            var orderTableData = orderTable.CreateSet<OrderTableData>();
            var orders = CreateOrders(orderTableData);
            _baseTestHelper.TestDependentData.Orders.AddRange(orders);
        }

        [When("customer updated the existing orders")]
        public void ThenCustomerUpdatedTheExistingOrders(Table orderTable)
        {
            var orderTableData = orderTable.CreateSet<OrderTableData>();
            UpdateOrders(orderTableData.ToList());
        }

        [Then("bill for the (.*) is correctly calculated")]
        public void ThenBillForTheOrderIsCorrectlyCalculated(string orderNumber)
        {
            var order = _baseTestHelper.TestDependentData.Orders.Single(o => o.OrderNumber == orderNumber);
            var expectedTotalCost = CalculateTotalOfOrder(order);
            var actualTotalCost = TotalOrderCalculator.CalculateTotalOfOrder(order);

            Assert.That(expectedTotalCost.AreFloatsEqual(actualTotalCost), Is.True, $"Total of the order is wrong. Expected: {expectedTotalCost}, Actual: {actualTotalCost}.");
        }


        #region Helpers

        private List<Order> CreateOrders(IEnumerable<OrderTableData> orderTableData)
        {
            var orders = new List<Order>();

            foreach (var orderData in orderTableData)
            {
                var order = orders.FirstOrDefault(o => o.OrderNumber == orderData.OrderNumber);
                var itemOrdered = new OrderItem(orderData.MenuCategoryType, _config.Prices.GetPriceByCategory(orderData.MenuCategoryType), orderData.OrderTime);

                for (var i = 0; i < orderData.Quantity; i++)
                {
                    if (order != null)
                    {
                        order.ItemsOrdered.Add(itemOrdered);
                    }
                    else
                    {
                        order = new Order
                        {
                            OrderNumber = orderData.OrderNumber,
                            ItemsOrdered = new List<OrderItem> { itemOrdered }
                        };

                        orders.Add(order);
                    }
                }
            }

            Console.WriteLine($"The following orders have been created: {string.Join(", ", orders.Select(o => o.OrderNumber))}.");
            return orders;
        }

        private void UpdateOrders(List<OrderTableData> orderTableData)
        {
            var ordersNumbersToUpdate = orderTableData.Select(o => o.OrderNumber).Distinct().ToList();
            Assert.That(_baseTestHelper.TestDependentData.Orders.Select(o => o.OrderNumber).SequenceEqual(ordersNumbersToUpdate), Is.True,
                $"Existing orders list doesn't contain all of the orders supposed to update. Existing orders: {string.Join(", ", _baseTestHelper.TestDependentData.Orders.Select(o => o.OrderNumber))}. Orders supposed to update: {string.Join(", ", ordersNumbersToUpdate)}");

            var ordersToUpdate = _baseTestHelper.TestDependentData.Orders.Where(o => ordersNumbersToUpdate.Contains(o.OrderNumber)).ToList();

            foreach (var orderData in orderTableData)
            {
                var order = ordersToUpdate.Single(o => o.OrderNumber == orderData.OrderNumber);
                var itemOrdered = new OrderItem(orderData.MenuCategoryType, _config.Prices.GetPriceByCategory(orderData.MenuCategoryType), orderData.OrderTime);

                if (orderData.Quantity > 0)
                {
                    for (var i = 0; i < orderData.Quantity; i++)
                    {
                        order.ItemsOrdered.Add(itemOrdered);
                    }
                }
                else
                {
                    var itemsCancelled = order.ItemsOrdered
                        .Where(i => i.ItemCategory == itemOrdered.ItemCategory)
                        .Take(Math.Abs(orderData.Quantity))
                        .ToList();

                    foreach (var item in itemsCancelled)
                    {
                        order.ItemsOrdered.Remove(item);
                    }
                }
            }

            Console.WriteLine($"The following orders have been updated: {string.Join(", ", ordersNumbersToUpdate)}.");
        }

        private float CalculateTotalOfOrder(Order order)
        {
            var totalCost = 0.0f;
            var itemsByCategory = order.ItemsOrdered.GroupBy(i => i.ItemCategory).ToDictionary(g => g.Key, g => g.Select(i => i).ToList());

            foreach (var menuCategory in itemsByCategory)
            {
                var categoryCost = menuCategory.Value.Sum(i => i.Price);

                // apply possible cost adjustments
                switch (menuCategory.Key)
                {
                    case MenuCategoryType.Starter:
                    case MenuCategoryType.Main:
                        categoryCost += categoryCost * ((float)_config.ServiceChargePercentage / 100);
                        break;
                    case MenuCategoryType.Drink:
                        // considering orders without time specified as no discount applicable
                        var drinksWithDiscount = menuCategory.Value.Where(i =>
                            i.OrderTime != null &&
                            i.OrderTime >= _config.RestaurantOpeningTime &&
                            i.OrderTime < _config.DrinksDiscountEndTime).ToList();

                        categoryCost -= drinksWithDiscount.Sum(i => i.Price) * ((float)_config.DiscountForDrinksPercentage / 100);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid {nameof(MenuCategoryType)} specified: {menuCategory.Key}. " +
                                                              $"Currently supported values are: {nameof(MenuCategoryType.Starter)}, {nameof(MenuCategoryType.Main)} and {nameof(MenuCategoryType.Drink)}.");
                }

                totalCost += categoryCost;
            }

            Console.WriteLine($"Total of the order {order.OrderNumber} is £{totalCost}.");
            return totalCost;
        }

        #endregion
    }
}