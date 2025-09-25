
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TomTatBenhAn_WPF.Services.Interface;
using TomTatBenhAn_WPF.Repos.Dto;
using TomTatBenhAn_WPF.Core;
using System.Net;

namespace TomTatBenhAn_WPF.Services.Implement
{
    public class PhacDoServices : IPhacDoServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigServices _configServices;
        private readonly JsonSerializerOptions _jsonOptions;

        public PhacDoServices(HttpClient httpClient, IConfigServices configServices)
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

        /// <summary>
        /// Lấy tất cả phác đồ từ server
        /// </summary>
        public async Task<ApiResponse<List<PhacDoItemDTO>>> GetAllPhacDoAsync()
        {
            try
            {
                var url = $"{GetBaseUrl()}/phacdo";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<PhacDoItemDTO>>>(content, _jsonOptions);
                    return apiResponse ?? ApiResponse<List<PhacDoItemDTO>>.ErrorResult("Không thể parse response từ server");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<List<PhacDoItemDTO>>>(content, _jsonOptions);
                    return errorResponse ?? ApiResponse<List<PhacDoItemDTO>>.ErrorResult($"API trả về lỗi: {response.StatusCode}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PhacDoItemDTO>>.ErrorResult($"Lỗi khi gọi API: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy phác đồ theo ID
        /// </summary>
        public async Task<ApiResponse<PhacDoItemDTO>> GetPhacDoByIdAsync(string id)
        {
            try
            {
                var url = $"{GetBaseUrl()}/phacdo/{id}";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<PhacDoItemDTO>>(content, _jsonOptions);
                    return apiResponse ?? ApiResponse<PhacDoItemDTO>.ErrorResult("Không thể parse response từ server");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<PhacDoItemDTO>>(content, _jsonOptions);
                    return errorResponse ?? ApiResponse<PhacDoItemDTO>.ErrorResult($"API trả về lỗi: {response.StatusCode}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<PhacDoItemDTO>.ErrorResult($"Lỗi khi gọi API: {ex.Message}");
            }
        }

        /// <summary>
        /// Tìm kiếm phác đồ theo tên
        /// </summary>
        public async Task<ApiResponse<List<PhacDoItemDTO>>> SearchPhacDoAsync(string searchTerm)
        {
            try
            {
                var url = $"{GetBaseUrl()}/phacdo/search?search={Uri.EscapeDataString(searchTerm)}";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<PhacDoItemDTO>>>(content, _jsonOptions);
                    return apiResponse ?? ApiResponse<List<PhacDoItemDTO>>.ErrorResult("Không thể parse response từ server");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<List<PhacDoItemDTO>>>(content, _jsonOptions);
                    return errorResponse ?? ApiResponse<List<PhacDoItemDTO>>.ErrorResult($"API trả về lỗi: {response.StatusCode}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PhacDoItemDTO>>.ErrorResult($"Lỗi khi gọi API: {ex.Message}");
            }
        }

        /// <summary>
        /// Kiểm tra phác đồ có tồn tại hay không
        /// </summary>
        public async Task<ApiResponse<PhacDoItemDTO>> CheckProtocolExistsAsync(string name, string? code = null)
        {
            try
            {
                var url = $"{GetBaseUrl()}/phacdo/check?name={Uri.EscapeDataString(name)}";
                if (!string.IsNullOrEmpty(code))
                {
                    url += $"&code={Uri.EscapeDataString(code)}";
                }

                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<PhacDoItemDTO>>(content, _jsonOptions);
                    return apiResponse ?? ApiResponse<PhacDoItemDTO>.ErrorResult("Không thể parse response từ server");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<PhacDoItemDTO>>(content, _jsonOptions);
                    return errorResponse ?? ApiResponse<PhacDoItemDTO>.ErrorResult($"API trả về lỗi: {response.StatusCode}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<PhacDoItemDTO>.ErrorResult($"Lỗi khi gọi API: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm phác đồ mới (phân tích từ text)
        /// </summary>
        public async Task<ApiResponse<ProtocolDTO>> AddPhacDoAsync(AddPhacDoRequestDTO request)
        {
            try
            {
                var url = $"{GetBaseUrl()}/phacdo/add";
                
                // Xử lý force parameter thông qua query string
                if (request.Force)
                {
                    url += "?force=true";
                }
                
                // Chỉ gửi raw text, không gửi JSON
                var content = new StringContent(request.Rawtext, Encoding.UTF8, "text/plain");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // API có thể trả về 409 (Conflict) nếu phác đồ đã tồn tại
                if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Conflict)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProtocolDTO>>(responseContent, _jsonOptions);
                    return apiResponse ?? ApiResponse<ProtocolDTO>.ErrorResult("Không thể parse response từ server");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<ProtocolDTO>>(responseContent, _jsonOptions);
                    return errorResponse ?? ApiResponse<ProtocolDTO>.ErrorResult($"API trả về lỗi: {response.StatusCode}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<ProtocolDTO>.ErrorResult($"Lỗi khi gọi API: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm phác đồ mới với tùy chọn ghi đè (gửi trực tiếp text/plain)
        /// </summary>
        public async Task<ApiResponse<ProtocolDTO>> AddPhacDoWithForceAsync(string rawText, bool force = false)
        {
            try
            {
                var url = $"{GetBaseUrl()}/phacdo/add";
                
                // Xử lý force parameter thông qua query string
                if (force)
                {
                    url += "?force=true";
                }
                
                // Chỉ gửi raw text, không gửi JSON
                var content = new StringContent(rawText, Encoding.UTF8, "text/plain");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // API có thể trả về 409 (Conflict) nếu phác đồ đã tồn tại
                if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Conflict)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProtocolDTO>>(responseContent, _jsonOptions);
                    return apiResponse ?? ApiResponse<ProtocolDTO>.ErrorResult("Không thể parse response từ server");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<ProtocolDTO>>(responseContent, _jsonOptions);
                    return errorResponse ?? ApiResponse<ProtocolDTO>.ErrorResult($"API trả về lỗi: {response.StatusCode}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<ProtocolDTO>.ErrorResult($"Lỗi khi gọi API: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật phác đồ hiện có
        /// </summary>
        public async Task<ApiResponse<PhacDoItemDTO>> UpdatePhacDoAsync(string id, PhacDoItemDTO updateData)
        {
            try
            {
                var url = $"{GetBaseUrl()}/phacdo/{id}";
                var jsonContent = JsonSerializer.Serialize(updateData, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<PhacDoItemDTO>>(responseContent, _jsonOptions);
                    return apiResponse ?? ApiResponse<PhacDoItemDTO>.ErrorResult("Không thể parse response từ server");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<PhacDoItemDTO>>(responseContent, _jsonOptions);
                    return errorResponse ?? ApiResponse<PhacDoItemDTO>.ErrorResult($"API trả về lỗi: {response.StatusCode}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<PhacDoItemDTO>.ErrorResult($"Lỗi khi gọi API: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa phác đồ theo ID
        /// </summary>
        public async Task<ApiResponse<object>> DeletePhacDoAsync(string id)
        {
            try
            {
                var url = $"{GetBaseUrl()}/phacdo/{id}";
                var response = await _httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(content, _jsonOptions);
                    return apiResponse ?? ApiResponse<object>.ErrorResult("Không thể parse response từ server");
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(content, _jsonOptions);
                    return errorResponse ?? ApiResponse<object>.ErrorResult($"API trả về lỗi: {response.StatusCode}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult($"Lỗi khi gọi API: {ex.Message}");
            }
        }
    }
}
