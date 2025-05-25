using Utils.Enums;

namespace Utils.DataModels
{
    public class OrderItem
    {
        public MenuCategoryType ItemCategory { get; set; }
        public float Price { get; set; }
        public TimeSpan? OrderTime { get; set; }

        public OrderItem(MenuCategoryType category, float price, TimeSpan? orderTime = null)
        {
            ItemCategory = category;
            Price = price;
            OrderTime = orderTime;
        }
    }
}