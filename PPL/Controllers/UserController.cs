using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using PPL.Models;

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

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Post(LoginRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string query = @"
                SELECT * FROM users where email=@req_email
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@req_email", req.Email);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            if(table.Rows.Count == 0 || !table.Rows[0].Field<string>("password").Equals(req.Password))
            {
                return Unauthorized();
            }
            Int64 user_id = table.Rows[0].Field<Int64>("id_user");
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            query = @"
                INSERT INTO auth_tokens(token, id_user)
                values (@token, @id_user)
            ";

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@token", token);
                    myCommand.Parameters.AddWithValue("id_user", user_id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            HttpContext.Response.Cookies.Append("authToken", token);
            return Ok(token);
        }

        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(RegisterRequest user)
        {
            if (user.Role == null)
            {
                user.Role = "user";
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string query = @"
                INSERT INTO 
                users(email,password,username,role)
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
                    myCommand.Parameters.AddWithValue("@email", user.Email);
                    myCommand.Parameters.AddWithValue("@username", user.Username);
                    myCommand.Parameters.AddWithValue("@password", user.Password);
                    myCommand.Parameters.AddWithValue("@role", user.Role);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return Ok();
        }


    }
}
