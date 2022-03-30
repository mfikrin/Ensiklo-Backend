﻿using Microsoft.AspNetCore.Mvc;
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
                    //DateOnly date = new DateOnly(2000, 3, 10);

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

        [HttpGet]
        [Route("search")]
        
        public JsonResult Search([FromQuery(Name = "title")] string title)
        {
            string query = @$"
                SELECT *
                FROM books b
                WHERE b.title LIKE '%{title}%'
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

    }
}
