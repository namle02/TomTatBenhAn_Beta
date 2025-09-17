using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using TomTatBenhAn_WPF.Core;
using TomTatBenhAn_WPF.Repos._Model.PatientPhacDo;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class KiemTraPhacDoServices : IKiemTraPhacDoServices
    {
        private readonly IConfigServices _configServices;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        public KiemTraPhacDoServices(IConfigServices configServices, HttpClient httpClient)
        {
            _configServices = configServices;
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        private string GetBaseUrl() => _configServices.GetApiBaseUrl();

        public ObservableCollection<PhacDoItemDTO> TimPhacDoPhuHop(PatientPhacDoAllData patient, ObservableCollection<PhacDoItemDTO> danhSachPhacDo)
        {
            var result = new ObservableCollection<PhacDoItemDTO>();
            var icd = patient.ChanDoanICD;
            if (icd == null || danhSachPhacDo is not { Count: > 0 }) return result;

            // Gom toàn bộ mã ICD (chính + kèm theo, vào + ra viện)
            var patientCodes = SplitIcdCodes(
                icd.MaICDChinhVaoVien,
                icd.MaICDPhuVaoVien,
                icd.MaICDChinhRaVien,
                icd.MaICDKemTheoRaVien
            ).ToHashSet(StringComparer.OrdinalIgnoreCase); // tra cứu O(1)

            foreach (var item in danhSachPhacDo)
            {
                var code = item?.Protocol?.Code;
                if (string.IsNullOrWhiteSpace(code)) continue;

                // Chuẩn hóa code phác đồ (phòng TH có khoảng trắng, chấm phẩy…)
                var norm = code.Trim().ToUpperInvariant();

                if (patientCodes.Contains(norm))
                    result.Add(item!);
            }

            return result;
        }

        private static IEnumerable<string> SplitIcdCodes(params string?[] fields)
        {
            if (fields == null) yield break;
            char[] seps = [',', ';', '|', ' ', '/'];

            foreach (var f in fields)
            {
                if (string.IsNullOrWhiteSpace(f)) continue;
                foreach (var token in f.Split(seps, StringSplitOptions.RemoveEmptyEntries))
                {
                    var code = token.Trim().ToUpperInvariant();
                    if (!string.IsNullOrEmpty(code))
                        yield return code;
                }
            }
        }

        public async Task<ApiResponse<BangKiemResponseDTO>> DanhGiaTuanThuPhacDoAsync(PatientPhacDoAllData patient, PhacDoItemDTO phacDo, BangKiemResponseDTO bangKiem)
        {
            try
            {
                var url = $"{GetBaseUrl()}/ai/DanhGiaTuanThuPhacDo";
                using var content = new StringContent(JsonSerializer.Serialize(new
                {
                    PhacDo = phacDo,
                    BangKiem = bangKiem,
                    PatientData = patient
                }, _jsonOptions), System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<BangKiemResponseDTO>>(body, _jsonOptions);
                return result ?? ApiResponse<BangKiemResponseDTO>.ErrorResult("Không parse được phản hồi");
            }
            catch (Exception ex)
            {
                return ApiResponse<BangKiemResponseDTO>.ErrorResult(ex.Message);
            }
        }
    }
}
