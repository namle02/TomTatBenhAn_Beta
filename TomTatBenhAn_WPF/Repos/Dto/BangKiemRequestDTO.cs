using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TomTatBenhAn_WPF.Repos.Dto
{
    /// <summary>
    /// DTO cho request tạo/cập nhật bảng kiểm
    /// </summary>
    public partial class BangKiemRequestDTO : ObservableObject
    {
        [ObservableProperty] private string phacDoId = string.Empty;
        [ObservableProperty] private string tenBangKiem = string.Empty;
        [ObservableProperty] private HangMucKiemTra danhGiaVaChanDoan = new HangMucKiemTra { Stt = "1", TenHangMucKiemTra = "Đánh giá và chẩn đoán" };
        [ObservableProperty] private HangMucKiemTra dieuTri = new HangMucKiemTra { Stt = "2", TenHangMucKiemTra = "Điều trị" };
        [ObservableProperty] private HangMucKiemTra chamSoc = new HangMucKiemTra { Stt = "3", TenHangMucKiemTra = "Chăm sóc" };
        [ObservableProperty] private HangMucKiemTra raVien = new HangMucKiemTra { Stt = "4", TenHangMucKiemTra = "Ra viện" };

        partial void OnTenBangKiemChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                // Viết hoa chữ cái đầu
                tenBangKiem = value.ToUpper();
            }
        }
    }

    /// <summary>
    /// DTO cho response bảng kiểm từ API
    /// </summary>
    public partial class BangKiemResponseDTO : ObservableObject
    {
        [ObservableProperty] private string _id = string.Empty;
        [ObservableProperty] private string bangKiemId = string.Empty;
        [ObservableProperty] private string phacDoId = string.Empty;
        [ObservableProperty] private string tenBangKiem = string.Empty;
        [ObservableProperty] private string tenPhacDo = string.Empty;
        [ObservableProperty] private HangMucKiemTra danhGiaVaChanDoan = new();
        [ObservableProperty] private HangMucKiemTra dieuTri = new();
        [ObservableProperty] private HangMucKiemTra chamSoc = new();
        [ObservableProperty] private HangMucKiemTra raVien = new();
        [ObservableProperty] private DateTime createdAt;
        [ObservableProperty] private DateTime updatedAt;

        // Metadata file Word gốc
        [ObservableProperty] private string originalFileName = string.Empty;
        [ObservableProperty] private string originalFilePath = string.Empty;
        [ObservableProperty] private long fileSize;
        [ObservableProperty] private DateTime uploadedAt;
    }

    /// <summary>
    /// DTO cho request kiểm tra tồn tại bảng kiểm
    /// </summary>
    public partial class CheckBangKiemExistsRequestDTO : ObservableObject
    {
        [ObservableProperty] private string tenBangKiem = string.Empty;
        [ObservableProperty] private string phacDoId = string.Empty;
    }

    /// <summary>
    /// DTO cho response kiểm tra tồn tại bảng kiểm
    /// </summary>
    public partial class CheckBangKiemExistsResponseDTO : ObservableObject
    {
        [ObservableProperty] private bool exists;
        [ObservableProperty] private BangKiemResponseDTO? data;
    }
}
