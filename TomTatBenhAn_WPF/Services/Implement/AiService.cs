using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
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
        private readonly Dictionary<string, string> _promptCache = new Dictionary<string, string>();

        public AiService(IFileServices fileServices, IConfigServices configServices)
        {
            _fileServices = fileServices;
            // Thiết lập timeout cho HttpClient để tránh chờ quá lâu
            _httpClient.Timeout = TimeSpan.FromSeconds(60); // Giảm xuống 60s để fail nhanh hơn nếu có vấn đề
            // Preload prompts để tăng tốc độ
            PreloadPrompts();
        }

        private void PreloadPrompts()
        {
            try
            {
                var promptFiles = new[] { "QuaTrinhBenhLyPromt.txt", "TinhTrangNguoiBenhRaVienPromt.txt", "KetQuaXNPromt.txt" };
                foreach (var file in promptFiles)
                {
                    try
                    {
                        _promptCache[file] = _fileServices.GetPromt(file);
                    }
                    catch
                    {
                        // Ignore nếu không load được, sẽ load lại khi cần
                    }
                }
            }
            catch
            {
                // Ignore nếu preload thất bại
            }
        }

        private string GetCachedPrompt(string fileName)
        {
            if (_promptCache.TryGetValue(fileName, out string cachedPrompt))
            {
                return cachedPrompt;
            }
            
            // Nếu không có trong cache, load và cache lại
            string prompt = _fileServices.GetPromt(fileName);
            _promptCache[fileName] = prompt;
            return prompt;
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
            
            // Đọc và giải mã API keys từ config (3 keys chính + 3 keys backup)
            string encryptedApiKey1 = ConfigurationManager.AppSettings["API_gemini_1"] ?? "";
            string encryptedApiKey2 = ConfigurationManager.AppSettings["API_gemini_2"] ?? "";
            string encryptedApiKey3 = ConfigurationManager.AppSettings["API_gemini_3"] ?? "";
            string encryptedApiKey4 = ConfigurationManager.AppSettings["API_gemini_4"] ?? "";
            string encryptedApiKey5 = ConfigurationManager.AppSettings["API_gemini_5"] ?? "";
            string encryptedApiKey6 = ConfigurationManager.AppSettings["API_gemini_6"] ?? "";
            
            // Giải mã API keys (nếu là Base64 thì decrypt, nếu không thì dùng trực tiếp để tương thích ngược)
            string apiKey1 = TryDecryptApiKey(encryptedApiKey1, decryptKey);
            string apiKey2 = TryDecryptApiKey(encryptedApiKey2, decryptKey);
            string apiKey3 = TryDecryptApiKey(encryptedApiKey3, decryptKey);
            string apiKey4 = TryDecryptApiKey(encryptedApiKey4, decryptKey);
            string apiKey5 = TryDecryptApiKey(encryptedApiKey5, decryptKey);
            string apiKey6 = TryDecryptApiKey(encryptedApiKey6, decryptKey);

            // Tạo danh sách API keys: 3 keys chính (primary) và 3 keys backup
            var primaryKeys = new[] { apiKey1, apiKey2, apiKey3 }.Where(k => !string.IsNullOrEmpty(k)).ToArray();
            var backupKeys = new[] { apiKey4, apiKey5, apiKey6 }.Where(k => !string.IsNullOrEmpty(k)).ToArray();
            var allApiKeys = primaryKeys.Concat(backupKeys).ToArray();

            // Phân bổ keys cho từng task để tránh rate limit khi chạy song song
            // Mỗi task sẽ dùng 3 keys chính trước, sau đó fallback sang 3 keys backup nếu cần
            // Task1: key1, key2, key3 (primary) -> key4, key5, key6 (backup)
            var apiKeys1 = primaryKeys.Concat(backupKeys).Where(k => !string.IsNullOrEmpty(k)).ToArray();
            
            // Task2: key2, key3, key1 (primary) -> key5, key6, key4 (backup)
            var apiKeys2 = primaryKeys.Length >= 3
                ? new[] { primaryKeys[1], primaryKeys[2], primaryKeys[0] }
                    .Concat(backupKeys.Length >= 3 ? new[] { backupKeys[1], backupKeys[2], backupKeys[0] } : backupKeys)
                    .Where(k => !string.IsNullOrEmpty(k)).ToArray()
                : apiKeys1; // Fallback về apiKeys1 nếu không đủ primary keys
            
            // Task3: key3, key1, key2 (primary) -> key6, key4, key5 (backup)
            var apiKeys3 = primaryKeys.Length >= 3
                ? new[] { primaryKeys[2], primaryKeys[0], primaryKeys[1] }
                    .Concat(backupKeys.Length >= 3 ? new[] { backupKeys[2], backupKeys[0], backupKeys[1] } : backupKeys)
                    .Where(k => !string.IsNullOrEmpty(k)).ToArray()
                : apiKeys1; // Fallback về apiKeys1 nếu không đủ primary keys

            // Chạy song song 3 API calls để tăng tốc độ (thay vì chạy tuần tự)
            // Các method này modify các properties khác nhau của tomTat nên an toàn khi chạy parallel
            // Mỗi task dùng keys khác nhau để tránh rate limit
            // Bỏ delay giữa các tasks để tăng tốc độ - các keys khác nhau sẽ tự phân tán load
            var task1 = TomTatQuaTrinhBenhLy(patient, tomTat, baseUri, apiKeys1);
            var task2 = TomTatTinhTrangRaVien(patient, tomTat, baseUri, apiKeys2);
            var task3 = TomTatKetQuaXetNghiem(patient, tomTat, baseUri, apiKeys3);

            // Đợi tất cả các task hoàn thành
            await Task.WhenAll(task1, task2, task3);
        }

        private async Task TomTatQuaTrinhBenhLy(PatientAllData patient, DataTomTat tomTat, string baseUri, string[] apiKeys)
        {
            if (patient.ThongTinKhamBenh == null || !patient.ThongTinKhamBenh.Any())
                return;

            string rawPrompt = GetCachedPrompt("QuaTrinhBenhLyPromt.txt");
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

            string rawPrompt = GetCachedPrompt("TinhTrangNguoiBenhRaVienPromt.txt");
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

            string rawPrompt = GetCachedPrompt("KetQuaXNPromt.txt");

            // Lấy chẩn đoán chính từ danh sách chẩn đoán ICD
            string chanDoanChinh = patient.ChanDoanIcd?.FirstOrDefault()?.BenhChinhRaVien ??
                                   patient.ChanDoanIcd?.FirstOrDefault()?.BenhChinhVaoVien ?? "";

            string chanDoanKemTheo = patient.ChanDoanIcd?.FirstOrDefault()?.BenhKemTheoRaVien ?? "";

            // Tối ưu dữ liệu trước khi serialize để giảm token
            var optimizedData = OptimizeKetQuaXetNghiemData(patient.KetQuaXetNghien);
            
            // Chuyển đổi danh sách kết quả xét nghiệm thành JSON với format compact
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.None // Compact format, không xuống dòng
            };
            string danhSachKQXN = JsonConvert.SerializeObject(optimizedData, settings);

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
            const int maxRetries = 3; // Số lần retry tối đa cho mỗi key
            const int baseDelayMs = 1000; // Delay cơ bản 1 giây

            // Thử từng API key cho đến khi thành công hoặc hết key
            foreach (var apiKey in apiKeys)
            {
                // Thử với retry logic cho lỗi 429 (rate limit)
                for (int retryAttempt = 0; retryAttempt < maxRetries; retryAttempt++)
                {
                    try
                    {
                        // Thêm delay nhỏ giữa các requests để tránh rate limit (trừ lần đầu)
                        if (retryAttempt > 0)
                        {
                            int delayMs = baseDelayMs * (int)Math.Pow(2, retryAttempt - 1); // Exponential backoff: 1s, 2s, 4s
                            await Task.Delay(delayMs);
                        }

                        return await CallGeminiApi(baseUri, apiKey, prompt);
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        
                        // Kiểm tra nếu là lỗi 429 (rate limit) và có retry delay
                        if (ex.Message.Contains("429") || ex.Message.Contains("TooManyRequests") || ex.Message.Contains("RESOURCE_EXHAUSTED"))
                        {
                            // Parse retry delay từ exception Data hoặc message
                            int retryDelayMs = 0;
                            if (ex.Data.Contains("RetryDelayMs") && ex.Data["RetryDelayMs"] is int delay)
                            {
                                retryDelayMs = delay;
                            }
                            else
                            {
                                retryDelayMs = ParseRetryDelayFromException(ex);
                            }
                            
                            if (retryDelayMs > 0 && retryAttempt < maxRetries - 1)
                            {
                                // Đợi theo retry delay được chỉ định bởi API
                                await Task.Delay(retryDelayMs);
                                continue; // Retry với cùng key
                            }
                            else if (retryAttempt < maxRetries - 1)
                            {
                                // Nếu không parse được delay, dùng exponential backoff
                                int delayMs = baseDelayMs * (int)Math.Pow(2, retryAttempt);
                                await Task.Delay(delayMs);
                                continue; // Retry với cùng key
                            }
                        }
                        
                        // Nếu không phải lỗi 429 hoặc đã hết retry, thử key tiếp theo
                        break;
                    }
                }
                
                // Chỉ delay giữa các keys nếu vừa gặp lỗi rate limit (để tránh spam)
                // Nếu thành công hoặc lỗi khác, không cần delay
                if (lastException != null && 
                    (lastException.Message.Contains("429") || 
                     lastException.Message.Contains("TooManyRequests") || 
                     lastException.Message.Contains("RESOURCE_EXHAUSTED")))
                {
                    await Task.Delay(100); // Delay nhỏ chỉ khi gặp rate limit
                }
            }

            // Nếu tất cả API keys đều thất bại, throw exception cuối cùng
            throw new Exception($"Tất cả API keys đều thất bại. Lỗi cuối cùng: {lastException?.Message}", lastException);
        }

        private int ParseRetryDelayFromException(Exception ex)
        {
            try
            {
                // Tìm retry delay trong exception message (ví dụ: "Please retry in 13.806261416s")
                string message = ex.Message;
                var match = Regex.Match(message, @"retry in ([\d.]+)s", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    if (double.TryParse(match.Groups[1].Value, out double seconds))
                    {
                        // Làm tròn lên và thêm 1 giây buffer
                        return (int)Math.Ceiling(seconds * 1000) + 1000;
                    }
                }
            }
            catch
            {
                // Nếu không parse được, trả về 0
            }
            return 0;
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

                // Tối ưu JSON serialization: không format, bỏ null values
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    Formatting = Formatting.None
                };
                string jsonContent = JsonConvert.SerializeObject(requestData, settings);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Thiết lập headers theo yêu cầu của Gemini API
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-goog-api-key", apiKey);

                // Sử dụng PostAsync với cancellation token để có thể cancel nếu cần
                var response = await _httpClient.PostAsync($"{baseUri}?key={apiKey}", content);

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
                    
                    // Kiểm tra nếu là lỗi 429 - Rate limit (TooManyRequests)
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        try
                        {
                            var errorObj = JsonConvert.DeserializeObject<dynamic>(errorContent);
                            string errorMessage = errorObj?.error?.message?.ToString() ?? "";
                            string errorStatus = errorObj?.error?.status?.ToString() ?? "";
                            
                            // Parse retry delay từ RetryInfo nếu có
                            int retryDelayMs = 0;
                            try
                            {
                                var retryInfo = errorObj?.error?.details;
                                if (retryInfo != null)
                                {
                                    foreach (var detail in retryInfo)
                                    {
                                        if (detail?["@type"]?.ToString() == "type.googleapis.com/google.rpc.RetryInfo")
                                        {
                                            string retryDelay = detail?.retryDelay?.ToString() ?? "";
                                            // Parse duration (ví dụ: "13s" hoặc "13.806261416s")
                                            var delayMatch = Regex.Match(retryDelay, @"([\d.]+)s", RegexOptions.IgnoreCase);
                                            if (delayMatch.Success && double.TryParse(delayMatch.Groups[1].Value, out double seconds))
                                            {
                                                retryDelayMs = (int)Math.Ceiling(seconds * 1000) + 1000; // Thêm 1s buffer
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // Nếu không parse được RetryInfo, sẽ dùng message
                            }
                            
                            // Tạo exception với thông tin retry delay
                            string exceptionMessage = $"API call failed with status: {response.StatusCode} (TooManyRequests). {errorMessage}";
                            if (retryDelayMs > 0)
                            {
                                exceptionMessage += $" Please retry in {retryDelayMs / 1000.0:F1}s.";
                            }
                            
                            var rateLimitException = new HttpRequestException(exceptionMessage);
                            rateLimitException.Data["RetryDelayMs"] = retryDelayMs;
                            throw rateLimitException;
                        }
                        catch (HttpRequestException)
                        {
                            throw; // Re-throw nếu đã là HttpRequestException
                        }
                        catch
                        {
                            // Nếu không parse được JSON, vẫn throw lỗi gốc
                        }
                    }
                    
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

        /// <summary>
        /// Tối ưu dữ liệu xét nghiệm để giảm token: rút gọn tên field và compress text
        /// Giữ nguyên toàn bộ dữ liệu, chỉ rút gọn tên field và loại bỏ khoảng trắng thừa
        /// </summary>
        private List<object> OptimizeKetQuaXetNghiemData(List<KetQuaXetNghiemModel>? data)
        {
            if (data == null || !data.Any())
                return new List<object>();

            return data.Select(x =>
            {
                var obj = new Dictionary<string, object>();
                
                // Rút gọn tên field để tiết kiệm token, nhưng giữ nguyên giá trị
                if (!string.IsNullOrEmpty(x.TenNhomDichVu))
                    obj["g"] = CompressText(x.TenNhomDichVu); // group (nhóm dịch vụ)
                    
                if (!string.IsNullOrEmpty(x.TenDichVu))
                    obj["dv"] = CompressText(x.TenDichVu); // dich vu (tên dịch vụ)
                    
                if (!string.IsNullOrEmpty(x.KetQua))
                    obj["kq"] = CompressText(x.KetQua); // ket qua (kết quả)
                    
                if (!string.IsNullOrEmpty(x.BatThuong))
                    obj["bt"] = x.BatThuong; // bat thuong (bất thường)
                    
                if (!string.IsNullOrEmpty(x.MucBinhThuongMin))
                    obj["min"] = x.MucBinhThuongMin; // min
                    
                if (!string.IsNullOrEmpty(x.MucBinhThuongMax))
                    obj["max"] = x.MucBinhThuongMax; // max
                    
                if (!string.IsNullOrEmpty(x.MucBinhThuong))
                    obj["mb"] = CompressText(x.MucBinhThuong); // muc binh thuong (mức bình thường)
                    
                if (x.ThoiGianThucHien.HasValue)
                    obj["tg"] = x.ThoiGianThucHien.Value.ToString("dd/MM/yyyy HH:mm:ss"); // thoi gian
                    
                if (!string.IsNullOrEmpty(x.KetLuan))
                    obj["kl"] = CompressText(x.KetLuan); // ket luan (kết luận)
                    
                if (!string.IsNullOrEmpty(x.MoTa_Text))
                    obj["mt"] = CompressText(x.MoTa_Text); // mo ta (mô tả)
                
                // Giữ nguyên các field quan trọng khác nếu có
                if (!string.IsNullOrEmpty(x.TenPhongBan))
                    obj["pb"] = CompressText(x.TenPhongBan); // phong ban
                    
                if (!string.IsNullOrEmpty(x.NoiDungChiTiet))
                    obj["nd"] = CompressText(x.NoiDungChiTiet); // noi dung
                
                return obj;
            })
            .Where(x => x.Count > 0) // Chỉ lấy các record có ít nhất 1 field có giá trị
            .Cast<object>()
            .ToList();
        }

        /// <summary>
        /// Compress text: loại bỏ khoảng trắng thừa, nhưng giữ nguyên nội dung
        /// </summary>
        private string CompressText(string? text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Loại bỏ khoảng trắng thừa (nhiều space liên tiếp thành 1 space)
            // Loại bỏ newline/carriage return thừa
            // Nhưng giữ nguyên nội dung chính xác
            return Regex.Replace(
                text.Trim(),
                @"\s+", // Nhiều khoảng trắng (space, tab, newline)
                " " // Thay bằng 1 space
            );
        }
    }
}
