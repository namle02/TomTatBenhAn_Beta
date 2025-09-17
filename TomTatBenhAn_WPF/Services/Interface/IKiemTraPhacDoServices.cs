using System.Collections.ObjectModel;
using TomTatBenhAn_WPF.Core;
using TomTatBenhAn_WPF.Repos._Model.PatientPhacDo;
using TomTatBenhAn_WPF.Repos.Dto;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IKiemTraPhacDoServices
    {
        ObservableCollection<PhacDoItemDTO> TimPhacDoPhuHop(PatientPhacDoAllData patient, ObservableCollection<PhacDoItemDTO> danhSachPhacDo);
        Task<ApiResponse<BangKiemResponseDTO>> DanhGiaTuanThuPhacDoAsync(PatientPhacDoAllData patient, PhacDoItemDTO phacDo, BangKiemResponseDTO bangKiem);
    }
}
