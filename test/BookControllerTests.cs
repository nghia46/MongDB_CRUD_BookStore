using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Model;

namespace test
{
    public class BookControllerTests
    {
        private readonly BookController _bookController;
        private readonly Mock<IMongoCollection<Book>> _mockCollection;

        public BookControllerTests()
        {
            // Setup the mock IMongoCollection<Book> instance
            _mockCollection = new Mock<IMongoCollection<Book>>();

            // Pass the mock IMongoCollection<Book> to the BookController constructor
            _bookController = new BookController(_mockCollection.Object);
        }

        [Fact]
        public void GetAll_ReturnsOkResult()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Book_ID = Guid.NewGuid(), Name = "Book 1", Author = "Author 1", Price = 10.99 },
                new Book { Book_ID = Guid.NewGuid(), Name = "Book 2", Author = "Author 2", Price = 19.99 }
            };
            _mockCollection.Setup(c => c.Find(_ => true).ToList()).Returns(books);

            // Act
            var result = _bookController.getAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsType<List<Book>>(okResult.Value);
            Assert.Equal(books.Count, returnedBooks.Count);
        }

        // Similarly, you can write tests for other functions in the BookController class.
        // Remember to mock the required dependencies and verify the expected behavior and outcomes.

        // Example tests for other functions:
        // - GetById_ReturnsOkResult()
        // - SearchByName_ReturnsOkResult()
        // - searchByAuthor_ReturnsOkResult()
        // - Add_ReturnsOkResult()
        // - AddMany_ReturnsOkResult()
        // - UpdateById_ReturnsOkResult()
        // - DeleteById_ReturnsOkResult()
        // - ClearTrash_ReturnsOkResult()
    }
}
