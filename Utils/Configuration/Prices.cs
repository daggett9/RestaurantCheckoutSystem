using Utils.Enums;

namespace Utils.Configuration
{
    public class Prices
    {
        public float Starter { get; set; }
        public float Main { get; set; }
        public float Drink { get; set; }

        public float GetPriceByCategory(MenuCategoryType category)
        {
            return category switch
            {
                MenuCategoryType.Starter => Starter,
                MenuCategoryType.Main => Main,
                MenuCategoryType.Drink => Drink,
                _ => throw new ArgumentOutOfRangeException(
                    $"Invalid {nameof(MenuCategoryType)} specified: {category}. " +
                    $"Currently supported values are: {nameof(MenuCategoryType.Starter)}, {nameof(MenuCategoryType.Main)} and {nameof(MenuCategoryType.Drink)}.")
            };
        }
    }
}