using Dapper;
using DapperAdvancedAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperAdvancedAPI.Repositories
{
    public interface IBookRepository
    {
        Task AddBooks(IEnumerable<Book> books);
        Task<IEnumerable<Book>> GetBooks();
        Task<(string, string)> GetBookDetail(Guid id);
        Task<(IEnumerable<Genre>, IEnumerable<Book>)> GetMultiple();
        Task<IEnumerable<Book>> GetBooksWithGenres();
    }

    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _config;
        public BookRepository(IConfiguration config)
        {
            _config = config;
        }

        //1.

        public async Task AddBooks(IEnumerable<Book> books)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var parameters = new DynamicParameters();
            parameters.Add("@typBook", ToDataTable(books), DbType.Object,
                ParameterDirection.Input);
            await connection.ExecuteAsync("sp_AddBooks", parameters, commandType:
                CommandType.StoredProcedure);
        }

        private DataTable ToDataTable(IEnumerable<Book> books)
        {
            var table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("Year", typeof(string));
            foreach (var book in books)
            {
                table.Rows.Add(book.Title, book.Author, book.Year);
            }
            return table;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var books = await connection.QueryAsync<Book>("select * from dbo.book");
            return books;
        }

        //2.
        public async Task<(string,string)> GetBookDetail(Guid id)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Guid);
            parameters.Add("@Title", dbType: DbType.String, direction: ParameterDirection.Output,size:100);
            parameters.Add("@Author", dbType: DbType.String, direction: ParameterDirection.Output,size:100);
            
            await connection.QueryAsync("uspGetBookDetail", parameters, commandType: CommandType.StoredProcedure);
            var title = parameters.Get<string>("@Title");
            var author = parameters.Get<string>("@Author");
            return (title, author);
        }

        //3.
        public async Task<(IEnumerable<Genre>, IEnumerable<Book>)> GetMultiple()
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            string query = @"select * from genre
                             select * from book";
            using SqlMapper.GridReader multi = await connection.QueryMultipleAsync(query, commandType: CommandType.Text);
            var genres = multi.Read<Genre>().ToList();
            var books = multi.Read<Book>().ToList();
            return (genres, books);
        }
        
        //5.
        public async Task<IEnumerable<Book>> GetBooksWithGenres()
        {
            string query = @"select b.Id, b.Title, b.Author, b.Year, g.Id As GenreId, g.Name As GenreName
                            From Book b
                            Inner Join Book_Genre bg on b.Id = bg.BookId
                            Inner Join Genre g on bg.GenreId = g.Id";

            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var booksDictionary = new Dictionary<Guid, Book>();
            await connection.QueryAsync<Book, GenreVm, Book>(
                sql: query,
                map: (book, genre) =>
                {
                    if(!booksDictionary.TryGetValue(book.Id, out var bookEntry))
                    {
                        bookEntry = book;
                        bookEntry.Genres = new List<GenreVm>();
                        booksDictionary.Add(bookEntry.Id, bookEntry);
                    }
                    bookEntry?.Genres.Add(genre);
                    return bookEntry;
                }, splitOn: "GenreId");
            return booksDictionary.Values;
        }
    }
}
