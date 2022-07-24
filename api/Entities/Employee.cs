using Misa.Cukcuk.Api.Enum;
using System.ComponentModel.DataAnnotations;

namespace Misa.Cukcuk.Api.Entities
{
    /// <summary>
    /// Class nhân viên
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        public Guid? EmployeeID { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required(ErrorMessage = "e005")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Số CMTND
        /// </summary>
        [Required(ErrorMessage = "e006")]
        public String IdentityNumber { get; set; }

        /// <summary>
        /// Nơi cấp CMTND
        /// </summary>
        public String? IdentityIssuedPlace { get; set; }

        /// <summary>
        /// Ngày cấp CMTND
        /// </summary>
        public DateTime? IdentityIssuedDate { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "e007")]
        [EmailAddress(ErrorMessage = "e009")]
        public String Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required(ErrorMessage = "e008")]
        public String PhoneNumber { get; set; }

        /// <summary>
        /// ID vị trí (khóa ngoại liên kết với bảng position)
        /// </summary>
        public Guid? PositionID { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public String? PositionName { get; set; }

        /// <summary>
        /// ID phòng ban (khóa ngoại liên kết với bảng phòng ban)
        /// </summary>
        public Guid? DepartmentID { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public String? DepartmentName { get; set; }

        /// <summary>
        /// Mã só thuế cá nhân
        /// </summary>
        public String TaxCode { get; set; }

        /// <summary>
        /// Lương cơ bản
        /// </summary>
        public Double Salary { get; set; }

        /// <summary>
        /// Ngày tham gia
        /// </summary>
        public DateTime? JoiningDate { get; set; }

        /// <summary>
        /// Trạng thái công việc
        /// </summary>
        public WorkStatus? WorkStatus { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public String? CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public String? ModifiedBy { get; set; }

    }
}
