using TomTatBenhAn_WPF.Repos._Model;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IReportService
    {
        void PrintFileWord(string templateFilePath, PatientAllData patient);
        
    }
}
