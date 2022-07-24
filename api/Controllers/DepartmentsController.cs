using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Cukcuk.Api.Entities;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace Misa.Cukcuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// Hàm lấy tất cả dữ liệu từ bảng vị trí
        /// </summary>
        /// <returns>Danh sách các vị trí</returns>
        /// Created by: BTHOANG
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Department))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllDepartment()
        {
            try
            {
                // Kết nối tới DB
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.LHNAM;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Select
                string selectCommand = "select * from Departments";

                // Thực hiện gọi DB và truy vấn câu lệnh
                var departments = mySqlConnection.Query<Department>(selectCommand);

                // Trả về kết quả
                return StatusCode(200, departments);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "e001");
            }
        }
    }
}
