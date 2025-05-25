namespace Utils.Configuration
{
    public class RestaurantConfiguration
    {
        public int ServiceChargePercentage { get; set; }
        public Prices Prices { get; set; }
        public int DiscountForDrinksPercentage { get; set; }
        public TimeSpan DrinksDiscountEndTime { get; set; }
        public TimeSpan RestaurantOpeningTime { get; set; }
    }
}