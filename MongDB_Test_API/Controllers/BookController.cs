using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            List<Book> books = _bookCollection.Find(_ => true).ToList(); // Retrieve all documents from the collection
            return Ok(books);
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
        [HttpPost("AddNewBook")]
        public IActionResult Add([FromBody] Book book)
        {
            book.Book_ID = Guid.NewGuid(); ;
            _bookCollection.InsertOne(book);
            return Ok("Book added successfully.");
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

        [HttpPut("UpdateByID/{id}")]
        public IActionResult UpdateById2(Guid id, [FromBody] Book updatedBook)
        {
            var filter = Builders<Book>.Filter.Eq("_id", id);

            var update = Builders<Book>.Update
                .Set("Name", updatedBook.Name)
                .Set("Author", updatedBook.Author)
                .Set("Price", updatedBook.Price);

            var options = new UpdateOptions { IsUpsert = false };

            var result = _bookCollection.UpdateOne(filter, update, options);

            if (result.ModifiedCount == 0)
            {
                return NotFound("Book not found.");
            }

            return Ok("Book updated successfully.");
        }

    }

}
