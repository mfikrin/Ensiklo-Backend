using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Collections.Generic;

namespace PPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        public JsonResult Post(User user)
        {
            string query = @"
                INSERT INTO 
                user(email,password,username,role)
                values (@email,@password,@username,@role)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@email", book.Email);
                    myCommand.Parameters.AddWithValue("@username", book.Username);
                    myCommand.Parameters.AddWithValue("@password", book.Password);
                    myCommand.Parameters.AddWithValue("@role", book.Role);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("User Registered Successfully");
        }


    }

}


