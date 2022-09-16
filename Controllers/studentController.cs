using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CRUD_dapper.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    public class studentController : Controller
    {
        private readonly IConfiguration _config;
        public studentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<student>>> GetAllStudents()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("config"));
            IEnumerable<student> students = await SelectAllstudents(connection);
            return Ok(students);
        }

       
        [HttpGet("{studentId}")]
        public async Task<ActionResult<student>> Getstudent(int studentId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("config"));
            var studen = await connection.QueryFirstAsync<student>("select * from NewStudent where id = @id",
                    new { id = studentId });
            return Ok(studen);
        }

        [HttpPost]
        public async Task<ActionResult<List<student>>> Createstudent(student std)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("config"));
            await connection.ExecuteAsync("insert into NewStudent (id,name,address,email,mobile) values (@id,@name,@address,@email,@mobile)", std);
            return Ok(await SelectAllstudents(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<student>>> Updatestudent(student std)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("config"));
            await connection.ExecuteAsync("update NewStudent set id = @id, name = @name, address = @address, email = @email, mobile = @mobile where id = @id", std);
            return Ok(await SelectAllstudents(connection));
        }

        [HttpDelete("{studentId}")]
        public async Task<ActionResult<List<student>>> DeleteHero(int studentId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("config"));
            await connection.ExecuteAsync("delete from NewStudent where id = @id", new { id = studentId });
            return Ok(await SelectAllstudents(connection));
        }

        private static async Task<IEnumerable<student>> SelectAllstudents(SqlConnection connection)
        {
            return await connection.QueryAsync<student>("select * from NewStudent");
        }
    }
}
