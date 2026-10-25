using BookStore.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStore.API.Services
{
    public class BookService(IOptions<BookStoreDatabaseSettings> options) : IBooksService
    {
        private readonly IMongoCollection<Book> booksCollection = new MongoClient(options.Value.ConnectionString)
            .GetDatabase(options.Value.DatabaseName)
            .GetCollection<Book>(options.Value.BooksCollectionName);
        public async Task CreateAsync(Book newBook) => await booksCollection.InsertOneAsync(newBook);

        public async Task<IEnumerable<Book>> GetAsync() => await booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book> GetAsync(string id) => await booksCollection.Find(b => b.Id == id).FirstOrDefaultAsync();

        public async Task RemoveAsync(string id) => await booksCollection.DeleteOneAsync(b => b.Id == id);

        public async Task UpdateAsync(string id, Book updatedBook) => await booksCollection.ReplaceOneAsync(b => b.Id == id, updatedBook);
    }
}
