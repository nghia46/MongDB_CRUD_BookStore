using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongDB_Test_API_BookStore.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Model;

namespace MongDB_Test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IMongoCollection<Book> _bookCollection;
        public BookController(IMongoClient client)
        {
            var database = client.GetDatabase("BookStoreDB");
            _bookCollection = database.GetCollection<Book>("Books");
        }
        [HttpGet("GetAllBook")]
        public IActionResult getAll()
        {
            List<Book> books = _bookCollection.Find(_ => true).ToList();
            var sortedBooks = books.OrderByDescending(b => b.Price).ToList();
            return Ok(sortedBooks);
        }
        [HttpGet("GetBookByID/{id}")]
        public IActionResult GetById(Guid id)
        {
            var filter = Builders<Book>.Filter.Eq("_id", id);
            Object book = _bookCollection.Find(filter).FirstOrDefault();

            if (book == null)
            {
                return NotFound("Book not found.");
            }
            return Ok(book);
        }
        [HttpGet("SearchByNameKeyWord")]
        public IActionResult SearchByName(string keyword)
        {
            var filter = Builders<Book>.Filter.Regex("Name", new BsonRegularExpression(keyword, "i")); // Case-insensitive regex match on the "Name" field
            List<Book> books = _bookCollection.Find(filter).ToList();

            if (books.Count == 0)
            {
                return NotFound("No books found matching the search keyword.");
            }
            return Ok(books);
        }
        [HttpGet("SearhBookByAuthor")]
        public IActionResult searchByAuthor(string author)
        {
            var filter = Builders<Book>.Filter.Eq("Author", author);
            var books = _bookCollection.Find(filter).ToList();
            if (books.Count == 0)
            {
                return NotFound("No book found matching the search author");
            }
            return Ok(books);
        }
        [HttpPost("AddNewBook")]
        public IActionResult Add(BookViewModel bookview)
        {
            Book b = new Book
            {
                Book_ID = new Guid(),
                Name = bookview.Name,
                Author = bookview.Author,
                Price = bookview.Price,
            };
            _bookCollection.InsertOne(b);
            return Ok("Book added successfully.");
        }
        [HttpPost("Add many")]
        public IActionResult AddMany(List<BookViewModel> bookList)
        {
            List<Book> bookLists = new List<Book>();
            foreach (var item in bookList)
            {
                bookLists.Add(new Book
                {
                    Book_ID = Guid.NewGuid(),
                    Author = item.Author,
                    Name = item.Name,
                    Price = item.Price
                });
            }
            _bookCollection.InsertMany(bookLists);
            return Ok("Books added!");
        }
        [HttpPut("UpdateByID/{id}")]
        public IActionResult UpdateById(Guid id, BookViewModel book)
        {
            var filter = Builders<Book>.Filter.Eq("_id", id);
            var update = Builders<Book>.Update
                .Set("Name", book.Name)
                .Set("Author", book.Author)
                .Set("Price", book.Price);

            var options = new UpdateOptions { IsUpsert = false };

            var result = _bookCollection.UpdateOne(filter, update, options);

            if (result.ModifiedCount == 0)
            {
                return NotFound("Book not found.");
            }
            return Ok("Book updated successfully.");
        }
        [HttpDelete("DelelteByID/{id}")]
        public IActionResult DeleteById(Guid id)
        {
            var filter = Builders<Book>.Filter.Eq("_id", id);
            var result = _bookCollection.DeleteOne(filter);

            if (result.DeletedCount == 0)
            {
                return NotFound("Book not found.");
            }
            return Ok("Book deleted successfully.");
        }
    }
}
