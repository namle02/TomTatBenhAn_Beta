using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Linq;
using TomTatBenhAn_WPF.Repos._Model;
using TomTatBenhAn_WPF.Repos._Model.PatientData;
using TomTatBenhAn_WPF.Services.Interface;
using Newtonsoft.Json;
using ControlzEx.Standard;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IFileServices _fileServices;

        public AiService(IFileServices fileServices, IConfigServices configServices)
        {
            _fileServices = fileServices;
        }

        public async Task TomTatBenhAn(PatientAllData patient)
        {
            // Khởi tạo đối tượng tóm tắt nếu chưa có
            if (patient.ThongTinTomTat == null || !patient.ThongTinTomTat.Any())
            {
                patient.ThongTinTomTat = new List<DataTomTat> { new DataTomTat() };
            }

            var tomTat = patient.ThongTinTomTat[0];

            // Cấu hình URL và API Key
            string baseUri = ConfigurationManager.AppSettings["URL_gemini"] ??
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
            string apiKey1 = ConfigurationManager.AppSettings["API_gemini_1"] ?? "";
            string apiKey2 = ConfigurationManager.AppSettings["API_gemini_2"] ?? "";
            string apiKey3 = ConfigurationManager.AppSettings["API_gemini_3"] ?? "";

            // Tóm tắt quá trình bệnh lý
            await TomTatQuaTrinhBenhLy(patient, tomTat, baseUri, apiKey1);

            // Tóm tắt tình trạng người bệnh ra viện
            await TomTatTinhTrangRaVien(patient, tomTat, baseUri, apiKey2);

            // Tóm tắt kết quả xét nghiệm
            await TomTatKetQuaXetNghiem(patient, tomTat, baseUri, apiKey3);
        }

        private async Task TomTatQuaTrinhBenhLy(PatientAllData patient, DataTomTat tomTat, string baseUri, string apiKey)
        {
            if (patient.ThongTinKhamBenh == null || !patient.ThongTinKhamBenh.Any())
                return;

            string rawPrompt = _fileServices.GetPromt("QuaTrinhBenhLyPromt.txt");
            string prompt = rawPrompt.Replace("@QuaTrinhBenhLy", patient.ThongTinKhamBenh[0].QuaTrinhBenhLy);

            string tinhTrangRaVien = patient.TinhTrangNguoiBenhRaVien?.FirstOrDefault()?.DienBien ?? "";
            prompt = prompt.Replace("@TinhTrangNguoiBenhRaVien", tinhTrangRaVien);

            string ketQuaDieuTri = patient.ThongTinHanhChinh?.FirstOrDefault()?.KetQuaDieuTri ?? "Không có thông tin";
            prompt = prompt.Replace("@KetQuaDieuTri", ketQuaDieuTri);

            string chanDoanChinhRaVien = patient.ChanDoanIcd?.FirstOrDefault()?.BenhChinhRaVien ?? "";
            prompt = prompt.Replace("@ChanDoanChinhRaVien", chanDoanChinhRaVien);

            string chanDoanPhuhRaVien = patient.ChanDoanIcd?.FirstOrDefault()?.BenhKemTheoRaVien ?? "";
            prompt = prompt.Replace("@ChanDoanPhuRaVie", chanDoanPhuhRaVien);

            string result = await CallGeminiApi(baseUri, apiKey, prompt);

            string marker = "Những dấu hiệu lâm sàng chính:";

            // Tìm vị trí bắt đầu của phần dấu hiệu lâm sàng
            int index = result.IndexOf(marker);

            // Kiểm tra nếu không tìm thấy marker
            if (index < 0)
            {
                // Nếu không tìm thấy marker, lấy toàn bộ kết quả làm quá trình bệnh lý
                tomTat.TomTatQuaTrinhBenhLy = result.Trim();
                tomTat.TomTatDauHieuLamSang = "";
                return;
            }

            string startMarker = "Quá trình bệnh lý và diễn biến lâm sàng:";
            int startIndex = result.IndexOf(startMarker);
            
            string QuaTrinhBenhLy;
            if (startIndex >= 0)
            {
                // Tìm thấy start marker, lấy phần từ sau marker đến trước marker "Những dấu hiệu lâm sàng chính:"
                int quaTrinhStartIndex = startIndex + startMarker.Length;
                int quaTrinhLength = index - quaTrinhStartIndex;
                
                // Đảm bảo length không âm
                if (quaTrinhLength > 0)
                {
                    QuaTrinhBenhLy = result.Substring(quaTrinhStartIndex, quaTrinhLength).Trim();
                }
                else
                {
                    // Nếu length <= 0, lấy từ đầu đến marker
                    QuaTrinhBenhLy = result.Substring(0, index).Trim();
                }
            }
            else
            {
                // Không tìm thấy start marker, lấy từ đầu đến marker "Những dấu hiệu lâm sàng chính:"
                QuaTrinhBenhLy = result.Substring(0, index).Trim();
            }

            string DauHieuLamSang = result.Substring(index + marker.Length).Trim();
            tomTat.TomTatQuaTrinhBenhLy = QuaTrinhBenhLy;
            tomTat.TomTatDauHieuLamSang = DauHieuLamSang;
        }

        private async Task TomTatTinhTrangRaVien(PatientAllData patient, DataTomTat tomTat, string baseUri, string apiKey)
        {
            if (patient.TinhTrangNguoiBenhRaVien == null || !patient.TinhTrangNguoiBenhRaVien.Any())
                return;

            string rawPrompt = _fileServices.GetPromt("TinhTrangNguoiBenhRaVienPromt.txt");
            string dienBien = patient.TinhTrangNguoiBenhRaVien[0].DienBien ?? "";
            string prompt = rawPrompt.Replace("@DienBien", dienBien);

            string result = await CallGeminiApi(baseUri, apiKey, prompt);
            string marker = "Hướng điều trị tiếp theo:";
            int index = result.IndexOf(marker);

            // Kiểm tra nếu không tìm thấy marker
            if (index < 0)
            {
                // Nếu không tìm thấy marker, lấy toàn bộ kết quả làm tình trạng ra viện
                tomTat.TomTatTinhTrangNguoiBenhRaVien = result.Trim();
                tomTat.TomTatHuongDieuTriTiepTheo = "";
                return;
            }

            string TinhTrangNguoiBenhRaVien = result.Substring(0, index).Trim();

            string HuongDieuTri = result.Substring(index + marker.Length).Trim();
            tomTat.TomTatTinhTrangNguoiBenhRaVien = TinhTrangNguoiBenhRaVien;
            tomTat.TomTatHuongDieuTriTiepTheo = HuongDieuTri;
        }

        private async Task TomTatKetQuaXetNghiem(PatientAllData patient, DataTomTat tomTat, string baseUri, string apiKey)
        {
            if (patient.KetQuaXetNghien == null || !patient.KetQuaXetNghien.Any())
                return;

            string rawPrompt = _fileServices.GetPromt("KetQuaXNPromt.txt");

            // Lấy chẩn đoán chính từ danh sách chẩn đoán ICD
            string chanDoanChinh = patient.ChanDoanIcd?.FirstOrDefault()?.BenhChinhRaVien ??
                                   patient.ChanDoanIcd?.FirstOrDefault()?.BenhChinhVaoVien ?? "";

            string chanDoanKemTheo = patient.ChanDoanIcd?.FirstOrDefault()?.BenhKemTheoRaVien ?? "";

            // Chuyển đổi danh sách kết quả xét nghiệm thành JSON
            string danhSachKQXN = JsonConvert.SerializeObject(patient.KetQuaXetNghien);

            string prompt = rawPrompt.Replace("@ChanDoanVaoVien", chanDoanChinh);
            prompt = prompt.Replace("@ChanDoanRaVien", chanDoanKemTheo);
            prompt = prompt.Replace("@DanhSachKQXN", danhSachKQXN);

            string result = await CallGeminiApi(baseUri, apiKey, prompt);
            tomTat.TomTatKetQuaXN = result;
        }

        private async Task<string> CallGeminiApi(string baseUri, string apiKey, string prompt)
        {
            try
            {
                var requestData = new
                {
                    contents = new[] {
                        new {
                            parts = new[] {
                                new {
                                    text = prompt
                                }
                            }
                        }
                    }
                };

                string jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Thiết lập headers theo yêu cầu của Gemini API
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-goog-api-key", apiKey);

                var response = await _httpClient.PostAsync($"{baseUri}", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse response để lấy text từ Gemini API
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string result = responseObj?.candidates?[0]?.content?.parts?[0]?.text ?? "";

                    return result;
                }
                else
                {
                    throw new HttpRequestException($"API call failed with status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gọi Gemini API: {ex.Message}", ex);
            }
        }
    }
}
