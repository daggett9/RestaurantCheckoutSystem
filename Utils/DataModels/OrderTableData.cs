using Utils.Enums;

namespace Utils.DataModels
{
    public class OrderTableData
    {
        public string OrderNumber { get; set; }
        public MenuCategoryType MenuCategoryType { get; set; }
        public int Quantity { get; set; }
        public TimeSpan? OrderTime { get; set; }
    }
}