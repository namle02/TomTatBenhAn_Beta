using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos.Model;

namespace TomTatBenhAn_WPF.Repos.Mappers.Interface
{
    public interface IDataMapper
    {
        Task<List<LoadData>> GetSoBenhAnData(string maYTe);
        Task<ThongTinBenhNhan> GetThongTinBenhNhanData(string SoBenhAn);
        Task<ChuanDoanModel> GetChuanDoanData(string SoBenhAn);
        Task<BenhAnTypeModel> GetBenhAnTypeData(string SoBenhAn);
        Task<BenhAnChiTietModel> GetBenhAnChiTietAsync(string loaiBenhAn, string benhAnTongQuatId);
        Task<List<KetQuaXetNghiemCLSModel>> GetKetQuaXetNghiemModelData(string SoBenhAn);
        Task<HanhChinhModel> GetHanhChinhData(string SoBenhAn);
        Task<CheckBoxModel> UpdateCheckBoxesFromKetQuaDieuTri(string ketQuaDieuTri);
        Task<List<DienBienModel>> GetDienBienData(string SoBenhAn);
    }
}

