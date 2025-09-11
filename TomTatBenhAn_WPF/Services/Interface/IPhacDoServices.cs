using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Core;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IPhacDoServices
    {
        /// <summary>
        /// Lấy tất cả phác đồ từ server
        /// </summary>
        /// <returns>Danh sách tất cả phác đồ</returns>
        Task<ApiResponse<List<PhacDoItemDTO>>> GetAllPhacDoAsync();

        /// <summary>
        /// Lấy phác đồ theo ID
        /// </summary>
        /// <param name="id">ID của phác đồ</param>
        /// <returns>Thông tin phác đồ</returns>
        Task<ApiResponse<PhacDoItemDTO>> GetPhacDoByIdAsync(string id);

        /// <summary>
        /// Tìm kiếm phác đồ theo tên
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách phác đồ phù hợp</returns>
        Task<ApiResponse<List<PhacDoItemDTO>>> SearchPhacDoAsync(string searchTerm);

        /// <summary>
        /// Kiểm tra phác đồ có tồn tại hay không
        /// </summary>
        /// <param name="name">Tên phác đồ</param>
        /// <param name="code">Mã phác đồ (tùy chọn)</param>
        /// <returns>Kết quả kiểm tra</returns>
        Task<ApiResponse<PhacDoItemDTO>> CheckProtocolExistsAsync(string name, string? code = null);

        /// <summary>
        /// Thêm phác đồ mới (phân tích từ text)
        /// </summary>
        /// <param name="request">Dữ liệu phác đồ raw text</param>
        /// <returns>Kết quả thêm phác đồ</returns>
        Task<ApiResponse<ProtocolDTO>> AddPhacDoAsync(AddPhacDoRequestDTO request);

        /// <summary>
        /// Thêm phác đồ mới với tùy chọn ghi đè
        /// </summary>
        /// <param name="rawText">Text phác đồ</param>
        /// <param name="force">Có ghi đè nếu đã tồn tại không</param>
        /// <returns>Kết quả thêm phác đồ</returns>
        Task<ApiResponse<ProtocolDTO>> AddPhacDoWithForceAsync(string rawText, bool force = false);

        /// <summary>
        /// Cập nhật phác đồ hiện có
        /// </summary>
        /// <param name="id">ID của phác đồ</param>
        /// <param name="updateData">Dữ liệu cập nhật</param>
        /// <returns>Kết quả cập nhật</returns>
        Task<ApiResponse<PhacDoItemDTO>> UpdatePhacDoAsync(string id, PhacDoItemDTO updateData);

        /// <summary>
        /// Xóa phác đồ theo ID
        /// </summary>
        /// <param name="id">ID của phác đồ</param>
        /// <returns>Kết quả xóa</returns>
        Task<ApiResponse<object>> DeletePhacDoAsync(string id);
    }
}
