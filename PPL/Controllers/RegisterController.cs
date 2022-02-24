using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PPL.Models;
using System;
using Npgsql;

using System.Data;

namespace PPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: api/<BookController>
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                INSERT INTO user (email, password, userName, role)
                VALUES (@p1),(@p2),(@p3),(@p4)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using var cmd = new NpgsqlCommand(query, myCon)
                {
                    Parameters =
                    {
                        new("p1", "some_value"),
                        new("p2", "some_other_value")
                    }
                };

                dataReader = cmd.ExecuteReader();
                table.Load(dataReader);

                dataReader.Close();
                myCon.Close();
                
            }



            return new JsonResult(table);

        }


    }

}


