using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Practice.Databases;
using Practice.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<Book> SearchBooksByAuthor(string author)
        {
            return _dbContext.Books
                .Where(book => book.Author.Contains(author))
                .ToList();
        }

        public List<Book> SearchBooksByGenre(string genre)
        {
            return _dbContext.Books
                .Where(book => book.Genre.Contains(genre))
                .ToList();
        }

        public void UpdateBook(Book existingBook)
        {
            if (existingBook == null)
            {
                throw new ArgumentNullException(nameof(existingBook));
            }

            var bookInDb = _dbContext.Books.Find(existingBook.BookId);

            if (bookInDb != null)
            {
                // Update properties of the existing book
                bookInDb.Title = existingBook.Title;
                bookInDb.Author = existingBook.Author;
                bookInDb.Genre = existingBook.Genre;
                bookInDb.ISBN = existingBook.ISBN;
                bookInDb.PublishDate = existingBook.PublishDate;

                // Save changes to the database
                _dbContext.SaveChanges();
            }
        }
    }
}
