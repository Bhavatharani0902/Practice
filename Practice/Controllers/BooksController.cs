using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Practice.DTOs; 
using Practice.Entities; 
using Practice.Service;
using log4net;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Practice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public BooksController(IBookService bookService, IMapper mapper, ILog logger = null)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetAllBooks()
        {
            try
            {
                var books = _bookService.GetAllBooks();
                var bookDTOs = _mapper.Map<List<BookDto>>(books);
                _logger?.Info("Retrieved all books successfully.");
                return StatusCode(200, bookDTOs);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error in GetAllBooks: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> GetBookById(int id)
        {
            try
            {
                var book = _bookService.GetBookById(id);

                if (book == null)
                {
                    _logger?.Warn($"Book with ID {id} not found.");
                    return StatusCode(404);
                }

                var bookDTO = _mapper.Map<BookDto>(book);
                _logger?.Info($"Retrieved book by ID successfully. Book ID: {id}");
                return StatusCode(200, bookDTO);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error in GetBookById: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        public ActionResult CreateBook([FromBody] BookDto bookDto)
        {
            try
            {
                var book = _mapper.Map<Book>(bookDto);
                _bookService.AddBook(book);
                _logger?.Info("Book created successfully.");
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error in CreateBook: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(int id)
        {
            var existingBook = _bookService.GetBookById(id);

            if (existingBook == null)
            {
                _logger?.Warn($"Book with ID {id} not found.");
                return StatusCode(404);
            }

            _bookService.DeleteBook(id);

            _logger?.Info($"Book deleted successfully. Book ID: {id}");
            return StatusCode(200);
        }

       
    }
}
