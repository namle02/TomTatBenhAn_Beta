using System.Net.Http;
using System.Text;
using System.Text.Json;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Services.Interface;
using TomTatBenhAn_WPF.Core;
using System.IO;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public partial class BangKiemServices : IBangKiemServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigServices _configServices;
        private readonly JsonSerializerOptions _jsonOptions;

        public BangKiemServices(HttpClient httpClient, IConfigServices configServices)
        {
            _httpClient = httpClient;
            _configServices = configServices;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        private string GetBaseUrl() => _configServices.GetApiBaseUrl();

        public async Task<ApiResponse<List<BangKiemResponseDTO>>> GetAllAsync(string? phacDoId = null, string? search = null)
        {
            try
            {
                var url = new StringBuilder($"{GetBaseUrl()}/bangdanhgia");
                var qs = new List<string>();
                if (!string.IsNullOrWhiteSpace(phacDoId)) qs.Add($"phacDoId={Uri.EscapeDataString(phacDoId)}");
                if (!string.IsNullOrWhiteSpace(search)) qs.Add($"search={Uri.EscapeDataString(search)}");
                if (qs.Count > 0) url.Append($"?{string.Join("&", qs)}");

                var response = await _httpClient.GetAsync(url.ToString());
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<List<BangKiemResponseDTO>>>(content, _jsonOptions)!
                       ?? ApiResponse<List<BangKiemResponseDTO>>.ErrorResult("Không thể parse response");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BangKiemResponseDTO>>.ErrorResult(ex.Message);
            }
        }

        public async Task<ApiResponse<BangKiemResponseDTO>> GetByIdAsync(string id)
        {
            try
            {
                var url = $"{GetBaseUrl()}/bangdanhgia/{id}";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<BangKiemResponseDTO>>(content, _jsonOptions)!
                       ?? ApiResponse<BangKiemResponseDTO>.ErrorResult("Không thể parse response");
            }
            catch (Exception ex)
            {
                return ApiResponse<BangKiemResponseDTO>.ErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// Tạo mới bảng kiểm với file Word gốc
        /// </summary>
        public async Task<ApiResponse<BangKiemResponseDTO>> CreateWithFileAsync(BangKiemRequestDTO payload, string wordFilePath)
        {
            try
            {
                var url = $"{GetBaseUrl()}/bangdanhgia";
                
                using var formData = new MultipartFormDataContent();
                
                // Thêm file Word
                if (!File.Exists(wordFilePath))
                {
                    return ApiResponse<BangKiemResponseDTO>.ErrorResult("File Word không tồn tại");
                }
                
                var fileBytes = await File.ReadAllBytesAsync(wordFilePath);
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                formData.Add(fileContent, "wordFile", Path.GetFileName(wordFilePath));
                
                // Thêm các trường dữ liệu JSON
                var json = JsonSerializer.Serialize(payload, _jsonOptions);
                formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "data");
                
                var response = await _httpClient.PostAsync(url, formData);
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<BangKiemResponseDTO>>(content, _jsonOptions)!
                       ?? ApiResponse<BangKiemResponseDTO>.ErrorResult("Không thể parse response");
            }
            catch (Exception ex)
            {
                return ApiResponse<BangKiemResponseDTO>.ErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// Tạo mới bảng kiểm (method cũ - giữ lại để tương thích)
        /// </summary>
        public async Task<ApiResponse<BangKiemResponseDTO>> CreateAsync(BangKiemRequestDTO payload)
        {
            try
            {
                var url = $"{GetBaseUrl()}/bangdanhgia";
                var json = JsonSerializer.Serialize(payload, _jsonOptions);
                var req = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, req);
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<BangKiemResponseDTO>>(content, _jsonOptions)!
                       ?? ApiResponse<BangKiemResponseDTO>.ErrorResult("Không thể parse response");
            }
            catch (Exception ex)
            {
                return ApiResponse<BangKiemResponseDTO>.ErrorResult(ex.Message);
            }
        }

        public async Task<ApiResponse<BangKiemResponseDTO>> UpdateAsync(string id, BangKiemRequestDTO payload)
        {
            try
            {
                var url = $"{GetBaseUrl()}/bangdanhgia/{id}";
                var json = JsonSerializer.Serialize(payload, _jsonOptions);
                var req = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, req);
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<BangKiemResponseDTO>>(content, _jsonOptions)!
                       ?? ApiResponse<BangKiemResponseDTO>.ErrorResult("Không thể parse response");
            }
            catch (Exception ex)
            {
                return ApiResponse<BangKiemResponseDTO>.ErrorResult(ex.Message);
            }
        }

        public async Task<ApiResponse<object>> DeleteAsync(string id)
        {
            try
            {
                var url = $"{GetBaseUrl()}/bangdanhgia/{id}";
                var response = await _httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<object>>(content, _jsonOptions)!
                       ?? ApiResponse<object>.ErrorResult("Không thể parse response");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// Download file Word gốc của bảng kiểm
        /// </summary>
        public async Task<ApiResponse<byte[]>> DownloadOriginalFileAsync(string bangKiemId, string outputPath)
        {
            try
            {
                var url = $"{GetBaseUrl()}/bangdanhgia/{bangKiemId}/download";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<byte[]>.ErrorResult($"Lỗi download file: {response.StatusCode}");
                }
                
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                
                // Lưu file vào đường dẫn chỉ định
                if (!string.IsNullOrWhiteSpace(outputPath))
                {
                    var directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    await File.WriteAllBytesAsync(outputPath, fileBytes);
                }
                
                return ApiResponse<byte[]>.SuccessResult(fileBytes, "Download file thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<byte[]>.ErrorResult(ex.Message);
            }
        }

        public async Task<ApiResponse<CheckBangKiemExistsResponseDTO>> CheckExistsAsync(string tenBangKiem, string phacDoId)
        {
            try
            {
                var url = $"{GetBaseUrl()}/bangdanhgia/check?tenBangKiem={Uri.EscapeDataString(tenBangKiem)}&phacDoId={Uri.EscapeDataString(phacDoId)}";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<CheckBangKiemExistsResponseDTO>>(content, _jsonOptions)!
                       ?? ApiResponse<CheckBangKiemExistsResponseDTO>.ErrorResult("Không thể parse response");
            }
            catch (Exception ex)
            {
                return ApiResponse<CheckBangKiemExistsResponseDTO>.ErrorResult(ex.Message);
            }
        }
    }
}
