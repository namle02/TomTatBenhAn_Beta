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
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
            
            // Lấy key để giải mã
            string decryptKey = ConfigurationManager.AppSettings["KeyDecrypt"] ?? "TomTatBenhAn";
            
            // Đọc và giải mã API keys từ config
            string encryptedApiKey1 = ConfigurationManager.AppSettings["API_gemini_1"] ?? "";
            string encryptedApiKey2 = ConfigurationManager.AppSettings["API_gemini_2"] ?? "";
            string encryptedApiKey3 = ConfigurationManager.AppSettings["API_gemini_3"] ?? "";
            
            // Giải mã API keys (nếu là Base64 thì decrypt, nếu không thì dùng trực tiếp để tương thích ngược)
            string apiKey1 = TryDecryptApiKey(encryptedApiKey1, decryptKey);
            string apiKey2 = TryDecryptApiKey(encryptedApiKey2, decryptKey);
            string apiKey3 = TryDecryptApiKey(encryptedApiKey3, decryptKey);

            // Tạo danh sách API keys để fallback
            var apiKeys = new[] { apiKey1, apiKey2, apiKey3 }.Where(k => !string.IsNullOrEmpty(k)).ToArray();

            // Tóm tắt quá trình bệnh lý (với fallback)
            await TomTatQuaTrinhBenhLy(patient, tomTat, baseUri, apiKeys);

            // Tóm tắt tình trạng người bệnh ra viện (với fallback)
            await TomTatTinhTrangRaVien(patient, tomTat, baseUri, apiKeys);

            // Tóm tắt kết quả xét nghiệm (với fallback)
            await TomTatKetQuaXetNghiem(patient, tomTat, baseUri, apiKeys);
        }

        private async Task TomTatQuaTrinhBenhLy(PatientAllData patient, DataTomTat tomTat, string baseUri, string[] apiKeys)
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

            string result = await CallGeminiApiWithFallback(baseUri, apiKeys, prompt);

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

        private async Task TomTatTinhTrangRaVien(PatientAllData patient, DataTomTat tomTat, string baseUri, string[] apiKeys)
        {
            if (patient.TinhTrangNguoiBenhRaVien == null || !patient.TinhTrangNguoiBenhRaVien.Any())
                return;

            string rawPrompt = _fileServices.GetPromt("TinhTrangNguoiBenhRaVienPromt.txt");
            string dienBien = patient.TinhTrangNguoiBenhRaVien[0].DienBien ?? "";
            string prompt = rawPrompt.Replace("@DienBien", dienBien);

            string result = await CallGeminiApiWithFallback(baseUri, apiKeys, prompt);
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
        

        private async Task TomTatKetQuaXetNghiem(PatientAllData patient, DataTomTat tomTat, string baseUri, string[] apiKeys)
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

            string result = await CallGeminiApiWithFallback(baseUri, apiKeys, prompt);
            tomTat.TomTatKetQuaXN = result;
        }

        private async Task<string> CallGeminiApiWithFallback(string baseUri, string[] apiKeys, string prompt)
        {
            if (apiKeys == null || apiKeys.Length == 0)
            {
                throw new Exception("Không có API key nào được cấu hình");
            }

            Exception lastException = null;

            // Thử từng API key cho đến khi thành công hoặc hết key
            foreach (var apiKey in apiKeys)
            {
                try
                {
                    return await CallGeminiApi(baseUri, apiKey, prompt);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    // Tiếp tục thử API key tiếp theo
                    continue;
                }
            }

            // Nếu tất cả API keys đều thất bại, throw exception cuối cùng
            throw new Exception($"Tất cả API keys đều thất bại. Lỗi cuối cùng: {lastException?.Message}", lastException);
        }

        private string TryDecryptApiKey(string encryptedKey, string decryptKey)
        {
            if (string.IsNullOrEmpty(encryptedKey))
                return "";

            // Kiểm tra xem có phải là Base64 string không (thường Base64 có độ dài > 20 và chỉ chứa A-Z, a-z, 0-9, +, /, =)
            // Nếu là Base64 và có thể decrypt thì decrypt, nếu không thì dùng trực tiếp (tương thích ngược)
            try
            {
                // Thử giải mã nếu là Base64
                if (encryptedKey.Length > 20 && IsBase64String(encryptedKey))
                {
                    return _fileServices.Decrypt(encryptedKey, decryptKey);
                }
            }
            catch
            {
                // Nếu decrypt thất bại, có thể là plain text, trả về nguyên bản
            }

            // Nếu không phải Base64 hoặc decrypt thất bại, trả về nguyên bản (tương thích ngược)
            return encryptedKey;
        }

        private bool IsBase64String(string s)
        {
            if (string.IsNullOrEmpty(s) || s.Length % 4 != 0)
                return false;

            try
            {
                Convert.FromBase64String(s);
                return true;
            }
            catch
            {
                return false;
            }
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
                    string errorContent = await response.Content.ReadAsStringAsync();
                    
                    // Kiểm tra nếu là lỗi 403 - API key bị leaked
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        try
                        {
                            var errorObj = JsonConvert.DeserializeObject<dynamic>(errorContent);
                            string errorMessage = errorObj?.error?.message?.ToString() ?? "";
                            string errorStatus = errorObj?.error?.status?.ToString() ?? "";
                            
                            if (errorMessage.Contains("leaked") || errorStatus == "PERMISSION_DENIED")
                            {
                                throw new Exception($"API key đã bị đánh dấu là rò rỉ (leaked) và không thể sử dụng. Vui lòng tạo API key mới từ Google Cloud Console. Status: {errorStatus}");
                            }
                        }
                        catch
                        {
                            // Nếu không parse được JSON, vẫn throw lỗi gốc
                        }
                    }
                    
                    throw new HttpRequestException($"API call failed with status: {response.StatusCode}. Response: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gọi Gemini API: {ex.Message}", ex);
            }
        }
    }
}
