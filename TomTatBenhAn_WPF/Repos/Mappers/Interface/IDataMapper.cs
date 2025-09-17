using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Repos._Model.PatientData;
using TomTatBenhAn_WPF.Repos._Model.PatientPhacDo;

namespace TomTatBenhAn_WPF.Repos.Mappers.Interface
{

    public enum DataPatientType
    {
        ChanDoanICD, DienBien, KetQuaXetNghiem, ThongTinHanhChinh, LoaiBenhAn, DanhSachBenhAn, ThongTinKhamBenh

    }

    public interface IDataMapper
    {
        /// <summary>
        /// Lấy toàn bộ thông tin bệnh nhân theo số bệnh án
        /// </summary>
        /// <param name="SoBenhAn"></param>
        /// <returns></returns>
        Task<PatientAllData> GetAllPatientData(string SoBenhAn);

        /// <summary>
        /// Lấy danh sách số bệnh án theo mã y tế
        /// </summary>
        /// <param name="MaYTe"></param>
        /// <returns></returns>
        Task<List<BenhAnIdModel>> GetBenhAnList(string MaYTe);

        Task<PatientPhacDoAllData> GetAllPatientPhacDoData(string SoBenhAn);
    }
}

