using System.ComponentModel.DataAnnotations.Schema;

namespace Many_To_Many_API_Demo.Models
{
    public class BookOrder
    {
        
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        public int Quantity { get; set; }
    }
}
