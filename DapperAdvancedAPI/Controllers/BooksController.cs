﻿using DapperAdvancedAPI.Models;
using DapperAdvancedAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace DapperAdvancedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        //1.
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _bookRepository.GetBooks();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Something went wrong!");
                //return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(IEnumerable<Book> books)
        {
            try
            {
                await _bookRepository.AddBooks(books);
                return CreatedAtAction(nameof(AddBook), books);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Oops! something went wrong");
            }
        }

        //2.
        [HttpGet("/book-detail/{id}")]
        public async Task<IActionResult> GetBookDetail(Guid id)
        {
            try
            {
                var (title,author) = await _bookRepository.GetBookDetail(id);
                return Ok(new { title,author });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Oops! something went wrong");
            }
        }

        [HttpGet("/multi")]
        public async Task<IActionResult> GetMultipleDs()
        {
            try
            {
                var (genres, books) = await _bookRepository.GetMultiple();
                return Ok(new { genres, books });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Oops! something went wrong");

            }
        }

        //5.
        [HttpGet("/books-with-genre")]
        public async Task<IActionResult> GetBooksWithGenres()
        {
            try
            {
                var books = await _bookRepository.GetBooksWithGenres();
                return Ok(books);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Oops! something went wrong");
            }
        }
    }
}
