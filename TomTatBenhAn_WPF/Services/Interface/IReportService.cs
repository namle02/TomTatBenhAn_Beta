using TomTatBenhAn_WPF.Repos._Model;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IReportService
    {
        /// <summary>
        /// Tạo HTML report từ PatientAllData
        /// </summary>
        /// <param name="patientData">Dữ liệu bệnh nhân</param>
        /// <returns>HTML content đã được thay thế bookmark</returns>
        Task<string> GenerateHtmlReportAsync(PatientAllData patientData);
        
        /// <summary>
        /// Lấy đường dẫn template HTML
        /// </summary>
        /// <returns>Đường dẫn tới file template</returns>
        string GetTemplateHtmlPath();
    }
}
