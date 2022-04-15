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
        [Route("sort/title")]

        public JsonResult SortTitle()
        {
            string query = @"
               SELECT * FROM books b order by b.title
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
        [Route("sort/AddedTime")]
        public JsonResult SortAddedTime()
        {
            string query = @"
               SELECT * FROM books b order by b.added_time
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
        [Route("sort/LastRead/{id}")]

        public JsonResult SortLastRead(int id)
        {
            string query = @"
               SELECT * FROM books natural join library_user where id_user = @id_user order by last_readtime
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
                library_user(id_user,id_book,at_page,last_readtime,finish_reading)
                values (@id_user,@id_book,@at_page,@last_readtime,@finish_reading)
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
                    
                    /*
                    DateTime time = DateTime.Now;
                    Debug.WriteLine(time);
                    myCommand.Parameters.AddWithValue("@last_readtime", time);*/
                    myCommand.Parameters.AddWithValue("@last_readtime", libraryUser.Last_readtime);
                    myCommand.Parameters.AddWithValue("@finish_reading", false);

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

    }
}
