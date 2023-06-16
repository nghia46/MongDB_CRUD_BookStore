using MongoDB.Bson.Serialization.Attributes;

namespace MongDB_Test_API_BookStore.ViewModel
{
    public class BookViewModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
    }
}
