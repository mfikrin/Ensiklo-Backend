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
    public class LibraryUserController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public LibraryUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<LibraryUserController>
        [HttpGet("{id}")]
        public JsonResult GetLibraryUser(int id)
        {
            string query = @"
               SELECT * FROM books natural join library_user where id_user = @id_user
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
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        // GET: api/<LibraryUserController>
        [HttpGet("get/{id_user}/{id_book}")]
        public JsonResult GetLibraryUserItem(int id_user, int id_book)
        {
            string query = @"
               SELECT * FROM books natural join library_user where id_user = @id_user and id_book = @id_book
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_user", id_user);
                    myCommand.Parameters.AddWithValue("@id_book", id_book);
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [HttpGet]
        [Route("sort/title/{id}")]

        public JsonResult SortTitle(int id)
        {
            string query = @"
               SELECT * FROM books natural join library_user where id_user = @id_user order by title
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
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [HttpGet]
        [Route("sort/AddedTime/{id}")]
        public JsonResult SortAddedTime(int id)
        {
            string query = @"
               SELECT * FROM books natural join library_user where id_user = @id_user order by added_time_to_library DESC
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
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }


        [HttpGet]
        [Route("sort/LastRead/{id}")]

        public JsonResult SortLastRead(int id)
        {
            string query = @"
               SELECT * FROM books natural join library_user where id_user = @id_user order by last_readtime DESC
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
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        // POST api/<LibraryUserController>
        [HttpPost]
        public JsonResult Post(LibraryUser libraryUser)
        {
            string query = @"
                INSERT INTO 
                library_user(id_user,id_book,at_page,last_readtime,finish_reading, added_time_to_library)
                values (@id_user,@id_book,@at_page,@last_readtime,@finish_reading, @added_time_to_library)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_user", libraryUser.Id_user);
                    myCommand.Parameters.AddWithValue("@id_book", libraryUser.Id_book);
                    myCommand.Parameters.AddWithValue("@at_page", libraryUser.At_page);
                    
                    myCommand.Parameters.AddWithValue("@last_readtime", libraryUser.Last_readtime);
                    myCommand.Parameters.AddWithValue("@finish_reading", false);
                    myCommand.Parameters.AddWithValue("@added_time_to_library", libraryUser.Added_time_to_library);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();


                }
            }

            Debug.WriteLine("Book added");

            return new JsonResult("Added to Library Successfully");
        }



        // DELETE api/<LibraryUserController>/5
        [HttpDelete("{id_user}/{id_book}")]
        public JsonResult Delete(int id_user, int id_book)
        {
            string query = @"
                DELETE FROM library_user
                where id_user=@id_user and id_book=@id_book
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_user", id_user);
                    myCommand.Parameters.AddWithValue("@id_book", id_book);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Deleted Successfully");
        }

        [HttpGet]
        [Route("FinishedBooks/{user_id}")]
        public JsonResult GetTopGenre(int user_id)
        {
            string query = @"
                select * from library_user where id_user = @id_user and finish_reading = true;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_user", user_id);
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [Route("search")]
        [HttpGet]
        public JsonResult Search([FromQuery(Name = "title")] string title, [FromQuery(Name = "user_id")] int user_id)
        {
            string query = @$"
                SELECT *
                FROM (
                    SELECT
                        CASE
                            WHEN b.title LIKE '%{title}%' THEN 4
                            WHEN b.author LIKE '%{title}%' THEN 3
                            WHEN b.publisher LIKE '%{title}%' THEN 2
                            WHEN b.description_book LIKE '%{title}%' THEN 1
                            ELSE 0
                        END AS score,
                        b.*

                    FROM books b natural join library_user 
                    WHERE
                        id_user = @id_user AND
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
                    myCommand.Parameters.AddWithValue("@id_user", user_id);
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [Route("updateFinishStatus/{id_user}/{id_book}")]
        [HttpPut]
        public JsonResult Put(int id_user, int id_book)
        {
            string query = @"
                UPDATE library_user
                SET 
                finish_reading = @finish_reading

                WHERE id_book=@id_book AND id_user=@id_user
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_user", id_user);
                    myCommand.Parameters.AddWithValue("@id_book", id_book);
                    myCommand.Parameters.AddWithValue("@finish_reading", true);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Updated Successfully");
        }


    }
}
