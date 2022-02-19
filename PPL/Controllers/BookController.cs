using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PPL.Models;
using System;
using System.Data;


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
                SELECT b.id_book,title,rating,description_book,pages,publisher,url_cover,array_to_string(array_agg(concat(a.author_first_name,' ',a.author_last_name) order by ba.author_ordinal ASC), ', ') AS author_names
                FROM books b
                LEFT JOIN books_authors ba
                  ON (b.id_book = ba.id_book)
                LEFT JOIN authors a 
                  ON (ba.id_author = a.id_author)
                GROUP BY b.id_book
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

        //// GET api/<BookController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<BookController>
        //[HttpPost]
        //public ActionResult Post([FromBody] Book newBook)
        //{
        //    bool badThingHappened = false;
        //    if (badThingHappened)
        //    {
        //        return BadRequest();
        //    }

        //    return Created("", newBook);
        //}

        //// PUT api/<BookController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //    //bool badThingHappened = false;
        //    //if (badThingHappened)
        //    //{
        //    //    return BadRequest();
        //    //}
                
        //    //return Created("",)
            

        //}

        //// DELETE api/<BookController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    //bool badThingsHappened = false;

        //    //if (badThingsHappened)
        //    //{
        //    //    return BadRequest();
        //    //}



        //    //return NoContent();
            
        //}
    }
}
