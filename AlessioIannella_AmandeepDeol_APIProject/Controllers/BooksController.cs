using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using AlessioIannella_AmandeepDeol.API.Models.Responses;
using System.Data;
using AlessioIannella_AmandeepDeol.API.Models.Requests;
using Microsoft.AspNetCore.Http;
using System.IO;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using MySql.Data.MySqlClient;
using AlessioIannella_AmandeepDeol.API.Models.Context;
using AlessioIannella_AmandeepDeol.API.Models;
using AlessioIannella_AmandeepDeol.API.Models.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlessioIannella_AmandeepDeol_APIProject.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private BooksContext Context
        {
            get
            {
                return HttpContext.RequestServices.GetService(typeof(BooksContext)) as BooksContext;
            }
        }

        private IHostingEnvironment _hostingEnvironment { get; set; }

        public BooksController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET" api/books
        // Get all books
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                BooksContext context = HttpContext.RequestServices.GetService(typeof(BooksContext)) as BooksContext;

                List<Book> list = await context.GetAllBooks();

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting list of moods from database"));
            }
           
        }

        // GET: api/books/{id}
        // Get book by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Book book = await Context.GetBookByID(id);

                if (book == null)
                {
                    return NotFound(new DataNotFoundResponse("Book not found in database"));
                }

                return Ok(book);
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting book from database"));
            }
        }

        // GET: api/books/isbn/{isbn}
        // Get book by id
        [HttpGet("isbn/{isbn}")]
        public async Task<IActionResult> Get(string isbn)
        {
            try
            {
                Book book = await Context.GetBookByISBN(isbn);

                if (book == null)
                {
                    return NotFound(new DataNotFoundResponse("Book not found in database"));
                }

                return Ok(book);
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting book from database"));
            }
        }

        // POST api/books
        // Save new book into database
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            if (book.Title == null || book.Title == "")
            {
                return BadRequest(new BadRequestResponse("Request body is incomplete"));
            }
            if (book.Author == null || book.Author == "")
            {
                return BadRequest(new BadRequestResponse("Request body is incomplete"));
            }
            if (book.ISBN == null || book.ISBN == "")
            {
                return BadRequest(new BadRequestResponse("Request body is incomplete"));
            }
            if (book.PublishedDate == null || book.PublishedDate == "")
            {
                return BadRequest(new BadRequestResponse("Request body is incomplete"));
            }

            book.UploadDate = DateTime.Now.ToString();

            try
            {
                Book result = await Context.GetBookByISBN(book.ISBN);

                if (result != null)
                {
                    return BadRequest(new BadRequestResponse("Book already exists"));
                }

                int bookID = await Context.SaveBook(book);

                result = await Context.GetBookByID(bookID);

                if (result == null)
                {
                    return BadRequest(new BadRequestResponse("Book saved into database but not available"));
                }

                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse(e.Message));
            }

        }

        // GET /books/mood/{id}
        // Get all books by mood id
        [HttpGet("mood/{moodID}")]
        public async Task<IActionResult> GetByMood(int moodID)
        {

            try
            {
                List<Book> books = await Context.GetBooksByMoodID(moodID);

                if (books.Count == 0)
                {
                    return NotFound(new DataNotFoundResponse("No books found in database for that mood"));
                }

                return Ok(books);

            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting list of books from database"));
            }
        }

        // POST /books/search
        // Get books by search value
        [HttpPost("search")]
        public async Task<IActionResult> GetBySearch([FromBody] Search search)
        {

            try
            {
                List<Book> books = await Context.GetBooksBySearchValue(search.Value);

                if (books.Count == 0)
                {
                    return NotFound(new DataNotFoundResponse("Books not found in database"));
                }

                return Ok(books);

            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting list of books from database"));
            }
        }

        // GET /books/{id}/recommended
        // Get recommended books for user id.
        // Recommended are based on most mood searched by user and on the most popular books of that mood
        [HttpGet("{userID}/recommended")]
        public async Task<IActionResult> GetRecommendedBooks(int userID)
        {

            try
            {
                List<Book> books = await Context.GetRecommendedBooks(userID);

                if (books.Count == 0)
                {
                    return NotFound(new DataNotFoundResponse("Books not found in database"));
                }

                return Ok(books);

            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting list of books from database"));
            }
        }

        // GET /books/popular
        // Get most popular books
        // Get first the most popular moods, then it gets the books associated with those moods
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularBooks()
        {
            try
            {
                List<Book> books = await Context.GetPopularBooks();

                if (books.Count == 0)
                {
                    return NotFound(new DataNotFoundResponse("Books not found in database"));
                }

                return Ok(books);

            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error getting list of books from database"));
            }
        }

        // PUT api/books/{id}
        // Update book by id
        [HttpPut("{bookID}")]
        public async Task<IActionResult> Put(int bookID, [FromBody] Book book)
        {
            

            try
            {
                Book result = await Context.GetBookByID(bookID);

                if (result == null)
                {
                    return NotFound(new DataNotFoundResponse("Book not found in database"));
                }

                if (book.Title != null && book.Title != "")
                {
                    result.Title = book.Title;
                }
                if (book.Author != null && book.Author != "")
                {
                    result.Author = book.Author;
                }
                if (book.ImageURL != null && book.ImageURL != "")
                {
                    result.ImageURL = book.ImageURL;
                }
                if (book.DownloadURL != null && book.DownloadURL != "")
                {
                    result.DownloadURL = book.DownloadURL;
                }
                if (book.PublishedDate != null && book.PublishedDate != "")
                {
                    result.PublishedDate = book.PublishedDate;
                }

                int resultID = await Context.UpdateBook(bookID, result);

                result = await Context.GetBookByID(bookID);

                if (result == null)
                {
                    return BadRequest(new BadRequestResponse("Book saved in database but not available"));
                }

                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error updating book"));
            }

        }

        // DELETE api/books/5
        // Delete book by id
        [HttpDelete("{bookID}")]
        public async Task<IActionResult> Delete(int bookID)
        {
            try
            {
                Book result = await Context.GetBookByID(bookID);

                if (result == null)
                {
                    return NotFound(new DataNotFoundResponse("Book not found in database"));
                }

                Context.DeleteBook(bookID);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error deleting book"));
            }
        }

        // POST /books/{userID}/{bookID}/image
        // Post new pdf file creating folder by id
        [HttpPost("{userID}/{bookID}/image")]
        public async Task<IActionResult> PostBookImage(int userID, int bookID, IFormFile image)
        {

            if (image == null)
            {
                return BadRequest(new BadRequestResponse("Body is not in the correct format"));
            }

            // Instantiates a client.
            StorageClient storageClient = StorageClient.Create();

            try
            {
                // Creates the new bucket if does not exist.
                var bucket = await storageClient.GetBucketAsync("api_project_books");

                if (bucket == null)
                {
                    CreateBucketOptions createBucketOptions = new CreateBucketOptions();
                    createBucketOptions.Projection = Projection.Full;

                    await storageClient.CreateBucketAsync("assignment1-179919", "api_project_books", createBucketOptions);
                }
            }
            catch (Google.GoogleApiException e) when (e.Error.Code == 409)
            {
                // The bucket already exists. That's fine.
                Console.WriteLine(e.Error.Message);
            }

            MemoryStream ms = new MemoryStream();

            var fileName = Guid.NewGuid().ToString();

            var fileType = image.FileName.Substring(image.FileName.LastIndexOf("."));

            fileName += fileType;

            if (image.Length == 0)
            {
                return BadRequest(new BadRequestResponse("file cannot be empty"));
            }

            await image.CopyToAsync(ms);

            var result = storageClient.UploadObjectAsync("api_project_books", userID + "/" + bookID + "/" + fileName, image.ContentType, ms);

            if (result == null)
            {
                try
                {
                    Book book = await Context.GetBookByID(bookID);

                    if (result == null)
                    {
                        return NotFound(new DataNotFoundResponse("Book not found in database"));
                    }

                    Context.DeleteBook(bookID);

                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(new BadRequestResponse("Error deleting book"));
                }
            }

            return Ok(new { imageURL = "https://storage.googleapis.com/api_project_books/" + userID + "/" + bookID + "/" + fileName });
        }

        // POST /books/{userID}/{bookID}/pdf
        // Post new pdf file creating folder by id
        [HttpPost("{userID}/{bookID}/pdf")]
        public async Task<IActionResult> PostBookPdf(int userID, int bookID, IFormFile pdf)
        {

            if (pdf == null)
            {
                return BadRequest(new BadRequestResponse("Body is not in the correct format"));
            }

            // Instantiates a client.
            StorageClient storageClient = StorageClient.Create();

            try
            {
                // Creates the new bucket if does not exist.
                var bucket = await storageClient.GetBucketAsync("api_project_books");

                if (bucket == null)
                {
                    CreateBucketOptions createBucketOptions = new CreateBucketOptions();
                    createBucketOptions.Projection = Projection.Full;

                    storageClient.CreateBucket("assignment1-179919", "api_project_books");
                }
            }
            catch (Google.GoogleApiException e) when (e.Error.Code == 409)
            {
                // The bucket already exists. That's fine.
                Console.WriteLine(e.Error.Message);
            }

            MemoryStream ms = new MemoryStream();

            var fileName = Guid.NewGuid().ToString();

            var fileType = pdf.FileName.Substring(pdf.FileName.LastIndexOf("."));

            fileName += fileType;

            if (pdf.Length == 0)
            {
                return BadRequest(new BadRequestResponse("file cannot be empty"));
            }

            await pdf.CopyToAsync(ms);

            var result = await storageClient.UploadObjectAsync("api_project_books", userID + "/" + bookID + "/" + fileName, pdf.ContentType, ms);

            if (result == null)
            {
                try
                {
                    Book book = await Context.GetBookByID(bookID);

                    if (result == null)
                    {
                        return NotFound(new DataNotFoundResponse("Book not found in database"));
                    }

                    Context.DeleteBook(bookID);

                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(new BadRequestResponse("Error deleting book"));
                }
            }

            return Ok(new { downloadURL = "https://storage.googleapis.com/api_project_books/" + userID + "/" + bookID + "/" + fileName });
        }

        // POST /bookUserMood
        // Set mood for book by user
        [HttpPost("bookUserMood")]
        public async Task<IActionResult> PostBookUserMood([FromBody] BookUserMood bookUserMood)
        {
            try
            {
                BookUserMood result = await Context.SaveBookUserMood(bookUserMood);

                if (result == null)
                {
                    return BadRequest(new BadRequestResponse("Error saving book for mood in database"));
                }

                return Ok(result);

            }
            catch (BookNotFoundException e)
            {
                return BadRequest(new BadRequestResponse(e.Message));
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(new BadRequestResponse(e.Message));
            }
            catch (MoodNotFoundException e)
            {
                return BadRequest(new BadRequestResponse(e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(new BadRequestResponse("Error saving book for mood in database"));
            }

        }

    }

}
