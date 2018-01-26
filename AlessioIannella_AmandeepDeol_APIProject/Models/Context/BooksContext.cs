using AlessioIannella_AmandeepDeol.API.Models.Exceptions;
using AlessioIannella_AmandeepDeol.API.Models.Requests;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Context
{
    public class BooksContext
    {
        public string ConnectionString { get; set; }

        public BooksContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> list = new List<Book>();

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("select * from Book", connection);

            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Book book = new Book()
                {
                    BookID = Convert.ToInt32(reader["bookID"]),
                    ISBN = reader["isbn"].ToString(),
                    Title = reader["title"].ToString(),
                    Author = reader["author"].ToString(),
                    ImageURL = reader["imageURL"].ToString(),
                    DownloadURL = reader["downloadURL"].ToString(),
                    PublishedDate = reader["publishedDate"].ToString(),
                    UploadDate = reader["uploadDate"].ToString()
                };

                list.Add(book);
            }

            reader.Close();

            await connection.CloseAsync();

            return list;
        }

        public async Task<Book> GetBookByID(int bookID)
        {
            Book book = null;

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Book WHERE bookID = @bookID", connection);
            cmd.Parameters.AddWithValue("@bookID", bookID);

            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                book = new Book()
                {
                    BookID = Convert.ToInt32(reader["bookID"]),
                    ISBN = reader["isbn"].ToString(),
                    Title = reader["title"].ToString(),
                    Author = reader["author"].ToString(),
                    ImageURL = reader["imageURL"].ToString(),
                    DownloadURL = reader["downloadURL"].ToString(),
                    PublishedDate = reader["publishedDate"].ToString(),
                    UploadDate = reader["uploadDate"].ToString()
                };
            }

            reader.Close();

            await connection.CloseAsync();

            return book;
        }

        public async Task<Book> GetBookByISBN(string isbn)
        {
            Book book = null;

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Book WHERE isbn = @isbn", connection);
            cmd.Parameters.AddWithValue("@isbn", isbn);

            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                book = new Book()
                {
                    BookID = Convert.ToInt32(reader["bookID"]),
                    ISBN = reader["isbn"].ToString(),
                    Title = reader["title"].ToString(),
                    Author = reader["author"].ToString(),
                    ImageURL = reader["imageURL"].ToString(),
                    DownloadURL = reader["downloadURL"].ToString(),
                    PublishedDate = reader["publishedDate"].ToString(),
                    UploadDate = reader["uploadDate"].ToString()
                };
            }

            reader.Close();

            await connection.CloseAsync();

            return book;
        }

        public async Task<int> SaveBook(Book book)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            DateTime publishedDate = DateTime.Parse(book.PublishedDate);
            DateTime uploadDate = DateTime.Parse(book.UploadDate);

            MySqlCommand myCommand = new MySqlCommand("INSERT INTO Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) VALUES (@isbn, @title, @author, @imageURL, @downloadURL, @publishedDate, @uploadDate)", connection);
            myCommand.Parameters.AddWithValue("@isbn", book.ISBN);
            myCommand.Parameters.AddWithValue("@title", book.Title);
            myCommand.Parameters.AddWithValue("@author", book.Author);
            myCommand.Parameters.AddWithValue("@imageURL", book.ImageURL);
            myCommand.Parameters.AddWithValue("@downloadURL", book.DownloadURL);
            myCommand.Parameters.AddWithValue("@publishedDate", publishedDate);
            myCommand.Parameters.AddWithValue("@uploadDate", uploadDate);

            var result = await myCommand.ExecuteScalarAsync();

            int bookID = -1;

            if (result == null)
            {
                bookID = Convert.ToInt32(myCommand.LastInsertedId);
            }
            else
            {
                bookID = Convert.ToInt32(result);
            }

            await connection.CloseAsync();

            return bookID;
        }

        public async Task<List<Book>> GetBooksByMoodID(int moodID)
        {
            List<Book> list = new List<Book>();

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT* FROM Book WHERE Book.bookID IN(SELECT bookID FROM BookUserMood WHERE moodID = @moodID)", connection);
            cmd.Parameters.AddWithValue("@moodID", moodID);

            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Book book = new Book()
                {
                    BookID = Convert.ToInt32(reader["bookID"]),
                    ISBN = reader["isbn"].ToString(),
                    Title = reader["title"].ToString(),
                    Author = reader["author"].ToString(),
                    ImageURL = reader["imageURL"].ToString(),
                    DownloadURL = reader["downloadURL"].ToString(),
                    PublishedDate = reader["publishedDate"].ToString(),
                    UploadDate = reader["uploadDate"].ToString()
                };

                list.Add(book);
            }

            reader.Close();

            await connection.CloseAsync();

            return list;
        }

        public async Task<List<Book>> GetBooksBySearchValue(string value)
        {
            List<Book> list = new List<Book>();

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Book WHERE Book.isbn LIKE @value OR Book.title LIKE @value OR Book.author LIKE @value", connection);
            cmd.Parameters.AddWithValue("@value", "%" + value + "%");

            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Book book = new Book()
                {
                    BookID = Convert.ToInt32(reader["bookID"]),
                    ISBN = reader["isbn"].ToString(),
                    Title = reader["title"].ToString(),
                    Author = reader["author"].ToString(),
                    ImageURL = reader["imageURL"].ToString(),
                    DownloadURL = reader["downloadURL"].ToString(),
                    PublishedDate = reader["publishedDate"].ToString(),
                    UploadDate = reader["uploadDate"].ToString()
                };

                list.Add(book);
            }

            reader.Close();

            await connection.CloseAsync();

            return list;
        }

        public async Task<List<Book>> GetRecommendedBooks(int userID)
        {
            List<Book> list = new List<Book>();

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Book WHERE bookID IN (SELECT MostSearchedBooks.bookID FROM (SELECT bookID, count(bookID) as searchedTimes FROM BookUserMood WHERE moodID IN (SELECT MostSearchedMoods.moodID FROM (SELECT moodID, COUNT(moodID) as searchedTimes FROM BookUserMood WHERE userID = @userID GROUP BY moodID LIMIT 3) as MostSearchedMoods) GROUP BY bookID ORDER BY searchedTimes DESC LIMIT 10) as MostSearchedBooks)", connection);

            //MySqlCommand cmd = new MySqlCommand("SELECT * FROM Book WHERE bookID IN (SELECT DISTINCT bookID FROM BookUserMood WHERE moodID IN (SELECT MostSearchedMoods.moodID FROM(SELECT COUNT(moodID) as searchedTimes, moodID FROM BookUserMood WHERE userID = @userID GROUP BY moodID LIMIT 3) as MostSearchedMoods));", connection);
            cmd.Parameters.AddWithValue("@userID", userID);

            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Book book = new Book()
                {
                    BookID = Convert.ToInt32(reader["bookID"]),
                    ISBN = reader["isbn"].ToString(),
                    Title = reader["title"].ToString(),
                    Author = reader["author"].ToString(),
                    ImageURL = reader["imageURL"].ToString(),
                    DownloadURL = reader["downloadURL"].ToString(),
                    PublishedDate = reader["publishedDate"].ToString(),
                    UploadDate = reader["uploadDate"].ToString()
                };

                list.Add(book);
            }

            reader.Close();

            await connection.CloseAsync();

            return list;
        }

        public async Task<List<Book>> GetPopularBooks()
        {
            List<Book> list = new List<Book>();

            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Book WHERE bookID IN (SELECT MostSearchedBooks.bookID FROM (SELECT bookID, count(bookID) as searchedTimes FROM BookUserMood WHERE moodID IN (SELECT MostSearchedMoods.moodID FROM (SELECT COUNT(moodID) as searchedTimes, moodID FROM BookUserMood GROUP BY moodID ORDER BY searchedTimes DESC LIMIT 3) as MostSearchedMoods) GROUP BY bookID ORDER BY searchedTimes DESC LIMIT 10) as MostSearchedBooks)", connection);

            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Book book = new Book()
                {
                    BookID = Convert.ToInt32(reader["bookID"]),
                    ISBN = reader["isbn"].ToString(),
                    Title = reader["title"].ToString(),
                    Author = reader["author"].ToString(),
                    ImageURL = reader["imageURL"].ToString(),
                    DownloadURL = reader["downloadURL"].ToString(),
                    PublishedDate = reader["publishedDate"].ToString(),
                    UploadDate = reader["uploadDate"].ToString()
                };

                list.Add(book);
            }

            reader.Close();

            await connection.CloseAsync();

            return list;
        }

        public async Task<int> UpdateBook(int bookID, Book book)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            DateTime publishedDate = DateTime.Parse(book.PublishedDate);

            MySqlCommand myCommand = new MySqlCommand("UPDATE Book SET title = @title, author = @author, imageURL = @imageURL, downloadURL = @downloadURL, publishedDate = @publishedDate WHERE bookID = @bookID", connection);
            myCommand.Parameters.AddWithValue("@title", book.Title);
            myCommand.Parameters.AddWithValue("@author", book.Author);
            myCommand.Parameters.AddWithValue("@imageURL", book.ImageURL);
            myCommand.Parameters.AddWithValue("@downloadURL", book.DownloadURL);
            myCommand.Parameters.AddWithValue("@publishedDate", publishedDate);
            myCommand.Parameters.AddWithValue("@bookID", bookID);

            await myCommand.ExecuteScalarAsync();

            await connection.CloseAsync();

            return bookID;
        }

        public async void DeleteBook(int bookID)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand myCommand = new MySqlCommand("DELETE FROM BookUserMood WHERE bookID = @bookID", connection);
            myCommand.Parameters.AddWithValue("@bookID", bookID);

            await myCommand.ExecuteNonQueryAsync();

            myCommand = new MySqlCommand("DELETE FROM Book WHERE bookID = @bookID", connection);
            myCommand.Parameters.AddWithValue("@bookID", bookID);

            await myCommand.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public async Task<BookUserMood> SaveBookUserMood(BookUserMood bookUserMood)
        {
            MySqlConnection connection = GetConnection();

            await connection.OpenAsync();

            MySqlCommand bookCommand = new MySqlCommand("SELECT * FROM Book WHERE bookID = @bookID", connection);
            bookCommand.Parameters.AddWithValue("@bookID", bookUserMood.BookID);

            var bookReader = await bookCommand.ExecuteReaderAsync();

            if (bookReader.HasRows)
            {
                bookReader.Close();

                await connection.CloseAsync();

                connection = GetConnection();

                await connection.OpenAsync();

                var userCommand = new MySqlCommand("SELECT * FROM DBUser WHERE userID = @userID", connection);
                userCommand.Parameters.AddWithValue("@userID", bookUserMood.UserID);

                var userReader = await userCommand.ExecuteReaderAsync();

                if (userReader.HasRows)
                {
                    userReader.Close();

                    await connection.CloseAsync();

                    connection = GetConnection();

                    await connection.OpenAsync();

                    var moodCommand = new MySqlCommand("SELECT * FROM Mood WHERE moodID = @moodID", connection);
                    moodCommand.Parameters.AddWithValue("@moodID", bookUserMood.MoodID);

                    var moodReader = await moodCommand.ExecuteReaderAsync();

                    if (moodReader.HasRows)
                    {
                        moodReader.Close();

                        await connection.CloseAsync();

                        connection = GetConnection();

                        await connection.OpenAsync();

                        var checkCommand = new MySqlCommand("SELECT* FROM BookUserMood WHERE bookID = @bookID AND userID = @userID", connection);
                        checkCommand.Parameters.AddWithValue("@bookID", bookUserMood.BookID);
                        checkCommand.Parameters.AddWithValue("@userID", bookUserMood.UserID);

                        var checkReader = await checkCommand.ExecuteReaderAsync();

                        if (checkReader.HasRows)
                        {
                            checkReader.Close();

                            await connection.CloseAsync();

                            connection = GetConnection();

                            await connection.OpenAsync();

                            var updateCommand = new MySqlCommand("UPDATE BookUserMood SET bookID = @bookID, userID = @userID, moodID = @moodID WHERE bookID = @bookID AND userID = @userID", connection);
                            updateCommand.Parameters.AddWithValue("@bookID", bookUserMood.BookID);
                            updateCommand.Parameters.AddWithValue("@userID", bookUserMood.UserID);
                            updateCommand.Parameters.AddWithValue("@moodID", bookUserMood.MoodID);

                            await updateCommand.ExecuteNonQueryAsync();
                        }
                        else
                        {
                            checkReader.Close();

                            await connection.CloseAsync();

                            connection = GetConnection();

                            await connection.OpenAsync();

                            var insertCommand = new MySqlCommand("INSERT INTO BookUserMood(bookID, userID, moodID) VALUES (@bookID, @userID, @moodID)", connection);
                            insertCommand.Parameters.AddWithValue("@bookID", bookUserMood.BookID);
                            insertCommand.Parameters.AddWithValue("@userID", bookUserMood.UserID);
                            insertCommand.Parameters.AddWithValue("@moodID", bookUserMood.MoodID);

                            await insertCommand.ExecuteNonQueryAsync();
                        }
                    }
                    else
                    {
                        moodReader.Close();
                        throw new MoodNotFoundException("Mood not found in database");
                    }
                }
                else
                {
                    userReader.Close();
                    throw new UserNotFoundException("User not found in database");
                }
            }
            else
            {
                bookReader.Close();
                throw new BookNotFoundException("Book not found in database");
            }

            await connection.CloseAsync();

            return bookUserMood;
        }
    }
}
