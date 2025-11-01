namespace Many_To_Many_API_Demo.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<BookOrder>? bookOrders { get; set; } = new List<BookOrder>();
    }
}
