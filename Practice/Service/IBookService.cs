using Practice.Entities;

namespace Practice.Service
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetBookById(int id);
        void AddBook(Book book);
      
        void DeleteBook(int id);
        void UpdateBook(Book existingBook);

        List<Book> SearchBooksByAuthor(string author);
        List<Book> SearchBooksByGenre(string genre);
    }
}
