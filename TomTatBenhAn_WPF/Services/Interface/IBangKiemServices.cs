using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Core;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IBangKiemServices
    {
        /// <summary>
        /// Lấy tất cả bảng kiểm
        /// </summary>
        /// <param name="phacDoId">ID phác đồ (tùy chọn)</param>
        /// <param name="search">Từ khóa tìm kiếm (tùy chọn)</param>
        /// <returns>Danh sách bảng kiểm</returns>
        Task<ApiResponse<List<BangKiemResponseDTO>>> GetAllAsync(string? phacDoId = null, string? search = null);

        /// <summary>
        /// Lấy bảng kiểm theo ID
        /// </summary>
        /// <param name="id">ID bảng kiểm</param>
        /// <returns>Thông tin bảng kiểm</returns>
        Task<ApiResponse<BangKiemResponseDTO>> GetByIdAsync(string id);

        /// <summary>
        /// Tạo bảng kiểm mới
        /// </summary>
        /// <param name="payload">Dữ liệu bảng kiểm</param>
        /// <returns>Kết quả tạo bảng kiểm</returns>
        Task<ApiResponse<BangKiemResponseDTO>> CreateAsync(BangKiemRequestDTO payload);

        /// <summary>
        /// Cập nhật bảng kiểm
        /// </summary>
        /// <param name="id">ID bảng kiểm</param>
        /// <param name="payload">Dữ liệu cập nhật</param>
        /// <returns>Kết quả cập nhật</returns>
        Task<ApiResponse<BangKiemResponseDTO>> UpdateAsync(string id, BangKiemRequestDTO payload);

        /// <summary>
        /// Xóa bảng kiểm
        /// </summary>
        /// <param name="id">ID bảng kiểm</param>
        /// <returns>Kết quả xóa</returns>
        Task<ApiResponse<object>> DeleteAsync(string id);

        /// <summary>
        /// Kiểm tra bảng kiểm có tồn tại không
        /// </summary>
        /// <param name="tenBangKiem">Tên bảng kiểm</param>
        /// <param name="phacDoId">ID phác đồ</param>
        /// <returns>Kết quả kiểm tra</returns>
        Task<ApiResponse<CheckBangKiemExistsResponseDTO>> CheckExistsAsync(string tenBangKiem, string phacDoId);
    }
}
