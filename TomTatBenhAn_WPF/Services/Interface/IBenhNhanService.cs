using TomTatBenhAn_WPF.Repos._Model;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IBenhNhanService
    {
        Task<ApiResponse<PatientAllData>> SaveBenhNhanAsync(PatientAllData benhNhanData);
        Task<ApiResponse<PatientAllData>> GetBenhNhanBySoBenhAnAsync(string soBenhAn);
        Task<ApiResponse<List<PatientAllData>>> GetAllBenhNhanAsync(int page = 1, int limit = 10);
        Task<ApiResponse<bool>> DeleteBenhNhanAsync(string id);
        Task<ApiResponse<List<PatientAllData>>> SearchBenhNhanByNameAsync(string tenBN);
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
