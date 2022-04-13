using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using System.Diagnostics;
using PPL.Models;

namespace PPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
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
                SELECT * FROM admin where email=@req_email
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

            if (table.Rows.Count == 0 || !table.Rows[0].Field<string>("password").Equals(req.Password))
            {
                return Unauthorized();
            }
            Int64 admin_id = table.Rows[0].Field<Int64>("id_user");
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            query = @"
                INSERT INTO auth_tokens_admin(token, id_user)
                values (@token, @id_user)
            ";

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@token", token);
                    myCommand.Parameters.AddWithValue("id_user", admin_id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            HttpContext.Response.Cookies.Append("authToken", token, new CookieOptions
            {
                Expires = DateTime.Now.AddYears(5),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });
            return Ok(token);
        }

        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(RegisterRequest admin)
        {
            //if (user.Role == null)
            //{
            //    user.Role = "admin";
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string query = @"
                INSERT INTO 
                admin(email,password,username)
                values (@email,@password,@username)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@email", admin.Email);
                    myCommand.Parameters.AddWithValue("@username", admin.Username);
                    myCommand.Parameters.AddWithValue("@password", admin.Password);
                   

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return Ok();
        }

        [Route("getAdmin/{id}")]
        [HttpGet]
        public JsonResult GetUser(Int64 id)
        {
            Debug.WriteLine(HttpContext.Request.Cookies["authToken"]);
            string query = @$"
               SELECT * FROM admin where id_admin=@id
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    dataReader = myCommand.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    myCon.Close();

                }
            }



            return new JsonResult(table);

        }

        [Route("currentAdmin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCurrent()
        {
            if (HttpContext.Request.Cookies["authToken"] is null)
            {
                return Unauthorized();
            }
            string token = HttpContext.Request.Cookies["authToken"];
            string query = @$"
               SELECT * FROM admin where id_admin=@id
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EnsikloAppCon");
            NpgsqlDataReader dataReader;

            Int64 id = ControllerUtils.authenticateAdmin(token, sqlDataSource);

            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
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
