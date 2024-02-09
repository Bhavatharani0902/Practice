using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Practice.Databases;
using Practice.Entities;

namespace Practice.Service
{
    public class BookService : IBookService
    {
        private readonly MyContext _dbContext;

        public BookService(MyContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddBook(Book book)
        {
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var bookToDelete = _dbContext.Books.Find(id);

            if (bookToDelete != null)
            {
                _dbContext.Books.Remove(bookToDelete);
                _dbContext.SaveChanges();
            }
        }

        public List<Book> GetAllBooks()
        {
            return _dbContext.Books.ToList();
        }

        public Book GetBookById(int id)
        {
            return _dbContext.Books.Find(id);

        }

        public void UpdateBook(int id, Book updatedBook)
        {
            var existingBook = _dbContext.Books.Find(id);

            if (existingBook != null)
            {
                existingBook.Title = updatedBook.Title;
                existingBook.Author = updatedBook.Author;
                existingBook.Genre = updatedBook.Genre;
                existingBook.ISBN = updatedBook.ISBN;
                existingBook.PublishDate = updatedBook.PublishDate;

                _dbContext.SaveChanges();
            }
        }
    }
}


