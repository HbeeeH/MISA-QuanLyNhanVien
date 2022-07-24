using System.ComponentModel.DataAnnotations;

namespace Misa.Cukcuk.Api.Entities
{
    /// <summary>
    /// Class vị trí
    /// </summary>
    /// Created by: LHNAM (11/7/2022)
    public class Position
    {
        /// <summary>
        /// ID vị trí
        /// </summary>
        public Guid PositionID { get; set; }

        /// <summary>
        /// Mã vị trí
        /// </summary>
        [Required(ErrorMessage = "")]
        public String PositionCode { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        [Required(ErrorMessage = "")]
        public String PositionName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public String CreatedBy { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        public String ModifiedBy { get; set; }
    }
}
