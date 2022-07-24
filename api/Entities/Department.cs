using System.ComponentModel.DataAnnotations;

namespace Misa.Cukcuk.Api.Entities
{
    /// <summary>
    /// Class phòng ban
    /// </summary>
    /// Created by: LHNAM  (11/7/2022)
    public class Department
    {
        /// <summary>
        /// ID phòng ban
        /// </summary>
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        [Required(ErrorMessage = "")]
        public String DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [Required(ErrorMessage = "")]
        public String DepartmentName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public String CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public String ModifiedBy { get; set; }
    }
}
