namespace Misa.Cukcuk.Api.Entities
{
    /// <summary>
    /// Dữ liệu trả về khi lọc và phân trang
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng trong mảng trả về</typeparam>
    /// Created by: BTHOANG
    public class PagingData<T>
    {
        /// <summary>
        /// Mảng đối tượng thỏa mãn điều kiện phân trang
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Số bản ghi thỏa mãn điều kiện
        /// </summary>
        public long TotalCount { get; set; }
    }
}
