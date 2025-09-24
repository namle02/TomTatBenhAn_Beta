using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IPhacDoReportServices
    {
        /// <summary>
        /// Đọc bảng đánh giá từ file Word và chuyển thành BangKiemRequestDTO hợp lệ
        /// </summary>
        /// <param name="filePath">Đường dẫn file Word</param>
        /// <param name="phacDoId">Id phác đồ liên kết</param>
        /// <param name="tenBangKiem">Tên bảng kiểm</param>
        /// <returns>Đối tượng BangKiemRequestDTO</returns>
        TomTatBenhAn_WPF.Repos.Dto.BangKiemRequestDTO ExtractTableBangDanhGiaFromWord(string filePath, string phacDoId, string tenBangKiem);

        /// <summary>
        /// Tạo file Word output từ file gốc và đổ dữ liệu bảng kiểm đã đánh giá vào
        /// </summary>
        /// <param name="originalFilePath">Đường dẫn file Word gốc</param>
        /// <param name="outputFilePath">Đường dẫn file Word output sẽ tạo</param>
        /// <param name="bangKiemData">Dữ liệu bảng kiểm đã đánh giá</param>
        /// <returns>Đường dẫn file output (y như outputFilePath) hoặc null nếu lỗi</returns>
        Task<string?> CreateOutputFileWithDataAsync(string originalFilePath, string outputFilePath, TomTatBenhAn_WPF.Repos.Dto.BangKiemResponseDTO bangKiemData);

        /// <summary>
        /// Đếm số lượng tiêu chí đã được đánh dấu (đạt/không đạt/không áp dụng)
        /// </summary>
        int CountUpdatedCriteria(TomTatBenhAn_WPF.Repos.Dto.BangKiemResponseDTO bangKiemData);
    }
}
