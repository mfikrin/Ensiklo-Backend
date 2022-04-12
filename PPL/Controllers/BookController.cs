using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PPL.Models;
using System;
using System.Data;
using System.Diagnostics;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: api/<BookController>
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
               SELECT * FROM books
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [HttpGet]
        [Route("limit/{limit}")]

        public JsonResult GetSomeBooks(int limit)
        {
            string query = @$"
               SELECT * FROM books ORDER BY RANDOM() LIMIT @limit
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@limit", limit);
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [HttpGet("{id}")]

        public JsonResult Get(int id)
        {
            string query = @"
               SELECT * FROM books
               WHERE id_book=@id_book
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_book", id);
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }


        [HttpPost]
        public JsonResult Post(Book book)
        {
            string query = @"
                INSERT INTO 
                books(title,author,publisher,year_published,description_book,book_content,page,url_cover,category,added_time,keywords)
                values (@title,@author,@publisher,@year_published,@description_book,@book_content,@page,@url_cover,@category,@added_time,@keywords)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@title", book.Title.ToLower());
                    myCommand.Parameters.AddWithValue("@author", book.Author);
                    myCommand.Parameters.AddWithValue("@publisher", book.Publisher);

                    //DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
                    ////DateOnly dateTime = new DateOnly(2000, 3, 10);
                    //DateTime dateTime = new DateTime(2000, 3, 10);

                    ////DateTime dateTime = book.Year_published;

                    //string dateTimeStr = dateTime.ToString("yyyy-MM-dd");

                    //myCommand.Parameters.AddWithValue("@year_published", dateTimeStr);

                    //DateTime dateTime = new DateTime(2000, 7, 24);

                    //myCommand.Parameters.AddWithValue("@year_published", dateTime);

                    myCommand.Parameters.AddWithValue("@year_published", book.Year_published);
                    myCommand.Parameters.AddWithValue("@description_book", book.Description_book);
                    myCommand.Parameters.AddWithValue("@book_content", book.Book_content);
                    myCommand.Parameters.AddWithValue("@page", book.Page);
                    myCommand.Parameters.AddWithValue("@url_cover", book.Url_cover);
                    myCommand.Parameters.AddWithValue("@category", book.Category);

                    //DateTime time = DateTime.Now;
                    //Debug.WriteLine(time);

                    myCommand.Parameters.AddWithValue("@added_time", book.Added_time);
                    myCommand.Parameters.AddWithValue("@keywords", book.Keywords);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Book book)
        {
            string query = @"
                UPDATE books
                SET 
                title = @title,
                author = @author,
                publisher = @publisher,
                year_published = @year_published,
                description_book = @description_book,
                book_content = @book_content,
                page = @page,
                url_cover = @url_cover,
                category = @category,
                added_time = @added_time,
                keywords = @keywords

                WHERE id_book=@id_book 
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@title", book.Title.ToLower());
                    myCommand.Parameters.AddWithValue("@author", book.Author);
                    myCommand.Parameters.AddWithValue("@publisher", book.Publisher);
                    myCommand.Parameters.AddWithValue("@year_published", book.Year_published);
                    myCommand.Parameters.AddWithValue("@description_book", book.Description_book);
                    myCommand.Parameters.AddWithValue("@book_content", book.Book_content);
                    myCommand.Parameters.AddWithValue("@page", book.Page);
                    myCommand.Parameters.AddWithValue("@url_cover", book.Url_cover);
                    myCommand.Parameters.AddWithValue("@category", book.Category);
                    myCommand.Parameters.AddWithValue("@added_time", book.Added_time);
                    myCommand.Parameters.AddWithValue("@keywords", book.Keywords);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                DELETE FROM books
                where id_book=@id_book
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_book", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Deleted Successfully");
        }

        [Route("search")]
        [HttpGet]
        public JsonResult Search([FromQuery(Name = "title")] string title)
        {
            string query = @$"
                SELECT id_book, title, url_cover
                FROM (
                    SELECT
                        b.id_book,
                        b.title,
                        b.url_cover,
                        CASE
                            WHEN b.title LIKE '%{title}%' THEN 4
                            WHEN b.author LIKE '%{title}%' THEN 3
                            WHEN b.publisher LIKE '%{title}%' THEN 2
                            WHEN b.description_book LIKE '%{title}%' THEN 1
                            ELSE 0
                        END AS score

                    FROM books b
                    WHERE
                        b.title LIKE '%{title}%' OR
                        b.author LIKE '%{title}%' OR
                        b.publisher LIKE '%{title}%' OR
                        b.description_book LIKE '%{title}%'
                    ORDER BY score DESC
                ) final              
                ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [HttpGet]
        [Route("TopGenre/{id}/{limit}")]
        public JsonResult GetTopGenre(int id, int limit)
        {
            string query = @"
                SELECT * FROM books where category = (
                SELECT category
                    FROM     books b natural join library_user lu where id_user = @id_user
                    GROUP BY category
                    ORDER BY COUNT(category) DESC
                    LIMIT    1
                )
                ORDER BY RANDOM() LIMIT @limit
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_user", id);
                    myCommand.Parameters.AddWithValue("@limit", limit);
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [HttpGet]
        [Route("MostPopular/{limit}")]
        public JsonResult GetMostPopular(int limit)
        {
            string query = @"
                SELECT id_book,title,author, publisher, year_published, description_book, book_content, url_cover, category, keywords, added_time, page
                FROM books b natural join library_user lu 
                GROUP BY id_book 
                ORDER BY COUNT(id_book) DESC
                LIMIT @limit
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@limit", limit);
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }



    }
}
