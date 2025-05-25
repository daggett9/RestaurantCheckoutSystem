namespace Utils.DataModels
{
    public class Order
    {
        public string OrderNumber { get; set; }
        public List<OrderItem> ItemsOrdered { get; set; }
    }
}