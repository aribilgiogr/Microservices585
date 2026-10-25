using BookStore.API.Models;

namespace BookStore.API.Services
{
    public interface IBooksService
    {
        Task<IEnumerable<Book>> GetAsync();
        Task<Book> GetAsync(string id);
        Task CreateAsync(Book newBook);
        Task UpdateAsync(string id, Book updatedBook);
        Task RemoveAsync(string id);
    }
}
