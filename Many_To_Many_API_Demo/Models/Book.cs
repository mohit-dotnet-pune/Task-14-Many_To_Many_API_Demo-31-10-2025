namespace Many_To_Many_API_Demo.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public ICollection<BookOrder>? bookOrders { get; set; } = new List<BookOrder>();    
    }
}
