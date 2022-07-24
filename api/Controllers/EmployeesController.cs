using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Cukcuk.Api.Entities;
using MySqlConnector;
using Dapper;
using Swashbuckle.AspNetCore.Annotations;
using api.Entities.DTO;

namespace Misa.Cukcuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        /// <summary>
        /// API cho phép lấy danh sách nhân viên cho phép lọc và phân trang
        /// </summary>
        /// <param name="code">Mã nhân viên</param>
        /// <param name="name">Tên nhân viên</param>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <param name="positionId">Mã vị trí</param>
        /// <param name="departmentId">Mã phòng ban</param>
        /// <param name="pageNumber">Thứ tự trang</param>
        /// <param name="pageSize">Số bản ghi trong 1 trang</param>
        /// <returns>
        ///     Một đối tượng gồm: 
        ///         + Danh sách nhân viên thỏa mãn điều kiện lọc trang
        ///         + Tổng số nhân viên thỏa mãn điều kiện
        /// </returns>
        /// CreatedBy: BTHOANG
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<Employee>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterEmployees([FromQuery] string? search, [FromQuery] Guid? positionId, [FromQuery] Guid? departmentId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.LHNAM;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh tên Stored procedure
                string storedProcName = "Proc_Employee_GetPaging";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@$skip", (pageNumber - 1) * pageSize);
                parameters.Add("@$Take", pageSize);
                parameters.Add("@$Sort", "modifiedDate DESC");

                string whereClause = "";
                if (search != null || positionId != null || departmentId != null)
                {
                    var whereConditions = new List<string>();
                    string searchClause = "";
                    if(search != null)
                    {
                        var searchConditions = new List<string>();

                        searchConditions.Add($"EmployeeCode LIKE \'%{search}%\'");

                        searchConditions.Add($"EmployeeName LIKE \'%{search}%\'");

                        searchConditions.Add($"PhoneNumber LIKE \'%{search}%\'");

                        searchClause = "(" + string.Join(" OR ", searchConditions) + ")";

                        whereConditions.Add(searchClause);
                    }

                    if (positionId != null)
                    {
                        whereConditions.Add($"PositionID = '{positionId}'");
                    }

                    if (departmentId != null)
                    {
                        whereConditions.Add($"DepartmentID = '{departmentId}'");
                    }
                    whereClause = "WHERE " + string.Join(" AND ", whereConditions);
                }

                parameters.Add("@$Where", whereClause);
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var multipResult = mySqlConnection.QueryMultiple(storedProcName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (multipResult != null)
                {
                    var employee = multipResult.Read<Employee>();
                    var totalCount = multipResult.Read<long>().Single();
                    return StatusCode(200, new PagingData<Employee>()
                    {
                        Data = employee.ToList(),
                        TotalCount = totalCount
                    });
                }
                else
                {
                    return StatusCode(400, "e002");
                }
                //return StatusCode(200, whereClause);
            }
            catch (Exception exception)
            {
                return StatusCode(400, "e001");
            }
        }


        /// <summary>
        /// API thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>ID của nhân viên thêm mới</returns>
        /// CreatedBy: BTHOANG 
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(GuidString))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                // khởi tạo kết nối DB 
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.LHNAM;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh insert
                string insertCommand = "INSERT INTO Employees (EmployeeID, EmployeeCode, EmployeeName, DateOfBirth, Gender, IdentityNumber, IdentityIssuedPlace, IdentityIssuedDate, Email, PhoneNumber, PositionID, DepartmentID, TaxCode, Salary, JoiningDate, WorkStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)" +
                                        "VALUES(@EmployeeID, @EmployeeCode, @EmployeeName, @DateOfBirth, @Gender, @IdentityNumber, @IdentityIssuedPlace, @IdentityIssuedDate, @Email, @PhoneNumber, @PositionID, @DepartmentID, @TaxCode, @Salary, @JoiningDate, @WorkStatus, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy); ";

                // Chuẩn bị tham số đầu vào cho lệnh insert
                var dateTimeNow = DateTime.Now;
                var EmployeeId = Guid.NewGuid();
                var parameters = new DynamicParameters();

                parameters.Add("@EmployeeID", EmployeeId);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityIssuedPlace", employee.IdentityIssuedPlace);
                parameters.Add("@IdentityIssuedDate", employee.IdentityIssuedDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionID", employee.PositionID);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@TaxCode", employee.TaxCode);
                parameters.Add("@Salary", employee.Salary);
                parameters.Add("@JoiningDate", employee.JoiningDate);
                parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@CreatedDate", dateTimeNow);
                parameters.Add("@CreatedBy", employee.CreatedBy);
                parameters.Add("@ModifiedDate", dateTimeNow);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);

                // Thực hiện gọi vào DB để chạy câu lệnh insert với tham số đầu vào ở trên
                var affectedRows = mySqlConnection.Execute(insertCommand, parameters);

                // Xử lý kết quả trả về từ DB
                if (affectedRows > 0)
                {
                    return StatusCode(201, new GuidString()
                    {
                        StringResult = EmployeeId
                    });
                }
                else
                {
                    return StatusCode(400, "e002");
                }

                // Trả về dữ liệu cho Client

                // bổ sung try catch để bắt exception
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(400, "e003");
                }

                return StatusCode(400, "e001");
            }
            catch (Exception)
            {
                return StatusCode(400, "e001");
                throw;
            }
        }


        /// <summary>
        /// API sửa thông tin của một nhân viên
        /// </summary>
        /// <param name="EmployeeId">ID của nhân viên cần sửa thông tin</param>
        /// <param name="employee">Nội dung cần sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// Created by: BTHOANG
        [HttpPut("{EmployeeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GuidString))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateEmployee([FromRoute] Guid EmployeeId, [FromBody] Employee employee)
        {
            try
            {
                // kết nối tới DB
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.LHNAM;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // chuẩn bị câu lệnh update
                string updateCommand = "update Employees " +
                                        "SET EmployeeCode = @EmployeeCode, " +
                                        "EmployeeName = @EmployeeName, " +
                                        "DateOfBirth = @DateOfBirth, " +
                                        "Gender = @Gender, " +
                                        "IdentityNumber = @IdentityNumber, " +
                                        "IdentityIssuedPlace = @IdentityIssuedPlace, " +
                                        "IdentityIssuedDate = @IdentityIssuedDate, " +
                                        "Email = @Email, " +
                                        "PhoneNumber = @PhoneNumber, " +
                                        "PositionID = @PositionID, " +
                                        "DepartmentID = @DepartmentID, " +
                                        "TaxCode = @TaxCode, " +
                                        "Salary = @Salary, " +
                                        "JoiningDate = @JoiningDate, " +
                                        "WorkStatus = @WorkStatus, " +
                                        "ModifiedDate = @ModifiedDate, " +
                                        "ModifiedBy = @ModifiedBy " +
                                        "WHERE EmployeeID = @EmployeeID;";
                // chuẩn bị tham số đầu vào cho lệnh update
                var dateTimeNow = DateTime.Now;
                var parameters = new DynamicParameters();

                parameters.Add("@EmployeeId", EmployeeId);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityIssuedPlace", employee.IdentityIssuedPlace);
                parameters.Add("@IdentityIssuedDate", employee.IdentityIssuedDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionID", employee.PositionID);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@TaxCode", employee.TaxCode);
                parameters.Add("@Salary", employee.Salary);
                parameters.Add("@JoiningDate", employee.JoiningDate);
                parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@ModifiedDate", dateTimeNow);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);

                // gọi db với tham số đầu vào
                int affectedRows = mySqlConnection.Execute(updateCommand, parameters);

                // trả về kết quả
                if (affectedRows > 0)
                {
                    return StatusCode(200, new GuidString()
                    {
                        StringResult = EmployeeId
                    });
                }
                else
                {
                    return StatusCode(400, "e002");
                }
            }
            catch (MySqlException mySqlEx)
            {
                // nếu như employeeCode bị trùng lặp
                if (mySqlEx.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(400, "e003");
                }
                return StatusCode(400, "e001");
            }
            catch (Exception ex)
            {
                return StatusCode(400, "e001");
            }
        }

        /// <summary>
        /// API xóa dữ liệu một nhân viên
        /// </summary>
        /// <param name="EmployeeId">Mã id của nhân viên</param>
        /// <returns>EmployeeId</returns>
        /// Created by: BTHOANG
        [HttpDelete("{EmployeeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GuidString))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteEmployeeById([FromRoute] Guid EmployeeId)
        {
            try
            {
                // tạo kết nối đến db
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.LHNAM;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // chuẩn bị câu lệnh delete
                string deleteCommand = "Delete from Employees where EmployeeID = @EmployeeId";

                // chuẩn bị tham số cho lệnh delete
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", EmployeeId);

                // gọi DB và chạy câu lệnh delete
                int affectedRows = mySqlConnection.Execute(deleteCommand, parameters);

                if (affectedRows > 0)
                {
                    return StatusCode(200, new GuidString()
                    {
                        StringResult = EmployeeId
                    });
                }
                else
                {
                    return StatusCode(400, "e002");
                }
            }
            catch (MySqlException mySqlEx)
            {
                return StatusCode(400, "e001");
            }
        }

        /// <summary>
        /// Hàm lấy thông tin của nhân viên bằng ID
        /// </summary>
        /// <param name="EmployeeId">ID nhân viên</param>
        /// <returns>Thông tin của nhân viên thỏa mãn</returns>
        /// Created by: BTHOANG
        [HttpGet("{EmployeeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Employee))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeeById([FromRoute] Guid EmployeeId)
        {
            try
            {
                // kết nối đến db
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.LHNAM;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // chuẩn bị store procedure 
                string storedProcedureName = "Proc_Employee_GetByEmployeeID";

                // chuẩn bị tham số cho procedure
                var parameters = new DynamicParameters();
                parameters.Add("@$EmployeeID", EmployeeId);

                // gọi đến db và chạy procedure
                var employee = mySqlConnection.QueryFirstOrDefault(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // trả kết quả
                if (employee != null)
                {
                    return StatusCode(200, employee);
                }
                else
                {
                    return StatusCode(404);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, "e001");
            }
        }

        [HttpGet("new-code")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(NewEmployeeCode))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                // Kết nối DB
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.LHNAM;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị function để lấy mã nhân viên mới
                string callFunctionCommand = "select Func_Get_Auto_EmployeeCode()";

                // Gọi tới DB và truy vấn câu lệnh
                string newEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(callFunctionCommand);

                // Trả về kết quả
                return StatusCode(200, new NewEmployeeCode()
                {
                    EmployeeCode = newEmployeeCode
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, "e001");
            }
        }
    }
}
