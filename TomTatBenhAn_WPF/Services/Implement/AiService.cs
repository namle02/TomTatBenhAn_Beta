using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TomTatBenhAn_WPF.Repos.Model;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class AiService : IAiService

    {
        private readonly HttpClient _httpClient;
        private readonly IConfigServices _configServices;

        public AiService(IConfigServices configServices)
        {
            _httpClient = new HttpClient();
            _configServices = configServices;
        }

        public async Task<Dictionary<string, string>> TomTatBenhLyAsync(string quaTrinhBenhLy)
        {
            var resultData = new Dictionary<string, string>();

            try
            {
                // 🔹 Get prompt from configDict
                string? promptTemplate = _configServices.Get("PROMT_BENHAN");
                if (string.IsNullOrWhiteSpace(promptTemplate))
                    throw new Exception("Không tìm thấy PROMT_BENHAN trong configDict");

                // 🔹 Replace variable in the prompt
                string finalPrompt = promptTemplate.Replace("@QuaTrinhBenhLy", quaTrinhBenhLy);

                // 🔹 Build the request body
                var requestBody = new
                {
                    contents = new[] {
                new {
                    parts = new[] {
                        new {
                            text = finalPrompt
                        }
                    }
                }
            }
                };

                string jsonContent = JsonSerializer.Serialize(requestBody);

                // 🔹 Get URL and API key from config
                string urlBase = ConfigurationManager.AppSettings["URL_gemini"]!;
                string apiKey = ConfigurationManager.AppSettings["API_gemini_1"]!;

                if (string.IsNullOrWhiteSpace(urlBase) || string.IsNullOrWhiteSpace(apiKey))
                    throw new Exception("URL_gemini hoặc API_gemini_1 bị thiếu trong cấu hình");

                string fullUrl = urlBase + apiKey;

                // 🔹 Prepare the request
                using var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // 🔹 Send request
                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // 🔹 Parse response
                string responseJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseJson);
                string aiText = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString()
                    ?? throw new Exception("Không tìm thấy nội dung trả về từ AI");

                // 🔹 Now split the result into two parts
                string start = "Quá trình bệnh lý và diễn biến lâm sàng:";
                string end = "Những dấu hiệu lâm sàng chính:";

                int startIndex = aiText.IndexOf(start) + start.Length;
                int endIndex = aiText.IndexOf(end);

                if (startIndex >= start.Length && endIndex > startIndex)
                {
                    string qtbl = aiText.Substring(startIndex, endIndex - startIndex).Trim();
                    resultData["BN_TomTatQuaTrinhBenhLy"] = qtbl;
                }

                if (endIndex >= 0)
                {
                    int contentStartIndex = endIndex + end.Length;
                    if (contentStartIndex < aiText.Length)
                    {
                        string dhls = aiText.Substring(contentStartIndex).Trim();
                        resultData["BN_DauHieuLamSang"] = dhls;
                    }
                }

                return resultData;
            }
            catch (Exception ex)
            {
                resultData["error"] = $"🛑 Lỗi khi tóm tắt bệnh lý: {ex.Message}";
                return resultData;
            }
        }

        public string resultTomTatKQXN { get; private set; } = string.Empty;
        public async Task<string> TomTatKetQuaXetNghiemCSLAsync(string chuanDoanChinh, List<KetQuaXetNghiemCLSModel> danhSachKQXN)
        {
            try
            {
                // 🔹 Lấy prompt từ config
                string? promptTemplate = _configServices.Get("PROMT_KQXN");
                if (string.IsNullOrWhiteSpace(promptTemplate))
                    throw new Exception("Không tìm thấy PROMT_KQXN trong configDict");
                string ketQuaText = string.Join("\n", danhSachKQXN.Select(x =>
                        $"- Tên nhóm DV   : {x.TenNhomDichVu}\n" +
                        $"- Phòng ban     : {x.TenPhongBan}\n" +
                        $"- Tên dịch vụ   : {x.TenDichvu}\n" +
                        $"- Nội dung chi tiết: {x.NoiDungChiTiet}\n" +
                        $"- Kết quả       : {x.KetQua}\n" +
                        $"- Bình thường   : {x.MucBinhThuong}\n" +
                        $"- BT Min        : {x.MucBinhThuongMin}\n" +
                        $"- BT Max        : {x.MucBinhThuongMax}\n" +
                        $"- Bất thường    : {x.BatThuong}\n" +
                        $"- Thời gian thực hiện: {x.ThoiGianThucHIen}\n" +
                        $"- Mô tả         : {x.MoTa}\n" +
                        $"- Kết luận      : {x.KetLuan}\n" +
                        $"--------------------------------------------------"
                    ));

                // 🔹 Thay thế vào prompt
                string finalPrompt = promptTemplate
                    .Replace("@ChanDoanChinh", chuanDoanChinh)
                    .Replace("@DanhSachKQXN", ketQuaText);

                var requestBody = new
                {
                    contents = new[] {
                        new {
                            parts = new[] {
                                new { text = finalPrompt }
                            }
                        }
                    }
                };

                string jsonContent = JsonSerializer.Serialize(requestBody);
                string urlBase = ConfigurationManager.AppSettings["URL_gemini"]!;
                string apiKey = ConfigurationManager.AppSettings["API_gemini_1"]!;
                string fullUrl = urlBase + apiKey;

                using var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseJson);

                // 🔸 Gán kết quả vào biến resultTomTatKQXN
                resultTomTatKQXN = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString()
                    ?? throw new Exception("Không tìm thấy nội dung trả về từ AI");

                return resultTomTatKQXN;
            }
            catch (Exception ex)
            {
                resultTomTatKQXN = $"🛑 Lỗi khi tóm tắt KQXN: {ex.Message}";
                return resultTomTatKQXN;
            }
        }
        public async Task<Dictionary<string, string>> HuongDieuTriAsync(string DienBien,string LoiDanThayThuoc)
        {
            var resultData = new Dictionary<string, string>();

            try
            {
                string? promptTemplate = _configServices.Get("PROMT_TTNB");
                if (string.IsNullOrWhiteSpace(promptTemplate))
                    throw new Exception("Không tìm thấy PROMT_TTNB trong configDict");

                string finalPrompt = promptTemplate.Replace("@DienBien", DienBien).Replace("@LoiDanThayThuoc",LoiDanThayThuoc);
                

                var requestBody = new
                {
                    contents = new[]
                    {
                new
                {
                    role = "user", // ✅ Rất quan trọng
                    parts = new[]
                    {
                        new { text = finalPrompt }
                    }
                }
            }
                };

                string jsonContent = JsonSerializer.Serialize(requestBody);

                string urlBase = ConfigurationManager.AppSettings["URL_gemini"]!;
                string apiKey = ConfigurationManager.AppSettings["API_gemini_1"]!;
                if (string.IsNullOrWhiteSpace(urlBase) || string.IsNullOrWhiteSpace(apiKey))
                    throw new Exception("Thiếu URL_gemini hoặc API_gemini_1 trong cấu hình");

                string fullUrl = urlBase.Contains("?key=")
                    ? urlBase + apiKey
                    : $"{urlBase}?key={apiKey}";

                using var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();

                //Console.WriteLine("Final Prompt:\n" + finalPrompt);
                //Console.WriteLine("Response:\n" + responseJson);

                using var doc = JsonDocument.Parse(responseJson);
                string aiText = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString()
                    ?? throw new Exception("Không tìm thấy nội dung phản hồi từ AI");

                resultData["HuongDieuTri"] = aiText;
            }
            catch (Exception ex)
            {
                resultData["Error"] = $"Lỗi: {ex.Message}";
            }

            return resultData;
        }


    }
}
