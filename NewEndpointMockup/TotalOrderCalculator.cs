using Utils.DataModels;
using Utils.Enums;

namespace NewEndpointMockup
{
    public class TotalOrderCalculator
    {
        private const float ServiceChargePercentage = 10.00f;
        private const float DiscountForDrinksPercentage = 30.00f;
        private static readonly TimeSpan DrinksDiscountEndTime = new(19, 0, 0);
        private static readonly TimeSpan RestaurantOpeningTime = new(9, 0, 0);

        public static float CalculateTotalOfOrder(Order order)
        {
            var totalCost = 0.0f;
            var itemsByCategory = order.ItemsOrdered.GroupBy(i => i.ItemCategory).ToDictionary(g => g.Key, g => g.Select(i => i).ToList());

            foreach (var menuCategory in itemsByCategory)
            {
                var categoryCost = menuCategory.Value.Sum(i => i.Price);

                // apply possible cost adjustments for each MenuCategoryType
                switch (menuCategory.Key)
                {
                    case MenuCategoryType.Starter:
                    case MenuCategoryType.Main:
                        categoryCost += categoryCost * (ServiceChargePercentage / 100);
                        break;
                    case MenuCategoryType.Drink:
                        // considering orders without time specified as no discount applicable
                        var drinksWithDiscount = menuCategory.Value.Where(i =>
                            i.OrderTime != null &&
                            i.OrderTime >= RestaurantOpeningTime &&
                            i.OrderTime < DrinksDiscountEndTime).ToList();

                        categoryCost -= drinksWithDiscount.Sum(i => i.Price) * (DiscountForDrinksPercentage / 100);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid {nameof(MenuCategoryType)} specified: {menuCategory.Key}. " +
                                                              $"Currently supported values are: {nameof(MenuCategoryType.Starter)}, {nameof(MenuCategoryType.Main)} and {nameof(MenuCategoryType.Drink)}.");
                }

                totalCost += categoryCost;
            }

            return totalCost;
        }
    }
}