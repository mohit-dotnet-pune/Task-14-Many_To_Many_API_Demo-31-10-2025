using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Many_To_Many_API_Demo.Models;

namespace Many_To_Many_API_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookOrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookOrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BookOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookOrder>>> GetBookOrders()
        {
            return await _context.Set<BookOrder>()
                .Include(bo => bo.Book)
                .Include(bo => bo.Order)
                .ToListAsync();
        }

        // GET: api/BookOrder/{bookId}/{orderId}
        [HttpGet("{bookId}/{orderId}")]
        public async Task<ActionResult<BookOrder>> GetBookOrder(int bookId, int orderId)
        {
            var bookOrder = await _context.Set<BookOrder>()
                .Include(bo => bo.Book)
                .Include(bo => bo.Order)
                .FirstOrDefaultAsync(bo => bo.BookId == bookId && bo.OrderId == orderId);

            if (bookOrder == null)
                return NotFound();

            return bookOrder;
        }

        // POST: api/BookOrder
        [HttpPost]
        public async Task<ActionResult<BookOrder>> CreateBookOrder([FromBody] BookOrder bookOrder)
        {
            var existingBook = await _context.Books.FindAsync(bookOrder.BookId);
            var existingOrder = await _context.Orders.FindAsync(bookOrder.OrderId);

            if (existingBook == null || existingOrder == null)
                return BadRequest("Invalid BookId or OrderId.");

            _context.Set<BookOrder>().Add(bookOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookOrder),
                new { bookId = bookOrder.BookId, orderId = bookOrder.OrderId },
                bookOrder);
        }

        // PUT: api/BookOrder/{bookId}/{orderId}
        [HttpPut("{bookId}/{orderId}")]
        public async Task<IActionResult> UpdateBookOrder(int bookId, int orderId, [FromBody] BookOrder updatedBookOrder)
        {
            if (bookId != updatedBookOrder.BookId || orderId != updatedBookOrder.OrderId)
                return BadRequest("Composite key mismatch.");

            var bookOrder = await _context.Set<BookOrder>().FindAsync(bookId, orderId);
            if (bookOrder == null)
                return NotFound();

            bookOrder.Quantity = updatedBookOrder.Quantity;
            _context.Entry(bookOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/BookOrder/{bookId}/{orderId}
        [HttpDelete("{bookId}/{orderId}")]
        public async Task<IActionResult> DeleteBookOrder(int bookId, int orderId)
        {
            var bookOrder = await _context.Set<BookOrder>().FindAsync(bookId, orderId);
            if (bookOrder == null)
                return NotFound();

            _context.Set<BookOrder>().Remove(bookOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
